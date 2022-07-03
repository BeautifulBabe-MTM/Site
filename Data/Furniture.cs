namespace Site.Data
{
    public class Furniture
    {
        public int Id { get; set; }
        public string Image { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int Price { get; set; }
        public Category Category { get; set; } = null!;
    }
}
