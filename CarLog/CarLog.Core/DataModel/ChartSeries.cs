using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarLog.Core.DataModel
{
    public class ChartSeries: INotifyPropertyChanged
    {
        private String _name;
        private Guid _id;

        public String Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
                this.NotifyPropertyChanged("Name");
            }
        }
        public Guid ID
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
            }
        }
        public ObservableCollection<ChartDataPoint> DataPoints { get; private set; }

        public ChartSeries(Guid id, String name)
        {
            this.Name = name;
            this.ID = id;
            this.DataPoints = new ObservableCollection<ChartDataPoint>();
        }

        public ChartSeries(String name)
            : this(Guid.Empty, name)
        {
        }

        public ChartSeries()
            : this(Guid.Empty, "")
        {
        }

        public ChartDataPoint GetDataPoint(DateTime dateTime)
        {
            ChartDataPoint returnPoint = null;

            foreach (ChartDataPoint dataPoint in DataPoints)
            {
                if (dataPoint.DateTime == dateTime)
                    returnPoint = dataPoint;
            }

            return returnPoint;
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
    }
}
