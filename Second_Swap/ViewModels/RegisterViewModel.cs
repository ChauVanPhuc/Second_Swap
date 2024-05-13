using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Second_Swap.ViewModels
{
    public class RegisterViewModel
    {
        [MaxLength(50)]
        [Required(ErrorMessage = "Please, Enter Full Name")]
        [Display(Name = "Full Name")]
        public string? FullName { get; set; }

        [MaxLength(50)]
        [EmailAddress]
        [Required(ErrorMessage = "Please, Enter Email")]
        [Remote(action: "ValidateEmail", controller: "Authentication", HttpMethod = "POST")]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [MaxLength(50)]
        [MinLength(5)]
        [Required(ErrorMessage = "Please, Enter Password")]
        [Display(Name = "Password")]
        public string? Password { get; set; }

        [Compare("Password", ErrorMessage = "Password Incorrect")]
        [Required(ErrorMessage = "Please, Enter Confirm Password")]
        [Display(Name = "Confirm Password")]
        public string? ConfirmPassword { get; set; }

        [Phone]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Please, Enter Phone")]
        [Remote(action: "ValidatePhone", controller: "Authentication", HttpMethod = "POST")]
        [Display(Name = "Phone")]
        public string? Phone { get; set; }


        [Required(ErrorMessage = "Please, Choose Gender")]
        [Display(Name = "Gender")]
        public string? Gender { get; set; }
        public DateTime? BirthDay { get; set; }

        public IFormFile? avatar { get; set; }

        [Required(ErrorMessage = "Please, Choose Province")]
        public int? Province { get; set; }

        [Required(ErrorMessage = "Please, Choose District")]
        public int? District { get; set; }

        [Required(ErrorMessage = "Please, Choose Wards")]
        public int? Wards { get; set; }
    }
}
