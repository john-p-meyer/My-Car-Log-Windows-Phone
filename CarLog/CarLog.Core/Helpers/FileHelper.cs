using CarLog.Core.DataModel;
using CarLog.Core.ViewModels;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace CarLog.Core.Helpers
{
    /// <summary>
    /// Contains methods that help with file format manipulation
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// The file format for My Car Log uses ' and ~. This method removes those characters from any text strings passed to it.
        /// </summary>
        /// <param name="value">The string to remove ' and ~ from</param>
        /// <returns>The string value passed in without ' and ~ or a blank string if an error occurs so as not to corrupt the file</returns>
        public static String ReplaceSaveFileCharacters(String value)
        {
            try
            {
                if (value == null)
                    return value;
                else
                    return value.Replace("`", "").Replace("~", "");
            }
            catch
            {
                return "";
            }

        }        

        public static async Task LoadData(MainViewModel mainViewModel)
        {
            try
            {
                // Clear Loaded Data
                mainViewModel.Cars.Clear();

                // Get the local folder
                StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;

                if (local != null)
                {
                    string contents = "";

                    // Get the DataFolder folder
                    var dataFolder = await local.GetFolderAsync("DataFolder");

                    // Get the file
                    var file = await dataFolder.GetFileAsync("DataFile.txt");

                    IRandomAccessStream stream = await file.OpenReadAsync();

                    // Read the data
                    using (IRandomAccessStream textStream = await file.OpenReadAsync())
                    {
                        using (DataReader textReader = new DataReader(textStream))
                        {
                            uint textLength = (uint)textStream.Size;
                            await textReader.LoadAsync(textLength);
                            contents = textReader.ReadString(textLength);

                            String[] lines = contents.Split(new string[] { "\r", "\n", "~" }, StringSplitOptions.None);

                            String version = "";
                            foreach (String line in lines)
                            {
                                String[] values = line.Split('`');

                                if (values[0] == "VERSION")
                                {
                                    version = values[1];

                                    if (version == "")
                                    {
                                    }

                                }
                                else if (version == "1.0.0")
                                {
                                    if (values[0] == "CAR")
                                    {
                                        Car car = new Car(values[1], values[2], values[3], values[4], values[5], values[6], values[7], values[8], "", "", "0", "", "", "0", values[9], values[10], values[11]);
                                        if (mainViewModel.FindCar(car.ID) == null) 
                                        {
                                            mainViewModel.Cars.Add(car);
                                        }
                                    }

                                    if (values[0] == "FUELEVENT")
                                    {
                                        FuelEvent fuelEvent = new FuelEvent(values[1], values[2], values[3], values[4], values[5], values[6], values[7], values[8], values[9], values[10], values[11], values[12], values[13], values[14], "");
                                        if (mainViewModel.FindFuelEvent(fuelEvent.ID) == null)
                                        {
                                            mainViewModel.FindCar(fuelEvent.CarID).FuelEvents.Add(fuelEvent);
                                        }
                                    }

                                    if (values[0] == "SERVICEEVENT")
                                    {
                                        ServiceEvent serviceEvent = new ServiceEvent(values[1], values[2], values[3], values[4], values[5], values[6], values[7], values[8], values[9], values[10], values[11], values[12]);
                                        if (mainViewModel.FindServiceEvent(serviceEvent.ID) == null)
                                        {
                                            mainViewModel.FindCar(serviceEvent.CarID).ServiceEvents.Add(serviceEvent);
                                        }
                                    }
                                }
                                else if (version == "1.1.0")
                                {
                                    if (values[0] == "CAR")
                                    {
                                        Car car = new Car(values[1], values[2], values[3], values[4], values[5], values[6], values[7], values[8], values[9], values[10], values[11], values[12], values[13], values[14], values[15], values[16], values[17]);
                                        if (mainViewModel.FindCar(car.ID) == null)
                                        {
                                            mainViewModel.Cars.Add(car);
                                        }
                                    }

                                    if (values[0] == "FUELEVENT")
                                    {
                                        FuelEvent fuelEvent = new FuelEvent(values[1], values[2], values[3], values[4], values[5], values[6], values[7], values[8], values[9], values[10], values[11], values[12], values[13], values[14], "");
                                        if (mainViewModel.FindFuelEvent(fuelEvent.ID) == null)
                                        {
                                            mainViewModel.FindCar(fuelEvent.CarID).FuelEvents.Add(fuelEvent);
                                        }
                                    }

                                    if (values[0] == "SERVICEEVENT")
                                    {
                                        ServiceEvent serviceEvent = new ServiceEvent(values[1], values[2], values[3], values[4], values[5], values[6], values[7], values[8], values[9], values[10], values[11], values[12]);
                                        if (mainViewModel.FindServiceEvent(serviceEvent.ID) == null)
                                        {
                                            mainViewModel.FindCar(serviceEvent.CarID).ServiceEvents.Add(serviceEvent);
                                        }
                                    }

                                    if (values[0] == "REMINDER")
                                    {
                                        Reminder reminder = new Reminder(values[1], values[2], values[3], values[4], values[5], values[6], values[7], values[8], values[9], values[10], values[11], values[12], values[13], values[14], values[15], values[16]);
                                        if (mainViewModel.FindReminder(reminder.ID) == null)
                                        {
                                            mainViewModel.FindCar(reminder.CarID).Reminders.Add(reminder);
                                        }
                                    }
                                }
                                else if (version == "1.2.0")
                                {
                                    if (values[0] == "CAR")
                                    {
                                        Car car = new Car(values[1], values[2], values[3], values[4], values[5], values[6], values[7], values[8], values[9], values[10], values[11], values[12], values[13], values[14], values[15], values[16], values[17]);
                                        if (mainViewModel.FindCar(car.ID) == null)
                                        {
                                            mainViewModel.Cars.Add(car);
                                        }
                                    }

                                    if (values[0] == "FUELEVENT")
                                    {
                                        FuelEvent fuelEvent = new FuelEvent(values[1], values[2], values[3], values[4], values[5], values[6], values[7], values[8], values[9], values[10], values[11], values[12], values[13], values[14], values[15]);
                                        if (mainViewModel.FindFuelEvent(fuelEvent.ID) == null)
                                        {
                                            mainViewModel.FindCar(fuelEvent.CarID).FuelEvents.Add(fuelEvent);
                                        }
                                    }

                                    if (values[0] == "SERVICEEVENT")
                                    {
                                        ServiceEvent serviceEvent = new ServiceEvent(values[1], values[2], values[3], values[4], values[5], values[6], values[7], values[8], values[9], values[10], values[11], values[12]);
                                        if (mainViewModel.FindServiceEvent(serviceEvent.ID) == null)
                                        {
                                            mainViewModel.FindCar(serviceEvent.CarID).ServiceEvents.Add(serviceEvent);
                                        }
                                    }

                                    if (values[0] == "REMINDER")
                                    {
                                        Reminder reminder = new Reminder(values[1], values[2], values[3], values[4], values[5], values[6], values[7], values[8], values[9], values[10], values[11], values[12], values[13], values[14], values[15], values[16]);
                                        if (mainViewModel.FindReminder(reminder.ID) == null)
                                        {
                                            mainViewModel.FindCar(reminder.CarID).Reminders.Add(reminder);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    dataFolder = null;
                    file = null;
                    stream = null;
                    
                }
                else
                {

                }                

                mainViewModel.IsDataLoaded = true;
            }
            catch (Exception ex)
            {
            }            
        }

        public static async Task<String> SaveString(MainViewModel mainModelView)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("VERSION`1.2.0~");

            foreach (Car car in mainModelView.Cars)
            {
                sb.Append(car.ToSaveString());
            }

            foreach (Car car in mainModelView.Cars)
            {
                foreach (FuelEvent fuelEvent in car.FuelEvents)
                {
                    sb.Append(fuelEvent.ToSaveString());
                }

                foreach (ServiceEvent serviceEvent in car.ServiceEvents)
                {
                    sb.Append(serviceEvent.ToSaveString());
                }

                foreach (Reminder reminder in car.Reminders)
                {
                    sb.Append(reminder.ToSaveString());
                }
            }

            return sb.ToString();
        }

        public static async Task SaveData(String sb)
        {
            bool errorOccurred = false;

            try
            {
                // Get the local folder
                StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;

                // Create a new folder
                StorageFolder dataFolder = await local.CreateFolderAsync("DataFolder", CreationCollisionOption.OpenIfExists);

                try
                {
                    // Create a new file
                    StorageFile fileBackup = await dataFolder.CreateFileAsync("DataFile.txt", CreationCollisionOption.OpenIfExists);
                    await fileBackup.RenameAsync("DataFile.txtBackup", NameCollisionOption.ReplaceExisting);
                    fileBackup = null;
                }
                catch
                {
                }

                StorageFile file = await dataFolder.CreateFileAsync("DataFile.txt", CreationCollisionOption.ReplaceExisting);


                // Write the data
                using (var s = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    using (DataWriter textWriter = new DataWriter(s))
                    {
                        textWriter.WriteString(sb);
                        await textWriter.StoreAsync();
                    }
                }

                file = null;
                local = null;
            }
            catch
            {
                errorOccurred = true;
            }

            if (errorOccurred)
            {
                // Get the local folder
                StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;

                // Create a new folder
                StorageFolder dataFolder = await local.CreateFolderAsync("DataFolder", CreationCollisionOption.OpenIfExists);

                // Create a new file
                StorageFile fileBackup = await dataFolder.CreateFileAsync("DataFile.txtBackup", CreationCollisionOption.OpenIfExists);
                await fileBackup.RenameAsync("DataFile.txt");

                local = null;
                dataFolder = null;
                fileBackup = null;
            }
        }

        public static async Task SaveData(MainViewModel mainModelView)
        {
            await SaveData(await SaveString(mainModelView));            
        }
    }
}
