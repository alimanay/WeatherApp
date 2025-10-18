using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WeatherApp.Models.DesarilizeApi
{
    public class OneCallApiResponse
    {
        // Sadece "daily" dizisine ihtiyacımız var
        [JsonPropertyName("daily")]
        public List<DailyForecast> Daily { get; set; } = new();
    }
}
