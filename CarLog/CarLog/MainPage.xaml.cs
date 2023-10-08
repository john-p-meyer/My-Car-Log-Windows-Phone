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
using CarLog.Core.Helpers;
using CarLog.Core.Resources;
using CarLog.Core.DataModel;

namespace CarLog
{
    public partial class MainPage : PhoneApplicationPage
    {
        /// <summary>
        /// Main page constructor.
        /// Creates the ad control, sets the data context, and builds the localized app bar
        /// </summary>
        public MainPage()
        {
            SystemTray.SetProgressIndicator(this, App.ProgressIndicator);

            try
            {
                InitializeComponent();

                AdControl adControl = AdControlGenerator.GenerateAdControl(1);
                adControl.AdRefreshed += adControl_AdRefreshed;
                adControl.ErrorOccurred += adControl_ErrorOccurred;

                this.LayoutRoot.Children.Add(adControl);
            }
            catch
            {
            }

            //this.BuildLocalizedApplicationBar();            
        }        

        /// <summary>
        /// When the page is navigated to.
        /// Loads the data, Resets all application app states, sets the available application bar buttons
        /// </summary>
        /// <param name="e">The navigation event arguments</param>
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

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;

            App.ViewModel.RefreshAllEvents();

            this.BuildLocalizedApplicationBar();            

            // Reset all the application current keys
            if (PhoneApplicationService.Current.State.Keys.Contains(App.appStateCurrentCar))
            {
                PhoneApplicationService.Current.State.Remove(App.appStateCurrentCar);
            }

            if (PhoneApplicationService.Current.State.Keys.Contains(App.appStateCurrentFuelEvent))
            {
                PhoneApplicationService.Current.State.Remove(App.appStateCurrentFuelEvent);
            }

            if (PhoneApplicationService.Current.State.Keys.Contains(App.appStateCurrentServiceEvent))
            {
                PhoneApplicationService.Current.State.Remove(App.appStateCurrentServiceEvent);
            }

            if (PhoneApplicationService.Current.State.Keys.Contains(App.appStateCurrentReminder))
            {
                PhoneApplicationService.Current.State.Remove(App.appStateCurrentReminder);
            }

            // Reset the selected items on the long list selectors so they can be selected again
            LongListSelector_Cars.SelectedItem = null;
            FuelEventLogSelector.SelectedItem = null;
            ServiceEventLogSelector.SelectedItem = null;
            ReminderLogSelector.SelectedItem = null;

            // Set the correct application bar button states
            try
            {
                if (App.ViewModel.Cars.Count == 0)
                {
                    ((ApplicationBarIconButton)(this.ApplicationBar.Buttons[1])).IsEnabled = false;
                    ((ApplicationBarIconButton)(this.ApplicationBar.Buttons[2])).IsEnabled = false;
                    ((ApplicationBarIconButton)(this.ApplicationBar.Buttons[3])).IsEnabled = false;
                }
                else
                {
                    ((ApplicationBarIconButton)(this.ApplicationBar.Buttons[1])).IsEnabled = true;
                    ((ApplicationBarIconButton)(this.ApplicationBar.Buttons[2])).IsEnabled = true;
                    ((ApplicationBarIconButton)(this.ApplicationBar.Buttons[3])).IsEnabled = true;
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// When a car is selected on the car long list selector. Loads the car to car page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LongListSelector_Car_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LongListSelector_Cars.SelectedItem != null)
            {
                PhoneApplicationService.Current.State[App.appStateCurrentCar] = (LongListSelector_Cars.SelectedItem as Car).ID.ToString();
                NavigationService.Navigate(new Uri("/CarPage.xaml", UriKind.Relative));
            }            
        }

        /// <summary>
        /// When a fuel event is selected on the fuel event long list selector. Loads the fuel event to the fuel event page 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LongListSelector_FuelEvent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FuelEventLogSelector.SelectedItem != null)
            {
                PhoneApplicationService.Current.State[App.appStateCurrentFuelEvent] = (FuelEventLogSelector.SelectedItem as FuelEvent).ID.ToString();
                NavigationService.Navigate(new Uri("/FuelEventPage.xaml", UriKind.Relative));
            }
        }

        /// <summary>
        /// When a service event is selected on the service event long list selector. Loads the service event to the service event page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LongListSelector_ServiceEvent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ServiceEventLogSelector.SelectedItem != null)
            {
                PhoneApplicationService.Current.State[App.appStateCurrentServiceEvent] = (ServiceEventLogSelector.SelectedItem as ServiceEvent).ID.ToString();
                NavigationService.Navigate(new Uri("/ServiceEventPage.xaml", UriKind.Relative));
            }
        }

        /// <summary>
        /// When a reminder is selected on the reminder long list selector. Loads the reminder to the reminder page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LongListSelector_Reminder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ReminderLogSelector.SelectedItem != null)
            {
                PhoneApplicationService.Current.State[App.appStateCurrentReminder] = (ReminderLogSelector.SelectedItem as Reminder).ID.ToString();
                NavigationService.Navigate(new Uri("/ReminderPage.xaml", UriKind.Relative));
            }
        }

        /// <summary>
        /// When the Add Car menu button is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddCarMenuButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/CarPageEdit.xaml", UriKind.Relative));
        }

        /// <summary>
        /// When the Add Fuel Event menu button is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddFuelEventMenuButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/FuelEventPageEdit.xaml", UriKind.Relative));
        }

        /// <summary>
        /// When the Add Service Event menu button is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddServiceEventMenuButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/ServiceEventPageEdit.xaml", UriKind.Relative));
        }

        /// <summary>
        /// When the Add Reminder menu button is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddReminderMenuButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/ReminderPageEdit.xaml", UriKind.Relative));
        }

        /// <summary>
        /// When the Settings menu item is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Settings_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
        }

        /// <summary>
        /// When the About menu item is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void About_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/About.xaml", UriKind.Relative));
        }

        /// <summary>
        /// When the Delete item is pressed on the Car popup menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Delete_Car_Click(object sender, RoutedEventArgs e)
        {
            Car car = ((sender as MenuItem).DataContext as Car);

            var result = MessageBox.Show(AppResources.DeleteCarQuestion, AppResources.Warning, MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.Cancel)
                return;
            
            await App.ViewModel.DeleteCar(car);
        }

        /// <summary>
        /// When the Delete item is pressed on the Fuel Event popup menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Delete_FuelEvent_Click(object sender, RoutedEventArgs e)
        {
            FuelEvent fuelEvent = ((sender as MenuItem).DataContext as FuelEvent);

            var result = MessageBox.Show(AppResources.DeleteFuelEventQuestion, AppResources.Warning, MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.Cancel)
                return;
                        
            await App.ViewModel.DeleteFuelEvent(fuelEvent);
        }

        /// <summary>
        /// When the Delete item is pressed on the Service Event popup menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Delete_ServiceEvent_Click(object sender, RoutedEventArgs e)
        {
            ServiceEvent serviceEvent = ((sender as MenuItem).DataContext as ServiceEvent);

            var result = MessageBox.Show(AppResources.DeleteServiceEventQuestion, AppResources.Warning, MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.Cancel)
                return;

            await App.ViewModel.DeleteServiceEvent(serviceEvent);
        }

        /// <summary>
        /// When the Delete item is pressed on the Reminder popup menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Delete_Reminder_Click(object sender, RoutedEventArgs e)
        {
            Reminder reminder = ((sender as MenuItem).DataContext as Reminder);

            var result = MessageBox.Show(AppResources.DeleteReminderQuestion, AppResources.Warning, MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.Cancel)
                return;

            await App.ViewModel.DeleteReminder(reminder);
        }

        /// <summary>
        /// When the Edit item is pressed on the Car popup menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Edit_Car_Click(object sender, RoutedEventArgs e)
        {
            Car car = ((sender as MenuItem).DataContext as Car);

            PhoneApplicationService.Current.State[App.appStateCurrentCar] = car.ID.ToString();

            NavigationService.Navigate(new Uri("/CarPageEdit.xaml", UriKind.Relative));
        }

        /// <summary>
        /// When the Edit item is pressed on the Fuel Event popup menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Edit_FuelEvent_Click(object sender, RoutedEventArgs e)
        {
            FuelEvent fuelEvent = ((sender as MenuItem).DataContext as FuelEvent);

            PhoneApplicationService.Current.State[App.appStateCurrentFuelEvent] = fuelEvent.ID.ToString();

            NavigationService.Navigate(new Uri("/FuelEventPageEdit.xaml", UriKind.Relative));

        }

        /// <summary>
        /// When the Edit item is pressed on the Service Event popup menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Edit_ServiceEvent_Click(object sender, RoutedEventArgs e)
        {
            ServiceEvent serviceEvent = ((sender as MenuItem).DataContext as ServiceEvent);

            PhoneApplicationService.Current.State[App.appStateCurrentServiceEvent] = serviceEvent.ID.ToString();

            NavigationService.Navigate(new Uri("/ServiceEventPageEdit.xaml", UriKind.Relative));
        }

        /// <summary>
        /// When the Edit item is pressed on the Reminder popup menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Edit_Reminder_Click(object sender, RoutedEventArgs e)
        {
            Reminder reminder = ((sender as MenuItem).DataContext as Reminder);

            PhoneApplicationService.Current.State[App.appStateCurrentReminder] = reminder.ID.ToString();

            NavigationService.Navigate(new Uri("/ReminderPageEdit.xaml", UriKind.Relative));
        }

        /// <summary>
        /// Builds the application bar in a localized fashion
        /// </summary>
        private void BuildLocalizedApplicationBar()
        {
            ApplicationBar.Buttons.Clear();
            ApplicationBar.MenuItems.Clear();

            ApplicationBarIconButton AddCarMenuButton = new ApplicationBarIconButton();
            AddCarMenuButton.IsEnabled = true;
            AddCarMenuButton.Text = AppResources.AddCar;
            AddCarMenuButton.IconUri = new Uri("/Assets/AppBar/AddCar.png", UriKind.Relative);
            AddCarMenuButton.Click += new EventHandler(AddCarMenuButton_Click);
                        
            ApplicationBarIconButton AddFuelEventMenuButton = new ApplicationBarIconButton();
            AddFuelEventMenuButton.IsEnabled = true;
            AddFuelEventMenuButton.Text = AppResources.AddFuel;
            AddFuelEventMenuButton.IconUri = new Uri("/Assets/AppBar/AddFuelEvent.png", UriKind.Relative);
            AddFuelEventMenuButton.Click += new EventHandler(AddFuelEventMenuButton_Click);

            ApplicationBarIconButton AddServiceEventMenuButton = new ApplicationBarIconButton();
            AddServiceEventMenuButton.IsEnabled = true;
            AddServiceEventMenuButton.Text = AppResources.AddService;
            AddServiceEventMenuButton.IconUri = new Uri("/Assets/AppBar/AddServiceEvent.png", UriKind.Relative);
            AddServiceEventMenuButton.Click += new EventHandler(AddServiceEventMenuButton_Click);

            ApplicationBarIconButton AddReminderMenuButton = new ApplicationBarIconButton();
            AddReminderMenuButton.IsEnabled = true;
            AddReminderMenuButton.Text = AppResources.AddReminder;
            AddReminderMenuButton.IconUri = new Uri("/Assets/AppBar/feature.calendar.add.png", UriKind.Relative);
            AddReminderMenuButton.Click += new EventHandler(AddReminderMenuButton_Click);

            ApplicationBarMenuItem AboutMenuItem = new ApplicationBarMenuItem();
            AboutMenuItem.IsEnabled = true;
            AboutMenuItem.Text = AppResources.About;
            AboutMenuItem.Click += new EventHandler(About_Click);

            ApplicationBarMenuItem SettingsMenuItem = new ApplicationBarMenuItem();
            SettingsMenuItem.IsEnabled = true;
            SettingsMenuItem.Text = AppResources.Settings;
            SettingsMenuItem.Click += new EventHandler(Settings_Click);

            ApplicationBar.Buttons.Add(AddCarMenuButton);
            ApplicationBar.Buttons.Add(AddFuelEventMenuButton);
            ApplicationBar.Buttons.Add(AddServiceEventMenuButton);
            ApplicationBar.Buttons.Add(AddReminderMenuButton);
            ApplicationBar.MenuItems.Add(AboutMenuItem);
            ApplicationBar.MenuItems.Add(SettingsMenuItem);
        }        

        /// <summary>
        /// When the ad control is refreshed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// When the ad control has an error occur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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