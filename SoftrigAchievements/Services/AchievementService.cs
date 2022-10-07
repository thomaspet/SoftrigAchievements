﻿using Microsoft.EntityFrameworkCore;
using DataCounter.Models;
using SoftrigAchievements.Models;
using Database;

namespace SoftrigAchievements.Services
{

    public interface IAchievementService
    {
        void EventTriggeredAchievement(Event ev);
    }


    public class AchievementService : IAchievementService
    {
        private Context _database;
        public AchievementService(Context database)
        {
            _database = database;
        }

        public void EventTriggeredAchievement(Event ev)
        {
            var eventType = AchievementType.InvoiceSent;//This needs changing
            var counterForUser = _database.CounterForUsers.FirstOrDefault(x => x.User == ev.GlobalIdentity && x.AchievementType == eventType);
            if ( counterForUser == null )
            {
                counterForUser = new CounterForUser
                {
                    Count = 1,
                    AchievementType = eventType,
                    User = ev.GlobalIdentity,
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
                    User = ev.GlobalIdentity,
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
