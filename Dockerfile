# ADIM 1: Kodu Derlemek İçin .NET SDK'sını Kullan
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Proje dosyalarını kopyala (.csproj ve .sln)
COPY *.sln .
COPY WeatherApp.Presentation/*.csproj ./WeatherApp.Presentation/
COPY WeatherApp.Business/*.csproj ./WeatherApp.Business/
COPY WeatherApp.Data/*.csproj ./WeatherApp.Data/
COPY WeatherApp.Models/*.csproj ./WeatherApp.Models/

# NuGet paketlerini geri yükle
RUN dotnet restore

# Tüm proje dosyalarını kopyala
COPY . .

# Projeyi yayınla (publish et)
WORKDIR "/app/WeatherApp.Presentation"
RUN dotnet publish -c Release -o /app/publish

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

