using Microsoft.EntityFrameworkCore;
using WeatherApp.Data.Context;
using WeatherApp.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WeatherApp.Data.Repositories
{
    public class WeatherRepository
    {
        private readonly AppDbContext _context;

        public WeatherRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddWeatherDataAsync(WeatherData data)
        {
            _context.WeatherDatas.Add(data);
            await _context.SaveChangesAsync();
        }
        public async Task<WeatherData> GetCurrentWeatherAsync(string city)
        {
            return await _context.WeatherDatas.FirstOrDefaultAsync(x => x.City == city);
        }

        public async Task<List<WeatherData>> GetForecastAsync(string city, int days)
        {
            return await _context.WeatherDatas.Where(a => a.City == city).OrderByDescending(b => b.Date).Take(days).ToListAsync();
        }
        public async Task<List<WeatherData>> GetHourlyWeatherAsync(string city)
        {
            return await _context.WeatherDatas.Where(z => z.City == city).OrderByDescending(a => a.Date).Take(24).ToListAsync();
        }
        public async Task AddOrUpdateWeatherDataAsync(WeatherData weatherData)
        {
            var cityName = weatherData.City.Trim();

            // Case-insensitive PostgreSQL kontrol
            var existing = await _context.WeatherDatas
                .FirstOrDefaultAsync(a => EF.Functions.ILike(a.City.Trim(), cityName));

            if (existing != null)
            {
                // Mevcut kayıt varsa sadece güncelle
                existing.Temperature = weatherData.Temperature;
                existing.Humidity = weatherData.Humidity;
                existing.WindSpeed = weatherData.WindSpeed;
                existing.Condition = weatherData.Condition;
                existing.Date = DateTime.UtcNow;

                _context.WeatherDatas.Update(existing);
            }
            else
            {
                // Yoksa yeni ekle
                weatherData.Date = DateTime.UtcNow;
                await _context.WeatherDatas.AddAsync(weatherData);
            }

            await _context.SaveChangesAsync();
        }
    }
}