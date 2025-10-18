using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Models;

namespace WeatherApp.Business.Interfaces
{
    public interface IOpenWeatherApiClient
    {
        // Anlık hava durumunu doğrudan API'den çeker ve ham JSON'u döndürür (veya bir API Response nesnesi).
        // Şu anki projenizdeki mantığı korumak için ham JSON'u string olarak alabiliriz,
        // ancak daha iyisi API'ye özel bir Response sınıfı döndürmektir.
        Task<string> FetchCurrentWeatherJsonAsync(string city);

        // Geocoding işlemini gerçekleştirir ve koordinatları döndürür.
        Task<(double lat, double lon)> GetCoordinatesByCityAsync(string city);

        // Koordinatları kullanarak günlük tahmin verisini çeker ve WeatherDaily Entity listesi olarak hazırlar.
        Task<List<WeatherDaily>> FetchDailyForecastsAsync(string city, double lat, double lon, int days);
    }
}
