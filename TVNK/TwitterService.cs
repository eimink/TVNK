using System;
using Tweetinvi;

namespace TVNK
{
    public class TwitterService
    {
        private string consumerKey = "";
        private string consumerSecret = "";
        private string userAccessToken = "";
        private string userAccessSecret = "";
        private string searchString = "";

        private Tweetinvi.Streaming.IFilteredStream tweetStream;

        public TwitterService()
        {
            Auth.SetUserCredentials(consumerKey, consumerSecret, userAccessToken, userAccessSecret);
            StartMonitoring(searchString);
        }

        public TwitterService(TwitterConfigModel config)
        {
            if (config != null)
            {
                consumerKey = config.consumerKey;
                consumerSecret = config.consumerSecret;
                userAccessToken = config.userAccessToken;
                userAccessSecret = config.userAccessSecret;
            }
            Auth.SetUserCredentials(consumerKey, consumerSecret, userAccessToken, userAccessSecret);
            StartMonitoring(searchString);
        }

        public void StartMonitoring(string query)
        {
            try {
                tweetStream = Stream.CreateFilteredStream();
                tweetStream.AddTrack(query);
                tweetStream.MatchingTweetReceived += newTweet;
                tweetStream.StartStreamMatchingAllConditions();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error starting Twitter monitoring: " + ex.Message);
            }

        }

        public void StopMonitoring(string query)
        {
            tweetStream.MatchingTweetReceived -= newTweet;
            tweetStream.StopStream();
            tweetStream.RemoveTrack(query);
        }

        public void Close()
        {
            StopMonitoring(searchString);
        }

        private static void newTweet(Object sender, Tweetinvi.Events.MatchedTweetReceivedEventArgs args)
        {
            Console.WriteLine("New tweet found: " + args.Tweet);
        }
    }
}
