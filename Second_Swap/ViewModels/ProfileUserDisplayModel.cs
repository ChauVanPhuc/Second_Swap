using Second_Swap.Models;

namespace Second_Swap.ViewModels
{
    public class ProfileUserDisplayModel
    {
        public User User { get; set; }

        public IEnumerable<Product> Products { get; set; }
    }
}
