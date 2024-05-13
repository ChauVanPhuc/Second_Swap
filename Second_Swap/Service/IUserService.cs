using Microsoft.EntityFrameworkCore;
using Second_Swap.Models;

namespace Second_Swap.Service
{
    public class IUserService
    {
        private readonly SecondSwapContext _context;

        public IUserService(SecondSwapContext context)
        {
            _context = context;
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(Order order)
        {

            order.PaymentToken = Guid.NewGuid().ToString();
            order.TokenExpiration = DateTime.UtcNow.AddHours(24); // Token
            await _context.SaveChangesAsync();
            return order.PaymentToken;
        }
    }
}
