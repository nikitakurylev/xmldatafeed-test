using System.Net;
using AngleSharp;
using xmldatafeed.Core.Parsers;
using xmldatafeed.Core.Providers;
using xmldatafeed.Core.Services;

namespace xmldatafeed
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            WebsiteService websiteService =
                new WebsiteService(new WebsiteParser(), new UrlProvider("../../../Список_URL.txt"));
            websiteService.ParseAndSaveWebsites();
        }
    }
}