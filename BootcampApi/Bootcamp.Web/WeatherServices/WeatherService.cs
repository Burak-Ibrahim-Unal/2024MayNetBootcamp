using System.Net;
using System.Net.Http.Headers;
using Bootcamp.Web.Models;
using Bootcamp.Web.TokenServices;

namespace Bootcamp.Web.WeatherServices
{
    public class WeatherService(HttpClient _httpClient, TokenService _tokenService, ILogger<WeatherService> _logger)
    {
        public async Task<ServiceResponseModel<int>> GetWeatherForecastWithCity(string cityName)
        {
            var response = await _httpClient.GetAsync($"/api/Weather?city={cityName}");

            var responseAsBody = await response.Content.ReadFromJsonAsync<ResponseModelDto<int>>();

            if (!response.IsSuccessStatusCode)
            {
                return ServiceResponseModel<int>.Fail(responseAsBody!.FailMessages);
            }

            return ServiceResponseModel<int>.Success(responseAsBody!.Data);
        }

        public async Task<string> GetWeatherForecastWithCityBetter(string cityName)
        {
            var response = await _httpClient.GetAsync($"/api/Weather?city={cityName}");

            var responseAsBody = await response.Content.ReadFromJsonAsync<ResponseModelDto<int>>();

            if (!response.IsSuccessStatusCode)
            {
                responseAsBody!.FailMessages!.ForEach(x => { _logger.LogError(x); });
                //loglama yapılacak

                return "sıcaklık bilgisi alınamadı.";
            }

            return responseAsBody!.Data.ToString();
        }
    }
}