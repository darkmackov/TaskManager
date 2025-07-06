using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Database;
using TaskManager.Entities;
using TaskManager.Entities.Enums;
using TaskManager.Models.TaskItem;
using TaskManager.Services;
using TaskManager.Validators;

namespace TaskManager.Controllers
{
    /// <summary>
    /// Controller responsible for handling CRUD operations for TaskItems.
    /// </summary>
    public class TaskItemController : ExtendedController
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
        public async Task<IActionResult> List(string? sort, string? state)
        {
            var taskItems = _context.TaskItems.AsQueryable();

            // Apply state and sort filters
            taskItems = TaskItemFilterService.ApplyTaskStateFilter(taskItems, state, out var currentState);
            taskItems = TaskItemFilterService.ApplySort(taskItems, sort, out var currentSort);

            ViewBag.CurrentSort = currentSort;
            ViewBag.CurrentState = currentState;

            // Map TaskItem entities to TaskItemViewModel
            var taskItemViewModels = await taskItems.Select(task => new TaskItemViewModel
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                State = task.State,
                CreatedAt = task.CreatedAt,
                DueDate = task.DueDate,
            }).ToListAsync();

            return View(taskItemViewModels);
        }

        /// <summary>
        /// Shows details for a specific TaskItem.
        /// </summary>
        public async Task<IActionResult> Detail(int id)
        {
            var taskItem = await _context.TaskItems.FindAsync(id);
            if (taskItem == null)
            {
                SetMessage("Úkol nebyl nalezen.", MessageType.Danger);
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
                StateOptions = EnumExtension.ToSelectListItems<TaskState>()
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
                StateOptions = EnumExtension.ToSelectListItems<TaskState>()
            };

            return View(viewModel);
        }

        /// <summary>
        /// Handles the POST request to create a new TaskItem.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create(TaskItemViewModel viewModel)
        {
            if (!ValidateViewModel(viewModel))
            {
                // Reinitialize the StateOptions for the view model
                viewModel.StateOptions = EnumExtension.ToSelectListItems<TaskState>();

                return View(viewModel);
            }

            var taskItem = new TaskItem
            {
                Title = viewModel.Title?.Trim()!,
                Description = viewModel.Description?.Trim()!,
                State = viewModel.State,
                CreatedAt = DateTime.Now,
                DueDate = viewModel.DueDate
            };

            _context.TaskItems.Add(taskItem);
            await _context.SaveChangesAsync();

            SetMessage("Úkol byl vytvořen.");
            return RedirectToAction("Detail", "TaskItem", new { Id = taskItem.Id });
        }

        /// <summary>
        /// Displays the form to update an existing TaskItem.
        /// </summary>
        public async Task<IActionResult> Update(int id)
        {
            var taskItem = await _context.TaskItems.FindAsync(id);
            if (taskItem == null)
            {
                SetMessage("Úkol nebyl nalezen.", MessageType.Danger);
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
                StateOptions = EnumExtension.ToSelectListItems<TaskState>()
            };

            return View(viewModel);
        }

        /// <summary>
        /// Handles the POST request to update an existing TaskItem.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Update(TaskItemViewModel viewModel)
        {
            if (!ValidateViewModel(viewModel))
            {
                // Reinitialize the StateOptions for the view model
                viewModel.StateOptions = EnumExtension.ToSelectListItems<TaskState>();

                return View(viewModel);
            }

            var taskItem = await _context.TaskItems.FindAsync(viewModel.Id);
            if (taskItem == null)
            {
                SetMessage("Úkol nebyl nalezen.", MessageType.Danger);
                return RedirectToAction("List", "TaskItem");
            }

            taskItem.Title = viewModel.Title?.Trim()!;
            taskItem.Description = viewModel.Description?.Trim()!;
            taskItem.State = viewModel.State;
            taskItem.DueDate = viewModel.DueDate;

            _context.TaskItems.Update(taskItem);
            await _context.SaveChangesAsync();

            SetMessage("Úkol byl upraven.");
            return RedirectToAction("Detail", "TaskItem", new { Id = taskItem.Id});
        }

        /// <summary>
        /// Deletes a TaskItem by its ID.
        /// </summary>
        public async Task<IActionResult> Delete(int id)
        {
            var taskItem = await _context.TaskItems.FindAsync(id);
            if (taskItem == null)
            {
                SetMessage("Úkol nebyl nalezen.", MessageType.Danger);
                return RedirectToAction("List", "TaskItem");
            }

            _context.TaskItems.Remove(taskItem);
            await _context.SaveChangesAsync();

            SetMessage("Úkol byl smazán.");
            return RedirectToAction("List", "TaskItem");
        }

        /// <summary>
        /// Validates the model using the validator service
        /// </summary>
        private bool ValidateViewModel(TaskItemViewModel viewModel)
        {
            var validationResults = _validator.Validate(viewModel);
            // Add validation results to ModelState
            AddValidationResults(validationResults);

            // Check if ModelState is valid
            return ModelState.IsValid;
        }
    }
}
