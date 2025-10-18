using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WeatherApp.Models.DesarilizeApi
{
    public class DailyForecast
    {
        // "dt" (Unix timestamp - saniye cinsinden uzun bir sayı)
        [JsonPropertyName("dt")]
        public long Dt { get; set; }

        // "temp" { ... } objesi için
        [JsonPropertyName("temp")]
        public Temperature Temp { get; set; } = new();

        // "weather" [ ... ] dizisi için
        [JsonPropertyName("weather")]
        public List<WeatherDescription> Weather { get; set; } = new();

        // --- YARDIMCI METOD ---
        // Bu metod, "dt" (Unix timestamp) değerini normal bir 
        // DateTime objesine dönüştürür. Servis katmanında işimizi çok kolaylaştırır.
        public DateTime GetDateTime()
        {
            return DateTimeOffset.FromUnixTimeSeconds(Dt).DateTime;
        }
    }
}
