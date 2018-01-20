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
using WebSocketSharp;
using WebSocketSharp.Server;

namespace TVNK
{
    public partial class Form1 : Form
    {
        private TwitterService twitterService;
        private ConfigService configService;
        private Dictionary<int, Tweetinvi.Models.ITweet> tweetDict;
        private Thread tweetThread;
        private WebSocketServer wssv;
        private Tweetinvi.Models.ITweet selectedTweet;

        public Form1()
        {
            InitializeComponent();
            configService = new ConfigService();
            twitterService = new TwitterService(configService.settings.twitter,false);
            tweetDict = new Dictionary<int, Tweetinvi.Models.ITweet>();

            tweetThread = new Thread(() => twitterService.StartMonitoring(configService.settings.twitter.searchString, HandleTweet));
            tweetThread.Start();
            wssv = new WebSocketServer(14330);
            wssv.Log.Level = LogLevel.Fatal;
            wssv.AddWebSocketService<layerino.LayerinoWebSocket>("/tvnk");
            wssv.Start();
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_Closing);
        }

        private void Form1_Closing(object sender, FormClosingEventArgs e)
        {
            Debug.WriteLine("Form closing!");
            twitterService.StopMonitoring(configService.settings.twitter.searchString, HandleTweet);
            tweetThread.Abort();
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
            selectedTweet = tweetDict[tweetList.SelectedIndex];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (tweetList.SelectedIndex >= 0)
            {
                selectedTweet = tweetDict[tweetList.SelectedIndex];
                Dictionary<string, string> payload = new Dictionary<string, string>();
                payload.Add("user", selectedTweet.CreatedBy.Name);
                payload.Add("tweet", "@" + selectedTweet.CreatedBy.Name + ": " + selectedTweet.FullText);
                payload.Add("profileImage", selectedTweet.CreatedBy.ProfileImageUrl400x400);
                wssv.WebSocketServices.Broadcast(Newtonsoft.Json.JsonConvert.SerializeObject(payload));
            }
            
        }

        public void OnClientConnected()
        {
            if (this.IsHandleCreated)
            {
                Invoke(new Action(() =>
                {
                    if (selectedTweet != null)
                    {
                        selectedTweet = tweetDict[tweetList.SelectedIndex];
                        Dictionary<string, string> payload = new Dictionary<string, string>();
                        payload.Add("user", selectedTweet.CreatedBy.Name);
                        payload.Add("tweet", "@"+ selectedTweet.CreatedBy.Name+": "+selectedTweet.FullText);
                        payload.Add("profileImage", selectedTweet.CreatedBy.ProfileImageUrl400x400);
                        wssv.WebSocketServices.Broadcast(Newtonsoft.Json.JsonConvert.SerializeObject(payload));
                    }
                }));
            }
        }
    }
}
