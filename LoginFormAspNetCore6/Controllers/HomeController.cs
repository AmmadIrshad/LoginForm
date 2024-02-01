using LoginFormAspNetCore6.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LoginFormAspNetCore6.Controllers
{
    public class HomeController : Controller
    {
        private readonly CodeFirstApproachDbContext _context;

        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public HomeController(CodeFirstApproachDbContext context)
        {
            this._context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("user-session") != null)
            {
                return RedirectToAction("Dashboard");
            }
            return View();
        }
        [HttpPost]
        public IActionResult Login(UserTable user)
        {
            var myuser = _context.UserTables.Where(x=>x.Email == user.Email && x.Password == user.Password).FirstOrDefault(); 
            if (myuser != null) 
            {
                HttpContext.Session.SetString("user-session",myuser.Email);
                return RedirectToAction("Dashboard");
            }
            else
            {
                ViewBag.Message = "Login Failed...!";
                //for clear the input fields if login failed
                ModelState.Clear();
            }

            return View();
        }
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("user-session") != null)
            {
                ViewBag.Mysession = HttpContext.Session.GetString("user-session");
            }
            else
            {
                return RedirectToAction("Login");
            }
            return View();
        }
        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("user-session") != null)
            {
                HttpContext.Session.Remove("user-session");
                return RedirectToAction("Login");
            }
            return View();
        }

        public IActionResult Register()
        {
            if (HttpContext.Session.GetString("user-session") != null)
            {
                return RedirectToAction("Dashboard");
            }
            return View();
        }
        [HttpPost]
        public async Task< IActionResult> Register(UserTable user)
        {
            if (ModelState.IsValid)
            {
                await _context.UserTables.AddAsync(user);
                await _context.SaveChangesAsync();
                TempData["success"] = "User Registered Succcessfully";
                return RedirectToAction("Login");
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
