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
using System.Windows.Data;
using CarLog.Core.DataModel;
using CarLog.Core.Resources;
using CarLog.Core.Helpers;

namespace CarLog
{
    public partial class ServiceEventPageEdit : PhoneApplicationPage
    {
        private Guid _carID;
        private Guid _backupCarID;

        private ServiceEvent _serviceEvent;
        private ServiceEvent _backupServiceEvent;

        private Boolean _isNewServiceEvent = false;

        private Boolean _isLoaded;

        private Control _controlWithFocus;

        // Main Service Event Constants
        private const string _carIDSelected = "ServiceEventCarIDSelected";
        private const string _dateSelected = "ServiceEventDateSelected";
        private const string _locationText = "ServiceEventLocationText";
        private const string _costText = "ServiceEventCostText";
        private const string _serviceEventTypeSelected = "ServiceEventServiceEventTypeSelected";
        private const string _odometerText = "ServiceEventOdometerText";
        private const string _noteText = "ServiceEventNoteText";
        
        private Boolean _hasUnsavedChanges
        {
            get
            {
                if (this._serviceEvent == null)
                    return false;

                if (this._backupServiceEvent == null)
                    return false;

                if (this._serviceEvent.DateTime != this._backupServiceEvent.DateTime ||
                    this._serviceEvent.CarID != this._backupServiceEvent.CarID ||
                    this._serviceEvent.Location != this._backupServiceEvent.Location ||
                    this._serviceEvent.Cost != this._backupServiceEvent.Cost ||
                    this._serviceEvent.Note != this._backupServiceEvent.Note ||
                    this._serviceEvent.Odometer != this._backupServiceEvent.Odometer ||
                    this._serviceEvent.ServiceEventDisplay != this._backupServiceEvent.ServiceEventDisplay ||
                    this._serviceEvent.ServiceEventType != this._backupServiceEvent.ServiceEventType ||
                    this._serviceEvent.RecordStatus != this._backupServiceEvent.RecordStatus ||
                    this._serviceEvent.SyncStatus != this._backupServiceEvent.SyncStatus ||
                    this._serviceEvent.Title != this._backupServiceEvent.Title ||
                    this._serviceEvent.UpdateDate != this._backupServiceEvent.UpdateDate)
                    return true;

                return false;
            }
        }

        public ServiceEventPageEdit()
        {
            InitializeComponent();

            AdControl adControl = AdControlGenerator.GenerateAdControl(2);
            adControl.AdRefreshed += adControl_AdRefreshed;
            adControl.ErrorOccurred += adControl_ErrorOccurred;

            this.LayoutRoot.Children.Add(adControl);

            this.lstCar.SelectionChanged += lstCar_SelectionChanged;
            this.lstServiceType.SelectionChanged += lstServiceType_SelectionChanged;
            GotFocus += ServiceEventPageEdit_GotFocus;

            _isLoaded = false;

            this.BuildLocalizedApplicationBar();
        }

        private void ServiceEventPageEdit_GotFocus(object sender, RoutedEventArgs e)
        {
            _controlWithFocus = e.OriginalSource as Control;
        }

        private void lstServiceType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstServiceType.SelectedItem != null && _serviceEvent != null && _isLoaded)
            {
                if (_serviceEvent.DateTime.ToString("MMMM dd, yyyy") == _serviceEvent.Title || _serviceEvent.Title == _serviceEvent.ServiceEventDisplay)
                {
                    _serviceEvent.ServiceEventType = ((ServiceEventDisplay)(lstServiceType.SelectedItem)).ServiceEventType;
                    _serviceEvent.Title = _serviceEvent.ServiceEventDisplay;
                }
                else
                {
                    _serviceEvent.ServiceEventType = ((ServiceEventDisplay)(lstServiceType.SelectedItem)).ServiceEventType;
                }
            }
        }

        private void lstCar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstCar.SelectedItem != null && _serviceEvent != null && _isLoaded)
            {
                _carID = (lstCar.SelectedItem as Car).ID;
                _serviceEvent.CarID = (lstCar.SelectedItem as Car).ID;

                if (_isNewServiceEvent)
                {
                    Car car = App.ViewModel.FindCar(_carID);
                    _serviceEvent.Odometer = car.CurrentOdometer;
                }
            }
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            _isLoaded = false;
            this.DataContext = null;

            // Load the data model if it is not already loaded
            if (!App.ViewModel.IsDataLoaded)
            {
                await App.ViewModel.LoadData();
            }
            else if (App.ViewModel.Cars.Count == 0)
            {
                await App.ViewModel.LoadData();
            }

            if (PhoneApplicationService.Current.State.Keys.Contains(App.appStateCurrentServiceEvent) && _serviceEvent == null)
            {
                Guid UniqueId;

                if (Guid.TryParse(PhoneApplicationService.Current.State[App.appStateCurrentServiceEvent].ToString(), out UniqueId))
                {
                    _serviceEvent = App.ViewModel.FindServiceEvent(UniqueId);

                    _carID = App.ViewModel.FindCar(_serviceEvent).ID;
                    _backupCarID = App.ViewModel.FindCar(_serviceEvent).ID;
                }
                else
                {
                    NavigationService.GoBack();
                }
            }

            if (_serviceEvent == null)
            {
                Car _car = null;

                _serviceEvent = new ServiceEvent();

                _isNewServiceEvent = true;

                if (PhoneApplicationService.Current.State.Keys.Contains(App.appStateCurrentCar))
                {
                    Guid UniqueId;

                    if (Guid.TryParse(PhoneApplicationService.Current.State[App.appStateCurrentCar].ToString(), out UniqueId))
                    {
                        _car = App.ViewModel.FindCar(UniqueId);
                    }
                    else
                    {
                        try
                        {
                            _car = App.ViewModel.Cars[0];
                        }
                        catch
                        {
                            NavigationService.GoBack();
                        }
                    }
                }
                else
                {
                    try
                    {
                        _car = App.ViewModel.Cars[0];
                    }
                    catch
                    {
                        NavigationService.GoBack();
                    }
                }
                
                // check if car is already selected.
                _carID = _car.ID;
                _backupCarID = _car.ID;                

                _serviceEvent.Odometer = _car.CurrentOdometer;
            }

            if (_backupServiceEvent == null)
                _backupServiceEvent = new ServiceEvent(_serviceEvent.ID, _serviceEvent.CarID, _serviceEvent.Cost, _serviceEvent.DateTime, _serviceEvent.Location, _serviceEvent.Note, _serviceEvent.Odometer, _serviceEvent.ServiceEventType, _serviceEvent.Title, _serviceEvent.RecordStatus, _serviceEvent.SyncStatus, _serviceEvent.UpdateDate);

            // Load Lists
            if (lstCar.Items.Count == 0)
            {
                lstCar.Items.Clear();
                foreach (Car carlst in App.ViewModel.Cars)
                {
                    lstCar.Items.Add(carlst);
                }
            }

            if (lstServiceType.Items.Count == 0)
            {
                this.lstServiceType.Items.Clear();
                foreach (ServiceEventType serviceEventType in Enum.GetValues(Type.GetType("CarLog.Core.DataModel.ServiceEventType,CarLog.Core")))
                {
                    //lstServiceType.Items.Add(serviceEventType);
                    lstServiceType.Items.Add(new ServiceEventDisplay(serviceEventType));
                }
            }

            if (e.NavigationMode != NavigationMode.New)
            {
                if (this.State.Keys.Contains(_carIDSelected))
                {
                    Guid guidCarID;
                    if (Guid.TryParse(this.State[_carIDSelected].ToString(), out guidCarID))
                    {
                        this._serviceEvent.CarID = guidCarID;
                        this._carID = guidCarID;
                    }
                }

                if (this.State.Keys.Contains(_dateSelected) && e.NavigationMode != NavigationMode.Back)
                {
                    DateTime dateTimeSelected;
                    if (DateTime.TryParse(this.State[_dateSelected].ToString(), out dateTimeSelected))
                    {
                        this._serviceEvent.DateTime = dateTimeSelected;
                    }
                }

                if (this.State.Keys.Contains(_serviceEventTypeSelected) && e.NavigationMode != NavigationMode.Back)
                {
                    try
                    {
                        this._serviceEvent.ServiceEventType = (ServiceEventType)(this.State[_serviceEventTypeSelected]);
                    }
                    catch
                    {                        
                    }
                }

                if (this.State.Keys.Contains(_locationText))
                {
                    try
                    {
                        this._serviceEvent.Location = (String)(this.State[_locationText]);
                    }
                    catch
                    {
                    }
                }

                if (this.State.Keys.Contains(_costText))
                {
                    try
                    {
                        this._serviceEvent.Cost = (Double)(this.State[_costText]);
                    }
                    catch
                    {
                    }
                }

                if (this.State.Keys.Contains(_odometerText))
                {
                    try
                    {
                        this._serviceEvent.Odometer = (Double)(this.State[_odometerText]);
                    }
                    catch
                    {
                    }
                }

                if (this.State.Keys.Contains(_noteText))
                {
                    try
                    {
                        this._serviceEvent.Note = this.State[_noteText].ToString();
                    }
                    catch
                    {
                    }
                }                
            }

            if (this.DataContext == null)
            {
                for (int i = 0; i < lstCar.Items.Count; i++)
                {
                    if (_carID == (lstCar.Items[i] as Car).ID)
                    {
                        lstCar.SelectedIndex = i;
                    }
                }

                foreach (ServiceEventDisplay item in lstServiceType.Items)
                {
                    if (item.ServiceEventType == _serviceEvent.ServiceEventType)
                    {
                        lstServiceType.SelectedItem = item;
                    }
                }

                DataContext = _serviceEvent;
            }

            _isLoaded = true;

            base.OnNavigatedTo(e);
        }

        protected override async void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            CommitControlWithFocus();

            if (e.NavigationMode != NavigationMode.Back)
            {
                this.State[_carIDSelected] = _serviceEvent.CarID;
                this.State[_dateSelected] = _serviceEvent.DateTime;
                this.State[_serviceEventTypeSelected] = _serviceEvent.ServiceEventType;
                this.State[_costText] = _serviceEvent.Cost;
                this.State[_locationText] = _serviceEvent.Location;
                this.State[_odometerText] = _serviceEvent.Odometer;
                this.State[_noteText] = _serviceEvent.Note;                
            }
            else
            {
                if (_isNewServiceEvent || _hasUnsavedChanges)
                {
                    var result = MessageBox.Show(AppResources.MessageLeaveNoSave, AppResources.MessageTitleWarning, MessageBoxButton.OKCancel);

                    if (result == MessageBoxResult.Cancel)
                    {
                        e.Cancel = true;

                        return;
                    }

                    ResetServiceEvent();

                    if (PhoneApplicationService.Current.State.Keys.Contains(App.appStateCurrentReminder))
                    {
                        await App.ViewModel.DeleteServiceEvent(_serviceEvent);
                    }
                }                                
            }

            base.OnNavigatingFrom(e);
        }

        private async void SaveMenuButton_Click(object sender, EventArgs e)
        {
            Car car = null;

            try
            {
                CommitControlWithFocus();

                if (!_isNewServiceEvent)
                {
                    car = App.ViewModel.FindCar(_backupCarID);
                    car.ServiceEvents.Remove(_serviceEvent);

                    car.SetOdometer();
                    App.ViewModel.SortEvents(car.ServiceEvents);
                }
                else
                {
                    _isNewServiceEvent = false;
                }

                car = App.ViewModel.FindCar(_carID);
                car.ServiceEvents.Add(_serviceEvent);
                car.SetOdometer();
                App.ViewModel.SortEvents(car.ServiceEvents);

                App.ViewModel.RefreshLast10ServiceEvents();

                if (PhoneApplicationService.Current.State.Keys.Contains(App.appStateCurrentReminder))
                {
                    if (PhoneApplicationService.Current.State.Keys.Contains(App.appStateCurrentServiceEvent))
                    {
                        PhoneApplicationService.Current.State.Remove(App.appStateCurrentServiceEvent);
                    }

                    Guid UniqueId;

                    if (Guid.TryParse(PhoneApplicationService.Current.State[App.appStateCurrentReminder].ToString(), out UniqueId))
                    {
                        Reminder _reminder = App.ViewModel.FindReminder(UniqueId);

                        if (_reminder != null)
                        {
                            if (_reminder.IsRecurring)
                            {
                                _reminder.Odometer = _serviceEvent.Odometer;
                                _reminder.DateTime = _serviceEvent.DateTime;
                            }
                            else
                            {
                                if (PhoneApplicationService.Current.State.Keys.Contains(App.appStateCurrentReminder))
                                {
                                    PhoneApplicationService.Current.State.Remove(App.appStateCurrentReminder);
                                }

                                await App.ViewModel.DeleteReminder(_reminder);
                            }
                        }
                    }
                }

                // Save Data
                await App.ViewModel.SaveData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(AppResources.MessageErrorOccurred, AppResources.MessageTitleError, MessageBoxButton.OK);
                return;
            }

            _backupServiceEvent = _serviceEvent;            

            NavigationService.GoBack();
        }

        private void CancelMenuButton_Click(object sender, EventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void txtCost_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.txtCost.Text.Length == 0)
            {
                this.txtCost.Text = "0.00";
            }
        }

        private void txtCost_GotFocus(object sender, RoutedEventArgs e)
        {
            if (this.txtCost.Text.EndsWith("0.00"))
            {
                this.txtCost.Text = "";
            }
        }

        private void CommitControlWithFocus()
        {
            if (_controlWithFocus == null || _controlWithFocus.GetType().ToString() != "System.Windows.Controls.TextBox") return;

            BindingExpression expression = _controlWithFocus.GetBindingExpression(TextBox.TextProperty);
            if (expression != null) expression.UpdateSource();
        }

        private void ResetServiceEvent()
        {
            this._serviceEvent.CarID = this._backupServiceEvent.CarID;
            this._serviceEvent.DateTime = this._backupServiceEvent.DateTime;
            this._serviceEvent.Cost = this._backupServiceEvent.Cost;
            this._serviceEvent.ServiceEventType = this._backupServiceEvent.ServiceEventType;
            this._serviceEvent.Location = this._backupServiceEvent.Location;
            this._serviceEvent.Note = this._backupServiceEvent.Note;
            this._serviceEvent.Odometer = this._backupServiceEvent.Odometer;
            this._serviceEvent.RecordStatus = this._backupServiceEvent.RecordStatus;
            this._serviceEvent.SyncStatus = this._backupServiceEvent.SyncStatus;
            this._serviceEvent.Title = this._backupServiceEvent.Title;
            this._serviceEvent.UpdateDate = this._backupServiceEvent.UpdateDate;
        }

        private void BuildLocalizedApplicationBar()
        {
            ApplicationBar.Buttons.Clear();

            ApplicationBarIconButton SaveMenuButton = new ApplicationBarIconButton();
            SaveMenuButton.IsEnabled = true;
            SaveMenuButton.Text = AppResources.MenuButtonSave;
            SaveMenuButton.IconUri = new Uri("/Assets/AppBar/save.png", UriKind.Relative);
            SaveMenuButton.Click += new EventHandler(SaveMenuButton_Click);

            ApplicationBarIconButton CancelMenuButton = new ApplicationBarIconButton();
            CancelMenuButton.IsEnabled = true;
            CancelMenuButton.Text = AppResources.MenuButtonCancel;
            CancelMenuButton.IconUri = new Uri("/Assets/AppBar/cancel.png", UriKind.Relative);
            CancelMenuButton.Click += new EventHandler(CancelMenuButton_Click);

            ApplicationBar.Buttons.Add(SaveMenuButton);
            ApplicationBar.Buttons.Add(CancelMenuButton);
        }

        private void txtOdometer_GotFocus(object sender, RoutedEventArgs e)
        {
            if (_isNewServiceEvent && this._serviceEvent.Odometer == this._backupServiceEvent.Odometer)
            {
                this.txtOdometer.Text = "";
            }
        }

        private void txtOdometer_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.txtOdometer.Text.Length == 0)
            {
                this.txtOdometer.Text = this._backupServiceEvent.Odometer.ToString();
            }
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