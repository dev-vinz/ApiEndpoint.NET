using Newtonsoft.Json;

namespace ApiEndpoint.Core
{
    internal sealed class RequestOptions
    {
        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                             PROPERTIES                            *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        public string? DateFormat { get; set; }

        public IThrottleRequests ThrottleRequests { get; set; } = null!;

        public Logger Logger { get; set; } = null!;

        public MissingMemberHandling? MissingMemberHandling { get; set; }
    }
}
