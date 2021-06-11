﻿using System;
using System.Net.Http;
using System.Text;
using MySystem.SharedKernel.Interfaces;
using Newtonsoft.Json;

namespace MySystem.SharedKernel.ExtensionMethods
{
    public static class DtoSerializationExtension
    {
        public static string SerializeToJson(this IRequestDto requestDto)
        {
            return JsonConvert.SerializeObject(requestDto);
        }

        public static HttpContent SerializeToHttpContent(this IRequestDto requestDto)
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
