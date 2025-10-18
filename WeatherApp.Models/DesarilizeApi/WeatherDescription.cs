using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WeatherApp.Models.DesarilizeApi
{
    public class WeatherDescription
    {
        // "description" (örn: "açık gökyüzü")
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        // "icon" (örn: "01d")
        [JsonPropertyName("icon")]
        public string Icon { get; set; } = string.Empty;
    }
}
