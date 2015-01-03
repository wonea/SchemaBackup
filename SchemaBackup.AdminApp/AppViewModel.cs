using SchemaBackup.AdminApp.UserControls;
using SchemaBackup.AdminApp.Properties;
using SchemaBackup.Core;
using SchemaBackup.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using SchemaBackup.AdminApp.Commands;

namespace SchemaBackup.AdminApp
{
    public class AppViewModel : IDisposable
    {
        public AppViewModel()
        {
            SettingsSerialisation = new SettingsSerialisation(Settings.Default.SettingFileLocation);
            SettingsSerialisation.Load();
            SettingsSerialisation.LoadTestData();
            ScrollToBottom = Settings.Default.ScrollToBottom;
            LogMessage("Started Application");
            _canExecute = true;
        }

        public AppViewModel(SchemaSettings schemaSettings)
        {
            SettingsSerialisation = new SettingsSerialisation(Settings.Default.SettingFileLocation);
            SettingsSerialisation.SchemaSettings = schemaSettings;
            _canExecute = true;
        }

        #region Properties
        private SettingsSerialisation SettingsSerialisation;

        public IEnumerable<string> SchemaNames
        {
            get
            {
                return SettingsSerialisation.SchemaSettings.Settings.Select(s => s.Name);
            }
        }

        public string LogStr { get; set; }
        public void LogMessage(string message)
        {
            LogStr += String.Format("{0}{1}", Environment.NewLine, message);
        }

        public bool ScrollToBottom { get; set; }
        public string SelectedSchemaName { get; set; }
        #endregion

        #region Commands
        private ICommand _aboutCommand;
        public ICommand AboutCommand
        {
            get
            {
                return _aboutCommand ?? (_aboutCommand = new GenericCommandHandler(() => AboutAction(), _canExecute));
            }
        }
        private bool _canExecute;
        public void AboutAction()
        {
            AboutWindow aboutwindow = new AboutWindow();
            aboutwindow.ShowDialog();
        }
        #endregion

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