using System;
using System.Collections.Generic;
using System.IO;
using Android.App;
using Android.Content;
using Android;
using Android.OS;
using Android.Security;
using Android.Security.Keystore;
using Android.Util;
using Java.IO;
using Java.Lang;
using Java.Math;
using Java.Security;
using Java.Util;
using Javax.Crypto;
using Javax.Crypto.Spec;
using Javax.Security.Auth.X500;
using MedCon.Services;
using Xamarin.Forms;
using MedCon.Droid.DependencySerices;

[assembly:Dependency(typeof(SecureStorage))]
namespace MedCon.Droid.DependencySerices
{
    public class SecureStorage : ISecureStorage
    {

        private static readonly string ANDROID_KEY_STORE_NAME = "AndroidKeyStore";
        private static readonly string AES_MODE_M_OR_GREATER = "AES/GCM/NoPadding";
        private static readonly string AES_MODE_LESS_THAN_M = "AES/ECB/PKCS7Padding";
        private static readonly string KEY_ALIAS = "YOUR-KeyAliasForCookieValueEncryption";
        private static readonly string FIXED_IV = "YOUR-12-char";
        private static readonly string CHARSET_NAME = "UTF-8";
        private static readonly string RSA_ALGORITHM_NAME = "RSA";
        private static readonly string RSA_MODE = "RSA/ECB/PKCS1Padding";
        private static readonly string CIPHER_PROVIDER_NAME_ENCRYPTION_DECRYPTION_RSA = "AndroidOpenSSL";
        private static readonly string CIPHER_PROVIDER_NAME_ENCRYPTION_DECRYPTION_AES = "BC";
        private static readonly string SHARED_PREFERENCE_NAME = "YOUR-EncryptedKeysSharedPreferences";
        private static readonly string ENCRYPTED_KEY_NAME = "YOUR-EncryptedKeysKeyName";

        public SecureStorage()
        {
            initKeys();
        }

        private void initKeys()
        {
            KeyStore keyStore = KeyStore.GetInstance(ANDROID_KEY_STORE_NAME);
            keyStore.Load(null);

            if (!keyStore.ContainsAlias(KEY_ALIAS))
            {
                initValidKeys();
            }
            else
            {
                bool keyValid = false;
                KeyStore.IEntry keyEntry = keyStore.GetEntry(KEY_ALIAS, null);

                if (keyEntry is KeyStore.SecretKeyEntry && Build.VERSION.SdkInt >= BuildVersionCodes.M)
                {
                    keyValid = true;
                }

                if (keyEntry is KeyStore.PrivateKeyEntry && Build.VERSION.SdkInt < BuildVersionCodes.M)
                {
                    keyValid = true;
                }

                if (!keyValid)
                {
                    // System upgrade or something made key invalid
                    keyStore.DeleteEntry(KEY_ALIAS);

                    removeSavedSharedPreferences();

                    initValidKeys();
                }
            }
        }


        private void initValidKeys()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                generateKeysForAPIMOrGreater();
            }
            else
            {
                generateKeysForAPILessThanM();
            }
        }

        private void removeSavedSharedPreferences()
        {
            ISharedPreferences pref = Android.App.Application.Context.GetSharedPreferences(SHARED_PREFERENCE_NAME, FileCreationMode.Private);
            pref.Edit().Clear().Commit();
        }

        private void generateKeysForAPILessThanM()
        {
            // Generate a key pair for encryption
            Calendar start = Calendar.Instance;
            Calendar end = Calendar.Instance;
            end.Add(CalendarField.Year, 30);
            KeyPairGeneratorSpec spec = new KeyPairGeneratorSpec.Builder(Android.App.Application.Context)
                                                                .SetAlias(KEY_ALIAS)
                                                                .SetSubject(new X500Principal("CN=" + KEY_ALIAS))
                                                                .SetSerialNumber(BigInteger.Ten)
                                                                .SetStartDate(start.Time)
                                                                .SetEndDate(end.Time)
                                                                .Build();
            KeyPairGenerator kpg = KeyPairGenerator.GetInstance(RSA_ALGORITHM_NAME, ANDROID_KEY_STORE_NAME);
            kpg.Initialize(spec);
            kpg.GenerateKeyPair();


            saveEncryptedKey();
        }

        private void saveEncryptedKey()
        {
            ISharedPreferences pref = Android.App.Application.Context.GetSharedPreferences(SHARED_PREFERENCE_NAME, FileCreationMode.Private);
            string encryptedKeyBase64encoded = pref.GetString(ENCRYPTED_KEY_NAME, null);
            if (encryptedKeyBase64encoded == null)
            {
                byte[] key = new byte[16];
                SecureRandom secureRandom = new SecureRandom();
                secureRandom.NextBytes(key);
                byte[] encryptedKey = rsaEncryptKey(key);
                encryptedKeyBase64encoded = Android.Util.Base64.EncodeToString(encryptedKey, Base64Flags.Default);
                ISharedPreferencesEditor edit = pref.Edit();
                edit.PutString(ENCRYPTED_KEY_NAME, encryptedKeyBase64encoded);
                edit.Commit();
            }
        }

        private IKey getSecretKeyAPILessThanM()
        {
            ISharedPreferences pref = Android.App.Application.Context.GetSharedPreferences(SHARED_PREFERENCE_NAME, FileCreationMode.Private);
            string encryptedKeyBase64Encoded = pref.GetString(ENCRYPTED_KEY_NAME, null);
            // need to check null, omitted here
            byte[] encryptedKey = Android.Util.Base64.Decode(encryptedKeyBase64Encoded, Base64Flags.Default);
            byte[] key = rsaDecryptKey(encryptedKey);
            return new SecretKeySpec(key, "AES");
        }


        //@RequiresApi(api = Build.VERSION_CODES.M)
        protected void generateKeysForAPIMOrGreater()
        {
            KeyGenerator keyGenerator;
            keyGenerator = KeyGenerator.GetInstance(KeyProperties.KeyAlgorithmAes, ANDROID_KEY_STORE_NAME);
            keyGenerator.Init(
                new KeyGenParameterSpec.Builder(KEY_ALIAS, KeyStorePurpose.Encrypt | KeyStorePurpose.Decrypt)
                .SetBlockModes(KeyProperties.BlockModeGcm)
                .SetEncryptionPaddings(KeyProperties.EncryptionPaddingNone)
                // NOTE no Random IV. According to above this is less secure by acceptably so.
                .SetRandomizedEncryptionRequired(false)
                .Build());
            // Note according to [docs](https://developer.android.com/reference/android/security/keystore/KeyGenParameterSpec.html)
            // this generation will also add it to the keystore.
            keyGenerator.GenerateKey();
        }

        public string encryptData(string stringDataToEncrypt)
        {

            if (stringDataToEncrypt == null)
            {
                throw new IllegalArgumentException("Data to be decrypted must be non null");
            }

            Cipher cipher;
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                cipher = Cipher.GetInstance(AES_MODE_M_OR_GREATER);
                cipher.Init(CipherMode.EncryptMode, getSecretKeyAPIMorGreater(),
                        new GCMParameterSpec(128, System.Text.Encoding.UTF8.GetBytes(FIXED_IV)));
            }
            else
            {
                cipher = Cipher.GetInstance(AES_MODE_LESS_THAN_M, CIPHER_PROVIDER_NAME_ENCRYPTION_DECRYPTION_AES);
                cipher.Init(CipherMode.EncryptMode, getSecretKeyAPILessThanM());
            }
            byte[] encodedBytes = cipher.DoFinal(System.Text.Encoding.UTF8.GetBytes(stringDataToEncrypt));
            string encryptedBase64Encoded = Android.Util.Base64.EncodeToString(encodedBytes, Base64Flags.Default);
            return encryptedBase64Encoded;
        }

        public string decryptData(string encryptedData)
        {
            if (encryptedData == null)
            {
                throw new IllegalArgumentException("Data to be decrypted must be non null");
            }

            byte[] encryptedDecodedData = Android.Util.Base64.Decode(encryptedData, Base64Flags.Default);

            Cipher c;
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                c = Cipher.GetInstance(AES_MODE_M_OR_GREATER);
                c.Init(CipherMode.DecryptMode, getSecretKeyAPIMorGreater(), new GCMParameterSpec(128, System.Text.Encoding.UTF8.GetBytes(FIXED_IV)));
            }
            else
            {
                c = Cipher.GetInstance(AES_MODE_LESS_THAN_M, CIPHER_PROVIDER_NAME_ENCRYPTION_DECRYPTION_AES);
                c.Init(CipherMode.DecryptMode, getSecretKeyAPILessThanM());
            }
            byte[] decodedBytes = c.DoFinal(encryptedDecodedData);
            return System.Text.Encoding.UTF8.GetString(decodedBytes);
        }

        private IKey getSecretKeyAPIMorGreater()
        {
            KeyStore keyStore = KeyStore.GetInstance(ANDROID_KEY_STORE_NAME);
            keyStore.Load(null);
            return keyStore.GetKey(KEY_ALIAS, null);
        }

        private byte[] rsaEncryptKey(byte[] secret)
        {
            KeyStore keyStore = KeyStore.GetInstance(ANDROID_KEY_STORE_NAME);
            keyStore.Load(null);

            KeyStore.PrivateKeyEntry privateKeyEntry = (KeyStore.PrivateKeyEntry)keyStore.GetEntry(KEY_ALIAS, null);
            Cipher inputCipher = Cipher.GetInstance(RSA_MODE, CIPHER_PROVIDER_NAME_ENCRYPTION_DECRYPTION_RSA);
            inputCipher.Init(CipherMode.EncryptMode, privateKeyEntry.Certificate.PublicKey);

            MemoryStream outputStream = new MemoryStream();
            CipherOutputStream cipherOutputStream = new CipherOutputStream(outputStream, inputCipher);
            cipherOutputStream.Write(secret);
            cipherOutputStream.Close();

            byte[] encryptedKeyAsByteArray = outputStream.ToArray();
            return encryptedKeyAsByteArray;
        }

        private byte[] rsaDecryptKey(byte[] encrypted)
        {

            KeyStore keyStore = KeyStore.GetInstance(ANDROID_KEY_STORE_NAME);
            keyStore.Load(null);

            KeyStore.PrivateKeyEntry privateKeyEntry = (KeyStore.PrivateKeyEntry)keyStore.GetEntry(KEY_ALIAS, null);
            Cipher output = Cipher.GetInstance(RSA_MODE, CIPHER_PROVIDER_NAME_ENCRYPTION_DECRYPTION_RSA);
            output.Init(CipherMode.DecryptMode, privateKeyEntry.PrivateKey);
            CipherInputStream cipherInputStream = new CipherInputStream(
                new MemoryStream(encrypted), output);
            List<byte> values = new List<byte>();
            int nextByte;
            while ((nextByte = cipherInputStream.Read()) != -1)
            {
                values.Add((byte)nextByte);
            }

            byte[] decryptedKeyAsBytes = new byte[values.Count];
            for (int i = 0; i < decryptedKeyAsBytes.Length; i++)
            {
                decryptedKeyAsBytes[i] = values[i];
            }
            return decryptedKeyAsBytes;
        }

        public string Get(string key)
        {
            try
            {
                ISharedPreferences pref = Android.App.Application.Context.GetSharedPreferences(SHARED_PREFERENCE_NAME, FileCreationMode.Private);
                string encrypted = pref.GetString(key, null);
                if (encrypted == null)
                {
                    return null;
                }
                return decryptData(encrypted);
            }
            catch (System.Exception ignored)
            {
                return null;
            }
        }

        public void Remove(string key)
        {
            ISharedPreferences pref = Android.App.Application.Context.GetSharedPreferences(SHARED_PREFERENCE_NAME, FileCreationMode.Private);
            pref.Edit().Remove(key).Commit();
        }

        public void Set(string key, string obj)
        {
            ISharedPreferences pref = Android.App.Application.Context.GetSharedPreferences(SHARED_PREFERENCE_NAME, FileCreationMode.Private);
            pref.Edit().PutString(key, encryptData(obj)).Commit();
        }
    }
}