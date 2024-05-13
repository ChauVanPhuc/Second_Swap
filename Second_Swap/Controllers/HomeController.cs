using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Second_Swap.Models;
using Second_Swap.ViewModels;
using System.Diagnostics;

namespace Second_Swap.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SecondSwapContext _db;
        public HomeController(ILogger<HomeController> logger, SecondSwapContext db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Category> Category = await _db.Categories.ToListAsync();
            IEnumerable<Product> Product = await _db.Products.ToListAsync();
            IEnumerable<User> User = await _db.Users.ToListAsync();
            IEnumerable<Address> Address = await _db.Addresses.ToListAsync();
            IEnumerable<Province> Province = await _db.Provinces.ToListAsync();
            IEnumerable<District> District = await _db.Districts.ToListAsync();
            IEnumerable<Ward> Ward = await _db.Wards.ToListAsync();
            IEnumerable<ProductImage> ProductImage = await _db.ProductImages.ToListAsync();

            HomeDisplayModel model = new HomeDisplayModel 
            {

                Categorys = Category,
                Products = Product,
                Users = User,
                District = District,
                Province = Province,
                Ward = Ward,
                Address = Address   ,
                ProductImage = ProductImage


            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        [Route("/Home/GetDistricts")]
        public  ActionResult GetDistricts(int provinces_id)
        {
            var district = _db.Districts.Where(a => a.ProvinceId == provinces_id).ToList();
            if (district != null)
            {
                return Json(district);

            }
            return Json(false);
        }

        [HttpPost]
        [Route("/Home/GetBrand")]
        public ActionResult GetBrand(int categoryId)
        {
            var brands = _db.Brands.Where(a => a.CategoryId == categoryId).ToList();
            if (brands != null)
            {
                return Json(brands);

            }
            return Json(false);
        }


        [HttpPost]
        [Route("/Home/GetWards")]
        public ActionResult GetWards(int district_id)
        {
            var wards = _db.Wards.Where(a => a.DistrictId == district_id).ToList();
            if(wards != null)
            {
                return Json(wards);
            }
            return Json(false);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}