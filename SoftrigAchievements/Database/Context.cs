using DataCounter.Models;
using Microsoft.EntityFrameworkCore;

namespace Database;

public class Context : DbContext
{
	public Context(DbContextOptions options) :base(options) { }

	public DbSet<Event> Events { get; set; } = null!;

	public DbSet<SoftrigAchievements.Models.Achievement> Achievements { get; set; } = null!;

	public DbSet<SoftrigAchievements.Models.AchievementForUser> AchievementForUsers { get; set; } = null!;
	public DbSet<SoftrigAchievements.Models.CounterForUser> CounterForUsers { get; set; } = null!;
}
