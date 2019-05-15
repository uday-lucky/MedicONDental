using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using awscognito.Cognito.Tokens;
using awscognito.Cognito.Util;
using awscognito.Cognito.Continuations;
using System.Diagnostics;
using MedCon.Services;
using Xamarin.Forms;

namespace awscognito
{
    public class CognitoUser
    {
        private const int SRP_RADIX = 16;

        public string Username { get; private set; }
        public CognitoUserPool Pool { get; private set; }
        public string Session { get; private set; }
        AmazonCognitoIdentityProviderClient client;
        CognitoUserSession cipSession;

        /**
		 * userId for this user, this is mutable to allow the userId to be set
		 * during authentication. This can be the username (users' unique sign-in
		 * username) or an alias (if available, such as email or phone number).
		 */
        private String userId;

        /**
		 * Username used for authentication process. This will be set from the
		 * results in the pre-auth API call.
		 */
        String usernameInternal;

        /**
		 * Secret-Hash for this user-pool, this is mutable because userId is
		 * mutable.
		 */
        String secretHash;

        /**
		 * Device-key of this device, if available.
		 */
        String deviceKey;

        /**
         * Provides cache support
         */
        ISecureStorage storage;

        public CognitoUser(string username, CognitoUserPool pool, ISecureStorage storage)
        {
            if (username == null || pool == null)
            {
                throw new Exception("Username and pool information are required.");
            }

            this.Username = username;
            this.Pool = pool;
            this.Session = null;

            this.client = pool.Client;

            this.cipSession = null;
            this.storage = storage;
        }

        public async Task<AuthenticationResult> respondToChallenge(RespondToAuthChallengeRequest challengeResponse)
        {
            try
            {
                if (challengeResponse != null && challengeResponse.ChallengeResponses != null)
                {
                    var challengeResponses = challengeResponse.ChallengeResponses;
                    challengeResponses[CognitoServiceConstants.CHLG_RESP_DEVICE_KEY] = deviceKey;
                    challengeResponse.ChallengeResponses = challengeResponses;
                }

                var challenge = await client.RespondToAuthChallengeAsync(challengeResponse);
                return HandleChallenge(challenge);
            }
            catch (ResourceNotFoundException rna)
            {
                CognitoUser cognitoUser = this;
                if (rna.Message.Contains("Device"))
                {
                    CognitoDeviceHelper.clearCachedDevice(usernameInternal, Pool.UserPoolId);
                    var authenticationContinuation = new AuthenticationContinuation(cognitoUser);
                    return new AuthenticationResult(null, null, null, authenticationContinuation);
                }
                throw;
            }
        }

        private AuthenticationResult HandleChallenge(RespondToAuthChallengeResponse challenge)
        {
            if (challenge == null)
            {
                throw new InternalErrorException("Authentication failed due to an internal error");
            }

            updateInternalUsername(challenge.ChallengeParameters);
            if (challenge.ChallengeName == null)
            {
                var session = getCognitoUserSession(challenge.AuthenticationResult);
                cacheTokens(session);
                var newDeviceMetadata = challenge.AuthenticationResult.NewDeviceMetadata;
                if (newDeviceMetadata == null)
                {
                    return new AuthenticationResult(session, null, null, null);
                }
                else
                {
                    //Console.WriteLine("ConfirmDeviceResult");
                    return new AuthenticationResult(session, null, null, null);
                    //					final ConfirmDeviceResult confirmDeviceResult = confirmDevice(newDeviceMetadata);
                    //	if (confirmDeviceResult != null
                    //			&& confirmDeviceResult.isUserConfirmationNecessary())
                    //	{
                    //		final CognitoDevice newDevice = new CognitoDevice(
                    //				newDeviceMetadata.getDeviceKey(), null, null, null, null, cognitoUser,
                    //				context);
                    //		nextTask = new Runnable() {
                    //		@Override

                    //		public void run()
                    //		{
                    //			callback.onSuccess(cognitoUserSession, newDevice);
                    //		}
                    //	};
                    //} else {
                    //	nextTask = new Runnable() {
                    //		@Override

                    //		public void run()
                    //	{
                    //		callback.onSuccess(cognitoUserSession, null);
                    //	}
                    //};
                    //}
                }
            }
            else if (ChallengeNameType.PASSWORD_VERIFIER == challenge.ChallengeName)
            {
                // 
                return new AuthenticationResult(null, null, null, null);
            }
            else if (ChallengeNameType.SMS_MFA == challenge.ChallengeName)
            {
                throw new NotImplementedException(challenge.ChallengeName);
            }
            else if (ChallengeNameType.DEVICE_SRP_AUTH == challenge.ChallengeName)
            {
                throw new NotImplementedException(challenge.ChallengeName);
            }
            else if (ChallengeNameType.NEW_PASSWORD_REQUIRED == challenge.ChallengeName)
            {
                var continuation = new NewPasswordContinuation(this, usernameInternal, secretHash, challenge);
                return new AuthenticationResult(null, null, continuation, null);
            }
            else
            {
                var continuation = new ChallengeContinuation(this, usernameInternal, secretHash, challenge);
                return new AuthenticationResult(null, null, continuation, null);
            }
        }

        private AuthenticationResult HandleChallenge(InitiateAuthResponse authResult)
        {
            var challenge = new RespondToAuthChallengeResponse
            {
                ChallengeName = authResult.ChallengeName,
                Session = authResult.Session,
                AuthenticationResult = authResult.AuthenticationResult,
                ChallengeParameters = authResult.ChallengeParameters
            };
            return HandleChallenge(challenge);
        }

        private InitiateAuthRequest InitiateUserSrpAuthRequest(AuthenticationDetails authenticationDetails, AuthenticationHelper authenticationHelper)
        {
            userId = authenticationDetails.getUserId();

            var request = new InitiateAuthRequest
            {
                ClientId = Pool.ClientId,
                AuthFlow = AuthFlowType.USER_SRP_AUTH,
                AuthParameters = new Dictionary<string, string>() {
                    { "USERNAME", authenticationDetails.getUserId() },
                    { "SRP_A", authenticationHelper.A.ToString(SRP_RADIX) },
                    { CognitoServiceConstants.AUTH_PARAM_SECRET_HASH, CognitoSecretHash.getSecretHash(userId, Pool.ClientId, Pool.ClientSecret) }
                }
            };

            if (deviceKey == null)
            {
                request.AuthParameters.Add(CognitoServiceConstants.AUTH_PARAM_DEVICE_KEY,
                    CognitoDeviceHelper.getDeviceKey(authenticationDetails.getUserId(), Pool.UserPoolId));
            }
            else
            {
                request.AuthParameters.Add(CognitoServiceConstants.AUTH_PARAM_DEVICE_KEY, deviceKey);
            }

            if (authenticationDetails.getValidationData() != null && authenticationDetails.getValidationData().Count > 0)
            {
                var userValidationData = new Dictionary<string, string>();
                foreach (var item in authenticationDetails.getValidationData())
                {
                    userValidationData.Add(item.Name, item.Value);
                }
                request.ClientMetadata = userValidationData;
            }

            return request;
        }

        protected async Task<CognitoUserSession> getCachedSession()
        {
            if (Username == null)
            {
                throw new NotAuthorizedException("Username is null");
            }

            if (cipSession != null)
            {
                if (cipSession.isValidForThreshold())
                {
                    return cipSession;
                }
            }

            CognitoUserSession cachedTokens = readCachedTokens();
            if (cachedTokens.isValidForThreshold())
            {
                cipSession = cachedTokens;
                return cipSession;
            }

            if (cachedTokens.RefreshToken != null)
            {
                try
                {
                    cipSession = await refreshSession(cachedTokens); //.WithNetworkIndicator();
                    cacheTokens(cipSession);
                    return cipSession;
                }
                catch (NotAuthorizedException nae)
                {
                    clearCachedTokens();
                    throw new NotAuthorizedException("User is not authenticated", nae);
                }
                catch (Exception e)
                {
                    throw new InternalErrorException("Failed to authenticate user", e);
                }
            }
            throw new NotAuthorizedException("User is not authenticated");
        }


        /**
         * Creates a user session with the tokens from authentication and overrider
         * the refresh token with the value passed.
         *
         * @param authResult REQUIRED: Authentication result which contains the
         *            tokens.
         * @param refreshTokenOverride REQUIRED: This will be used to create a new
         *            session object if it is not null.
         * @return {@link CognitoUserSession} with the latest tokens.
         */
        private CognitoUserSession getCognitoUserSession(AuthenticationResultType authResult,
                CognitoRefreshToken refreshTokenOverride = null)
        {
            String idtoken = authResult.IdToken;
            CognitoIdToken idToken = new CognitoIdToken(idtoken);

            String acctoken = authResult.AccessToken;
            CognitoAccessToken accessToken = new CognitoAccessToken(acctoken);

            CognitoRefreshToken refreshToken;

            if (refreshTokenOverride != null)
            {
                refreshToken = refreshTokenOverride;
            }
            else
            {
                String reftoken = authResult.RefreshToken;
                refreshToken = new CognitoRefreshToken(reftoken);
            }
            return new CognitoUserSession(idToken, accessToken, refreshToken);
        }

        private async Task<CognitoUserSession> refreshSession(CognitoUserSession currSession)
        {
            CognitoUserSession cognitoUserSession = null;

            InitiateAuthRequest initiateAuthRequest = initiateRefreshTokenAuthRequest(currSession);
            var refreshSessionResult = await client.InitiateAuthAsync(initiateAuthRequest);
            if (refreshSessionResult.AuthenticationResult == null)
            {
                throw new NotAuthorizedException("User is not authenticated");
            }

            cognitoUserSession = getCognitoUserSession(refreshSessionResult.AuthenticationResult, currSession.RefreshToken);

            return cognitoUserSession;
        }

        InitiateAuthRequest initiateRefreshTokenAuthRequest(CognitoUserSession currSession)
        {
            var initiateAuthRequest = new InitiateAuthRequest
            {
                ClientId = Pool.ClientId,
                AuthFlow = AuthFlowType.REFRESH_TOKEN_AUTH,
                AuthParameters = new Dictionary<string, String>()
                {
                    { AuthFlowType.REFRESH_TOKEN, currSession.RefreshToken.Token },

                }
            };

            if (deviceKey == null)
            {
                if (usernameInternal != null)
                {
                    deviceKey = CognitoDeviceHelper.getDeviceKey(usernameInternal, Pool.UserPoolId);
                }
                else
                {
                    deviceKey = CognitoDeviceHelper.getDeviceKey(Username, Pool.UserPoolId);
                }
            }

            initiateAuthRequest.AuthParameters.Add(CognitoServiceConstants.AUTH_PARAM_DEVICE_KEY, deviceKey);
            initiateAuthRequest.AuthParameters.Add(CognitoServiceConstants.AUTH_PARAM_SECRET_HASH, Pool.ClientSecret);

            return initiateAuthRequest;
        }

        private void cacheTokens(CognitoUserSession session)
        {
            if (storage != null)
            {
				try
				{
					string csiIdTokenKey = string.Format("CognitoIdentityProvider.{0}.{1}.idToken", Pool.ClientId, Username);
					string csiAccessTokenKey = string.Format("CognitoIdentityProvider.{0}.{1}.accessToken", Pool.ClientId, Username);
					string csiRefreshTokenKey = string.Format("CognitoIdentityProvider.{0}.{1}.refreshToken", Pool.ClientId, Username);
					string csiLastUserKey = string.Format("CognitoIdentityProvider.{0}.LastAuthUser", Pool.ClientId);

					storage.Set(csiIdTokenKey, session.IdToken.Token);
                    storage.Set(csiAccessTokenKey, session.AccessToken.Token);
					storage.Set(csiRefreshTokenKey, session.RefreshToken.Token);
					storage.Set(csiLastUserKey, Username);
				}
				catch (Exception e)
				{
					// Logging exception, this is not a fatal error
					Debug.WriteLine("Error while writing to Keychain", e);
				}
            }
        }

        private void clearCachedTokens()
        {
			if (storage != null)
			{
				try
				{
					string csiIdTokenKey = string.Format("CognitoIdentityProvider.{0}.{1}.idToken", Pool.ClientId, Username);
					string csiAccessTokenKey = string.Format("CognitoIdentityProvider.{0}.{1}.accessToken", Pool.ClientId, Username);
					string csiRefreshTokenKey = string.Format("CognitoIdentityProvider.{0}.{1}.refreshToken", Pool.ClientId, Username);
                    string csiLastUserKey = string.Format("CognitoIdentityProvider.{0}.LastAuthUser", Pool.ClientId);

					storage.Remove(csiIdTokenKey);
					storage.Remove(csiAccessTokenKey);
					storage.Remove(csiRefreshTokenKey);
					storage.Remove(csiLastUserKey);
				}
				catch (Exception e)
				{
					// Logging exception, this is not a fatal error
					Debug.WriteLine("Error while writing to Keychain", e);
				}
			}
        }

        private CognitoUserSession readCachedTokens()
        {
            CognitoUserSession userSession = new CognitoUserSession(null, null, null);

            if (storage != null)
            {
                try
                {
                    string csiIdTokenKey = string.Format("CognitoIdentityProvider.{0}.{1}.idToken", Pool.ClientId, Username);
                    string csiAccessTokenKey = string.Format("CognitoIdentityProvider.{0}.{1}.accessToken", Pool.ClientId, Username);
                    string csiRefreshTokenKey = string.Format("CognitoIdentityProvider.{0}.{1}.refreshToken", Pool.ClientId, Username);
                    string csiLastUserKey = string.Format("CognitoIdentityProvider.{0}.LastAuthUser", Pool.ClientId);

					CognitoIdToken csiCachedIdToken = new CognitoIdToken(storage.Get(csiIdTokenKey));
					CognitoAccessToken csiCachedAccessToken = new CognitoAccessToken(storage.Get(csiAccessTokenKey));
					CognitoRefreshToken csiCachedRefreshToken = new CognitoRefreshToken(storage.Get(csiRefreshTokenKey));

                    userSession = new CognitoUserSession(csiCachedIdToken, csiCachedAccessToken, csiCachedRefreshToken);
                }
                catch (Exception e)
                {
                    // Logging exception, this is not a fatal error
                    Debug.WriteLine("Error while writing to Keychain", e);
				}
			}

			return userSession;
        }

        public async Task<CognitoUserSession> getSession()
        {
            try
            {
                await getCachedSession();
                return cipSession;
            }
            catch (InvalidParameterException e)
            {
                throw;
            }
            catch (NotAuthorizedException e)
            {
                throw;
            }
            catch (Exception e)
            {
                throw;
            }

            return null;
        }

        public Task<AuthenticationResult> initiateUserAuthentication(AuthenticationDetails authenticationDetails)
        {
          
            if(authenticationDetails.getAuthenticationType() == CognitoServiceConstants.CHLG_TYPE_USER_PASSWORD_VERIFIER) {
                return startWithUserSrpAuthAsync(authenticationDetails);
            } else if (authenticationDetails.getAuthenticationType() == CognitoServiceConstants.CHLG_TYPE_CUSTOM_CHALLENGE) {
                throw new NotImplementedException();
            } else {
				throw new InvalidParameterException("Unsupported authentication type " + authenticationDetails.getAuthenticationType());
            }
        }

        private async Task<AuthenticationResult> startWithUserSrpAuthAsync(AuthenticationDetails authenticationDetails)
        {            
            var authenticationHelper = new AuthenticationHelper(Pool.UserPoolId);
            var initiateAuthRequest = InitiateUserSrpAuthRequest(authenticationDetails, authenticationHelper);

            try
            {
                var initiateAuthResult = await client.InitiateAuthAsync(initiateAuthRequest);
                updateInternalUsername(initiateAuthResult.ChallengeParameters);
                if (ChallengeNameType.PASSWORD_VERIFIER == initiateAuthResult.ChallengeName)
                {
                    if (authenticationDetails.getPassword() != null)
                    {
                        RespondToAuthChallengeRequest challengeRequest = userSrpAuthRequest(initiateAuthResult, authenticationDetails, authenticationHelper);
                        return await respondToChallenge(challengeRequest);
                    }
                }
                return HandleChallenge(initiateAuthResult);
            } catch (ResourceNotFoundException rna) {
                CognitoUser cognitoUser = this;
                if (rna.Message.Contains("Device"))
                {
                    CognitoDeviceHelper.clearCachedDevice(usernameInternal, Pool.UserPoolId);
                    return new AuthenticationResult(null, null, null, new AuthenticationContinuation(cognitoUser));
                }
            }

            return new AuthenticationResult(null, null, null, null);
        }

        private RespondToAuthChallengeRequest userSrpAuthRequest(InitiateAuthResponse challenge, AuthenticationDetails authenticationDetails, AuthenticationHelper authenticationHelper)
        {
            this.usernameInternal = challenge.ChallengeParameters[CognitoServiceConstants.CHLG_PARAM_USERNAME];
            this.deviceKey = CognitoDeviceHelper.getDeviceKey(usernameInternal, Pool.UserPoolId);
            secretHash = CognitoSecretHash.getSecretHash(usernameInternal, Pool.ClientId, Pool.ClientSecret);

            DateTime timestamp = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Utc);
            //The timestamp format returned to AWS _needs_ to be in US Culture
            CultureInfo usCulture = new CultureInfo("en-US");
            String timeStr = timestamp.ToString("ddd MMM d HH:mm:ss \"UTC\" yyyy", usCulture);

            //Do the hard work to generate the claim we return to AWS
            byte[] claim = authenticationHelper.authenticateUser(
                challenge.ChallengeParameters["USERNAME"],
                authenticationDetails.getPassword(),
                challenge.ChallengeParameters["SALT"],
                challenge.ChallengeParameters["SRP_B"],
                challenge.ChallengeParameters["SECRET_BLOCK"],
                timeStr
            );

            String claimBase64 = System.Convert.ToBase64String(claim);

            //Our response to AWS. If successful it will return an object with Tokens,
            //if unsuccessful, it will throw an Exception that you should catch and handle.

            var ChallengeResponses = new Dictionary<string, string>() {
                { "PASSWORD_CLAIM_SECRET_BLOCK", challenge.ChallengeParameters["SECRET_BLOCK"] },
                { "PASSWORD_CLAIM_SIGNATURE", claimBase64 },
                { "USERNAME", usernameInternal },
                { "TIMESTAMP", timeStr },
                { CognitoServiceConstants.CHLG_RESP_DEVICE_KEY, deviceKey },
                { CognitoServiceConstants.CHLG_RESP_SECRET_HASH, secretHash }
            };
            
            return new RespondToAuthChallengeRequest
            {
                ChallengeName = "PASSWORD_VERIFIER",
                ClientId = Pool.ClientId,
                ChallengeResponses = ChallengeResponses,
                Session = Session
            };
        }

        private void updateInternalUsername(Dictionary<string, string> challengeParameters)
        {
			if (usernameInternal == null)
			{
				if (challengeParameters != null && challengeParameters
                    .ContainsKey(CognitoServiceConstants.CHLG_PARAM_USERNAME))
				{
					usernameInternal = challengeParameters[CognitoServiceConstants.CHLG_PARAM_USERNAME];
                    deviceKey = CognitoDeviceHelper.getDeviceKey(usernameInternal, Pool.UserPoolId);
					if (secretHash == null)
					{
                        secretHash = CognitoSecretHash.getSecretHash(usernameInternal, Pool.ClientId, Pool.ClientSecret);
					}
				}
			}
        }

        public void SignOut() {
			cipSession = null;
			clearCachedTokens();
        }
    }
}