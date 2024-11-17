using ApiEndpoint.Core;
using ApiEndpoint.Serialization;
using ApiEndpoint.Tests.Models;
using Newtonsoft.Json;

namespace ApiEndpoint.Tests
{
    public sealed class Methods
    {
        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                             CONSTANTS                             *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        private const string BASE_URI = "https://httpbin.org/";

        private const string ENDPOINT = "/anything";

        private const string DELETE_METHOD = "DELETE";
        private const string GET_METHOD = "GET";
        private const string POST_METHOD = "POST";
        private const string PUT_METHOD = "PUT";

        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                               FIELDS                              *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        private readonly IApiEndpoint _api;

        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                            CONSTRUCTORS                           *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        public Methods()
        {
            // Tools
            {
                _api = new ApiEndpointBuilder(BASE_URI)
                    .WithMissingMemberHandling(MissingMemberHandling.Ignore)
                    .Build();
            }
        }

        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                           PUBLIC METHODS                          *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        [Fact]
        public async Task GetRequestAsync()
        {
            // Arrange
            IRequest<HttpBinResponse> request = _api.Get<HttpBinResponse>(ENDPOINT);

            // Act
            HttpBinResponse response = await request.ExecuteAsync();

            // Assert
            Assert.NotNull(response);
            Assert.Empty(response.Args);
            Assert.Empty(response.Data);
            Assert.Empty(response.Form);
            Assert.Equal(GET_METHOD, response.Method);
        }

        [Fact]
        public async Task GetRequestAsync_WithParam()
        {
            // Arrange
            IRequest<HttpBinResponse> request = _api.Get<HttpBinResponse>(ENDPOINT)
                .AddParam("key", "value");

            // Act
            HttpBinResponse response = await request.ExecuteAsync();

            // Assert
            Assert.NotNull(response);
            KeyValuePair<string, string> kvp = Assert.Single(response.Args);
            Assert.Equal("key", kvp.Key);
            Assert.Equal("value", kvp.Value);
            Assert.Empty(response.Data);
            Assert.Empty(response.Form);
            Assert.Equal(GET_METHOD, response.Method);
        }

        [Fact]
        public async Task PostRequestAsync_WithBody()
        {
            // Arrange
            object body = new { key = "value" };
            IRequest<HttpBinResponse> request = _api.Post<object, HttpBinResponse>(ENDPOINT, body);

            // Act
            HttpBinResponse response = await request.ExecuteAsync();

            // Assert
            Assert.NotNull(response);
            Assert.Empty(response.Args);
            Assert.Equal(JsonConvert.SerializeObject(body), response.Data);
            Assert.Empty(response.Form);
            Assert.Equal(POST_METHOD, response.Method);
        }

        [Fact]
        public async Task PostRequestAsync_WithForm()
        {
            // Arrange
            MessageFormData body = new MessageFormData().AddField("key", "value");
            IRequest<HttpBinResponse> request = _api.Post<HttpBinResponse>(ENDPOINT, body);

            // Act
            HttpBinResponse response = await request.ExecuteAsync();

            // Assert
            Assert.NotNull(response);
            Assert.Empty(response.Args);
            Assert.Empty(response.Data);
            KeyValuePair<string, string> kvp = Assert.Single(response.Form);
            Assert.Equal("key", kvp.Key);
            Assert.Equal("value", kvp.Value);
            Assert.Equal(POST_METHOD, response.Method);
        }

        [Fact]
        public async Task DeleteRequestAsync()
        {
            // Arrange
            IRequest<HttpBinResponse> request = _api.Delete<HttpBinResponse>(ENDPOINT);

            // Act
            HttpBinResponse response = await request.ExecuteAsync();

            // Assert
            Assert.NotNull(response);
            Assert.Empty(response.Args);
            Assert.Empty(response.Data);
            Assert.Empty(response.Form);
            Assert.Equal(DELETE_METHOD, response.Method);
        }

        [Fact]
        public async Task PutRequestAsync_WithBody()
        {
            // Arrange
            object body = new { key = "value" };
            IRequest<HttpBinResponse> request = _api.Put<object, HttpBinResponse>(ENDPOINT, body);

            // Act
            HttpBinResponse response = await request.ExecuteAsync();

            // Assert
            Assert.NotNull(response);
            Assert.Empty(response.Args);
            Assert.Equal(JsonConvert.SerializeObject(body), response.Data);
            Assert.Empty(response.Form);
            Assert.Equal(PUT_METHOD, response.Method);
        }

        [Fact]
        public async Task PutRequestAsync_WithForm()
        {
            // Arrange
            MessageFormData body = new MessageFormData().AddField("key", "value");
            IRequest<HttpBinResponse> request = _api.Put<HttpBinResponse>(ENDPOINT, body);

            // Act
            HttpBinResponse response = await request.ExecuteAsync();

            // Assert
            Assert.NotNull(response);
            Assert.Empty(response.Args);
            Assert.Empty(response.Data);
            KeyValuePair<string, string> kvp = Assert.Single(response.Form);
            Assert.Equal("key", kvp.Key);
            Assert.Equal("value", kvp.Value);
            Assert.Equal(PUT_METHOD, response.Method);
        }
    }
}
