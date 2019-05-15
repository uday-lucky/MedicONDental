using System;
using System.Collections.Generic;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using awscognito.Cognito.Util;

namespace awscognito
{
    public class ChallengeContinuation : CognitoIdentityProviderContinuation<Dictionary<string, string>>
    {
        private readonly string _username;
        private readonly string _secretHash;

        public CognitoUser User { get; }
        public RespondToAuthChallengeResponse ChallengeRequest { get; }
        protected Dictionary<string, string> challengeResponses;

        public ChallengeContinuation(CognitoUser user, string username, string secretHash, RespondToAuthChallengeResponse challengeRequest)
        {
            _username = username;
            _secretHash = secretHash;
            ChallengeRequest = challengeRequest;
            User = user;
            challengeResponses = new Dictionary<string, string>();
        }

        public System.Threading.Tasks.Task<AuthenticationResult> ContinueTask()
        {
            challengeResponses[CognitoServiceConstants.CHLG_RESP_USERNAME] = _username;
            challengeResponses[CognitoServiceConstants.CHLG_RESP_SECRET_HASH] = _secretHash;

            var request = new RespondToAuthChallengeRequest
            {
                ChallengeName = GetChallengeName().Value,
                ClientId = User.Pool.ClientId,
                ChallengeResponses = challengeResponses,
                Session = ChallengeRequest.Session
            };

            return User.respondToChallenge(request);
        }

        public Dictionary<string, string> GetParameters()
        {
            return ChallengeRequest.ChallengeParameters;
        }

        public ChallengeNameType GetChallengeName()
        {
            return ChallengeRequest.ChallengeName;
        }

        public void SetChallengeResponse(string responseKey, string responseValue)
        {
            challengeResponses[responseKey] = responseValue;
        }
    }
}
