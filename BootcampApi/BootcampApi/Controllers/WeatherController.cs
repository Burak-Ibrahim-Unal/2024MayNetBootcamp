using Bootcamp.Service.Weather;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BootcampApi.Controllers
{
    public class WeatherController(IWeatherService _weatherService) : CustomBaseController
    {
        [Authorize]
        [HttpGet]
        public IActionResult GetWeather(string city)
        {
            var weather = _weatherService.GetWeather(city);

            return Ok(weather);
        }
    }
}