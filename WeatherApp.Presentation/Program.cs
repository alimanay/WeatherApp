using Business.Services;
using Microsoft.EntityFrameworkCore;
using WeatherApp.Business.Interfaces;
using WeatherApp.Business.Services;
using WeatherApp.Data.Context;
using WeatherApp.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// DbContext'i bu ba�lant� dizesiyle yap�land�r.
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
        context.Database.Migrate(); // Migration'lar� uygula
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
