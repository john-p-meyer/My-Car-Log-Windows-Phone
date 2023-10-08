using System;
using System.IO.IsolatedStorage;

namespace CarLog.Core.DataModel
{
    public class AppSettings
    {
        // App Setting Storage
        IsolatedStorageSettings settings;

        // The key names of settings
        const string DistanceIndexKeyName = "DistanceIndexSetting";
        const string QuantityIndexKeyName = "QuantityIndexSetting";
        const string CompactViewIndexKeyName = "CompactViewSetting";

        // The default values
        const int DistanceIndexSettingDefault = 0;
        const int QuantityIndexSettingDefault = 0;
        const int CompactViewIndexSettingDefault = 0;

        /// <summary>
        /// Returns true if program is supposed to use miles. Works by checking if the value for 
        /// Distance Index Setting is 0 or not as 0 is the index of miles
        /// </summary>
        public Boolean IsMiles
        {
            get
            {
                return (this.DistanceIndex == 0);
            }
        }
        /// <summary>
        /// Returns true if program is supposed to use gallons. Works by checking if the value for 
        /// Quantity Index Setting is 0 or not as 0 is the index of gallons
        /// </summary>
        public Boolean IsGallons
        {
            get
            {
                return (this.QuantityIndex == 0);
            }            
        }
        /// <summary>
        /// Returns true if program is supposed to use compact view. Works by checking if the value for
        /// Compact View Index Setting is 1 or not as 1 is the index of compact view
        /// </summary>
        public Boolean IsCompactView
        {
            get
            {
                return (this.CompactViewIndex == 1);
            }
        }
        /// <summary>
        /// Gets the index of the distance selector. 0 is miles, 1 is kilometers
        /// </summary>
        public int DistanceIndex
        {
            get
            {
                return GetValueOrDefault<int>(DistanceIndexKeyName, DistanceIndexSettingDefault);
            }
            set
            {
                if (AddOrUpdateValue(DistanceIndexKeyName, value))
                {
                    Save();
                    LocalizedVariableStrings.LocalizedVariableSettingsStrings.RefreshSettingText();
                }
            }
        }
        /// <summary>
        /// Gets the index of the quantity selector. 0 is gallons, 1 is liters
        /// </summary>
        public int QuantityIndex
        {
            get
            {
                return GetValueOrDefault<int>(QuantityIndexKeyName, QuantityIndexSettingDefault);
            }
            set
            {
                if (AddOrUpdateValue(QuantityIndexKeyName, value))
                {
                    Save();
                    LocalizedVariableStrings.LocalizedVariableSettingsStrings.RefreshSettingText();
                }
            }
        }
        /// <summary>
        /// Gets the index of the compact view selector. 0 is normal, 1 is compact view
        /// </summary>
        public int CompactViewIndex
        {
            get
            {
                return GetValueOrDefault<int>(CompactViewIndexKeyName, CompactViewIndexSettingDefault);
            }
            set
            {
                if (AddOrUpdateValue(CompactViewIndexKeyName, value))
                {
                    Save();
                    LocalizedVariableStrings.LocalizedVariableSettingsStrings.RefreshSettingText();
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public AppSettings()
        {
            try
            {
                settings = IsolatedStorageSettings.ApplicationSettings;
            }
            catch
            {
                
            }
        }

        /// <summary>
        /// Update a setting value for our application. If the setting does not
        /// exist, then add the setting.
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool AddOrUpdateValue(string Key, Object value)
        {
            bool valueChanged = false;

            // If the key exists
            if (settings.Contains(Key))
            {
                // If the value has changed
                if (settings[Key] != value)
                {
                    // Store the new value
                    settings[Key] = value;
                    valueChanged = true;
                }
            }
            // Otherwise create the key.
            else
            {
                settings.Add(Key, value);
                valueChanged = true;
            }
            return valueChanged;
        }

        /// <summary>
        /// Get the current value of the setting, or if it is not found, set the 
        /// setting to the default setting.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T GetValueOrDefault<T>(string Key, T defaultValue)
        {
            T value;

            // If the key exists, retrieve the value.
            if (settings.Contains(Key))
            {
                value = (T)settings[Key];
            }
            // Otherwise, use the default value.
            else
            {
                value = defaultValue;
            }
            return value;
        }

        /// <summary>
        /// Save the settings.
        /// </summary>
        public void Save()
        {
            settings.Save();
        }


    }
}
