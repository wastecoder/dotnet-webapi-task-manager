using Microsoft.EntityFrameworkCore;

namespace TaskManager.Infrastructure.Database;

public class TaskDbContext(DbContextOptions<TaskDbContext> options) : DbContext(options)
{
    public DbSet<Task> Tasks { get; set; }
}