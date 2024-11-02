using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Web.Models.Domain;

namespace TaskManager.Web.Services.Interfaces
{
    public interface ITodoTaskService
    {
        Task<IEnumerable<TodoTask>> GetAllTasksAsync();
        Task<TodoTask> GetTaskByIdAsync(int id);
        Task<TodoTask> CreateTaskAsync(TodoTask task);
        Task UpdateTaskAsync(TodoTask task);
        Task DeleteTaskAsync(int id);
        Task<bool> TaskExistsAsync(int id);
        Task ToggleTaskStatusAsync(int id);
    }
}