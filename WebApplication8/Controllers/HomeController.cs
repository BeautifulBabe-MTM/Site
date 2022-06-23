using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication8.Models;
using MySql.Data.MySqlClient;
public class DBStuff
{
    public static List<Stuff> stuff { get; set; }
    public static List<Category> categories { get; set; }
    public static MySqlConnection connection { set; get; }
    static DBStuff()
    {
        string connString = "Data Source = SQL5109.site4now.net; Initial Catalog = db_a88e21_weed; User Id = db_a88e21_weed_admin; Password = 123zxc34";
        connection = new MySqlConnection(connString);

        stuff = new List<Stuff>();
        categories = new List<Category>();
    }
    public static void GetCategory()
    {
        connection.Open();
        string sql = "SELECT * FROM [Category];";
        MySqlCommand cmd = new MySqlCommand(sql, connection);
        MySqlDataReader reader = cmd.ExecuteReader();
        categories = new List<Category>();

        while (reader.Read())
        {
            int id = Convert.ToInt32(reader["id"]);
            string name = reader["name"].ToString();

            categories.Add(new Category(id, name));
        }
        connection.Close();
    }
    public static void GetStuff()
    {
        connection.Open();
        stuff = new List<Stuff>();
        string sql = "SELECT * FROM [Stuff];";
        MySqlCommand cmd = new MySqlCommand(sql, connection);
        MySqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            int id = int.Parse(reader["id"].ToString());
            float price = float.Parse(reader["price"].ToString());
            string name = reader["name"].ToString();
            string image = reader["name"].ToString();
            int category = Convert.ToInt32(reader["category"]);

            stuff.Add(new Stuff(id, price, image, name, category));
        }
        connection.Close();
    }
    public static void AddStuff(float price, string image, string name, int category)
    {
        connection.Open();
        MySqlCommand command = new MySqlCommand($"INSERT INTO [Stuff] ([Price], [Image], [Name], [Category]) VALUES ('{price}','{image}','{name}','{category}", connection);
        command.ExecuteNonQuery();
        connection.Close();
        GetStuff();
    }
    public static void AddCategory(string name)
    {
        connection.Open();
        MySqlCommand command = new MySqlCommand($"INSERT INTO [Category]([Name]) VALUES ('{name}')", connection);
        command.ExecuteNonQuery();
        connection.Close();
        GetCategory();
    }
    public static void DeleteCategory(int id)
    {
        connection.Open();
        MySqlCommand command = new MySqlCommand($"DELETE FROM [Category] WHERE `id` = {id};", connection);
        command.ExecuteNonQuery();
        connection.Close();
        GetCategory();
    }
    public static void DeleteStuff(int id)
    {
        connection.Open();
        MySqlCommand command = new MySqlCommand($"DELETE FROM [Stuff] WHERE [ID] = {id};", connection);
        command.ExecuteNonQuery();
        command = new MySqlCommand($"DELETE FROM [Cart] WHERE [Stuff_ID]={id};", connection);
        command.ExecuteNonQuery();
        connection.Close();
        GetStuff();
    }
}
namespace WebApplication8.Contollers 
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index(string q = "")
        {
            if (q == null)
                q = string.Empty;

            if (HttpContext.Session.GetString("UserID") == null)
                HttpContext.Session.SetString("UserID", "");

            ViewBag.Stuff = DBStuff.stuff.Where(p => p.Name.ToLower().Contains(q.ToLower())).ToList();
            ViewBag.Categories = DBStuff.categories;


            ViewBag.Login = HttpContext.Session.GetString("UserID");

            ViewBag.Stf = new List<string>();
            ViewBag.Ctg = new List<string>();

            return View();
        }
        [HttpPost]
        public IActionResult Index(string[] category, string[] genre, string[] author, string[] publisher)
        {
            if (HttpContext.Session.GetString("UserID") == null)
                HttpContext.Session.SetString("UserID", "");

            var data_s = author.ToList();
            var data_c = category.ToList();

            ViewBag.Stf = data_s;
            ViewBag.Ctg = data_c;

            ViewBag.Stuff = DBStuff.stuff
                .Where((item) => data_s.Any(a => a == item.ID.ToString()) || data_s.Count == 0)
                .Where((item) => data_c.Any(c => c == item.Category_id.ToString()) || data_c.Count == 0);
            ViewBag.Categories = DBStuff.categories;

            ViewBag.Login = HttpContext.Session.GetString("UserID");

            return View();
        }
        [HttpGet]
        public IActionResult Stuff(int id)
        {
            ViewBag.Login = HttpContext.Session.GetString("UserID");
            ViewBag.CurrentStuff = DBStuff.stuff.Where(p => p.ID == id).First();


            if (HttpContext.Session.GetString("UserID") != null)
            {
                if (HttpContext.Session.GetString("UserID") != string.Empty)
                    ViewBag.InCart = UserOptions.GetCart(int.Parse(HttpContext.Session.GetString("UserID"))).Any(i => i == id);
                else
                    ViewBag.InCart = false;
            }
            else
                ViewBag.InCart = false;


            ViewBag.Stuff = DBStuff.stuff;
            ViewBag.Categories = DBStuff.categories;

            return View();
        }

        [HttpPost]
        public IActionResult Stuff(int id, int other = 0)
        {
            ViewBag.Login = HttpContext.Session.GetString("UserID");

            if (HttpContext.Session.GetString("UserID") == string.Empty)
                return Redirect("../Profile/LogIn");

            else
                UserOptions.AddToCart(id, int.Parse(HttpContext.Session.GetString("UserID")));

            ViewBag.InCart = UserOptions.GetCart(int.Parse(HttpContext.Session.GetString("UserID"))).Any(i => i == id);

            ViewBag.CurrentStuff = DBStuff.stuff.Where(p => p.ID == id).First();

            ViewBag.Stuff = DBStuff.stuff;
            ViewBag.Categories = DBStuff.categories;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
