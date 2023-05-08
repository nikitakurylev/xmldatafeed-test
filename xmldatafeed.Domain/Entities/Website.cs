namespace xmldatafeed.Domain.Entities;

public class Website
{
    public Website()
    {
        PrimaryKey = Guid.NewGuid();
    }
    
    public Guid PrimaryKey { get; protected set; }
    
    public string Url { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}