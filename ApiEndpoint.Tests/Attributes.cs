using ApiEndpoint.Core;
using ApiEndpoint.Tests.Models;
using Newtonsoft.Json;

namespace ApiEndpoint.Tests
{
    public sealed class Attributes
    {
        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                             CONSTANTS                             *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        private const string BASE_URI = "https://jsonplaceholder.typicode.com/";

        private const string COMMENTS_ENDPOINT = "/comments/6";
        private const string ALBUMS_ENDPOINT = "/albums/15";

        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                               FIELDS                              *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        private readonly IApiEndpoint _api;

        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                            CONSTRUCTORS                           *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        public Attributes()
        {
            // Tools
            {
                _api = new ApiEndpointBuilder(BASE_URI)
                    .WithMissingMemberHandling(MissingMemberHandling.Error)
                    .Build();
            }
        }

        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                           PUBLIC METHODS                          *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        [Fact]
        public async Task When_PublicProperty_Expect_Deserialized()
        {
            // Arrange
            IRequest<Comment> request = _api.Get<Comment>(COMMENTS_ENDPOINT);

            // Act
            Comment response = await request.ExecuteAsync();

            // Assert
            Assert.NotNull(response);
            Assert.IsType<Comment>(response);
            Assert.NotEqual(int.MinValue, response.PostId);
        }

        [Fact]
        public async Task When_InternalPropertyWithoutAttribute_Expect_NotDeserialized()
        {
            // Arrange
            IRequest<Comment> request = _api.Get<Comment>(COMMENTS_ENDPOINT);

            // Act
            Comment response = await request.ExecuteAsync();

            // Assert
            Assert.NotNull(response);
            Assert.IsType<Comment>(response);
            Assert.Equal(int.MinValue, response.Id);
        }

        [Fact]
        public async Task When_InternalPropertyWithAttribute_Expect_Deserialized()
        {
            // Arrange
            IRequest<Comment> request = _api.Get<Comment>(COMMENTS_ENDPOINT);

            // Act
            Comment response = await request.ExecuteAsync();

            // Assert
            Assert.NotNull(response);
            Assert.IsType<Comment>(response);
            Assert.NotNull(response.Name);
        }

        [Fact]
        public async Task When_PrivatePropertyWithoutAttribute_Expect_NotDeserialized()
        {
            // Arrange
            IRequest<Comment> request = _api.Get<Comment>(COMMENTS_ENDPOINT);

            // Act
            Comment response = await request.ExecuteAsync();

            // Assert
            Assert.NotNull(response);
            Assert.IsType<Comment>(response);
            Assert.Null(response.Email);
        }

        [Fact]
        public async Task When_PrivatePropertyWithAttribute_Expect_NotDeserialized()
        {
            // Arrange
            IRequest<Comment> request = _api.Get<Comment>(COMMENTS_ENDPOINT);

            // Act
            Comment response = await request.ExecuteAsync();

            // Assert
            Assert.NotNull(response);
            Assert.IsType<Comment>(response);
            Assert.Null(response.Body);
        }

        [Fact]
        public async Task When_AttributeOnClass_Expect_AllInternalDeserialized()
        {
            // Arrange
            IRequest<Album> request = _api.Get<Album>(ALBUMS_ENDPOINT);

            // Act
            Album response = await request.ExecuteAsync();

            // Assert
            Assert.NotNull(response);
            Assert.IsType<Album>(response);
            Assert.NotEqual(int.MinValue, response.UserId);
            Assert.NotEqual(int.MaxValue, response.Id);
            Assert.NotNull(response.Title);
        }
    }
}
