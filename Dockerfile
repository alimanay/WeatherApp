# ADIM 1: Kodu Derlemek İçin .NET SDK'sını Kullan
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Bütün proje dosyalarını kopyala
COPY . .

# NuGet paketlerini geri yükle.
# Bu komut, projenin ana dizininde (.sln dosyasının olduğu yerde) çalıştırılır.
RUN dotnet restore "WeatherApp.Presentation.sln"

# Ana web projesini yayınla (publish et).
# Hangi projenin yayınlanacağını tam yoluyla belirtiyoruz.
RUN dotnet publish "WeatherApp.Presentation/WeatherApp.Presentation.csproj" -c Release -o /app/publish --no-restore

# ADIM 2: Uygulamayı Çalıştırmak İçin Daha Küçük Olan .NET Runtime'ı Kullan
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app

# Derlenmiş dosyaları build aşamasından kopyala
COPY --from=build /app/publish .

# Uygulamanın çalışacağı portu belirt (Railway bunu otomatik yönetir)
ENV PORT 8080
EXPOSE 8080

# Uygulamayı başlat
ENTRYPOINT ["dotnet", "WeatherApp.Presentation.dll"]

