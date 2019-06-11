using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeatherInformation.BusinessLayer
{
    public static class Factory
    {
        public static IFileReaderStrategy GetStrategy(string fileType)
        {
            IFileReaderStrategy factory;
            switch (fileType)
            {
                case "json":
                    factory = new JSONFileReader();
                    break;
                case "xlsx": case "xlx":
                    factory = new ExcelFileReader();
                    break;
                default:
                    factory = new JSONFileReader();
                    break;
            }
            return factory;
        }
    }
}