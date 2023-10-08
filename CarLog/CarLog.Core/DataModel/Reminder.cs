using System;
using System.ComponentModel;
using System.Text;
using CarLog.Core.Helpers;
using CarLog.Core.Resources;

namespace CarLog.Core.DataModel
{
    public class Reminder : CommonEvent
    {
        private ServiceEventType _serviceEventType;
        private ServiceEventDisplay _serviceEventDisplay = new ServiceEventDisplay();
        private ReminderType _reminderType;
        private ReminderTypeDisplay _reminderTypeDisplay = new ReminderTypeDisplay();
        private int _months;
        private Double _distance;
        private DateTime _endDate;
        private Double _endOdometer;
        private Boolean _isRecurring;
        private DateTime _reminderDateTime;

        public ServiceEventType ServiceEventType
        {
            get { return this._serviceEventType; }
            set
            {
                this._serviceEventType = value;
                this._serviceEventDisplay.ServiceEventType = this._serviceEventType;
                this.NotifyPropertyChanged("ServiceEventType");
                this.NotifyPropertyChanged("ServiceEventDisplay");
                this.NotifyPropertyChanged("ServiceEventImagePath");
            }
        }
        public String ServiceEventDisplay
        {
            get { return this._serviceEventDisplay.ServiceEventText; }
        }
        public Uri ServiceEventImagePath
        {
            get { return this._serviceEventDisplay.ServiceEventImagePath; }
        }
        public ReminderType ReminderType
        {
            get { return this._reminderType; }
            set
            {
                this._reminderType = value;
                this._reminderTypeDisplay.ReminderType = this._reminderType;
                SetReminderValuesByType();
                this.NotifyPropertyChanged("ReminderType");
                this.NotifyPropertyChanged("ReminderEventDisplay");
                this.NotifyPropertyChanged("ReminderDateTime");
                this.NotifyPropertyChanged("ReminderDueStatus");
            }
        }
        public String ReminderTypeDisplay
        {
            get { return this._reminderTypeDisplay.ReminderTypeText; }
        }
        public int Months
        {
            get { return this._months; }
            set
            {
                try
                {
                    this._months = value;
                    SetReminderValuesByType();
                    this.NotifyPropertyChanged("Months");
                    this.NotifyPropertyChanged("ReminderDateTime");
                    this.NotifyPropertyChanged("ReminderDueStatus");
                }
                catch
                {
                }
            }
        }
        public Double Distance
        {
            get { return this._distance; }
            set
            {
                try
                {
                    this._distance = value;
                    SetReminderValuesByType();
                    this.NotifyPropertyChanged("Distance");
                    this.NotifyPropertyChanged("ReminderDateTime");
                    this.NotifyPropertyChanged("ReminderDueStatus");
                }
                catch
                {
                }
            }
        }
        public DateTime EndDate
        {
            get { return this._endDate; }
            set
            {
                try
                {
                    this._endDate = value;
                    NotifyPropertyChanged("EndDate");
                    this.NotifyPropertyChanged("ReminderDateTime");
                    this.NotifyPropertyChanged("ReminderDueStatus");
                }
                catch
                {
                }
            }
        }
        public Double EndOdometer
        {
            get { return this._endOdometer; }
            set
            {
                try
                {
                    this._endOdometer = value;                    
                    this.NotifyPropertyChanged("EndOdometer");                    
                    this.NotifyPropertyChanged("ReminderDueStatus");
                }
                catch
                {
                }
            }
        }
        public Boolean IsRecurring
        {
            get { return this._isRecurring; }
            set
            {
                try
                {
                    this._isRecurring = value;
                    NotifyPropertyChanged("IsRecurring");
                }
                catch
                {
                }
            }
        }
        public DateTime ReminderDateTime
        {
            get
            {
                return this._reminderDateTime;                
            }
            set
            {
                try
                {
                    this._reminderDateTime = value;                    
                    this.NotifyPropertyChanged("ReminderDateTime");                    
                }
                catch
                {
                }
            }
        }
        public String ReminderDueStatus
        {
            get
            {
                try
                {
                    if (this.ReminderType == DataModel.ReminderType.Date)
                    {
                        return "";
                    }
                    else if (this.ReminderType == DataModel.ReminderType.DistanceSince)
                    {
                        return this.Distance.ToString("N0") + " " + new LocalizedVariableSettingsStrings().DistanceShort;//AppResources.MilesShort;
                    }
                    else if (this.ReminderType == DataModel.ReminderType.MonthsSince)
                    {
                        return this.Months.ToString() + " " + AppResources.MonthsShort;
                    }
                    else if (this.ReminderType == DataModel.ReminderType.Odometer)
                    {
                        return this.EndOdometer.ToString("N0") + " " + new LocalizedVariableSettingsStrings().DistanceShort;
                    }
                    else if (this.ReminderType == DataModel.ReminderType.MonthsOrDistanceSince)
                    {
                        return this.Months.ToString() + " " + AppResources.MonthsShort + "/" + this.Distance.ToString("N0") + " " + new LocalizedVariableSettingsStrings().DistanceShort;
                    }
                    else
                    {
                        return "";
                    }
                }
                catch
                {
                    return "";
                }
            }
        }

        public Reminder()
            : base()
        {
            this.Months = 0;
            this.Distance = 0;
            this.EndDate = DateTime.Now;
            this.EndOdometer = 0;
            this.ServiceEventType = DataModel.ServiceEventType.None;
            this.ReminderType = DataModel.ReminderType.Date;
        }

        public Reminder(Guid id, Guid carID, DateTime dateTime, String note, Double odometer, String title, ServiceEventType serviceEventType, ReminderType reminderType, int months, Double distance, DateTime endDate, Double endOdometer, Boolean isRecurring, RecordStatus recordStatus, SyncStatus syncStatus, DateTime updateDate)
        {
            this.ID = id;
            this.CarID = carID;
            this.DateTime = dateTime;
            this.Note = note;
            this.Odometer = odometer;
            this.Title = title;
            this.ServiceEventType = serviceEventType;
            this.ReminderType = reminderType;
            this.Months = months;
            this.Distance = distance;
            this.EndDate = endDate;
            this.EndOdometer = endOdometer;
            this.IsRecurring = isRecurring;
            this.RecordStatus = recordStatus;
            this.SyncStatus = syncStatus;
            this.UpdateDate = updateDate;
        }

        public Reminder(String id, String carID, String dateTime, String note, String odometer, String title, String serviceEventType, String reminderType, String months, String distance, String endDate, String endOdometer, String isRecurring, String recordStatus, String syncStatus, String updateDate)
        {
            try
            {
                this.ID = Guid.Parse(id);
                this.CarID = Guid.Parse(carID);
                this.DateTime = DateTime.Parse(dateTime);
                this.Note = note;
                this.Odometer = Double.Parse(odometer);
                this.Title = title;
                this.ServiceEventType = (ServiceEventType)(Enum.Parse(ServiceEventType.GetType(), serviceEventType));
                this.ReminderType = (ReminderType)(Enum.Parse(ReminderType.GetType(), reminderType));
                this.Months = int.Parse(months);
                this.Distance = Double.Parse(distance);
                this.EndDate = DateTime.Parse(endDate);
                this.EndOdometer = Double.Parse(endOdometer);
                this.IsRecurring = Boolean.Parse(isRecurring);
                this.RecordStatus = (DataModel.RecordStatus)(Enum.Parse(RecordStatus.GetType(), recordStatus));
                this.SyncStatus = (DataModel.SyncStatus)(Enum.Parse(SyncStatus.GetType(), syncStatus));
                this.UpdateDate = DateTime.Parse(updateDate);
            }
            catch
            {
            }
        }

        public String ToSaveString()
        {
            StringBuilder sbSaveString = new StringBuilder();

            sbSaveString.Append("REMINDER`");
            sbSaveString.Append(this.ID);
            sbSaveString.Append("`");
            sbSaveString.Append(this.CarID);
            sbSaveString.Append("`");
            sbSaveString.Append(this.DateTime.ToString("yyyy-MM-dd hh:mm:ss"));
            sbSaveString.Append("`");
            sbSaveString.Append(this.Note);
            sbSaveString.Append("`");
            sbSaveString.Append(this.Odometer);
            sbSaveString.Append("`");
            sbSaveString.Append(this.Title);
            sbSaveString.Append("`");
            sbSaveString.Append(this._serviceEventType);
            sbSaveString.Append("`");
            sbSaveString.Append(this._reminderType);
            sbSaveString.Append("`");
            sbSaveString.Append(this._months);
            sbSaveString.Append("`");
            sbSaveString.Append(this._distance);
            sbSaveString.Append("`");
            sbSaveString.Append(this._endDate.ToString("yyyy-MM-dd hh:mm:ss"));
            sbSaveString.Append("`");
            sbSaveString.Append(this._endOdometer);
            sbSaveString.Append("`");
            sbSaveString.Append(this._isRecurring);
            sbSaveString.Append("`");
            sbSaveString.Append(this.RecordStatus);
            sbSaveString.Append("`");
            sbSaveString.Append(this.SyncStatus);
            sbSaveString.Append("`");
            sbSaveString.Append(this.UpdateDate.ToString("yyyy-MM-dd hh:mm:ss"));
            sbSaveString.Append("~");

            return sbSaveString.ToString();
        }

        public String ToDeletedSaveString()
        {
            StringBuilder sbSaveString = new StringBuilder();

            sbSaveString.Append("DELETEDREMINDER`");
            sbSaveString.Append(this.ID);
            sbSaveString.Append("`");
            sbSaveString.Append(this.CarID);
            sbSaveString.Append("`");
            sbSaveString.Append(this.DateTime.ToString("yyyy-MM-dd hh:mm:ss"));
            sbSaveString.Append("`");
            sbSaveString.Append(this.Note);
            sbSaveString.Append("`");
            sbSaveString.Append(this.Odometer);
            sbSaveString.Append("`");
            sbSaveString.Append(this.Title);
            sbSaveString.Append("`");
            sbSaveString.Append(this._serviceEventType);
            sbSaveString.Append("`");
            sbSaveString.Append(this._reminderType);
            sbSaveString.Append("`");
            sbSaveString.Append(this._months);
            sbSaveString.Append("`");
            sbSaveString.Append(this._distance);
            sbSaveString.Append("`");
            sbSaveString.Append(this._endDate.ToString("yyyy-MM-dd hh:mm:ss"));
            sbSaveString.Append("`");
            sbSaveString.Append(this._endOdometer);
            sbSaveString.Append("`");
            sbSaveString.Append(this._isRecurring);
            sbSaveString.Append("`");
            sbSaveString.Append(this.RecordStatus);
            sbSaveString.Append("`");
            sbSaveString.Append(this.SyncStatus);
            sbSaveString.Append("`");
            sbSaveString.Append(this.UpdateDate.ToString("yyyy-MM-dd hh:mm:ss"));
            sbSaveString.Append("~");

            return sbSaveString.ToString();
        }

        private void SetReminderValuesByType()
        {
            switch (this.ReminderType)
            {
                case ReminderType.Date:

                    break;

                case ReminderType.DistanceSince:
                    try
                    {
                        this.EndOdometer = this.Odometer + this.Distance;
                    }
                    catch
                    {                        
                    }

                    break;

                case ReminderType.MonthsSince:
                    try
                    {
                        this.EndDate = this.DateTime.AddMonths(this.Months);
                    }
                    catch
                    {

                    }

                    break;

                case ReminderType.Odometer:
                    try
                    {

                    }
                    catch
                    {

                    }

                    break;

                case ReminderType.MonthsOrDistanceSince:
                    try
                    {
                        this.EndOdometer = this.Odometer + this.Distance;
                        this.EndDate = this.DateTime.AddMonths(this.Months);
                    }
                    catch
                    {

                    }

                    break;

                default:
                    break;
            }
        }
    }

    public class ReminderTypeDisplay : INotifyPropertyChanged
    {
        private ReminderType _reminderType;

        public ReminderType ReminderType
        {
            get
            {
                return this._reminderType;
            }
            set
            {
                this._reminderType = value;
                this.NotifyPropertyChanged("ReminderType");
                this.NotifyPropertyChanged("ReminderTypeText");
            }
        }

        public String ReminderTypeText
        {
            get
            {
                try
                {
                    switch (this._reminderType)
                    {
                        case ReminderType.Date:
                            return AppResources.DateDisplayName;

                        case ReminderType.MonthsSince:
                            return AppResources.MonthsSinceDisplayName;

                        case ReminderType.Odometer:
                            return AppResources.OdometerDisplayName;

                        case ReminderType.DistanceSince:
                            return new LocalizedVariableSettingsStrings().DistanceSinceLast;

                        case ReminderType.MonthsOrDistanceSince:
                            return new LocalizedVariableSettingsStrings().MonthsOrDistanceSinceLast;

                        default:
                            return AppResources.DateDisplayName;
                    }
                }
                catch
                {
                    return AppResources.NoneDisplayName;
                }
            }
        }

        public ReminderTypeDisplay() : this(ReminderType.Date) { }

        public ReminderTypeDisplay(ReminderType reminderType)
        {
            this.ReminderType = reminderType;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public enum ReminderType
    {
        Date,
        MonthsSince,
        Odometer,
        DistanceSince,
        MonthsOrDistanceSince
    }
}
