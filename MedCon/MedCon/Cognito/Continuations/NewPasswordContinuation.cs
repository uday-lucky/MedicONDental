using System;
using System.Collections.Generic;
using Amazon.CognitoIdentityProvider.Model;
using awscognito.Cognito.Util;
using Newtonsoft.Json;

namespace awscognito
{
    public class NewPasswordContinuation : ChallengeContinuation
    {
        /**
         * Required attributes to complete first sign-in.
         */
        private List<String> requiredAttributes;

        /**
         * The current values of all user attributes.
         */
        private Dictionary<string, string> currentUserAttributes;

        /// <summary>
        /// Constructs a new continuation for new user sign-in.
        /// </summary>
        /// <param name="user">REQUIRED: Reference to the {@link CognitoUser} object.</param>
        /// <param name="challengeRequest">REQUIRED: The response to respond to the authentication challenge.</param>
        public NewPasswordContinuation(CognitoUser user, string username, string secretHash, RespondToAuthChallengeResponse challengeRequest) : base(user, username, secretHash, challengeRequest)
        {
            parseUserAttributes(GetParameters()[CognitoServiceConstants.CHLG_PARAM_USER_ATTRIBUTE]);
            parseRequiredAttributes(GetParameters()[CognitoServiceConstants.CHLG_PARAM_REQUIRED_ATTRIBUTE]);
        }

        /**
         * Returns all required attributes to complete user sign-up. All these user attributes have to be
         * set to complete the user sign-up.
         *
         * @return A {@code List<String>} of all required user attributes.
         */
        public List<string> getRequiredAttributes()
        {
            return requiredAttributes;
        }

        /**
         * Returns all current user attributes. These attributes are set by the Admin when creating a new
         * user.
         *
         * @return A {@code Map<String, String>} containing all current values of user attributes.
         */
        public Dictionary<string, string> getCurrentUserAttributes()
        {
            return currentUserAttributes;
        }

        /**
         * Add a user attribute, will override current value.
         *
         * @param attributeName     REQUIRED: The attribute name.
         * @param attributeValue    REQUIRED: The attribute value.
         */
        public void setUserAttribute(String attributeName, String attributeValue)
        {
            SetChallengeResponse(CognitoServiceConstants.CHLG_PARAM_USER_ATTRIBUTE_PREFIX + attributeName, attributeValue);
        }

        /**
         * Set new user password, must not be {@code null}. This is required to complete the user sign-up.
         *
         * @param userPassword      REQUIRED: New user password.
         */
        public void setPassword(String userPassword)
        {
            if (userPassword != null)
            {
                SetChallengeResponse(CognitoServiceConstants.CHLG_RESP_NEW_PASSWORD, userPassword);
            }
        }

        /**
         * Calls {@Code continueTask()} of the parent after checking if all the required attributes have been set.
         */
        public new System.Threading.Tasks.Task<AuthenticationResult> ContinueTask()
        {
            if (requiredAttributes != null && requiredAttributes.Count > 1)
            {
                foreach (var requiredAttribute in requiredAttributes)
                {
                    String requiredAttrKey = CognitoServiceConstants.CHLG_PARAM_USER_ATTRIBUTE_PREFIX + requiredAttribute;
                    if (!challengeResponses.ContainsKey(requiredAttrKey))
                    {
                        throw new InvalidParameterException($"Missing required attribute: {requiredAttribute}");
                    }
                }
            }

            if (challengeResponses.ContainsKey(CognitoServiceConstants.CHLG_RESP_NEW_PASSWORD) &&
                (challengeResponses[CognitoServiceConstants.CHLG_RESP_NEW_PASSWORD] != null))
            {
                return base.ContinueTask();
            }
            else
            {
                throw new InvalidParameterException("New password was not set");
            }
        }

        /**
         * Parses user attributes.
         *
         * @param userAttributesJsonString      REQUIRED: User attributes as a Json String.
         */
        private void parseUserAttributes(string userAttributesJsonString)
        {
            currentUserAttributes = new Dictionary<string, string>();
            if (userAttributesJsonString != null)
            {
                try
                {
                    dynamic userAttributesJson = JsonConvert.DeserializeObject(userAttributesJsonString);
                    foreach (var pair in userAttributesJson)
                    {
                        currentUserAttributes.Add(pair.Name, pair.Value.ToString());
                    }
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }

        /**
         * Parse required attributes.
         *
         * @param requiredAttributesJsonString      REQUIRED: Required user attributes as a Json array.
         */
        private void parseRequiredAttributes(string requiredAttributesJsonString)
        {
            requiredAttributes = new List<string>();
            if (requiredAttributesJsonString != null)
            {
                try
                {
                    dynamic requiredAttributesJson = JsonConvert.DeserializeObject(requiredAttributesJsonString);
                    
                    foreach (var attribute in requiredAttributesJson)
                    {
                        var s = attribute.ToString();
                        var index = s.IndexOf(CognitoServiceConstants.CHLG_PARAM_USER_ATTRIBUTE_PREFIX, StringComparison.Ordinal);
                        if (index == -1)
                        {
                            requiredAttributes.Add(s);
                        }
                        else
                        {
                            requiredAttributes.Add(s.Substring(index + CognitoServiceConstants.CHLG_PARAM_USER_ATTRIBUTE_PREFIX.Length));
                        }
                    }
                }
                catch (Exception e) {
                    throw;
                }
            }
        }
}
}
