using System;

namespace AnkrSDK.WalletConnectSharp.Unity.Network.Client
{
	public class WebSocketInvalidArgumentException : WebSocketException
	{
		public WebSocketInvalidArgumentException()
		{
		}

		public WebSocketInvalidArgumentException(string message) : base(message)
		{
		}

		public WebSocketInvalidArgumentException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}