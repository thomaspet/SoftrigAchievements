
namespace DataCounter.Models;

public class Event
{
    public int Id { get; set; }
    public int EntityID { get; set; }
    public string EntityType { get; set; } = string.Empty;
    public string ChangeName { get; set; } = string.Empty;
    public string CompanyKey { get; set; } = string.Empty;
    public string GlobalIdentity { get; set; } = string.Empty;
    public string Route { get; set; } = string.Empty;
    public CUDType ChangeType { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    public DateTime Deleted { get; set; }
    
}

public enum CUDType
{
    Create = 0,
    Update = 1,
    Delete = 2
}