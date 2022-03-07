using MirageSDK.Core.Implementation;

namespace MirageSDK.Core.Infrastructure
{
	public interface IMirageSDK : IContractProvider
	{
		ICommonProvider Eth();
	}
}