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

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiEndpointException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ApiEndpointException(string message)
            : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiEndpointException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public ApiEndpointException(string message, Exception innerException)
            : base(message, innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiEndpointException"/> class.
        /// </summary>
        /// <param name="error">The error that occurred when the API endpoint failed.</param>
        public ApiEndpointException(ApiEndpointError error)
            : base(error.Message, error.InnerException)
        {
            // Inputs
            {
                Error = error;
            }
        }
    }
}
