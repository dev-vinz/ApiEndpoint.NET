﻿using System.Diagnostics;
using System.Net.Http.Headers;
using ApiEndpoint.Core;
using ApiEndpoint.Serialization;

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
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    return MessageSerializer.Deserialize<TOutput>(data);
                }
                else
                {
                    throw new Exception($"Failed to execute POST request to '{Endpoint}'.");
                }
            }
            catch (Exception ex)
            {
                Options.Logger.Error(
                    ex,
                    $"Deserialization encountered an error while executing POST request to '{Endpoint}'."
                );

                throw;
            }
        }
    }
}
