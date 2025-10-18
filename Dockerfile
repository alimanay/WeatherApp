
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app


COPY . .

RUN dotnet restore "WeatherApp.Presentation/WeatherApp.Presentation.csproj"


RUN dotnet publish "WeatherApp.Presentation/WeatherApp.Presentation.csproj" -c Release -o /app/publish --no-restore


FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app


COPY --from=build /app/publish .


ENV PORT 8080
EXPOSE 8080


ENTRYPOINT ["dotnet", "WeatherApp.Presentation.dll"]

