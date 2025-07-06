using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models.TaskItem
{
    public class TaskItemViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Název")]
        public string Title { get; set; } = string.Empty;
        [Display(Name = "Popis")]
        public string Description { get; set; } = string.Empty;
        [Display(Name = "Stav")]
        public int State { get; set; } = 0;
        [Display(Name = "Vytvořeno")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Display(Name = "Termín dokončení")]
        public DateTime? DueDate { get; set; }
    }
}
