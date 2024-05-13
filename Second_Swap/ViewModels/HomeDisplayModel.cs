

using Second_Swap.Models;

namespace Second_Swap.ViewModels
{
    public class HomeDisplayModel
    {
        public IEnumerable<Category> Categorys { get; set; }

        public IEnumerable<Product> Products { get; set; }

        public IEnumerable<User> Users { get; set; }
        public IEnumerable<Address> Address { get; set; }
        public IEnumerable<Province> Province { get; set; }
        public IEnumerable<District> District { get; set; }
        public IEnumerable<Ward> Ward { get; set; }
        public IEnumerable<ProductImage> ProductImage { get; set; }
    }
}
