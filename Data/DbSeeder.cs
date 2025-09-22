using TaskManagerApi.Models;

namespace TaskManagerApi.Data
{
    public static class DbSeeder
    {
        public static void Seed(ApplicationDbContext db)
        {
            if (db.Tasks.Any()) return;

            var now = DateTime.UtcNow;
            var seed = new[]
            {
                new TaskItem { Title = "Setup project", Description = "Initialize repo and API project", IsCompleted = true, Priority = 4, CreatedAt = now, UpdatedAt = now },
                new TaskItem { Title = "Create DB", Description = "Create MySQL database and user", IsCompleted = true, Priority = 5, CreatedAt = now, UpdatedAt = now },
                new TaskItem { Title = "Implement CRUD", Description = "Tasks endpoints", IsCompleted = false, Priority = 3, CreatedAt = now, UpdatedAt = now }
            };

            db.Tasks.AddRange(seed);
            db.SaveChanges();
        }
    }
}