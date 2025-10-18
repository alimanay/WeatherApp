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
// VERÝTABANI BAÐLANTISI - SADELEÞTÝRÝLMÝÞ VE DOÐRU HALÝ
// =================================================================
// Bu tek satýr, hem kendi bilgisayarýnýzda (appsettings.Development.json'dan)
// hem de Railway'de (ortam deðiþkenlerinden) doðru þekilde çalýþacaktýr.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// DbContext'i bu baðlantý dizesiyle yapýlandýr.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        connectionString,
        b => b.MigrationsAssembly("WeatherApp.Data")
    ));

// =================================================================
// SERVÝS VE REPOSITORY KAYITLARI
// =================================================================
builder.Services.AddScoped<UserRepository>();

builder.Services.AddScoped<UserService>();

builder.Services.AddScoped<WeatherRepository>();

builder.Services.AddScoped<IWeatherService, WeatherService>();

builder.Services.AddHttpClient<WeatherService>();

builder.Services.AddControllersWithViews();
// IWeatherService isteyen birine WeatherService ver ve ona bir HttpClient saðla.
builder.Services.AddHttpClient<IWeatherService, WeatherService>();

var app = builder.Build();

// =================================================================
// OTOMATÝK VERÝTABANI MIGRATION
// Bu kod, uygulama her baþladýðýnda Railway'deki boþ veritabanýna
// tablolarýnýzý otomatik olarak oluþturacaktýr.
// =================================================================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        // Veritabaný yoksa oluþturur ve migration'larý uygular.
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        // Migration sýrasýnda bir hata olursa bunu log'layabiliriz.
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Veritabaný migration'ý sýrasýnda bir hata oluþtu.");
        // Bu aþamada programýn durmasý genellikle istenen bir durumdur,
        // çünkü veritabaný olmadan uygulama düzgün çalýþamaz.
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

