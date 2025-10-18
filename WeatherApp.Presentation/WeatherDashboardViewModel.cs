using WeatherApp.Business.DTOs;

namespace WeatherApp.Presentation
{
    public class WeatherDashboardViewModel
    {
        // 1. Anlık hava durumu için 
        public WeatherDTO CurrentWeather { get; set; }

        // 2. 7 günlük tahmin listesi için 
        public List<WeatherDailyDTO> DailyForecast { get; set; }

    
        public WeatherDashboardViewModel()
        {
            CurrentWeather = new WeatherDTO();
            DailyForecast = new List<WeatherDailyDTO>();
        }
    }
}
