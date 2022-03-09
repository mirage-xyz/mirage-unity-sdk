namespace MirageSDK.Core.Infrastructure
{
	public interface IMirageSDK : IContractProvider
	{
		ICommonProvider Eth();
	}
}