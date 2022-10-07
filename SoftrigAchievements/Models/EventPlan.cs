namespace SoftrigAchievements.Models;


public sealed class EventSubscriber
{
    public DateTime UpdatedAt { get; set; }
    public bool Active { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Endpoint { get; set; } = string.Empty;
    public int EventplanID { get; set; }
    public int StatusCode { get; set; }
    public int ID { get; set; }
    public string Headers { get; set; } = string.Empty;
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool Deleted { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
    public string Authorization { get; set; } = string.Empty;
    public Eventplan Eventplan { get; set; } = null!;
}

public sealed class Eventplan
{
    public DateTime UpdatedAt { get; set; }
    public int PlanType { get; set; }
    public bool Active { get; set; }
    public string Name { get; set; } = string.Empty;
    public string OperationFilter { get; set; } = string.Empty;
    public int StatusCode { get; set; }
    public string JobNames { get; set; } = string.Empty;
    public string Cargo { get; set; } = string.Empty;
    public string ModelFilter { get; set; } = string.Empty;
    public string SigningKey { get; set; } = string.Empty;
    public int ID { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool Deleted { get; set; }
    public bool IsSystemPlan { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
    public List<ExpressionFilter> ExpressionFilters { get; set; } = new();
    public List<EventSubscriber> Subscribers { get; set; } = new();
}

public class ExpressionFilter
{
    public DateTime UpdatedAt { get; set; }
    public int EventplanID { get; set; }
    public int StatusCode { get; set; }
    public string EntityName { get; set; } = string.Empty;
    public int ID { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string Expression { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool Deleted { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
    public Eventplan Eventplan { get; set; } = null!;
}

