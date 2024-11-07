namespace ApiEndpoint.Attributes
{
    /// <summary>
    /// Indicates that a class or property is deserializable from internal sources.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public sealed class DeserializableInternalAttribute : Attribute { }
}
