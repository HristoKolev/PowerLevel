namespace PowerLevel.RpcGenerator;

using System.IO;
using Server;
using Xdxd.DotNet.Rpc;

public class Program
{
    public static void Main()
    {
        var engine = new RpcEngine(HttpServerApp.RpcEngineOptions);

        File.WriteAllText(Path.Combine(
            Path.GetDirectoryName(typeof(Program).Assembly.Location)!,
            "../../../../../server/PowerLevel.Server.Tests/RpcClient.cs"
        ), CSharpCodeGenerator.Generate(engine.Metadata));

        File.WriteAllText(Path.Combine(
            Path.GetDirectoryName(typeof(Program).Assembly.Location)!,
            "../../../../../client/src/infrastructure/RpcClient.ts"
        ), TypeScriptCodeGenerator.Generate(engine.Metadata));
    }
}
