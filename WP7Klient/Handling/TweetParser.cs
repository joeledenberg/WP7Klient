using System;
using WP7Klient.Models;
using System.Collections.ObjectModel;
using System.Json;
using System.Diagnostics;

namespace WP7Klient.Handling
{
    public static class TweetParser
    {
        public static ObservableCollection<Tweet> Parse(string raw)
        {
            ObservableCollection<Tweet> returnable = new ObservableCollection<Tweet>();
            try
            {
                JsonArray array = JsonObject.Parse(raw) as JsonArray;
                //Debug.WriteLine(">> array | " + array.ToString());
                foreach (JsonObject tweet in array)
                {
                    //Debug.WriteLine(">> tweet | " + tweet.ToString() + "\n");
                    Tweet timelineItem = new Tweet()
                    {
                        InReplyToUserIdStr = (tweet["in_reply_to_user_id_str"] ?? string.Empty),
                        CreatedAt = Utility.StringManipulation.TimeFormatting(tweet["created_at"]),
                        RetweetCount = tweet["retweet_count"],
                        Favorited = tweet["favorited"],
                        Source = (tweet["source"] ?? string.Empty),
                        Text = tweet["text"],
                        Author = GetUser(tweet["user"])
                    };

                    returnable.Add(timelineItem);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return returnable;
        }

        public static ObservableCollection<Tweet> ParseSearch(string raw)
        {

            ObservableCollection<Tweet> returnable = new ObservableCollection<Tweet>();
            try
            {
                JsonObject array = JsonObject.Parse(raw) as JsonObject;
                JsonArray temp = array["statuses"] as JsonArray;
                foreach (JsonObject tweet in temp)
                {
                    Tweet timelineItem = new Tweet()
                    {
                        CreatedAt = Utility.StringManipulation.TimeFormatting(tweet["created_at"]),
                        Text = tweet["text"],
                        Author = new User() { Name = tweet["user"]["name"] }
                    };
                    returnable.Add(timelineItem);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return returnable;
        }


        private static User GetUser(JsonValue rawUser)
        {
            User returnable = new User()
            {
                ID = rawUser["id"],
                FollowersCount = rawUser["followers_count"],
                StatusesCount = rawUser["statuses_count"],
                Name = rawUser["name"],
                FavoritesCount = rawUser["favourites_count"],
                ProfileImageUrlHTTPS = rawUser["profile_image_url_https"],
                Following = rawUser["following"],
                ScreenName = rawUser["screen_name"]
            };
            return returnable;
        }
    }
}
