using System.Threading.Tasks;
using MirageSDK.Core.Infrastructure;
using MirageSDK.Core.Utils;
using MirageSDK.Plugins.WalletConnectSharp.Core.Models.Ethereum;
using MirageSDK.Plugins.WalletConnectSharp.Unity;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.Eth.Transactions;
using Nethereum.Signer;
using Nethereum.Web3;

namespace MirageSDK.Core.Implementation
{
	public class CommonProvider : ICommonProvider
	{
		private readonly IWeb3 _web3Provider;
		
		public CommonProvider(IWeb3 web3Provider)
		{
			_web3Provider = web3Provider;
		}
		
		public Task<string> SendTransaction(
			string to,
			string data = null,
			string value = null,
			string gas = null,
			string gasPrice = null,
			string nonce = null
		)
		{
			var address = WalletConnect.ActiveSession.Accounts[0];

			var transactionData = new TransactionData
			{
				from = address,
				to = to,
				data = data,
				value = value != null ? MirageSDKHelpers.StringToBigInteger(value) : null,
				gas = gas != null ? MirageSDKHelpers.StringToBigInteger(gas) : null,
				gasPrice = gasPrice != null ? MirageSDKHelpers.StringToBigInteger(gasPrice) : null,
				nonce = nonce
			};

			return transactionData.SendTransaction();
		}
		
		public Task<TransactionReceipt> GetTransactionReceipt(string transactionHash)
		{
			return _web3Provider.TransactionManager.TransactionReceiptService.PollForReceiptAsync(transactionHash);
		}

		public Task<Transaction> GetTransaction(string transactionReceipt)
		{
			var src = new EthGetTransactionByHash(_web3Provider.Client);
			return src.SendRequestAsync(transactionReceipt);
		}
		
		/// <summary>
		/// Sign a message using  currently active session.
		/// </summary>
		/// <param name="messageToSign">Message you would like to sign</param>
		/// <returns>Signed message</returns>
		public Task<string> Sign(string messageToSign)
		{
			return WalletConnect.ActiveSession.EthSign(WalletConnect.ActiveSession.Accounts[0], messageToSign);
		}

		/// <summary>
		/// Checks if message was signed with provided <paramref name="signature"/>
		/// For more info look into Netherium.Signer implementation.
		/// </summary>
		/// <param name="messageToCheck"></param>
		/// <param name="signature"></param>
		/// <returns>Messages public address.</returns>
		public string CheckSignature(string messageToCheck, string signature)
		{
			var signer = new EthereumMessageSigner();
			return signer.EncodeUTF8AndEcRecover(messageToCheck, signature);
		}
	}
	
}