using xmldatafeed.Abstractions.Core;
using xmldatafeed.Abstractions.DataAccess;
using xmldatafeed.Core.Extensions;
using xmldatafeed.Domain.Entities;

namespace xmldatafeed.Core.Services;

public class WebsiteService : IWebsiteService
{
    private IWebsiteParser _websiteParser;
    private IWebsiteDbContext _websiteDbContext;
    private IUrlProvider _urlProvider;

    public WebsiteService(IWebsiteParser websiteParser, IWebsiteDbContext websiteDbContext, IUrlProvider urlProvider)
    {
        _websiteParser = websiteParser;
        _websiteDbContext = websiteDbContext;
        _urlProvider = urlProvider;

    }

    public async void ParseAndSaveWebsites()
    {
        Queue<string> urlQueue = new Queue<string>(_urlProvider.GetUrls());

        while (urlQueue.Any())
        {
            var websites = await _websiteParser.ParseWebsites(urlQueue.DequeueChunk(20).ToArray());
            await _websiteDbContext.Websites.AddRangeAsync(websites);
            await _websiteDbContext.SaveChangesAsync();
        }
    }
}