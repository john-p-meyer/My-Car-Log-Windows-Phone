using System;
using System.ComponentModel;
using CarLog.Core.Helpers;

namespace CarLog.Core.DataModel
{
    public class CommonEvent : INotifyPropertyChanged, IComparable<CommonEvent>
    {
        private Guid _id;
        private Guid _carID;
        private string _carTitle;
        private DateTime _dateTime;
        private String _title;
        private String _note;
        private Double _odometer;
        private String _location;
        private RecordStatus _recordStatus;
        private SyncStatus _syncStatus;
        private DateTime _updateDate;

        /// <summary>
        /// The Guid for this event
        /// </summary>
        public Guid ID
        {
            get { return this._id; }
            set
            {
                this._id = value;
                this.NotifyPropertyChanged("UniqueID");
            }
        }

        /// <summary>
        /// The Guid of the Car for this event
        /// </summary>
        public Guid CarID
        {
            get { return this._carID; }
            set
            {
                this._carID = value;
            }
        }

        /// <summary>
        /// The Date/Time the event occurred
        /// </summary>
        public DateTime DateTime
        {
            get { return this._dateTime; }
            set
            {
                DateTime tempDate;

                try
                {
                    if (DateTime.TryParse(value.ToShortDateString(), out tempDate))
                    {
                        this._dateTime = tempDate;
                        this.NotifyPropertyChanged("DateTime");
                        this.NotifyPropertyChanged("Title");
                    }
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// The title of the event (defaults to the date)
        /// </summary>
        public String Title
        {
            get
            {
                if (string.IsNullOrEmpty(this._title) || string.IsNullOrWhiteSpace(this._title))
                {
                    return this.DateTime.ToString("MMMM dd, yyyy");
                }
                else
                {
                    return this._title;
                }
            }
            set
            {
                this._title = FileHelper.ReplaceSaveFileCharacters(value);
                this.NotifyPropertyChanged("Title");
            }
        }

        /// <summary>
        /// The note for the event
        /// </summary>
        public String Note
        {
            get { return this._note; }
            set
            {
                this._note = FileHelper.ReplaceSaveFileCharacters(value);
                this.NotifyPropertyChanged("Note");
            }
        }

        /// <summary>
        /// The odometer of the event
        /// </summary>
        public Double Odometer
        {
            get { return this._odometer; }
            set
            {
                try
                {
                    this._odometer = Double.Parse(value.ToString());
                    this.NotifyPropertyChanged("Odometer");
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// The Location of the service event
        /// </summary>
        public String Location
        {
            get { return this._location; }
            set
            {
                this._location = FileHelper.ReplaceSaveFileCharacters(value);
                this.NotifyPropertyChanged("Location");
            }
        }
        
        /// <summary>
        /// The record status for the event
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
        /// The sync status for the event
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
        /// The update date for the event
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
        /// The title on the car (set outside of class)
        /// </summary>
        public String CarTitle
        {
            get
            {
                return this._carTitle;
            }
            set
            {
                this._carTitle = value;
                this.NotifyPropertyChanged("CarTitle");
            }
        }

        /// <summary>
        /// Generic Constructor for the event
        /// </summary>
        public CommonEvent()
        {
            this.ID = Guid.NewGuid();
            this.DateTime = DateTime.Now;
            this.Note = "";
            this.RecordStatus = DataModel.RecordStatus.Insert;
            this.SyncStatus = DataModel.SyncStatus.Unsynced;
            this.UpdateDate = DateTime.UtcNow;
        }

        /// <summary>
        /// Constructor that takes date/time and odometer
        /// </summary>
        /// <param name="dateTime">The date/time the event occurred</param>
        /// <param name="odometer">The odometer when the event occurred</param>
        public CommonEvent(DateTime dateTime, Double odometer)
        {
            this.ID = Guid.NewGuid();
            this.DateTime = dateTime;
            this.Odometer = odometer;
            this.Note = "";
            this.RecordStatus = DataModel.RecordStatus.Insert;
            this.SyncStatus = DataModel.SyncStatus.Unsynced;
            this.UpdateDate = DateTime.UtcNow;
        }

        /// <summary>
        /// Constructor that takes the Guid of the event in string form
        /// </summary>
        /// <param name="id">The Guid of the event in string form</param>
        public CommonEvent(String id)
        {
            this.ID = Guid.Parse(id);
            this.Note = "";
            this.RecordStatus = DataModel.RecordStatus.Insert;
            this.SyncStatus = DataModel.SyncStatus.Unsynced;
            this.UpdateDate = DateTime.UtcNow;
        }

        /// <summary>
        /// Constructor that takes the Guid of the event in Guid form
        /// </summary>
        /// <param name="id">The Guid of the event in Guid form</param>
        public CommonEvent(Guid id)
        {
            this.ID = id;
            this.Note = "";
            this.RecordStatus = DataModel.RecordStatus.Insert;
            this.SyncStatus = DataModel.SyncStatus.Unsynced;
            this.UpdateDate = DateTime.UtcNow;
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

        /// <summary>
        /// Allows for comparing to events together based on the date/time and then the odometer
        /// </summary>
        /// <param name="other">The event to compare to</param>
        /// <returns>The value of the compare</returns>
        public int CompareTo(CommonEvent other)
        {
            // If other is not a valid object reference, this instance is greater. 
            if (other == null) return 1;

            if (DateTime != other.DateTime)
            {
                return DateTime.CompareTo(other.DateTime);
            }
            else
            {
                return Odometer.CompareTo(other.Odometer);
            }
        }

    }
}
