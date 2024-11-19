using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories
{
    public class TaskRepository(ApplicationDbContext context) : ITaskRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<TaskItem> CreateAsync(TaskItem task)
        {
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<int> DeleteAsync(int id)
        {
            return await _context.Tasks
                  .Where(model => model.Id == id)
                  .ExecuteDeleteAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetAllByUserIdAsync(int userId)
        {
            return await _context.Tasks.Where(t => t.UserId == userId).AsNoTracking().ToListAsync();
        }

        public async Task<TaskItem?> GetByIdAsync(int id)
        {
            return await _context.Tasks.AsNoTracking()
                    .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<int> UpdateAsync(int id,TaskItem task)
        {
            return await _context.Tasks
                  .Where(model => model.Id == id)
                  .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Title, task.Title)
                    .SetProperty(m => m.Description, task.Description)
                    .SetProperty(m => m.IsCompleted, task.IsCompleted)
                  );
        }
    }
}
