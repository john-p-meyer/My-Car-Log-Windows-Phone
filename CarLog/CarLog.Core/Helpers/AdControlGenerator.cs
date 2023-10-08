using Microsoft.Advertising.Mobile.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CarLog.Core.Helpers
{
    public static class AdControlGenerator
    {
        public static AdControl GenerateAdControl(int row)
        {
            // <UI:AdControl ApplicationId="" AdUnitId="" HorizontalAlignment="Left" Height="80" VerticalAlignment="Bottom" Width="480"/>
            AdControl adControl;

#if DEBUG
            adControl = new AdControl("test_client", "Image480_80", true);
#else
            adControl = new AdControl("", "", true);
#endif
            adControl.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            adControl.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            adControl.Height = 80;
            adControl.Width = 480;
            adControl.IsAutoCollapseEnabled = false;
            adControl.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;            
                        
            Grid.SetRow(adControl, row);

            return adControl;
        }

        public static AdControl GenerateAdControl()
        {
            return GenerateAdControl(0);
        }

        
    }
}
