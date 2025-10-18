using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Business.DTOs;

namespace WeatherApp.Business.Interfaces
{
    public interface IWeatherService
    {

        Task<WeatherDTO> GetCurrentWeatherAsync(string city);
        Task<List<WeatherDTO>> GetHourlyWeatherAsync(string city);
        Task<List<WeatherDTO>> GetForecastAsync(string city, int days);
        Task<List<WeatherDailyDTO>> GetDailyWeatherAsync (string city, int days);

    }
}
