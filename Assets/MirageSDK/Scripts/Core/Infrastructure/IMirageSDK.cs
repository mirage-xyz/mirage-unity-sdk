namespace MirageSDK.Core.Infrastructure
{
	public interface IMirageSDK : IContractProvider
	{
		IEthHandler Eth { get; }
	}
}