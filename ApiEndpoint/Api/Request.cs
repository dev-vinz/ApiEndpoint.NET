using System.Collections.Specialized;
using System.Diagnostics;
using System.Web;
using ApiEndpoint.Core;
using ApiEndpoint.Errors;
using ApiEndpoint.Serialization;

namespace ApiEndpoint.Api
{
    internal abstract class Request<T> : IRequest<T>
        where T : class
    {
        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                               FIELDS                              *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        private readonly string _endpoint;
        private readonly NameValueCollection _queryParams;
        private readonly RequestOptions _options;

        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                             PROPERTIES                            *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        protected string Endpoint =>
            new UriBuilder() { Path = _endpoint, Query = _queryParams.ToString(), }
                .Uri
                .PathAndQuery[1..];

        protected RequestOptions Options => _options;

        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                            CONSTRUCTORS                           *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        protected Request(string endpoint, RequestOptions options)
        {
            // Validation
            {
                if (!endpoint.StartsWith('/'))
                {
                    throw new ApiEndpointException("Endpoint must start with '/' character.");
                }
            }

            // Inputs
            {
                _endpoint = endpoint;
                _options = options;
            }

            // Tools
            {
                _queryParams = HttpUtility.ParseQueryString(string.Empty);
            }

            // Logging
            {
                _options.Logger.Information($"Created request to endpoint '{endpoint}'.");
            }
        }

        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                           PUBLIC METHODS                          *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        public IRequest<T> AddParam(string key, string value)
        {
            _options.Logger.Information($"Added query parameter '{key}' with value '{value}'.");
            _queryParams[key] = HttpUtility.UrlEncode(value);

            return this;
        }

        /* * * * * * * * * * * * * * * * * *\
        |*             ABSTRACT            *|
        \* * * * * * * * * * * * * * * * * */

        public abstract Task<T> ExecuteAsync();

        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                         PROTECTED METHODS                         *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        protected async Task<T> ExecuteRequestAsync(
            Func<Task<HttpResponseMessage>> sendRequestFuncAsync,
            string method
        )
        {
            Stopwatch watch = Stopwatch.StartNew();

            await Options.ThrottleRequests.WaitAsync().ConfigureAwait(false);
            Options.Logger.Information(
                $"{method} {Endpoint} throttling {watch.ElapsedMilliseconds}ms."
            );

            watch.Restart();

            HttpResponseMessage response = await sendRequestFuncAsync();

            Options.Logger.Information(
                $"{method} {Endpoint} {response.StatusCode} {watch.ElapsedMilliseconds}ms."
            );

            watch.Stop();

            try
            {
                string content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return MessageSerializer.Deserialize<T>(content, Options);
                }
                else
                {
                    ApiEndpointError error =
                        new()
                        {
                            StatusCode = response.StatusCode,
                            Message = response.ReasonPhrase ?? string.Empty,
                            ErrorContent = content,
                        };

                    throw new ApiEndpointException(error);
                }
            }
            catch (Exception ex) when (ex is not ApiEndpointException)
            {
                Options.Logger.Error(
                    ex,
                    $"Deserialization encountered an error while executing {method} request to '{Endpoint}'."
                );

                ApiEndpointError error =
                    new()
                    {
                        StatusCode = response.StatusCode,
                        Message =
                            $"Deserialization encountered an error while executing {method} request to '{Endpoint}'.",
                        InnerException = ex,
                    };

                throw new ApiEndpointException(error);
            }
        }
    }
}
