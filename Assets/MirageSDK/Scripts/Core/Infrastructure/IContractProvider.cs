namespace MirageSDK.Core.Infrastructure
{
	public interface IContractProvider
	{
		IContract GetContract(string contractAddress, string contractABI);
	}
}