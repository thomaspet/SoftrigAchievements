namespace SoftrigAchievements.Models
{
    public class Achievement
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public AchievementType AchievementType { get; set; }
        public int Count { get; set; }
    }

    public enum AchievementType
    {
        InvoiceSent = 0,
        InvoiceCreated = 1,
    }
}
