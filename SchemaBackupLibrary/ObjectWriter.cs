using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;

namespace SchemaBackup.Scheme
{
    public class ObjectWriter
    {
        private string _FileName;
        private TextWriter _TextWriter;

        public ObjectWriter(string fileName)
        {
            _FileName = fileName;
            if (File.Exists(fileName))
            {
                File.Delete(_FileName);
                _TextWriter = File.CreateText(_FileName);
            }
            else
                _TextWriter = File.AppendText(_FileName);
        }

        public void Write(char c)
        {
            _TextWriter.Write(c);
        }

        public void Write(string str)
        {
            _TextWriter.Write(str);
        }

        public void WriteLine()
        {
            _TextWriter.WriteLine(string.Empty);
        }

        public void WriteLine(string str)
        {
            // split lines otherwise we might reach the max line size of 1631
            string[] lines = str.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            foreach (string linestr in lines)
            {
                _TextWriter.WriteLine(linestr);
                _TextWriter.Flush();
            }
        }

        // Repurposed from http://sandunangelo.blogspot.ch/2010/08/display-datatable-in-console-via.html
        public void WriteDataTable(DataTable dt, bool displayTableNames)
        {
            int columnWidth = 25; //must be >5
            int tableWidth = (columnWidth * dt.Columns.Count) + dt.Columns.Count;
            if (displayTableNames)
            {
                WriteLine("");
                WriteLine("Table name : " + dt.TableName + "\n");
            }

            #region PRINT THE TABLE HEADER

            DrawHorizontalSeperator(tableWidth, '=');
            Write("|");

            foreach (DataColumn column in dt.Columns)
            {
                string name = (" " + column.ColumnName + " ").PadRight(columnWidth);
                Write(name + "|");
            }
            WriteLine("");
            DrawHorizontalSeperator(tableWidth, '=');

            #endregion

            #region PRINTING DATA ROWS

            foreach (DataRow row in dt.Rows)
            {
                Write("|");
                foreach (DataColumn column in dt.Columns)
                {
                    string value = (" " + GetShortString(row[column.ColumnName].ToString(), columnWidth) + " ").PadRight(columnWidth);
                    Write(value + "|");
                }
                WriteLine("");
                DrawHorizontalSeperator(tableWidth, '-');
            }

            #endregion

            WriteLine("");
        }

        private void DrawHorizontalSeperator(int width, char seperator)
        {
            for (int counter = 0; counter <= width; counter++)
            {
                Write(seperator);
            }
            WriteLine("");
        }

        private string GetShortString(string text, int length)
        {
            if (text.Length >= length - 1)
            {
                string shortText = text.Substring(0, length - 5) + "...";
                return shortText;
            }
            else
            {
                return text;
            }
        }
    }
}
