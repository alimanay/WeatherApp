# WeatherApp

WeatherApp, OpenWeatherMap API kullanarak hava durumu verilerini gösteren bir C# .NET 7.0 MVC uygulamasıdır. Bu proje PostgreSQL veritabanı ile çalışır ve hassas bilgiler (API key ve DB şifre) GitHub’da paylaşılmaz.

---

## İçindekiler

1. [Özellikler](#özellikler)  
2. [Gereksinimler](#gereksinimler)  
3. [Kurulum](#kurulum)  
4. [API Key ve Veritabanı Bağlantısı](#api-key-ve-veritabanı-bağlantısı)  
5. [Projeyi Çalıştırma](#projeyi-çalıştırma)  
6. [Önemli Notlar](#önemli-notlar)  
7. [Katkıda Bulunma](#katkıda-bulunma)  

---

## Özellikler

- Güncel hava durumu verilerini görüntüleme  
- Saatlik ve günlük tahminler  
- PostgreSQL veritabanı ile verilerin kaydedilmesi  
- MVC yapısı ve katmanlı mimari kullanımı  
- Hassas bilgilerin güvenli yönetimi  

---

## Gereksinimler

- [.NET 7.0 SDK veya üzeri](https://dotnet.microsoft.com/download/dotnet/7.0)  
- [PostgreSQL](https://www.postgresql.org/download/)  
- Visual Studio 2022 veya başka bir IDE (isteğe bağlı)

---

## Kurulum

1. Repo’yu klonlayın:

```bash
git clone https://github.com/alimanay/WeatherApp.git
cd WeatherApp
Gerekli NuGet paketlerini yükleyin:
bash
dotnet restore
API Key ve Veritabanı Bağlantısı
Proje hassas bilgileri GitHub’da tutmaz. Kullanıcı olarak kendi API key ve DB bilgilerinizi environment variable olarak ayarlamalısınız.

Windows
PowerShell veya Komut İstemi açın.

Aşağıdaki komutları çalıştırın:

powershell

setx WEATHERAPI_KEY "KENDI_API_KEYINIZ"
setx WEATHERAPP_DB "Host=localhost;Port=5432;Database=WeatherAppDb;Username=postgres;Password=SIFRENIZ"
Visual Studio veya terminali yeniden başlatmayı unutmayın.

Linux / Mac
bash
Kodu kopyala
export WEATHERAPI_KEY="KENDI_API_KEYINIZ"
export WEATHERAPP_DB="Host=localhost;Port=5432;Database=WeatherAppDb;Username=postgres;Password=SIFRENIZ"
Projeyi Çalıştırma
Projeyi build edip çalıştırmak için:

bash

dotnet build
dotnet run --project WeatherApp.Presentation



Katkıda Bulunma

Forklayın, branch açın ve değişikliklerinizi gönderin.

Pull request kabul edilir ve projeye katkı sağlayabilirsiniz.
