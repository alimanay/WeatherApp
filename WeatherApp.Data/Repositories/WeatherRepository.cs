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
        

        public async Task<List<WeatherData>> GetForecastAsync(string city, int days)
        {
            return await _context.WeatherDatas.Where(a => a.City == city).OrderByDescending(b => b.Date).Take(days).ToListAsync();
        }
       
       

        public async Task<List<WeatherData>> GetHourlyWeatherAsync(string city)
        {
            return await _context.WeatherDatas.Where(z => z.City == city).OrderByDescending(a => a.Date).Take(24).ToListAsync();
        }

        public async Task<List<WeatherDaily>> GetWeatherDailies(string city,int days)
        {
            var today = DateTime.Today;

            return await _context.WeatherDailies
                // Belirtilen şehirdeki, tarihi bugünden büyük veya eşit olan kayıtları filtrele.
                .Where(a => a.City == city && a.Date.Date >= today)
                .OrderBy(a => a.Date) // Tarih sırasına göre artan (eskiden yeniye) sırala.
                .Take(days)           // Sadece istenen gün sayısı kadarını al.
                .ToListAsync();
        }
        public async Task AddOrUpdateWeatherDailyAsync(WeatherDaily weatherDaily)
        {
            // Bu yüzden kontrolü sadece şehre değil, ŞEHİR ve TARIH'e göre yapmalıyız.
            var date = weatherDaily.Date.Date; // Sadece tarih kısmını alıyoruz (saat önemsiz).
            var cityName = weatherDaily.City;

            // Belirtilen tarih ve şehir için mevcut bir kayıt var mı kontrol et.
            var existing = await _context.WeatherDailies
                .FirstOrDefaultAsync(a => a.City == cityName && a.Date.Date == date);

            if (existing == null)
            {
                // Eğer mevcut kayıt YOKSA: Yeni kayıt ekle (Insert)
                // NOT: weatherDaily.Date'i değiştirmemelisiniz, API'den gelen gerçek tahmin tarihi olmalı.
                await _context.WeatherDailies.AddAsync(weatherDaily);
            }
            else
            {
                // Eğer mevcut kayıt VARSA: Verileri güncelle (Update)
                // Existing (Mevcut) nesneyi yeni verilerle güncelliyoruz.
                existing.Condition = weatherDaily.Condition;
                existing.MinTemp = weatherDaily.MinTemp;
                existing.MaxTemp = weatherDaily.MaxTemp;
                existing.Icon = weatherDaily.Icon;

                // Date alanını güncellemeye gerek yok çünkü eşleşme zaten bu alana göre yapıldı.
                // _context.WeatherDailies.Update(existing); // EF Core, 'existing' nesnesi zaten takip edildiği için bu satırı gerektirmez.
            }

            // Tüm değişiklikleri veritabanına kaydet.
            await _context.SaveChangesAsync();

        }

        public async Task<WeatherData> GetWeatherDataByCityAsync(string city)
        {
            var cityName = city.Trim();

            // Büyük/küçük harf duyarsız arama
            return await _context.WeatherDatas
                .FirstOrDefaultAsync(w => EF.Functions.ILike(w.City.Trim(), cityName));
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