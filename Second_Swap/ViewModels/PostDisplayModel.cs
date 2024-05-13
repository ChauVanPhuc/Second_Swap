using Second_Swap.Models;

namespace Second_Swap.ViewModels
{
    public class PostDisplayModel
    {
        public Product Products { get; set; }
        public IEnumerable<Category> Categorys { get; set; }
        public User Users { get; set; }
        public IEnumerable<Brand> Brands { get; set; }
        public IEnumerable<Address> Address { get; set; }
        public IEnumerable<District> Districts { get; set; }
        public IEnumerable<Province> Province { get; set; }
        public IEnumerable<Ward> Wards { get; set; }
    }
}
