

using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces
{
    public interface ITaskRepository
    {
        Task<TaskItem?> GetByIdAsync(int id);
        Task<IEnumerable<TaskItem>> GetAllByUserIdAsync(int userId);
        Task<TaskItem> CreateAsync(TaskItem task);
        Task<int> UpdateAsync(int id,TaskItem task);
        Task<int> DeleteAsync(int id);
    }

}
