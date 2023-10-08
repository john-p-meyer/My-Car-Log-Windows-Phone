using CarLog.Core.DataModel;
using CarLog.Core.Resources;
using System;
using System.ComponentModel;
using System.Windows;

namespace CarLog.Core.Helpers
{
    /// <summary>
    /// Provides public properties which change based on what is currently in the settings. 
    /// (e.g. Distance can return eithers Miles or Kilometers in local language)
    /// </summary>
    public class LocalizedVariableSettingsStrings : INotifyPropertyChanged
    {
        /// <summary>
        /// Returns either Miles or Kilometers based on IsMiles
        /// </summary>
        public String Distance
        {
            get
            {
                try
                {
                    if (this._appSettings.IsMiles)
                    {
                        return AppResources.Miles;
                    }
                    else
                    {
                        return AppResources.Kilometers;
                    }
                }
                catch
                {
                    return "miles";
                }
            }
        }

        /// <summary>
        /// Returns Miles per Gallon, Miles per Liter, Kilometers per Gallon, or Kilometers per Liter based on IsMiles and IsGallons
        /// </summary>
        public String DistancePerQuantity
        {
            get
            {
                try
                {
                    if (this._appSettings.IsMiles)
                    {
                        if (this._appSettings.IsGallons)
                        {
                            return AppResources.MilesPerGallon;
                        }
                        else
                        {
                            return AppResources.MilesPerLiter;
                        }
                    }
                    else
                    {
                        if (this._appSettings.IsGallons)
                        {
                            return AppResources.KilometersPerGallon;
                        }
                        else
                        {
                            return AppResources.KilometersPerLiter;
                        }
                    }
                }
                catch
                {
                    return "miles per gallon";
                }
            }
        }

        /// <summary>
        /// Returns MPG, MPL, KPG, or KPL depending on IsMiles and IsGallons
        /// </summary>
        public String DistancePerQuantityShort
        {
            get
            {
                try
                {
                    if (this._appSettings.IsMiles)
                    {
                        if (this._appSettings.IsGallons)
                        {
                            return AppResources.MilesPerGallonShort;
                        }
                        else
                        {
                            return AppResources.MilesPerLiterShort;
                        }
                    }
                    else
                    {
                        if (this._appSettings.IsGallons)
                        {
                            return AppResources.KilometersPerGallonShort;
                        }
                        else
                        {
                            return AppResources.KilometersPerLiterShort;
                        }
                    }
                }
                catch
                {
                    return "mpg";
                }
            }
        }

        /// <summary>
        /// Returms either mi or ki depending on IsMiles
        /// </summary>
        public String DistanceShort
        {
            get
            {
                try
                {
                    if (this._appSettings.IsMiles)
                    {
                        return AppResources.MilesShort;
                    }
                    else
                    {
                        return AppResources.KilometersShort;
                    }
                }
                catch
                {
                    return "mi";
                }
            }
        }

        /// <summary>
        /// Returns per gallon or per liter based on IsGallons
        /// </summary>
        public String PerQuantity
        {
            get
            {
                try
                {
                    if (this._appSettings.IsGallons)
                    {
                        return AppResources.PerGallon;
                    }
                    else
                    {
                        return AppResources.PerLiter;
                    }
                }
                catch
                {
                    return "per gallon";
                }
            }
        }

        /// <summary>
        /// Returns gallons or liters based on IsGallons
        /// </summary>
        public String Quantity
        {
            get
            {
                try
                {
                    if (this._appSettings.IsGallons)
                    {
                        return AppResources.Gallons;
                    }
                    else
                    {
                        return AppResources.Liters;
                    }
                }
                catch
                {
                    return "gallons";
                }
            }
        }

        /// <summary>
        /// Returns Gallons or Liters based on IsGallons
        /// </summary>
        public String QuantityC
        {
            get
            {
                try
                {
                    if (this._appSettings.IsGallons)
                    {
                        return AppResources.GallonsC;
                    }
                    else
                    {
                        return AppResources.LitersC;
                    }
                }
                catch
                {
                    return "Gallons";
                }
            }
        }

        /// <summary>
        /// Returns gal or lit based on IsGallons
        /// </summary>
        public String QuantityShort
        {
            get
            {
                try
                {
                    if (this._appSettings.IsGallons)
                    {
                        return AppResources.GallonShort;
                    }
                    else
                    {
                        return AppResources.LiterShort;
                    }
                }
                catch
                {
                    return "gal";
                }
            }
        }

        /// <summary>
        /// Returns Miles Since Last or Kilometers Since Last based on IsMiles
        /// </summary>
        public String DistanceSinceLast
        {
            get
            {
                try
                {
                    if (this._appSettings.IsMiles)
                    {
                        return AppResources.MilesSinceDisplayName;
                    }
                    else
                    {
                        return AppResources.KilometersSinceDisplayName;
                    }
                }
                catch
                {
                    return "Miles Since Last";
                }
            }
        }

        /// <summary>
        /// Returns Months or Miles or Months or Kilometers based on IsMiles
        /// </summary>
        public String MonthsOrDistanceSinceLast
        {
            get
            {
                try
                {
                    if (this._appSettings.IsMiles)
                    {
                        return AppResources.MonthsOrMilesSinceDisplayName;
                    }
                    else
                    {
                        return AppResources.MonthsOrKilometersSinceDisplayName;
                    }
                }
                catch
                {
                    return "Months or Miles Since Last";
                }
            }
        }

        /// <summary>
        /// Returns whether compact view is being used or not based on IsCompactView
        /// </summary>
        public Visibility IsCompactView
        {
            get
            {
                try
                {
                    if (this._appSettings.IsCompactView)
                    {
                        return Visibility.Visible;
                    }
                    else
                    {
                        return Visibility.Collapsed;
                    }
                }
                catch
                {
                    return Visibility.Collapsed;
                }
            }
        }

        /// <summary>
        /// Returns whether compact view is being used or not based on IsCompactView
        /// </summary>
        public Visibility IsNormalView
        {
            get
            {
                try
                {
                    if (this._appSettings.IsCompactView)
                    {
                        return Visibility.Collapsed;
                    }
                    else
                    {
                        return Visibility.Visible;
                    }
                }
                catch
                {
                    return Visibility.Visible;
                }
            }
        }

        /// <summary>
        /// Creates a new instance of AppSettings for use by properties
        /// </summary>
        public LocalizedVariableSettingsStrings()
        {
            this._appSettings = new AppSettings();
        }

        /// <summary>
        /// Refreshes all properties, used when settings change
        /// </summary>
        public void RefreshSettingText()
        {
            this.NotifyPropertyChanged("Distance");
            this.NotifyPropertyChanged("DistancePerQuantity");
            this.NotifyPropertyChanged("DistancePerQuantityShort");
            this.NotifyPropertyChanged("DistanceShort");
            this.NotifyPropertyChanged("PerQuantity");
            this.NotifyPropertyChanged("Quantity");
            this.NotifyPropertyChanged("QuantityShort");
            this.NotifyPropertyChanged("QuantityC");
            this.NotifyPropertyChanged("DistanceSinceLast");
            this.NotifyPropertyChanged("MonthsOrDistanceSinceLast");
            this.NotifyPropertyChanged("IsCompactView");
            this.NotifyPropertyChanged("IsNormalView");
        }

        // Settings determined text variables
        private AppSettings _appSettings;

        /// <summary>
        /// Property Changed event handler
        /// </summary>
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
