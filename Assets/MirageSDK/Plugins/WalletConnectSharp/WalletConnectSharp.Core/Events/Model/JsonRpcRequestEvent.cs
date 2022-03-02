using WalletConnectSharp.Core.Models;

namespace MirageSDK.Plugins.WalletConnectSharp.WalletConnectSharp.Core.Events.Model
{
    public class JsonRpcRequestEvent<T> : GenericEvent<T> where T : JsonRpcRequest
    {
    }
}