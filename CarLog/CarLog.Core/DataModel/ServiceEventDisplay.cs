using System;
using System.ComponentModel;
using CarLog.Core.Resources;
using CarLog.Core.Helpers;

namespace CarLog.Core.DataModel
{
    /// <summary>
    /// Contains translation and image information for a service event
    /// </summary>
    public class ServiceEventDisplay : INotifyPropertyChanged
    {
        private ServiceEventType _serviceEventType;

        /// <summary>
        /// The Service Event type
        /// </summary>
        public ServiceEventType ServiceEventType
        {
            get
            {
                return this._serviceEventType;
            }
            set
            {
                this._serviceEventType = value;
                this.NotifyPropertyChanged("ServiceEventType");
                this.NotifyPropertyChanged("ServiceEventText");
                this.NotifyPropertyChanged("ServiceEventImagePath");
            }
        }

        /// <summary>
        /// The text to return associated with the Service Event Type
        /// </summary>
        public String ServiceEventText
        {
            get
            {
                switch (this._serviceEventType)
                {
                    case ServiceEventType.None:
                        return AppResources.NoneDisplayName;

                    case ServiceEventType.OilChange:
                        return AppResources.OilChangeDisplayName;

                    case ServiceEventType.Other:
                        return AppResources.OtherDisplayName;

                    case ServiceEventType.Repair:
                        return AppResources.RepairDisplayName;

                    case ServiceEventType.ScheduledMaintenance:
                        return AppResources.ScheduledMaintenanceDisplayName;
                                            
                    case ServiceEventType.ReplacementPart:
                        return AppResources.ReplacementPart;

                    default:
                        return AppResources.NoneDisplayName;
                }
            }
        }

        /// <summary>
        /// The image path for the Service Event Type
        /// </summary>
        public Uri ServiceEventImagePath
        {
            get
            {
                return ThemeImage.Uri("/Assets/ServiceEvent/[THEME]/" + this.ServiceEventType.ToString() + ".png", UriKind.Relative);
            }
        }

        /// <summary>
        /// Generic Constructor (sets to Service Event Type: None)
        /// </summary>
        public ServiceEventDisplay() : this(ServiceEventType.None) { }

        /// <summary>
        /// Constructor that takes a Service Event Type to set to
        /// </summary>
        /// <param name="serviceEventType">Service Event Type to keep properties of</param>
        public ServiceEventDisplay(ServiceEventType serviceEventType)
        {
            this.ServiceEventType = serviceEventType;
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
}
