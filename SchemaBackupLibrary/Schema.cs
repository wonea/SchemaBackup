using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SchemaBackup.Core;
using System.Data.SqlClient;
using SchemaBackup.Definitions;
using System.IO;

namespace SchemaBackup.Core
{
    public class Schema : ObjectDefinition
    {
        public Schema(string dbConnectionStr, string workingPath, string[] ignoredObjects)
        {
            DbConnectionStr = dbConnectionStr;
            WorkingPath = workingPath;
            IgnoredObjects = ignoredObjects;
            CreateStructureObject();
        }

        public void Initialise()
        {
            CreateFolderStructure();
        }

        #region Properties
        public string DatabaseName
        {
            get
            {
                return SqlMethods.GetDbName(DbConnectionStr);
            }
        }

        public string DataSource
        {
            get 
            {
                return SqlMethods.GetDbDataSource(DbConnectionStr);
            }
        }
        #endregion

        public void GetLastUpdate()
        {
            string sql = @"SELECT StartTime, LoginName
                            --,f.*
                            FROM   sys.traces t
                                CROSS APPLY fn_trace_gettable(REVERSE(SUBSTRING(REVERSE(t.path),
                                            CHARINDEX('\', REVERSE(t.path)), 260)
                                             ) + N'log.trc', DEFAULT) f
                            WHERE  t.is_default = 1
                                   AND ObjectName = @Name
                                   AND EventClass IN (46, /*Object:Created*/
                                                      47, /*Object:Dropped*/
                                                      164 /*Object:Altered*/ )
							ORDER BY StartTime DESC;";
            IEnumerable<DataTable> datatables = SqlMethods.ExecuteDataTableReturnCommand(DbConnectionStr, sql, new SqlParameter("Name", Name));
            if (datatables.Any() && datatables.First().Rows.Count > 0)
            {
                // get the most recent update info
                LastUpdated = DateTime.Parse(datatables.First().Rows[0]["StartTime"].ToString());
                string loginname = datatables.First().Rows[0]["LoginName"].ToString();
                string[] split = loginname.Split('\\');
                MachineName = split.ElementAt(0);
                UserName = split.ElementAt(1);
            }
        }

        public void GetInfo()
        {
            string sql = "sp_help @Name";
            IEnumerable<DataTable> datatables =
                SqlMethods.ExecuteDataTableReturnCommand(DbConnectionStr, sql, new SqlParameter("Name", Name));
            if (datatables.Any())
            {
                foreach (DataTable datatable in datatables)
                {
                    
                }
            }
        }

        public void Process()
        {
            // look for entries which no longer exist
            CheckForDeletedRecords();
            foreach (ObjectType objecttype in (ObjectType[]) Enum.GetValues(typeof(ObjectType)))
            {
                string[] objectnames = EnumerateDbObjects(objecttype);
                foreach (string objectname in objectnames)
                {
                    SchemaObject schemaObject = new SchemaObject(DbConnectionStr, WorkingPath, objecttype, objectname, IgnoredObjects);
                    schemaObject.GetInfo();
                }
            }
            // database security info
            ProcessDatabaseUsers();
            ProcessDatabaseSchemas();
        }

        /// <summary>
        /// Delete files where they no longer exist in database
        /// </summary>
        public void CheckForDeletedRecords()
        {
            // get a list exists objects
            string[] files = Directory.GetFiles(WorkingPath, "*", SearchOption.AllDirectories);
            string sql = "IF OBJECT_ID(@Name) IS NOT NULL PRINT 'Object exists' ELSE PRINT 'Object does not exist';";
            foreach (var filestr in files)
            {
                string nameonly = Path.GetFileNameWithoutExtension(filestr);
                string response = SqlMethods.ExecuteStringReturnCommand(DbConnectionStr, sql, new SqlParameter("Name", nameonly));
                if (response.Length > 0)
                {
                    if (response == "Object does not exist")
                        File.Delete(filestr);
                }
            }
        }

        public string[] EnumerateDbObjects(ObjectType dbObjectType)
        {
            string sql = "SELECT sobjects.name FROM sysobjects sobjects WHERE sobjects.xtype = @Type";
            IEnumerable<DataTable> datatables =
                SqlMethods.ExecuteDataTableReturnCommand(DbConnectionStr, sql, new SqlParameter("Type", dbObjectType.ToString()));
            if (datatables.Any() && datatables.First().Rows.Count > 0)
            {
                List<string> dbnames = datatables.First().AsEnumerable().Select(a => a[0].ToString()).ToList();
                foreach (string nametoignore in IgnoredObjects)
                    dbnames.Remove(nametoignore);
                return dbnames.ToArray();
            }
            return new string[0];
        }

        #region Folder Structure
        private void CreateStructureObject()
        {
            FolderStructure =
                new FolderInfo(DataSource,
                    new FolderInfo(DatabaseName,
                        new FolderInfo("Table", ObjectType.U),
                        new FolderInfo("View", ObjectType.V),
                        new FolderInfo("Programmability",
                            new FolderInfo("Stored Procedure", ObjectType.P),
                            new FolderInfo("Table-valued Functions", ObjectType.IF),
                            new FolderInfo("Scalar-valued Functions", ObjectType.FN)),
                        new FolderInfo("Security",
                            new FolderInfo("User"),
                            new FolderInfo("Roles"),
                            new FolderInfo("Schemas"))),
                    new FolderInfo("Security",
                        new FolderInfo("Logins"),
                        new FolderInfo("Server Roles")),
                    new FolderInfo("Server Objects",
                        new FolderInfo("Backup Devices"),
                        new FolderInfo("Linked Servers",
                            new FolderInfo("Providers")),
                        new FolderInfo("Triggers")),
                    new FolderInfo("Replication",
                        new FolderInfo("Local subscriptions")));
        }

        public string GetPath(ObjectType objectType)
        {
            GetFolderInfo("", FolderStructure, objectType);
            return WorkingPath + objectpath;
        }

        private string objectpath = string.Empty;
        private void GetFolderInfo(string str, FolderInfo folderInfo, ObjectType requestType)
        {
            str += @"\" + folderInfo.Name;
            if (folderInfo.ObjType == requestType)
                objectpath = str;
            if (folderInfo.ContainingFolders != null)
                foreach (var finfo in folderInfo.ContainingFolders)
                    GetFolderInfo(str, finfo, requestType);
        }

        public string GetPath(string name)
        {
            GetFolderInfo("", FolderStructure, name);
            return WorkingPath + objectpath;
        }

        private void GetFolderInfo(string str, FolderInfo folderInfo, string name)
        {
            str += @"\" + folderInfo.Name;
            if (folderInfo.Name == name)
                objectpath = str;
            if (folderInfo.ContainingFolders != null)
                foreach (var finfo in folderInfo.ContainingFolders)
                    GetFolderInfo(str, finfo, name);
        }

        private void CreateFolderStructure()
        {
            foreach (ObjectType objecttype in (ObjectType[])Enum.GetValues(typeof(ObjectType)))
            {
                objectpath = string.Empty;
                string typepath = GetPath(objecttype);
                if (!Directory.Exists(typepath))
                    Directory.CreateDirectory(typepath);
            }
        }

        private void ProcessDatabaseUsers()
        {
            string userpath = GetPath("User");
            if (!Directory.Exists(userpath))
                Directory.CreateDirectory(userpath);
            string[] usernames = GetDbUsers();
            var objectwriter = new ObjectWriter(userpath + @"\Users.txt");
            objectwriter.WriteLine("Users; ");
            foreach (string str in usernames)
            {
                objectwriter.WriteLine(str);
            }
        }

        private void ProcessDatabaseSchemas()
        {
            string schemapath = GetPath("Schemas");
            if (!Directory.Exists(schemapath))
                Directory.CreateDirectory(schemapath);
            string[] schemanames = GetSchemas();
            var objectwriter = new ObjectWriter(schemapath + @"\Schemas.txt");
            objectwriter.WriteLine("Roles; ");
            foreach (string str in schemanames)
            {
                objectwriter.WriteLine(str);
            }
        }

        public string[] GetDbUsers()
        {
            string sql = "SELECT name FROM sys.server_principals WHERE default_database_name LIKE @DbName ORDER BY default_database_name DESC;";
            IEnumerable<DataTable> datatables =
                SqlMethods.ExecuteDataTableReturnCommand(DbConnectionStr, sql, new SqlParameter("DbName", DatabaseName));
            if (datatables.Any() && datatables.First().Rows.Count > 0)
            {
                List<string> dbusers = datatables.First().AsEnumerable().Select(a => a[0].ToString()).ToList();
                return dbusers.ToArray();
            }
            return new string[] { "No database users" };
        }

        private string[] GetSchemas()
        {
            string sql = @"SELECT name FROM sys.schemas";
            IEnumerable<DataTable> datatables =
                SqlMethods.ExecuteDataTableReturnCommand(DbConnectionStr, sql);
            if (datatables.Any() && datatables.First().Rows.Count > 0)
            {
                List<string> dbschemas = datatables.First().AsEnumerable().Select(a => a[0].ToString()).ToList();
                return dbschemas.ToArray();
            }
            return new string [] { "No database schemas" };
        }
        #endregion
        
        public override string ToString()
        {
            return String.Format("Working Schema {0}, {1}", DbConnectionStr, WorkingPath);
        }
    }
}