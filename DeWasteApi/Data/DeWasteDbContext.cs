using Microsoft.EntityFrameworkCore;
using DeWasteApi.Models;
using DeWasteApi.Interceptors;

namespace DeWasteApi.Data
{
    public class DeWasteDbContext : DbContext
    {
        public DeWasteDbContext(DbContextOptions<DeWasteDbContext> options)
        {
           
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.AddInterceptors(new TimeLogInterceptor());

            optionsBuilder.UseNpgsql("Server=193.219.91.103;Port=1353;Database=dewasted;User Id=postgres;Password=cisco;");

        }




        public DbSet<Category> category { get; set; }
        public DbSet<Item> item { get; set; }
        public DbSet<Item_Category> item_category { get; set; }
        public DbSet<Comment> comment { get; set; } = default!;
        public DbSet<Rating> rating { get; set; }
    }

    public class MyLoggerFactory
    {
        private readonly ILoggerFactory _loggerFactory;

        public MyLoggerFactory()
        {
            _loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("DeWaste", LogLevel.Debug)
                    .AddConsole();
            });
        }

        public ILoggerFactory GetLoggerFactory()
        {
            return _loggerFactory;
        }
    }
}
