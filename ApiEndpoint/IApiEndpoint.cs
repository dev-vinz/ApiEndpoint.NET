using ApiEndpoint.Core;
using ApiEndpoint.Serialization;

namespace ApiEndpoint
{
    /// <summary>
    /// Represents an API endpoint.
    /// </summary>
    public interface IApiEndpoint
    {
        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                           PUBLIC METHODS                          *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        /// <summary>
        /// Create a DELETE request to the specified endpoint.
        /// </summary>
        /// <typeparam name="T">The type of the response object.</typeparam>
        /// <param name="endpoint">The endpoint to send the DELETE request to.</param>
        /// <returns>An <see cref="IRequest{T}"/> representing the DELETE request.</returns>
        IRequest<T> Delete<T>(string endpoint)
            where T : class;

        /// <summary>
        /// Create a GET request to the specified endpoint.
        /// </summary>
        /// <typeparam name="T">The type of the response object.</typeparam>
        /// <param name="endpoint">The endpoint to send the GET request to.</param>
        /// <returns>An <see cref="IRequest{T}"/> representing the GET request.</returns>
        IRequest<T> Get<T>(string endpoint)
            where T : class;

        /// <summary>
        /// Create a POST request to the specified endpoint.
        /// </summary>
        /// <typeparam name="T">The type of the response object.</typeparam>
        /// <param name="endpoint">The endpoint to send the POST request to.</param>
        /// <param name="body">The body of the POST request.</param>
        /// <returns>An <see cref="IRequest{T}"/> representing the POST request.</returns>
        IRequest<T> Post<T>(string endpoint, MessageFormData body)
            where T : class;

        /// <summary>
        /// Create a POST request to the specified endpoint.
        /// </summary>
        /// <typeparam name="TInput">The type of the request body.</typeparam>
        /// <typeparam name="TOutput">The type of the response object.</typeparam>
        /// <param name="endpoint">The endpoint to send the POST request to.</param>
        /// <param name="body">The body of the POST request.</param>
        /// <returns>An <see cref="IRequest{TOutput}"/> representing the POST request.</returns>
        IRequest<TOutput> Post<TInput, TOutput>(string endpoint, TInput body)
            where TOutput : class
            where TInput : class;

        /// <summary>
        /// Create a PUT request to the specified endpoint.
        /// </summary>
        /// <typeparam name="T">The type of the response object.</typeparam>
        /// <param name="endpoint">The endpoint to send the PUT request to.</param>
        /// <param name="body">The body of the PUT request.</param>
        /// <returns>An <see cref="IRequest{T}"/> representing the PUT request.</returns>

        IRequest<T> Put<T>(string endpoint, MessageFormData body)
            where T : class;

        /// <summary>
        /// Create a PUT request to the specified endpoint.
        /// </summary>
        /// <typeparam name="TInput">The type of the request body.</typeparam>
        /// <typeparam name="TOutput">The type of the response object.</typeparam>
        /// <param name="endpoint">The endpoint to send the PUT request to.</param>
        /// <param name="body">The body of the PUT request.</param>
        /// <returns>An <see cref="IRequest{TOutput}"/> representing the PUT request.</returns>
        IRequest<TOutput> Put<TInput, TOutput>(string endpoint, TInput body)
            where TOutput : class
            where TInput : class;
    }
}
