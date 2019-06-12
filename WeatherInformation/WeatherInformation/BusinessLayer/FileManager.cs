using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WeatherInformation.Entity;

namespace WeatherInformation.BusinessLayer
{
    public static class FileManager
    {
        public static string ReadFileAsText(string path, string fileName)
        {
            return File.ReadAllText(Path.Combine(path, fileName));
        }
        public static bool SaveFileToBlob(string path, string fileName, HttpPostedFileBase file)
        {
            bool isFileSaved = false;
            //path += DateTime.Now.Date.Day + "." + DateTime.Now.Month + "." + DateTime.Now.Year + "." + DateTime.Now.Hour + "." + DateTime.Now.Minute;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (System.IO.File.Exists(Path.Combine(path, fileName)))
            {
                return WriteChunksToFile(Path.Combine(path, fileName), file);
            }
            else
            {
                file.SaveAs(Path.Combine(path, fileName));
                isFileSaved = true;
            }
            return true;
        }


        /// <summary>
        /// gets the file path. Reads the file. Gets city ids. Fetches data from weather API and stores output to individual file.
        /// </summary>
        /// <param name="filePath">Path for the file to read</param>
        /// <returns></returns>
        public static async Task StoreCityWiseOutputForHistoryAsync(string filePath)
        {
            // 1. Read the json file
            IFileReaderStrategy strategy = Factory.GetStrategy(GetFileExtension(filePath));

            // 2. get the first twenty City ids and call the Weather API to get the weather information according to city
            IEnumerable<int> lstWeatherInfo = new List<int>();
            lstWeatherInfo = strategy.ReadCityIds(filePath);
            
            int cnt = 1;
            int remainder = 0;
            int iteration = Math.DivRem(lstWeatherInfo.Count(), 20, out remainder);
            if (remainder > 0)
                ++iteration;

            Task[] tasks = new Task[iteration];
            while (cnt <= iteration)
            {
                tasks[cnt-1] = SaveCityWiseWeatherInfoAsync(strategy, lstWeatherInfo.Skip(20 * (cnt - 1)).Take(20).ToList());
                cnt++;
            }
            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Gets strategy and City Ids list and saves weather info for each city to individual file according to city id
        /// </summary>
        /// <param name="strategy"></param>
        /// <param name="lstCityIds"></param>
        /// <returns></returns>
        private static async Task SaveCityWiseWeatherInfoAsync(IFileReaderStrategy strategy, List<int> lstCityIds)
        {
            if (lstCityIds.Count <= 20)
            {
                // 3. Get the Weather information for Cities from city id.
                string json = await strategy.GetWetherInfoFromCityIds(lstCityIds);
                // 4. store the output respective to each city with nomenclature as - folders created with (mm_dd_yyyy) and file name created as "<CityId>_hh.MM.ss.json"
                await CreateSaveJSONFile(json);
            }
        }
        /// <summary>
        /// Gets the extension of a file when given with file address with name
        /// </summary>
        /// <param name="fileName">Name of the file with extension</param>
        /// <returns></returns>
        public static string GetFileExtension(string fileName)
        {
            var arr = fileName.Split('.');
            return arr[arr.Count() - 1];
        }
        private static bool WriteChunksToFile(string filePath, HttpPostedFileBase file)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Append))
            {
                byte[] bytes = GetBytes(file.InputStream);
                fs.Write(bytes, 0, bytes.Length);
            }
            return true;
        }
        private static byte[] GetBytes(Stream input)
        {
            byte[] buffer = new byte[input.Length];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }

                return ms.ToArray();
            }
        }
        public static async Task<bool> CreateSaveJSONFile(string json)
        {
            bool success = false;
            try
            {
                var _json = JsonConvert.DeserializeObject<IEnumerable<Dictionary<string, object>>>(json);
                Task[] tasks = new Task[_json.Count()];
                int cnt = 0;
                foreach (Dictionary<string, object> dct in _json)
                {
                    tasks[cnt] = Task.Run(() =>
                      {
                          try
                          {
                              string cityId = dct["id"].ToString();
                              string cityName = dct["name"].ToString();
                              string path = AppDomain.CurrentDomain.BaseDirectory + Common.FileUploadDirectory + "historicalData/" + DateTime.Now.Day + "." + DateTime.Now.Month + "." + DateTime.Now.Year + "/";
                              string fileName = cityId + "_" + cityName + "_" + DateTime.Now.Hour + "." + DateTime.Now.Minute + "." + DateTime.Now.Second + ".json";
                              if (System.IO.File.Exists(Path.Combine(path, fileName)))
                              {
                                 //throw new Exception("file location already exists");
                             }
                              if (!Directory.Exists(path))
                              {
                                  Directory.CreateDirectory(path);
                              }
                              if (!File.Exists(Path.Combine(path, fileName)))
                                  File.WriteAllText(Path.Combine(path, fileName), json);
                          }
                          catch (IOException iox)
                          {
                              
                          }
                      });
                    cnt++;
                }
                await Task.WhenAll(tasks);
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
                throw;
            }
            return success;
        }

    }
}