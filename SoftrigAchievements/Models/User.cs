namespace SoftrigAchievements.Models;

public sealed class User
{
    public int ID { get; set; }
    public Guid? GlobalIdentity { get; set; }
    public Guid? CompanyKey { get; set; }
}
