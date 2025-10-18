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
