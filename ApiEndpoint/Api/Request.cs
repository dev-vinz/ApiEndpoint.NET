using System.Collections.Specialized;
using System.Web;
using ApiEndpoint.Core;

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
                    throw new ArgumentException("Endpoint must start with '/' character.");
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
    }
}
