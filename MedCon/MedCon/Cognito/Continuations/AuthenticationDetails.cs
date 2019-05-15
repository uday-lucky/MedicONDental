using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using System;
using System.Collections.Generic;

namespace awscognito.Cognito.Continuations
{
    public class AuthenticationDetails
    {
        private String authenticationType;
        private String userId;
        private String password;
        private List<AttributeType> validationData;
        private Dictionary<String, String> authenticationParameters;

        /**
         * Constructs a new object with authentication details.
         *
         * @param userId            REQUIRED: User ID, NOTE: This will over ride the current Used ID.
         * @param password          REQUIRED: Users' password.
         * @param validationData    REQUIRED: Validation data parameters for the pre-auth lambda.
         */
        public AuthenticationDetails(String userId, String password,
                Dictionary<String, String> validationData)
        {
            this.authenticationType = ChallengeNameType.PASSWORD_VERIFIER;
            this.userId = userId;
            this.password = password;
            setValidationData(validationData);
        }

        /**
         * Constructs a new object for custom authentication.
         *
         * @param userId REQUIRED: User ID, NOTE: This will over ride the current
         *            Used ID.
         * @param authenticationParameters REQUIRED: Authentication details to
         *            launch custom authentication process.
         * @param validationData REQUIRED: Contains authentication parameters 
         *            which are passed to triggered pre-auth lambda. trigger 
         */
        public AuthenticationDetails(String userId, Dictionary<String, String> authenticationParameters,
                Dictionary<String, String> validationData)
        {
            this.userId = userId;
            if (authenticationParameters != null)
            {
                this.authenticationType = ChallengeNameType.CUSTOM_CHALLENGE;
                this.authenticationParameters = authenticationParameters;
                setValidationData(validationData);
            }
            else
            {
                this.authenticationType = null;
            }
        }

        /**
         * Set the type of authentication to be used in this instance.
         *  
         * @param authenticationType REQUIRED: The authentication type indicator. 
         */
        public void setAuthenticationType(String authenticationType)
        {
            this.authenticationType = authenticationType;
            if (ChallengeNameType.PASSWORD_VERIFIER
                    .Equals(this.authenticationType))
            {
                this.authenticationParameters = null;
            }
            else if (ChallengeNameType.CUSTOM_CHALLENGE
                  .Equals(this.authenticationType))
            {
                this.password = null;
            }
        }

        /**
         * This method returns the password.
         *
         * @return password set.
         */
        public String getPassword()
        {
            return password;
        }

        /**
         * This method returns the User Id.
         *
         * @return userId set in this object.
         */
        public String getUserId()
        {
            return userId;
        }

        /**
         * This method returns the validation data.
         *
         * @return validation data set in this object.
         */
        public List<AttributeType> getValidationData()
        {
            return validationData;
        }

        /**
         * This method returns the authentication type.
         *
         * @return the authentication type set for this object.
         */
        public String getAuthenticationType()
        {
            return authenticationType;
        }

        /**
         * The authentication parameters set for custom authentication process.
         *
         * @return Authentication details as a Map.
         */
        public Dictionary<String, String> getAuthenticationParameters()
        {
            return authenticationParameters;
        }

        /**
         * Set the name of the custom challenge. This will override the existing authentication name.
         *
         * @param customChallenge           REQUIRED: Custom challenge name.
         */
        public void setCustomChallenge(String customChallenge)
        {
            if (ChallengeNameType.PASSWORD_VERIFIER.Equals(this.authenticationType))
            {
                throw new InvalidParameterException(
                        String.Format("Cannot set custom challenge when the authentication type is {0}.",
                                ChallengeNameType.PASSWORD_VERIFIER));
            }
            this.authenticationType = ChallengeNameType.CUSTOM_CHALLENGE;
            setAuthenticationParameter("CHALLENGE_NAME",
                    customChallenge);
        }

        /**
         * Set the name of the authentication challenge.
         *
         * @param validationData
         */
        private void setValidationData(Dictionary<String, String> validationData)
        {
            if (validationData != null)
            {
                this.validationData = new List<AttributeType>();
                                   
                foreach (var data in validationData)
                {
                    AttributeType validation = new AttributeType();
                    validation.Name = data.Key;
                    validation.Value = data.Value;
                    this.validationData.Add(validation);
                }
            }
            else
            {
                this.validationData = null;
            }
        }

        /**
         * Sets new authentication details, will override the current values.
         *
         * @param authenticationParameters      REQUIRED: Authentication details as a Map.
         */
        public void setAuthenticationParameters(Dictionary<String, String> authenticationParameters)
        {
            this.authenticationParameters = authenticationParameters;
        }

        /**
         * Set an authentication detail, will override the current value.
         *
         * @param key                       REQUIRED: Authentication detail key.
         * @param value                     REQUIRED: Authentication detail value.
         */
        public void setAuthenticationParameter(String key, String value)
        {
            if (key != null)
            {
                if (this.authenticationParameters == null)
                {
                    this.authenticationParameters = new Dictionary<string, string>();
                }
                authenticationParameters.Add(key, value);
            }
            else
            {
                throw new InvalidParameterException(
                        "A null key was used to add a new authentications parameter.");
            }
        }
    }
}