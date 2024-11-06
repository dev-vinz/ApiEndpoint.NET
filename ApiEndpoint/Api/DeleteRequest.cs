using System.Diagnostics;
using ApiEndpoint.Core;
using ApiEndpoint.Serialization;

namespace ApiEndpoint.Api
{
    internal sealed class DeleteRequest<T> : Request<T>
        where T : class
    {
        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                               FIELDS                              *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        private readonly HttpClient _client;

        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                            CONSTRUCTORS                           *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        public DeleteRequest(HttpClient client, string endpoint, RequestOptions options)
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
            Options.Logger.Information(
                $"DELETE {Endpoint} throttling {watch.ElapsedMilliseconds}ms."
            );

            watch.Restart();

            HttpResponseMessage response = await _client.DeleteAsync(Endpoint);

            Options.Logger.Information(
                $"DELETE {Endpoint} {response.StatusCode} {watch.ElapsedMilliseconds}ms."
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
                    throw new Exception($"Failed to execute DELETE request to '{Endpoint}'.");
                }
            }
            catch (Exception ex)
            {
                Options.Logger.Error(
                    ex,
                    $"Deserialization encountered an error while executing DELETE request to '{Endpoint}'."
                );

                throw;
            }
        }
    }
}
