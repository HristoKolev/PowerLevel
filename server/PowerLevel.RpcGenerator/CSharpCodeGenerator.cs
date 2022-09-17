namespace PowerLevel.RpcGenerator;

using System.Collections.Generic;
using System.Linq;
using Xdxd.DotNet.Rpc;

public static class CSharpCodeGenerator
{
    private static string GetMethodName(RpcRequestMetadata metadata)
    {
        const string REQUEST_POSTFIX = "request";

        string methodName = metadata.RequestType.Name;

        if (methodName.ToLower().EndsWith(REQUEST_POSTFIX))
        {
            return methodName.Remove(methodName.Length - REQUEST_POSTFIX.Length, REQUEST_POSTFIX.Length);
        }

        return methodName;
    }

    public static string Generate(List<RpcRequestMetadata> metadata)
    {
        var methods = metadata.Select(x => $"    public virtual Task<ApiResult<{x.DeclaringType.Name}.{x.ResponseType.Name}>> " +
                                           $"{GetMethodName(x)}({x.DeclaringType.Name}.{x.RequestType.Name} request)\n    {{\n    " +
                                           $"    return this.RpcExecute<{x.DeclaringType.Name}.{x.RequestType.Name}, {x.DeclaringType.Name}.{x.ResponseType.Name}>(request);\n    }}");

        return @$"namespace PowerLevel.Server.Tests;

using System.Threading.Tasks;
using Auth;
using Infrastructure;

public abstract class RpcClient
{{
    protected abstract Task<ApiResult<TResponse>> RpcExecute<TRequest, TResponse>(TRequest request)
        where TRequest : class where TResponse : class;

{string.Join("\n\n", methods)}
}}
";
    }
}
