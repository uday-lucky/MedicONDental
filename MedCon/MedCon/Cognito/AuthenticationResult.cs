using System;
namespace awscognito
{
	public class AuthenticationResult
	{
		public CognitoUserSession CognitoUserSession { get; }
		public CognitoDevice CognitoDevice { get; }
		public ChallengeContinuation ChallengeContinuation { get; }
		public AuthenticationContinuation AuthenticationContinuation { get; }

		public AuthenticationResult(CognitoUserSession cognitoUserSession, CognitoDevice cognitoDevice, ChallengeContinuation challengeContinuation, AuthenticationContinuation authenticationContinuation)
		{
			AuthenticationContinuation = authenticationContinuation;
			ChallengeContinuation = challengeContinuation;
			CognitoDevice = cognitoDevice;
			CognitoUserSession = cognitoUserSession;
		}
	}
}
