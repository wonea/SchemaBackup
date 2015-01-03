using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SchemaBackup.Definitions;
using SchemaBackup.Core;
using System.IO;

namespace SchemaBackup.Tests
{
    [TestClass]
    public class SchemaSettingTests
    {
        [TestMethod]
        public void SaveSettings()
        {
            // build example schema settings
            var schemasetting = new SchemaSettings
            {
                Settings = new[]
                { 
                    new SchemaSetting
                    {
                        Name = "Test Schema",
                        WorkingPath = @"C:\Temp\",
                        DBConnectionStr = "test",
                        VpnCredential = new VpnCredential
                        {
                            HostName = "test",
                            Username = "test",
                            Password = "test"
                        },
                        SvnCredential = new SvnCredential
                        {
                            Path = new Uri("VisualSVN path"),
                            UserName = "testuser",
                            Password = "testpassword"
                        },
                        CheckFrequency = new CheckFrequency
                        {
                            Frequency = " 0 0 12 1/1 * ? * "
                        }
                    }
                }
            };
            // export schema
            const string workingpathfile = @"C:\Temp\SchemaBackupTest\settings.xml";
            var settingserialisation = new SettingsSerialisation(workingpathfile);
            // check file exists
            Assert.IsTrue(File.Exists(workingpathfile));
        }
    }
}
