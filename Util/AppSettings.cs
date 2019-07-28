using Microsoft.Extensions.Configuration;
using System;

namespace ContactBook.Util
{

    public static class AppSettings
    {
        /// <summary>
        /// returns values from appsettings.json with hierarchy key values mentioned in the configuration
        /// </summary>
        /// <param name="settings">dot separated key-hierarchy inside App Settings (ie smtp.port)</param>
        public static string GetSetting(string settings)
        {
            string[] settingsHeirarchy = settings.Split('.');
            if (settings.Length == 0)
                throw new Exception("Invalid settings input. Add at least one key");

            IConfigurationSection section = Startup.Configuration.GetSection("AppSettings");

            if (section == null)
                throw new Exception("AppSettings not defined in appsettings.json");
            try
            {
                foreach (string key in settingsHeirarchy)
                {
                    section = section.GetSection(key);
                }
            }
            catch (Exception ex)
            {
                Email.SendTechLogs("Trying to access invalid app settings: " + string.Join('.', settings), ex);
                throw ex;
            }
            return section.Value;
        }
    }
}
