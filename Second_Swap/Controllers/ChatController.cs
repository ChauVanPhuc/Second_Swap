using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using Second_Swap.Hubs;
using Second_Swap.Models;
using Second_Swap.ViewModels;

namespace Second_Swap.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly SecondSwapContext _db;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatController(SecondSwapContext dataContext, IHubContext<ChatHub> hubContext)
        {
            _db = dataContext;
            _hubContext = hubContext;
        }


        [Route("/Chat/SendMessage")]
        public async Task<IActionResult> SendMessage(string to, string text)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return StatusCode(500);
                }
                var account = HttpContext.Session.GetString("AccountId");

                var user = _db.Users.AsNoTracking().SingleOrDefault(x => x.Id == int.Parse(account));
                var recipient = await _db.Users.SingleOrDefaultAsync(x => x.Id == int.Parse(to));

                Message message = new Message()
                {
                    Sender = user.Id,
                    Receiver = recipient.Id,
                    Content = text,
                    SentAt = DateTime.Now,
                    Read = false,
                    
                };
                await _db.AddAsync(message);
                await _db.SaveChangesAsync();

                var countRecipient = _db.Messages.Where(x=> x.Sender == user.Id && x.Receiver == recipient.Id && x.Read == false || 
                                                        x.Receiver == user.Id && x.Sender == recipient.Id && x.Read == false).Count();

                string connectionId = ChatHub.UsernameConnectionId[to];

                await _hubContext.Clients.Clients(connectionId).SendAsync("RecieveMessage", message.Content, message.SentAt,
                                                                        message.Sender.ToString(), countRecipient);

                return Ok();
            }
            catch (Exception)
            {
                return Ok();
            }

        }

        [Route("/Chat")]
        public async Task<IActionResult> Chat()
        {
            var account = HttpContext.Session.GetString("AccountId");
            if (account == null)
            {
                return Redirect("/Login");
            }
            var user = _db.Users.Find(int.Parse(account));

            List<Message> allMessages = _db.Messages.Where(x =>
                x.Sender == user.Id ||
                x.Receiver == user.Id)
                .ToList();

            var chats = new List<ChatViewModel>();
            List<User> listUser = await GetAllUsersInMessages(allMessages);

            foreach (var i in listUser)
            {
                if (i != user)
                {

                    var chat = new ChatViewModel()
                    {
                        MyMessages = allMessages.Where(x => x.Sender == user.Id && x.Receiver == i.Id).ToList(),
                        OtherMessages = allMessages.Where(x => x.Sender == i.Id && x.Receiver == user.Id).ToList(),
                        RecipientName = i.FullName,
                        revId = i.Id,
                        sendvId = user.Id,
                        avatar = i.Avatar,

                        countMeess = _db.Messages.Where(x => x.Sender == i.Id && x.Receiver ==user.Id && x.Read == false).Count()
                    };
                    var chatMessages = new List<Message>();
                    chatMessages.AddRange(chat.MyMessages);
                    chatMessages.AddRange(chat.OtherMessages);
                    chat.LastMessage = chatMessages.OrderByDescending(x => x.SentAt).FirstOrDefault();

                    chats.Add(chat);
                }
            }      
            ViewData["sender"] = user.Id;
            return View(chats);
        }

        public async Task<List<User>> GetAllUsersInMessages(List<Message> allMessages)
        {
            // Get all sender and receiver IDs
            var userIds = allMessages.Select(x => x.Sender).Union(allMessages.Select(x => x.Receiver)).Distinct().ToList();
            
            // Query the database to get user information based on the retrieved IDs
            var users = await _db.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();

            return users;
        }

        [HttpGet]
        [Route("/Chat/GetChatMessages")]
        public async Task<IActionResult> GetChatMessages(string recipientName)
        {

            var account = HttpContext.Session.GetString("AccountId");
            if (account == null)
            {
                return Redirect("/Login"); 
            }

            var user = _db.Users.AsNoTracking().FirstOrDefault(x => x.Id == int.Parse(account));
            var recipient = _db.Users.AsNoTracking().FirstOrDefault(x => x.Id == int.Parse(recipientName));


            var messages = await _db.Messages
                                .Where(m => (m.Sender == user.Id && m.Receiver == recipient.Id) ||
                                            (m.Sender == recipient.Id && m.Receiver == user.Id))
                                .OrderBy(m => m.SentAt)
                                .ToListAsync();

            var readMess = _db.Messages.Where(x => x.Receiver == user.Id).ToList();
            foreach (var message in readMess)
            {
                message.Read = true;
                _db.Messages.Update(message);
            }
            _db.SaveChanges();

            var messageViewModels = messages.Select(m => new
            {
                text = m.Content,
                timestamp = m.SentAt.Value.ToShortTimeString(),
                isMyMessage = m.Sender == user.Id,
                recipientName = recipientName
            }).ToList();

            return Json(messageViewModels);
        }

    }
}
