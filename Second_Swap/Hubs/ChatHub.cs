using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Second_Swap.Models;

namespace Second_Swap.Hubs
{
    public class ChatHub : Hub
    {
        private readonly SecondSwapContext db;
        public static Dictionary<string, string> UsernameConnectionId = new();

        public ChatHub(SecondSwapContext dataContext)
        {
            db = dataContext;
        }
        public async Task<string> GetConnectionId(string username)
        {
            var user = await db.Users.SingleOrDefaultAsync(x => x.Id == int.Parse(username));

            if (UsernameConnectionId.ContainsKey(username))
            {
                UsernameConnectionId[username] = Context.ConnectionId;
            }
            else
            {
                UsernameConnectionId.Add(username, Context.ConnectionId);
            }

            return Context.ConnectionId;
        }
    }
}
