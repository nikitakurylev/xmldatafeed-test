using xmldatafeed.Domain.Entities;

namespace xmldatafeed.Abstractions.Core;

public interface IWebsiteParser
{
    Task<List<Website>> ParseWebsites(string[] url);
}