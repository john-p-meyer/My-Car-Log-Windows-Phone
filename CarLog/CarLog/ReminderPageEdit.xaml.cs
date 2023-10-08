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
using CarLog.Core.Helpers;
using CarLog.Core.Resources;

namespace CarLog
{
    public partial class ReminderPageEdit : PhoneApplicationPage
    {
        private Guid _carID;
        private Guid _backupCarID;

        private Reminder _reminder;
        private Reminder _backupReminder;

        private Boolean _isNewReminder = false;

        private Boolean _isLoaded;

        private Control _controlWithFocus;

        // Main Reminder Constants
        // TODO: Add constants for Reminders
        private const string _reminderCarIDSelected = "ReminderCarIDSelected";
        private const string _reminderDateTimeSelected = "ReminderDateTimeSelected";
        private const string _reminderDistanceText = "ReminderDistanceText";
        private const string _reminderEndDateSelected = "ReminderEndDateSelected";
        private const string _reminderEndOdometerText = "ReminderEndOdometerText";
        private const string _reminderIsRecurringSelected = "ReminderIsRecurringSelected";
        private const string _reminderMonthsText = "ReminderMonthsText";
        private const string _reminderNoteText = "ReminderNoteText";
        private const string _reminderOdometerText = "ReminderOdometerText";
        private const string _reminderReminderTypeSelected = "ReminderReminderTypeSelected";
        private const string _reminderServiceEventTypeSelected = "ReminderServiceEventTypeSelected";
        private const string _reminderTitleText = "ReminderTitleText";
        private const string _reminderIsNewReminderFlag = "ReminderIsNewReminderFlag";

        private Boolean _hasUnsavedChanges
        {
            get
            {
                if (this._reminder == null)
                    return false;

                if (this._backupReminder == null)
                    return false;

                if (this._reminder.CarID != this._backupReminder.CarID ||
                    this._reminder.DateTime != this._backupReminder.DateTime ||
                    this._reminder.Distance != this._backupReminder.Distance ||
                    this._reminder.EndDate != this._backupReminder.EndDate ||
                    this._reminder.EndOdometer != this._backupReminder.EndOdometer ||
                    this._reminder.IsRecurring != this._backupReminder.IsRecurring ||
                    this._reminder.Months != this._backupReminder.Months ||
                    this._reminder.Note != this._backupReminder.Note ||
                    this._reminder.Odometer != this._backupReminder.Odometer ||
                    this._reminder.RecordStatus != this._backupReminder.RecordStatus ||
                    this._reminder.ReminderType != this._backupReminder.ReminderType ||
                    this._reminder.ServiceEventType != this._backupReminder.ServiceEventType ||
                    this._reminder.SyncStatus != this._backupReminder.SyncStatus ||
                    this._reminder.Title != this._backupReminder.Title ||
                    this._reminder.UpdateDate != this._backupReminder.UpdateDate)
                    return true;

                return false;
            }
        }

        public ReminderPageEdit()
        {
            InitializeComponent();

            AdControl adControl = AdControlGenerator.GenerateAdControl(2);
            adControl.AdRefreshed += adControl_AdRefreshed;
            adControl.ErrorOccurred += adControl_ErrorOccurred;

            this.LayoutRoot.Children.Add(adControl);

            this.lstCar.SelectionChanged += lstCar_SelectionChanged;
            this.lstServiceType.SelectionChanged += lstServiceType_SelectionChanged;
            this.lstReminderType.SelectionChanged += lstReminderType_SelectionChanged;
            GotFocus += ReminderPageEdit_GotFocus;

            _isLoaded = false;

            this.BuildLocalizedApplicationBar();
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

        private void ReminderPageEdit_GotFocus(object sender, RoutedEventArgs e)
        {
            _controlWithFocus = e.OriginalSource as Control;
        }

        private void lstServiceType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstServiceType.SelectedItem != null && _reminder != null && _isLoaded)
            {
                if (_reminder.DateTime.ToString("MMMM dd, yyyy") == _reminder.Title || _reminder.Title == _reminder.ServiceEventDisplay)
                {
                    _reminder.ServiceEventType = ((ServiceEventDisplay)(lstServiceType.SelectedItem)).ServiceEventType;
                    _reminder.Title = _reminder.ServiceEventDisplay;
                }
                else
                {
                    _reminder.ServiceEventType = ((ServiceEventDisplay)(lstServiceType.SelectedItem)).ServiceEventType;
                }
            }
        }

        private void lstCar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstCar.SelectedItem != null && _reminder != null && _isLoaded)
            {
                _carID = (lstCar.SelectedItem as Car).ID;
                _reminder.CarID = (lstCar.SelectedItem as Car).ID;

                if (_isNewReminder)
                {
                    Car car = App.ViewModel.FindCar(_carID);
                    _reminder.Odometer = car.CurrentOdometer;
                }
            }
        }

        void lstReminderType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstReminderType.SelectedItem != null && _reminder != null && _isLoaded)
            {
                _reminder.ReminderType = ((ReminderTypeDisplay)(lstReminderType.SelectedItem)).ReminderType;

                SetReminderFieldVisibility();                
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

            if (PhoneApplicationService.Current.State.Keys.Contains(App.appStateCurrentReminder) && _reminder == null)
            {
                Guid UniqueId;

                if (Guid.TryParse(PhoneApplicationService.Current.State[App.appStateCurrentReminder].ToString(), out UniqueId))
                {
                    _reminder = App.ViewModel.FindReminder(UniqueId);

                    _carID = App.ViewModel.FindCar(_reminder).ID;
                    _backupCarID = App.ViewModel.FindCar(_reminder).ID;
                }
                else
                {
                    NavigationService.GoBack();
                }
            }

            if (_reminder == null)
            {
                Car _car = null;

                _reminder = new Reminder();

                _isNewReminder = true;

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

                _reminder.Odometer = _car.CurrentOdometer;
            }

            if (_backupReminder == null)
                _backupReminder = new Reminder(_reminder.ID, _reminder.CarID, _reminder.DateTime, _reminder.Note, _reminder.Odometer, _reminder.Title, _reminder.ServiceEventType, _reminder.ReminderType, _reminder.Months, _reminder.Distance, _reminder.EndDate, _reminder.EndOdometer, _reminder.IsRecurring, _reminder.RecordStatus, _reminder.SyncStatus, _reminder.UpdateDate);

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

            if (lstReminderType.Items.Count == 0)
            {
                this.lstReminderType.Items.Clear();
                foreach (ReminderType reminderType in Enum.GetValues(Type.GetType("CarLog.Core.DataModel.ReminderType,CarLog.Core")))
                {
                    lstReminderType.Items.Add(new ReminderTypeDisplay(reminderType));
                }
            }

            if (e.NavigationMode != NavigationMode.New)
            {
                if (this.State.Keys.Contains(_reminderCarIDSelected))
                {
                    Guid guidCarID;
                    if (Guid.TryParse(this.State[_reminderCarIDSelected].ToString(), out guidCarID))
                    {
                        this._reminder.CarID = guidCarID;
                        this._carID = guidCarID;
                    }
                }

                if (this.State.Keys.Contains(_reminderDateTimeSelected) && e.NavigationMode != NavigationMode.Back)
                {
                    DateTime dateTimeSelected;
                    if (DateTime.TryParse(this.State[_reminderDateTimeSelected].ToString(), out dateTimeSelected))
                    {
                        this._reminder.DateTime = dateTimeSelected;
                    }
                }

                if (this.State.Keys.Contains(_reminderServiceEventTypeSelected) && e.NavigationMode != NavigationMode.Back)
                {
                    try
                    {
                        this._reminder.ServiceEventType = (ServiceEventType)(this.State[_reminderServiceEventTypeSelected]);
                    }
                    catch
                    {
                    }
                }

                if (this.State.Keys.Contains(_reminderDistanceText))
                {
                    try
                    {
                        this._reminder.Distance = (Double)(this.State[_reminderDistanceText]);
                    }
                    catch
                    {
                    }
                }

                if (this.State.Keys.Contains(_reminderEndDateSelected) && e.NavigationMode != NavigationMode.Back)
                {
                    DateTime endDateSelected;
                    if (DateTime.TryParse(this.State[_reminderEndDateSelected].ToString(), out endDateSelected))
                    {
                        this._reminder.EndDate = endDateSelected;
                    }
                }

                if (this.State.Keys.Contains(_reminderEndOdometerText))
                {
                    try
                    {
                        this._reminder.EndOdometer = (Double)(this.State[_reminderEndOdometerText]);
                    }
                    catch
                    {
                    }
                }

                if (this.State.Keys.Contains(_reminderIsRecurringSelected))
                {
                    try
                    {
                        this._reminder.IsRecurring = (Boolean)(this.State[_reminderIsRecurringSelected]);
                    }
                    catch
                    {
                    }
                }

                if (this.State.Keys.Contains(_reminderMonthsText))
                {
                    try
                    {
                        this._reminder.Months = (int)(this.State[_reminderMonthsText]);
                    }
                    catch
                    {
                    }
                }

                if (this.State.Keys.Contains(_reminderNoteText))
                {
                    try
                    {
                        this._reminder.Note = (String)(this.State[_reminderNoteText].ToString());
                    }
                    catch
                    {
                    }
                }

                if (this.State.Keys.Contains(_reminderOdometerText))
                {
                    try
                    {
                        this._reminder.Odometer = (Double)(this.State[_reminderOdometerText]);
                    }
                    catch
                    {
                    }
                }

                if (this.State.Keys.Contains(_reminderReminderTypeSelected) && e.NavigationMode != NavigationMode.Back)
                {
                    try
                    {
                        this._reminder.ReminderType = (ReminderType)(this.State[_reminderReminderTypeSelected]);
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
                    if (item.ServiceEventType == _reminder.ServiceEventType)
                    {
                        lstServiceType.SelectedItem = item;
                    }
                }

                foreach (ReminderTypeDisplay item in lstReminderType.Items)
                {
                    if (item.ReminderType == _reminder.ReminderType)
                    {
                        lstReminderType.SelectedItem = item;
                    }
                }

                DataContext = _reminder;
            }

            _isLoaded = true;

            SetReminderFieldVisibility();

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            CommitControlWithFocus();

            if (e.NavigationMode != NavigationMode.Back)
            {
                this.State[_reminderCarIDSelected] = _reminder.CarID;
                this.State[_reminderDateTimeSelected] = _reminder.DateTime;
                this.State[_reminderDistanceText] = _reminder.Distance;
                this.State[_reminderEndDateSelected] = this._reminder.EndDate;
                this.State[_reminderEndOdometerText] = this._reminder.EndOdometer;
                this.State[_reminderIsRecurringSelected] = this._reminder.IsRecurring;
                this.State[_reminderMonthsText] = this._reminder.Months;
                this.State[_reminderNoteText] = this._reminder.Note;
                this.State[_reminderOdometerText] = this._reminder.Odometer;
                this.State[_reminderReminderTypeSelected] = this._reminder.ReminderType;
                this.State[_reminderServiceEventTypeSelected] = this._reminder.ServiceEventType;                
                this.State[_reminderTitleText] = this._reminder.Title;                
            }
            else
            {
                if (_isNewReminder || _hasUnsavedChanges)
                {
                    var result = MessageBox.Show(AppResources.MessageLeaveNoSave, AppResources.MessageTitleWarning, MessageBoxButton.OKCancel);

                    if (result == MessageBoxResult.Cancel)
                    {
                        e.Cancel = true;

                        return;
                    }
                }
                ResetReminder();

            }

            base.OnNavigatingFrom(e);
        }

        private async void SaveMenuButton_Click(object sender, EventArgs e)
        {
            Car car = null;

            try
            {
                CommitControlWithFocus();

                //_reminder

                if (!_isNewReminder)
                {
                    car = App.ViewModel.FindCar(_backupCarID);
                    car.Reminders.Remove(_reminder);

                    car.SetOdometer();
                    App.ViewModel.SortEvents(car.Reminders);
                }
                else
                {
                    _isNewReminder = false;
                }

                if (_reminder.ReminderType == ReminderType.Date || _reminder.ReminderType == ReminderType.Odometer)
                {
                    _reminder.IsRecurring = false;
                }

                car = App.ViewModel.FindCar(_carID);
                car.Reminders.Add(_reminder);
                car.SetOdometer();
                App.ViewModel.SortEvents(car.Reminders);

                App.ViewModel.RefreshLast10Reminders();

                // Save Data
                await App.ViewModel.SaveData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(AppResources.MessageErrorOccurred, AppResources.MessageTitleError, MessageBoxButton.OK);
                return;
            }

            _backupReminder = _reminder;

            NavigationService.GoBack();
        }

        private void CancelMenuButton_Click(object sender, EventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void CommitControlWithFocus()
        {
            if (_controlWithFocus == null || _controlWithFocus.GetType().ToString() != "System.Windows.Controls.TextBox") return;

            BindingExpression expression = _controlWithFocus.GetBindingExpression(TextBox.TextProperty);
            if (expression != null) expression.UpdateSource();
        }

        private void ResetReminder()
        {
            this._reminder.CarID = this._backupReminder.CarID;
            this._reminder.DateTime = this._backupReminder.DateTime;
            this._reminder.Distance = this._backupReminder.Distance;
            this._reminder.EndDate = this._backupReminder.EndDate;
            this._reminder.EndOdometer = this._backupReminder.EndOdometer;
            this._reminder.IsRecurring = this._backupReminder.IsRecurring;
            this._reminder.Months = this._backupReminder.Months;
            this._reminder.Note = this._backupReminder.Note;
            this._reminder.Odometer = this._backupReminder.Odometer;
            this._reminder.RecordStatus = this._backupReminder.RecordStatus;
            this._reminder.ReminderType = this._backupReminder.ReminderType;
            this._reminder.ServiceEventType = this._backupReminder.ServiceEventType;
            this._reminder.SyncStatus = this._backupReminder.SyncStatus;
            this._reminder.Title = this._backupReminder.Title;
            this._reminder.UpdateDate = this._backupReminder.UpdateDate;
        }

        private void SetReminderFieldVisibility()
        {
            switch (_reminder.ReminderType)
            {
                case ReminderType.Date:
                    this.textBlockLastServiceDateTime.Visibility = System.Windows.Visibility.Collapsed;
                    this.datLastServiceDateTime.Visibility = System.Windows.Visibility.Collapsed;
                    this.textBlockLastServiceOdometer.Visibility = System.Windows.Visibility.Collapsed;
                    this.txtLastServiceOdometer.Visibility = System.Windows.Visibility.Collapsed;
                    this.textBlockMonths.Visibility = System.Windows.Visibility.Collapsed;
                    this.txtMonths.Visibility = System.Windows.Visibility.Collapsed;
                    this.textBlockDistance.Visibility = System.Windows.Visibility.Collapsed;
                    this.txtDistance.Visibility = System.Windows.Visibility.Collapsed;
                    this.textBlockNextServiceDate.Visibility = System.Windows.Visibility.Visible;
                    this.datNextServiceDate.Visibility = System.Windows.Visibility.Visible;
                    this.textBlockNextOdometer.Visibility = System.Windows.Visibility.Collapsed;
                    this.txtNextOdometer.Visibility = System.Windows.Visibility.Collapsed;
                    this.txtRecurring.Visibility = System.Windows.Visibility.Collapsed;
                    this.toggleRecurring.Visibility = System.Windows.Visibility.Collapsed;

                    break;

                case ReminderType.DistanceSince:
                    this.textBlockLastServiceDateTime.Visibility = System.Windows.Visibility.Collapsed;
                    this.datLastServiceDateTime.Visibility = System.Windows.Visibility.Collapsed;
                    this.textBlockLastServiceOdometer.Visibility = System.Windows.Visibility.Visible;
                    this.txtLastServiceOdometer.Visibility = System.Windows.Visibility.Visible;
                    this.textBlockMonths.Visibility = System.Windows.Visibility.Collapsed;
                    this.txtMonths.Visibility = System.Windows.Visibility.Collapsed;
                    this.textBlockDistance.Visibility = System.Windows.Visibility.Visible;
                    this.txtDistance.Visibility = System.Windows.Visibility.Visible;
                    this.textBlockNextServiceDate.Visibility = System.Windows.Visibility.Collapsed;
                    this.datNextServiceDate.Visibility = System.Windows.Visibility.Collapsed;
                    this.textBlockNextOdometer.Visibility = System.Windows.Visibility.Collapsed;
                    this.txtNextOdometer.Visibility = System.Windows.Visibility.Collapsed;
                    this.txtRecurring.Visibility = System.Windows.Visibility.Visible;
                    this.toggleRecurring.Visibility = System.Windows.Visibility.Visible;

                    break;

                case ReminderType.MonthsSince:
                    this.textBlockLastServiceDateTime.Visibility = System.Windows.Visibility.Visible;
                    this.datLastServiceDateTime.Visibility = System.Windows.Visibility.Visible;
                    this.textBlockLastServiceOdometer.Visibility = System.Windows.Visibility.Collapsed;
                    this.txtLastServiceOdometer.Visibility = System.Windows.Visibility.Collapsed;
                    this.textBlockMonths.Visibility = System.Windows.Visibility.Visible;
                    this.txtMonths.Visibility = System.Windows.Visibility.Visible;
                    this.textBlockDistance.Visibility = System.Windows.Visibility.Collapsed;
                    this.txtDistance.Visibility = System.Windows.Visibility.Collapsed;
                    this.textBlockNextServiceDate.Visibility = System.Windows.Visibility.Collapsed;
                    this.datNextServiceDate.Visibility = System.Windows.Visibility.Collapsed;
                    this.textBlockNextOdometer.Visibility = System.Windows.Visibility.Collapsed;
                    this.txtNextOdometer.Visibility = System.Windows.Visibility.Collapsed;
                    this.txtRecurring.Visibility = System.Windows.Visibility.Visible;
                    this.toggleRecurring.Visibility = System.Windows.Visibility.Visible;

                    break;

                case ReminderType.Odometer:
                    this.textBlockLastServiceDateTime.Visibility = System.Windows.Visibility.Collapsed;
                    this.datLastServiceDateTime.Visibility = System.Windows.Visibility.Collapsed;
                    this.textBlockLastServiceOdometer.Visibility = System.Windows.Visibility.Collapsed;
                    this.txtLastServiceOdometer.Visibility = System.Windows.Visibility.Collapsed;
                    this.textBlockMonths.Visibility = System.Windows.Visibility.Collapsed;
                    this.txtMonths.Visibility = System.Windows.Visibility.Collapsed;
                    this.textBlockDistance.Visibility = System.Windows.Visibility.Collapsed;
                    this.txtDistance.Visibility = System.Windows.Visibility.Collapsed;
                    this.textBlockNextServiceDate.Visibility = System.Windows.Visibility.Collapsed;
                    this.datNextServiceDate.Visibility = System.Windows.Visibility.Collapsed;
                    this.textBlockNextOdometer.Visibility = System.Windows.Visibility.Visible;
                    this.txtNextOdometer.Visibility = System.Windows.Visibility.Visible;
                    this.txtRecurring.Visibility = System.Windows.Visibility.Collapsed;
                    this.toggleRecurring.Visibility = System.Windows.Visibility.Collapsed;

                    break;

                case ReminderType.MonthsOrDistanceSince:
                    this.textBlockLastServiceDateTime.Visibility = System.Windows.Visibility.Visible;
                    this.datLastServiceDateTime.Visibility = System.Windows.Visibility.Visible;
                    this.textBlockLastServiceOdometer.Visibility = System.Windows.Visibility.Visible;
                    this.txtLastServiceOdometer.Visibility = System.Windows.Visibility.Visible;
                    this.textBlockMonths.Visibility = System.Windows.Visibility.Visible;
                    this.txtMonths.Visibility = System.Windows.Visibility.Visible;
                    this.textBlockDistance.Visibility = System.Windows.Visibility.Visible;
                    this.txtDistance.Visibility = System.Windows.Visibility.Visible;
                    this.textBlockNextServiceDate.Visibility = System.Windows.Visibility.Collapsed;
                    this.datNextServiceDate.Visibility = System.Windows.Visibility.Collapsed;
                    this.textBlockNextOdometer.Visibility = System.Windows.Visibility.Collapsed;
                    this.txtNextOdometer.Visibility = System.Windows.Visibility.Collapsed;
                    this.txtRecurring.Visibility = System.Windows.Visibility.Visible;
                    this.toggleRecurring.Visibility = System.Windows.Visibility.Visible;

                    break;

                default:
                    break;
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