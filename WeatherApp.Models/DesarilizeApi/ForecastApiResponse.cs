using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WeatherApp.Models.DesarilizeApi
{
    public class ForecastApiResponse
    {
        [JsonPropertyName("list")]
        public List<ForecastItem> List { get; set; } = new();
    }

    public class ForecastItem
    {
        [JsonPropertyName("dt")]
        public long Dt { get; set; }

        [JsonPropertyName("main")]
        public MainData Main { get; set; } = new();

        [JsonPropertyName("weather")]
        public List<WeatherDescription> Weather { get; set; } = new();

        [JsonPropertyName("dt_txt")]
        public string DtTxt { get; set; } = string.Empty;
    }

    public class MainData
    {
        [JsonPropertyName("temp")]
        public double Temp { get; set; }

        [JsonPropertyName("temp_min")]
        public double TempMin { get; set; }

        [JsonPropertyName("temp_max")]
        public double TempMax { get; set; }
    }

    // WeatherDescription sınıfı aynı kalabilir, 
    // ama referans olması için buraya da ekliyorum.
    // Zaten varsa tekrar oluşturmanıza gerek yok.
    public class WeatherDescription
    {
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("icon")]
        public string Icon { get; set; } = string.Empty;
    }
}
