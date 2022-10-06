using DataCounter.Models;
using Microsoft.EntityFrameworkCore;

namespace DataCounter.Database;

public class CounterDatabase : DbContext
{
	public CounterDatabase(DbContextOptions options) :base(options) { }

	public DbSet<Event> Events { get; set; } = null!;
}
