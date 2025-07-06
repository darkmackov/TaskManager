using TaskManager.Entities.Enums;

namespace TaskManager.Helpers
{
    public static class ViewHelper
    {
        public static string GetBadgeClass(TaskState state)
        {
            return state switch
            {
                TaskState.New => "bg-secondary",
                TaskState.Active => "bg-primary",
                TaskState.Completed => "bg-success",
                _ => "bg-info text-dark"
            };
        }
    }
}
