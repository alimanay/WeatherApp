using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WeatherApp.Business.DTOs;
using WeatherApp.Business.Interfaces;

namespace WeatherApp.Controllers
{
    public class WeatherController : Controller
    {
        private readonly IWeatherService _weatherService;
        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new WeatherDTO());
        }
        public async Task<IActionResult> Index(string city)
        {
         

            WeatherDTO weather = await _weatherService.GetCurrentWeatherAsync(city);

            // weather null olabilir mi? Kontrol et
            if (weather == null)
            {
                return View("Error"); // Veya boş bir model döndür
            }

            return View(weather);

        }


    }
}

