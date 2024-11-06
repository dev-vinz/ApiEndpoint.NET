namespace ApiEndpoint.Core
{
    /// <summary>
    /// Represents a request to an API endpoint.
    /// </summary>
    /// <typeparam name="T">The type of the response object.</typeparam>
    public interface IRequest<T>
        where T : class
    {
        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                           PUBLIC METHODS                          *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        /// <summary>
        /// Adds a query parameter to the request.
        /// </summary>
        /// <param name="key">The key of the query parameter.</param>
        /// <param name="value">The value of the query parameter.</param>
        /// <returns>The request object.</returns>
        public IRequest<T> AddParam(string key, string value);

        /// <summary>
        /// Executes the request asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, with a result of type <typeparamref name="T"/>.</returns>
        public abstract Task<T> ExecuteAsync();
    }
}
