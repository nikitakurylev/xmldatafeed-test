using System.Net;
using AngleSharp;
using xmldatafeed.Abstractions.Core;
using xmldatafeed.Core.Extensions;
using xmldatafeed.Domain.Entities;

namespace xmldatafeed.Core.Parsers;

public class WebsiteParser : IWebsiteParser
{
    private readonly IBrowsingContext _context;
    private List<Website> _websites;
    private Queue<string> _urlQueue;
    private static readonly object DataQueueLock = new();
    private const int ThreadCount = 20;

    public WebsiteParser()
    {
        IConfiguration config = Configuration.Default.WithDefaultLoader();
        _context = BrowsingContext.New(config);
        _websites = new List<Website>();
        _urlQueue = new Queue<string>();
    }

    private async Task<Website?> ParseWebsite(string url)
    {
        var doc = await _context.OpenAsync("https://" + url);

        if (doc.StatusCode != HttpStatusCode.OK)
        {
            doc = await _context.OpenAsync("http://" + url);
            if (doc.StatusCode != HttpStatusCode.OK)
                return null;
        }

        Website website = new Website
        {
            Title = doc.Title ?? string.Empty
        };

        var elements = doc.GetElementsByName("description");
        if (elements.Length > 0)
            website.Description = elements[0].Attributes["content"]?.Value ?? string.Empty;

        Console.WriteLine(website.Title);
        return website;
    }

    private async void ParseQueue()
    {
        while (_urlQueue.Any())
        {
            string url;
            lock (DataQueueLock)
            {
                url = _urlQueue.Dequeue();
            }

            var website = await ParseWebsite(url);
            if (website != null)
                _websites.Add(website);
        }
    }

    public List<Website> ParseWebsites(ICollection<string> urls)
    {
        _websites = new List<Website>();
        var bigQueue = new Queue<string>(urls);
        while (bigQueue.Any())
        {
            _urlQueue = new Queue<string>(bigQueue.DequeueChunk(1000));
            var threads = new Thread[ThreadCount];
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(ParseQueue);
                threads[i].Start();
            }

            foreach (var thread in threads)
                thread.Join();
        }

        return _websites;
    }
}