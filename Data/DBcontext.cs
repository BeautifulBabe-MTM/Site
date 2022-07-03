using Site.Data;
using Microsoft.EntityFrameworkCore;

namespace Site.Net
{
    public class DBContext : DbContext
    {
        public DbSet<Furniture> Furniture { get; set; } = null!;
        public DbSet<Category> Category { get; set; } = null!;
        public DbSet<Client> Client { get; set; } = null!;
        public DbSet<Basket> Basket { get; set; } = null!;
        public DBContext()
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=aspdatabase.cxp0pm4y0ro5.us-east-1.rds.amazonaws.com;Initial Catalog=aspdatabase;User ID=admin;Password=qwerty123");
        }
    }
}