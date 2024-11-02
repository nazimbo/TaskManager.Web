using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Web.Models.Domain;
using TaskManager.Web.Repositories.Interfaces;
using TaskManager.Web.Services.Interfaces;

namespace TaskManager.Web.Services
{
    public class TodoTaskService : ITodoTaskService
    {
        private readonly ITodoTaskRepository _repository;

        public TodoTaskService(ITodoTaskRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TodoTask>> GetAllTasksAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<TodoTask> GetTaskByIdAsync(int id)
        {
            var task = await _repository.GetByIdAsync(id);
            if (task == null)
            {
                throw new KeyNotFoundException($"Task with ID {id} not found.");
            }
            return task;
        }

        public async Task<TodoTask> CreateTaskAsync(TodoTask task)
        {
            task.CreatedAt = DateTime.UtcNow;
            return await _repository.CreateAsync(task);
        }

        public async Task UpdateTaskAsync(TodoTask task)
        {
            if (!await _repository.ExistsAsync(task.Id))
            {
                throw new KeyNotFoundException($"Task with ID {task.Id} not found.");
            }

            task.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(task);
        }

        public async Task DeleteTaskAsync(int id)
        {
            if (!await _repository.ExistsAsync(id))
            {
                throw new KeyNotFoundException($"Task with ID {id} not found.");
            }

            await _repository.DeleteAsync(id);
        }

        public async Task<bool> TaskExistsAsync(int id)
        {
            return await _repository.ExistsAsync(id);
        }

        public async Task ToggleTaskStatusAsync(int id)
        {
            var task = await GetTaskByIdAsync(id);
            task.IsCompleted = !task.IsCompleted;
            task.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(task);
        }
    }
}