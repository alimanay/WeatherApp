
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src


COPY ["WeatherApp.Presentation.sln", "./"]
COPY ["WeatherApp.Business/WeatherApp.Business.csproj", "WeatherApp.Business/"]
COPY ["WeatherApp.Data/WeatherApp.Data.csproj", "WeatherApp.Data/"]
COPY ["WeatherApp.Models/WeatherApp.Models.csproj", "WeatherApp.Models/"]
COPY ["WeatherApp.Presentation/WeatherApp.Presentation.csproj", "WeatherApp.Presentation/"]


RUN dotnet restore "WeatherApp.Presentation.sln"


COPY . .


RUN dotnet publish "WeatherApp.Presentation/WeatherApp.Presentation.csproj" -c Release -o /app/publish --no-restore


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app


COPY --from=build /app/publish .


ENV PORT 8080
EXPOSE 8080


ENTRYPOINT ["dotnet", "WeatherApp.Presentation.dll"]
```



