﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MirageSDK.Core.Data;
using MirageSDK.Core.Infrastructure;
using MirageSDK.Core.Utils;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.Eth.Transactions;
using Nethereum.Web3;
using WalletConnectSharp.Core.Models.Ethereum;
using WalletConnectSharp.Unity;

namespace MirageSDK.Core.Implementation
{	
	internal class Contract : IContract
	{
		private readonly string _contractABI;
		private readonly string _contractAddress;
		private readonly IWeb3 _web3Provider;

		internal Contract(IWeb3 web3Provider, string contractAddress, string contractABI)
		{
			_web3Provider = web3Provider;
			_contractABI = contractABI;
			_contractAddress = contractAddress;
		}

		public Task<TReturnType> GetData<TFieldData, TReturnType>(TFieldData requestData = null)
			where TFieldData : FunctionMessage, new()
		{
			var contract = _web3Provider.Eth.GetContractHandler(_contractAddress);
			return contract.QueryAsync<TFieldData, TReturnType>(requestData);
		}

		public Task<List<EventLog<TEvDto>>> GetAllChanges<TEvDto>(EventFilterData evFilter)
			where TEvDto : IEventDTO, new()
		{
			var eventHandler = _web3Provider.Eth.GetEvent<TEvDto>(_contractAddress);

			var filters = EventFilterHelper.CreateEventFilters(eventHandler, evFilter);

			return eventHandler.GetAllChangesAsync(filters);
		}

		public Task<string> CallMethod(string methodName, object[] arguments = null, string gas = null)
		{
			var activeSessionAccount = WalletConnect.ActiveSession.Accounts[0];
			var raw = _web3Provider.Eth.GetContract(_contractABI, _contractAddress)
				.GetFunction(methodName)
				.CreateTransactionInput(activeSessionAccount, arguments);

			return SendTransaction(_contractAddress, raw.Data, null, gas);
		}
		
		public EventController SendMethod(string methodName, object[] arguments = null, string gas = null)
		{
			var evController = new EventController();
		
			TransactionInput raw = _web3Provider.Eth.GetContract(_contractABI, _contractAddress)
				.GetFunction(methodName)
				.CreateTransactionInput(WalletConnect.ActiveSession.Accounts[0], arguments);
		
			evController.InvokeSendingEvent(raw);
				
			Task<string> task = SendTransaction(_contractAddress, raw.Data, null, gas);
			
			evController.InvokeSentEvent(raw);
			
			task.ContinueWith(_task =>
			{
				if (!_task.IsFaulted)
				{
					var transactionHash = _task.Result;
					LoadReceipt(transactionHash, evController);
				}
				else
				{
					evController.SetError(_task.Exception);
				}
			});

			return evController;
		}
	
		private void LoadReceipt(string transactionHash, EventController evController)
		{
			evController.SetTransactionHash(transactionHash);			
			
			var receiptTask = GetTransactionReceipt(transactionHash);
			receiptTask.ContinueWith(_task =>
			{
				if (!_task.IsFaulted)
				{
					var receipt = _task.Result;
					evController.SetReceipt(receipt);
				}
				else
				{
					evController.SetError(_task.Exception);
				}
			});
		}

		public Task<TransactionReceipt> GetTransactionReceipt(string transactionHash)
		{
			return _web3Provider.TransactionManager.TransactionReceiptService.PollForReceiptAsync(transactionHash);
		}

		public Task<Transaction> GetTransactionInfo(string transactionReceipt)
		{
			var src = new EthGetTransactionByHash(_web3Provider.Client);
			return src.SendRequestAsync(transactionReceipt);
		}

		public async Task<string> SendTransaction(
			string to,
			string data = null,
			string value = null,
			string gas = null)
		{
			var address = WalletConnect.ActiveSession.Accounts[0];

			var transactionData = new TransactionData
			{
				from = address,
				to = to
			};

			if (data != null)
			{
				transactionData.data = data;
			}

			if (value != null)
			{
				transactionData.value = MirageSDKHelpers.StringToBigInteger(value);
			}

			if (gas != null)
			{
				transactionData.gas = MirageSDKHelpers.StringToBigInteger(gas);
			}

			return await transactionData.SendTransaction();
		}
	}
}