using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SchemaBackup.Tests.Properties;
using System.Data;
using System.Data.SqlClient;
using SchemaBackup.Definitions;
using SchemaBackup.Core;
using SchemaBackup.Scheme;

namespace SchemaBackup.Tests
{
    [TestClass]
    public class UnitTest1
    {
        public string[] DbConnectionStrings;
        public string WorkingPath;
        public string[] IgnoredObjects;

        [TestInitialize]
        public void TestSetup()
        {
            // load test connection strings
            DbConnectionStrings = new string[Settings.Default.ConnectionStrings.Count];
            Settings.Default.ConnectionStrings.CopyTo(DbConnectionStrings, 0);
            WorkingPath = Settings.Default.WorkingPath;
            IgnoredObjects = new string[Settings.Default.ObjectsToIgnore.Count];
            Settings.Default.ObjectsToIgnore.CopyTo(IgnoredObjects, 0);
        }

        [TestMethod]
        public void GetObjectInfo()
        {
            IEnumerable<DataTable> datatables =
                SqlMethods.ExecuteDataTableReturnCommand(DbConnectionStrings[0], "sp_help @Name", new SqlParameter("Name", "Track"));
        }

        [TestMethod]
        public void ProcessDatabaseObjects()
        {
            Schema schema = new Schema(DbConnectionStrings[0], WorkingPath, IgnoredObjects);
            schema.Initialise();
            schema.Process();
        }

        [TestCategory("Schema Object")]
        [TestMethod]
        public void ProcessTable()
        {
            SchemaObject schemaObject = new SchemaObject(DbConnectionStrings[0], WorkingPath, ObjectType.U, "Track", IgnoredObjects);
            schemaObject.GetInfo();
        }

        [TestCategory("Schema Object")]
        [TestMethod]
        public void ProcessStoredProcedure()
        {
            SchemaObject schemaObject = new SchemaObject(DbConnectionStrings[0], WorkingPath, ObjectType.P, "sp_ScriptTable", IgnoredObjects);
            schemaObject.GetInfo();
        }

        [TestCategory("Schema Object")]
        [TestMethod]
        public void ProcessView()
        {
            SchemaObject schemaObject = new SchemaObject(DbConnectionStrings[0], WorkingPath, ObjectType.V, "TestView", IgnoredObjects);
            schemaObject.GetInfo();
        }

        [TestCategory("Schema Object")]
        [TestMethod]
        public void ProcessSchemeObject()
        {
            SchemaObject schemaObject = new SchemaObject(DbConnectionStrings[0], WorkingPath, ObjectType.FN, "ISOweek", IgnoredObjects);
            schemaObject.GetInfo();
        }

        [TestMethod]
        public void GetFolderPath()
        {
            Schema schema = new Schema(DbConnectionStrings[0], WorkingPath, IgnoredObjects);
            var s = schema.GetPath(ObjectType.U);
            Assert.IsTrue(s.EndsWith("Table"));
        }
    }
}
