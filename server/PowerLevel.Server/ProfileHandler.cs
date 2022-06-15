namespace PowerLevel.Server;

using System.Threading.Tasks;
using Xdxd.DotNet.Rpc;

public class ProfileHandler
{
    [RpcBind(typeof(ProfileInfoRequest), typeof(ProfileInfoResponse))]
    public async Task<ProfileInfoResponse> ProfileInfo(ProfileInfoRequest req)
    {
        await Task.CompletedTask;
        return new ProfileInfoResponse
        {
            Count = req.Count + 1,
        };
    }
}

public class ProfileInfoRequest
{
    public int Count { get; set; }
}

public class ProfileInfoResponse
{
    public int Count { get; set; }
}
