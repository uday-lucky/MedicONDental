/*
 * https://gist.github.com/CodesInChaos/8710228
 * https://crypto.stackexchange.com/users/180/codesinchaos* 
 */

using System;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;

namespace awscognito
{
	public class Hkdf
	{
		Func<byte[], byte[], byte[]> keyedHash;

		public Hkdf()
		{
			keyedHash = (key, message) =>
			{
			    var hmac = new HMac(new Sha256Digest());
                hmac.Init(new KeyParameter(key));;
                hmac.BlockUpdate(message, 0, message.Length);
                byte[] result = new byte[hmac.GetMacSize()];
				hmac.DoFinal(result, 0);
			    return result;
			};
		}

		public byte[] Extract(byte[] salt, byte[] inputKeyMaterial)
		{
			return keyedHash(salt, inputKeyMaterial);
		}

		public byte[] Expand(byte[] prk, byte[] info, int outputLength)
		{
			var resultBlock = new byte[0];
			var result = new byte[outputLength];
			var bytesRemaining = outputLength;
			for (int i = 1; bytesRemaining > 0; i++)
			{
				var currentInfo = new byte[resultBlock.Length + info.Length + 1];
				Array.Copy(resultBlock, 0, currentInfo, 0, resultBlock.Length);
				Array.Copy(info, 0, currentInfo, resultBlock.Length, info.Length);
				currentInfo[currentInfo.Length - 1] = (byte)i;
				resultBlock = keyedHash(prk, currentInfo);
				Array.Copy(resultBlock, 0, result, outputLength - bytesRemaining, Math.Min(resultBlock.Length, bytesRemaining));
				bytesRemaining -= resultBlock.Length;
			}
			return result;
		}

		public byte[] DeriveKey(byte[] salt, byte[] inputKeyMaterial, byte[] info, int outputLength)
		{
			var prk = Extract(salt, inputKeyMaterial);
			var result = Expand(prk, info, outputLength);
			return result;
		}
	}
}
