namespace PowerLevel.RpcGenerator;

using System.Collections.Generic;
using System.Linq;
using Xdxd.DotNet.Rpc;

public static class CSharpCodeGenerator
{
    public static string Generate(List<RpcRequestMetadata> metadata)
    {
        const string FILE_TEMPLATE = @"namespace PowerLevel.Server.Tests;

using System.Threading.Tasks;
using Auth;
using Xdxd.DotNet.Shared;

public abstract class RpcClient
{
    protected abstract Task<Result<TResponse>> RpcExecute<TRequest, TResponse>(TRequest request)
        where TRequest : class where TResponse : class;

{methods}
}
";
        var methods = metadata.Select(x =>
        {
            string methodName = x.RequestType.Name;

            const string REQUEST_POSTFIX = "request";

            if (methodName.ToLower().EndsWith(REQUEST_POSTFIX))
            {
                methodName = methodName.Remove(methodName.Length - REQUEST_POSTFIX.Length, REQUEST_POSTFIX.Length);
            }

            return $"    public virtual Task<Result<{x.ResponseType.Name}>> " +
                   $"{methodName}({x.RequestType.Name} request)\n    {{\n    " +
                   $"    return this.RpcExecute<{x.RequestType.Name}, {x.ResponseType.Name}>(request);\n    }}";
        });

        return FILE_TEMPLATE.Replace("{methods}", string.Join("\n\n", methods));
    }
}
