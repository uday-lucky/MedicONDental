namespace awscognito.Cognito.Tokens
{
	public class CognitoRefreshToken : CognitoUserToken
	{
		public CognitoRefreshToken(string jwtToken) : base(jwtToken)
		{
		}
	}
}