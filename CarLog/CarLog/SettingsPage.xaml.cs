using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using CarLog.Core.Resources;
using Microsoft.Live;
using System.IO;
using CarLog.Core.Helpers;
using System.Threading.Tasks;

namespace CarLog
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        /// <summary>
        /// Settings Page constructor
        /// </summary>
        public SettingsPage()
        {
            InitializeComponent();

            Microsoft.Live.Controls.SignInButton signInButton = new Microsoft.Live.Controls.SignInButton();
            signInButton.ClientId = OneDriveHelper.ClientId;
            signInButton.Scopes = "wl.signin wl.skydrive";
            signInButton.Content = "SignInButton";            
            signInButton.SessionChanged += SignInButton_SessionChanged;

            Grid.SetRow(signInButton, 7);

            this.ContentPanel.Children.Add(signInButton);
                        
            SystemTray.SetProgressIndicator(this, App.ProgressIndicator);
        }        
        
        /// <summary>
        /// Occurs when a list picker has its selection changed. Required because for some 
        /// reason wasn't saving the selected item and therefore wouldn't bind correctly
        /// </summary>
        /// <param name="sender">list picker who's selection changed</param>
        /// <param name="e"></param>
        private void ListPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ListPicker listPicker = sender as ListPicker;

                if (listPicker == null)
                    return;

                listPicker.SelectedItem = listPicker.Items[listPicker.SelectedIndex];
            }
            catch
            {               
            }
        }

        private async void btnSaveLocal_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(AppResources.OneDriveAreYouSureSave, AppResources.MessageTitleWarning, MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.Cancel)
            {
                return;
            }

            App.DisplayProgressIndicator(true, "");

            try
            {
                await App.ViewModel.SaveData();
                await OneDriveHelper.UploadFile(await FileHelper.SaveString(App.ViewModel));
                MessageBox.Show(AppResources.OneDriveFileSaveSuccessful);
            }
            catch (Exception ex)
            {
                MessageBox.Show(AppResources.OneDriveFileSaveUnsuccessful);
            }

            App.DisplayProgressIndicator(false, "");
        }

        private async void btnLoadLocal_Click(object sender, RoutedEventArgs e)
        {
            String downloadFile;

            var result = MessageBox.Show(AppResources.OneDriveAreYouSureLoad, AppResources.MessageTitleWarning, MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.Cancel)
            {
                return;
            }

            App.DisplayProgressIndicator(true, "");

            try
            {
                downloadFile = await OneDriveHelper.DownloadFile();

                if (downloadFile != "")
                {
                    await FileHelper.SaveData(downloadFile);
                    await App.ViewModel.LoadData();
                    MessageBox.Show(AppResources.OneDriveFileLoadSuccessful);
                }
                else
                {
                    MessageBox.Show(AppResources.OneDriveFileNotFound);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(AppResources.OneDriveFileLoadUnsuccessful);
            }

            App.DisplayProgressIndicator(false, "");

        }

        private async void SignInButton_SessionChanged(object sender, Microsoft.Live.Controls.LiveConnectSessionChangedEventArgs e)
        {
            Boolean success = await OneDriveHelper.buttonSignin_SessionChanged(sender, e);

            btnLoadLocal.IsEnabled = success;
            btnSaveLocal.IsEnabled = success;
        }        
    }
}