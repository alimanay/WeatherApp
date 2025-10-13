using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WeatherApp.Business.DTOs;
using WeatherApp.Business.Interfaces;
using WeatherApp.Data.Repositories;
using WeatherApp.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WeatherApp.Business.Services
{
    public class WeatherService : IWeatherService
    {
        WeatherRepository _weatherRepository;
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl;
        public WeatherService( WeatherRepository weatherRepository,HttpClient httpClient, IConfiguration configuration) { 
            _weatherRepository = weatherRepository;
            _httpClient = httpClient;
            _apiKey = configuration["WeatherApi:ApiKey"];
            _baseUrl = configuration["WeatherApi:BaseUrl"];
        
        }

        public async Task<WeatherDTO> GetCurrentWeatherAsync(string city)
        {
            var url = $"{_baseUrl}weather?q={city}&appid={_apiKey}&units=metric";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var cityName = root.GetProperty("name").GetString().Trim();

            // WeatherData nesnesi oluştur
            var data = new WeatherData
            {
                City = cityName,
                Temperature = root.GetProperty("main").GetProperty("temp").GetDouble(),
                Humidity = root.GetProperty("main").GetProperty("humidity").GetInt32(),
                WindSpeed = root.GetProperty("wind").GetProperty("speed").GetDouble(),
                Condition = root.GetProperty("weather")[0].GetProperty("description").GetString(),
                Date = DateTime.UtcNow
            };

            // Repository zaten ekleme veya güncelleme yapacak
            await _weatherRepository.AddOrUpdateWeatherDataAsync(data);

            // DTO olarak döndür
            return new WeatherDTO
            {
                City = data.City,
                Temperature = data.Temperature,
                Humidity = data.Humidity,
                WindSpeed = data.WindSpeed,
                Condition = data.Condition,
                Date = data.Date
            };
        }

        public async Task<List<WeatherDTO>> GetForecastAsync(string city, int days)
        {
            var forecastData = await _weatherRepository.GetForecastAsync(city, days);
            return forecastData.Select(data => new WeatherDTO
            {
                City = data.City,
                Temperature = data.Temperature,
                Condition = data.Condition,
                Date = data.Date
               
            }).ToList();

        }

        public async Task<List<WeatherDTO>> GetHourlyWeatherAsync(string city)
        {
            var HourlyWeather = await _weatherRepository.GetHourlyWeatherAsync(city);
            return HourlyWeather.Select(data => new WeatherDTO
            {
                City = data.City,
                Temperature = data.Temperature,
                Condition = data.Condition,
                Date = data.Date
                

            }).ToList();
            
        }
    }
}
