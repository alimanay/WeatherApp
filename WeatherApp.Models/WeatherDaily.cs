using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Models
{
    public class WeatherDaily
    {
        public int Id { get; set; }                // (veritabanı için opsiyonel)
        public string City { get; set; } = string.Empty;
        public DateTime Date { get; set; }         // Günün tarihi
        public double MinTemp { get; set; }        // Günün minimum sıcaklığı
        public double MaxTemp { get; set; }        // Günün maksimum sıcaklığı
        public string Condition { get; set; } = string.Empty; // Açıklama (örnek: "açık", "parçalı bulutlu")
        public string Icon { get; set; } = string.Empty;
        public int WeatherDataId { get; set; }       // Foreign key
        public WeatherData WeatherData { get; set; } ///
    }
}
