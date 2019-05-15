using System;
using System.Text;
using Amazon.CognitoIdentityProvider.Model;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;

namespace awscognito
{
    public class CognitoSecretHash
    {
        /**
		 * Generates secret hash. Uses HMAC SHA256.
		 *
		 * @param userId            REQUIRED: User ID
		 * @param clientId          REQUIRED: Client ID
		 * @param clientSecret      REQUIRED: Client secret
		 * @return  secret hash as a {@code String}, {@code null } if {@code clinetSecret if null}
		 */
        public static String getSecretHash(String userId, String clientId, String clientSecret)
        {
            // Arguments userId and clientId have to be not-null.
            if (userId == null)
            {
                throw new InvalidParameterException("user ID cannot be null");
            }

            if (clientId == null)
            {
                throw new InvalidParameterException("client ID cannot be null");
            }

            // Return null as secret hash if clientSecret is null.
            if (clientSecret == null)
            {
                return null;
            }
            
            try
            {
                var mac = new HMac(new Sha256Digest());
                mac.Init(new KeyParameter(Encoding.UTF8.GetBytes(clientSecret)));

                byte[] idArr = Encoding.UTF8.GetBytes(userId);
                byte[] cleintArr = Encoding.UTF8.GetBytes(clientId);

                byte[] userIdContent = new byte[idArr.Length + clientId.Length];
                Buffer.BlockCopy(idArr, 0, userIdContent, 0, idArr.Length);
                Buffer.BlockCopy(cleintArr, 0, userIdContent, idArr.Length, clientId.Length);
                mac.BlockUpdate(userIdContent, 0, userIdContent.Length);
                
                byte[] rawHmac = new byte[mac.GetMacSize()];
                mac.DoFinal(rawHmac, 0);

                return Convert.ToBase64String(rawHmac);
            }
            catch (Exception e)
            {
                throw new InternalErrorException("errors in HMAC calculation");
            }
        }
    }
}
