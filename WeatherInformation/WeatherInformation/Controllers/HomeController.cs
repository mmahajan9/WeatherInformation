using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WeatherInformation.BusinessLayer;

namespace WeatherInformation.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult WeatherInfo()
        {
            return View();
        }

        /// <summary>
        /// This method uploads a file to the base directory and then fetches data for every city 
        /// and stores output to individual file according to city id
        /// </summary>
        /// <param name="isFinalChunk">This flag notifies if the file data has it's last chunk</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> UploadFile(bool isFinalChunk=true)
        {
            bool fileUploadSuccess = false;
            string path = AppDomain.CurrentDomain.BaseDirectory + Common.FileUploadDirectory;
            string fileName = Request.Files.AllKeys[0];
            // Saves this file to blob
            fileUploadSuccess = FileManager.SaveFileToBlob(path, fileName, Request.Files[0]);
            // If this is a final chunk 
            // - then read the file. 
            // - Get all the city ids. 
            // - Get data for each city 
            // - and store it in individual file.
            if (isFinalChunk)
                await FileManager.StoreCityWiseOutputForHistoryAsync(Path.Combine(path, fileName));
            return true;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}