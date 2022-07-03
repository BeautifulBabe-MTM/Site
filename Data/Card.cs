namespace Site
{
    public class Card
    {
        public int id { get; set; }
        public string image { get; set; }
        public string name { get; set; }
        public string price { get; set; }
        public Card(string image, string name, string price)
        {
            this.image = image;
            this.name = name;
            this.price = price;
        }
    }
}
