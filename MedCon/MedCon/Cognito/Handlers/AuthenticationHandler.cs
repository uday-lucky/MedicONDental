using System;

namespace awscognito.Cognito.Handlers
{
    public interface AuthenticationHandler
    {
        /**
         * This method is called when a fatal exception was encountered during
         * authentication. The current authentication process continue because of the error
         * , hence a continuation is not available. Probe {@code exception} for details.
         *
         * @param exception is this Exception leading to authentication failure.
         */
        void onFailure(Exception exception);
    }
}
