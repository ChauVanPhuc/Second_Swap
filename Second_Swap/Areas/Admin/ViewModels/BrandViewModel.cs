using System.ComponentModel.DataAnnotations;

namespace Second_Swap.Areas.Admin.ViewModels
{
    public class BrandViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please, Brand Name")]
        [Display(Name = "Brand Name")]
        public string? Name { get; set; }

        public string? Description { get; set; }

        public bool? Status { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? LastUpdated { get; set; }

        [Required(ErrorMessage = "Please, Category Name")]
        [Display(Name = "Category Name")]
        public int? CategoryId { get; set; }

        public string? Avatar { get; set; }
        public IFormFile? img { get; set; }
    }
}
