using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using WeeControl.Common.SharedKernel.Interfaces;

namespace WeeControl.Common.SharedKernel.Extensions
{
    public static class DtoSerializationExtension
    {
        public static string SerializeToJson(this ISerializable requestDto)
        {
            return JsonConvert.SerializeObject(requestDto);
        }

        public static T DeserializeFromJson<T>(string responseDto)
        {
            var obj = JsonConvert.DeserializeObject<T>(responseDto);
            return obj;
        }

        public static HttpContent SerializeToHttpContent(this ISerializable requestDto, string mediaType = "application/json")
        {
            var json = SerializeToJson(requestDto);
            return new StringContent(json, Encoding.UTF8, mediaType);
        }
    }
}
