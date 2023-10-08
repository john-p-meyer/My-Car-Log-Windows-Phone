using System;
using System.Text;
using CarLog.Core.Helpers;

namespace CarLog.Core.DataModel
{
    /// <summary>
    /// Contains Service Event information
    /// </summary>
    public class ServiceEvent : CommonEvent
    {
        private ServiceEventType _serviceEventType;
        private ServiceEventDisplay _serviceEventDisplay = new ServiceEventDisplay();
        private Double _cost;

        /// <summary>
        /// The type of service event
        /// </summary>
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

        /// <summary>
        /// The Cost of the service event
        /// </summary>
        public Double Cost
        {
            get { return this._cost; }
            set
            {
                try
                {
                    this._cost = Double.Parse(value.ToString());
                    this.NotifyPropertyChanged("Cost");
                }
                catch
                {
                }
            }
        }        

        /// <summary>
        /// The text associated with type of service event (Read Only)
        /// </summary>
        public String ServiceEventDisplay
        {
            get { return this._serviceEventDisplay.ServiceEventText; }
        }

        /// <summary>
        /// The Uri of the image associated with type of service event (Read Only)
        /// </summary>
        public Uri ServiceEventImagePath
        {
            get { return this._serviceEventDisplay.ServiceEventImagePath; }
        }

        /// <summary>
        /// Generic Constructor
        /// </summary>
        public ServiceEvent()
            : base()
        {
        }

        /// <summary>
        /// Constructor that takes in correct types for all values
        /// </summary>
        /// <param name="id">Guid of the service event</param>
        /// <param name="carID">Guid of the car for the service event</param>
        /// <param name="cost">The cost of the service event</param>
        /// <param name="dateTime">The date/time the service event took place</param>
        /// <param name="location">The location where the service event took place</param>
        /// <param name="note">The note associated with the service event</param>
        /// <param name="odometer">The odometer reading when the service event took place</param>
        /// <param name="serviceEventType">The type of the service event</param>
        /// <param name="title">The title of the service event</param>
        /// <param name="recordStatus">The record status of the service event</param>
        /// <param name="syncStatus">The sync status of the service event</param>
        /// <param name="updateDate">The update date of the service event</param>
        public ServiceEvent(Guid id, Guid carID, Double cost, DateTime dateTime, String location, String note, Double odometer, ServiceEventType serviceEventType, String title, RecordStatus recordStatus, SyncStatus syncStatus, DateTime updateDate)
        {
            this.ID = id;
            this.CarID = carID;
            this.Cost = cost;
            this.DateTime = dateTime;
            this.Location = location;
            this.Note = note;
            this.Odometer = odometer;
            this.ServiceEventType = serviceEventType;
            this.Title = title;
            this.RecordStatus = recordStatus;
            this.SyncStatus = syncStatus;
            this.UpdateDate = updateDate;
        }

        /// <summary>
        /// Constructor that takes in strings for all values
        /// </summary>
        /// <param name="id">Guid of the service event</param>
        /// <param name="carID">Guid of the car for the service event</param>
        /// <param name="cost">The cost of the service event</param>
        /// <param name="dateTime">The date/time the service event took place</param>
        /// <param name="location">The location where the service event took place</param>
        /// <param name="note">The note associated with the service event</param>
        /// <param name="odometer">The odometer reading when the service event took place</param>
        /// <param name="serviceEventType">The type of the service event</param>
        /// <param name="title">The title of the service event</param>
        /// <param name="recordStatus">The record status of the service event</param>
        /// <param name="syncStatus">The sync status of the service event</param>
        /// <param name="updateDate">The update date of the service event</param>
        public ServiceEvent(String id, String carID, String cost, String dateTime, String location, String note, String odometer, String serviceEventType, String title, String recordStatus, String syncStatus, String updateDate)
        {
            try
            {
                this.ID = Guid.Parse(id);
                this.CarID = Guid.Parse(carID);
                this.Cost = Double.Parse(cost);
                this.DateTime = DateTime.Parse(dateTime);
                this.Location = location;
                this.Note = note;
                this.Odometer = Double.Parse(odometer);
                this.ServiceEventType = (ServiceEventType)(Enum.Parse(ServiceEventType.GetType(), serviceEventType));
                this.Title = title;
                this.RecordStatus = (DataModel.RecordStatus)(Enum.Parse(RecordStatus.GetType(), recordStatus));
                this.SyncStatus = (DataModel.SyncStatus)(Enum.Parse(SyncStatus.GetType(), syncStatus));
                this.UpdateDate = DateTime.Parse(updateDate);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Creates a string in the correct format for saving to a file
        /// </summary>
        /// <returns>String in the correct format for saving to a file</returns>
        public String ToSaveString()
        {
            StringBuilder sbSaveString = new StringBuilder();

            sbSaveString.Append("SERVICEEVENT`");
            sbSaveString.Append(this.ID);
            sbSaveString.Append("`");
            sbSaveString.Append(this.CarID);
            sbSaveString.Append("`");
            sbSaveString.Append(this._cost);
            sbSaveString.Append("`");
            sbSaveString.Append(this.DateTime.ToString("yyyy-MM-dd hh:mm:ss"));
            sbSaveString.Append("`");
            sbSaveString.Append(this.Location);
            sbSaveString.Append("`");
            sbSaveString.Append(this.Note);
            sbSaveString.Append("`");
            sbSaveString.Append(this.Odometer);
            sbSaveString.Append("`");
            sbSaveString.Append(this._serviceEventType);
            sbSaveString.Append("`");
            sbSaveString.Append(this.Title);
            sbSaveString.Append("`");
            sbSaveString.Append(this.RecordStatus);
            sbSaveString.Append("`");
            sbSaveString.Append(this.SyncStatus);
            sbSaveString.Append("`");
            sbSaveString.Append(this.UpdateDate.ToString("yyyy-MM-dd hh:mm:ss"));
            sbSaveString.Append("~");

            return sbSaveString.ToString();
        }

        /// <summary>
        /// Creates a deleted string in the correct format for saving to a file
        /// </summary>
        /// <returns>String in the correct format for saving to a file a deleted service event</returns>
        public String ToDeletedSaveString()
        {
            StringBuilder sbSaveString = new StringBuilder();

            sbSaveString.Append("DELETEDSERVICEEVENT`");
            sbSaveString.Append(this.ID);
            sbSaveString.Append("`");
            sbSaveString.Append(this.CarID);
            sbSaveString.Append("`");
            sbSaveString.Append(this._cost);
            sbSaveString.Append("`");
            sbSaveString.Append(this.DateTime.ToString("yyyy-MM-dd hh:mm:ss"));
            sbSaveString.Append("`");
            sbSaveString.Append(this.Location);
            sbSaveString.Append("`");
            sbSaveString.Append(this.Note);
            sbSaveString.Append("`");
            sbSaveString.Append(this.Odometer);
            sbSaveString.Append("`");
            sbSaveString.Append(this._serviceEventType);
            sbSaveString.Append("`");
            sbSaveString.Append(this.Title);
            sbSaveString.Append("`");
            sbSaveString.Append(this.RecordStatus);
            sbSaveString.Append("`");
            sbSaveString.Append(this.SyncStatus);
            sbSaveString.Append("`");
            sbSaveString.Append(this.UpdateDate.ToString("yyyy-MM-dd hh:mm:ss"));
            sbSaveString.Append("~");

            return sbSaveString.ToString();
        }
    }    
}
