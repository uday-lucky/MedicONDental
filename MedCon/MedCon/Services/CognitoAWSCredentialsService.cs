using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Amazon.CognitoIdentity;
using Amazon.CognitoSync.SyncManager;
using awscognito;

namespace MedCon.Services
{
	class CognitoAWSCredentialsService : ICognitoAWSCredentialsService
	{
		CognitoUserPool _pool;
		CognitoAWSCredentials _credentials;
		CognitoSyncManager _syncManager;

		public CognitoAWSCredentialsService(ISecureStorage secureStorage)
		{
			_pool = new CognitoUserPool(Constants.CognitoUserPoolId, Constants.CognitoClientId, secureStorage);
			_credentials = new CognitoAWSCredentials(Constants.IdentityPoolId, _pool.Region);
			//_syncManager = new CognitoSyncManager(_credentials, _pool.Region);
		}

		public void Clear()
		{
			_credentials.Clear();
		}

		public async Task<CognitoAWSCredentials> GetCredentials()
		{
			var user = Pool.GetCurrentUser();
			if (user != null)
			{
				try
				{
					var session = await user.getSession();
					if (session != null)
					{
						_credentials.AddLogin("cognito-idp." + Pool.Region.SystemName + ".amazonaws.com/" + Pool.UserPoolId,
							session.IdToken.Token);
					}
				}
				catch (Exception e)
				{
					// TODO: This should be handled better, need to figure out if it's recoverable
					Debug.WriteLine("Got exception: {0}", e.Message);
				}
			}

			return _credentials;
		}

		public async Task<CognitoSyncManager> CognitoSyncManager() {
			await GetCredentials();
			return _syncManager;
		}

		public CognitoUserPool Pool
        {
            get => _pool;
        }

		public async Task<string> GetToken()
        {
            var user = Pool.GetCurrentUser();
            if (user == null)
            {
                return null;
            }
            try
            {
                var session = await user.getSession();
                return session?.IdToken.Token;
            } catch (Exception e) {
                return null;
            }
        }
    }
}
