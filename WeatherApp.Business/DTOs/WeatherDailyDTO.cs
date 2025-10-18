using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Business.DTOs
{
    public  class WeatherDailyDTO
    {
        public string City { get; set; }    
        public DateTime Date { get; set; }
        public double MinTemp { get; set; }
        public double MaxTemp { get; set; }
        public string Condition { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
    }
}
