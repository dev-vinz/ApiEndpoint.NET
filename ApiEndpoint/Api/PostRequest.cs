using ApiEndpoint.Core;
using ApiEndpoint.Errors;
using ApiEndpoint.Serialization;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace ApiEndpoint.Api
{
    internal sealed class PostRequest<TInput, TOutput> : Request<TOutput>
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

        public PostRequest(HttpClient client, string endpoint, TInput body, RequestOptions options)
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
                content.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");
            }
            else
            {
                // Handle JSON data
                content = new StringContent(MessageSerializer.Serialize(_body));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            Stopwatch watch = Stopwatch.StartNew();

            await Options.ThrottleRequests.WaitAsync().ConfigureAwait(false);
            Options.Logger.Information(
                $"POST {Endpoint} throttling {watch.ElapsedMilliseconds}ms."
            );

            watch.Restart();

            HttpResponseMessage response = await _client.PostAsync(Endpoint, content);

            Options.Logger.Information(
                $"POST {Endpoint} {response.StatusCode} {watch.ElapsedMilliseconds}ms."
            );

            watch.Stop();

            try
            {
                string data = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return MessageSerializer.Deserialize<TOutput>(data, Options);
                }
                else
                {
                    ApiEndpointError error =
                        new()
                        {
                            StatusCode = response.StatusCode,
                            Message = response.ReasonPhrase,
                            ErrorContent = data,
                        };

                    throw new ApiEndpointException(error);
                }
            }
            catch (Exception ex) when (ex is not ApiEndpointException)
            {
                Options.Logger.Error(
                    ex,
                    $"Deserialization encountered an error while executing POST request to '{Endpoint}'."
                );

                ApiEndpointError error =
                    new()
                    {
                        StatusCode = response.StatusCode,
                        Message =
                            $"Deserialization encountered an error while executing POST request to '{Endpoint}'.",
                        InnerException = ex,
                    };

                throw new ApiEndpointException(error);
            }
        }
    }
}
