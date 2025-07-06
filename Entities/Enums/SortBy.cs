using System.ComponentModel.DataAnnotations;

namespace TaskManager.Entities.Enums
{
    public enum SortBy : short
    {
        [Display(Name = "Vytvořeno")]
        CreatedAt,
        [Display(Name = "Název")]
        Title,
        [Display(Name = "Termín")]
        DueDate
    }
}
