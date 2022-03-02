namespace MirageSDK.Plugins.WalletConnectSharp.WalletConnectSharp.Core.Events
{
    public interface IEventProvider
    {
        void PropagateEvent(string topic, string responseJson);
    }
}