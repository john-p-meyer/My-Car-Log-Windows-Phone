using System;
using System.Text;

namespace CarLog.Core.DataModel
{
    /// <summary>
    /// Contains the data related to a fuel even
    /// </summary>
    public class FuelEvent : CommonEvent
    {
        private Double _quantity;
        private Double _price;
        private Double _distance;
        private FuelGrade _fuelGrade;
        private Boolean _ignoreForStats;        

        /// <summary>
        /// The quantity of fuel purchased
        /// </summary>
        public Double Quantity
        {
            get { return this._quantity; }
            set
            {
                try
                {
                    this._quantity = Double.Parse(value.ToString());
                    this.NotifyPropertyChanged("Quantity");
                    this.NotifyPropertyChanged("DistancePerQuantity");
                    this.NotifyPropertyChanged("TotalCost");
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// The price of the fuel purchased
        /// </summary>
        public Double Price
        {
            get { return this._price; }
            set
            {
                try
                {
                    this._price = Double.Parse(value.ToString());
                    this.NotifyPropertyChanged("Price");
                    this.NotifyPropertyChanged("TotalCost");
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// The distance traveled since the last fuel purchase
        /// </summary>
        public Double Distance
        {
            get { return this._distance; }
            set
            {
                try
                {
                    this._distance = Double.Parse(value.ToString());
                    this.NotifyPropertyChanged("Distance");
                    this.NotifyPropertyChanged("DistancePerQuantity");
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// The grade of the fuel purchased
        /// </summary>
        public FuelGrade FuelGrade
        {
            get { return this._fuelGrade; }
            set
            {
                this._fuelGrade = value;
                this.NotifyPropertyChanged("FuelGrade");
            }
        }

        /// <summary>
        /// The Distance per Quantity (Read Only)
        /// </summary>
        public Double DistancePerQuantity
        {
            get
            {
                try
                {
                    Double result = this.Distance / this.Quantity;

                    if (Double.IsNaN(result))
                    {
                        return 0;
                    }
                    else
                    {
                        return result;
                    }
                }
                catch
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// The total cost of the refueling
        /// </summary>
        public Double TotalCost
        {
            get
            {
                try
                {
                    Double result = this.Quantity * this.Price;
                    
                    if (Double.IsNaN(result))
                    {
                        return 0;
                    }
                    else
                    {
                        return result;
                    } 
                }
                catch
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Whether or not to include this event in calculations
        /// </summary>
        public Boolean IgnoreForStats
        {
            get
            {
                return this._ignoreForStats;
            }
            set
            {
                this._ignoreForStats = value;
                this.NotifyPropertyChanged("IgnoreForStats");
            }
        }        

        /// <summary>
        /// Generic constructor for fuel events
        /// </summary>
        public FuelEvent()
            : base()
        {
        }

        /// <summary>
        /// Constructor for Fuel Events that takes the correct types on parameters
        /// </summary>
        /// <param name="id">The Guid of the fuel event</param>
        /// <param name="carID">The Guid of the car of the fuel event</param>
        /// <param name="dateTime">The date/time the fuel event took place</param>
        /// <param name="distance">The distance travelled during the fuel event</param>
        /// <param name="fuelGrade">The fuel grade purchased for the fuel event</param>
        /// <param name="note">The note associated with the fuel event</param>
        /// <param name="odometer">The odometer reading associated with the fuel event</param>
        /// <param name="price">The price of the fuel for the fuel event</param>
        /// <param name="quantity">The quantity of fuel purchased for the fuel event</param>
        /// <param name="title">The title of the fuel event</param>
        /// <param name="ignoreForStats">Whether or not to ignore for calculations for the fuel event</param>
        /// <param name="recordStatus">The record status of the fuel event</param>
        /// <param name="syncStatus">The sync status of the fuel event</param>
        /// <param name="updateDate">The update date of the fuel event</param>
        /// <param name="location">The location of the fuel event</param>
        public FuelEvent(Guid id, Guid carID, DateTime dateTime, Double distance, FuelGrade fuelGrade, String note, Double odometer, Double price, Double quantity, String title, Boolean ignoreForStats, RecordStatus recordStatus, SyncStatus syncStatus, DateTime updateDate, String location)
        {
            this.ID = id;
            this.CarID = carID;
            this.DateTime = dateTime;
            this.Distance = distance;
            this.FuelGrade = fuelGrade;
            this.Note = note;
            this.Odometer = odometer;
            this.Price = price;
            this.Quantity = quantity;
            this.Title = title;
            this.IgnoreForStats = ignoreForStats;
            this.RecordStatus = recordStatus;
            this.SyncStatus = syncStatus;
            this.UpdateDate = updateDate;
            this.Location = location;
        }

        /// <summary>
        /// Constructor for Fuel Events that takes strings for all parameters
        /// </summary>
        /// <param name="id">The Guid of the fuel event</param>
        /// <param name="carID">The Guid of the car of the fuel event</param>
        /// <param name="dateTime">The date/time the fuel event took place</param>
        /// <param name="distance">The distance travelled during the fuel event</param>
        /// <param name="fuelGrade">The fuel grade purchased for the fuel event</param>
        /// <param name="note">The note associated with the fuel event</param>
        /// <param name="odometer">The odometer reading associated with the fuel event</param>
        /// <param name="price">The price of the fuel for the fuel event</param>
        /// <param name="quantity">The quantity of fuel purchased for the fuel event</param>
        /// <param name="title">The title of the fuel event</param>
        /// <param name="ignoreForStats">Whether or not to ignore for calculations for the fuel event</param>
        /// <param name="recordStatus">The record status of the fuel event</param>
        /// <param name="syncStatus">The sync status of the fuel event</param>
        /// <param name="updateDate">The update date of the fuel event</param>
        /// <param name="location">The location of the fuel event</param>
        public FuelEvent(String id, String carID, String dateTime, String distance, String fuelGrade, String note, String odometer, String price, String quantity, String title, String ignoreForStats, String recordStatus, String syncStatus, String updateDate, String location)
        {
            this.ID = Guid.Parse(id);
            this.CarID = Guid.Parse(carID);
            this.DateTime = DateTime.Parse(dateTime);
            this.Distance = Double.Parse(distance);
            this.FuelGrade = (FuelGrade)(Enum.Parse(FuelGrade.GetType(), fuelGrade));
            this.Note = note;
            this.Odometer = Double.Parse(odometer);
            this.Price = Double.Parse(price);
            this.Quantity = Double.Parse(quantity);
            this.Title = title;
            this.IgnoreForStats = Boolean.Parse(ignoreForStats);
            this.RecordStatus = (DataModel.RecordStatus)(Enum.Parse(RecordStatus.GetType(), recordStatus));
            this.SyncStatus = (DataModel.SyncStatus)(Enum.Parse(SyncStatus.GetType(), syncStatus));
            this.UpdateDate = DateTime.Parse(updateDate);
            this.Location = location;
        }

        /// <summary>
        /// Creates a save string for the fuel event for the save file
        /// </summary>
        /// <returns>The string containing the fuel event for the save file</returns>
        public String ToSaveString()
        {
            StringBuilder sbSaveString = new StringBuilder();

            sbSaveString.Append("FUELEVENT`");
            sbSaveString.Append(this.ID);
            sbSaveString.Append("`");
            sbSaveString.Append(this.CarID);
            sbSaveString.Append("`");
            sbSaveString.Append(this.DateTime.ToString("yyyy-MM-dd hh:mm:ss"));
            sbSaveString.Append("`");
            sbSaveString.Append(this._distance);
            sbSaveString.Append("`");
            sbSaveString.Append(this._fuelGrade);
            sbSaveString.Append("`");
            sbSaveString.Append(this.Note);
            sbSaveString.Append("`");
            sbSaveString.Append(this.Odometer);
            sbSaveString.Append("`");
            sbSaveString.Append(this._price);
            sbSaveString.Append("`");
            sbSaveString.Append(this._quantity);
            sbSaveString.Append("`");
            sbSaveString.Append(this.Title);
            sbSaveString.Append("`");
            sbSaveString.Append(this.IgnoreForStats);
            sbSaveString.Append("`");
            sbSaveString.Append(this.RecordStatus);
            sbSaveString.Append("`");
            sbSaveString.Append(this.SyncStatus);
            sbSaveString.Append("`");
            sbSaveString.Append(this.UpdateDate.ToString("yyyy-MM-dd hh:mm:ss"));
            sbSaveString.Append("`");
            sbSaveString.Append(Helpers.FileHelper.ReplaceSaveFileCharacters(this.Location));
            sbSaveString.Append("~");

            return sbSaveString.ToString();
        }

        /// <summary>
        /// Creates a delete string for the fuel event for the save file
        /// </summary>
        /// <returns>The delete string for the fuel event for save file</returns>
        public String ToDeletedSaveString()
        {
            StringBuilder sbSaveString = new StringBuilder();

            sbSaveString.Append("DELETEDFUELEVENT`");
            sbSaveString.Append(this.ID);
            sbSaveString.Append("`");
            sbSaveString.Append(this.CarID);
            sbSaveString.Append("`");
            sbSaveString.Append(this.DateTime.ToString("yyyy-MM-dd hh:mm:ss"));
            sbSaveString.Append("`");
            sbSaveString.Append(this._distance);
            sbSaveString.Append("`");
            sbSaveString.Append(this._fuelGrade);
            sbSaveString.Append("`");
            sbSaveString.Append(this.Note);
            sbSaveString.Append("`");
            sbSaveString.Append(this.Odometer);
            sbSaveString.Append("`");
            sbSaveString.Append(this._price);
            sbSaveString.Append("`");
            sbSaveString.Append(this._quantity);
            sbSaveString.Append("`");
            sbSaveString.Append(this.Title);
            sbSaveString.Append("`");
            sbSaveString.Append(this.RecordStatus);
            sbSaveString.Append("`");
            sbSaveString.Append(this.SyncStatus);
            sbSaveString.Append("`");
            sbSaveString.Append(this.UpdateDate.ToString("yyyy-MM-dd hh:mm:ss"));
            sbSaveString.Append("`");
            sbSaveString.Append(Helpers.FileHelper.ReplaceSaveFileCharacters(this.Location));
            sbSaveString.Append("~");

            return sbSaveString.ToString();
        }
    }    
}
