namespace awscognito.Cognito.Tokens
{
	public abstract class CognitoUserToken
	{
		// A Cognito Token - can be an Access, Id or Refresh token
		public string Token { get; private set;}

		public CognitoUserToken(string token)
		{
            this.Token = token;
		}
	}
}
