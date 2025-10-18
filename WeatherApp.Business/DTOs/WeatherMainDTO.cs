using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Business.DTOs
{
    public class WeatherMainDTO
{
    // 1. Anlık Hava Durumu (Sizin Mevcut Veriniz)
    public WeatherDTO CurrentWeather { get; set; } = new WeatherDTO();

    // 2. Haftalık Tahmin (Listeniz)
    public List<WeatherDailyDTO> DailyForecasts { get; set; } = new List<WeatherDailyDTO>();

    // Aranan Şehir (Formdan gelen değeri tutmak için)
    public string City { get; set; } = string.Empty;

    // Hata mesajı (ViewBag yerine model içinde taşımak daha iyi bir pratiktir)
    public string ErrorMessage { get; set; } = string.Empty;
}
}
