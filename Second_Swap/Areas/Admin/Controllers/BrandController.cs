using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Second_Swap.Areas.Admin.ViewModels;
using Second_Swap.Models;
using Second_Swap.Service;

namespace Second_Swap.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController : Controller
    {
        private readonly SecondSwapContext _db;
        private readonly IFileService _fileService;
        private readonly INotyfService _notyf;

        public BrandController(SecondSwapContext db, IFileService fileService, INotyfService notyf)
        {
            _db = db;
            _fileService = fileService;
            _notyf = notyf;
        }

        [Route("/Admin/Brand")]
        public async Task<IActionResult> Index()
        {
            var brands = await _db.Brands
                .Include(x => x.Category)
                .OrderByDescending(x => x.Id)
                .ToListAsync();

            return View(brands);
        }

        [Route("/Admin/Brand/Create")]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_db.Categories, "Id", "Name").ToList();
            return View();
        }

        [HttpPost]
        [Route("/Admin/Brand/Create")]
        public IActionResult Create(BrandViewModel model)
        {

            if (ModelState.IsValid)
            {
                string img = "";
                if (model.img != null)
                {
                    img = _fileService.SaveImage(model.img);
                }

                Brand brand = new Brand
                {
                    Name = model.Name,
                    Description = model.Description,
                    Status = true,
                    Created = DateTime.Now,
                    Avatar = img,
                    CategoryId = model.CategoryId,
                    
                };

                _db.Brands.Add(brand);
                _db.SaveChanges();
                _notyf.Success("Create Success");
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [Route("/Admin/Brand/Edit/{id:}")]
        public IActionResult Edit(int id)
        {
            Brand brand = _db.Brands.Find(id);

            if (brand == null)
            {
                _notyf.Error("Find Not Success");
                return RedirectToAction("Index");
            }


            BrandViewModel model = new BrandViewModel
            {
                Name = brand.Name,
                Description = brand.Description,
                CategoryId = brand.CategoryId,
                Avatar = brand.Avatar
            };
            ViewData["CategoryId"] = new SelectList(_db.Categories, "Id", "Name").ToList();
            return View(model);
        }

        [HttpPost]
        [Route("/Admin/Brand/Edit/{id:}")]
        public IActionResult Edit(int id, BrandViewModel model)
        {
            string img = "";
            if (ModelState.IsValid)
            {
                try
                {
                    var brand = _db.Brands.Find(id);

                    var imgold = brand.Avatar;
                    if (model.img != null)
                    {
                        img = _fileService.SaveImage(model.img);
                    }
                    else
                    {
                        img = brand.Avatar;
                    }
                    
                    

                    if (brand != null)
                    {
                        brand.Name = model.Name;
                        brand.Description = model.Description;
                        brand.CategoryId = model.CategoryId;
                        brand.LastUpdated = DateTime.Now;
                        brand.Avatar = img;


                        _db.Brands.Update(brand);
                        _db.SaveChanges();
                        _notyf.Success("Update Success");

                        if (model.img != null)
                        {
                            if (imgold!=null)
                            {
                                _fileService.Delete(imgold);
                            }
                            
                        }
                        return RedirectToAction("Index");
                    }

                }
                catch
                {
                    if (model.img != null)
                    {
                        _fileService.Delete(img);
                    }
                    _notyf.Success("Update Faill");
                    return View(model);

                }



            }
            return View(model);
        }


        [Route("/Admin/Brand/Delete/{id:}")]
        public ActionResult Delete(int id)
        {
            Brand brand = _db.Brands.Find(id);
            if (brand == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                if (brand.Status == true)
                {
                    brand.Status = false;
                }
                else
                {
                    brand.Status = true;
                }

                _db.Brands.Update(brand);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }
}
