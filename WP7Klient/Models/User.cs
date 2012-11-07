namespace WP7Klient.Models
{
    public class User
    {
        public int ID { get; set; }
        public int FollowersCount { get; set; }
        public int StatusesCount { get; set; }
        public string Name { get; set; }
        public int FavoritesCount { get; set; }
        public string ProfileImageUrlHTTPS { get; set; }
        public bool Following { get; set; }
        public string ScreenName { get; set; }
    }
}
