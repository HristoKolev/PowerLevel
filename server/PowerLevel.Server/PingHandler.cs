namespace PowerLevel.Server;

using System.Threading.Tasks;
using Infrastructure;
using Xdxd.DotNet.Rpc;

[RpcAuth(RequiresAuthentication = false)]
public class PingHandler
{
    public class PingRequest { }

    public class PingResponse
    {
        public string Message { get; set; }
    }

    [RpcBind(typeof(PingRequest), typeof(PingResponse))]
    public Task<PingResponse> Ping(PingRequest req)
    {
        return Task.FromResult(new PingResponse
        {
            Message = "Works.",
        });
    }
}
