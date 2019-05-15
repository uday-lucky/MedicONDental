using Amazon.CognitoIdentity;
using System.Net.Http;
using Amazon;
using Newtonsoft.Json.Linq;

namespace MedCon.Cognito
{
    /// <summary>
    /// Your custom BYOI Credentials
    /// </summary>
    public class CognitoCustomAWSCredentials : CognitoAWSCredentials
    {
        private const string URL = "https://dev-api.med-conllc.com";
        private const string PROVIDER_NAME = "";
        private const string IDENTITY_POOL_ID = Constants.CognitoUserPoolId;
        private static RegionEndpoint CognitoRegion = RegionEndpoint.USEast1;
        private string Username;

        public CognitoCustomAWSCredentials(string username)
            : base(IDENTITY_POOL_ID, CognitoRegion)
        {
            this.Username = username;
        }

        public override async System.Threading.Tasks.Task<CognitoAWSCredentials.IdentityState> RefreshIdentityAsync()
        {
            var client = new HttpClient();
            var response = await client.GetAsync(string.Format(URL, this.Username));
            var content = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(content);

            //The backend has to send us back an Identity and a OpenID token
            string identityId = json["IdentityId"].ToString();
            string token = json["Token"].ToString();

            var idState = new IdentityState(identityId, PROVIDER_NAME, token, false);

            response.Dispose();
            client.Dispose();


            return idState;
        }
    }
}