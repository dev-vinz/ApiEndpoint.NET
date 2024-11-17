namespace ApiEndpoint.Tests.Models
{
    internal sealed class HttpBinResponse
    {
        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                             PROPERTIES                            *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        public Dictionary<string, string> Args { get; set; } = [];

        public string Data { get; set; } = string.Empty;

        public Dictionary<string, string> Form { get; set; } = [];

        public string Method { get; set; } = string.Empty;
    }
}
