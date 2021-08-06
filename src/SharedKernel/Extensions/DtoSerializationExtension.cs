using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.SharedKernel.Extensions
{
    public static class DtoSerializationExtension
    {
        public static string SerializeToJson(this ISerializable requestDto)
        {
            return JsonConvert.SerializeObject(requestDto);
        }

        public static HttpContent SerializeToHttpContent(this ISerializable requestDto)
        {
            var json = SerializeToJson(requestDto);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        public static T DeserializeFromJson<T>(string responseDto)
        {
            var obj = JsonConvert.DeserializeObject<T>(responseDto);
            return obj;
        }
    }
}
