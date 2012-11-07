using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using TweetSharp;
using System.Runtime.Serialization;
using System.IO;
using WP7Klient.Handling;
using WP7Klient.Utility;
using System.Collections.ObjectModel;
//eemalda
using System.Diagnostics;


namespace WP7Klient
{
    public partial class Login : PhoneApplicationPage
    {

        private OAuthRequestToken _requestToken;
        private TwitterService service;

        private Stack<Uri> NavigationStack = new Stack<Uri>();

        // Constructor
        public Login()
        {
            InitializeComponent();

            VisualStateManager.GoToState(this, "Loading", true);

            BrowserControl.LoadCompleted += BrowserNavigationCompleted;
            BrowserControl.Navigating += BrowserNavigating;
            BrowserControl.NavigationFailed += BrowserNavigationFailed;
        }


        private void BrowserNavigating(object sender, NavigatingEventArgs e)
        {

            VisualStateManager.GoToState(this, "Loading", true);

        }


        private void BrowserNavigationFailed(object sender,
          NavigationFailedEventArgs e)
        {
            VisualStateManager.GoToState(this, "LoadingDone", true);
            OAuthWebBrowser_LoadCompleted(sender, null);
        }


        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (NavigationStack.Count > 1)
            {
                // get rid of the topmost item...
                NavigationStack.Pop();
                // now navigate to the next topmost item
                // note that this is another Pop - as when the navigate occurs a Push() will happen
                BrowserControl.Navigate(NavigationStack.Pop());
                e.Cancel = true;
                return;
            }

            base.OnBackKeyPress(e);
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!Microsoft.Phone.Net.NetworkInformation.DeviceNetworkInformation.IsNetworkAvailable)
            {
                MessageBox.Show("Puudub internetiühendus!");
                return;
            }
            else
            {
                GetTwitterToken();
            }
        }

        private void GetTwitterToken()
        {
            service = new TwitterService(TwitterSettings.ConsumerKey, TwitterSettings.ConsumerKeySecret);
            var cb = new Action<OAuthRequestToken, TwitterResponse>(CallBackToken);
            service.GetRequestToken("oob", CallBackToken);
        }

        void CallBackToken(OAuthRequestToken rt, TwitterResponse response)
        {
            try
            {
                Uri uri = service.GetAuthorizationUri(rt);
                _requestToken = rt;
                BrowserControl.Dispatcher.BeginInvoke(() => BrowserControl.Navigate(uri));
            }
            catch(Exception e)
            {
                Debug.WriteLine(e);
                MessageBox.Show("Viga ühendumisel!");
                return;
            }
        }


        void BrowserNavigationCompleted(Object sender, NavigationEventArgs e)
        {
            NavigationStack.Push(e.Uri);
            VisualStateManager.GoToState(this, "LoadingDone", true);

            if (e.Uri.AbsoluteUri == "https://api.twitter.com/oauth/authorize")
            {
                OAuthWebBrowser_LoadCompleted(sender, e);
            }

        }
        private void OAuthWebBrowser_LoadCompleted(object sender, NavigationEventArgs e)
        {

            string html = BrowserControl.SaveToString();
            if (html.Contains("<code>"))
            {
                // Hide the browser while proccessing the PIN
                VisualStateManager.GoToState(this, "Loading", true);

                int i = html.IndexOf("<code>") + 6;
                string code = html.Substring(i, 7);

                try
                {
                    //var cb = new Action<OAuthAccessToken, TwitterResponse>(CallBackVerifiedResponse);
                    service.GetAccessToken(_requestToken, code, (access, Response) =>
                    {
                        if (Response.StatusCode == HttpStatusCode.OK)
                        {

                            SerializeHelper.SaveSetting<TwitterAccess>("TwitterAccess", new TwitterAccess
                            {
                                AccessToken = access.Token,
                                AccessTokenSecret = access.TokenSecret,
                                ScreenName = access.ScreenName,
                                UserId = access.UserId.ToString()

                            });
                            Debug.WriteLine("Acess token: " + access.Token);
                            Debug.WriteLine("Acess token secret: " + access.TokenSecret);
                            Debug.WriteLine("Screen name: " + access.ScreenName);
                            Debug.WriteLine("User ID: " + access.UserId.ToString());

                            System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                            {
                                Debug.WriteLine("Navigating away ...");
                                NavigationService.Navigate(new Uri("/Main.xaml", UriKind.Relative));
                            });

                        }

                    });

                    Debug.WriteLine("PIN Code: "+code);
                    Debug.WriteLine("Request token: "+ _requestToken.Token);
                    Debug.WriteLine("Request secret: " + _requestToken.TokenSecret);
                }
                catch
                {
                    MessageBox.Show("Midagi läks valesti");
                }

            }
        }


    }
}