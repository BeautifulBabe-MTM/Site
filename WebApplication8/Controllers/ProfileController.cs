using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using WebApplication8.Models;

public static class UserOptions
{
    public static MySqlConnection connection { set; get; }
    static UserOptions()
    {
        string connString = "Data Source = SQL5109.site4now.net; Initial Catalog = db_a88e21_weed; User Id = db_a88e21_weed_admin; Password = 123zxc34";
        connection = new MySqlConnection(connString);
    }
    public static int GetUserID(string login, string password)
    {
        connection.Open();
        int result = -1;
        try
        {
            string sql = $"SELECT * FROM [Users] WHERE [Login] = [{login}] AND [Pass]=[{password}];";
            MySqlCommand cmd = new MySqlCommand(sql, connection);
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int id = Convert.ToInt32(reader["id"]);
                result = id;
            }
        }
        catch (Exception)
        {
            result = -1;
        }
        connection.Close();

        return result;
    }
    public static User GetUser(int user_id)
    {
        connection.Open();
        User result = new User(0, "", "");
        try
        {
            string sql = $"SELECT * FROM [Users] WHERE [ID]={user_id};";
            MySqlCommand cmd = new MySqlCommand(sql, connection);
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int id = Convert.ToInt32(reader["id"]);
                string login = reader["login"].ToString();
                string pass = reader["pass"].ToString();

                result = new User(id, login, pass);
            }

        }
        catch (Exception)
        {

        }
        connection.Close();

        return result;
    }

    public static bool RegisterUser(string login, string pass)
    {
        bool result = true;

        connection.Open();

        try
        {
            string sql = $"INSERT INTO [User]([Login], [Pass], [Root]) VALUES ('{login}','{pass}', 0');";
            MySqlCommand cmd = new MySqlCommand(sql, connection);
            cmd.ExecuteNonQuery();
        }
        catch (Exception)
        {
            result = false;
        }
        connection.Close();

        return result;

    }

    public static void AddToCart(int user_id, int stuff_id)
    {
        connection.Open();

        string sql = $"INSERT INTO [Cart]([User_ID], [Product_ID]) VALUES ('{user_id}','{stuff_id}');";
        MySqlCommand cmd = new MySqlCommand(sql, connection);
        cmd.ExecuteNonQuery();

        connection.Close();
    }
    public static List<int> GetCart(int user_id)
    {
        connection.Open();

        string sql = $"SELECT * FROM [Cart] WHERE [User_ID] = '{user_id}';";
        MySqlCommand cmd = new MySqlCommand(sql, connection);
        MySqlDataReader rdr = cmd.ExecuteReader();
        List<int> cart = new List<int>();
        while (rdr.Read())
        {
            int id = Convert.ToInt32(rdr["id"]);
            int stuff_id = Convert.ToInt32(rdr["stuff_id"]);

            cart.Add(user_id);
        }
        connection.Close();

        return cart;
    }

    public static void RemoveFromCart(int stuff_id, int user_id)
    {
        connection.Open();

        string sql = $"DELETE FROM [Cart] WHERE [Product_ID]={stuff_id} AND [User_ID]={user_id};";
        MySqlCommand cmd = new MySqlCommand(sql, connection);
        cmd.ExecuteNonQuery();

        connection.Close();
    }

}

namespace WebApplication8.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ILogger<ProfileController> _logger;

        public ProfileController(ILogger<ProfileController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            ViewBag.Login = string.Empty;
            return View();
        }
        public IActionResult Profile()
        {
            if (HttpContext.Session.GetString("UserID") == null)
            {
                return Redirect("../Profile/LogIn");
            }
            if (HttpContext.Session.GetString("UserID") == string.Empty)
            {
                return Redirect("../Profile/LogIn");
            }

            User user = UserOptions.GetUser(int.Parse(HttpContext.Session.GetString("UserID")));
            ViewBag.Login = user.Login;

            ViewBag.User = user;

            return View();
        }


        [HttpPost]
        public RedirectResult LogIn(string username, string password)
        {
            int user_id = UserOptions.GetUserID(username, password);
            if (user_id != -1)
            {
                HttpContext.Session.SetString("UserID", user_id.ToString());
                return Redirect("/Profile/Profile");
            }
            return Redirect("/Profile/LogIn");
        }
        [HttpGet]
        public IActionResult SignIn()
        {
            ViewBag.Login = string.Empty;
            return View();
        }

        [HttpPost]
        public RedirectResult SignIn(string login, string pass)
        {
            ViewBag.Login = string.Empty;
            if ((login.Length > 4 && login.Length < 16) && (pass.Length > 6 && pass.Length < 20))
            {
                if (UserOptions.RegisterUser(login, pass))
                {
                    return Redirect("/Profile/LogIn");
                }
            }
            return Redirect("/Profile/SignIn");
        }
        [HttpGet]
        public IActionResult Cart()
        {

            if (HttpContext.Session.GetString("UserID") == null)
            {
                return Redirect("../Profile/LogIn");
            }
            if (HttpContext.Session.GetString("UserID") == string.Empty)
            {
                return Redirect("../Profile/LogIn");
            }

            if (HttpContext.Session.GetString("UserID") != string.Empty)
            {
                ViewBag.Login = UserOptions.GetUser(int.Parse(HttpContext.Session.GetString("UserID"))).Login;

                ViewBag.cartlist = DBStuff.stuff.Where((b) => UserOptions.GetCart(int.Parse(HttpContext.Session.GetString("UserID"))).Any((i) => i == b.ID)).ToList();

            }
            else
            {
                ViewBag.Login = string.Empty;
            }

            ViewBag.Stuff = DBStuff.stuff;

            return View();
        }
        [HttpPost]
        public IActionResult Cart(int stuff_id)
        {

            if (HttpContext.Session.GetString("UserID") == null)
            {
                return Redirect("../Profile/LogIn");
            }
            if (HttpContext.Session.GetString("UserID") == string.Empty)
            {
                return Redirect("../Profile/LogIn");
            }

            if (HttpContext.Session.GetString("UserID") != string.Empty)
            {
                ViewBag.Login = UserOptions.GetUser(int.Parse(HttpContext.Session.GetString("UserID"))).Login;

                UserOptions.RemoveFromCart(stuff_id, int.Parse(HttpContext.Session.GetString("UserID")));
                ViewBag.cartlist = DBStuff.stuff.Where((b) => UserOptions.GetCart(int.Parse(HttpContext.Session.GetString("UserID"))).Any((i) => i == b.ID)).ToList();
            }
            else
            {
                ViewBag.Login = string.Empty;
            }
            ViewBag.Stuff = DBStuff.stuff;

            return View();
        }
    }
}
