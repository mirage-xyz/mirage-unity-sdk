using MirageSDK.Core.Implementation;

namespace MirageSDK.Core.Infrastructure
{
	public interface IMirageSDK : IContractProvider
	{
		Eth Eth();
	}
}