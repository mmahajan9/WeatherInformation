using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using WeatherInformation.Entity;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace WeatherInformation.BusinessLayer
{
    public static class WeatherAPIProxy
    {
        
        public static async Task<HttpResponseMessage> GetWeatherFromCityId(List<int> lstCtyIds)
        {
            HttpResponseMessage response;
            try
            {
                string url = string.Format(Common.MultitCity_weatherAPIURL, string.Join(",", lstCtyIds), Common.weatherAPIAppId);
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(url);

                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

                // List data response.
                response = await client.GetAsync(url);
            }
            catch (Exception ex)
            {
                throw;
            }
            return response;
        }

        private static void readCityInfoAndCreateFile(IEnumerable<Dictionary<string, object>> lstDct)
        {
            foreach (Dictionary<string, object> dct in lstDct)
            {
                //FileManager.CreateFolderStructure.CreateSaveJSONFile(JsonConvert.SerializeObject(dct));
            }
        }
    
    }
}