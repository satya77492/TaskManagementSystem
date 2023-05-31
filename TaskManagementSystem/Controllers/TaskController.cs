using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Models;
using System.Linq;
using TaskManagementSystem.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TaskManagementSystem.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ILogger<TaskController> _logger;
        public TaskController(ITaskRepository taskRepository, ILogger<TaskController> logger)
        {
            _taskRepository = taskRepository;
            _logger = logger;
        }
        // GET: TaskController
        public async Task<ActionResult> Index()
        {
            var result = new List<TaskModel>();
            try
            {
                var tasks = await _taskRepository.GetAllAsync();
                foreach (var task in tasks)
                {
                    result.Add(new TaskModel { Id = task.Id, Status = task.Status.ToString(), Name = task.Name, Description = task.Description, StatusDescription = Enum.GetName(typeof(Models.TaskStatus), task.Status) });
                }
                return View(result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("exception in retreiving tasks {exception}", ex.Message);
            }
            return View(result);
        }

        // GET: TaskController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var task = await _taskRepository.GetAsync(id);
                var taskModel = new TaskModel() { Id = task.Id, Description = task.Description, Name = task.Name, Status = task.Status.ToString(), StatusDescription = Enum.GetName(typeof(Models.TaskStatus), task.Status) };
                return View(taskModel);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("exception in retreiving task details {exception}", ex.Message);
            }
            return View();
        }

        // GET: TaskController/Create
        public ActionResult Create()
        {
            var task = new TaskModel();
            task.TaskStatusList = GetTaskStatusList();
            return View(task);
        }

        // POST: TaskController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TaskModel model)
        {
            try
            {
                ModelState.Remove("TaskStatusList");
                ModelState.Remove("StatusDescription"); 
                if (ModelState.IsValid)
                {
                    await _taskRepository.CreateAsync(model);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogInformation("exception in creating task {exception}", ex.Message);
                return RedirectToAction(nameof(Create));
            }
        }

        // GET: TaskController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var task = await _taskRepository.GetAsync(id);
            var taskModel = new TaskModel() { Id = task.Id, Description = task.Description, Name = task.Name, Status = task.Status.ToString() };
            taskModel.TaskStatusList = GetTaskStatusList();
            return View(taskModel);
        }

        // POST: TaskController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, TaskModel model)
        {
            try
            {
                ModelState.Remove("TaskStatusList");
                ModelState.Remove("StatusDescription");
                if (ModelState.IsValid)
                {
                    await _taskRepository.UpdateAsync(id, model);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogInformation("exception in updating task {exception}", ex.Message);
                return RedirectToAction(nameof(Edit), id);
            }
        }

        // GET: TaskController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var task = await _taskRepository.GetAsync(id);
            var taskModel = new TaskModel() { Id = task.Id, Description = task.Description, Name = task.Name, Status = task.Status.ToString(), StatusDescription = Enum.GetName(typeof(Models.TaskStatus), task.Status) };
            return View(taskModel);
        }

        // POST: TaskController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, TaskModel model)
        {
            try
            {
                _taskRepository.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogInformation("exception in deleting task {exception}", ex.Message);
                return RedirectToAction(nameof(Delete), id);
            }
        }

        public List<SelectListItem> GetTaskStatusList()
        {
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "New", Value = "0" });
            list.Add(new SelectListItem { Text = "In Progress", Value = "1" });
            list.Add(new SelectListItem { Text = "Completed", Value = "2" });
            return list;
        }
    }
}
