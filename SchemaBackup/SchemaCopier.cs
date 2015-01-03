using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using SchemaBackup.Service.Properties;

namespace SchemaBackup.Service
{
    public class SchemaCopier
    {
        //private string[] DbConnectionStrings;

        public SchemaCopier(SqlConnection sqlConnection)
        {
            Initialise();
        }

        public void Initialise()
        {

        }
    }
}