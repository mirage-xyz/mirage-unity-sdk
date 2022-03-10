using MirageSDK.Core.Implementation;

namespace MirageSDK.Core.Infrastructure
{
	public interface IMirageSDK : IContractProvider
	{
		EthHandler Eth { get; }
	}
}