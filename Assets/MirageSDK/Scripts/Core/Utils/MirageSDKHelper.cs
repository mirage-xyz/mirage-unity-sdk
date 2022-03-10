using System.Numerics;
using System.Text;
using UnityEngine.Networking;

namespace MirageSDK.Core.Utils
{
	public static class MirageSDKHelper
	{
		public static string StringToBigInteger(string value)
		{
			var bnValue = BigInteger.Parse(value);
			return "0x" + bnValue.ToString("X");
		}

		public static UnityWebRequest GetUnityWebRequestFromJSON(string url, string json)
		{
			var request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
			var bytes = GetBytes(json);
			var uH = new UploadHandlerRaw(bytes);
			request.uploadHandler = uH;
			request.SetRequestHeader("Content-Type", "application/json");
			request.downloadHandler = new DownloadHandlerBuffer();
			return request;
		}

		private static byte[] GetBytes(string str)
		{
			var bytes = Encoding.UTF8.GetBytes(str);
			return bytes;
		}
	}
}