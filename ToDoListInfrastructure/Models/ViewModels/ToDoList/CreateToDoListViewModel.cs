using System.ComponentModel.DataAnnotations;

namespace ToDoListInfrastructure.Models.ViewModels.ToDoList
{
    public class CreateToDoListViewModel
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Title is too long.")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Title { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}
