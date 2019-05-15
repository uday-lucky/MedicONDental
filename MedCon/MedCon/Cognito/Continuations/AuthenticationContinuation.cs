using System;
using System.Threading.Tasks;
using awscognito.Cognito.Continuations;

namespace awscognito
{
    public class AuthenticationContinuation : CognitoIdentityProviderContinuation<string>
    {
        public CognitoUser User { get; }
        public AuthenticationDetails AuthenticationDetails { get; set; }

        public AuthenticationContinuation(CognitoUser user)
        {
            User = user;
        }

        public Task<AuthenticationResult> ContinueTask()
        {
            return User.initiateUserAuthentication(AuthenticationDetails);
        }

        public string GetParameters()
        {
            return "AuthenticationDetails";
        }
    }
}
