using Business.Services;
using Microsoft.EntityFrameworkCore;
using WeatherApp.Business.Interfaces;
using WeatherApp.Business.Services;
using WeatherApp.Data.Context;
using WeatherApp.Data.Repositories;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// =================================================================
// VER�TABANI BA�LANTISI - SADELE�T�R�LM�� VE DO�RU HAL�
// =================================================================
// Bu tek sat�r, hem kendi bilgisayar�n�zda (appsettings.Development.json'dan)
// hem de Railway'de (ortam de�i�kenlerinden) do�ru �ekilde �al��acakt�r.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// DbContext'i bu ba�lant� dizesiyle yap�land�r.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        connectionString,
        b => b.MigrationsAssembly("WeatherApp.Data")
    ));

// =================================================================
// SERV�S VE REPOSITORY KAYITLARI
// =================================================================
builder.Services.AddScoped<UserRepository>();

builder.Services.AddScoped<UserService>();

builder.Services.AddScoped<WeatherRepository>();

builder.Services.AddScoped<IWeatherService, WeatherService>();

builder.Services.AddHttpClient<WeatherService>();

builder.Services.AddControllersWithViews();
// IWeatherService isteyen birine WeatherService ver ve ona bir HttpClient sa�la.
builder.Services.AddHttpClient<IWeatherService, WeatherService>();

var app = builder.Build();

// =================================================================
// OTOMAT�K VER�TABANI MIGRATION
// Bu kod, uygulama her ba�lad���nda Railway'deki bo� veritaban�na
// tablolar�n�z� otomatik olarak olu�turacakt�r.
// =================================================================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        // Veritaban� yoksa olu�turur ve migration'lar� uygular.
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        // Migration s�ras�nda bir hata olursa bunu log'layabiliriz.
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Veritaban� migration'� s�ras�nda bir hata olu�tu.");
        // Bu a�amada program�n durmas� genellikle istenen bir durumdur,
        // ��nk� veritaban� olmadan uygulama d�zg�n �al��amaz.
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Weather}/{action=Index}/{id?}");

app.Run();

