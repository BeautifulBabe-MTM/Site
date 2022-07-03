using Site.Net;
using Microsoft.EntityFrameworkCore;

namespace Site.Data;

public static class DBservice
{
    public static void Init_db()
    {
        using (DBContext db = new DBContext())
        {
            db.SaveChanges();
            GetDataFromDB();
        }
    }

    public static void AddFurniture(string image, string name, int price, int category)
    {
        using (DBContext db = new DBContext())
        {
            db.Furniture.Add(new Furniture { Image = image, Name = name, Price = price, Category = db.Category.FirstOrDefault(x => x.Id == category) });
            db.SaveChanges();
            GetDataFromDB();
        }
    }

    public static void RemoveFurniture(Furniture furniture)
    {
        using (DBContext db = new DBContext())
        {
            db.Furniture.Remove(furniture);
            db.SaveChanges();
            GetDataFromDB();
        }
    }

    public static void EditFurniture(int cardID, string image, string name, int price, int category)
    {
        using (DBContext db = new DBContext())
        {
            var d = db.Furniture.Where(x => x.Id == cardID).First();
            d.Image = image;
            d.Name = name;
            d.Price = price;
            d.Category = db.Category.FirstOrDefault(x => x.Id == category)!;

            db.SaveChanges();
            GetDataFromDB();
        }
    }
    public static void EditCategory(string name, int category)
    {
        using (DBContext db = new DBContext())
        {
            var d = db.Category.Where(x => x.Id == category).First();
            d.Id = db.Category.FirstOrDefault(x => x.Id == category)!.Id;
            d.Name = name;

            db.SaveChanges();
            GetDataFromDB();
        }
    }
    public static void AddCategory(string name)
    {
        using (DBContext db = new DBContext())
        {
            db.Category.Add(new Category { Name = name });
            db.SaveChanges();
            GetDataFromDB();
        }
    }
    public static void RemoveCategory(Category category)
    {
        using (DBContext db = new DBContext())
        {
            db.Category.Remove(category);
            db.SaveChanges();
            GetDataFromDB();
        }
    }
    public static void AddUser(string login, string password)
    {
        using (DBContext db = new DBContext())
        {
            db.Client.Add(new Client { Login = login, Pass = password, Root = false });
            db.SaveChanges();
            GetDataFromDB();
        }
    }
    public static void AddBasket(Basket basket)
    {
        using (DBContext db = new DBContext())
        {
            db.Basket.Add(basket);
            db.SaveChanges();
            GetDataFromDB();
        }
    }
    public static void DeleteBasket(Basket id)
    {
        using (DBContext db = new DBContext())
        {
            db.Basket.Remove(id);
            db.SaveChanges();
            GetDataFromDB();
        }
    }
    //public static void DeleteofBasket(Basket basket)
    //{
    //    using (DBContext db = new DBContext())
    //    {
    //        db.Basket.Remove(basket);
    //        db.SaveChanges();
    //        GetDataFromDB();
    //    }
    //}
    //public static void ClearBasket()
    //{
    //    using (DBContext db = new DBContext())
    //    {
    //        db.Database.ExecuteSqlInterpolated($"TRUNCATE TABLE [Basket]");
    //    }
    //}
    public static List<Furniture> furniture { get; set; } = new List<Furniture>();
    public static List<Category> category { get; set; } = new List<Category>();
    public static List<Basket> basket { get; set; } = new List<Basket>();
    public static List<Client> client { get; set; } = new List<Client>();
    public static void GetDataFromDB()
    {
        using (DBContext db = new DBContext())
        {
            furniture = db.Furniture.ToList();
            category = db.Category.ToList();
            //basket = db.Basket.ToList();
            client = db.Client.ToList();
        }
    }

    public static Tuple<bool, string> authadmin(string login, string pass)
    {
        using (DBContext db = new DBContext())
        {
            if (db.Client.ToList().Any(x => x.Login == login && x.Pass == pass))
            {
                return Tuple.Create(db.Client.ToList().Find(x => x.Login.Equals(login) && x.Pass.Equals(pass)).Root, login);
            }
            return Tuple.Create(false, "Вход");
        }
    }
}