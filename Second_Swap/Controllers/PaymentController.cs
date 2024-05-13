using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Second_Swap.Models;
using Second_Swap.Service;
using Second_Swap.ViewModels;

namespace Second_Swap.Controllers
{
    public class PaymentController : Controller
    {

        private readonly SecondSwapContext _db;
        private readonly INotyfService _notyf;
        private readonly IFileService _fileService;
        private readonly IVnPayService _vnPayservice;


        public PaymentController(SecondSwapContext db, INotyfService notyf, IFileService fileService, IVnPayService vnPayservice)
        {
            _db = db;
            _notyf = notyf;
            _fileService = fileService;
            _vnPayservice = vnPayservice;
        }



        public IActionResult Payment(string token, string email)
        {
            var order = _db.Orders.Include(x => x.Buyer).Include(x => x.Product).AsNoTracking().FirstOrDefault(x => x.PaymentToken == token);

            if (order.PaymentToken == null)
            {
                return Redirect("/AccessDenied");
            }

            var vnPayModel = new VnPaymentRequestModel
            {
                Amount = (double)order.Product.Price,
                CreatedDate = DateTime.Now,
                Description = order.Message,
                FullName = order.Buyer.FullName,
                OrderId = order.Id
            };
            return Redirect(_vnPayservice.CreatePaymentUrl(HttpContext, vnPayModel));
        }

        public IActionResult PaymentCallBack()
        {
            var response = _vnPayservice.PaymentExecute(Request.Query);

            if (response == null || response.VnPayResponseCode != "00")
            {
                return RedirectToAction("PaymentFail");
            }
            else
            {

                return RedirectToAction("PaymentSuccess");
            }
        }


        public IActionResult PaymentFail()
        {
            return View();
        }

        public IActionResult PaymentSuccess()
        {
            return View();
        }
    }
}
