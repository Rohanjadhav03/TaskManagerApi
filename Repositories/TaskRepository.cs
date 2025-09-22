using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Data;
using TaskManagerApi.Models;

namespace TaskManagerApi.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _db;

        public TaskRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync()
        {
            return await _db.Tasks.OrderByDescending(t => t.CreatedAt).ToListAsync();
        }

        public async Task<TaskItem?> GetByIdAsync(int id)
        {
            return await _db.Tasks.FindAsync(id);
        }

        public async Task AddAsync(TaskItem item)
        {
            _db.Tasks.Add(item);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(TaskItem item)
        {
            _db.Tasks.Update(item);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(TaskItem item)
        {
            _db.Tasks.Remove(item);
            await _db.SaveChangesAsync();
        }
    }
}