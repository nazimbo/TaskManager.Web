using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskManager.Web.Models.Domain;
using TaskManager.Web.Services.Interfaces;

namespace TaskManager.Web.Controllers
{
    public class TodoTaskController : Controller
    {
        private readonly ITodoTaskService _taskService;
        private readonly ILogger<TodoTaskController> _logger;

        public TodoTaskController(ITodoTaskService taskService, ILogger<TodoTaskController> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        // GET: TodoTask
        public async Task<IActionResult> Index()
        {
            var tasks = await _taskService.GetAllTasksAsync();
            return View(tasks);
        }

        // GET: TodoTask/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var task = await _taskService.GetTaskByIdAsync(id);
                return View(task);
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning($"Task with ID {id} was not found.");
                return NotFound();
            }
        }

        // GET: TodoTask/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TodoTask/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,DueDate")] TodoTask task)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _taskService.CreateTaskAsync(task);
                    _logger.LogInformation($"Task created successfully: {task.Title}");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error creating task: {ex.Message}");
                    ModelState.AddModelError("", "Unable to create task. Please try again.");
                }
            }
            return View(task);
        }

        // GET: TodoTask/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var task = await _taskService.GetTaskByIdAsync(id);
                return View(task);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // POST: TodoTask/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,DueDate,IsCompleted")] TodoTask task)
        {
            if (id != task.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _taskService.UpdateTaskAsync(task);
                    _logger.LogInformation($"Task updated successfully: {task.Title}");
                    return RedirectToAction(nameof(Index));
                }
                catch (KeyNotFoundException)
                {
                    return NotFound();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error updating task: {ex.Message}");
                    ModelState.AddModelError("", "Unable to update task. Please try again.");
                }
            }
            return View(task);
        }

        // POST: TodoTask/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _taskService.DeleteTaskAsync(id);
                _logger.LogInformation($"Task deleted successfully: ID {id}");
                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting task: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: TodoTask/ToggleStatus/5
        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            try
            {
                await _taskService.ToggleTaskStatusAsync(id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error toggling task status: {ex.Message}");
                return Json(new { success = false, message = "Unable to update task status." });
            }
        }
    }
}