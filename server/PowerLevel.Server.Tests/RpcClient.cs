namespace PowerLevel.Server.Tests;

using System.Threading.Tasks;
using Auth;
using Infrastructure;

public abstract class RpcClient
{
    protected abstract Task<ApiResult<TResponse>> RpcExecute<TRequest, TResponse>(TRequest request)
        where TRequest : class where TResponse : class;

    public virtual Task<ApiResult<LoginResponse>> Login(LoginRequest request)
    {
        return this.RpcExecute<LoginRequest, LoginResponse>(request);
    }

    public virtual Task<ApiResult<LogoutResponse>> Logout(LogoutRequest request)
    {
        return this.RpcExecute<LogoutRequest, LogoutResponse>(request);
    }

    public virtual Task<ApiResult<PingResponse>> Ping(PingRequest request)
    {
        return this.RpcExecute<PingRequest, PingResponse>(request);
    }

    public virtual Task<ApiResult<ProfileInfoResponse>> ProfileInfo(ProfileInfoRequest request)
    {
        return this.RpcExecute<ProfileInfoRequest, ProfileInfoResponse>(request);
    }

    public virtual Task<ApiResult<RegisterResponse>> Register(RegisterRequest request)
    {
        return this.RpcExecute<RegisterRequest, RegisterResponse>(request);
    }
}
