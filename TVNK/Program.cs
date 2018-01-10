using System;
using System.Text;
using System.Collections.Generic;

namespace TVNK
{
    class MainClass
    {

        public static CasparService casparService;

        public static void Main(string[] args)
        {
            ConfigService configService = new ConfigService();
            casparService = new CasparService(configService.settings.caspar);
            TwitterService twitterService = new TwitterService(configService.settings.twitter,false);
            twitterService.StartMonitoring(configService.settings.twitter.searchString, ShowTweet);
            if (twitterService.StatusCode >= 0)
            {
                Console.WriteLine("TVNK! Press ENTER to quit...");
                while (!Console.KeyAvailable);
            }
            twitterService.Close();
            casparService.Disconnect();
        }

        public static void ShowTweet(object sender, Tweetinvi.Events.MatchedTweetReceivedEventArgs args)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("tweet",args.Tweet.Text);
            parameters.Add("profileImageUrl",args.Tweet.CreatedBy.ProfileImageUrl400x400);
            StringBuilder sb = new StringBuilder();
            sb.Append("\"ShowTweet('");
            sb.Append(Newtonsoft.Json.JsonConvert.SerializeObject(parameters));
            sb.Append("')\"");
            casparService.SendCommand(CasparCommand.Invoke(1,10,sb.ToString()));
        }
    }
}
