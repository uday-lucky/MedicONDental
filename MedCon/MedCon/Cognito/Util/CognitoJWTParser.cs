using System;
using System.Text;
using Amazon.CognitoIdentity.Model;

namespace awscognito.Cognito.Util
{
	public static class CognitoJWTParser
	{
		private static readonly int HEADER = 0;
		private static readonly int PAYLOAD = 1;
		private static readonly int SIGNATURE = 2;
		private static readonly int JWT_PARTS = 3;

		/**
		* Returns header for a JWT as a JSON object.
		*
		* @param jwt       REQUIRED: valid JSON Web Token as String.
		* @return header as a JSONObject
		*/
		public static dynamic getHeader(string jwt)
		{
			try
			{
				validateJWT(jwt);
				byte[] sectionDecoded = Base64Url.Base64UrlDecode(jwt.Split('.')[HEADER]);
				string jwtSection = Encoding.UTF8.GetString(sectionDecoded, 0, sectionDecoded.Length);
				return Newtonsoft.Json.JsonConvert.DeserializeObject(jwtSection);
			}
			catch (Exception e)
			{
				throw new InvalidParameterException("error in parsing JSON");
			}
		}

		/**
		 * Returns payload of a JWT as a JSON object.
		 *
		 * @param jwt       REQUIRED: valid JSON Web Token as String.
		 * @return payload as a JSONObject.
		 */
		public static dynamic getPayload(String jwt)
		{
			try
			{
				validateJWT(jwt);
				byte[] sectionDecoded = Base64Url.Base64UrlDecode(jwt.Split('.')[PAYLOAD]);
				string jwtSection = Encoding.UTF8.GetString(sectionDecoded, 0, sectionDecoded.Length);
                return Newtonsoft.Json.JsonConvert.DeserializeObject(jwtSection);

			}
			catch (Exception e)
			{
				throw new InvalidParameterException("error in parsing JSON");
			}
		}

		/**
		 * Returns signature of a JWT as a String.
		 *
		 * @param jwt       REQUIRED: valid JSON Web Token as String.
		 * @return signature as a String.
		 */
		public static string getSignature(String jwt)
		{
			try
			{
				validateJWT(jwt);
				byte[] sectionDecoded = Base64Url.Base64UrlDecode(jwt.Split('.')[SIGNATURE]);
				string jwtSection = Encoding.UTF8.GetString(sectionDecoded, 0, sectionDecoded.Length);
                return jwtSection;
			}
			catch (Exception e)
			{
				throw new InvalidParameterException("error in parsing JSON");
			}
		}

		/**
		 * Returns a claim, from the {@code JWT}s' payload, as a String.
		 *
		 * @param jwt       REQUIRED: valid JSON Web Token as String.
		 * @param claim     REQUIRED: claim name as String.
		 * @return  claim from the JWT as a String.
		 */
		public static string getClaim(String jwt, String claim)
		{
			try
			{
				dynamic payload = getPayload(jwt);
				object claimValue = payload[claim];

				if (claimValue != null)
				{
					return claimValue.ToString();
				}

			}
			catch (Exception e)
			{
				throw new InvalidParameterException("invalid token");
			}
			return null;
		}

		/**
		* Checks if {@code JWT} is a valid JSON Web Token.
		*
		* @param jwt REQUIRED: The JWT as a {@link String}.
		*/
		public static void validateJWT(String jwt)
		{
			// Check if the the JWT has the three parts
			string[] jwtParts = jwt.Split('.');
			if (jwtParts.Length != JWT_PARTS)
			{
				throw new InvalidParameterException("not a JSON Web Token");
			}
		}
	}
}