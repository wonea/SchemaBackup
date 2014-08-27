using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using SchemaBackup.Service.Properties;
using SchemaBackup.Core;

namespace SchemaBackup
{
    public partial class Service1 : ServiceBase
    {
        public string[] DbConnectionStrings;
        public SettingsSerialisation SettingsSerialisation;

        public Service1()
        {
            InitializeComponent();
            // load test connection strings
            string[] strArray = new string[Settings.Default.ConnectionStrings.Count];
            DbConnectionStrings.CopyTo(strArray, 0);
        }

        protected override void OnStart(string[] args)
        {
            // load settings
            SettingsSerialisation = new SettingsSerialisation(Settings.Default.SettingFileLocation);
            SettingsSerialisation.Load();
        }

        protected override void OnStop() 
        {
            // Don't save settings
        }
    }
}
