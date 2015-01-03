using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchemaBackup.Definitions
{
    public class SchemaSettings
    {
        public IEnumerable<SchemaSetting> Settings;
    }

    /// <summary>
    /// Complete settings for a schema
    /// </summary>
    public class SchemaSetting
    {
        /// <summary>
        /// Name of schema
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Local Working Path (used to keeping a copy of the schema locally)
        /// </summary>
        public string WorkingPath { get; set; }
        /// <summary>
        /// VPN Credentials
        /// </summary>
        public VpnCredential VpnCredential { get; set; }
        /// <summary>
        /// SVN Credentials
        /// </summary>
        public SvnCredential SvnCredential { get; set; }
        /// <summary>
        /// Database connection string
        /// </summary>
        public string DBConnectionStr { get; set; }
        /// <summary>
        /// Check Frequency
        /// </summary>
        public CheckFrequency CheckFrequency { get; set; }
    }

    /// <summary>
    /// Optional: VPN Credentials to gain access to database
    /// </summary>
    public class VpnCredential
    {
        public string HostName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    /// <summary>
    /// SVN Credentials for committing schema
    /// </summary>
    public class SvnCredential
    {
        /// <summary>
        /// Path to commit to
        /// </summary>
        public Uri Path { get; set; }
        /// <summary>
        /// UserName to use
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Password to use
        /// </summary>
        public string Password { get; set; }
    }

    /// <summary>
    /// Check Frequency i.e. when a schema will be checked
    /// </summary>
    public class CheckFrequency
    {
        /// <summary>
        /// CRON string representing time intervals
        /// </summary>
        public string Frequency { get; set; }
    }
}