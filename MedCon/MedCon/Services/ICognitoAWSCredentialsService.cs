using System.Threading;
using System.Threading.Tasks;
using Amazon.CognitoSync.SyncManager;
using awscognito;

namespace MedCon.Services
{
    public interface ICognitoAWSCredentialsService
    {
        void Clear();
        Task<Amazon.CognitoIdentity.CognitoAWSCredentials> GetCredentials();
        CognitoUserPool Pool { get; }

        Task<string> GetToken();
		Task<CognitoSyncManager> CognitoSyncManager();
    }
}
