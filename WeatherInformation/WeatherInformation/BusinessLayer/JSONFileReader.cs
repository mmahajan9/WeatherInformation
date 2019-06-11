using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using WeatherInformation.Entity;

namespace WeatherInformation.BusinessLayer
{
    public class JSONFileReader : IFileReaderStrategy
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cityIds"></param>
        /// <returns></returns>
        public async Task<string> GetWetherInfoFromCityIds(List<int> cityIds)
        {
            try
            {
                // Get 20 Ids in loop and store each result in blob with method 'GetWeatherFromCityId' from weatherAPIProxy.
                HttpResponseMessage response = await WeatherAPIProxy.GetWeatherFromCityId(cityIds);
                if (response.IsSuccessStatusCode)
                {
                    var res = JsonConvert.DeserializeObject<Dictionary<string, object>>(response.Content.ReadAsStringAsync().Result);
                    return Convert.ToString(res["list"]);
                }
                else
                    return "";
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the file path and reads the city ids
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public IEnumerable<int> ReadCityIds(string filePath)
        {
            List<int> lstResult = new List<int>();
            HashSet<int> h = new HashSet<int>();
            using (StreamReader r = new StreamReader(filePath))
            {
                string json = r.ReadToEnd();
                JArray jArray = JsonConvert.DeserializeObject<JArray>(json);
                foreach (JObject j in jArray.Children())
                {
                    int a = Convert.ToInt32(j["id"]);
                    // WE COULD HAVE USED ANOTHER DATA STRUCTURE HERE. BUT I HAVEN'T WORKED ON ANY REQUIREMENT WHERE THERE IS SO BIG DATA IN A OBJECT. 
                    // HENCE LEAVING THIS RND FOR NOW AND USING LIST.
                    if (h.Add(a))
                        lstResult.Add(a);
                }
            }
            //return h;
            return lstResult;
        }
    }
}