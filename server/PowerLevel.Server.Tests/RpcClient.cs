namespace PowerLevel.Server.Tests;

using System.Threading.Tasks;
using Auth;
using Infrastructure;

public abstract class RpcClient
{
    protected abstract Task<ApiResult<TResponse>> RpcExecute<TRequest, TResponse>(TRequest request)
        where TRequest : class where TResponse : class;

    public virtual Task<ApiResult<QuizHandler.DeleteQuizResponse>> DeleteQuiz(QuizHandler.DeleteQuizRequest request)
    {
        return this.RpcExecute<QuizHandler.DeleteQuizRequest, QuizHandler.DeleteQuizResponse>(request);
    }

    public virtual Task<ApiResult<QuizHandler.GetQuizResponse>> GetQuiz(QuizHandler.GetQuizRequest request)
    {
        return this.RpcExecute<QuizHandler.GetQuizRequest, QuizHandler.GetQuizResponse>(request);
    }

    public virtual Task<ApiResult<AuthHandler.LoginResponse>> Login(AuthHandler.LoginRequest request)
    {
        return this.RpcExecute<AuthHandler.LoginRequest, AuthHandler.LoginResponse>(request);
    }

    public virtual Task<ApiResult<AuthHandler.LogoutResponse>> Logout(AuthHandler.LogoutRequest request)
    {
        return this.RpcExecute<AuthHandler.LogoutRequest, AuthHandler.LogoutResponse>(request);
    }

    public virtual Task<ApiResult<PingHandler.PingResponse>> Ping(PingHandler.PingRequest request)
    {
        return this.RpcExecute<PingHandler.PingRequest, PingHandler.PingResponse>(request);
    }

    public virtual Task<ApiResult<ProfileHandler.ProfileInfoResponse>> ProfileInfo(ProfileHandler.ProfileInfoRequest request)
    {
        return this.RpcExecute<ProfileHandler.ProfileInfoRequest, ProfileHandler.ProfileInfoResponse>(request);
    }

    public virtual Task<ApiResult<AuthHandler.RegisterResponse>> Register(AuthHandler.RegisterRequest request)
    {
        return this.RpcExecute<AuthHandler.RegisterRequest, AuthHandler.RegisterResponse>(request);
    }

    public virtual Task<ApiResult<QuizHandler.SaveQuizResponse>> SaveQuiz(QuizHandler.SaveQuizRequest request)
    {
        return this.RpcExecute<QuizHandler.SaveQuizRequest, QuizHandler.SaveQuizResponse>(request);
    }

    public virtual Task<ApiResult<QuizHandler.SearchQuizzesResponse>> SearchQuizzes(QuizHandler.SearchQuizzesRequest request)
    {
        return this.RpcExecute<QuizHandler.SearchQuizzesRequest, QuizHandler.SearchQuizzesResponse>(request);
    }
}
