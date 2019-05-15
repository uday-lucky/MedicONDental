using System;
using Org.BouncyCastle.Crypto.Digests;
using System.Text;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Math;

namespace awscognito
{
    public class CognitoDeviceHelper
    {
        private static readonly String COGNITO_DEVICE_CACHE = "CognitoIdentityProviderDeviceCache";
        private static readonly String COGNITO_DEVICE_KEY = "DeviceKey";
        private static readonly String COGNITO_DEVICE_GROUP_KEY = "DeviceGroupKey";
        private static readonly String COGNITO_DEVICE_SECRET = "DeviceSecret";

        static deviceSRP srpCalculator = null;

        public static string getDeviceKey(String username, String userPoolId)
        {
            //try
            //{
            //    var cipCachedDeviceDetailsRecord = new SecRecord(SecKind.GenericPassword)
            //    {
            //        Server = getDeviceDetailsCacheForUser(username, userPoolId),
            //        Account = COGNITO_DEVICE_KEY
            //    };
            //    NSData data = SecKeyChain.QueryAsData(cipCachedDeviceDetailsRecord);
            //    if (data != null)
            //    {
            //        return data.ToString(NSStringEncoding.UTF8);
            //    }
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("Error accessing Keychain", e);
            //}

            return null;
        }

        /**
         * This method caches the device key. Device key is assigned by the Amazon Cognito service and is
         * used as a device identifier.
         *
         * @param username          REQUIRED: The current user.
         * @param userPoolId        REQUIRED: Client ID of the device.
         * @param deviceKey         REQUIRED: Cognito assigned device key.
         */
        public static void cacheDeviceKey(String username, String userPoolId, String deviceKey)
        {
            //try
            //{
            //    var cipCachedDeviceDetailsRecord = new SecRecord(SecKind.GenericPassword)
            //    {
            //        Server = getDeviceDetailsCacheForUser(username, userPoolId),
            //        Account = COGNITO_DEVICE_KEY
            //    };

            //    SecKeyChain.Remove(cipCachedDeviceDetailsRecord);
            //    cipCachedDeviceDetailsRecord.ValueData = NSData.FromString(deviceKey);
            //    SecKeyChain.Add(cipCachedDeviceDetailsRecord);
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("Error accessing Keychain", e);
            //}
        }

        /**
         * This method caches the device verifier. Device verifier is generated locally by the SDK and
         * it is used to authenticate the device through device SRP authentication.
         *
         * @param username          REQUIRED: The current user.
         * @param userPoolId        REQUIRED: Client ID of the device.
         * @param deviceSecret      REQUIRED: Cognito assigned device key.
         */
        public static void cacheDeviceVerifier(String username, String userPoolId, String deviceSecret)
        {
            //try
            //{
            //    var cipCachedDeviceDetailsRecord = new SecRecord(SecKind.GenericPassword)
            //    {
            //        Server = getDeviceDetailsCacheForUser(username, userPoolId),
            //        Account = COGNITO_DEVICE_SECRET
            //    };

            //    SecKeyChain.Remove(cipCachedDeviceDetailsRecord);
            //    cipCachedDeviceDetailsRecord.ValueData = NSData.FromString(deviceSecret);
            //    SecKeyChain.Add(cipCachedDeviceDetailsRecord);
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("Error accessing Keychain", e);
            //}
        }

        /**
		 * This method caches the device group key. Device verifier is generated locally by the SDK and
		 * it is used to authenticate the device through device SRP authentication.
		 *
		 * @param username          REQUIRED: The current user.
		 * @param userPoolId        REQUIRED: Client ID of the device.
		 * @param deviceGroupKey    REQUIRED: Cognito assigned device group key.
		 */
        public static void cacheDeviceGroupKey(String username, String userPoolId, String deviceGroupKey)
        {
            //try
            //{
            //    var cipCachedDeviceDetailsRecord = new SecRecord(SecKind.GenericPassword)
            //    {
            //        Server = getDeviceDetailsCacheForUser(username, userPoolId),
            //        Account = COGNITO_DEVICE_GROUP_KEY
            //    };

            //    SecKeyChain.Remove(cipCachedDeviceDetailsRecord);
            //    cipCachedDeviceDetailsRecord.ValueData = NSData.FromString(deviceGroupKey);
            //    SecKeyChain.Add(cipCachedDeviceDetailsRecord);
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("Error accessing Keychain", e);
            //}
        }

        /**
		 * Clears cached device details for this user.
		 *
		 * @param username          REQUIRED: The current user.
		 * @param userPoolId        REQUIRED: Client ID of the device.
		 */
        public static void clearCachedDevice(String username, String userPoolId)
        {
            //try
            //{
            //    var cipCachedDeviceDetailsRecord = new SecRecord(SecKind.GenericPassword)
            //    {
            //        Server = getDeviceDetailsCacheForUser(username, userPoolId)
            //    };

            //    var result = SecKeyChain.Remove(cipCachedDeviceDetailsRecord);
            //    if (result != SecStatusCode.Success)
            //    {
            //        Console.WriteLine("Remove failed?");
            //    }
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("Error accessing Keychain", e);
            //}
        }

        private static String getDeviceDetailsCacheForUser(String username, String userPoolId)
        {
            return COGNITO_DEVICE_CACHE + "." + userPoolId + "." + username;
        }

        private class deviceSRP
        {
            private static String HEX_N =
                    "FFFFFFFFFFFFFFFFC90FDAA22168C234C4C6628B80DC1CD1"
                            + "29024E088A67CC74020BBEA63B139B22514A08798E3404DD"
                            + "EF9519B3CD3A431B302B0A6DF25F14374FE1356D6D51C245"
                            + "E485B576625E7EC6F44C42E9A637ED6B0BFF5CB6F406B7ED"
                            + "EE386BFB5A899FA5AE9F24117C4B1FE649286651ECE45B3D"
                            + "C2007CB8A163BF0598DA48361C55D39A69163FA8FD24CF5F"
                            + "83655D23DCA3AD961C62F356208552BB9ED529077096966D"
                            + "670C354E4ABC9804F1746C08CA18217C32905E462E36CE3B"
                            + "E39E772C180E86039B2783A2EC07A28FB5C55DF06F4C52C9"
                            + "DE2BCBF6955817183995497CEA956AE515D2261898FA0510"
                            + "15728E5A8AAAC42DAD33170D04507A33A85521ABDF1CBA64"
                            + "ECFB850458DBEF0A8AEA71575D060C7DB3970F85A6E1E4C7"
                            + "ABF5AE8CDB0933D71E8C94E04A25619DCEE3D2261AD2EE6B"
                            + "F12FFA06D98A0864D87602733EC86A64521F2B18177B200C"
                            + "BBE117577A615D6C770988C0BAD946E208E24FA074E5AB31"
                            + "43DB5BFCE0FD108E4B82D120A93AD2CAFFFFFFFFFFFFFFFF";


            public static BigInteger N = new BigInteger(HEX_N, 16);
            private static BigInteger GG = BigInteger.ValueOf(2);

            public static int EPHEMERAL_KEY_LENGTH = 1024;
            public static int DERIVED_KEY_SIZE = 16;
            public static String DERIVED_KEY_INFO = "Caldera Derived Key";
            public static SecureRandom SECURE_RANDOM;

            private static int SALT_LENGTH_BITS = 128;


            static deviceSRP()
            {
                SECURE_RANDOM = SecureRandom.GetInstance("SHA1PRNG");
            }

            [ThreadStatic]
            private static Sha256Digest THREAD_MESSAGE_DIGEST = new Sha256Digest();


            private BigInteger salt;
            private BigInteger verifier;

            /**
			 * Helps to start the SRP validation of the device.
			 * @param deviceGroupKey REQUIRED: Group assigned to the device.
			 * @param deviceKey REQUIRED: Unique identifier assigned to the device. 
			 * @param password REQUIRED: The device password.
			 */
            public deviceSRP(String deviceGroupKey, String deviceKey, String password)
            {
                byte[] deviceKeyHash = getUserIdHash(deviceGroupKey, deviceKey, password);

                salt = new BigInteger(SALT_LENGTH_BITS, SECURE_RANDOM);
                verifier = calcVerifier(salt, deviceKeyHash);
            }

            /**
			 * Generates the SRP verifier.
			 * @param salt REQUIRED: The random salt created by the service.
			 * @param userIdHash REQIURED: Username hash.
			 * @return verifier as a BigInteger.
			 */
            private static BigInteger calcVerifier(BigInteger salt, byte[] userIdHash)
            {
                begin();
                update(salt);
                update(userIdHash);
                byte[] digest = end();

                BigInteger x = new BigInteger(1, digest);
                return GG.ModPow(x, N);
            }

            /**
			 * Computes the user hash.
			 * @param poolName REQUIRED: The pool-id of the user.
			 * @param userName REQUIRED: The internal username of the user.
			 * @param password REQUIRED: The password intered by the user.
			 * @return hash as a byte array.
			 */
            private byte[] getUserIdHash(String poolName, String userName, String password)
            {
                begin();
                update(poolName, userName, ":", password);
                return end();
            }

            /**
			 * Start byte digest for SRP.
			 */
            public static void begin()
            {
                Sha256Digest md = THREAD_MESSAGE_DIGEST;
                md.Reset();
            }

            /**
			 * Complete digest.
			 * @return the digest as a byte array.
			 */
            public static byte[] end()
            {
                Sha256Digest md = THREAD_MESSAGE_DIGEST;
                byte[] output = new byte[md.GetDigestSize()];
                md.DoFinal(output, 0);
                return output;
            }

            /**
			 * Adds a series of strings to the digest.
			 * @param strings REQUIRED: Strings to add.
			 */
            public static void update(params string[] strings)
            {
                Sha256Digest md = THREAD_MESSAGE_DIGEST;
                foreach (string s in strings)
                {
                    if (s != null)
                    {
                        var encData = Encoding.UTF8.GetBytes(s);
                        md.BlockUpdate(encData, 0, encData.Length);
                    }
                }
            }

            /**
			 * Adds a string to the digest.
			 * @param s REQUIRED: String to add.
			 */
            public static void update(String s)
            {
                Sha256Digest md = THREAD_MESSAGE_DIGEST;
                if (s != null)
                {
                    var encData = Encoding.UTF8.GetBytes(s);
                    md.BlockUpdate(encData, 0, encData.Length);
                }
            }

            /**
			 * Adds a series of BigIntegers to the digest.
			 * @param bigInts REQUIRED: Numbers to add.
			 */
            public static void update(params BigInteger[] bigInts)
            {
                Sha256Digest md = THREAD_MESSAGE_DIGEST;
                foreach (BigInteger n in bigInts)
                {
                    if (n != null)
                    {
                        var encData = n.ToByteArray();
                        md.BlockUpdate(encData, 0, encData.Length);
                    }
                }
            }

            /**
			 * Adds a BigInteger to the digest.
			 * @param n REQUIRED: The number to add.
			 */
            public static void update(BigInteger n)
            {
                Sha256Digest md = THREAD_MESSAGE_DIGEST;
                if (n != null)
                {
                    var encData = n.ToByteArray();
                    md.BlockUpdate(encData, 0, encData.Length);
                }
            }

            /**
			 * Adds a byte array to the digest.
			 * @param b REQUIRED: bytes to add.
			 */
            public static void update(byte[] b)
            {
                Sha256Digest md = THREAD_MESSAGE_DIGEST;
                if (b != null)
                {
                    md.BlockUpdate(b, 0, b.Length);
                }
            }
        }
    }
}
