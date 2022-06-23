namespace WebApplication8.Models
{
    public class Stuff
    {
        public int ID { set; get; }
        public float Price { set; get; }
        public string Image { set; get; }
        public string Name { set; get; }
        public int Category_id { set; get; }
        public Stuff(int id, float price, string image, string name, int category_id)
        {
            this.ID = id;
            this.Price = price;
            this.Image= image;
            this.Name = name;
            this.Category_id = category_id;
        }
    }
}
