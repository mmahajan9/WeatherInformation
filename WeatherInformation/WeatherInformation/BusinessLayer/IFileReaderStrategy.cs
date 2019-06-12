using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WeatherInformation.Entity;

namespace WeatherInformation.BusinessLayer
{
    public interface IFileReaderStrategy
    {
        IEnumerable<int> ReadCityIds(string filePath);
        Task<string> GetWetherInfoFromCityIds(List<int> cityIds);
    }
}
