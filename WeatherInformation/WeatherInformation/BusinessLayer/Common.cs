using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeatherInformation.BusinessLayer
{
    public static class Common
    {
        public const string FileUploadDirectory = "WeatherInfo/Uploads/";
        public const string SingleCity_WeatherAPIURL = "http://api.openweathermap.org/data/2.5/weather?id={0}&APPID={1}";
        public const string MultitCity_weatherAPIURL = "https://api.openweathermap.org/data/2.5/group?id={0}&appid={1}";
        public const string weatherAPIAppId = "aa69195559bd4f88d79f9aadeb77a8f6";

        public static bool IsValidJson(string json)
        {
            try
            {
                JObject j = JObject.Parse(json);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}