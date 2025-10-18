using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Models;

namespace WeatherApp.Business
{
    public class WeatherDaily
    {
        public DateTime Date { get; set; }        // Günün tarihi
        public double MinTemp { get; set; }       // Günün en düşük sıcaklığı
        public double MaxTemp { get; set; }       // Günün en yüksek sıcaklığı
        public string Condition { get; set; }     // Açıklama (örnek: "parçalı bulutlu")
        public string Icon { get; set; }
        public int WeatherDataId { get; set; }       // Foreign key
        public WeatherData WeatherData { get; set; }// OpenWeatherMap ikon kodu (örnek: "03d")
    }
}

