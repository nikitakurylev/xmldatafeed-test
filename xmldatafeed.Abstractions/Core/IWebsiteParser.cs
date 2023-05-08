using xmldatafeed.Domain.Entities;

namespace xmldatafeed.Abstractions.Core;

public interface IWebsiteParser
{
    List<Website> ParseWebsites(ICollection<string> url);
}