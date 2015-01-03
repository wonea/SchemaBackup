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
                        WorkingPath = @"C:\Temp\",
                        VpnCredential = 
                            new VpnCredential
                            {
                                HostName = "test",
                                Username = "test",
                                Password = "test"
                            },
                        SvnCredential = 
                            new SvnCredential
                            {
                                Path = new Uri("VisualSVN path"),
                                UserName = "testuser",
                                Password = "testpassword"
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
