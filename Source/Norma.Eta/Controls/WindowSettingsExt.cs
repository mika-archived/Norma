using System.Configuration;
using System.Windows;

using MetroRadiance.UI.Controls;

namespace Norma.Eta.Controls
{
    public class WindowSettingsExt : WindowSettings
    {
        [UserScopedSetting]
        public bool? UpdateRequired
        {
            get { return (bool?) this["UpdateRequired"]; }
            set { this["UpdateRequired"] = value; }
        }

        public WindowSettingsExt(Window window) : base(window)
        {
            UpdateRequired = true;
        }
    }
}