using Second_Swap.Models;

namespace Second_Swap.ViewModels
{
    public class ProfileDisplayModel
    {
        public User User {  get; set; }
        public IEnumerable<Order> Order { get; set; }
    }
}
