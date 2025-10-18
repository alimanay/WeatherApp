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
// Railway'in otomatik olarak saðladýðý DATABASE_URL deðiþkenini oku.
var dbUrl = builder.Configuration["DATABASE_URL"];

if (!string.IsNullOrEmpty(dbUrl))
{
    // Eðer dbUrl varsa, bu Railway ortamýdýr. Adresi ayrýþtýrýp doðru formata çevir.
    var uri = new Uri(dbUrl);
    var userInfo = uri.UserInfo.Split(':');

    var dbHost = uri.Host;
    var dbPort = uri.Port.ToString();
    var dbUser = userInfo[0];
    var dbPass = userInfo[1];
    var dbName = uri.AbsolutePath.TrimStart('/');

    // Npgsql'in anlayacaðý, anahtar-deðer formatýndaki baðlantý dizesini oluþtur.
    // SSL Mode=Require ve Trust Server Certificate=true cloud veritabanlarý için önemlidir.
    connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPass};SSL Mode=Require;Trust Server Certificate=true;";
}
else
{
    // Eðer dbUrl yoksa, bu yerel (local) ortamdýr. appsettings.json'dan oku.
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
}

// Oluþturulan baðlantý dizesini (connectionString) kullanarak DbContext'i yapýlandýr.
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
