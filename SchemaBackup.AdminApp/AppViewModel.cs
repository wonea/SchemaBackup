using SchemaBackup.AdminApp.Properties;
using SchemaBackup.Core;
using SchemaBackup.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchemaBackup.AdminApp
{
    public class AppViewModel : IDisposable
    {
        public AppViewModel()
        {
            SettingsSerialisation = new SettingsSerialisation(Settings.Default.SettingFileLocation);
            SettingsSerialisation.Load();
            ScrollToBottom = Settings.Default.ScrollToBottom;
        }

        public AppViewModel(SchemaSettings schemaSettings)
        {
            SchemaSettings = schemaSettings;
        }

        public SchemaSettings SchemaSettings;
        private SettingsSerialisation SettingsSerialisation;

        public IEnumerable<string> SchemaNames
        {
            get
            {
                return SchemaSettings.Settings.Select(s => s.Name);
            }
        }

        public string LogStr { get; set; }
        public void LogMessage(string message)
        {
            LogStr += String.Format("{0}{1]", Environment.NewLine, message);
        }

        public bool ScrollToBottom { get; set; }

        public void SaveDetails()
        {
            Settings.Default.ScrollToBottom = ScrollToBottom;
        }

        #region Dispose
        public void Dispose()
        {
            SaveDetails();
            Dispose(true);
        }

        private void Dispose(bool doDispose)
        {
            if (doDispose)
                SettingsSerialisation.Dispose();
        }
        #endregion
    }
}