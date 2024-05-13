using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Second_Swap.Areas.Admin.ViewModels;
using Second_Swap.Models;
using Second_Swap.Service;

namespace Second_Swap.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly SecondSwapContext _db;
        private readonly IFileService _fileService;
        private readonly INotyfService _notyf;

        public CategoryController(SecondSwapContext db, IFileService fileService, INotyfService notyf)
        {
            _db = db;
            _fileService = fileService;
            _notyf = notyf;
        }


        [Route("/Admin/Category")]
        public IActionResult Index()
        {
            IEnumerable<Category> ds = _db.Categories.ToList();

            return View(ds);
        }

        [Route("/Admin/Category/Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("/Admin/Category/Create")]
        public IActionResult Create(CategoryViewModel category)
        {

            if (ModelState.IsValid)
            {
                string img = "";
                if (category.img != null)
                {
                    img = _fileService.SaveImage(category.img);
                }

                Category cate = new Category 
                {
                    Name = category.Name,
                    Description = category.Description,
                    Status = true,
                    CreateDay = DateTime.Now,
                    Avatar = img
                };

                _db.Categories.Add(cate);
                _db.SaveChanges();
                _notyf.Success("Create Success");
                return RedirectToAction("Index");
            }
            return View(category);
        }

        [Route("/Admin/Category/Edit/{id:}")]
        public IActionResult Edit(int id)
        {
            Category category = _db.Categories.Find(id);

            if (category == null)
            {
                _notyf.Error("Find Not Success");
                return RedirectToAction("Index");
            }
            
            
            CategoryViewModel model = new CategoryViewModel
            {
                Name = category.Name,
                Description = category.Description,
                Avatar = category.Avatar
            };
            return View(model);
        }

        [HttpPost]
        [Route("/Admin/Category/Edit/{id:}")]
        public IActionResult Edit(int id, CategoryViewModel category)
        {
            string img = "";
            if (ModelState.IsValid)
            {
                try
                {
                    var cate = _db.Categories.Find(id);
                    var imgold = cate.Avatar;

                    if (category.img != null)
                    {
                        img = _fileService.SaveImage(category.img);
                        
                    }
                    else
                    {
                        img = cate.Avatar;
                    }
                   

                    if (cate != null)
                    {
                        
                        cate.Name = category.Name;
                        cate.Description = category.Description;
                        cate.LastUpdated = DateTime.Now;
                        cate.Avatar = img;


                        _db.Categories.Update(cate);
                        _db.SaveChanges();
                        _notyf.Success("Update Success");


                        if (category.img != null)
                        {
                            if (imgold != null)
                            {
                                _fileService.Delete(imgold);
                            }

                        }

                        return RedirectToAction("Index");
                    }

                }
                catch 
                {
                    if (category.img != null)
                    {
                        _fileService.Delete(img);
                    }
                    _notyf.Success("Update Faill");
                    return View(category);

                }
                

                
            }
            return View(category);
        }


        [Route("/Admin/Category/Delete/{id:}")]
        public ActionResult Delete(int id)
        {
            Category category =  _db.Categories.Find(id);
            if (category == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                if (category.Status == true)
                {
                    category.Status = false;
                }
                else
                {
                    category.Status = true;
                }
                
                _db.Categories.Update(category);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }
}
