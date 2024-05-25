using Bootcamp.Service.Weather;
using BootcampApi.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bootcamp.Api.Weather
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