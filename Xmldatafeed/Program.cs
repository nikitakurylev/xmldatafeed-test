using Xmldatafeed.Abstractions.Core;
using Xmldatafeed.Core.Parsers;
using Xmldatafeed.Core.Providers;
using Xmldatafeed.Core.Services;
using Xmldatafeed.DataAccess;

IWebsiteService websiteService =
    new WebsiteService(new WebsiteParser(),
        new WebsiteDbContext("server=localhost;user=root;password=12345qwert;database=Xmldatafeed;"),
        new UrlProvider("../../../Список_URL.txt"));
var task = websiteService.ParseAndSaveWebsites();
task.Wait();