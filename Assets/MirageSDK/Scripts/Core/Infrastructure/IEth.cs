using System.Collections.Generic;
using System.Threading.Tasks;
using MirageSDK.Core.Data;
using MirageSDK.Core.Implementation;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.RPC.Eth.DTOs;

namespace MirageSDK.Core.Infrastructure
{
	public interface IEth : ISignatureProvider
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