using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Data;
using Microsoft.Advertising.Mobile.UI;
using CarLog.Core.DataModel;
using CarLog.Core.Resources;
using CarLog.Core.Helpers;

namespace CarLog
{
    public partial class CarPageEdit : PhoneApplicationPage
    {
        private Car _car;
        private Car _backupCar;

        private Boolean _isNewCar = false;

        private Control _controlWithFocus;

        // Main Car Constants
        private const string _makeText = "CarMakeText";
        private const string _modelText = "CarModelText";
        private const string _startOdometerText = "CarStartOdometerText";
        private const string _titleText = "CarTitleText";
        private const string _vinText = "CarVinText";
        private const string _yearText = "CarYearText";
        private const string _isNewCarFlag = "CarIsNewCar";
        private const string _bankName = "CarBankName";
        private const string _bankAccountNumber = "CarBankAccountNumber";
        private const string _bankPayment = "CarBankPayment";
        private const string _insuranceName = "CarInsuranceName";
        private const string _insuranceAccountNumber = "CarInsuranceAccountNumber";
        private const string _insurancePayment = "CarInsurancePayment";

        private Boolean _hasUnsavedChanges
        {
            get
            {
                if (_car == null)
                    return false;

                if (_backupCar == null)
                    return false;

                if (_car.Make != _backupCar.Make ||
                    _car.Model != _backupCar.Model ||
                    _car.StartOdometer != _backupCar.StartOdometer ||
                    _car.Title != _backupCar.Title ||
                    _car.VIN != _backupCar.VIN ||
                    _car.Year != _backupCar.Year)
                    return true;

                return false;
            }
        }

        public CarPageEdit()
        {
            InitializeComponent();

            AdControl adControl = AdControlGenerator.GenerateAdControl(2);
            adControl.AdRefreshed += adControl_AdRefreshed;
            adControl.ErrorOccurred += adControl_ErrorOccurred;

            this.LayoutRoot.Children.Add(adControl);            

            GotFocus += CarPageEdit_GotFocus;

            this.BuildLocalizedApplicationBar();
        }

        void CarPageEdit_GotFocus(object sender, RoutedEventArgs e)
        {
            _controlWithFocus = e.OriginalSource as Control;
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
            
            if (_car == null)
            {
                _car = new Car();

                _isNewCar = true;
            }

            _backupCar = new Car(_car.ID, _car.CurrentOdometer, _car.Make, _car.Model, _car.StartOdometer, _car.Title, _car.VIN, _car.Year, _car.BankName, _car.BankAccountNumber, _car.BankPayment, _car.InsuranceName, _car.InsuranceAccountNumber, _car.InsurancePayment, _car.RecordStatus, _car.SyncStatus, _car.UpdateDate);

            if (e.NavigationMode != NavigationMode.New)
            {

                if (this.State.Keys.Contains(_makeText))
                {
                    try
                    {
                        _car.Make = (String)this.State[_makeText];
                    }
                    catch
                    {
                    }
                }

                if (this.State.Keys.Contains(_modelText))
                {
                    try
                    {
                        _car.Model = (String)this.State[_modelText];
                    }
                    catch
                    {
                    }
                }

                if (this.State.Keys.Contains(_startOdometerText))
                {
                    try
                    {
                        _car.StartOdometer = (Double)this.State[_startOdometerText];
                    }
                    catch
                    {
                    }
                }

                if (this.State.Keys.Contains(_titleText))
                {
                    try
                    {
                        _car.Title = (String)this.State[_titleText];
                    }
                    catch
                    {
                    }
                }

                if (this.State.Keys.Contains(_vinText))
                {
                    try
                    {
                        _car.VIN = (String)this.State[_vinText];
                    }
                    catch
                    {
                    }
                }

                if (this.State.Keys.Contains(_yearText))
                {
                    try
                    {
                        _car.Year = (String)this.State[_yearText];
                    }
                    catch
                    {
                    }
                }

                if (this.State.Keys.Contains(_isNewCarFlag))
                {
                    try
                    {
                        _isNewCar = (Boolean)this.State[_isNewCarFlag];
                    }
                    catch
                    {
                    }
                }

                if (this.State.Keys.Contains(_bankName))
                {
                    try
                    {
                        _car.BankName = (String)this.State[_bankName];
                    }
                    catch 
                    { 
                    }
                }

                if (this.State.Keys.Contains(_bankAccountNumber))
                {
                    try
                    {
                        _car.BankAccountNumber = (String)this.State[_bankAccountNumber];
                    }
                    catch
                    {
                    }
                }

                if (this.State.Keys.Contains(_bankPayment))
                {
                    try
                    {
                        _car.BankPayment = (Double)this.State[_bankPayment];
                    }
                    catch
                    {
                    }
                }

                if (this.State.Keys.Contains(_insuranceName))
                {
                    try
                    {
                        _car.InsuranceName = (String)this.State[_insuranceName];
                    }
                    catch
                    {
                    }
                }

                if (this.State.Keys.Contains(_insuranceAccountNumber))
                {
                    try
                    {
                        _car.InsuranceAccountNumber = (String)this.State[_insuranceAccountNumber];
                    }
                    catch
                    {
                    }
                }

                if (this.State.Keys.Contains(_insurancePayment))
                {
                    try
                    {
                        _car.InsurancePayment = (Double)this.State[_insurancePayment];
                    }
                    catch
                    {
                    }
                }
            }
            
            DataContext = _car;

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            CommitControlWithFocus();

            if (e.NavigationMode != NavigationMode.Back)
            {
                this.State[_makeText] = _car.Make;
                this.State[_modelText] = _car.Model;
                this.State[_startOdometerText] = _car.StartOdometer;
                this.State[_titleText] = _car.Title;
                this.State[_vinText] = _car.VIN;
                this.State[_yearText] = _car.Year;
                this.State[_isNewCarFlag] = _isNewCar;
                this.State[_bankName] = _car.BankName;
                this.State[_bankAccountNumber] = _car.BankAccountNumber;
                this.State[_bankPayment] = _car.BankPayment;
                this.State[_insuranceName] = _car.InsuranceName;
                this.State[_insuranceAccountNumber] = _car.InsuranceAccountNumber;
                this.State[_insurancePayment] = _car.InsurancePayment;
            }
            else
            {
                if (_isNewCar || _hasUnsavedChanges)
                {
                    var result = MessageBox.Show(AppResources.MessageLeaveNoSave, AppResources.MessageTitleWarning, MessageBoxButton.OKCancel);

                    if (result == MessageBoxResult.Cancel)
                    {
                        e.Cancel = true;

                        return;
                    }
                }
                ResetCar();
            }            

            base.OnNavigatingFrom(e);
        }

        private void ResetCar()
        {
            _car.Make = _backupCar.Make;
            _car.Model = _backupCar.Model;
            _car.StartOdometer = _backupCar.StartOdometer;
            _car.Title = _backupCar.Title;
            _car.VIN = _backupCar.VIN;
            _car.Year = _backupCar.Year;
            _car.BankName = _backupCar.BankName;
            _car.BankAccountNumber = _backupCar.BankAccountNumber;
            _car.BankPayment = _backupCar.BankPayment;
            _car.InsuranceName = _backupCar.InsuranceName;
            _car.InsuranceAccountNumber = _backupCar.InsuranceAccountNumber;
            _car.InsurancePayment = _backupCar.InsurancePayment;
        }

        private async void SaveMenuButton_Click(object sender, EventArgs e)
        {
            CommitControlWithFocus();

            // If this was a new car add it to the list
            if (_isNewCar)
            {
                _isNewCar = false;
                App.ViewModel.Cars.Add(_car);
            }

            _car.SetOdometer();
            _car.UpdateFuelEventDistance();

            /// Save the list
            await App.ViewModel.SaveData();

            _backupCar = _car;

            NavigationService.GoBack();
        }

        private void CancelMenuButton_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void CommitControlWithFocus()
        {
            if (_controlWithFocus == null || _controlWithFocus.GetType().ToString() != "System.Windows.Controls.TextBox") return;

            BindingExpression expression = _controlWithFocus.GetBindingExpression(TextBox.TextProperty);
            if (expression != null) expression.UpdateSource();
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

        private void txtPayment_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox txt = sender as TextBox;

            if (txt != null)
            {
                if (txt.Text.Contains("0.00"))
                {
                    txt.Text = "";
                }
            }
        }

        private void txtPayment_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox txt = sender as TextBox;

            if (txt != null)
            {
                if (txt.Text.Length == 0)
                {
                    txt.Text = "0.00";
                }
            }
        }
    }
}