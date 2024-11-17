using System.Net.Http.Headers;
using ApiEndpoint.Core;
using ApiEndpoint.Serialization;

namespace ApiEndpoint.Api
{
    internal sealed class PutRequest<TInput, TOutput> : Request<TOutput>
        where TInput : class
        where TOutput : class
    {
        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                               FIELDS                              *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        private readonly HttpClient _client;
        private readonly TInput _body;

        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                            CONSTRUCTORS                           *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        public PutRequest(HttpClient client, string endpoint, TInput body, RequestOptions options)
            : base(endpoint, options)
        {
            // Inputs
            {
                _client = client;
                _body = body;
            }
        }

        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                           PUBLIC METHODS                          *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        public override async Task<TOutput> ExecuteAsync()
        {
            HttpContent content;

            if (_body is MessageFormData formData)
            {
                // Handle multi-part form data
                content = formData;
                content.Headers.ContentDisposition = formData.Headers.ContentDisposition;
                content.Headers.ContentLength = formData.Headers.ContentLength;
                content.Headers.ContentType = formData.Headers.ContentType;
            }
            else
            {
                // Handle JSON data
                content = new StringContent(MessageSerializer.Serialize(_body));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            return await ExecuteRequestAsync(() => _client.PutAsync(Endpoint, content), "PUT");
        }
    }
}
