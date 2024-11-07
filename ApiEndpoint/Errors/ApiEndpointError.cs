using System.Net;

namespace ApiEndpoint.Errors
{
    /// <summary>
    /// Represents an error that occurs when an API endpoint fails.
    /// </summary>
    public sealed class ApiEndpointError
    {
        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                             PROPERTIES                            *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        /// <summary>
        /// Gets or sets the status code of the error.
        /// </summary>
        public HttpStatusCode StatusCode { get; set; } = default;

        /// <summary>
        /// Gets or sets the message of the error.
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Gets or sets the content of the error.
        /// </summary>
        public string? ErrorContent { get; set; }

        /// <summary>
        /// Gets or sets the exception that caused the error.
        /// </summary>
        public Exception? InnerException { get; set; }
    }
}
