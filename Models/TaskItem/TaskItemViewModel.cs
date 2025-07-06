using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManager.Entities.Enums;

namespace TaskManager.Models.TaskItem
{
    public class TaskItemViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Název")]
        public string? Title { get; set; } = string.Empty; // Nullable to disable automatic DataAnnotations validation
        [Display(Name = "Popis")]
        public string? Description { get; set; } = string.Empty; // Nullable to disable automatic DataAnnotations validation
        [Display(Name = "Stav")]
        public TaskState State { get; set; } = 0;
        [Display(Name = "Vytvořeno")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Display(Name = "Termín dokončení")]
        public DateTime? DueDate { get; set; }

        public List<SelectListItem> StateOptions { get; set; } = new();
    }
}
