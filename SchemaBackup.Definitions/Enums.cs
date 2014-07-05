using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace SchemaBackup.Definitions
{
    /// <summary>
    /// Partially based Database Object Type
    /// http://msdn.microsoft.com/en-gb/library/ms190324.aspx
    /// </summary>
    public enum ObjectType
    {
        /// <summary>
        /// Table
        /// </summary>
        [Description("Table")]
        U,
        /// <summary>
        /// Stored Procedure
        /// </summary>
        [Description("Stored Procedure")]
        P,
        /// <summary>
        /// View
        /// </summary>
        [Description("View")]
        V,
        /// <summary>
        /// Table-valued Functions
        /// </summary>
        [Description("Table-valued Functions")]
        IF,
        [Description("Scalar-valed Functions")]
        FN,

        DatabaseUser
    }


/*  public enum ObjectType
    {
        Schema,
        Table,
        View,
        StoredProcedure,
        User,
        LinkedServer,
        Role
        Trigger
    }*/
}
