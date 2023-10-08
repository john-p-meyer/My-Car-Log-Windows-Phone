using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarLog.Core.DataModel
{
    public class ChartDataPoint: INotifyPropertyChanged
    {
        private Double _value;
        private DateTime _dateTime;
        private Double _count;

        public Double Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
                this.NotifyPropertyChanged("Value");
            }
        }
        public DateTime DateTime {
            get
            {
                return this._dateTime;
            }
            set
            {
                this._dateTime = value;
                this.NotifyPropertyChanged("DateTime");
            }
        }
        public Double Count
        {
            get
            {
                return this._count;
            }
            set
            {
                this._count = value;
                this.NotifyPropertyChanged("Count");
            }
        }

        public ChartDataPoint()
            : this(0, DateTime.Now)
        {
        }

        public ChartDataPoint(Double value, DateTime dateTime)            
        {
            this.Value = value;
            this.DateTime = dateTime;
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
