namespace PowerLevel.Server.Auth;

using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure;
using LinqToDB;
using Xdxd.DotNet.Rpc;
using Xdxd.DotNet.Shared;

public class AuthHandler
{
    private readonly IDbService db;
    private readonly PasswordService passwordService;
    private readonly AuthService authService;
    private readonly JwtService jwtService;
    private readonly DateTimeService dateTimeService;
    private readonly RecaptchaService recaptchaService;
    private readonly HttpServerAppConfig appConfig;
    private readonly HttpRequestState httpRequestState;

    public AuthHandler(
        IDbService db,
        PasswordService passwordService,
        AuthService authService,
        JwtService jwtService,
        DateTimeService dateTimeService,
        RecaptchaService recaptchaService,
        HttpServerAppConfig appConfig,
        HttpRequestState httpRequestState)
    {
        this.db = db;
        this.passwordService = passwordService;
        this.authService = authService;
        this.jwtService = jwtService;
        this.dateTimeService = dateTimeService;
        this.recaptchaService = recaptchaService;
        this.appConfig = appConfig;
        this.httpRequestState = httpRequestState;
    }

    public class RegisterRequest
    {
        [Email]
        [Required(ErrorMessage = "The email address field is required.")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "The password field is required.")]
        [StringLength(40, MinimumLength = 10, ErrorMessage = "The password must be between 10 and 40 characters long.")]
        public string Password { get; set; }

        public string RecaptchaToken { get; set; }
    }

    public class RegisterResponse { }

    [RpcAuth(RequiresAuthentication = false)]
    [RpcBind(typeof(RegisterRequest), typeof(RegisterResponse))]
    [RpcConstantTime(3000)]
    public async Task<ApiResult<RegisterResponse>> Register(RegisterRequest req)
    {
        if (this.appConfig.EnableRecaptchaValidation && !await this.recaptchaService.Verify(req.RecaptchaToken))
        {
            return "Invalid Recaptcha token.";
        }

        req.EmailAddress = req.EmailAddress.Trim().ToLower();
        req.Password = req.Password.Trim();

        var existingLogin = await this.authService.FindLogin(req.EmailAddress);

        if (existingLogin != null)
        {
            return "There already is an account with that email address.";
        }

        await using (var tx = await this.db.BeginTransaction())
        {
            await this.authService.CreateProfile(req.EmailAddress, req.Password);

            await tx.CommitAsync();

            return new RegisterResponse();
        }
    }

    public class LoginRequest
    {
        [Required(ErrorMessage = "The email address field is required.")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "The password field is required.")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public string RecaptchaToken { get; set; }
    }

    public class LoginResponse
    {
        public string CsrfToken { get; set; }

        public string EmailAddress { get; set; }

        public int UserProfileID { get; set; }
    }

    public class LoginError : DefaultApiError
    {
        [RpcNullable]
        public bool UserNotVerified { get; set; }
    }

    public class LoginApiResult : ApiResult<LoginResponse, LoginError>
    {
        public LoginApiResult(bool isOk, LoginResponse payload, LoginError error) : base(isOk, payload, error) { }

        public static LoginApiResult Fail(string errorMessage, bool userNotVerified = false)
        {
            return new LoginApiResult(false, default, new LoginError
            {
                ErrorMessages = new[] { errorMessage },
                UserNotVerified = userNotVerified,
            });
        }

        public static LoginApiResult Ok(LoginResponse payload)
        {
            return new LoginApiResult(true, payload, default);
        }
    }

    [RpcAuth(RequiresAuthentication = false)]
    [RpcBind(typeof(LoginRequest), typeof(LoginResponse))]
    [RpcConstantTime(3000)]
    public async Task<LoginApiResult> Login(LoginRequest req)
    {
        if (this.appConfig.EnableRecaptchaValidation && !await this.recaptchaService.Verify(req.RecaptchaToken))
        {
            return LoginApiResult.Fail("Invalid Recaptcha token.");
        }

        req.EmailAddress = req.EmailAddress.Trim().ToLower();
        req.Password = req.Password.Trim();

        var login = await this.authService.FindLogin(req.EmailAddress);

        if (login == null)
        {
            return LoginApiResult.Fail("Wrong email or password.");
        }

        if (!login.Enabled)
        {
            return LoginApiResult.Fail("Account disabled.");
        }

        if (!this.passwordService.VerifyPassword(req.Password, login.PasswordHash))
        {
            return LoginApiResult.Fail("Wrong email or password.");
        }

        var profile = await this.db.Poco.UserProfiles.FirstOrDefaultAsync(x => x.UserProfileID == login.UserProfileID);

        await using (var tx = await this.db.BeginTransaction())
        {
            var (csrfToken, session) = await this.authService.CreateSession(login.LoginID, profile!.UserProfileID, req.RememberMe);

            var response = new LoginResponse
            {
                EmailAddress = profile.EmailAddress,
                UserProfileID = profile.UserProfileID,
                CsrfToken = csrfToken,
            };

            string cookieHeader = this.FormatCookie(session);

            await tx.CommitAsync();

            this.httpRequestState.HttpContext.Response.Headers["Set-Cookie"] = cookieHeader;

            return LoginApiResult.Ok(response);
        }
    }

    public class LogoutRequest { }

    public class LogoutResponse { }

    [RpcBind(typeof(LogoutRequest), typeof(LogoutResponse))]
    public async Task<LogoutResponse> Logout(AuthResult authResult)
    {
        await using (var tx = await this.db.BeginTransaction())
        {
            await this.authService.Logout(authResult.SessionID);

            await tx.CommitAsync();
        }

        return new LogoutResponse();
    }

    private string FormatCookie(UserSessionPoco session)
    {
        var now = this.dateTimeService.EventTime();

        var jwtPayload = new JwtPayload
        {
            SessionID = session.SessionID,
            JwtID = session.SessionID,
            ExpirationTime = session.ExpirationDate.ToUnixEpochTime(),
            IssuedAt = now.ToUnixEpochTime(),
            NotBefore = now.ToUnixEpochTime(),
        };

        string jwt = this.jwtService.EncodeSession(jwtPayload);

        string secure = this.appConfig.Environment is "development" or "testing" ? "" : "Secure; ";

        string cookie = $"jwt={jwt}; Expires={session.ExpirationDate:R}; Path=/; SameSite=Strict; {secure}HttpOnly;";

        return cookie;
    }
}

public interface AuthService
{
    Task<UserLoginPoco> FindLogin(string emailAddress);

    Task<(string, UserSessionPoco)> CreateSession(int loginID, int userProfileID, bool rememberMe);

    Task<(UserProfilePoco, UserLoginPoco)> CreateProfile(string emailAddress, string password);

    Task<UserSessionPoco> GetSession(int sessionID);

    Task Logout(int sessionID);
}

public class AuthServiceImpl : AuthService
{
    private readonly IDbService db;
    private readonly PasswordService passwordService;
    private readonly RngService rngService;
    private readonly DateTimeService dateTimeService;

    public AuthServiceImpl(
        IDbService db,
        PasswordService passwordService,
        RngService rngService,
        DateTimeService dateTimeService)
    {
        this.db = db;
        this.passwordService = passwordService;
        this.rngService = rngService;
        this.dateTimeService = dateTimeService;
    }

    public Task<UserLoginPoco> FindLogin(string emailAddress)
    {
        return this.db.Poco.UserLogins.FirstOrDefaultAsync(x => x.EmailAddress == emailAddress);
    }

    public async Task<(string, UserSessionPoco)> CreateSession(int loginID, int userProfileID, bool rememberMe)
    {
        string csrfToken = this.rngService.GenerateSecureString(40);

        var session = new UserSessionPoco
        {
            LoginDate = this.dateTimeService.EventTime(),
            LoginID = loginID,
            ProfileID = userProfileID,
            ExpirationDate = rememberMe ? this.dateTimeService.EventTime().AddDays(30) : this.dateTimeService.EventTime().AddHours(3),
            CsrfTokenHash = this.passwordService.HashPassword(csrfToken),
        };

        await this.db.Save(session);

        return (csrfToken, session);
    }

    public async Task<(UserProfilePoco, UserLoginPoco)> CreateProfile(string emailAddress, string password)
    {
        var profile = new UserProfilePoco
        {
            EmailAddress = emailAddress,
            RegistrationDate = this.dateTimeService.EventTime(),
        };

        await this.db.Save(profile);

        var login = new UserLoginPoco
        {
            UserProfileID = profile.UserProfileID,
            EmailAddress = emailAddress,
            Enabled = true,
            PasswordHash = this.passwordService.HashPassword(password),
            VerificationCode = this.rngService.GenerateSecureString(100),
            Verified = false,
        };

        await this.db.Save(login);

        return (profile, login);
    }

    public Task<UserSessionPoco> GetSession(int sessionID)
    {
        return this.db.Poco.UserSessions.FirstOrDefaultAsync(x => x.SessionID == sessionID);
    }

    public async Task Logout(int sessionID)
    {
        var session = await this.db.Poco.UserSessions.Where(x => x.SessionID == sessionID).FirstOrDefaultAsync();

        if (session!.LoggedOut)
        {
            throw new DetailedException("Can't log out a session that is already logged out.")
            {
                Details =
                {
                    { "sessionID", sessionID },
                },
            };
        }

        session.LoggedOut = true;
        session.LogoutDate = this.dateTimeService.EventTime();

        await this.db.Save(session);
    }
}
