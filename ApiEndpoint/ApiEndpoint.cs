using ApiEndpoint.Api;
using ApiEndpoint.Core;
using ApiEndpoint.Serialization;

namespace ApiEndpoint
{
    internal sealed class ApiEndpoint : IApiEndpoint
    {
        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                               FIELDS                              *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        private readonly IReadOnlyList<HttpClient> _clients = [];
        private readonly RequestOptions _options;

        private int _index;

        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                             PROPERTIES                            *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        private HttpClient Client
        {
            get
            {
                int index = Interlocked.Increment(ref _index);
                return _clients[index % _clients.Count];
            }
        }

        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                            CONSTRUCTORS                           *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        public ApiEndpoint(IReadOnlyList<HttpClient> clients, RequestOptions options)
        {
            // Inputs
            {
                _clients = clients;
                _options = options;
            }
        }

        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                           PUBLIC METHODS                          *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        public IRequest<T> Delete<T>(string endpoint)
            where T : class
        {
            return new DeleteRequest<T>(Client, endpoint, _options);
        }

        public IRequest<T> Get<T>(string endpoint)
            where T : class
        {
            return new GetRequest<T>(Client, endpoint, _options);
        }

        public IRequest<TOutput> Post<TInput, TOutput>(string endpoint, TInput body)
            where TOutput : class
            where TInput : class
        {
            return new PostRequest<TInput, TOutput>(Client, endpoint, body, _options);
        }

        public IRequest<T> Post<T>(string endpoint, MessageFormData body)
            where T : class
        {
            return Post<MessageFormData, T>(endpoint, body);
        }

        public IRequest<TOutput> Put<TInput, TOutput>(string endpoint, TInput body)
            where TOutput : class
            where TInput : class
        {
            return new PutRequest<TInput, TOutput>(Client, endpoint, body, _options);
        }

        public IRequest<T> Put<T>(string endpoint, MessageFormData body)
            where T : class
        {
            return Put<MessageFormData, T>(endpoint, body);
        }
    }
}
