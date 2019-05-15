using System;
using Amazon.CognitoIdentity;
using Amazon.CognitoSync;
using Amazon.CognitoSync.SyncManager;

namespace MedCon.Cognito
{
   public class CognitoUtils
    {
        private static CognitoAWSCredentials _credentials;

        public static CognitoAWSCredentials Credentials
        {
            get
            {
                if (_credentials == null)
                    _credentials = new CognitoAWSCredentials(Constants.IdentityPoolId, Constants.CognitoIdentityRegion);

                return _credentials;
            }
        }

        private static CognitoSyncManager _syncManager;
        public static CognitoSyncManager SyncManagerInstance
        {
            get
            {
                if (_syncManager == null)
                    _syncManager = new CognitoSyncManager(Credentials, new AmazonCognitoSyncConfig { RegionEndpoint = Constants.CognitoSyncRegion });

                return _syncManager;
            }
        }
    }
}