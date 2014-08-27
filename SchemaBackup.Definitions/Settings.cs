using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchemaBackup.Definitions
{
    public class SchemaSettings
    {
        public SchemaSetting[] Settings;
    }

    public class SchemaSetting
    {
        public string WorkingPath { get; set; }
        public VpnCredential VpnCredential { get; set; }
        public SvnCredential SvnCredential { get; set; }
        public string DBConnectionStr { get; set; }
    }

    public class VpnCredential
    {
        public string VpnHostName { get; set; }
        public string VpnUsername { get; set; }
        public string VpnPassword { get; set; }
    }

    public class SvnCredential
    {
        public string Path { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}