using System.Diagnostics;
using System.Net;
using ApiEndpoint.Errors;
using ApiEndpoint.Tests.Models;
using Newtonsoft.Json;

namespace ApiEndpoint.Tests
{
    public sealed class BuilderSettings
    {
        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                             CONSTANTS                             *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        private const string BASE_URI = "https://httpbin.org/";

        private const string BEARER_TOKEN = "my-valid-token";

        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                           PUBLIC METHODS                          *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        [Fact]
        public async Task When_BearerToken_Expect_Authorized()
        {
            // Arrange
            IApiEndpoint endpoint = new ApiEndpointBuilder(BASE_URI)
                .AddBearerToken(BEARER_TOKEN)
                .Build();

            // Act
            BearerAuthentication response = await endpoint
                .Get<BearerAuthentication>("/bearer")
                .ExecuteAsync();

            // Assert
            Assert.NotNull(response);
            Assert.True(response.Authenticated);
            Assert.Equal(BEARER_TOKEN, response.Token);
        }

        [Fact]
        public async Task When_BearerTokens_Expect_Authorized()
        {
            // Arrange
            IApiEndpoint endpoint = new ApiEndpointBuilder(BASE_URI)
                .WithBearerTokens(BEARER_TOKEN, BEARER_TOKEN)
                .Build();

            // Act - First request
            BearerAuthentication response = await endpoint
                .Get<BearerAuthentication>("/bearer")
                .ExecuteAsync();

            // Assert - First request
            Assert.NotNull(response);
            Assert.True(response.Authenticated);
            Assert.Equal(BEARER_TOKEN, response.Token);

            // Act - Second request
            response = await endpoint.Get<BearerAuthentication>("/bearer").ExecuteAsync();

            // Assert - Second request
            Assert.NotNull(response);
            Assert.True(response.Authenticated);
            Assert.Equal(BEARER_TOKEN, response.Token);
        }

        [Fact]
        public async Task When_MissingBearerToken_Expect_Unauthorized()
        {
            // Arrange
            IApiEndpoint endpoint = new ApiEndpointBuilder(BASE_URI).Build();

            // Act
            Task action() => endpoint.Get<object>("/bearer").ExecuteAsync();

            // Assert
            ApiEndpointException ex = await Assert.ThrowsAsync<ApiEndpointException>(action);
            Assert.Equal(HttpStatusCode.Unauthorized, ex.Error.StatusCode);
        }

        [Fact]
        public async Task When_MissingMemberHandling_Ignore_Expect_NoException()
        {
            // Arrange
            IApiEndpoint endpoint = new ApiEndpointBuilder(BASE_URI)
                .WithMissingMemberHandling(MissingMemberHandling.Ignore)
                .Build();

            // Act
            object response = await endpoint.Get<object>("/get").ExecuteAsync();

            // Assert
            Assert.NotNull(response);
        }

        [Fact]
        public async Task When_MissingMemberHandling_Error_Expect_InternalServerError()
        {
            // Arrange
            IApiEndpoint endpoint = new ApiEndpointBuilder(BASE_URI)
                .WithMissingMemberHandling(MissingMemberHandling.Error)
                .Build();

            // Act
            Task action() => endpoint.Get<HttpBinResponse>("/get").ExecuteAsync();

            // Assert
            ApiEndpointException ex = await Assert.ThrowsAsync<ApiEndpointException>(action);
            Assert.Equal(HttpStatusCode.InternalServerError, ex.Error.StatusCode);
        }

        [Fact]
        public async Task When_ThrottleRequests_SecondRequest_Waits_Timeout()
        {
            // Arrange
            IApiEndpoint endpoint = new ApiEndpointBuilder(BASE_URI) //
                .WithRequestThrottle(1)
                .Build();

            Task first = endpoint.Get<object>("/get").ExecuteAsync();
            Task second = endpoint.Get<object>("/get").ExecuteAsync();

            // Act
            Stopwatch watch = Stopwatch.StartNew();
            await Task.WhenAll(first, second);
            watch.Stop();

            // Assert
            Assert.True(
                watch.Elapsed >= TimeSpan.FromSeconds(1),
                $"Expected delay of at least 1 second between requests, but got {watch.ElapsedMilliseconds}ms"
            );
        }
    }
}
