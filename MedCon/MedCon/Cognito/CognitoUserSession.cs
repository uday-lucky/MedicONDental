using System;
using awscognito.Cognito.Tokens;

namespace awscognito
{
    public class CognitoUserSession
    {
        private static readonly long REFRESH_THRESHOLD_DEFAULT = 300 * 1000;

        public CognitoIdToken IdToken { get; private set; }
        public CognitoAccessToken AccessToken { get; private set; }
        public CognitoRefreshToken RefreshToken { get; private set; }

        public CognitoUserSession(Amazon.CognitoIdentityProvider.Model.AuthenticationResultType result) : this(
            new CognitoIdToken(result.IdToken),
            new CognitoAccessToken(result.AccessToken),
            new CognitoRefreshToken(result.RefreshToken)
        )
        {
        }

        public CognitoUserSession(CognitoIdToken idToken, CognitoAccessToken accessToken, CognitoRefreshToken refreshToken)
        {
            RefreshToken = refreshToken;
            AccessToken = accessToken;
            IdToken = idToken;
        }

        public bool isValid()
        {
            DateTime currentTimeStamp = Amazon.Util.AWSSDKUtils.CorrectedUtcNow;

            try
            {
                return DateTime.Compare(currentTimeStamp, IdToken.getExpiration().Value) < 0
                               && DateTime.Compare(currentTimeStamp, AccessToken.getExpiration().Value) < 0;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /**
         * Returns true if this session for the threshold set in {@link CognitoIdentityProviderClientConfig#refreshThreshold}.
         *
         * @return boolean to indicate if the session is valid for atleast {@link CognitoIdentityProviderClientConfig#refreshThreshold} seconds.
         */
        public bool isValidForThreshold()
        {
            try
            {
                var Jan1st1970 = new DateTime
                    (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                var now = (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
                var expiration = (long)(IdToken.getExpiration().Value.ToUniversalTime() - Jan1st1970).TotalMilliseconds;
                var expiresInMilliseconds = expiration - now;
                return expiresInMilliseconds > REFRESH_THRESHOLD_DEFAULT;

                //var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                //var currentTime = (DateTime.Now.ToUniversalTime() - epoch).TotalSeconds;
                //var expiration = (IdToken.getExpiration().Value.ToUniversalTime() - epoch).TotalSeconds;
                //var expiresInMilliSeconds = expiration - currentTime;
                //return (expiresInMilliSeconds > REFRESH_THRESHOLD_DEFAULT);
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
