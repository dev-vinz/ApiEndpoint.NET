using System.Diagnostics;
using ApiEndpoint.Core;
using ApiEndpoint.Serialization;

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
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    return MessageSerializer.Deserialize<T>(data, Options);
                }
                else
                {
                    throw new Exception($"Failed to execute GET request to '{Endpoint}'.");
                }
            }
            catch (Exception ex)
            {
                Options.Logger.Error(
                    ex,
                    $"Deserialization encountered an error while executing GET request to '{Endpoint}'."
                );

                throw;
            }
        }
    }
}
