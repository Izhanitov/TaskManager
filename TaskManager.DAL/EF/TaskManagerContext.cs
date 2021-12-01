using Microsoft.EntityFrameworkCore;
using TaskManager.DAL.Models;

namespace TaskManager.DAL.EF
{
    public class TaskManagerContext : DbContext
    {
        public TaskManagerContext(DbContextOptions<TaskManagerContext> options) : base(options) { }

        public DbSet<TaskRecord> TaskRecord { get; set; }

        public DbSet<TaskStatus> TaskStatus { get; set; }
    }
}
