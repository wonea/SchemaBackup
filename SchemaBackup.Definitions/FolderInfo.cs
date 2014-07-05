using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchemaBackup.Definitions;

namespace SchemaBackup.Definitions
{
    public class FolderInfo
    {
        public Nullable<ObjectType> ObjType;
        public string Name;
        public FolderInfo[] ContainingFolders;

        public FolderInfo(string name)
        {
            Name = name;
        }

        public FolderInfo(string name, params FolderInfo[] containingfolders)
        {
            Name = name;
            ContainingFolders = containingfolders;
        }

        public FolderInfo(string name, ObjectType objectType)
        {
            ObjType = objectType;
            Name = name;
        }

        public FolderInfo(string name, ObjectType objectType, params FolderInfo[] containingfolders)
            : this(name, objectType)
        {
            ContainingFolders = containingfolders;
        }
    }
}