using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Models;

namespace WeatherApp.Business.DTOs
{
    public class WeatherDTO
    {
        public string City { get; set; }
        public string Condition { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public double WindSpeed { get; set; }
        public DateTime Date { get; set; }

        public List<WeatherDailyDTO> DailyForecast { get; set; }
    }
}
   
