namespace ApiEndpoint.Errors
{
    /// <summary>
    /// Represents an exception that occurs when an API endpoint fails.
    /// </summary>
    public class ApiEndpointException : Exception
    {
        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                             PROPERTIES                            *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        /// <summary>
        /// Gets the error that occurred when the API endpoint failed.
        /// </summary>
        public ApiEndpointError Error { get; } = default!;

        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                            CONSTRUCTORS                           *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        internal ApiEndpointException(string message)
            : base(message)
        {
            // Outputs
            {
                Error = new ApiEndpointError() { Message = message };
            }
        }

        internal ApiEndpointException(string message, Exception innerException)
            : base(message, innerException)
        {
            // Outputs
            {
                Error = new ApiEndpointError()
                {
                    Message = message,
                    InnerException = InnerException
                };
            }
        }

        internal ApiEndpointException(ApiEndpointError error)
            : base(error.Message, error.InnerException)
        {
            // Inputs
            {
                Error = error;
            }
        }
    }
}
