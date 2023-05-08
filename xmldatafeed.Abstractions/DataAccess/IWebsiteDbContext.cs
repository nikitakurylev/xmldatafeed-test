using Microsoft.EntityFrameworkCore;
using xmldatafeed.Domain.Entities;

namespace xmldatafeed.Abstractions.DataAccess;

public interface IWebsiteDbContext
{
    DbSet<Website> Websites { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}