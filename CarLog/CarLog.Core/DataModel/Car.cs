using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarLog.Core.Helpers;

namespace CarLog.Core.DataModel
{
    public class Car : INotifyPropertyChanged
    {
        private Guid _id;
        private string _title;
        private string _year;
        private string _make;
        private string _model;
        private string _vin;
        private Double _startOdometer;
        private Double _currentOdometer;
        private RecordStatus _recordStatus;
        private SyncStatus _syncStatus;
        private DateTime _updateDate;
        private String _bankName;
        private String _bankAccountNumber;
        private Double _bankPayment;
        private String _insuranceName;
        private String _insuranceAccountNumber;
        private Double _insurancePayment;
        private ObservableCollection<FuelEvent> _fuelEvents;
        private ObservableCollection<ServiceEvent> _serviceEvents;
        private ObservableCollection<Reminder> _reminders;
        
        /// <summary>
        /// Car's ID
        /// </summary>
        public Guid ID
        {
            get { return this._id; }
            set
            {
                this._id = value;
                NotifyPropertyChanged("ID");
            }
        }
        /// <summary>
        /// Title of the Car
        /// </summary>
        public string Title
        {
            get { return this._title; }
            set
            {
                this._title = FileHelper.ReplaceSaveFileCharacters(value);
                
                NotifyPropertyChanged("Title");
            }
        }
        /// <summary>
        /// Year Car Was Made
        /// </summary>
        public string Year
        {
            get { return this._year; }
            set
            {
                this._year = FileHelper.ReplaceSaveFileCharacters(value);

                NotifyPropertyChanged("Year");
                NotifyPropertyChanged("YearMakeModel");
            }
        }
        /// <summary>
        /// Make of the car
        /// </summary>
        public string Make
        {
            get { return this._make; }
            set
            {
                this._make = FileHelper.ReplaceSaveFileCharacters(value);

                NotifyPropertyChanged("Make");
                NotifyPropertyChanged("YearMakeModel");
            }
        }
        /// <summary>
        /// Model of the car
        /// </summary>
        public string Model
        {
            get { return this._model; }
            set
            {
                this._model = FileHelper.ReplaceSaveFileCharacters(value);

                NotifyPropertyChanged("Model");
                NotifyPropertyChanged("YearMakeModel");
            }
        }
        /// <summary>
        /// VIN of the car
        /// </summary>
        public string VIN
        {
            get { return this._vin; }
            set
            {
                this._vin = FileHelper.ReplaceSaveFileCharacters(value);

                NotifyPropertyChanged("VIN");
            }
        }
        /// <summary>
        /// Starting Odometer of the Car
        /// </summary>
        public Double StartOdometer
        {
            get { return this._startOdometer; }
            set
            {
                try
                {
                    this._startOdometer = Double.Parse(value.ToString());                    
                }
                catch
                {
                    //this._odometer = 0;
                }

                NotifyPropertyChanged("Odometer");
            }
        }
        /// <summary>
        /// Current Odometer of the Car
        /// </summary>
        public Double CurrentOdometer
        {
            get { return this._currentOdometer; }
            set
            {
                try
                {
                    this._currentOdometer = Double.Parse(value.ToString());
                }
                catch
                {
                }

                NotifyPropertyChanged("CurrentOdometer");                
            }
        }
        /// <summary>
        /// Bank Name
        /// </summary>
        public String BankName
        {
            get { return this._bankName; }
            set
            {
                try
                {
                    this._bankName = FileHelper.ReplaceSaveFileCharacters(value);
                }
                catch
                {
                }

                NotifyPropertyChanged("BankName");
            }
        }
        /// <summary>
        /// Bank Account Number
        /// </summary>
        public String BankAccountNumber
        {
            get { return this._bankAccountNumber; }
            set
            {
                try
                {
                    this._bankAccountNumber = FileHelper.ReplaceSaveFileCharacters(value);
                }
                catch
                {
                }

                NotifyPropertyChanged("BankAccountNumber");
            }
        }
        /// <summary>
        /// Bank Payment (Used for recurring costs)
        /// </summary>
        public Double BankPayment
        {
            get { return this._bankPayment; }
            set
            {
                try
                {
                    this._bankPayment = Double.Parse(value.ToString());
                }
                catch
                {
                }

                NotifyPropertyChanged("BankPayment");
            }
        }
        /// <summary>
        /// Insurance Name
        /// </summary>
        public String InsuranceName
        {
            get { return this._insuranceName; }
            set
            {
                try
                {
                    this._insuranceName = FileHelper.ReplaceSaveFileCharacters(value);
                }
                catch
                {
                }

                NotifyPropertyChanged("InsuranceName");
            }
        }
        /// <summary>
        /// Insurance Account Number
        /// </summary>
        public String InsuranceAccountNumber
        {
            get { return this._insuranceAccountNumber; }
            set
            {
                try
                {
                    this._insuranceAccountNumber = FileHelper.ReplaceSaveFileCharacters(value);
                }
                catch
                {
                }

                NotifyPropertyChanged("InsuranceAccountNumber");
            }
        }
        /// <summary>
        /// Insurance Payment (Used for recurring costs)
        /// </summary>
        public Double InsurancePayment
        {
            get { return this._insurancePayment; }
            set
            {
                try
                {
                    this._insurancePayment = Double.Parse(value.ToString());
                }
                catch
                {
                }

                NotifyPropertyChanged("InsurancePayment");
            }
        }
        /// <summary>
        /// Year/Make/Model of the car
        /// </summary>
        public string YearMakeModel
        {
            get
            {
                string year = this.Year;
                string make = this.Make;
                string model = this.Model;
                StringBuilder yearMakeModel = new StringBuilder("");

                if (!string.IsNullOrEmpty(year))
                {
                    yearMakeModel.Append(year);
                    yearMakeModel.Append(" ");
                }

                if (!string.IsNullOrEmpty(make))
                {
                    yearMakeModel.Append(make);
                    yearMakeModel.Append(" ");
                }

                if (!string.IsNullOrEmpty(model))
                {
                    yearMakeModel.Append(model);
                }

                return yearMakeModel.ToString().Trim();
            }
        }
        /// <summary>
        /// Record status of the car. For future use of syncing.
        /// </summary>
        public RecordStatus RecordStatus
        {
            get
            {
                return this._recordStatus;
            }
            set
            {
                this._recordStatus = value;
                this.NotifyPropertyChanged("RecordStatus");
            }
        }
        /// <summary>
        /// Sync status of the car. For future use of syncing.
        /// </summary>
        public SyncStatus SyncStatus
        {
            get
            {
                return this._syncStatus;
            }
            set
            {
                this._syncStatus = value;
                this.NotifyPropertyChanged("SyncStatus");
            }
        }
        /// <summary>
        /// Last time this record was updates. For future use of syncing.
        /// </summary>
        public DateTime UpdateDate
        {
            get
            {
                return this._updateDate;
            }
            set
            {
                this._updateDate = value;
            }
        }
        /// <summary>
        /// Contains all the fuel events associated with this car.
        /// </summary>
        public ObservableCollection<FuelEvent> FuelEvents
        {
            get
            {
                return this._fuelEvents;
            }
            private set
            {
                this._fuelEvents = value;
            }
        }
        /// <summary>
        /// Contains all the service events associated with this car.
        /// </summary>
        public ObservableCollection<ServiceEvent> ServiceEvents
        {
            get
            {
                return this._serviceEvents;
            }
            set
            {
                this._serviceEvents = value;
            }
        }
        /// <summary>
        /// Contains all the reminders associated with this car.
        /// </summary>
        public ObservableCollection<Reminder> Reminders
        {
            get
            {
                return this._reminders;
            }
            set
            {
                this._reminders = value;
            }
        }

        /// <summary>
        /// The Average Distance per Quantity (Read Only)
        /// </summary>
        public Double AverageDistancePerQuantity
        {
            get
            {
                try
                {
                    Double _totalDistance = 0;
                    Double _totalQuantity = 0;
                    Double _distancePerQuantity = 0;

                    _totalDistance = this.FuelEvents.Where(f => f.IgnoreForStats == false).Sum(f => f.Distance);
                    _totalQuantity = this.FuelEvents.Where(f => f.IgnoreForStats == false).Sum(f => f.Quantity);
                    _distancePerQuantity = _totalDistance / _totalQuantity;

                    if (Double.IsNaN(_distancePerQuantity))
                    {
                        return 0;
                    }
                    else
                    {
                        return _distancePerQuantity;
                    }
                }
                catch
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// The Average Price Paid per Quantity (Read Only)
        /// </summary>
        public Double AveragePricePaidPerQuantity
        {
            get
            {
                try
                {
                    Double _totalPrice = 0;
                    Double _totalQuantity = 0;
                    Double _pricePaidPerQuantity = 0;

                    _totalPrice = this.FuelEvents.Where(f => f.IgnoreForStats == false).Sum(f => Math.Round(f.TotalCost, 2));
                    _totalQuantity = this.FuelEvents.Where(f => f.IgnoreForStats == false).Sum(f => f.Quantity);

                    _pricePaidPerQuantity = _totalPrice / _totalQuantity;

                    if (Double.IsNaN(_pricePaidPerQuantity))
                    {
                        return 0;
                    }
                    else
                    {
                        return _pricePaidPerQuantity;
                    }
                }
                catch
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// The Average Days Between Refueling (Read Only)
        /// </summary>
        public Double AverageDaysBetweenFillUp
        {
            get
            {
                if (this.FuelEvents.Count == 0)
                {
                    return 1;
                }

                try
                {
                    DateTime _startDate = this.FuelEvents.Where(f => f.IgnoreForStats == false).Min(f => f.DateTime);
                    DateTime _endDate = this.FuelEvents.Where(f => f.IgnoreForStats == false).Max(f => f.DateTime);
                    int _fillUps = this.FuelEvents.Where(f => f.IgnoreForStats == false).Count();

                    TimeSpan _difference = _endDate.Subtract(_startDate);

                    return Math.Round((Double)(_difference.Days / _fillUps), 0);
                }
                catch
                {
                    return 1;
                }
            }
        }

        /// <summary>
        /// The Average Distance Between Refueling (Read Only)
        /// </summary>
        public Double AverageDistancePerFillUp
        {
            get
            {
                if (this.FuelEvents.Count == 0)
                {
                    return 0;
                }

                try
                {
                    Double _totalDistance = 0;
                    Double _totalFillUps = 0;

                    _totalDistance = this.FuelEvents.Where(f => f.IgnoreForStats == false).Sum(f => f.Distance);
                    _totalFillUps = this.FuelEvents.Where(f => f.IgnoreForStats == false).Count();

                    return _totalDistance / _totalFillUps;
                }
                catch
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// The Total Cost for 1 Month of Owning Car
        /// </summary>
        public Double TotalCost1Month
        {
            get
            {
                try
                {
                    return this.TotalCostSince(DateTime.Now.AddMonths(-1), false);
                }
                catch
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// The Total Cost for 3 Months of Owning Car
        /// </summary>
        public Double TotalCost3Month
        {
            get
            {
                try
                {
                    return this.TotalCostSince(DateTime.Now.AddMonths(-3), false);
                }
                catch
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// The Total Cost for 6 Months of Owning Car
        /// </summary>
        public Double TotalCost6Month
        {
            get
            {
                try
                {
                    return this.TotalCostSince(DateTime.Now.AddMonths(-6), false);
                }
                catch
                {
                    return 0;
                }
            }
        }


        /// <summary>
        /// The Total Cost for 12 Months of Owning Car
        /// </summary>
        public Double TotalCost12Month
        {
            get
            {
                try
                {
                    return this.TotalCostSince(DateTime.Now.AddMonths(-12), false);
                }
                catch
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// The Total Cost for the current Month of Owning Car
        /// </summary>
        public Double TotalCostMonthToDate
        {
            get
            {
                try
                {
                    return this.TotalCostSince(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0, 0, DateTimeKind.Local), true);
                }
                catch
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// /// The Total Cost for current day of Owning Car
        /// </summary>
        public Double TotalCostYearToDate
        {
            get
            {
                try
                {
                    return this.TotalCostSince(new DateTime(DateTime.Now.Year, 1, 1, 0, 0, 0, 0, DateTimeKind.Local), true);
                }
                catch
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Generic constructor for Car.
        /// </summary>
        public Car()
        {
            this.ID = Guid.NewGuid();
            this.RecordStatus = DataModel.RecordStatus.Insert;
            this.SyncStatus = DataModel.SyncStatus.Unsynced;
            this.UpdateDate = DateTime.UtcNow;
            this.FuelEvents = new ObservableCollection<FuelEvent>();
            this.ServiceEvents = new ObservableCollection<ServiceEvent>();
            this.Reminders = new ObservableCollection<Reminder>();
        }

        /// <summary>
        /// Parameterized constructor for Car using all strings
        /// </summary>
        /// <param name="id">Car's ID</param>
        /// <param name="currentOdometer">The last updated odometer reading for the car</param>
        /// <param name="make">The make of the car</param>
        /// <param name="model">The model of the car</param>
        /// <param name="startOdometer">The starting odometer reading for the car</param>
        /// <param name="title">The title of the car</param>
        /// <param name="vin">The VIN of the car</param>
        /// <param name="year">The year of the car</param>
        /// <param name="recordStatus">The record status for this car</param>
        /// <param name="syncStatus">The sync status for this car</param>
        /// <param name="updateDate">The update date for this car</param>
        public Car(String id, String currentOdometer, String make, String model, String startOdometer, String title, String vin, String year, String bankName, String bankAccountNumber, String bankPayment, String insuranceName, String insuranceAccountNumber, String insurancePayment, String recordStatus, string syncStatus, string updateDate)
        {
            this.ID = Guid.Parse(id);
            this.StartOdometer = Double.Parse(startOdometer);
            this.CurrentOdometer = Double.Parse(currentOdometer);
            this.Make = make;
            this.Model = model;
            this.Title = title;
            this.VIN = vin;
            this.Year = year;
            this.BankName = bankName;
            this.BankAccountNumber = bankAccountNumber;
            this.BankPayment = Double.Parse(bankPayment);
            this.InsuranceName = insuranceName;
            this.InsuranceAccountNumber = insuranceAccountNumber;
            this.InsurancePayment = Double.Parse(insurancePayment);
            this.RecordStatus = (DataModel.RecordStatus)(Enum.Parse(RecordStatus.GetType(), recordStatus));
            this.SyncStatus = (DataModel.SyncStatus)(Enum.Parse(SyncStatus.GetType(), syncStatus));
            this.UpdateDate = DateTime.Parse(updateDate);
            this.FuelEvents = new ObservableCollection<FuelEvent>();
            this.ServiceEvents = new ObservableCollection<ServiceEvent>();
            this.Reminders = new ObservableCollection<Reminder>();
        }

        /// <summary>
        /// Parameterized constructor for Car using actual types
        /// </summary>
        /// <param name="id">Car's ID</param>
        /// <param name="currentOdometer">The last updated odometer reading for the car</param>
        /// <param name="make">The make of the car</param>
        /// <param name="model">The model of the car</param>
        /// <param name="startOdometer">The starting odometer reading for the car</param>
        /// <param name="title">The title of the car</param>
        /// <param name="vin">The VIN of the car</param>
        /// <param name="year">The year of the car</param>
        /// <param name="recordStatus">The record status for this car</param>
        /// <param name="syncStatus">The sync status for this car</param>
        /// <param name="updateDate">The update date for this car</param>
        public Car(Guid id, Double currentOdometer, String make, String model, Double startOdometer, String title, String vin, String year, String bankName, String bankAccountNumber, Double bankPayment, String insuranceName, String insuranceAccountNumber, Double insurancePayment, RecordStatus recordStatus, SyncStatus syncStatus, DateTime updateDate)
        {
            this.ID = id;
            this.StartOdometer = startOdometer;
            this.CurrentOdometer = currentOdometer;
            this.Make = make;
            this.Model = model;
            this.Title = title;
            this.VIN = vin;
            this.Year = year;
            this.BankName = bankName;
            this.BankAccountNumber = bankAccountNumber;
            this.BankPayment = bankPayment;
            this.InsuranceName = insuranceName;
            this.InsuranceAccountNumber = insuranceAccountNumber;
            this.InsurancePayment = insurancePayment;
            this.RecordStatus = recordStatus;
            this.SyncStatus = syncStatus;
            this.UpdateDate = updateDate;
            this.FuelEvents = new ObservableCollection<FuelEvent>();
            this.ServiceEvents = new ObservableCollection<ServiceEvent>();
            this.Reminders = new ObservableCollection<Reminder>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Used to provide a string to write to a save file
        /// </summary>
        /// <returns>String containing the save data for this car.</returns>
        public String ToSaveString()
        {
            StringBuilder sbSaveString = new StringBuilder();

            sbSaveString.Append("CAR`");
            sbSaveString.Append(this._id);
            sbSaveString.Append("`");
            sbSaveString.Append(this._currentOdometer);
            sbSaveString.Append("`");
            sbSaveString.Append(this._make);
            sbSaveString.Append("`");
            sbSaveString.Append(this._model);
            sbSaveString.Append("`");
            sbSaveString.Append(this._startOdometer);
            sbSaveString.Append("`");
            sbSaveString.Append(this._title);
            sbSaveString.Append("`");
            sbSaveString.Append(this._vin);
            sbSaveString.Append("`");
            sbSaveString.Append(this._year);
            sbSaveString.Append("`");
            sbSaveString.Append(this._bankName);
            sbSaveString.Append("`");
            sbSaveString.Append(this._bankAccountNumber);
            sbSaveString.Append("`");
            sbSaveString.Append(this._bankPayment);
            sbSaveString.Append("`");
            sbSaveString.Append(this._insuranceName);
            sbSaveString.Append("`");
            sbSaveString.Append(this._insuranceAccountNumber);
            sbSaveString.Append("`");
            sbSaveString.Append(this._insurancePayment);
            sbSaveString.Append("`");
            sbSaveString.Append(this._recordStatus);
            sbSaveString.Append("`");
            sbSaveString.Append(this._syncStatus);
            sbSaveString.Append("`");
            sbSaveString.Append(this._updateDate.ToString("yyyy-MM-dd hh:mm:ss"));
            sbSaveString.Append("~");
            
            return sbSaveString.ToString();
        }

        /// <summary>
        /// Used to save a deleted car
        /// </summary>
        /// <returns>String containing the deleted car string</returns>
        public String ToDeletedSaveString()
        {
            StringBuilder sbSaveString = new StringBuilder();

            sbSaveString.Append("DELETEDCAR`");
            sbSaveString.Append(this._id);
            sbSaveString.Append("`");
            sbSaveString.Append(this._currentOdometer);
            sbSaveString.Append("`");
            sbSaveString.Append(this._make);
            sbSaveString.Append("`");
            sbSaveString.Append(this._model);
            sbSaveString.Append("`");
            sbSaveString.Append(this._startOdometer);
            sbSaveString.Append("`");
            sbSaveString.Append(this._title);
            sbSaveString.Append("`");
            sbSaveString.Append(this._vin);
            sbSaveString.Append("`");
            sbSaveString.Append(this._year);
            sbSaveString.Append("`");
            sbSaveString.Append(this._bankName);
            sbSaveString.Append("`");
            sbSaveString.Append(this._bankAccountNumber);
            sbSaveString.Append("`");
            sbSaveString.Append(this._bankPayment);
            sbSaveString.Append("`");
            sbSaveString.Append(this._insuranceName);
            sbSaveString.Append("`");
            sbSaveString.Append(this._insuranceAccountNumber);
            sbSaveString.Append("`");
            sbSaveString.Append(this._insurancePayment);
            sbSaveString.Append("`");
            sbSaveString.Append(this._recordStatus);
            sbSaveString.Append("`");
            sbSaveString.Append(this._syncStatus);
            sbSaveString.Append("`");
            sbSaveString.Append(this._updateDate.ToString("yyyy-MM-dd hh:mm:ss"));
            sbSaveString.Append("~");

            return sbSaveString.ToString();
        }

        /// <summary>
        /// Sets the odometer for the car based on the start odometer, all fuel events, and all service events
        /// </summary>
        public void SetOdometer()
        {
            try
            {
                Double maxOdometer = 0;
                Double maxFuelEventOdometer = 0;
                Double maxServiceEventOdometer = 0;

                maxOdometer = this.StartOdometer;

                maxFuelEventOdometer = this.FuelEvents.Max(f => f.Odometer);
                maxServiceEventOdometer = this.ServiceEvents.Max(s => s.Odometer);

                if (maxOdometer < maxFuelEventOdometer)
                {
                    maxOdometer = maxFuelEventOdometer;
                }

                if (maxOdometer < maxServiceEventOdometer)
                {
                    maxOdometer = maxServiceEventOdometer;
                }

                this.CurrentOdometer = maxOdometer;
            }
            catch
            {
            }

            NotifyPropertyChanged("AverageDistancePerQuantity");
            NotifyPropertyChanged("AverageDistancePerFillUp");
            NotifyPropertyChanged("AveragePricePaidPerQuantity");
            NotifyPropertyChanged("AverageDaysBetweenFillUp");
            NotifyPropertyChanged("TotalCost1Month");
            NotifyPropertyChanged("TotalCost3Month");
            NotifyPropertyChanged("TotalCost6Month");
            NotifyPropertyChanged("TotalCost12Month");
            NotifyPropertyChanged("TotalCostMonthToDate");
            NotifyPropertyChanged("TotalCostYearToDate");
        }

        /// <summary>
        /// Updates the distances of all fuel events based on the Odometer for the fuel event and the one previous to it.
        /// </summary>
        public void UpdateFuelEventDistance()
        {
            try
            {
                System.Collections.Generic.List<FuelEvent> fuelEvents = new System.Collections.Generic.List<FuelEvent>();

                foreach (FuelEvent fuelEvent in this.FuelEvents)
                {
                    fuelEvents.Add(fuelEvent);
                }

                fuelEvents.Sort();

                for (int i = 0; i < fuelEvents.Count; i++)
                {
                    if (i == 0)
                    {
                        fuelEvents[i].Distance = fuelEvents[i].Odometer - this.StartOdometer;
                    }
                    else
                    {
                        fuelEvents[i].Distance = fuelEvents[i].Odometer - fuelEvents[i - 1].Odometer;
                    }
                }
            }
            catch
            {
            }

            NotifyPropertyChanged("AverageDistancePerQuantity");
            NotifyPropertyChanged("AverageDistancePerFillUp");
            NotifyPropertyChanged("AveragePricePaidPerQuantity");
            NotifyPropertyChanged("AverageDaysBetweenFillUp");
            NotifyPropertyChanged("TotalCost1Month");
            NotifyPropertyChanged("TotalCost3Month");
            NotifyPropertyChanged("TotalCost6Month");
            NotifyPropertyChanged("TotalCost12Month");
            NotifyPropertyChanged("TotalCostMonthToDate");
            NotifyPropertyChanged("TotalCostYearToDate");
        }

        /// <summary>
        /// Calculates the total cost of owning the vehicle over a set time frame
        /// </summary>
        /// <param name="start_date">The date to start from</param>
        /// <param name="to_date">The date to end at</param>
        /// <returns>The total cost for owning the vehicle over the time frame provided</returns>
        private Double TotalCostSince(DateTime start_date, Boolean to_date)
        {
            Double total_cost = 0;            
            DateTime end_date = DateTime.Now;
            int months = 0;

            start_date = new DateTime(start_date.Year, start_date.Month, 1, 0, 0, 0, 0, DateTimeKind.Local);

            if (!to_date)
            {
                end_date = new DateTime(end_date.Year, end_date.Month, 1, 0, 0, 0, 0, DateTimeKind.Local);
            }
            else
            {
                end_date = new DateTime(end_date.Year, end_date.Month, end_date.Day + 1, 0, 0, 0, 0, DateTimeKind.Local);
            }

            months = ((end_date.Year - start_date.Year) * 12) + (end_date.Month - start_date.Month) + (to_date ? 1 : 0);

            total_cost = Math.Round(this.BankPayment, 2) * months + Math.Round(this.InsurancePayment, 2) * months;
            total_cost += this.FuelEvents.Where(f => f.DateTime >= start_date).Where(f => f.DateTime < end_date).Sum(f => Math.Round(f.TotalCost, 2));
            total_cost += this.ServiceEvents.Where(s => s.DateTime >= start_date).Where(s => s.DateTime < end_date).Sum(s => Math.Round(s.Cost, 2));

            return total_cost;
        }
    }
}
