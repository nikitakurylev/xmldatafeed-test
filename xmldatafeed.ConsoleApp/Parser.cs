using AngleSharp;

namespace xmldatafeed;

public class Parser
{
    private IBrowsingContext _context;
    
    Parser()
    {
        IConfiguration config = Configuration.Default.WithDefaultLoader();
        _context = BrowsingContext.New(config);
    }
}