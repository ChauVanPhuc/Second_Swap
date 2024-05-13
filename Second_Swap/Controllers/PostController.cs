using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Second_Swap.Models;
using Second_Swap.Service;
using Second_Swap.ViewModels;

namespace Second_Swap.Controllers
{
    public class PostController : Controller
    {


        private readonly SecondSwapContext _db;
        private readonly INotyfService _notyf;
        private readonly IFileService _fileService;
        private readonly IVnPayService _vnPayservice;


        public PostController(SecondSwapContext db, INotyfService notyf, IFileService fileService, IVnPayService vnPayservice)
        {
            _db = db;
            _notyf = notyf;
            _fileService = fileService;
            _vnPayservice = vnPayservice;
        }

        [HttpPost]
        [Route("/Post/GetBrand")]
        public ActionResult GetBrand(int category_id)
        {
            var brands = _db.Brands.Where(a => a.CategoryId == category_id).ToList();
            if (brands != null)
            {
                return Json(brands);

            }
            return Json(false);
        }

        [Route("/Post/AddPost")]
        public IActionResult AddPost()
        {
            ViewData["CategoryId"] = new SelectList(_db.Categories, "Id", "Name").ToList();
            return View();
        }


        [HttpPost]
        [Route("/Post/AddPost")]
        public async Task<IActionResult> AddPost(PostViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = HttpContext.Session.GetString("AccountId");

                Product product = new Product
                {
                    Title = model.Title,
                    Description = model.Description,
                    Price = model.Price,
                    Status = true,
                    CreateDay = DateTime.Now,
                    CreateBy = int.Parse(user),
                    CategoryId = model.CategoryId,
                    Need = true,
                    BrandId = model.BrandId,
                    IsActive = true
                };

                _db.Products.Add(product);
                _db.SaveChanges();

                if (model.Img != null)
                {
                    foreach (var productFile in model.Img)
                    {
                        var ext = Path.GetExtension(productFile.FileName);
                        var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };

                        if (allowedExtensions.Contains(ext))
                        {
                            var imgFile = new ProductImage
                            {
                                Url = _fileService.SaveImage(productFile),
                                ProductId = product.Id,
                            };
                            _db.ProductImages.Add(imgFile);
                        }
                        else
                        {
                            ViewData["CategoryId"] = new SelectList(_db.Categories, "Id", "Name").ToList();
                            _notyf.Error("Only accepts Images");
                            return View(model);
                        }
                    }
                    _db.SaveChanges();
                }
                ViewData["CategoryId"] = new SelectList(_db.Categories, "Id", "Name").ToList();
                _notyf.Success("Add Post Success");
                return Redirect("/");
            }
            ViewData["CategoryId"] = new SelectList(_db.Categories, "Id", "Name").ToList();
            _notyf.Error("Add Faill");
            return View(model);
        }



        [Route("/Post/Detail/{id:}")]
        public async Task<IActionResult> Detail(int id)
        {
            var user = HttpContext.Session.GetString("AccountId");


            Product Product = _db.Products.Include(x => x.ProductImages).AsNoTracking().SingleOrDefault(x => x.Id == id);
            IEnumerable<Category> Category = await _db.Categories.ToListAsync();
            IEnumerable<Brand> Brand = await _db.Brands.ToListAsync();
            var User = _db.Users.AsNoTracking().SingleOrDefault(x => x.Id == int.Parse(user));
            var addressId = User.AddressId;
            IEnumerable<Address> Address = await _db.Addresses.Where(x => x.Id == addressId).ToListAsync();
            IEnumerable<Province> Province = await _db.Provinces.ToListAsync();
            IEnumerable<District> District = await _db.Districts.ToListAsync();
            IEnumerable<Ward> Ward = await _db.Wards.ToListAsync();



            PostDisplayModel model = new PostDisplayModel
            {
                Products = Product,
                Categorys = Category,
                Brands = Brand,
                Address = Address,
                Province = Province,
                Districts = District,
                Wards = Ward
            };

            return View(model);
        }

        [Route("/Post/PostDetail/{id:}")]
        public async Task<IActionResult> PostDetail(int id)
        {
            Product Product = _db.Products.Include(x => x.ProductImages).AsNoTracking().SingleOrDefault(x => x.Id == id);
            IEnumerable<Category> Category = await _db.Categories.ToListAsync();
            IEnumerable<Brand> Brand = await _db.Brands.ToListAsync();
            User User = _db.Users.AsNoTracking().SingleOrDefault(x => x.Id == Product.CreateBy);
            var addressId = User.AddressId;
            IEnumerable<Address> Address = await _db.Addresses.Where(x => x.Id == addressId).ToListAsync();
            IEnumerable<Province> Province = await _db.Provinces.ToListAsync();
            IEnumerable<District> District = await _db.Districts.ToListAsync();
            IEnumerable<Ward> Ward = await _db.Wards.ToListAsync();



            PostDisplayModel model = new PostDisplayModel
            {
                Products = Product,
                Categorys = Category,
                Brands = Brand,
                Address = Address,
                Province = Province,
                Districts = District,
                Wards = Ward,
                Users = User
            };

            return View(model);
        }

        [Route("/Post/Edit/{id:}")]
        public IActionResult Edit(int id)
        {
            var product = _db.Products
                    .Include(x => x.ProductImages)
                    .Include(x => x.Category)
                    .Include(x => x.Brand)
                    .FirstOrDefault(b => b.Id == id);

            if (product == null)
            {
                _notyf.Error("product not exist");
                return RedirectToAction("Index");
            }

            PostEditModel model = new PostEditModel
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                Price = product.Price,
                Status = product.Status,
                CreateDay = product.CreateDay,
                CreateBy = product.CreateBy,

                CategoryId = product.CategoryId,
                BrandId = product.BrandId,
                Need = product.Need,
                IsActive = product.IsActive,

                ProductImages = product.ProductImages
            };

            ViewData["CategoryId"] = new SelectList(_db.Categories, "Id", "Name").ToList();
            ViewData["BrandId"] = new SelectList(_db.Categories, "Id", "Name").ToList();
            return View(model);
        }


        [HttpPost]
        [Route("/Post/Edit/{id:}")]
        public IActionResult Edit(PostEditModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var product = _db.Products
                    .Include(x => x.ProductImages)
                    .Include(x => x.Category)
                    .Include(x => x.Brand)
                    .FirstOrDefault(b => b.Id == model.Id);

                    product.Title = model.Title;
                    product.Description = model.Description;
                    product.Price = model.Price;
                    product.CategoryId = model.CategoryId;
                    product.BrandId = model.BrandId;


                    string img = null;
                    if (model.Img != null)
                    {
                        foreach (var file in model.Img)
                        {
                            var ext = Path.GetExtension(file.FileName);
                            var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };
                            if (allowedExtensions.Contains(ext))
                            {
                                var imgFile = new ProductImage
                                {
                                    Url = _fileService.SaveImage(file),
                                    ProductId = model.Id
                                };
                                _db.ProductImages.Add(imgFile);

                            }
                            else
                            {
                                _notyf.Error("Only accepts Images files 'jpg' 'jpeg' and '.png' ");
                                return View(model);
                            }
                        }
                    }

                    _db.Products.Update(product);
                    _db.SaveChanges();

                    _notyf.Success("Update Post Success");
                    return Redirect("/Post/Detail/"+model.Id+"");
                }
                catch
                {
                    _notyf.Error("Update Post Fail");
                    ViewData["BrandId"] = new SelectList(_db.Categories, "Id", "Name").ToList();
                    ViewData["CategoryId"] = new SelectList(_db.Categories, "Id", "Name").ToList();
                    return View(model);
                }
            }
            else
            {
                _notyf.Error("Update Post Fail");
                ViewData["BrandId"] = new SelectList(_db.Categories, "Id", "Name").ToList();
                ViewData["CategoryId"] = new SelectList(_db.Categories, "Id", "Name").ToList();
                return View(model);
            }
        }

        [Route("/Post/isActive/{id:}")]
        public IActionResult isActive(int id)
        {
            var post = _db.Products.Find(id);

            if (post == null)
            {
                return NotFound();
            }
            else
            {
                if (post.IsActive == true)
                {
                    post.IsActive = false;
                }
                else
                {
                    post.IsActive = true;
                }
                
                _db.Products.Update(post);
                _db.SaveChanges();

                return Redirect("/Post/Detail/"+id+" ");
            }
        }

        [Route("/Post/Edit/DeleteFile")]
        public ActionResult DeleteFile(string UrlFile, int productId, int imgId)
        {
            _fileService.Delete(UrlFile);

            var file = _db.ProductImages.Find(imgId);
            if (file != null)
            {
                _db.ProductImages.Remove(file);
                _db.SaveChanges();
            }

            return Redirect("/Post/Edit/" + productId + "");
        }


		[Route("/Post/BuyProduct/{id:}")]
        public IActionResult BuyProduct(int id)
        {
            var product = _db.Products.Find(id);
			var user = HttpContext.Session.GetString("AccountId");
            User u = _db.Users.FirstOrDefault(x => x.Id == int.Parse(user));

			BuyProductViewModel model = new BuyProductViewModel
            {
                ProductName = product.Title,
                ProductPrice = (double)product.Price,
                ProductId = product.Id,
                Buyer = u.Id
            };

            return View(model);
        }


        
    }
}
