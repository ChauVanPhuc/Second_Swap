using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Second_Swap.Extension;
using Second_Swap.Models;
using Second_Swap.Service;
using Second_Swap.ViewModels;
using System.Security.Claims;

namespace Second_Swap.Controllers
{
    public class AuthenticationController : Controller
    {

        private readonly SecondSwapContext _db;
        private readonly INotyfService _notyf;
        private readonly IFileService _fileService;

        public AuthenticationController(SecondSwapContext db, INotyfService notyf, IFileService fileService)
        {
            _db = db;
            _notyf = notyf;
            _fileService = fileService;
        }



        [HttpPost]
        public async Task<IActionResult> ValidatePhone(string phone)
        {
            try
            {
                var account = _db.Users.AsNoTracking().SingleOrDefault(x => x.Phone.ToLower() == phone.ToLower());
                if (account == null)
                {

                    return Json(true);
                }
                else
                {
                    return Json($"Phone: {phone} already exist");
                }

            }
            catch
            {

                return Json(true);
            }
        }

        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public IActionResult ValidateEmail(string email)
        {
            try
            {
                var account = _db.Users.AsNoTracking().SingleOrDefault(x => x.Email.ToLower() == email.ToLower());
                if (account != null)
                {
                    return Json(data: "Email: " + email + " already exist");

                }
                return Json(data: true);
            }
            catch
            {

                return Json(data: true);
            }
        }

        [Route("/Login")]
        public IActionResult Login()
        {
            var account = HttpContext.Session.GetString("AccountId");
            if (account != null)
            {
                return Redirect("/");
            }
            return View();
        }

        [HttpPost]
        [Route("/Login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var account = _db.Users.Include(x => x.Role).AsNoTracking().SingleOrDefault(x => x.Email == model.Email);
                    if (account == null)
                    {
                        _notyf.Error("Account is not registered ");
                        return Redirect("/Login");
                    }

                    string password = HashMD5.ToMD5(model.Password);
                    if (account.Password != password)
                    {
                        _notyf.Error("Invalid information");
                        return View(model);
                    }

                    //check account disable ?
                    if (account.Status == false)
                    {
                        _notyf.Error("Account Disable");
                        return View(model);
                    }

                    //Save Session 
                    HttpContext.Session.SetString("AccountId", account.Id.ToString());
                    var accountId = HttpContext.Session.GetString("AccountId");

                    //Identity
                    var claim = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, account.FullName),
                        new Claim("AccountId", account.Id.ToString()),
                        new Claim(account.Role.Name,account.Role.Name),
                    };
                    ClaimsIdentity claims = new ClaimsIdentity(claim, "AccountId" );
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claims);
                    await HttpContext.SignInAsync( "AccountId", claimsPrincipal);
                    _notyf.Success("Login Success");
                    return CheckLogin(account);
                }
            }
            catch
            {
                _notyf.Success("Login Fail");
                return Redirect("/Login");
            }
            _notyf.Success("Login Fail");
            return View(model);
        }


        [Route("/Register")]
        public IActionResult Register()
        {
            ViewData["Provinces"] = new SelectList(_db.Provinces, "Id", "Name").ToList();
            return View();
        }


        [HttpPost]
        [Route("/Register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string img = null;
                    if (model.avatar != null)
                    {
                        var ext = Path.GetExtension(model.avatar.FileName);
                        var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };
                        if (!allowedExtensions.Contains(ext))
                        {
                            string msg = string.Format("Only {0} extensions are allowed", string.Join(",", allowedExtensions));
                            _notyf.Error(msg);
                            return View(model);
                        }
                        else
                        {
                            img = _fileService.SaveImage(model.avatar);
                        }
                    }


                    var role = _db.Roles.FirstOrDefault(x => x.Name == "Client");
                    int roles = 0;

                    if (role != null)
                    {
                        roles = role.Id;
                    }
                    else
                    {
                        Role r = new Role
                        {
                            Name = "Client",
                            Description = "Client"
                        };

                        _db.Roles.Add(r);
                        _db.SaveChanges();

                        roles = r.Id;
                    }

                    var address = new Address
                    {
                        ProvinceId = model.Province,
                        DistrictId = model.District,
                        WardsId = model.Wards

                    };
                    _db.Addresses.Add(address);
                    await _db.SaveChangesAsync();

                    User account = new User
                    {
                        Email = model.Email,
                        Password = HashMD5.ToMD5(model.Password),
                        Phone = model.Phone,
                        RoleId = roles,
                        Status = true,
                        CreateDay = DateTime.Now,
                        FullName = model.FullName,
                        Avatar = img,
                        BirthDay = model.BirthDay,
                        AddressId = address.Id,
                        Gender = model.Gender,
                    };

                   

                    try
                    {
                        _db.Add(account);
                        await _db.SaveChangesAsync();

                        //Save session 
                        HttpContext.Session.SetString("AccountId", account.Id.ToString());
                        var accountId = HttpContext.Session.GetString("AccountId");

                        //Identity
                        var claim = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, account.FullName),
                            new Claim("AccountId", account.Id.ToString())
                        };
                        ClaimsIdentity claims = new ClaimsIdentity(claim, "login");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claims);

                        await HttpContext.SignInAsync(claimsPrincipal);
                        _notyf.Success("Register Success");
                        return Redirect("/");
                    }
                    catch
                    {
                        return Redirect("/Register");
                    }
                }
                else
                {
                    _notyf.Error("Register Fail");
                    return View(model);
                }
            }
            catch
            {
                _notyf.Error("Register Fail");
                return View(model);
            }
        }

        [HttpPost]
        [Route("/Logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            HttpContext.Session.Clear();
            return Redirect("/");
        }

        public IActionResult CheckLogin(User accountId)
        {
            string role = CheckRole.CheckRoleLogin(accountId);
            if (role == "Admin")
            {
                return Redirect("/Admin");
            }
            else 
            {
                return Redirect("/");
            }
            
        }
    }
}
