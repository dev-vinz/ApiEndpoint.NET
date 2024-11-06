using System.Net.Http.Headers;
using ApiEndpoint.Core;
using ApiEndpoint.Validators;
using Newtonsoft.Json;

namespace ApiEndpoint
{
    /// <summary>
    /// Builds an <see cref="IApiEndpoint"/> instance.
    /// </summary>
    public sealed class ApiEndpointBuilder
    {
        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                             CONSTANTS                             *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        private const uint DEFAULT_MAX_REQUESTS_PER_SECOND = 10;

        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                               FIELDS                              *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        private readonly Uri _baseUri;
        private readonly List<HttpClient> _clients;

        private Action<string> _logger;
        private MissingMemberHandling? _missingMemberHandling;
        private string? _dateFormat;
        private uint _maxRequestsPerSecond;

        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                            CONSTRUCTORS                           *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiEndpointBuilder"/> class.
        /// </summary>
        /// <param name="baseUri">The base URI of the API.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="baseUri"/> is null or empty.</exception>
        public ApiEndpointBuilder(string baseUri)
        {
            // Validation
            {
                if (string.IsNullOrEmpty(baseUri))
                {
                    throw new ArgumentException("Value cannot be null or empty.", nameof(baseUri));
                }
            }

            // Inputs
            {
                _baseUri = new Uri(baseUri);
            }

            // Tools
            {
                _clients = [];
                _logger = (_) => { };
                _maxRequestsPerSecond = DEFAULT_MAX_REQUESTS_PER_SECOND;
            }
        }

        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                           PUBLIC METHODS                          *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        /// <summary>
        /// Adds a secure token to the <see cref="IApiEndpoint"/> instance.
        /// </summary>
        /// <param name="secureToken">The secure token to add to the <see cref="IApiEndpoint"/> instance.</param>
        /// <returns>The <see cref="ApiEndpointBuilder"/> instance with the secure token added.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="secureToken"/> is null or empty.</exception>
        public ApiEndpointBuilder AddSecureToken(string secureToken)
        {
            return WithSecureTokens(secureToken);
        }

        /// <summary>
        /// Builds an <see cref="IApiEndpoint"/> instance.
        /// </summary>
        /// <remarks>If no secure tokens were provided, a default client will be added.</remarks>
        /// <returns>An <see cref="IApiEndpoint"/> instance.</returns>
        public IApiEndpoint Build()
        {
            // If no secure tokens were provided, add a default client
            if (_clients.Count == 0)
            {
                _clients.Add(new() { BaseAddress = _baseUri });
            }

            // Create the throttle requests option
            IThrottleRequests throttle = new ThrottleRquestsPerSeconds(
                _maxRequestsPerSecond,
                (uint)_clients.Count
            );

            // Create the logger
            Logger logger = new(_logger);

            // Create the request options
            RequestOptions options =
                new()
                {
                    DateFormat = _dateFormat,
                    ThrottleRequests = throttle,
                    Logger = logger,
                    MissingMemberHandling = _missingMemberHandling
                };

            return new ApiEndpoint(_clients, options);
        }

        /// <summary>
        /// Adds a date format to the <see cref="IApiEndpoint"/> instance.
        /// </summary>
        /// <param name="dateFormat">The date format to add to the <see cref="IApiEndpoint"/> instance.</param>
        /// <returns>The <see cref="ApiEndpointBuilder"/> instance with the date format added.</returns>
        public ApiEndpointBuilder WithDateFormat(string dateFormat)
        {
            _dateFormat = dateFormat;
            return this;
        }

        /// <summary>
        /// Adds a logger to the <see cref="IApiEndpoint"/> instance.
        /// </summary>
        /// <param name="logger">The logger to add to the <see cref="IApiEndpoint"/> instance.</param>
        /// <returns>The <see cref="ApiEndpointBuilder"/> instance with the logger added.</returns>
        public ApiEndpointBuilder WithLogger(Action<string> logger)
        {
            _logger = logger;
            return this;
        }

        /// <summary>
        /// Adds a <see cref="MissingMemberHandling"/> to the <see cref="IApiEndpoint"/> instance.
        /// </summary>
        /// <param name="handling">The <see cref="MissingMemberHandling"/> to add to the <see cref="IApiEndpoint"/> instance.</param>
        /// <returns>The <see cref="ApiEndpointBuilder"/> instance with the missing member handling added.</returns>
        public ApiEndpointBuilder WithMissingMemberHandling(MissingMemberHandling handling)
        {
            _missingMemberHandling = handling;
            return this;
        }

        /// <summary>
        /// Adds a request throttle to the <see cref="IApiEndpoint"/> instance.
        /// </summary>
        /// <remarks>The request throttle will limit the number of requests per second among all secure tokens.</remarks>
        /// <param name="maxRequestsPerSecond">The maximum number of requests per second to allow.</param>
        /// <returns>The <see cref="ApiEndpointBuilder"/> instance with the request throttle added.</returns>
        public ApiEndpointBuilder WithRequestThrottle(uint maxRequestsPerSecond)
        {
            _maxRequestsPerSecond = maxRequestsPerSecond;
            return this;
        }

        /// <summary>
        /// Adds secure tokens to the <see cref="IApiEndpoint"/> instance.
        /// </summary>
        /// <remarks>Each secure token will be used to create a separate <see cref="HttpClient"/> instance.</remarks>
        /// <param name="secureTokens">The secure tokens to add to the <see cref="IApiEndpoint"/> instance.</param>
        /// <returns>The <see cref="ApiEndpointBuilder"/> instance with the secure tokens added.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="secureTokens"/> is null or empty.</exception>
        public ApiEndpointBuilder WithSecureTokens(params string[] secureTokens)
        {
            // Validate the secure tokens
            TokenValidator.Validate(secureTokens);

            // Create a client for each secure token
            foreach (string secureToken in secureTokens)
            {
                _clients.Add(CreateClient(secureToken));
            }

            return this;
        }

        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                          PRIVATE METHODS                          *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        private HttpClient CreateClient(string secureToken)
        {
            HttpClient client = new() { BaseAddress = _baseUri };

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                secureToken
            );

            return client;
        }
    }
}
