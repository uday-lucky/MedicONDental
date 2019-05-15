using System;
using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.Runtime;
using MedCon.Services;

namespace awscognito
{
    public class CognitoUserPool
    {
        public string UserPoolId { get; private set; }
        public string ClientId { get; private set; }
        public string ClientSecret { get; private set; }
        public RegionEndpoint Region { get; private set; }
        public AmazonCognitoIdentityProviderClient Client { get; private set; }
        ISecureStorage storage;

        public CognitoUserPool(string userPoolId, string clientId, ISecureStorage storage = null)
        {
            if (userPoolId == null)
                throw new ArgumentNullException(nameof(userPoolId));

            if (clientId == null)
                throw new ArgumentNullException(nameof(clientId));

            if (!new System.Text.RegularExpressions.Regex("^[\\w-]+_.+$").IsMatch(userPoolId))
            {
                throw new Exception("Invalid UserPoolId format.");
            }

            this.ClientId = clientId;
            this.UserPoolId = userPoolId;
            this.Region = RegionEndpoint.GetBySystemName(userPoolId.Split('_')[0]);

            this.Client = new AmazonCognitoIdentityProviderClient(new AnonymousAWSCredentials(), Region);
            this.storage = storage;
        }

        public CognitoUser GetCurrentUser()
        {
            if (storage == null) {
                return null;
            }

            string lastUserKey = string.Format("CognitoIdentityProvider.{0}.LastAuthUser", ClientId);

            var username = storage.Get(lastUserKey);
			if (username != null)
			{
                var user = new CognitoUser(username, this, storage);
                return user;
            }

            return null;
        }
    }
}