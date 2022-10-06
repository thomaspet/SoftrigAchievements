namespace SoftrigAchievements.Models
{
    public class CounterForUser
    {
        public int Id { get; set; }
        public string User { get; set; } = string.Empty;
        public AchievementType AchievementType { get; set; }
        public int Count { get; set; }
    }

    
}
