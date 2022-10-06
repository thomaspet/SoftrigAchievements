namespace SoftrigAchievements.Models
{
    public class AchievementForUser
    {
        public int ID { get; set; }
        public string User { get; set; } = string.Empty;
        public int AchievementId { get; set; }
        public Achievement? Achievement { get; set; }
        public bool Achieved { get; set; }
        public bool Recieved { get; set; }
    }
}
