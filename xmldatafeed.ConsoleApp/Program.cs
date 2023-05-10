using Microsoft.Extensions.Configuration;
using xmldatafeed.Core.Parsers;
using xmldatafeed.Core.Providers;
using xmldatafeed.Core.Services;
using xmldatafeed.DataAccess;

namespace xmldatafeed
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            WebsiteService websiteService =
                new WebsiteService(new WebsiteParser(), new WebsiteDbContext("server=localhost;user=root;password=12345qwert;database=xmldatafeed;"), new UrlProvider("../../../Список_URL.txt"));
            websiteService.ParseAndSaveWebsites();
        }
    }
}