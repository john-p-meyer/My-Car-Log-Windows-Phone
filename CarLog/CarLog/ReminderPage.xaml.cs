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
    public partial class ReminderPage : PhoneApplicationPage
    {
        private Reminder _reminder = new Reminder();

        public ReminderPage()
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

            ApplicationBarIconButton AddServiceEventMenuButton = new ApplicationBarIconButton();
            AddServiceEventMenuButton.IsEnabled = true;
            AddServiceEventMenuButton.Text = AppResources.AddService;
            AddServiceEventMenuButton.IconUri = new Uri("/Assets/AppBar/AddServiceEvent.png", UriKind.Relative);
            AddServiceEventMenuButton.Click += new EventHandler(AddServiceEventMenuButton_Click);

            ApplicationBar.Buttons.Add(EditMenuButton);
            ApplicationBar.Buttons.Add(DeleteMenuButton);
            ApplicationBar.Buttons.Add(AddServiceEventMenuButton);
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

            if (PhoneApplicationService.Current.State.Keys.Contains(App.appStateCurrentReminder))
            {
                Guid UniqueId;

                if (Guid.TryParse(PhoneApplicationService.Current.State[App.appStateCurrentReminder].ToString(), out UniqueId))
                {
                    _reminder = App.ViewModel.FindReminder(UniqueId);

                    this.txtCarTitle.Text = App.ViewModel.FindCar(_reminder).Title;

                    DataContext = _reminder;
                }
                else
                {
                    NavigationService.GoBack();
                }
            }
            else
            {
                NavigationService.GoBack();
            }           

            base.OnNavigatedTo(e);
        }

        private void EditMenuButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/ReminderPageEdit.xaml", UriKind.Relative));
        }

        private async void DeleteMenuButton_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(AppResources.DeleteReminderQuestion, AppResources.Warning, MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.Cancel)
                return;

            await App.ViewModel.DeleteReminder(_reminder);

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

        private void AddServiceEventMenuButton_Click(object sender, EventArgs e)
        {
            ServiceEvent _serviceEvent = new ServiceEvent();

            _serviceEvent.CarID = _reminder.CarID;
            _serviceEvent.DateTime = DateTime.Now;
            _serviceEvent.Odometer = App.ViewModel.FindCar(_serviceEvent.CarID).CurrentOdometer;
            _serviceEvent.ServiceEventType = _reminder.ServiceEventType;

            App.ViewModel.FindCar(_serviceEvent.CarID).ServiceEvents.Add(_serviceEvent);

            PhoneApplicationService.Current.State[App.appStateCurrentServiceEvent] = _serviceEvent.ID.ToString();

            NavigationService.Navigate(new Uri("/ServiceEventPageEdit.xaml", UriKind.Relative));
        }
    }
}