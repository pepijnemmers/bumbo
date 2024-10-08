using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace BumboApp.Models
{
    public class ContextFactory : IDesignTimeDbContextFactory<BumboDbContext>
    {
        public ContextFactory()
        {
        }

        private IConfiguration Configuration => new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appsettings.json")
          .Build();

        public BumboDbContext CreateDbContext(string[] args)
        {

            var builder = new DbContextOptionsBuilder<BumboDbContext>();
            builder.UseSqlServer(Configuration.GetConnectionString("FirstAndSecond"));

            return new BumboDbContext(builder.Options);
        }
    }
}
