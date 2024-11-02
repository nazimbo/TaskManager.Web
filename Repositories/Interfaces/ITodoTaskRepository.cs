using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Web.Models.Domain;

namespace TaskManager.Web.Repositories.Interfaces
{
    public interface ITodoTaskRepository
    {
        Task<IEnumerable<TodoTask>> GetAllAsync();
        Task<TodoTask> GetByIdAsync(int id);
        Task<TodoTask> CreateAsync(TodoTask task);
        Task UpdateAsync(TodoTask task);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}