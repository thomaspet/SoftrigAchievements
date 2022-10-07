using Microsoft.EntityFrameworkCore;
using DataCounter.Models;
using SoftrigAchievements.Models;
using Database;

namespace SoftrigAchievements.Services
{

    public interface IAchievementService
    {
        Task EventTriggeredAchievementAsync(Guid companyKey, int invoiceID, AchievementType achievementType);
    }


    public class AchievementService : IAchievementService
    {
        private Context _database;
        private IEconomyHttpService _httpService;
        public AchievementService(Context database, IEconomyHttpService httpService)
        {
            _database = database;
            _httpService = httpService;

        }

        public async Task EventTriggeredAchievementAsync(Guid companyKey, int invoiceID, AchievementType achievementType)
        {
            var user = await _httpService.GetUserFromInvoiceAsync(companyKey, invoiceID);
            var counterForUser = _database.CounterForUsers.FirstOrDefault(x => x.User == user && x.AchievementType == achievementType);
            if ( counterForUser == null )
            {
                counterForUser = new CounterForUser
                {
                    Count = 1,
                    AchievementType = achievementType,
                    User = user,
                };
            }
            else
            {
                counterForUser.Count++;   
            }
            _database.Update(counterForUser);
            _database.SaveChanges();
            
            var achievementsGained = GetNewAchievements(counterForUser);
            foreach (var achievement in achievementsGained)
            {
                var achievementForUser = new AchievementForUser
                {
                    User = user,
                    Achievement = achievement,
                    AchievementId = achievement.Id,
                    Achieved = true,
                    Recieved = false,
                };
                _database.Add(achievementForUser);
            }
            _database.SaveChanges();
        }

        private List<Achievement> GetNewAchievements(CounterForUser counterForUser)
        {
            var possibleAchievements = _database.Achievements.Where(x => x.AchievementType == counterForUser.AchievementType && x.Count <= counterForUser.Count);
            if (!possibleAchievements.Any()) return new List<Achievement>();
            var achievedAllready = _database.AchievementForUsers.Where(x => x.User == counterForUser.User && x.Recieved)?.Select(x => x.AchievementId);
            if (achievedAllready == null || !achievedAllready.Any()) return possibleAchievements.ToList();
            var achievedNow = new List<Achievement>();
            foreach (var achievement in possibleAchievements)
            {
                if (achievedAllready.Contains(achievement.Id)) achievedNow.Add(achievement);
            }
            return achievedNow;
        }
    }
}
