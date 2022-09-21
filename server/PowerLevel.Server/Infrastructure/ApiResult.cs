namespace PowerLevel.Server.Infrastructure;

using System;
using Xdxd.DotNet.Shared;

public interface ApiError
{
    public string[] ErrorMessages { get; set; }
}

public class DefaultApiError : ApiError
{
    public string[] ErrorMessages { get; set; } = Array.Empty<string>();

    [RpcNullable]
    public string ErrorID { get; set; }

    [RpcNullable]
    public bool SessionRejected { get; set; }
}

public class ApiResult
{
    public static ApiResult<TPayload, TError> Ok<TPayload, TError>() where TError : ApiError
    {
        return new(true, default, default);
    }

    public static ApiResult<TPayload, TError> Ok<TPayload, TError>(TPayload payload) where TError : ApiError
    {
        return new(true, payload, default);
    }

    public static ApiResult<TPayload> Ok<TPayload>(TPayload payload)
    {
        return new(true, payload, default);
    }

    public static ApiResult<TPayload> Ok<TPayload>()
    {
        return new(true, default, default);
    }

    public static ApiResult<object> Ok()
    {
        return new(true, default, default);
    }

    public static ApiResult<TPayload, TError> Fail<TPayload, TError>(TError error) where TError : ApiError
    {
        return new(false, default, error);
    }

    public static ApiResult<TPayload> Fail<TPayload>(string[] errorMessages)
    {
        return new(false, default, new DefaultApiError { ErrorMessages = errorMessages });
    }

    public static ApiResult<TPayload> Fail<TPayload>(string errorMessage)
    {
        return new(false, default, new DefaultApiError { ErrorMessages = new[] { errorMessage } });
    }

    public static ApiResult<object> Fail(string[] errorMessages)
    {
        return new(false, default, new DefaultApiError { ErrorMessages = errorMessages });
    }

    public static ApiResult<object> Fail(string errorMessage)
    {
        return new(false, default, new DefaultApiError { ErrorMessages = new[] { errorMessage } });
    }

    public static ApiResult<object> Fail(string errorMessage, string errorID)
    {
        return new(false, default, new DefaultApiError { ErrorMessages = new[] { errorMessage }, ErrorID = errorID });
    }
}

public class ApiResult<TPayload, TError> : Result<TPayload, TError> where TError : ApiError
{
    public ApiResult(bool isOk, TPayload payload, TError error) : base(isOk, payload, error) { }
}

public class ApiResult<TPayload> : ApiResult<TPayload, DefaultApiError>
{
    public ApiResult(bool isOk, TPayload payload, DefaultApiError error) : base(isOk, payload, error) { }

    public static implicit operator ApiResult<TPayload>(TPayload payload)
    {
        return new ApiResult<TPayload>(true, payload, default);
    }

    public static implicit operator ApiResult<TPayload>(string errorMessage)
    {
        return new ApiResult<TPayload>(false, default, new DefaultApiError { ErrorMessages = new[] { errorMessage } });
    }
}
