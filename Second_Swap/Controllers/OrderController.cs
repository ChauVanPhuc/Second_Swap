using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Second_Swap.Models;
using Second_Swap.Service;
using Second_Swap.ViewModels;

namespace Second_Swap.Controllers
{
    public class OrderController : Controller
    {
        private readonly SecondSwapContext _db;
        private readonly INotyfService _notyf;
        private readonly IFileService _fileService;
        private readonly IVnPayService _vnPayservice;
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;

        public OrderController(SecondSwapContext db, INotyfService notyf, IFileService fileService,
            IVnPayService vnPayservice, IEmailService emailService, IUserService userService)
        {
            _db = db;
            _notyf = notyf;
            _fileService = fileService;
            _vnPayservice = vnPayservice;
            _emailService = emailService;
            _userService = userService;
        }


        [Route("/Post/AddOrder")]
        public IActionResult AddOrder(BuyProductViewModel model, int proId)
        {
            var user = HttpContext.Session.GetString("AccountId");
            if (user == null) return Redirect("/Login");
            
            var buyer = _db.Users.FirstOrDefault(x => x.Id == int.Parse(user));

            var order = new Order
            {
                ProductId = proId,
                BuyerId = buyer.Id,
                MethodPayment = model.MethodPayment,
                Status = "Processing",
                CreateAt = DateTime.Now
            };

            _db.Orders.Add(order);
            _db.SaveChanges();

            _notyf.Success("Add Order Success");

            return Redirect("/");
        }

        [Route("/Order/Approved/{id:}")]
        public async Task<IActionResult> Approved(int id)
        {
            
            var order = _db.Orders.Include(x => x.Buyer).FirstOrDefault(x => x.Id == id);


            if (order == null)
            {
                return NotFound();
            }

            _notyf.Success("Approved Order Success");

            var EmailBuyer = order.Buyer.Email;

            if (EmailBuyer != null)
            {
                var token = await _userService.GeneratePasswordResetTokenAsync(order);
                var callbackUrl = Url.Action("Payment", "Payment", new { token, email = EmailBuyer }, Request.Scheme);

                await _emailService.SendEmailAsync(EmailBuyer, "Approved Order", order.Buyer.FullName + " Your order has been Approved: "+callbackUrl+ ". Time 24 Hours");
            }

            return Redirect("/");
        }

        [Route("/Order/Reject/{id:}")]
        public async Task<IActionResult> Reject(int id)
        {

            var order = _db.Orders.FirstOrDefault(x => x.Id == id);


            if (order == null)
            {
                return NotFound();
            }

            order.Status = "Reject";
            _db.Orders.Add(order);
            _db.SaveChanges();

            _notyf.Success("Reject Order Success");

            var EmailBuyer = order.Buyer.Email;

            if (EmailBuyer != null)
            {
                await _emailService.SendEmailAsync(EmailBuyer, "Reject Order", order.Buyer.FullName + " Your order has been Reject");
            }

            return Redirect("/");
        }
    }
}
