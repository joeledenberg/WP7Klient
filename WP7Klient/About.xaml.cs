using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace WP7Klient
{
    public partial class About : PhoneApplicationPage
    {
        public About()
        {
            InitializeComponent();
            this.aboutText.Text = "Versioon 1.0\r\n\r\nTwitteri baasfunktsionaalsust realiseeriv rakendus. Kirjutatud Joel Edenbergi poolt eesmärgiga õppida kasutama Windows Phone 7.5 SDK'd.";
        }

        private void Suggestion_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Phone.Tasks.EmailComposeTask emailTask = new Microsoft.Phone.Tasks.EmailComposeTask();
            emailTask.Subject = "WP7Kliendi kohta ettepanek";
            emailTask.To = "zugzog@gmail.com";
            emailTask.Show();
        }

        private void Author_Click(object sender, RoutedEventArgs e)
        {
            WebBrowserTask task = new WebBrowserTask();
            task.Uri = new Uri("http://www.joel.ee", UriKind.Absolute);
            task.Show();
        }

    }
}