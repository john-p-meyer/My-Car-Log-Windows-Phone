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
    public partial class FuelEventPage : PhoneApplicationPage
    {
        private FuelEvent _fuelEvent = new FuelEvent();

        public FuelEventPage()
        {
            InitializeComponent();

            AdControl adControl = AdControlGenerator.GenerateAdControl(2);
            adControl.AdRefreshed += adControl_AdRefreshed;
            adControl.ErrorOccurred += adControl_ErrorOccurred;

            this.LayoutRoot.Children.Add(adControl);

            this.BuildLocalizedApplicationBar();
        }

        protected override async void  OnNavigatedTo(NavigationEventArgs e)
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

            if (PhoneApplicationService.Current.State.Keys.Contains(App.appStateCurrentFuelEvent))
            {
                Guid UniqueId;

                if (Guid.TryParse(PhoneApplicationService.Current.State[App.appStateCurrentFuelEvent].ToString(), out UniqueId))
                {
                    _fuelEvent = App.ViewModel.FindFuelEvent(UniqueId);
                }
                else
                {
                    NavigationService.GoBack();
                }
            }

            //this.txtCarTitle.Text = App.ViewModel.FindCar(_fuelEvent).Title;                        

            DataContext = _fuelEvent;

            base.OnNavigatedTo(e);
        }

        private void About_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/About.xaml", UriKind.Relative));
        }

        private void EditMenuButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/FuelEventPageEdit.xaml", UriKind.Relative));
        }

        private async void DeleteMenuButton_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(AppResources.DeleteFuelEventQuestion, AppResources.Warning, MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.Cancel)
                return;            

            await App.ViewModel.DeleteFuelEvent(_fuelEvent);

            NavigationService.GoBack();
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