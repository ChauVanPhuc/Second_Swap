using Second_Swap.Models;
using System.ComponentModel.DataAnnotations;

namespace Second_Swap.Areas.Admin.ViewModels
{
    public class CategoryViewModel
    {
        public int? Id { get; set; } 
        public string? Avatar { get; set; }
        public IFormFile? img {  get; set; }

        [Required(ErrorMessage = "Please, Category Name")]
        [Display(Name = "Category Name")]
        public string? Name { get; set; }

        public bool? Status { get; set; }

        public DateTime? CreateDay { get; set; }
        public string? Description { get; set; }

        public DateTime? LastUpdated { get; set; }

    }
}
