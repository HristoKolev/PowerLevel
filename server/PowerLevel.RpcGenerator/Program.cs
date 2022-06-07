namespace PowerLevel.RpcGenerator;

using System.IO;
using System.Threading.Tasks;
using Server;
using Xdxd.DotNet.Rpc;

public class Program
{
    public static async Task Main()
    {
        var engine = new RpcEngine(HttpServerApp.RpcEngineOptions);

        await File.WriteAllTextAsync(Path.Combine(
            Path.GetDirectoryName(typeof(Program).Assembly.Location)!,
            "../../../../../server/PowerLevel.Server.Tests/RpcClient.cs"
        ), CSharpCodeGenerator.Generate(engine.Metadata));

        await File.WriteAllTextAsync(Path.Combine(
            Path.GetDirectoryName(typeof(Program).Assembly.Location)!,
            "../../../../../client/src/infrastructure/RpcClient.ts"
        ), TypeScriptCodeGenerator.Generate(engine.Metadata));
    }
}
