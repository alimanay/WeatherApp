using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WeatherApp.Business.DTOs;
using WeatherApp.Business.Interfaces;
using WeatherApp.Presentation;
using WeatherApp.Presentation.Models; // <-- BU SATIRI EKLEYİN

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
            // View'a artık boş WeatherDTO değil, boş ViewModel gönderiyoruz
            return View(new WeatherDashboardViewModel());
        }

        public async Task<IActionResult> Index(string city)
        {
            // 1. Ana ViewModel'i oluştur
            var viewModel = new WeatherDashboardViewModel();

            try
            {
                if (string.IsNullOrWhiteSpace(city))
                {
                    ViewBag.ErrorMessage = "Lütfen bir şehir adı giriniz.";
                    return View(viewModel); // Boş ViewModel'i döndür
                }

                // 2. Anlık veriyi çek ve ViewModel'e ata
                viewModel.CurrentWeather = await _weatherService.GetCurrentWeatherAsync(city);

                // 3. (YENİ) 7 günlük veriyi çek ve ViewModel'e ata
                viewModel.DailyForecast = await _weatherService.GetDailyWeatherAsync(city, 5); // 7 gün istiyoruz

                // İki çağrı da başarısız olursa hata ver
                if (viewModel.CurrentWeather == null && (viewModel.DailyForecast == null || !viewModel.DailyForecast.Any()))
                {
                    ViewBag.ErrorMessage = "Hava durumu bilgisi bulunamadı.";
                    return View(viewModel);
                }

                // 4. Dolu ViewModel'i View'a gönder
                return View(viewModel);
            }
            catch (Exception ex)
            {
                // Gerçek hata mesajını ViewBag'e yazdırıyoruz.
                // Bu bize sorunun kaynağını gösterecektir (örn: "Response status code does not indicate success: 401 (Unauthorized).")
                ViewBag.ErrorMessage = $"Bir hata oluştu: {ex.Message}";

                // Hatanın detaylarını görmek için Debug konsoluna da yazdırabilirsiniz.
                System.Diagnostics.Debug.WriteLine(ex.ToString());

                return View(viewModel);
            }
        }
    }
}