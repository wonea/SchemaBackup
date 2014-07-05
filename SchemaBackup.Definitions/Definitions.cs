using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SchemaBackup.Definitions
{
    public abstract class SchemaDefintion
    {
        public string DbConnectionStr;
        public string WorkingPath;
        public FolderInfo FolderStructure;
        public string[] IgnoredObjects;
    }

    public abstract class ObjectDefinition : SchemaDefintion
    {
        public string Name;
        public DateTime LastUpdated;
        public DateTime Created;

        public string MachineName;
        public string UserName;
        /// <summary>
        /// Actual SQL
        /// </summary>
        public string RawSql;
        public ObjectType ObjType;
        public string Owner;
        public string FileName;
    }

    public interface ICommon
    {
        void Initialise();
        void GetInfo();
        void GetRaw();
    }
}
