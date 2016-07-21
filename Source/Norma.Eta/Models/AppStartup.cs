using System;
using System.Linq;
using System.Windows;

using Microsoft.Win32;

namespace Norma.Eta.Models
{
    public static class AppStartup
    {
        private static readonly string RegKey = @"Software\Microsoft\Windows\CurrentVersion\Run";

        public static void Register()
        {
            var registry = Registry.CurrentUser.OpenSubKey(RegKey, true);
            if (registry == null)
                return;
            if (registry.GetSubKeyNames().Contains(NormaConstants.IpsilonAppId))
                return;

            try
            {
                registry.SetValue(NormaConstants.IpsilonAppId, $"\"{NormaConstants.IpsilonExecutableFile}\"");
                MessageBox.Show("Registered Norma.Ipsilon to Windows startup.");
                IsRegistered = true;
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occured:" + Environment.NewLine + e.Message);
            }
            finally
            {
                registry.Close();
            }
        }

        public static void Unregister()
        {
            var registry = Registry.CurrentUser.OpenSubKey(RegKey, true);
            if (registry == null)
                return;
            if (!registry.GetValueNames().Contains(NormaConstants.IpsilonAppId))
                return;

            try
            {
                registry.DeleteValue(NormaConstants.IpsilonAppId);
                MessageBox.Show("Unregistered Norma.Ipsilon from Windows startup.");
                IsRegistered = false;
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occured:" + Environment.NewLine + e.Message);
            }
            finally
            {
                registry.Close();
            }
        }

        #region IsRegistered

        private static bool? _isRegistered;

        public static bool IsRegistered
        {
            get
            {
                if (_isRegistered.HasValue)
                    return _isRegistered.Value;

                var registry = Registry.CurrentUser.OpenSubKey(RegKey);
                if (registry == null)
                    _isRegistered = false;
                else if (registry.GetValueNames().Contains(NormaConstants.IpsilonAppId))
                    _isRegistered = true;
                else
                    _isRegistered = false;
                return _isRegistered.Value;
            }
            private set { _isRegistered = value; }
        }

        #endregion
    }
}