using Xmldatafeed.Entities;

namespace Xmldatafeed.Abstractions.Core;

public interface IWebsiteParser
{
    IEnumerable<Website> ParseWebsites(IEnumerable<string> url);
}