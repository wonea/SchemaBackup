using SchemaBackup.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchemaBackup.AdminApp
{
    public class AppViewModel
    {
        public AppViewModel(SchemaSettings schemaSettings)
        {
            SchemaSettings = schemaSettings;
        }

        public SchemaSettings SchemaSettings;

        public void LogMessage()
        {

        }
    }
}
