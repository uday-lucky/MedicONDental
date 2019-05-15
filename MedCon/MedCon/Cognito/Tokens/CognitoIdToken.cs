using System;
using Amazon.CognitoIdentity.Model;
using awscognito.Cognito.Util;

namespace awscognito.Cognito.Tokens
{
	public class CognitoIdToken : CognitoUserToken
	{
		public CognitoIdToken(string jwtToken) : base(jwtToken)
		{
		}

		/**
		* Returns expiration of this id token.
		*
		* @return id token expiration claim as {@link java.util.Date} in UTC
		*/
		public DateTime? getExpiration()
		{
			try
			{
				string claim = CognitoJWTParser.getClaim(Token, "exp");
				if (claim == null)
				{
					return null;
				}
				long epocTimeSec = long.Parse(claim);
				var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
				return epoch.AddSeconds(epocTimeSec);
			}
			catch (Exception e)
			{
				throw new InternalErrorException(e.Message, e);
			}
		}

		/**
		* Returns "not before" claim of this id token
		*
		* @return not before claim as {@link java.util.Date} in UTC.
		*/
		public DateTime? getNotBefore()
		{
			try
			{
				string claim = CognitoJWTParser.getClaim(Token, "nbf");
				if (claim == null)
				{
					return null;
				}
				long epocTimeSec = long.Parse(claim);
				var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
				return epoch.AddSeconds(epocTimeSec);
			}
			catch (Exception e)
			{
				throw new InternalErrorException(e.Message, e);
			}
		}

		/**
		 * Returns "issued at" claim of this id token
		 *
		 * @return issue at claim as {@link java.util.Date} in UTC.
		 */
		public DateTime? getIssuedAt()
		{
			try
			{
				string claim = CognitoJWTParser.getClaim(Token, "iat");
				if (claim == null)
				{
					return null;
				}
				long epocTimeSec = long.Parse(claim);
				var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
				return epoch.AddSeconds(epocTimeSec);
			}
			catch (Exception e)
			{
				throw new InternalErrorException(e.Message, e);
			}
		}
	}
}