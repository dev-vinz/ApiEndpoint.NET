using System.Reflection;
using ApiEndpoint.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ApiEndpoint.Serialization
{
    internal sealed class InternalSetterContractResolver : DefaultContractResolver
    {
        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                         PROTECTED METHODS                         *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        /* * * * * * * * * * * * * * * * * *\
        |*             OVERRIDE            *|
        \* * * * * * * * * * * * * * * * * */

        protected override List<MemberInfo> GetSerializableMembers(Type objectType)
        {
            return
            [
                .. objectType.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance) //
                             .Where(m => m.MemberType == MemberTypes.Property)
            ];
        }

        protected override JsonProperty CreateProperty(
            MemberInfo member,
            MemberSerialization memberSerialization
        )
        {
            // Create the JsonProperty
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            // Check if the member is a property
            if (member is PropertyInfo propertyInfo)
            {
                // Get the set method
                MethodInfo? setMethod = propertyInfo.GetSetMethod(nonPublic: true);

                // Rule 1: If the set method is not null and public, then the property is writable
                if (setMethod != null && setMethod.IsPublic)
                {
                    property.Writable = true;
                }
                // Rule 2: If the set method is not null and private, then the property is not writable
                else if (setMethod != null && setMethod.IsPrivate)
                {
                    property.Writable = false;
                }
                // Rule 3: If the set method is not null and internal, and the property or the class has the DeserializableInternalAttribute, then the property is writable
                else if (setMethod != null && setMethod.IsAssembly)
                {
                    bool isClass =
                        member.DeclaringType?.GetCustomAttribute<DeserializableInternalAttribute>()
                        != null;

                    bool isProperty =
                        propertyInfo.GetCustomAttribute<DeserializableInternalAttribute>() != null;

                    if (isClass || isProperty)
                    {
                        property.Writable = true;
                    }
                    // Rule 4: If the set method is not null and internal, and the property and the class do not have the DeserializableInternalAttribute, then the property is not writable
                    else
                    {
                        property.Writable = false;
                    }
                }
            }

            return property;
        }
    }
}
