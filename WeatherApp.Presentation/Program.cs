using Business.Services;
using Microsoft.EntityFrameworkCore;
using WeatherApp.Business.Interfaces;
using WeatherApp.Business.Services;
using WeatherApp.Data.Context;
using WeatherApp.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
string connectionString;
// Railway'in otomatik olarak sa�lad��� DATABASE_URL de�i�kenini oku.
var dbUrl = builder.Configuration["DATABASE_URL"];

if (!string.IsNullOrEmpty(dbUrl))
{
    // E�er dbUrl varsa, bu Railway ortam�d�r. Adresi ayr��t�r�p do�ru formata �evir.
    var uri = new Uri(dbUrl);
    var userInfo = uri.UserInfo.Split(':');

    var dbHost = uri.Host;
    var dbPort = uri.Port.ToString();
    var dbUser = userInfo[0];
    var dbPass = userInfo[1];
    var dbName = uri.AbsolutePath.TrimStart('/');

    // Npgsql'in anlayaca��, anahtar-de�er format�ndaki ba�lant� dizesini olu�tur.
    // SSL Mode=Require ve Trust Server Certificate=true cloud veritabanlar� i�in �nemlidir.
    connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPass};SSL Mode=Require;Trust Server Certificate=true;";
}
else
{
    // E�er dbUrl yoksa, bu yerel (local) ortamd�r. appsettings.json'dan oku.
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
}

// Olu�turulan ba�lant� dizesini (connectionString) kullanarak DbContext'i yap�land�r.
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
