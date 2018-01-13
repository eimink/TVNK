using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace TVNK
{
    public partial class Form1 : Form
    {
        private TwitterService twitterService;
        private CasparService casparService;
        private ConfigService configService;
        private Dictionary<int, Tweetinvi.Models.ITweet> tweetDict;
        private Thread tweetThread;

        public Form1()
        {
            InitializeComponent();
            configService = new ConfigService();
            twitterService = new TwitterService(configService.settings.twitter,false);
            casparService = new CasparService(configService.settings.caspar);
            tweetDict = new Dictionary<int, Tweetinvi.Models.ITweet>();

            tweetThread = new Thread(() => twitterService.StartMonitoring(configService.settings.twitter.searchString, HandleTweet));
            tweetThread.Start();
        }


        private void HandleTweet(object sender, Tweetinvi.Events.MatchedTweetReceivedEventArgs e)
        {
            if (this.IsHandleCreated)
            {
                Invoke(new Action(() =>
                {
                    int index = tweetList.Items.Add(e.Tweet.CreatedBy.Name +": "+e.Tweet.Text);
                    tweetDict.Add(index, e.Tweet);
                }));
                tweetList.Invalidate();
            }
        }

        private void tweetList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (tweetList.SelectedIndex >= 0)
            {
                var t = tweetDict[tweetList.SelectedIndex];
                Dictionary<string, string> payload = new Dictionary<string, string>();
                payload.Add("user", t.CreatedBy.Name);
                payload.Add("tweet", t.FullText);
                payload.Add("profileImage", t.CreatedBy.ProfileImageUrl400x400);
                casparService.SendCommand(CasparCommand.Invoke(1, 10, "SetTweet('" + Newtonsoft.Json.JsonConvert.SerializeObject(payload)+"')"));
            }
            
        }
    }
}
