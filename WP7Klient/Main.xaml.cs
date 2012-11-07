using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml.Linq;
using Microsoft.Phone.Controls;
using Hammock;
using Hammock.Authentication.OAuth;
using TweetSharp;
using System.IO;
using System.Text;
using System.Threading;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Shell;
using System.ComponentModel;
using WP7Klient.Utility;
using WP7Klient.Handling;
using WP7Klient.Models;

//eemalda

using System.Diagnostics;


namespace WP7Klient
{
    public partial class Main : PhoneApplicationPage
    {

        public Main()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            var file = SerializeHelper.LoadSetting<TwitterAccess>("TwitterAccess");
            TwitterService service = new TwitterService(TwitterSettings.ConsumerKey, TwitterSettings.ConsumerKeySecret);
            try
            {
                TwitterCredentials.credentials = new OAuthCredentials
                {
                    Type = OAuthType.ProtectedResource,
                    SignatureMethod = OAuthSignatureMethod.HmacSha1,
                    ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
                    ConsumerKey = TwitterSettings.ConsumerKey,
                    ConsumerSecret = TwitterSettings.ConsumerKeySecret,
                    Token = file.AccessToken,
                    TokenSecret = file.AccessTokenSecret,
                    ClientUsername = file.ScreenName,
                    Version = "1.0"
                };
            }
            catch
            {
                MessageBox.Show("Sisselogimine ebaõnnestus!");
            }

            ShowTimeline();
        }

        public void ShowTimeline()
        {
            var restClient = new RestClient
            {
                Authority = "https://api.twitter.com"
            };

            var restRequest = new RestRequest
            {
                Credentials = TwitterCredentials.credentials,
                Path = "/1.1/statuses/home_timeline.json",
                Method = Hammock.Web.WebMethod.Get
            };
            var callback = new RestCallback((restRt, restResponse, userState) =>
            {
                if (restResponse.StatusCode == HttpStatusCode.Unauthorized)
                {
                    System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show("Fail!");
                    });
                }
                else
                {
                    System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        var parsedData = TweetParser.Parse(restResponse.Content);
                        tweetsList.ItemsSource = parsedData;
                    });
                }

                    
            });

            restClient.BeginRequest(restRequest, callback);
        }

 
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            while (NavigationService.CanGoBack)
                NavigationService.RemoveBackEntry();
        }

        private void tweetSubmit_Click(object sender, RoutedEventArgs e)
        {
            tweetSubmit.Background = new SolidColorBrush(Colors.Gray);

            var restClient = new RestClient
            {
                Authority = "https://api.twitter.com"
            };

            var restRequest = new RestRequest
            {
                Credentials = TwitterCredentials.credentials,
                Path = "/1.1/statuses/update.json",
                Method = Hammock.Web.WebMethod.Post
            };

            var callback = new RestCallback((restRt, restResponse, userState) =>
            {
                if (restResponse.StatusCode == HttpStatusCode.Unauthorized)
                {
                    System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show("Postitamine ebaõnnestus!");
                    });
                }
                else
                {
                    System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show("Postitus edukalt lisatud!");
                        tweetBox.Text = "";
                        tweetSubmit.Background = App.Current.Resources["PhoneAccentBrush"] as SolidColorBrush;

                        //Update list after 3 seconds
                        this.Perform(() => ShowTimeline(), 3000);
                    });
                }
            });

            restRequest.AddField("status", StringManipulation.SanitizeStatus(tweetBox.Text));
            restClient.BeginRequest(restRequest, callback);        
        }


        private void Perform(Action myMethod, int delayInMilliseconds)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (s, e) => Thread.Sleep(delayInMilliseconds);
            worker.RunWorkerCompleted += (s, e) => myMethod.Invoke();
            worker.RunWorkerAsync();
        }

        private void tweetsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If selected index is -1 (no selection) do nothing
            if (tweetsList.SelectedIndex == -1)
                return;

            Tweet selectedItem = tweetsList.SelectedItem as Tweet;

            if (selectedItem != null)
            {
                string textValue = selectedItem.Text;
                int linkLocation = textValue.ToLower().IndexOf("http");
                if (linkLocation != -1)
                {
                    StringBuilder b = new StringBuilder();
                    for (int i = linkLocation; i < textValue.Length; i++)
                    {
                        char nextChar = textValue[i];
                        if (nextChar == ' ')
                            break;
                        b.Append(nextChar);
                    }

                    string result = b.ToString();
                    if (result.Length > 0)
                    {
                        WebBrowserTask task = new WebBrowserTask();
                        task.Uri = new Uri(result);
                        task.Show();
                    }
                }
 
                    
            }
            tweetsList.SelectedIndex = -1;
        }
        private void searchList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If selected index is -1 (no selection) do nothing
            if (searchList.SelectedIndex == -1)
                return;

            Tweet selectedItem = searchList.SelectedItem as Tweet;

            if (selectedItem != null)
            {
                string textValue = selectedItem.Text;
                int linkLocation = textValue.ToLower().IndexOf("http");
                if (linkLocation != -1)
                {
                    StringBuilder b = new StringBuilder();
                    for (int i = linkLocation; i < textValue.Length; i++)
                    {
                        char nextChar = textValue[i];
                        if (nextChar == ' ')
                            break;
                        b.Append(nextChar);
                    }

                    string result = b.ToString();
                    if (result.Length > 0)
                    {
                        WebBrowserTask task = new WebBrowserTask();
                        task.Uri = new Uri(result);
                        task.Show();
                    }
                }


            }
            searchList.SelectedIndex = -1;
        }

        private void Search_Click(object sender, EventArgs e)
        {
            searchSubmit.Background = new SolidColorBrush(Colors.Gray);

            var restClient = new RestClient
            {
                Authority = "https://api.twitter.com"
            };

            var restRequest = new RestRequest
            {
                Credentials = TwitterCredentials.credentials,
                Path = "/1.1/search/tweets.json",
                Method = Hammock.Web.WebMethod.Get
            };

            var callback = new RestCallback((restRt, restResponse, userState) =>
            {

                if (restResponse.StatusCode == HttpStatusCode.Unauthorized)
                {
                    System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show("Fail!");
                        Debug.WriteLine(Search.Text);
                        Debug.WriteLine(restResponse.Content);
                    });
                }
                else
                {
                    System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        var parsedSearchData = TweetParser.ParseSearch(restResponse.Content);
                        searchList.ItemsSource = parsedSearchData;
                        searchSubmit.Background = App.Current.Resources["PhoneAccentBrush"] as SolidColorBrush;
                    });
                }

            });
            //Search.Text
            restRequest.AddParameter("q", Search.Text);
            restRequest.AddParameter("result_type", "mixed");
            restRequest.AddParameter("count", "20");
            restClient.BeginRequest(restRequest, callback);

        
        }

        private void Search_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Search_Click(sender, e);
                searchList.Focus();
            }
        }

        private void About_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/About.xaml", UriKind.Relative));

        }

    }
}