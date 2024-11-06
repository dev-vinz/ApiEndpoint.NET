namespace ApiEndpoint.Core
{
    internal sealed class RequestOptions
    {
        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                             PROPERTIES                            *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        public IThrottleRequests ThrottleRequests { get; set; } = null!;

        public Logger Logger { get; set; } = null!;
    }
}
