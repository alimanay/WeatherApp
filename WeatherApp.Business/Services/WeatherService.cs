using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq; // LINQ için gerekli
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using WeatherApp.Business.DTOs;
using WeatherApp.Business.Interfaces;
using WeatherApp.Data.Repositories;
using WeatherApp.Models;
using WeatherApp.Models.DesarilizeApi;

namespace WeatherApp.Business.Services
{
    public class WeatherService : IWeatherService
    {
        WeatherRepository _weatherRepository;
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl;


        public WeatherService(WeatherRepository weatherRepository, HttpClient httpClient, IConfiguration configuration)
        {
            _weatherRepository = weatherRepository;
            _httpClient = httpClient;
            _apiKey = configuration["WeatherApi:ApiKey"];
            _baseUrl = configuration["WeatherApi:BaseUrl"];
   
        }

        // =================================================================
        // ANLIK HAVA DURUMU (CURRENT)
        // =================================================================
        public async Task<WeatherDTO> GetCurrentWeatherAsync(string city)
        {
            var url = $"{_baseUrl}weather?q={city}&appid={_apiKey}&units=metric";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode) return null; // Başarısız olursa null döndür

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;


            var cityName = root.GetProperty("name").GetString().Trim();
            var data = new WeatherData
            {
                City = cityName,
                Temperature = root.GetProperty("main").GetProperty("temp").GetDouble(),
                Humidity = root.GetProperty("main").GetProperty("humidity").GetInt32(),
                WindSpeed = root.GetProperty("wind").GetProperty("speed").GetDouble(),
                Condition = root.GetProperty("weather")[0].GetProperty("description").GetString(),
                Date = DateTime.UtcNow
            };

            await _weatherRepository.AddOrUpdateWeatherDataAsync(data);

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

       
        public async Task<List<WeatherDTO>> GetHourlyWeatherAsync(string city)
        {
            // Bu metot, sizin ilk yazdığınız mantık gibi sadece Repository'den çekme işini yapar.
            var HourlyWeather = await _weatherRepository.GetHourlyWeatherAsync(city);
            return HourlyWeather.Select(data => new WeatherDTO
            {
                City = data.City,
                Temperature = data.Temperature,
                Condition = data.Condition,
                Date = data.Date
            }).ToList();
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
        public async Task<List<WeatherDailyDTO>> GetDailyWeatherAsync(string city, int days)
        {
            try
            {
                var fullUrl = $"{_baseUrl}forecast?q={city}&appid={_apiKey}&units=metric";
                var apiResponse = await _httpClient.GetFromJsonAsync<ForecastApiResponse>(fullUrl);

                if (apiResponse == null || apiResponse.List == null || !apiResponse.List.Any())
                {
                    return new List<WeatherDailyDTO>();
                }

                var dailyForecasts = apiResponse.List
                    .GroupBy(item => DateTime.Parse(item.DtTxt).Date)
                    .Select(group => new WeatherDailyDTO
                    {
                        City = city,
                        Date = DateTime.SpecifyKind(group.Key, DateTimeKind.Utc),
                        MinTemp = group.Min(item => item.Main.TempMin),
                        MaxTemp = group.Max(item => item.Main.TempMax),
                        Condition = group.FirstOrDefault(item => DateTime.Parse(item.DtTxt).Hour == 12)?.Weather.FirstOrDefault()?.Description ?? group.First().Weather.First().Description,
                        Icon = group.FirstOrDefault(item => DateTime.Parse(item.DtTxt).Hour == 12)?.Weather.FirstOrDefault()?.Icon ?? group.First().Weather.First().Icon
                    })
                    .Take(days)
                    .ToList();

               
                var parentWeatherData = await _weatherRepository.GetWeatherDataByCityAsync(city);
                if (parentWeatherData == null)
                {
                    Console.WriteLine($"KRİTİK HATA: {city} için ana WeatherData kaydı bulunamadı. Günlük tahminler kaydedilemedi.");
                    return dailyForecasts; // Veriyi yine de göster ama kaydetme.
                }

                // ===== 2. ADIM: Her bir günlük tahmini, ana kaydın ID'si ile kaydet =====
                foreach (var dailyDto in dailyForecasts)
                {
                    var entity = new WeatherDaily
                    {
                        City = dailyDto.City,
                        Date = dailyDto.Date,
                        MinTemp = dailyDto.MinTemp,
                        MaxTemp = dailyDto.MaxTemp,
                        Condition = dailyDto.Condition,
                        Icon = dailyDto.Icon,
                        // Yabancı anahtarı (Foreign Key) burada atıyoruz.
                        WeatherDataId = parentWeatherData.Id
                    };
                    await _weatherRepository.AddOrUpdateWeatherDailyAsync(entity);
                }

                return dailyForecasts;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"!!!! KRİTİK HATA: GetDailyWeatherAsync metodunda bir exception oluştu!");
                Console.WriteLine($"!!!! HATA MESAJI: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"!!!! INNER EXCEPTION: {ex.InnerException.Message}");
                }
                return new List<WeatherDailyDTO>();
            }
        }


    }
}
