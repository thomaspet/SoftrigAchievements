using Database;
using SoftrigAchievements.Models;
using System.Security.Claims;

namespace SoftrigAchievements.Controllers;

public class NewAchievements
{
    public static List<Achievement> GetNewAchievements(HttpContext httpContext, Context database)
    {
        var user = httpContext.User;
        var temp = user.FindFirst(ClaimTypes.NameIdentifier);
        return new List<Achievement>();
    }
}
