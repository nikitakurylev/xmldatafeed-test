using Microsoft.EntityFrameworkCore;
using xmldatafeed.Abstractions.DataAccess;
using xmldatafeed.Domain.Entities;

namespace xmldatafeed.DataAccess;

public class WebsiteDbContext : DbContext, IWebsiteDbContext
{
    private string _connectionString;
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