using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MirageSDK.Core.Infrastructure;
using MirageSDK.Plugins.WalletConnectSharp.Unity;
using Nethereum.Signer;
using Nethereum.Web3;
using WalletConnectSharp.NEthereum;

namespace MirageSDK.Core.Implementation
{
	public class MirageSDKWrapper : IMirageSDK
	{
		private readonly string _providerURI;
		private readonly Dictionary<string, Web3> _web3Providers = new Dictionary<string, Web3>();
		private readonly Eth _eth;

		private MirageSDKWrapper()
		{
		}

		private MirageSDKWrapper(string providerURI)
		{
			_providerURI = providerURI;
			
			var web3Provider = GetOrCreateWeb3Provider(_providerURI);
			_eth = new Eth(web3Provider);
		}

		/// <summary>
		/// Use this if you do not need to work with contracts or you want to use many web3 providers
		/// </summary>
		public static IMirageSDK GetSDKInstance()
		{
			return new MirageSDKWrapper();
		}

		/// <summary>
		/// Use this if you want to work with contracts from a single web3 provider.
		/// </summary>
		/// <param name="providerURI"></param>
		/// <returns></returns>
		public static IMirageSDK GetSDKInstance(string providerURI)
		{
			return new MirageSDKWrapper(providerURI);
		}

		/// <summary>
		/// Creates a contract handler to work with web3 using provided contract address and contract ABI
		/// </summary>
		/// <param name="contractAddress"></param>
		/// <param name="contractABI"></param>
		/// <param name="providerURI"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException">
		/// ProviderURI should be setup before usage of this method.
		/// Provider URI can be setup with SetupProviderURI() Method
		/// </exception>
		public IContract GetContract(string contractAddress, string contractABI)
		{
			if (string.IsNullOrEmpty(_providerURI))
			{
				throw new ArgumentNullException(nameof(_providerURI));
			}

			var web3Provider = GetOrCreateWeb3Provider(_providerURI);
			return GetContract(web3Provider, contractAddress, contractABI);
		}

		public Eth Eth()
		{
			return _eth;
		}
		
		public IContract GetContract(string providerURI, string contractAddress, string contractABI)
		{
			if (string.IsNullOrEmpty(providerURI))
			{
				throw new ArgumentNullException(nameof(providerURI));
			}

			var web3Provider = GetOrCreateWeb3Provider(providerURI);
			return GetContract(web3Provider, contractAddress, contractABI);
		}

		/// <summary>
		/// Creates a contract using provided web3 instance.
		/// </summary>
		/// <param name="web3">Web3 provider to process all further calls</param>
		/// <param name="contractAddress">Contract address</param>
		/// <param name="contractABI">Contract ABI</param>
		/// <returns>Initialized contract handler</returns>
		public IContract GetContract(IWeb3 web3, string contractAddress, string contractABI)
		{
			return new Contract(web3, _eth, contractAddress, contractABI);
		}

		private Web3 GetOrCreateWeb3Provider(string providerURI)
		{
			if (_web3Providers.ContainsKey(providerURI))
			{
				return _web3Providers[providerURI];
			}

			var web3Provider = CreateWeb3Provider(providerURI);
			_web3Providers.Add(providerURI, web3Provider);
			return web3Provider;
		}

		private static Web3 CreateWeb3Provider(string providerURI)
		{
			var wcProtocol = WalletConnect.Instance.Session;
			var client = wcProtocol.CreateProvider(new Uri(providerURI));
			var web3Provider = new Web3(client);
			return web3Provider;
		}
	}
}