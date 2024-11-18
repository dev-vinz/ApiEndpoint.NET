using ApiEndpoint.Errors;
using System.Net;

namespace ApiEndpoint.Tests
{
    public sealed class Errors
    {
        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                             CONSTANTS                             *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        private const string BASE_URI = "https://httpbin.org/";

        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                           PUBLIC METHODS                          *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        [Fact]
        public void When_EmptyBaseUri_Expect_Exception()
        {
            // Arrange
            string baseUri = string.Empty;

            // Act
            void action() => new ApiEndpointBuilder(baseUri).Build();

            // Assert
            Assert.Throws<ApiEndpointException>(action);
        }

        [Fact]
        public void When_NullBaseUri_Expect_Exception()
        {
            // Arrange
            string baseUri = null!;

            // Act
            void action() => new ApiEndpointBuilder(baseUri).Build();

            // Assert
            Assert.Throws<ApiEndpointException>(action);
        }

        [Fact]
        public void When_NullEndpoint_Expect_Exception()
        {
            // Arrange
            string endpoint = null!;

            // Act
            void action() => new ApiEndpointBuilder(BASE_URI).Build().Get<object>(endpoint);

            // Assert
            Assert.Throws<ApiEndpointException>(action);
        }

        [Fact]
        public void When_EmptyEndpoint_Expect_Exception()
        {
            // Arrange
            string endpoint = string.Empty;

            // Act
            void action() => new ApiEndpointBuilder(BASE_URI).Build().Get<object>(endpoint);

            // Assert
            Assert.Throws<ApiEndpointException>(action);
        }

        [Fact]
        public void When_EndpointDoesntStartWithSlash_Expect_Exception()
        {
            // Arrange
            string endpoint = "get";

            // Act
            void action() => new ApiEndpointBuilder(BASE_URI).Build().Get<object>(endpoint);

            // Assert
            Assert.Throws<ApiEndpointException>(action);
        }

        [Theory]
        [MemberData(nameof(GetErrorStatusCodes))]
        public async Task When_HttpStatusCodeErrors_Expect_Exception_And_StatusCodes(int statusCode)
        {
            // Arrange
            string endpoint = $"/status/{statusCode}";

            // Act
            Task action() =>
                new ApiEndpointBuilder(BASE_URI).Build().Get<object>(endpoint).ExecuteAsync();

            // Assert
            ApiEndpointException ex = await Assert.ThrowsAsync<ApiEndpointException>(action);
            Assert.Equal((HttpStatusCode)statusCode, ex.Error.StatusCode);
        }

        [Fact]
        public void When_NullBearerToken_Expect_Exception()
        {
            // Arrange
            string bearerToken = null!;

            // Act
            void action() => new ApiEndpointBuilder(BASE_URI).AddBearerToken(bearerToken);

            // Assert
            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void When_EmptyBearerToken_Expect_Exception()
        {
            // Arrange
            string bearerToken = string.Empty;

            // Act
            void action() => new ApiEndpointBuilder(BASE_URI).AddBearerToken(bearerToken);

            // Assert
            Assert.Throws<ArgumentException>(action);
        }

        /* * * * * * * * * * * * * * * * * *\
        |*              STATIC             *|
        \* * * * * * * * * * * * * * * * * */

        public static TheoryData<int> GetErrorStatusCodes()
        {
            return
            [
                .. Enum.GetValues(typeof(HttpStatusCode))
                       .Cast<int>()
                       .Where(code => code >= 400)
                       .Distinct()
            ];
        }
    }
}
