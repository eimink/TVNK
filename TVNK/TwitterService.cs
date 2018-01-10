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

        private int status = 2;

        public int StatusCode { get { return status; } }

        public TwitterService(bool autoStart = false)
        {
            Auth.SetUserCredentials(consumerKey, consumerSecret, userAccessToken, userAccessSecret);
            status = 1;
            if (autoStart)
                StartMonitoring(searchString,newTweet);
        }

        public TwitterService(TwitterConfigModel config, bool autoStart = false)
        {
            if (config != null)
            {
                consumerKey = config.consumerKey;
                consumerSecret = config.consumerSecret;
                userAccessToken = config.userAccessToken;
                userAccessSecret = config.userAccessSecret;
            }
            Auth.SetUserCredentials(consumerKey, consumerSecret, userAccessToken, userAccessSecret);
            status = 1;
            if (autoStart)
                StartMonitoring(searchString, newTweet);
        }

        public void StartMonitoring(string query, EventHandler<Tweetinvi.Events.MatchedTweetReceivedEventArgs> callback)
        {
            try {
                tweetStream = Stream.CreateFilteredStream();
                tweetStream.AddTrack(query);
                tweetStream.MatchingTweetReceived += callback;
                tweetStream.StartStreamMatchingAllConditions();
                status = 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error starting Twitter monitoring: " + ex.Message);
                status = -1;
            }

        }

        public void StopMonitoring(string query, EventHandler<Tweetinvi.Events.MatchedTweetReceivedEventArgs> callback)
        {
            tweetStream.MatchingTweetReceived -= callback;
            tweetStream.StopStream();
            tweetStream.RemoveTrack(query);
            status = 2;
        }

        public void Close()
        {
            StopMonitoring(searchString,newTweet);
        }

        private void newTweet(Object sender, Tweetinvi.Events.MatchedTweetReceivedEventArgs args)
        {
            Console.WriteLine("New tweet found: " + args.Tweet);
        }
    }
}
