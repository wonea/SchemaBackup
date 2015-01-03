using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using SchemaBackup.Service.Properties;
using SchemaBackup.Definitions;

namespace SchemaBackup.Service
{
    public class SchemaCopier
    {
        public SchemaCopier(SchemaSetting schemaSetting)
        {
            Initialise();
        }

        public void Initialise()
        {

        }
    }
}