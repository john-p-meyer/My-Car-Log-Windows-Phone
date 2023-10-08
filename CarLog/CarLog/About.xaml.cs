using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using CarLog.Core.Helpers;
using CarLog.Core.Resources;

namespace CarLog
{
    public partial class About : PhoneApplicationPage
    {
        public Uri AboutImage
        {
            get
            {
                return ThemeImage.Uri("/Assets/AboutPageImages/[THEME]/ApplicationIcon.png", UriKind.Relative);                
            }
        }
        public About()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.txtVersion.Text = System.Reflection.Assembly.GetExecutingAssembly().FullName.Split('=')[1].Split(',')[0];
            this.DataContext = this;

        }

        private void Email_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EmailComposeTask emailComposeTask = new EmailComposeTask();

                emailComposeTask.Subject = AppResources.ApplicationTitle;
                emailComposeTask.Body = "";
                emailComposeTask.To = AppResources.ContactEmailText;
                emailComposeTask.Cc = "";
                emailComposeTask.Bcc = "";

                emailComposeTask.Show();
            }
            catch
            {
            }
        }
    }
}