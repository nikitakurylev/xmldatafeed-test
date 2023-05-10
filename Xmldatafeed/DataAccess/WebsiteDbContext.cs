using Microsoft.EntityFrameworkCore;
using Xmldatafeed.Abstractions.DataAccess;
using Xmldatafeed.Entities;

namespace Xmldatafeed.DataAccess;

public class WebsiteDbContext : DbContext, IWebsiteDbContext
{
    private readonly string _connectionString;
    public DbSet<Website> Websites { get; set; }

    public WebsiteDbContext(string connectionString)
    {
        _connectionString = connectionString;
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql(
            _connectionString,
            new MySqlServerVersion(new Version(8, 0, 33))
        );
    }
}