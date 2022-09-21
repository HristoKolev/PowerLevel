namespace PowerLevel.Server.Infrastructure;

using System;
using System.Buffers;
using System.Linq;
using System.Threading.Tasks;
using Auth;
using Microsoft.AspNetCore.Http;
using Xdxd.DotNet.Http;
using Xdxd.DotNet.Logging;
using Xdxd.DotNet.Rpc;
using Xdxd.DotNet.Shared;

/// <summary>
/// Serves Rpc requests over HTTP.
/// </summary>
public class RpcRequestHandler
{
    private readonly RpcEngine rpcEngine;
    private readonly InstanceProvider instanceProvider;
    private readonly ErrorReporter errorReporter;

    public RpcRequestHandler(RpcEngine rpcEngine, InstanceProvider instanceProvider, ErrorReporter errorReporter)
    {
        this.rpcEngine = rpcEngine;
        this.instanceProvider = instanceProvider;
        this.errorReporter = errorReporter;
    }

    public async Task HandleRpcRequest(HttpRequestState httpRequestState)
    {
        Result<object, object> result;

        try
        {
            result = await this.ProcessRequest(httpRequestState);
        }
        catch (Exception err)
        {
            string errorID = await this.errorReporter.Error(err, "RPC_GENERAL_SERVER_ERROR");
            result = ApiResult.Fail("General RPC server error.", errorID).ToGeneralForm();
        }

        try
        {
            httpRequestState.HttpContext.Response.StatusCode = 200;
            httpRequestState.HttpContext.Response.Headers.ContentType = "application/json";
            await JsonHelper.Serialize(httpRequestState.HttpContext.Response.Body, result);
        }
        catch (Exception err)
        {
            await this.errorReporter.Error(err, "RPC_SERVER_FAILED_TO_WRITE_RESPONSE");
        }
    }

    private async Task<Result<object, object>> ProcessRequest(HttpRequestState httpRequestState)
    {
        // Find request metadata.
        if (string.IsNullOrWhiteSpace(httpRequestState.RpcState.RpcRequestType))
        {
            return ApiResult.Fail("Request type is null or an empty string.").ToGeneralForm();
        }

        // Read request body.
        try
        {
            httpRequestState.RpcState.RpcRequestBody = await httpRequestState.HttpContext.Request.ReadToEnd();

            if (httpRequestState.RpcState.RpcRequestBody.Memory.Length == 0)
            {
                return ApiResult.Fail("The HTTP request has an empty body.").ToGeneralForm();
            }
        }
        catch (Exception err)
        {
            string errorID = await this.errorReporter.Error(err, "FAILED_TO_READ_RPC_BODY");
            return ApiResult.Fail("Failed to read RPC request body.", errorID).ToGeneralForm();
        }

        var metadata = this.rpcEngine.GetMetadataByRequestName(httpRequestState.RpcState.RpcRequestType);

        if (metadata == null)
        {
            return ApiResult.Fail($"No RPC handler for request. RequestType: {httpRequestState.RpcState.RpcRequestType}.").ToGeneralForm();
        }

        // Parse the RPC message.
        try
        {
            httpRequestState.RpcState.RpcRequestPayload = JsonHelper.Deserialize(
                httpRequestState.RpcState.RpcRequestBody.Memory.Span,
                metadata.RequestType
            );
        }
        catch (Exception err)
        {
            string errorID = await this.errorReporter.Error(err, "FAILED_TO_PARSE_RPC_BODY");
            return ApiResult.Fail("Failed to parse RPC body.", errorID).ToGeneralForm();
        }

        // Execute.
        try
        {
            var requestMessage = new RpcRequestMessage
            {
                Type = httpRequestState.RpcState.RpcRequestType,
                Payload = httpRequestState.RpcState.RpcRequestPayload,
            };

            httpRequestState.RpcState.RpcResponse = await this.rpcEngine.Execute(requestMessage, this.instanceProvider);
        }
        catch (Exception err)
        {
            string errorID = await this.errorReporter.Error(err);
            return ApiResult.Fail("Failed to execute RPC request.", errorID).ToGeneralForm();
        }

        return httpRequestState.RpcState.RpcResponse;
    }
}

public class RpcInputValidationMiddleware : RpcMiddleware
{
    public async Task Run(RpcContext context, InstanceProvider instanceProvider, RpcRequestDelegate next)
    {
        var validationResult = InputValidator.Validate(context.RequestMessage.Payload);

        if (!validationResult.IsOk)
        {
            context.SetResponse(ApiResult.Fail(validationResult.Error).ToGeneralForm());
            return;
        }

        await next(context, instanceProvider);
    }
}

public class RpcAuthorizationMiddleware : RpcMiddleware
{
    private static readonly RpcAuthAttribute DefaultAuthAttribute = new()
    {
        RequiresAuthentication = true,
    };

    private readonly HttpRequestState httpRequestState;

    public RpcAuthorizationMiddleware(HttpRequestState httpRequestState)
    {
        this.httpRequestState = httpRequestState;
    }

    private static ApiResult<object, DefaultApiError> CreateUnauthorizedAccessError()
    {
        const string UNAUTHORIZED_ACCESS_MESSAGE = "Unauthorized access.";

        return new ApiResult<object, DefaultApiError>(false, null,
            new DefaultApiError
            {
                SessionRejected = true,
                ErrorMessages = new[]
                {
                    UNAUTHORIZED_ACCESS_MESSAGE,
                },
            });
    }

    public async Task Run(RpcContext context, InstanceProvider instanceProvider, RpcRequestDelegate next)
    {
        var authAttribute = context.GetSupplementalAttribute<RpcAuthAttribute>() ?? DefaultAuthAttribute;

        var authResult = this.httpRequestState.AuthResult;

        bool isAuthenticated = authResult.IsAuthenticated && authResult.ValidCsrfToken;

        if (authAttribute.RequiresAuthentication && !isAuthenticated)
        {
            context.SetResponse(CreateUnauthorizedAccessError().ToGeneralForm());
            return;
        }

        context.SetHandlerArgument(authResult);

        await next(context, instanceProvider);
    }
}

public class RpcAuthAttribute : RpcSupplementalAttribute
{
    public bool RequiresAuthentication { get; set; }
}

public class RpcConstantTimeMiddleware : RpcMiddleware
{
    public async Task Run(RpcContext context, InstanceProvider instanceProvider, RpcRequestDelegate next)
    {
        var nextTask = next(context, instanceProvider);

        var constantTimeAttribute = context.GetSupplementalAttribute<RpcConstantTimeAttribute>();

        if (constantTimeAttribute != null)
        {
            await Task.Delay(constantTimeAttribute.Delay);
        }

        await nextTask;
    }
}

public class RpcConstantTimeAttribute : RpcSupplementalAttribute
{
    public int Delay { get; }

    public RpcConstantTimeAttribute(int delay)
    {
        this.Delay = delay;
    }
}

public class HttpRequestState : IDisposable
{
    public HttpContext HttpContext { get; set; }

    public AuthResult AuthResult { get; set; }

    public DateTime RequestStart { get; set; }

    public DateTime RequestEnd { get; set; }

    public RpcRequestState RpcState { get; set; }

    public void Dispose()
    {
        this.RpcState?.Dispose();
    }
}

public class RpcRequestState : IDisposable
{
    public string RpcRequestType { get; set; }

    public IMemoryOwner<byte> RpcRequestBody { get; set; }

    public object RpcRequestPayload { get; set; }

    public Result<object, object> RpcResponse { get; set; }

    public void Dispose()
    {
        this.RpcRequestBody?.Dispose();
    }
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class RpcNullableAttribute : Attribute { }

public class HttpLogData : AppInfoEventData
{
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global

    public HttpLogData(HttpRequestState httpRequestState, bool detailedLog)
    {
        var context = httpRequestState.HttpContext;
        var connectionInfo = context.Connection;
        var httpRequest = context.Request;

        this.LocalIp = connectionInfo.LocalIpAddress + ":" + connectionInfo.LocalPort;
        this.RemoteIp = connectionInfo.RemoteIpAddress + ":" + connectionInfo.RemotePort;
        this.HttpRequestID = connectionInfo.Id;
        this.Method = httpRequest.Method;
        this.Path = httpRequest.Path.ToString();
        this.Query = httpRequest.QueryString.ToString();
        this.Protocol = httpRequest.Protocol;
        this.Scheme = httpRequest.Scheme;
        this.Aborted = context.RequestAborted.IsCancellationRequested;
        this.StatusCode = context.Response.StatusCode;

        // --------

        this.RequestStart = httpRequestState.RequestStart;
        this.RequestEnd = httpRequestState.RequestEnd;
        this.RequestDurationMs = (long)(httpRequestState.RequestEnd - httpRequestState.RequestStart).TotalMilliseconds;

        // --------

        if (httpRequestState.AuthResult != null)
        {
            this.SessionID = httpRequestState.AuthResult.SessionID;
            this.LoginID = httpRequestState.AuthResult.LoginID;
            this.ProfileID = httpRequestState.AuthResult.ProfileID;
            this.ValidCsrfToken = httpRequestState.AuthResult.ValidCsrfToken;
        }

        var rpcState = httpRequestState.RpcState;

        if (rpcState != null)
        {
            this.RpcRequestType = rpcState.RpcRequestType;
        }

        if (detailedLog)
        {
            this.HeadersJson = JsonHelper.Serialize(httpRequest.Headers.ToDictionary(x => x.Key, x => x.Value));

            if (rpcState != null)
            {
                if (rpcState.RpcRequestBody != null)
                {
                    this.RpcRequestBodyBase64 = Convert.ToBase64String(rpcState.RpcRequestBody.Memory.Span);
                }

                if (rpcState.RpcRequestPayload != null)
                {
                    this.RpcRequestPayloadJson = JsonHelper.Serialize(rpcState.RpcRequestPayload);
                }

                if (rpcState.RpcResponse != null)
                {
                    this.RpcResponseJson = JsonHelper.Serialize(rpcState.RpcResponse);
                }
            }
        }
    }

    public string LocalIp { get; set; }

    public string RemoteIp { get; set; }

    public string HttpRequestID { get; set; }

    public string Method { get; set; }

    public string Path { get; set; }

    public string Query { get; set; }

    public string Protocol { get; set; }

    public string Scheme { get; set; }

    public bool Aborted { get; set; }

    public int StatusCode { get; set; }

    public string HeadersJson { get; set; }

    // --------

    public DateTime RequestStart { get; set; }

    public DateTime RequestEnd { get; set; }

    public long RequestDurationMs { get; set; }

    // --------

    public int SessionID { get; set; }

    public int LoginID { get; set; }

    public int ProfileID { get; set; }

    public bool ValidCsrfToken { get; set; }

    // --------

    public string RpcRequestType { get; set; }

    public string RpcRequestBodyBase64 { get; set; }

    public string RpcRequestPayloadJson { get; set; }

    public string RpcResponseJson { get; set; }

    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore AutoPropertyCanBeMadeGetOnly.Global
    // ReSharper restore MemberCanBePrivate.Global
}

public static class HttpLoggingExtensions
{
    public const string HTTP_KEY = "HTTP_REQUESTS";
    public const string HTTP_DETAILED_KEY = "HTTP_REQUESTS_DETAILED";

    public static void Http(this Log log, Func<HttpLogData> func)
    {
        log.Log(HTTP_KEY, func);
    }

    public static void HttpDetailed(this Log log, Func<HttpLogData> func)
    {
        log.Log(HTTP_DETAILED_KEY, func);
    }
}
