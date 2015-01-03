using SchemaBackup.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchemaBackup.Core.ExampleObjects
{
    public static class ExampleObjects
    {
        public static SchemaSettings SchemaSettings()
        {
            var schemasetting = new SchemaSettings
            {
                Settings = new[]
                { 
                    new SchemaSetting
                    {
                        Name = "Test Schema",
                        WorkingPath = @"C:\Temp\",
                        DBConnectionStr = "test",
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
            return schemasetting;
        }
    }
}
