using System;

namespace TVNK
{
    class MainClass
    {

        public static void Main(string[] args)
        {
            ConfigService configService = new ConfigService();
            CasparService casparService = new CasparService(configService.settings.caspar);
            TwitterService twitterService = new TwitterService(configService.settings.twitter);
            Console.WriteLine("TVNK! Press ENTER to quit...");

            while (!Console.KeyAvailable);
            twitterService.Close();
            casparService.Disconnect();
        }
    }
}
