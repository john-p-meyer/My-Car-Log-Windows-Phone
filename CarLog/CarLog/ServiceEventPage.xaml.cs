using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Advertising.Mobile.UI;
using CarLog.Core.DataModel;
using CarLog.Core.Resources;
using CarLog.Core.Helpers;

namespace CarLog
{
    public partial class ServiceEventPage : PhoneApplicationPage
    {
        private ServiceEvent _serviceEvent = new ServiceEvent();

        public ServiceEventPage()
        {
            InitializeComponent();

            AdControl adControl = AdControlGenerator.GenerateAdControl(2);
            adControl.AdRefreshed += adControl_AdRefreshed;
            adControl.ErrorOccurred += adControl_ErrorOccurred;

            this.LayoutRoot.Children.Add(adControl);

            this.BuildLocalizedApplicationBar();
        }

        private void BuildLocalizedApplicationBar()
        {
            ApplicationBar.Buttons.Clear();

            ApplicationBarIconButton EditMenuButton = new ApplicationBarIconButton();
            EditMenuButton.IsEnabled = true;
            EditMenuButton.Text = AppResources.Edit;
            EditMenuButton.IconUri = new Uri("/Assets/AppBar/edit.png", UriKind.Relative);
            EditMenuButton.Click += new EventHandler(EditMenuButton_Click);

            ApplicationBarIconButton DeleteMenuButton = new ApplicationBarIconButton();
            DeleteMenuButton.IsEnabled = true;
            DeleteMenuButton.Text = AppResources.Delete;
            DeleteMenuButton.IconUri = new Uri("/Assets/AppBar/delete.png", UriKind.Relative);
            DeleteMenuButton.Click += new EventHandler(DeleteMenuButton_Click);

            ApplicationBar.Buttons.Add(EditMenuButton);
            ApplicationBar.Buttons.Add(DeleteMenuButton);
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            // Load the data model if it is not already loaded
            if (!App.ViewModel.IsDataLoaded)
            {
                await App.ViewModel.LoadData();
            }
            else if (App.ViewModel.Cars.Count == 0)
            {
                await App.ViewModel.LoadData();
            }

            if (PhoneApplicationService.Current.State.Keys.Contains(App.appStateCurrentServiceEvent))
            {
                Guid UniqueId;

                if (Guid.TryParse(PhoneApplicationService.Current.State[App.appStateCurrentServiceEvent].ToString(), out UniqueId))
                {
                    _serviceEvent = App.ViewModel.FindServiceEvent(UniqueId);
                }
                else
                {
                    NavigationService.GoBack();
                }
            }

            //this.txtCarTitle.Text = App.ViewModel.FindCar(_serviceEvent).Title;
            this.txtCarTitle.Text = _serviceEvent.CarTitle;

            DataContext = _serviceEvent;

            base.OnNavigatedTo(e);
        }

        private void EditMenuButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/ServiceEventPageEdit.xaml", UriKind.Relative));
        }

        private async void DeleteMenuButton_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(AppResources.DeleteServiceEventQuestion, AppResources.Warning, MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.Cancel)
                return;

            await App.ViewModel.DeleteServiceEvent(_serviceEvent);

            NavigationService.GoBack();
        }

        public void adControl_AdRefreshed(object sender, EventArgs e)
        {
            AdControl adControl = sender as AdControl;

            Dispatcher.BeginInvoke(() =>
            {
                try
                {
                    adControl.Visibility = System.Windows.Visibility.Visible;
                }
                catch
                {
                }
            });
        }

        public void adControl_ErrorOccurred(object sender, Microsoft.Advertising.AdErrorEventArgs e)
        {
            AdControl adControl = sender as AdControl;

            Dispatcher.BeginInvoke(() =>
            {
                try
                {
                    adControl.Visibility = System.Windows.Visibility.Collapsed;
                }
                catch
                {
                }
            });
        }
    }
}