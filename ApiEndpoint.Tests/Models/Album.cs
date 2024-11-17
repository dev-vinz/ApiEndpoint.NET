using ApiEndpoint.Attributes;

namespace ApiEndpoint.Tests.Models
{
    [DeserializableInternal]
    internal sealed class Album
    {
        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                             PROPERTIES                            *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        public int UserId { get; internal set; } = int.MinValue;

        public int Id { get; internal set; } = int.MinValue;

        public string? Title { get; internal set; } = null;
    }
}
