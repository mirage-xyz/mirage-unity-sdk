using WalletConnectSharp.Core.Models;

namespace MirageSDK.Plugins.WalletConnectSharp.WalletConnectSharp.Core.Events.Model
{
    public class JsonRpcResponseEvent<T> : GenericEvent<T> where T : JsonRpcResponse
    {
    }
}