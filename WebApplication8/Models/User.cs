namespace WebApplication8.Models
{
    public class User
    {
        public int ID { get; set; }
        public string Login{ get; set; }
        public string Pass { get; set; }
        public User(int id, string login, string pass)
        {
            this.ID = id;
            this.Login = login;
            this.Pass = pass;
        }
    }
}
