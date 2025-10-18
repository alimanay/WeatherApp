using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WeatherApp.Models.DesarilizeApi
{
    public class GeocodingResponse
    {
        // JSON'daki "lat" alanını Lat property'sine atar
        [JsonPropertyName("lat")]
        public double Lat { get; set; }

        // JSON'daki "lon" alanını Lon property'sine atar
        [JsonPropertyName("lon")]
        public double Lon { get; set; }

        // JSON'daki "name" alanını (örn: "Istanbul") Name property'sine atar
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }
}
