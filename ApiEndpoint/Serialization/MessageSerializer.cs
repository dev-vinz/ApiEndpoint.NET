using System.Text;
using ApiEndpoint.Core;
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

        public static T Deserialize<T>(string data, RequestOptions options)
            where T : class
        {
            using MemoryStream ms = new(Encoding.UTF8.GetBytes(data));
            JsonSerializer serializer =
                new() { ContractResolver = new InternalSetterContractResolver(), };

            if (options.DateFormat != null)
            {
                serializer.DateFormatString = options.DateFormat;
            }

            if (options.MissingMemberHandling != null)
            {
                serializer.MissingMemberHandling = options.MissingMemberHandling.Value;
            }

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
