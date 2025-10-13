using Microsoft.EntityFrameworkCore;
using WeatherApp.Models;

namespace WeatherApp.Data.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<WeatherData> WeatherDatas { get; set; }
    }
}
