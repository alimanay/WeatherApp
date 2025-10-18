using Business.Services;
using Microsoft.EntityFrameworkCore;
using WeatherApp.Business.Interfaces;
using WeatherApp.Business.Services;
using WeatherApp.Data.Context;
using WeatherApp.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// DbContext'i bu baðlantý dizesiyle yapýlandýr.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        connectionString,
        b => b.MigrationsAssembly("WeatherApp.Data")
    ));
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<WeatherRepository>();
builder.Services.AddScoped<IWeatherService,WeatherService>();
builder.Services.AddHttpClient<WeatherService>();
builder.Services.AddControllersWithViews();


var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.Migrate(); // Migration'larý uygula
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
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
