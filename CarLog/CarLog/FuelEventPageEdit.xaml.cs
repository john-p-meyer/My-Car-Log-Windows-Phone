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
    public partial class FuelEventPageEdit : PhoneApplicationPage
    {
        private Guid _carID;
        private Guid _backupCarID;

        private FuelEvent _fuelEvent;
        private FuelEvent _backupFuelEvent;

        private Boolean _isNewFuelEvent = false;
        
        private Boolean _isLoaded;

        private Control _controlWithFocus;

        // Main Fuel Event Constants
        private const string _carIDSelected = "fuelEventCarIDSelected";
        private const string _dateSelected = "fuelEventDateSelected";
        private const string _fuelGradeSelected = "fuelEventFuelGradeSelected";
        private const string _priceText = "fuelEventPriceText";
        private const string _quantityText = "fuelEventQuantityText";
        private const string _odometerText = "fuelEventOdometerText";
        private const string _noteText = "fuelEventNoteText";
        private const string _ignoreForStats = "fuelEventIgnoreForStats";
        private const string _isNewFuelEventFlag = "fuelEventIsNewFuelEventFlag";
        private const string _locationText = "fuelEventLocationText";

        private Boolean _hasUnsavedChanges
        {
            get
            {
                if (this._fuelEvent == null)
                    return false;

                if (this._backupFuelEvent == null)
                    return false;

                if (this._fuelEvent.DateTime != this._backupFuelEvent.DateTime ||
                    this._fuelEvent.Distance != this._backupFuelEvent.Distance ||
                    this._fuelEvent.FuelGrade != this._backupFuelEvent.FuelGrade ||
                    this._fuelEvent.IgnoreForStats != this._backupFuelEvent.IgnoreForStats ||
                    this._fuelEvent.Note != this._backupFuelEvent.Note ||
                    this._fuelEvent.Odometer != this._backupFuelEvent.Odometer ||
                    this._fuelEvent.Price != this._backupFuelEvent.Price ||
                    this._fuelEvent.Quantity != this._backupFuelEvent.Quantity ||
                    this._fuelEvent.RecordStatus != this._backupFuelEvent.RecordStatus ||
                    this._fuelEvent.SyncStatus != this._backupFuelEvent.SyncStatus ||
                    this._fuelEvent.Title != this._backupFuelEvent.Title ||
                    this._fuelEvent.UpdateDate != this._backupFuelEvent.UpdateDate ||
                    this._fuelEvent.Location != this._backupFuelEvent.Location)
                    return true;

                return false;
            }
        }

        public FuelEventPageEdit()
        {
            InitializeComponent();

            AdControl adControl = AdControlGenerator.GenerateAdControl(2);
            adControl.AdRefreshed += adControl_AdRefreshed;
            adControl.ErrorOccurred += adControl_ErrorOccurred;

            this.LayoutRoot.Children.Add(adControl);

            this.lstCar.SelectionChanged += lstCar_SelectionChanged;
            this.lstFuelGrade.SelectionChanged += lstFuelGrade_SelectionChanged;
            GotFocus += FuelEventPageEdit_GotFocus;

            _isLoaded = false;

            this.BuildLocalizedApplicationBar();
        }

        void lstFuelGrade_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstFuelGrade.SelectedItem != null && _fuelEvent != null && _isLoaded)
            {
                _fuelEvent.FuelGrade = (FuelGrade)(lstFuelGrade.SelectedItem);
            }
        }

        void lstCar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstCar.SelectedItem != null && _fuelEvent != null && _isLoaded)
            {
                _carID = (lstCar.SelectedItem as Car).ID;
                _fuelEvent.CarID = (lstCar.SelectedItem as Car).ID;

                if (_isNewFuelEvent)
                {
                    Car car = App.ViewModel.FindCar(_carID);
                    try
                    {
                        _fuelEvent.Odometer = car.CurrentOdometer + Math.Round(car.AverageDistancePerFillUp, 0);
                    }
                    catch
                    {
                        _fuelEvent.Odometer = car.CurrentOdometer;
                    }
                }
            }
        }

        void FuelEventPageEdit_GotFocus(object sender, RoutedEventArgs e)
        {
            _controlWithFocus = e.OriginalSource as Control;
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

            if (PhoneApplicationService.Current.State.Keys.Contains(App.appStateCurrentFuelEvent))
            {
                Guid UniqueId;

                if (Guid.TryParse(PhoneApplicationService.Current.State[App.appStateCurrentFuelEvent].ToString(), out UniqueId))
                {
                    _fuelEvent = App.ViewModel.FindFuelEvent(UniqueId);

                    _carID = App.ViewModel.FindCar(_fuelEvent).ID;
                    _backupCarID = App.ViewModel.FindCar(_fuelEvent).ID;
                }
                else
                {
                    NavigationService.GoBack();
                }
            }

            if (_fuelEvent == null)
            {
                Car _car = null;

                _fuelEvent = new FuelEvent();

                _isNewFuelEvent = true;

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

                try
                {
                    _fuelEvent.Odometer = _car.CurrentOdometer + _car.AverageDistancePerFillUp;
                }
                catch
                {
                    _fuelEvent.Odometer = _car.CurrentOdometer;
                }
            }

            _backupFuelEvent = new FuelEvent(_fuelEvent.ID, _fuelEvent.CarID, _fuelEvent.DateTime, _fuelEvent.Distance, _fuelEvent.FuelGrade, _fuelEvent.Note, _fuelEvent.Odometer, _fuelEvent.Price, _fuelEvent.Quantity, _fuelEvent.Title, _fuelEvent.IgnoreForStats, _fuelEvent.RecordStatus, _fuelEvent.SyncStatus, _fuelEvent.UpdateDate, _fuelEvent.Location);

            // Load Lists
            lstCar.Items.Clear();
            foreach (Car carlst in App.ViewModel.Cars)
            {
                lstCar.Items.Add(carlst);
            }

            lstFuelGrade.Items.Clear();
            foreach (FuelGrade fuelGrade in Enum.GetValues(Type.GetType("CarLog.Core.DataModel.FuelGrade,CarLog.Core")))
            {
                lstFuelGrade.Items.Add(fuelGrade);
            }
            
            if (e.NavigationMode != NavigationMode.New)
            {
                if (this.State.Keys.Contains(_carIDSelected))
                {
                    Guid guidCarID;
                    if (Guid.TryParse(this.State[_carIDSelected].ToString(), out guidCarID))
                    {
                        this._fuelEvent.CarID = guidCarID;
                        this._carID = guidCarID;
                    }
                }

                if (this.State.Keys.Contains(_dateSelected) && e.NavigationMode != NavigationMode.Back)
                {
                    DateTime dateTimeSelected;
                    if (DateTime.TryParse(this.State[_dateSelected].ToString(), out dateTimeSelected))
                    {
                        this._fuelEvent.DateTime = dateTimeSelected;
                    }
                }

                if (this.State.Keys.Contains(_fuelGradeSelected))
                {
                    try
                    {
                        this._fuelEvent.FuelGrade = (FuelGrade)(this.State[_fuelGradeSelected]);
                    }
                    catch
                    {                        
                    }
                }

                if (this.State.Keys.Contains(_priceText))
                {
                    try
                    {
                        this._fuelEvent.Price = (Double)(this.State[_priceText]);
                    }
                    catch
                    {                        
                    }
                }

                if (this.State.Keys.Contains(_quantityText))
                {
                    try
                    {
                        this._fuelEvent.Quantity = (Double)(this.State[_quantityText]);
                    }
                    catch
                    {
                    }
                }

                if (this.State.Keys.Contains(_odometerText))
                {
                    try
                    {
                        this._fuelEvent.Odometer = (Double)(this.State[_odometerText]);                        
                    }
                    catch
                    {
                    }
                }

                if (this.State.Keys.Contains(_noteText))
                {
                    try
                    {
                        this._fuelEvent.Note = this.State[_noteText].ToString();
                    }
                    catch
                    {
                    }
                }

                if (this.State.Keys.Contains(_locationText))
                {
                    try
                    {
                        this._fuelEvent.Location = this.State[_locationText].ToString();
                    }
                    catch
                    {
                    }
                }

                if (this.State.Keys.Contains(_ignoreForStats))
                {
                    try
                    {
                        this._fuelEvent.IgnoreForStats = (Boolean)(this.State[_ignoreForStats]);
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

                foreach (FuelGrade item in lstFuelGrade.Items)
                {
                    if (item == _fuelEvent.FuelGrade)
                    {
                        lstFuelGrade.SelectedItem = item;
                    }
                }

                DataContext = _fuelEvent;
            }
            
            _isLoaded = true;

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            CommitControlWithFocus();

            if (e.NavigationMode != NavigationMode.Back)
            {
                this.State[_carIDSelected] = _fuelEvent.CarID;
                this.State[_dateSelected] = _fuelEvent.DateTime;
                this.State[_fuelGradeSelected] = _fuelEvent.FuelGrade;
                this.State[_priceText] = _fuelEvent.Price;
                this.State[_quantityText] = _fuelEvent.Quantity;
                this.State[_odometerText] = _fuelEvent.Odometer;
                this.State[_noteText] = _fuelEvent.Note;
                this.State[_locationText] = _fuelEvent.Location;
                this.State[_ignoreForStats] = _fuelEvent.IgnoreForStats;
            }
            else
            {
                if (_isNewFuelEvent || _hasUnsavedChanges)
                {
                    var result = MessageBox.Show(AppResources.MessageLeaveNoSave, AppResources.MessageTitleWarning, MessageBoxButton.OKCancel);

                    if (result == MessageBoxResult.Cancel)
                    {
                        e.Cancel = true;

                        return;
                    }
                }
                ResetFuelEvent();
                
            }

            base.OnNavigatingFrom(e);
        }

        private async void SaveMenuButton_Click(object sender, EventArgs e)
        {
            Car car = null;

            try
            {
                CommitControlWithFocus();

                if (!_isNewFuelEvent)
                {
                    car = App.ViewModel.FindCar(_backupCarID);
                    car.FuelEvents.Remove(_fuelEvent);

                    car.SetOdometer();
                    car.UpdateFuelEventDistance();
                    App.ViewModel.SortEvents(car.FuelEvents);
                }
                else
                {
                    _isNewFuelEvent = false;
                }

                car = App.ViewModel.FindCar(_carID);
                car.FuelEvents.Add(_fuelEvent);
                car.SetOdometer();
                car.UpdateFuelEventDistance();
                App.ViewModel.SortEvents(car.FuelEvents);

                App.ViewModel.RefreshLast10FuelEvents();

                await App.ViewModel.SaveData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(AppResources.MessageErrorOccurred, AppResources.MessageTitleError, MessageBoxButton.OK);
                return;
            }

            _backupFuelEvent = _fuelEvent;

            NavigationService.GoBack();
        }

        private void CancelMenuButton_Click(object sender, EventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void ResetFuelEvent()
        {
            this._fuelEvent.CarID = this._backupFuelEvent.CarID;
            this._fuelEvent.DateTime = this._backupFuelEvent.DateTime;
            this._fuelEvent.Distance = this._backupFuelEvent.Distance;
            this._fuelEvent.FuelGrade = this._backupFuelEvent.FuelGrade;
            this._fuelEvent.IgnoreForStats = this._backupFuelEvent.IgnoreForStats;
            this._fuelEvent.Note = this._backupFuelEvent.Note;
            this._fuelEvent.Odometer = this._backupFuelEvent.Odometer;
            this._fuelEvent.Price = this._backupFuelEvent.Price;
            this._fuelEvent.Quantity = this._backupFuelEvent.Quantity;
            this._fuelEvent.RecordStatus = this._backupFuelEvent.RecordStatus;
            this._fuelEvent.SyncStatus = this._backupFuelEvent.SyncStatus;
            this._fuelEvent.Title = this._backupFuelEvent.Title;
            this._fuelEvent.UpdateDate = this._backupFuelEvent.UpdateDate;
            this._fuelEvent.Location = this._backupFuelEvent.Location;
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            base.OnBackKeyPress(e);
        }

        private void CommitControlWithFocus()
        {
            if (_controlWithFocus == null || _controlWithFocus.GetType().ToString() != "System.Windows.Controls.TextBox") return;

            BindingExpression expression = _controlWithFocus.GetBindingExpression(TextBox.TextProperty);
            if (expression != null) expression.UpdateSource();
        }

        private void txtPrice_GotFocus(object sender, RoutedEventArgs e)
        {
            if (this.txtPrice.Text.Contains("0.000"))
            {
                this.txtPrice.Text = "";
            }
        }

        private void txtPrice_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.txtPrice.Text.Length == 0)
            {
                this.txtPrice.Text = "0.000";
            }
        }

        private void txtQuantity_GotFocus(object sender, RoutedEventArgs e)
        {
            if (this.txtQuantity.Text.Contains("0.000"))
            {
                this.txtQuantity.Text = "";
            }
        }

        private void txtQuantity_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.txtQuantity.Text.Length == 0)
            {
                this.txtQuantity.Text = "0.000";
            }
        }

        private void txtOdometer_GotFocus(object sender, RoutedEventArgs e)
        {
            // Removed because now the average distance between fill ups is added to the last known good odometer
            // So it should be a lot closer than the last known good odometer.
            /*if (_isNewFuelEvent && this._fuelEvent.Odometer == this._backupFuelEvent.Odometer)
            {
                this.txtOdometer.Text = "";
            }*/
        }

        private void txtOdometer_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.txtOdometer.Text.Length == 0)
            {
                this.txtOdometer.Text = this._backupFuelEvent.Odometer.ToString();
            }
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