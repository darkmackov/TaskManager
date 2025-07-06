using System.ComponentModel.DataAnnotations;
using TaskManager.Entities.Enums;
using TaskManager.Models.TaskItem;

namespace TaskManager.Validators
{
    public class TaskItemViewModelValidator : IValidator<TaskItemViewModel>
    {
        public IEnumerable<ValidationResult> Validate(TaskItemViewModel model)
        {
            var results = new[]
            {
                () => ValidateTitle(model.Title!),
                () => ValidateDescription(model.Description!),
                () => ValidateState(model.State),
                () => ValidateDueDate(model.DueDate)
            };

            // Execute each validation function and filter out null results
            return results
                .Select(validator => validator())
                .Where(result => result != null)!
                .Cast<ValidationResult>();
        }

        private static ValidationResult? ValidateTitle(string title)
        {
            title = title?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(title))
            {
                // Title is required
                return new ValidationResult("Název je povinný.", [nameof(TaskItemViewModel.Title)]);
            }
            else if (title.Length > 128)
            {
                // Title length limit
                return new ValidationResult("Název může mít maximálně 128 znaků.", [nameof(TaskItemViewModel.Title)]);
            }

            return null;
        }

        private static ValidationResult? ValidateDescription(string description)
        {
            description = description?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(description))
            {
                // Description is required
                return new ValidationResult("Popis je povinný.", [nameof(TaskItemViewModel.Description)]);
            }
            else if (description.Length > 4096)
            {
                // Description length limit
                return new ValidationResult("Popis může mít maximálně 4096 znaků.", [nameof(TaskItemViewModel.Description)]);
            }

            return null;
        }

        private static ValidationResult? ValidateState(TaskState state)
        {
            // Check if the state is defined in the TaskState enum
            if (!Enum.IsDefined(typeof(TaskState), state))
                return new ValidationResult("Stav je neplatný.", [nameof(TaskItemViewModel.State)]);

            return null;
        }

        private static ValidationResult? ValidateDueDate(DateTime? dueDate)
        {
            // If dueDate is null, it is considered valid (no due date set)
            if (!dueDate.HasValue)
                return null;

            var now = DateTime.Now;
            var max = now.AddYears(3);

            // Check if dueDate is in the past or more than 3 years in the future
            if (dueDate.Value < now || dueDate.Value > max)
                return new ValidationResult($"Termín dokončení musí být mezi {now:d} a {max:d}.",[nameof(TaskItemViewModel.DueDate)]);

            return null;
        }
    }
}
