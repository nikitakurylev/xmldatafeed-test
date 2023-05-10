using System.Collections.Concurrent;
using System.Net;
using AngleSharp;
using Xmldatafeed.Abstractions.Core;
using Xmldatafeed.Entities;

namespace Xmldatafeed.Core.Parsers;

public class WebsiteParser : IWebsiteParser
{
    private const int ThreadCount = 20;
    private readonly IBrowsingContext _context;
    private List<Website> _websites;
    private ConcurrentQueue<string> _urlQueue;

    public WebsiteParser()
    {
        var config = Configuration.Default.WithDefaultLoader();
        _context = BrowsingContext.New(config);
        _websites = new List<Website>();
        _urlQueue = new ConcurrentQueue<string>();
    }

    private Website? ParseWebsiteAsync(string url)
    {
        var document = _context.OpenAsync("https://" + url).Result;

        if (document.StatusCode != HttpStatusCode.OK)
            return null;

        var description = document.GetElementsByName("description").FirstOrDefault();

        var website = new Website(url)
        {
            Title = document.Title,
            Description = description?.Attributes["content"]?.Value
        };

        return website;
    }

    private void ParseUrlQueue()
    {
        while (_urlQueue.TryDequeue(out var url))
        {
            var website = ParseWebsiteAsync(url);
            if (website != null)
                _websites.Add(website);
        }
    }

    public IEnumerable<Website> ParseWebsites(IEnumerable<string> urls)
    {
        _websites = new List<Website>();
        _urlQueue = new ConcurrentQueue<string>(urls);
        var threads = new Thread[ThreadCount];

        for (var i = 0; i < threads.Length; i++)
        {
            threads[i] = new Thread(ParseUrlQueue);
            threads[i].Start();
        }

        foreach (var thread in threads)
            thread.Join();

        return _websites;
    }
}