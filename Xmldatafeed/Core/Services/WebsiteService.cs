using Xmldatafeed.Abstractions.Core;
using Xmldatafeed.Abstractions.DataAccess;
using Xmldatafeed.Core.Extensions;

namespace Xmldatafeed.Core.Services;

public class WebsiteService : IWebsiteService
{
    private const int BatchSize = 1000;
    private readonly IWebsiteParser _websiteParser;
    private readonly IWebsiteDbContext _websiteDbContext;
    private readonly IUrlProvider _urlProvider;

    public WebsiteService(IWebsiteParser websiteParser, IWebsiteDbContext websiteDbContext, IUrlProvider urlProvider)
    {
        _websiteParser = websiteParser;
        _websiteDbContext = websiteDbContext;
        _urlProvider = urlProvider;
    }

    public async Task ParseAndSaveWebsites()
    {
        var urlQueue = new Queue<string>(_urlProvider.GetUrls());

        while (urlQueue.Any())
        {
            var websites = _websiteParser.ParseWebsites(urlQueue.DequeueChunk(BatchSize).ToArray());
            await _websiteDbContext.Websites.AddRangeAsync(websites);
            await _websiteDbContext.SaveChangesAsync();
        }
    }
}