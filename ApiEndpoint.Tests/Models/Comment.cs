using ApiEndpoint.Attributes;

namespace ApiEndpoint.Tests.Models
{
    internal sealed class Comment
    {
        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                             PROPERTIES                            *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        public int PostId { get; set; } = int.MinValue;

        public int Id { get; internal set; } = int.MinValue;

        [DeserializableInternal]
        public string? Name { get; internal set; } = null;

        public string? Email { get; private set; } = null;

        [DeserializableInternal]
        public string? Body { get; private set; } = null;
    }
}
