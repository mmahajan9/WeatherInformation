using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeatherInformation.Entity
{
    [JsonArrayAttribute]
    [Serializable]
    public class WeatherInformationEntity
    {
        public int id { get; set; }
        public string name { get; set; }
        public string visibility { get; set; }
        public main main { get; set; }
        public wind wind { get; set; }
        public weather weather { get; set; }
    }
    [JsonArrayAttribute]
    [Serializable]
    public class main
    {
        public string temp { get; set; }
        public string pressure { get; set; }
        public string humidity { get; set; }
        public string temp_min { get; set; }
        public string temp_max { get; set; }
    }
    [JsonArrayAttribute]
    [Serializable]
    public class wind
    {
        public string speed { get; set; }
        public string deg { get; set; }
    }
    [JsonArrayAttribute]
    [Serializable]
    public class weather
    {
        public string description { get; set; }
    }
}