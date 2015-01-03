using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchemaBackup.Definitions;
using System.Data.SqlClient;
using System.Data;
using SchemaBackup.Core;

namespace SchemaBackup.Scheme
{
    public class Table : Schema
    {
        #region Properties
        public string TableType { get; set; }
        public string RowGuid { get; set; }
        public string DataLocatedOnFileGroup { get; set; }
        public IEnumerable<TableIndex> Indexes { get; set; }
        public IEnumerable<TableIdentity> Identities { get; set; }
        public IEnumerable<TableConstraint> Constraints { get; set; }
        public IEnumerable<TableColumn> Columns { get; set; }
        #endregion

        public Table(string dbConnectionStr, string workingFolder, string tableName)
            : base(dbConnectionStr, workingFolder, new string[1])
        {
            Name = tableName;
            Initialise();
        }

        public new void Initialise()
        {
            GetLastUpdate();
            GetInfo();
        }

        public new void GetInfo()
        {
            string sql = "sp_help @Name";
            IEnumerable<DataTable> datatables =
                SqlMethods.ExecuteDataTableReturnCommand(DbConnectionStr, sql, new SqlParameter("Name", Name));
            if (datatables.Any())
            {
                /*
                // basic data
                DataTable basicsDataTable = datatables.ElementAt(0);
                Owner = basicsDataTable.Rows[0]["Owner"].ToString();
                TableType = basicsDataTable.Rows[0]["Type"].ToString();
                Created = DateTime.Parse(basicsDataTable.Rows[0]["Created_datetime"].ToString());
                // columns
                List<TableColumn> columns = new List<TableColumn>();
                DataTable columnDataTable = datatables.ElementAt(1);
                int rowcount = 1;
                foreach (DataRow columnrow in columnDataTable.Rows)
                {
                    var tablecolumn = new TableColumn()
                    {
                        OrdinalPosition = rowcount,
                        Name = columnrow["Column_name"].ToString(),
                        DataType = columnrow["Type"].ToString(),
                        Computed = columnrow["Computed"].ToString(),
                        Length = int.Parse(columnrow["Length"].ToString()),
                        Precision = int.Parse(columnrow["Precision"].ToString()),
                        Scale = int.Parse(columnrow["Scale"].ToString()),
                        IsNullable = (columnrow["Nullable"].ToString() == "yes") ? true : false,
                        TrimTrailingBlanks = columnrow["TrimTrailingBlanks"].ToString(),
                        FixedLenNullInSource = columnrow["FixedLenNullInSource"].ToString(),
                        Collation = columnrow["Collation"].ToString()
                    };
                    columns.Add(tablecolumn);
                    rowcount += 1;
                }
                Columns = columns;
                // table identities
                List<TableIdentity> identites = new List<TableIdentity>();
                DataTable identityDataTable = datatables.ElementAt(2);
                foreach (DataRow identityrow in columnDataTable.Rows)
                {
                    var tableidentity = new TableIdentity()
                    {
                        Identity = identityrow["Identity"].ToString(),
                        Seed = int.Parse(identityrow["Seed"].ToString()),
                        Increment = int.Parse(identityrow["Increment"].ToString()),
                        NotForReplication = int.Parse(identityrow["Not For Replication"].ToString())
                    };
                    identites.Add(tableidentity);
                }
                Identities = identites;
                // rowguid
                DataTable rowguidDataTable = datatables.ElementAt(3);
                RowGuid = rowguidDataTable.Rows[0][0].ToString();
                // data location
                DataTable datalocationDataTable = datatables.ElementAt(4);
                DataLocatedOnFileGroup = datalocationDataTable.Rows[0][0].ToString();
                // indexes
                List<TableIndex> indexes = new List<TableIndex>();
                DataTable indexesDataTable = datatables.ElementAt(5);
                foreach (DataRow indexrow in indexesDataTable.Rows)
                {
                    var tableindex = new TableIndex()
                    {
                        Name = indexrow["index_name"].ToString(),
                        Description = indexrow["index_description"].ToString(),
                        IndexKey = indexrow["index_keys"].ToString()
                    };
                    indexes.Add(tableindex);
                }
                Indexes = indexes;
                // constraints
                List<TableConstraint> constraints = new List<TableConstraint>();
                DataTable constraintDataTable = datatables.ElementAt(6);
                for (int x = 0; x < constraintDataTable.Rows.Count; x++)
                {
                    
                }
                // foreign keys
                List<TableForeignKey> foreignkeys = new List<TableForeignKey>();
                DataTable foreignkeyDataTable = datatables.ElementAt(7);
                foreach (DataRow foreignkeyrow in foreignkeyDataTable.Rows)
                {
                    foreignkeys.Add(new TableForeignKey() { Name = foreignkeyrow[0].ToString() });
                }*/
            }
        }
    }

    public class TableColumn
    {
        public int OrdinalPosition { get; set; }
        public string Name { get; set; }
        public string DataType { get; set; }
        public string Computed { get; set; }
        public int Length { get; set; }
        public int Precision { get; set; }
        public Nullable<int> Scale { get; set; }
        public bool IsNullable { get; set; }
        public string TrimTrailingBlanks { get; set; }
        public string FixedLenNullInSource { get; set; }
        public string Collation { get; set; }
    }

    public class TableIdentity
    {
        public string Identity { get; set; }
        public int Seed { get; set; }
        public int Increment { get; set; }
        public int NotForReplication { get; set; }
    }

    public class TableIndex
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string IndexKey { get; set; }
    }

    public class TableConstraint
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsUnique { get; set; }
        public bool IsPrimary { get; set; }
    }

    public class TableForeignKey
    {
        public string Name { get; set; }
    }
}