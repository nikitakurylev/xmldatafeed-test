using xmldatafeed.Domain.Entities;

namespace xmldatafeed.Abstractions.Core;

public interface IWebsiteParser
{
    List<Website> ParseWebsites(IEnumerable<string> url);
}