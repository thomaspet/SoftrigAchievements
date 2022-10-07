using Database;
using Microsoft.EntityFrameworkCore;
using SoftrigAchievements.Models;
using System.Security.Claims;

namespace SoftrigAchievements.Controllers;

public class NewAchievements
{
    public static List<Achievement> GetNewAchievements(ClaimsPrincipal user, Context database)
    {
        var globalIdentity = user.FindFirstValue(ClaimTypes.NameIdentifier);
        var achievementForUsers = database.AchievementForUsers.Where(x => x.Achieved && !x.Recieved && x.User == globalIdentity).Include(x => x.Achievement).ToList();
        if (achievementForUsers == null || !achievementForUsers.Any()) return new List<Achievement>();
        var achievements = achievementForUsers.Where(x => x.Achievement != null)?.Select(x => x.Achievement).ToList();
        achievementForUsers.ForEach(x => x.Recieved = true);
        database.UpdateRange(achievementForUsers);
        database.SaveChanges();
        return achievements ?? new List<Achievement>();
    }
}
