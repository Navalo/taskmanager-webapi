using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Services
{
    public class TaskService(ITaskRepository taskRepository) : ITaskService
    {
        private readonly ITaskRepository _taskRepository = taskRepository;

        public async Task<TaskItem> CreateAsync(TaskItem taskDto)
        {
            return await _taskRepository.CreateAsync(taskDto);
        }

        public async Task<int> DeleteAsync(int id)
        {
            return await _taskRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync()
        {
            return await _taskRepository.GetAllAsync();
        }

        public async Task<TaskItem> GetByIdAsync(int id)
        {
            return await _taskRepository.GetByIdAsync(id);
        }

        public async Task<int> UpdateAsync(int id, TaskItem taskDto)
        {
            return await _taskRepository.UpdateAsync(id, taskDto);
        }
    }
}
