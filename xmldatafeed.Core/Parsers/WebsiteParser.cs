using System.Net;
using AngleSharp;
using xmldatafeed.Abstractions.Core;
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

    private async Task<Website?> ParseWebsiteAsync(string url)
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
            Url = url,
            Title = doc.Title
        };

        var elements = doc.GetElementsByName("description");
        if (elements.Length > 0)
            website.Description = elements[0].Attributes["content"]?.Value;

        Console.WriteLine(url + " " + Thread.CurrentThread.ManagedThreadId);
        return website;
    }

    private async Task ParseQueueAsync()
    {
        try
        {
            while (_urlQueue.Any())
            {
                string url;
                lock (DataQueueLock)
                {
                    url = _urlQueue.Dequeue();
                }

                var website = await ParseWebsiteAsync(url);
                if (website != null)
                    _websites.Add(website);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

    }

    public async Task<List<Website>> ParseWebsites(string[] urls)
    {
        _websites = new List<Website>();
        _urlQueue = new Queue<string>(urls);
        var threads = new Task[ThreadCount];

        for (int i = 0; i < threads.Length; i++)
        {
            threads[i] = ParseQueueAsync();
        }

        Task.WaitAll(threads);

        return _websites;
    }
}