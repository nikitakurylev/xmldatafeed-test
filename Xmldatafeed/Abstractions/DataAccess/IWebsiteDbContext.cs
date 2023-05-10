using Microsoft.EntityFrameworkCore;
using Xmldatafeed.Entities;

namespace Xmldatafeed.Abstractions.DataAccess;

public interface IWebsiteDbContext
{
    DbSet<Website> Websites { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}