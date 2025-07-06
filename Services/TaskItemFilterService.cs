using TaskManager.Entities;
using TaskManager.Entities.Enums;

namespace TaskManager.Services
{
    /// <summary>
    /// Service for filtering and sorting TaskItems.
    /// </summary>
    public static class TaskItemFilterService
    {
        /// <summary>
        /// Applies sorting to the TaskItems based on the provided sort parameter.
        /// </summary>
        public static IQueryable<TaskItem> ApplySort(IQueryable<TaskItem> taskItems, string? sort, out string? currentSort)
        {
            // If the sort parameter is missing or invalid, default to sorting by CreatedAt (newest first).
            if (!Enum.TryParse(sort, true, out SortBy sortEnum))
                sortEnum = SortBy.CreatedAt;

            currentSort = sortEnum.ToString();
            return sortEnum switch
            {
                SortBy.Title => taskItems.OrderBy(t => t.Title),
                // If DueDate is null, it appears last in the list
                SortBy.DueDate => taskItems.OrderBy(t => t.DueDate == null).ThenBy(t => t.DueDate),
                _ => taskItems.OrderByDescending(t => t.CreatedAt)
            };
        }

        /// <summary>
        /// Applies a filter to the TaskItems based on the provided state parameter.
        /// </summary>
        public static IQueryable<TaskItem> ApplyTaskStateFilter(IQueryable<TaskItem> taskItems, string? state, out string? currentState)
        {
            // If the state parameter is missing or invalid, default to no filtering.
            if (!Enum.TryParse(state, true, out TaskState stateEnum))
            {
                currentState = null;
                return taskItems;
            }

            currentState = stateEnum.ToString();
            return taskItems.Where(task => task.State == stateEnum);
        }
    }
}
