using Microsoft.Live;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarLog.Core.Helpers
{
    public static class OneDriveHelper
    {
        public static Microsoft.Live.LiveConnectSession OneDriveSession;
        public static String ClientId = "0000000048136654";

        public async static Task<String> DownloadFile()
        {
            string path = null;
            string dllink = null;
            int _isize;

            try
            {
                // Ensure that the user has consented to the wl.skydrive and wl.skydrive_update scopes                
                LiveConnectClient client = new LiveConnectClient(OneDriveHelper.OneDriveSession);

                LiveOperationResult operationResult = await client.GetAsync("me/skydrive/files");

                dynamic dyResult = ((dynamic)operationResult.Result).data;

                foreach (var item in dyResult)
                {
                    if (((dynamic)item).name == "MyCarLog.txt")
                    {
                        path = ((dynamic)item).id + "/content/";
                        dllink = ((dynamic)item).source;
                        _isize = int.Parse(((dynamic)item).size.ToString());
                        break;
                    }
                }

                if (path != null)
                {
                    var downloadOperationResult = await client.DownloadAsync(path);

                    using (Stream downloadStream = downloadOperationResult.Stream)
                    {
                        if (downloadStream != null)
                        {
                            StreamReader reader = new StreamReader(downloadStream);
                            String text = reader.ReadToEnd();

                            return text;
                        }
                    }
                }

            }
            catch (LiveConnectException ex)
            {

            }
            catch
            {

            }

            return "";            
        }

        public static async Task<Boolean> UploadFile(String text)
        {
            try
            {
                if (OneDriveHelper.OneDriveSession != null)
                {
                    var liveConnectClient = new LiveConnectClient(OneDriveHelper.OneDriveSession);

                    // Upload to OneDrive
                    LiveOperationResult uploadOperation = await liveConnectClient.UploadAsync("me/SkyDrive", "MyCarLog.txt", new MemoryStream(System.Text.Encoding.UTF8.GetBytes(text)), OverwriteOption.Overwrite);
                    await HandleUploadResults(uploadOperation);

                    return true;
                }
            }
            catch (LiveAuthException ex)
            {
                // Handle errors
                return false;
            }
            catch (LiveConnectException ex)
            {
                // Handle errors
                return false;
            }
            catch
            {
                return false;
            }

            return false;
        }

        private static async Task HandleUploadResults(LiveOperationResult operation)
        {
            /*var pendingOperations = await LiveConnectClient ;
            foreach (LiveDownloadOperation pendingOperation in pendingOperations)
            {
                try
                {
                    var opResult = await pendingOperation.AttachAsync();
                    // Handle results.
                }
                catch
                {
                    // Handle errors.
                }
            }*/

        }

        public static async Task<Boolean> buttonSignin_SessionChanged(object sender, Microsoft.Live.Controls.LiveConnectSessionChangedEventArgs e)
        {
            if (e.Session != null && e.Status == LiveConnectSessionStatus.Connected)
            {
                OneDriveHelper.OneDriveSession = e.Session;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
