using ApiEndpoint.Core;
using ApiEndpoint.Errors;
using ApiEndpoint.Serialization;
using System.Diagnostics;

namespace ApiEndpoint.Api
{
    internal sealed class GetRequest<T> : Request<T>
        where T : class
    {
        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                               FIELDS                              *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        private readonly HttpClient _client;

        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                            CONSTRUCTORS                           *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        public GetRequest(HttpClient client, string endpoint, RequestOptions options)
            : base(endpoint, options)
        {
            // Inputs
            {
                _client = client;
            }
        }

        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                           PUBLIC METHODS                          *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        public override async Task<T> ExecuteAsync()
        {
            Stopwatch watch = Stopwatch.StartNew();

            await Options.ThrottleRequests.WaitAsync().ConfigureAwait(false);
            Options.Logger.Information($"GET {Endpoint} throttling {watch.ElapsedMilliseconds}ms.");

            watch.Restart();

            HttpResponseMessage response = await _client.GetAsync(Endpoint);

            Options.Logger.Information(
                $"GET {Endpoint} {response.StatusCode} {watch.ElapsedMilliseconds}ms."
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
                            Message = response.ReasonPhrase,
                            ErrorContent = content,
                        };

                    throw new ApiEndpointException(error);
                }
            }
            catch (Exception ex) when (ex is not ApiEndpointException)
            {
                Options.Logger.Error(
                    ex,
                    $"Deserialization encountered an error while executing GET request to '{Endpoint}'."
                );

                ApiEndpointError error =
                    new()
                    {
                        StatusCode = response.StatusCode,
                        Message =
                            $"Deserialization encountered an error while executing GET request to '{Endpoint}'.",
                        InnerException = ex,
                    };

                throw new ApiEndpointException(error);
            }
        }
    }
}
