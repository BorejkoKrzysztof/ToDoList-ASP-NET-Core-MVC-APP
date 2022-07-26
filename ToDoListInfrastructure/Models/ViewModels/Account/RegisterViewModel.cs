using System.ComponentModel.DataAnnotations;

namespace ToDoListInfrastructure.Models.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MaxLength(100)]
        public string? Password { get; set; }

        [Compare("Password", ErrorMessage = "Password must match")]
        [Display(Name = "Confirm Password")]
        public string? ConfirmPassword { get; set; }
    }
}
