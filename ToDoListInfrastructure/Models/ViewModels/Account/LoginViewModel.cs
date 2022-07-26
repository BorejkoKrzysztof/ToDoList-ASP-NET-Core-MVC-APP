using System.ComponentModel.DataAnnotations;

namespace ToDoListInfrastructure.Models.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MaxLength(100)]
        public string? Password { get; set; }
    }
}
