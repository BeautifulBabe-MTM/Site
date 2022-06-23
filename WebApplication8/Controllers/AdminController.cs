using Microsoft.AspNetCore.Mvc;
using WebApplication8.Models;

namespace WebApplication8.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        public AdminController(ILogger<AdminController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Menu()
        {
            if (HttpContext.Session.GetString("UserID") != null)
            {
                if (HttpContext.Session.GetString("UserID") != string.Empty)
                {
                    User user = UserOptions.GetUser(int.Parse(HttpContext.Session.GetString("UserID")));
                    ViewBag.Login = user.Login;
                }
                else
                {
                    ViewBag.Login = string.Empty;
                }
            }
            else
                ViewBag.Login = string.Empty;

            return View();
        }
        [HttpGet]
        public IActionResult AddStuff()
        {
            if (HttpContext.Session.GetString("UserID") != null)
            {
                if (HttpContext.Session.GetString("UserID") != string.Empty)
                {
                    User user = UserOptions.GetUser(int.Parse(HttpContext.Session.GetString("UserID")));
                    ViewBag.Login = user.Login;
                }
                else
                    ViewBag.Login = string.Empty;
            }
            else
                ViewBag.Login = string.Empty;

            ViewBag.Categories = DBStuff.categories;

            return View();
        }
        [HttpPost]
        public IActionResult AddStuff(float price, string image, string name, int category)
        {
            ViewBag.Categories = DBStuff.categories;

            DBStuff.AddStuff(price, image, name, category);

            if (HttpContext.Session.GetString("UserID") != null)
            {
                if (HttpContext.Session.GetString("UserID") != string.Empty)
                {
                    User user = UserOptions.GetUser(int.Parse(HttpContext.Session.GetString("UserID")));
                    ViewBag.Login = user.Login;
                }
                else
                {
                    ViewBag.Login = string.Empty;
                }
            }
            else
            {
                ViewBag.Login = string.Empty;
            }

            return View();
        }

        [HttpGet]
        public IActionResult AddCategory()
        {
            if (HttpContext.Session.GetString("UserID") != null)
            {
                if (HttpContext.Session.GetString("UserID") != string.Empty)
                {
                    User user = UserOptions.GetUser(int.Parse(HttpContext.Session.GetString("UserID")));
                    ViewBag.Login = user.Login;
                }
                else
                    ViewBag.Login = string.Empty;
            }
            else
                ViewBag.Login = string.Empty;

            return View();
        }

        [HttpPost]
        public IActionResult AddCategory(string name)
        {
            if (HttpContext.Session.GetString("UserID") != null)
            {
                if (HttpContext.Session.GetString("UserID") != string.Empty)
                {
                    User user = UserOptions.GetUser(int.Parse(HttpContext.Session.GetString("UserID")));
                    ViewBag.Login = user.Login;
                }
                else
                    ViewBag.Login = string.Empty;
            }
            else
                ViewBag.Login = string.Empty;

            DBStuff.AddCategory(name);
            return View();
        }
        [HttpPost]
        public IActionResult DeleteCategory(int id)
        {
            if (HttpContext.Session.GetString("UserID") != null)
            {
                if (HttpContext.Session.GetString("UserID") != string.Empty)
                {
                    User user = UserOptions.GetUser(int.Parse(HttpContext.Session.GetString("UserID")));
                    ViewBag.Login = user.Login;
                }
                else
                    ViewBag.Login = string.Empty;
            }
            else
                ViewBag.Login = string.Empty;

            if (DBStuff.stuff.Where(p => p.Category_id == id).Count() == 0)
                DBStuff.DeleteCategory(id);

            ViewBag.Categories = DBStuff.categories;
            return View();
        }


        [HttpGet]
        public IActionResult DeleteCategory()
        {
            if (HttpContext.Session.GetString("UserID") != null)
            {
                if (HttpContext.Session.GetString("UserID") != string.Empty)
                {
                    User user = UserOptions.GetUser(int.Parse(HttpContext.Session.GetString("UserID")));
                    ViewBag.Login = user.Login;
                }
                else
                    ViewBag.Login = string.Empty;
            }
            else
                ViewBag.Login = string.Empty;


            ViewBag.Categories = DBStuff.categories;
            return View();
        }
        [HttpGet]
        public IActionResult DeleteStuff()
        {
            if (HttpContext.Session.GetString("UserID") != null)
            {
                if (HttpContext.Session.GetString("UserID") != string.Empty)
                {
                    User user = UserOptions.GetUser(int.Parse(HttpContext.Session.GetString("UserID")));
                    ViewBag.Login = user.Login;
                }
                else
                {
                    ViewBag.Login = string.Empty;
                }
            }
            else
            {
                ViewBag.Login = string.Empty;
            }

            ViewBag.Stuff = DBStuff.stuff;
            ViewBag.Categories = DBStuff.categories;

            return View();
        }

        [HttpPost]
        public IActionResult DeleteStuff(int stuff_id)
        {
            DBStuff.DeleteStuff(stuff_id);

            if (HttpContext.Session.GetString("UserID") != null)
            {
                if (HttpContext.Session.GetString("UserID") != string.Empty)
                {
                    User user = UserOptions.GetUser(int.Parse(HttpContext.Session.GetString("UserID")));
                    ViewBag.Login = user.Login;
                }
                else
                {
                    ViewBag.Login = string.Empty;
                }
            }
            else
            {
                ViewBag.Login = string.Empty;
            }

            ViewBag.Stuff = DBStuff.stuff;
            ViewBag.Categories = DBStuff.categories;

            return View();
        }

    }
}
