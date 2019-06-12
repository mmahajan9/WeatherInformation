using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WeatherInformation.Entity;

namespace WeatherInformation.BusinessLayer
{
    public class ExcelFileReader : IFileReaderStrategy
    {
        public IEnumerable<int> ReadCityIds(string filePath)
        {
            throw new NotImplementedException();
        }

        Task<string> IFileReaderStrategy.GetWetherInfoFromCityIds(List<int> cityIds)
        {
            throw new NotImplementedException();
        }
    }
}