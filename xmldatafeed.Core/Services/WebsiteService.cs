using xmldatafeed.Abstractions.Core;
using xmldatafeed.Abstractions.DataAccess;

namespace xmldatafeed.Core.Services;

public class WebsiteService : IWebsiteService
{
    private IWebsiteParser _websiteParser;
    private IWebsiteDbContext _websiteDbContext;
    private IUrlProvider _urlProvider;

    public WebsiteService(IWebsiteParser websiteParser,/* IWebsiteDbContext websiteDbContext,*/ IUrlProvider urlProvider)
    {
        _websiteParser = websiteParser;
        //_websiteDbContext = websiteDbContext;
        _urlProvider = urlProvider;
    }
    
    public async void ParseAndSaveWebsites()
    {
        string[] urls = _urlProvider.GetUrls();

        _websiteParser.ParseWebsites(urls);
    }
}