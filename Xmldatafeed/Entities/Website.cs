namespace Xmldatafeed.Entities;

public class Website
{
    public Website()
    {
        Id = Guid.NewGuid();
        Date = DateTime.Now;
    }
    
    public Guid Id { get; protected set; }
    
    public string Url { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime Date { get; set; }
}