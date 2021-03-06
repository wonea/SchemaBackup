﻿using SchemaBackup.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchemaBackup.Core.ExampleObjects
{
    public static class ExampleObjects
    {
        public static SchemaSettings SchemaSettings
        {
            get
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
                            VpnCredential = new VpnCredential
                            {
                                HostName = "test",
                                Username = "test",
                                Password = "test"
                            },
                            SvnCredential = new SvnCredential
                            {
                                Path = new Uri("file:\\C:\\Users\\john\\documents\\visual studio 2010\\Projects\\proj"),
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
                return schemasetting;
            }
        }
    }
}
