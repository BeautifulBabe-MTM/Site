using Microsoft.AspNetCore.Mvc;
using WebApplication8.Models;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using System.IO;
using System.Text;

namespace WebApplication8.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        public AdminController(ILogger<AdminController> logger)
        {
            _logger = logger;
        }
        //[HttpGet]
        //public IActionResult Menu()
        //{
        //    if (HttpContext.Session.GetString("UserID") != null)
        //    {
        //        if (HttpContext.Session.GetString("UserID") != string.Empty)
        //        {
        //            User user = UserOptions.GetUser(int.Parse(HttpContext.Session.GetString("UserID")));
        //            ViewBag.Login = user.Login;
        //        }
        //        else
        //        {
        //            ViewBag.Login = string.Empty;
        //        }
        //    }
        //    else
        //        ViewBag.Login = string.Empty;

        //    return View();
        //}
        [HttpGet]
        public IActionResult AddStuff()
        {
            if (HttpContext.Session.GetString("UserID") != null)
            {
                if (HttpContext.Session.GetString("UserID") != string.Empty)
                {
                    User user = new User(int.Parse(HttpContext.Session.GetString("UserID")), "admin123", "123zxc34");
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

            return View();
        }

        [HttpGet]
        public IActionResult AddCategory()
        {

            return View();
        }

        [HttpPost]
        public IActionResult AddCategory(string name)
        {

            DBStuff.AddCategory(name);
            return View();
        }
        [HttpPost]
        public IActionResult DeleteCategory(int id)
        {

            if (DBStuff.stuff.Where(p => p.Category_id == id).Count() == 0)
                DBStuff.DeleteCategory(id);

            ViewBag.Categories = DBStuff.categories;
            return View();
        }


        [HttpGet]
        public IActionResult DeleteCategory()
        {
            ViewBag.Categories = DBStuff.categories;
            return View();
        }
        [HttpGet]
        public IActionResult DeleteStuff()
        {
            ViewBag.Stuff = DBStuff.stuff;
            ViewBag.Categories = DBStuff.categories;

            return View();
        }

        [HttpPost]
        public IActionResult DeleteStuff(int stuff_id)
        {
            DBStuff.DeleteStuff(stuff_id);

            ViewBag.Stuff = DBStuff.stuff;
            ViewBag.Categories = DBStuff.categories;

            return View();
        }

    }
}
