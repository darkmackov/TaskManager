using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using TaskManager.Database;
using TaskManager.Entities;
using TaskManager.Entities.Enums;
using TaskManager.Models.TaskItem;
using TaskManager.Validators;

namespace TaskManager.Controllers
{
    /// <summary>
    /// Controller responsible for handling CRUD operations for TaskItems.
    /// </summary>
    public class TaskItemController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly TaskItemViewModelValidator _validator;

        public TaskItemController(DatabaseContext context)
        {
            _context = context;
            _validator = new TaskItemViewModelValidator();
        }

        /// <summary>
        /// Displays a list of all TaskItems.
        /// </summary>
        public IActionResult List()
        {
            var taskItems = _context.TaskItems.ToList();

            // Map TaskItem entities to TaskItemViewModel
            var taskItemViewModels = taskItems.Select(task => new TaskItemViewModel
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                State = task.State,
                CreatedAt = task.CreatedAt,
                DueDate = task.DueDate,
            }).ToList();

            return View(taskItemViewModels);
        }

        /// <summary>
        /// Shows details for a specific TaskItem.
        /// </summary>
        public IActionResult Detail(int id)
        {
            var taskItem = _context.TaskItems.Find(id);
            if (taskItem == null)
            {
                TempData["Message"] = "Úkol nebyl nalezen.";
                TempData["MessageType"] = "danger";
                return RedirectToAction("List", "TaskItem");
            }

            var viewModel = new TaskItemViewModel
            {
                Id = taskItem.Id,
                Title = taskItem.Title,
                Description = taskItem.Description,
                State = taskItem.State,
                CreatedAt = taskItem.CreatedAt,
                DueDate = taskItem.DueDate,
                StateOptions = GetStateOptions()
            };

            return View(viewModel);
        }

        /// <summary>
        /// Displays the form to create a new TaskItem.
        /// </summary>
        public IActionResult Create()
        {
            var viewModel = new TaskItemViewModel()
            {
                // Initialize the StateOptions with all TaskState enum values using reflection to get display names
                StateOptions = GetStateOptions()
            };

            return View(viewModel);
        }

        /// <summary>
        /// Handles the POST request to create a new TaskItem.
        /// </summary>
        [HttpPost]
        public IActionResult Create(TaskItemViewModel viewModel)
        {
            // Validate the model using the validator service
            var validationResults = _validator.Validate(viewModel);
            AddValidationResults(validationResults);

            if (!ModelState.IsValid)
            {
                // Reinitialize the StateOptions for the view model
                viewModel.StateOptions = GetStateOptions();

                return View(viewModel);
            }

            var taskItem = new TaskItem
            {
                Title = viewModel.Title,
                Description = viewModel.Description,
                State = viewModel.State,
                CreatedAt = DateTime.Now,
                DueDate = viewModel.DueDate
            };

            _context.TaskItems.Add(taskItem);
            _context.SaveChanges();

            TempData["Message"] = "Úkol byl vytvořen.";
            TempData["MessageType"] = "success";
            return RedirectToAction("Detail", "TaskItem", new { Id = taskItem.Id });
        }

        /// <summary>
        /// Displays the form to update an existing TaskItem.
        /// </summary>
        public IActionResult Update(int id)
        {
            var taskItem = _context.TaskItems.Find(id);
            if (taskItem == null)
            {
                TempData["Message"] = "Úkol nebyl nalezen.";
                TempData["MessageType"] = "danger";
                return RedirectToAction("List", "TaskItem");
            }

            var viewModel = new TaskItemViewModel
            {
                Id = taskItem.Id,
                Title = taskItem.Title,
                Description = taskItem.Description,
                State = taskItem.State,
                CreatedAt = taskItem.CreatedAt,
                DueDate = taskItem.DueDate,
                StateOptions = GetStateOptions()
            };

            return View(viewModel);
        }

        /// <summary>
        /// Handles the POST request to update an existing TaskItem.
        /// </summary>
        [HttpPost]
        public IActionResult Update(TaskItemViewModel viewModel)
        {
            // Validate the model using the validator service
            var validationResults = _validator.Validate(viewModel);
            AddValidationResults(validationResults);

            if (!ModelState.IsValid)
            {
                // Reinitialize the StateOptions for the view model
                viewModel.StateOptions = GetStateOptions();

                return View(viewModel);
            }

            var taskItem = _context.TaskItems.Find(viewModel.Id);
            if (taskItem == null)
            {
                TempData["Message"] = "Úkol nebyl nalezen.";
                TempData["MessageType"] = "danger";
                return RedirectToAction("List", "TaskItem");
            }

            taskItem.Title = viewModel.Title;
            taskItem.Description = viewModel.Description;
            taskItem.State = viewModel.State;
            taskItem.DueDate = viewModel.DueDate;

            _context.TaskItems.Update(taskItem);
            _context.SaveChanges();

            TempData["Message"] = "Úkol byl upraven.";
            TempData["MessageType"] = "success";
            return RedirectToAction("Detail", "TaskItem", new { Id = taskItem.Id});
        }

        /// <summary>
        /// Deletes a TaskItem by its ID.
        /// </summary>
        public IActionResult Delete(int id)
        {
            var taskItem = _context.TaskItems.Find(id);
            if (taskItem == null)
            {
                TempData["Message"] = "Úkol nebyl nalezen.";
                TempData["MessageType"] = "danger";
                return RedirectToAction("List", "TaskItem");
            }

            _context.TaskItems.Remove(taskItem);
            _context.SaveChanges();

            TempData["Message"] = "Úkol byl smazán.";
            TempData["MessageType"] = "success";
            return RedirectToAction("List", "TaskItem");
        }

        /// <summary>
        /// Helper method to get the state options for the dropdown list.
        /// </summary>
        private static List<SelectListItem> GetStateOptions()
        {
            return Enum.GetValues(typeof(TaskState))
                .Cast<TaskState>()
                .Select(state => new SelectListItem
                {
                    Value = ((int)state).ToString(),
                    Text = state.GetDisplayName()
                }).ToList();
        }

        /// <summary>
        /// Helper method to add validation results to the ModelState.
        /// </summary>
        private void AddValidationResults(IEnumerable<ValidationResult> results)
        {
            foreach (var result in results)
            {
                foreach (var member in result.MemberNames)
                {
                    ModelState.AddModelError(member, result.ErrorMessage!);
                }
            }
        }
    }
}
