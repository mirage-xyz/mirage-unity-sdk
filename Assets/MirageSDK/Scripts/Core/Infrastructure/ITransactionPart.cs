using System.Threading.Tasks;
using Nethereum.RPC.Eth.DTOs;

namespace MirageSDK.Core.Infrastructure
{
	public interface ITransactionPart
	{
		Task<string> SendTransaction(
			string to,
			string data = null,
			string value = null,
			string gas = null,
			string gasPrice = null,
			string nonce = null
		);

		Task<TransactionReceipt> GetTransactionReceipt(string transactionHash);

		Task<Transaction> GetTransaction(string transactionReceipt);
	}
}