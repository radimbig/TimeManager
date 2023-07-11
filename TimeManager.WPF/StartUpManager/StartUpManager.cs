using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.Win32;
using System;
using System.Reflection;

namespace TimeManager.WPF.StartUpManager
{
    public static class StartUpManager
    {
        private const string StartupRegistryKey = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
        public static void SetStartUp(string appName, bool status)
        {
            using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(StartupRegistryKey, true))
            {
                if(key == null)
                {
                    throw new InvalidOperationException("Error when setting registry key");
                }
                if(status)
                {
                    key.SetValue(appName, Assembly.GetExecutingAssembly().Location.Replace("/", "\\"));
                    return;
                }
                key.DeleteValue(appName, false);
            }
        }
        public static bool IsStartUpEnabled(string appName)
        {
            using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(StartupRegistryKey))
            {
                if (key != null)
                {
                    object? value = key.GetValue(appName);
                    return value != null;
                }
            }

            return false;
        }


        // funtion from another program as exaple
        /*private void SetStartup(bool enable)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(StartupRegistryKey, true))
            {
                if (enable)
                {
                    key.SetValue(ApplicationName, Application.ExecutablePath.Replace("/", "\\"));
                }
                else
                {
                    key.DeleteValue(ApplicationName, false);
                }
            }
        }*/
    }
}
