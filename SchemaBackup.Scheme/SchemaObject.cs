using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using SchemaBackup.Definitions;
using Microsoft.Data.Schema.ScriptDom.Sql;
using Microsoft.Data.Schema.ScriptDom;
using System.IO;
using SchemaBackup.Core;

namespace SchemaBackup.Scheme
{
    public class SchemaObject : Schema
    {
        private ObjectWriter ObjectWriter;

        public SchemaObject(string databaseConnectionStr, string workingPath, ObjectType objectType, string name, string[] ignoredObjects)
            : base(databaseConnectionStr, workingPath, ignoredObjects)
        {
            ObjType = objectType;
            Name = name;
            ObjectWriter = new ObjectWriter(FileName);
            GetLastUpdate();
        }

        #region Properties
        public new string FileName
        {
            get { return String.Format(@"{0}\{1}.txt", GetPath(ObjType), Name); }
        }

        public string Summary
        {
            get { return string.Format("Object Name: {0}, Last Update: {1}, Username: {2}, Machine: {3}", Name, LastUpdated, UserName, MachineName); }
        }
        #endregion

        public void GetObjectDefinitions()
        {
            string sql = SqlQueries.GetObjectDefinitions;
            IEnumerable<DataTable> datatables =
                SqlMethods.ExecuteDataTableReturnCommand(DbConnectionStr, sql, new SqlParameter("Name", Name));
            if (datatables.Any() && datatables.First().Rows.Count > 0)
            {
                ObjectWriter.WriteDataTable(datatables.First(), false);
            }
        }

        public new void GetInfo()
        {
            // write summary block
            ObjectWriter.WriteLine(String.Format("Object Name: {0}", Name));
            ObjectWriter.WriteLine(String.Format("Last Update: {0}", LastUpdated));
            ObjectWriter.WriteLine(String.Format("Updated By (username): {0}", Name));
            ObjectWriter.WriteLine(String.Format("Updated From: {0}", MachineName));
            ObjectWriter.WriteLine();
            ObjectWriter.WriteLine("Object Definition :");
            string sql = "sp_help @Name";
            IEnumerable<DataTable> datatables =
                SqlMethods.ExecuteDataTableReturnCommand(DbConnectionStr, sql, new SqlParameter("Name", Name));
            if (datatables.Any())
            {
                foreach (DataTable datatable in datatables)
                {
                    ObjectWriter.WriteDataTable(datatable, false);
                }
            }
            // depending on object decide how to get the raw SQL
            ObjectWriter.WriteLine("Raw SQL :");
            GetRaw();
        }

        private void GetRaw()
        {
            if (ObjType == ObjectType.U)
            {
                string sql = SqlQueries.GetRaw;
                IEnumerable<DataTable> datatables =
                    SqlMethods.ExecuteDataTableReturnCommand(DbConnectionStr, sql, new SqlParameter("Name", Name));
                if (datatables.Any() && datatables.First().Rows.Count > 0)
                {
                    RawSql   = datatables.First().Rows[0][0].ToString();
                }
            }
            else
            {
                string sql = "sp_helptext @Name";
                IEnumerable<DataTable> datatables =
                    SqlMethods.ExecuteDataTableReturnCommand(DbConnectionStr, sql, new SqlParameter("Name", Name));
                if (datatables.Any() && datatables.First().Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (DataRow row in datatables.First().Rows)
                    {
                        sb.Append(row[0].ToString());
                    }
                    RawSql = sb.ToString();
                }
            }
            ObjectWriter.WriteLine(FormatSQL());
        }

        public string FormatSQL()
        {
            TSql100Parser _parser;
            Sql100ScriptGenerator _scriptGen;
            IScriptFragment fragment;
            IList<ParseError> errors;

             bool fQuotedIdenfifiers = false;
            _parser = new TSql100Parser(fQuotedIdenfifiers);
            SqlScriptGeneratorOptions options = new SqlScriptGeneratorOptions()
            {
                SqlVersion = SqlVersion.Sql100,
                KeywordCasing = KeywordCasing.Uppercase
            };
            _scriptGen = new Sql100ScriptGenerator(options);
            using (StringReader sr = new StringReader(RawSql))
            {
                fragment = _parser.Parse(sr, out errors);
            }

            String script;
            _scriptGen.GenerateScript(fragment, out script);
            return script;
        }

        public override string ToString()
        {
            return Summary;
        }
    }
}