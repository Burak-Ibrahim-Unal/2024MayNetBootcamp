using Bootcamp.Service.SharedDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.Service.Weather
{
    public interface IWeatherService
    {
        ResponseModelDto<int> GetWeather(string city);
    }
}
