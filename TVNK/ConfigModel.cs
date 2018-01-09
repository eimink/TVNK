using System;
namespace TVNK
{
    [Serializable]
    public class ConfigModel
    {
        public CasparConfigModel caspar;
        public TwitterConfigModel twitter;

        public ConfigModel()
        {
            caspar = new CasparConfigModel();
            twitter = new TwitterConfigModel();
        }
    }

    [Serializable]
    public class CasparConfigModel
    {
        public string host;
        public int port;
    }

    [Serializable]
    public class TwitterConfigModel
    {
        public string consumerKey;
        public string consumerSecret;
        public string userAccessToken;
        public string userAccessSecret;
        public string searchString;
    }
}
