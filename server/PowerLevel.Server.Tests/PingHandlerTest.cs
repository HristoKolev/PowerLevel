namespace PowerLevel.Server.Tests;

using System.Threading.Tasks;
using Xdxd.DotNet.Testing;
using Xunit;

public class PingHandlerTest : HttpServerAppTest
{
    [Fact]
    public async Task Ping_returns_correct_result()
    {
        var result = await this.RpcClient.Ping(new PingHandler.PingRequest());

        Snapshot.Match(result);
    }
}
