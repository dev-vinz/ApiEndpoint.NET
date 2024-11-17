namespace ApiEndpoint.Tests.Models
{
    internal sealed class BearerAuthentication
    {
        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                             PROPERTIES                            *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        public bool Authenticated { get; set; }

        public string Token { get; set; } = null!;
    }
}
