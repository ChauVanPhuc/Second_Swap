using System.ComponentModel.DataAnnotations;

namespace Second_Swap.ViewModels
{
    public class LoginViewModel
    {
        [MaxLength(50)]
        [EmailAddress]
        [Required(ErrorMessage = "Please, Enter Email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [MaxLength(50)]
        [MinLength(5)]
        [Required(ErrorMessage = "Please, Enter Password")]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
