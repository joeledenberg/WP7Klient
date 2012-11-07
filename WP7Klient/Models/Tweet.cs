namespace WP7Klient.Models
{
    public class Tweet
    {
        public string InReplyToUserIdStr { get; set; }
        public string CreatedAt { get; set; }
        public User Author { get; set; }
        public int RetweetCount { get; set; }
        public bool Favorited { get; set; }
        public string Source { get; set; }
        public string Text { get; set; }
    }
}
