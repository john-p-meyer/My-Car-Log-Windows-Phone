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
    public partial class CarPage : PhoneApplicationPage
    {
        private Car _car;
        private Boolean _carLoanExpanded = false;
        private Boolean _insuranceExpanded = false;
        private Boolean _totalCostExpanded = false;

        public String CarLoanExpandCollapseImage
        {
            get
            {
                if (_carLoanExpanded)
                {
                    return ThemeImage.Path("/Assets/CarPageImages/[THEME]/collapse.png");
                }
                else
                {
                    return ThemeImage.Path("/Assets/CarPageImages/[THEME]/expand.png");
                }
            }
        }

        public String InsuranceExpandCollapseImage
        {
            get
            {
                if (_insuranceExpanded)
                {
                    return ThemeImage.Path("/Assets/CarPageImages/[THEME]/collapse.png");
                }
                else
                {
                    return ThemeImage.Path("/Assets/CarPageImages/[THEME]/expand.png");
                }
            }
        }

        public String TotalCostExpandCollapseImage
        {
            get
            {
                if (_totalCostExpanded)
                {
                    return ThemeImage.Path("/Assets/CarPageImages/[THEME]/collapse.png");
                }
                else
                {
                    return ThemeImage.Path("/Assets/CarPageImages/[THEME]/expand.png");
                }
            }
        }

        public CarPage()
        {
            InitializeComponent();

            AdControl adControl = AdControlGenerator.GenerateAdControl(1);
            adControl.AdRefreshed += adControl_AdRefreshed;
            adControl.ErrorOccurred += adControl_ErrorOccurred;

            this.LayoutRoot.Children.Add(adControl);
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
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

            this.BuildLocalizedApplicationBar();

            if (PhoneApplicationService.Current.State.Keys.Contains(App.appStateCurrentCar))
            {
                Guid UniqueId;

                if (Guid.TryParse(PhoneApplicationService.Current.State[App.appStateCurrentCar].ToString(), out UniqueId))
                {
                    _car = App.ViewModel.FindCar(UniqueId);
                }
                else
                {
                    NavigationService.GoBack();
                }
            }

            DataContext = _car;

            SetCarLoanIconBoxes();
            SetInsuranceIconBoxes();
            SetTotalCostIconBoxes();

            FuelEventLogSelector.SelectedItem = null;
            ServiceEventLogSelector.SelectedItem = null;

            base.OnNavigatedTo(e);
        }

        private async void Delete_FuelEvent_Click(object sender, RoutedEventArgs e)
        {
            FuelEvent fuelEvent = ((sender as MenuItem).DataContext as FuelEvent);

            var result = MessageBox.Show(AppResources.DeleteFuelEventQuestion, AppResources.Warning, MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.Cancel)
                return;

            await App.ViewModel.DeleteFuelEvent(fuelEvent);
        }

        private void Edit_FuelEvent_Click(object sender, RoutedEventArgs e)
        {
            FuelEvent fuelEvent = ((sender as MenuItem).DataContext as FuelEvent);

            PhoneApplicationService.Current.State[App.appStateCurrentFuelEvent] = fuelEvent.ID.ToString();

            NavigationService.Navigate(new Uri("/FuelEventPageEdit.xaml", UriKind.Relative));
        }

        private async void Delete_ServiceEvent_Click(object sender, RoutedEventArgs e)
        {
            ServiceEvent serviceEvent = ((sender as MenuItem).DataContext as ServiceEvent);

            var result = MessageBox.Show(AppResources.DeleteServiceEventQuestion, AppResources.Warning, MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.Cancel)
                return;

            await App.ViewModel.DeleteServiceEvent(serviceEvent);
        }

        private void Edit_ServiceEvent_Click(object sender, RoutedEventArgs e)
        {
            ServiceEvent serviceEvent = ((sender as MenuItem).DataContext as ServiceEvent);

            PhoneApplicationService.Current.State[App.appStateCurrentServiceEvent] = serviceEvent.ID.ToString();

            NavigationService.Navigate(new Uri("/ServiceEventPageEdit.xaml", UriKind.Relative));
        }

        private async void Delete_Reminder_Click(object sender, RoutedEventArgs e)
        {
            Reminder reminder = ((sender as MenuItem).DataContext as Reminder);

            var result = MessageBox.Show(AppResources.DeleteReminderQuestion, AppResources.Warning, MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.Cancel)
                return;

            await App.ViewModel.DeleteReminder(reminder);
        }

        private void Edit_Reminder_Click(object sender, RoutedEventArgs e)
        {
            Reminder reminder = ((sender as MenuItem).DataContext as Reminder);

            PhoneApplicationService.Current.State[App.appStateCurrentReminder] = reminder.ID.ToString();

            NavigationService.Navigate(new Uri("/ReminderPageEdit.xaml", UriKind.Relative));
        }
                
        private void LongListSelector_ServiceEvent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ServiceEventLogSelector.SelectedItem != null)
            {
                PhoneApplicationService.Current.State[App.appStateCurrentServiceEvent] = (ServiceEventLogSelector.SelectedItem as ServiceEvent).ID.ToString();
                NavigationService.Navigate(new Uri("/ServiceEventPage.xaml", UriKind.Relative));
            }
        }

        private void LongListSelector_FuelEvent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FuelEventLogSelector.SelectedItem != null)
            {
                PhoneApplicationService.Current.State[App.appStateCurrentFuelEvent] = (FuelEventLogSelector.SelectedItem as FuelEvent).ID.ToString();
                NavigationService.Navigate(new Uri("/FuelEventPage.xaml", UriKind.Relative));
            }
        }

        private void LongListSelector_Reminder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ReminderLogSelector.SelectedItem != null)
            {
                PhoneApplicationService.Current.State[App.appStateCurrentReminder] = (ReminderLogSelector.SelectedItem as Reminder).ID.ToString();
                NavigationService.Navigate(new Uri("/ReminderPage.xaml", UriKind.Relative));
            }
        }

        private void BuildLocalizedApplicationBar()
        {
            ApplicationBar.Buttons.Clear();

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

            ApplicationBarIconButton EditMenuButton = new ApplicationBarIconButton();
            EditMenuButton.IsEnabled = true;
            EditMenuButton.Text = AppResources.Edit;
            EditMenuButton.IconUri = new Uri("/Assets/AppBar/edit.png", UriKind.Relative);
            EditMenuButton.Click += new EventHandler(EditMenuButton_Click);

            ApplicationBarIconButton AddReminderMenuButton = new ApplicationBarIconButton();
            AddReminderMenuButton.IsEnabled = true;
            AddReminderMenuButton.Text = AppResources.AddReminder;
            AddReminderMenuButton.IconUri = new Uri("/Assets/AppBar/feature.calendar.add.png", UriKind.Relative);
            AddReminderMenuButton.Click += new EventHandler(AddReminderMenuButton_Click);

            ApplicationBarMenuItem DeleteMenuItem = new ApplicationBarMenuItem();
            DeleteMenuItem.IsEnabled = true;
            DeleteMenuItem.Text = AppResources.Delete;
            DeleteMenuItem.Click += new EventHandler(DeleteMenuItem_Click);

            ApplicationBar.Buttons.Add(EditMenuButton);
            ApplicationBar.Buttons.Add(AddFuelEventMenuButton);
            ApplicationBar.Buttons.Add(AddServiceEventMenuButton);
            ApplicationBar.Buttons.Add(AddReminderMenuButton);

            ApplicationBar.MenuItems.Add(DeleteMenuItem);
        }

        private async void DeleteMenuItem_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(AppResources.DeleteCarQuestion, AppResources.Warning, MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.Cancel)
                return;

            await App.ViewModel.DeleteCar(_car);

            NavigationService.GoBack();
        }

        private void EditMenuButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/CarPageEdit.xaml", UriKind.Relative));
        }

        private void AddServiceEventMenuButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/ServiceEventPageEdit.xaml", UriKind.Relative));            
        }

        private void AddFuelEventMenuButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/FuelEventPageEdit.xaml", UriKind.Relative));
        }

        private void AddReminderMenuButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/ReminderPageEdit.xaml", UriKind.Relative));
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

        private void CarLoan_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            _carLoanExpanded = !_carLoanExpanded;

            SetCarLoanIconBoxes();
        }

        private void SetCarLoanIconBoxes()
        {
            CarLoanExpandCollapse.Source = (System.Windows.Media.ImageSource)(new System.Windows.Media.ImageSourceConverter().ConvertFromString(CarLoanExpandCollapseImage));

            this.txtBankName.Visibility = _carLoanExpanded ? Visibility.Visible : Visibility.Collapsed;
            this.txtBankAccountNumber.Visibility = _carLoanExpanded ? Visibility.Visible : Visibility.Collapsed;
            this.txtBankPayment.Visibility = _carLoanExpanded ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Insurance_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            _insuranceExpanded = !_insuranceExpanded;

            SetInsuranceIconBoxes();
        }

        private void SetInsuranceIconBoxes()
        {
            InsuranceExpandCollapse.Source = (System.Windows.Media.ImageSource)(new System.Windows.Media.ImageSourceConverter().ConvertFromString(InsuranceExpandCollapseImage));

            this.txtInsuranceName.Visibility = _insuranceExpanded ? Visibility.Visible : Visibility.Collapsed;
            this.txtInsuranceAccountNumber.Visibility = _insuranceExpanded ? Visibility.Visible : Visibility.Collapsed;
            this.txtInsurancePayment.Visibility = _insuranceExpanded ? Visibility.Visible : Visibility.Collapsed;
        }

        private void TotalCost_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            _totalCostExpanded = !_totalCostExpanded;

            SetTotalCostIconBoxes();
        }

        private void SetTotalCostIconBoxes()
        {
            TotalCostExpandCollapse.Source = (System.Windows.Media.ImageSource)(new System.Windows.Media.ImageSourceConverter().ConvertFromString(TotalCostExpandCollapseImage));

            this.GridTotalCost.Visibility = _totalCostExpanded ? Visibility.Visible : Visibility.Collapsed;            
        }

        
    }
}