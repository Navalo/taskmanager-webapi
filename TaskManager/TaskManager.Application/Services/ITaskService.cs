﻿using TaskManager.Domain.Entities;

namespace TaskManager.Application.Services
{
    public interface ITaskService
    {
        Task<TaskItem?> GetByIdAsync(int id);
        Task<IEnumerable<TaskItem>> GetAllByUserIdAsync(int userId);
        Task<TaskItem> CreateAsync(TaskItem taskDto);
        Task<int> UpdateAsync(int id,TaskItem taskDto);
        Task<int> DeleteAsync(int id);
    }
}
