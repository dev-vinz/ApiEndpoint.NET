using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ApiEndpoint.Serialization
{
    internal class MessageSerializer
    {
        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
        |*                           PUBLIC METHODS                          *|
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        /* * * * * * * * * * * * * * * * * *\
        |*              STATIC             *|
        \* * * * * * * * * * * * * * * * * */

        public static T Deserialize<T>(string data)
            where T : class
        {
            using MemoryStream ms = new(Encoding.UTF8.GetBytes(data));
            JsonSerializer serializer =
                new()
                {
#if DEBUG
                    MissingMemberHandling = MissingMemberHandling.Error,
#endif
                };

            using StreamReader sr = new(ms);
            JsonTextReader reader = new(sr);

            return serializer.Deserialize<T>(reader)!;
        }

        public static string Serialize(object value)
        {
            JsonSerializerSettings settings =
                new() { ContractResolver = new CamelCasePropertyNamesContractResolver() };

            return JsonConvert.SerializeObject(value, settings);
        }
    }
}
