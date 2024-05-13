
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Second_Swap.Models;

namespace Second_Swap.ViewModels
{
    public class PostEditModel
    {
        public int? Id { get; set; }

        [MaxLength(150)]
        [Required(ErrorMessage = "Please, Enter Title")]
        [Display(Name = " Title")]
        public string? Title { get; set; }



        [MaxLength(500)]
        [Required(ErrorMessage = "Please, Enter ShortDescription")]
        [Display(Name = " Short Description")]
        public string? Description { get; set; }

        public double? Price { get; set; }

        public bool? Status { get; set; }

        public DateTime? CreateDay { get; set; }

        public int? CreateBy { get; set; }

        public int? CategoryId { get; set; }

        public bool? Need { get; set; }

        public int? BrandId { get; set; }

        public bool? IsActive { get; set; }

        public virtual Brand? Brand { get; set; }

        public virtual Category? Category { get; set; }

        public virtual User? CreateByNavigation { get; set; }

        public IEnumerable<ProductImage>? ProductImages { get; set; }

        public IFormFileCollection? Img { get; set; }
    }
}
