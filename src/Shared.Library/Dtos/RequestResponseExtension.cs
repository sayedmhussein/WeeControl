using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace MySystem.Shared.Library.Dtos
{
    public static class RequestResponseExtension
    {
        #region RequestDto
        public static string SerializeToJson<T>(this IRequestDto<T> requestDto)
        {
            return JsonConvert.SerializeObject(requestDto);
        }

        public static HttpContent SerializeToHttpContent<T>(this IRequestDto<T> requestDto)
        {
            var json = SerializeToJson(requestDto);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
        #endregion

        #region ResponseDto
        public static string SerializeToJson<T>(this IResponseDto<T> responseDto)
        {
            return JsonConvert.SerializeObject(responseDto);
        }

        public static T DeserializeFromJson<T>(string responseDto)
        {
            var obj = JsonConvert.DeserializeObject<T>(responseDto);
            return obj;
        }
        #endregion
    }
}
