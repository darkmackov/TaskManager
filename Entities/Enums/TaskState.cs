using System.ComponentModel.DataAnnotations;

namespace TaskManager.Entities.Enums
{
    public enum TaskState : short
    {
        [Display(Name = "Nový")]
        New = 0,
        [Display(Name = "Aktivní")]
        Active = 1,
        [Display(Name = "Dokončený")]
        Completed = 2,
    }
}
