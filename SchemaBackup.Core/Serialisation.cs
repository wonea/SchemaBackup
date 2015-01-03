using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchemaBackup.Definitions;
using System.Xml.Serialization;
using System.IO;

namespace SchemaBackup.Core
{
    public class SettingsSerialisation : IDisposable
    {
        private string _path;
        public SchemaSettings SchemaSettings;

        public SettingsSerialisation(string path)
        {
            _path = path;
        }

        public void Load()
        {
            string settingsfile = LoadSettings();
            if (!string.IsNullOrEmpty(settingsfile))
                SchemaSettings = LoadFromXMLString(settingsfile);
        }

        public void LoadTestData()
        {
            SchemaSettings = ExampleObjects.ExampleObjects.SchemaSettings;
        }

        public void Save()
        {
            string savestr = ObjectToXml(SchemaSettings);
            SaveSettings(savestr);
        }

        private string LoadSettings()
        {
            try
            {
                // check if file exists
                if (!File.Exists(_path))
                    return null;
                // attempt to load file
                using (FileStream filestream = File.OpenRead(_path))
                {
                    StreamReader streamreader = new StreamReader(filestream);
                    string readstr = streamreader.ReadToEnd();
                    return readstr;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void SaveSettings(string saveStr)
        {
            try
            {
                if (File.Exists(_path))
                {
                    File.Delete(_path);
                }
                StreamWriter streamwriter = File.CreateText(_path);
                // TODO: this will break when string gets really long
                streamwriter.Write(saveStr);
            }
            catch (Exception) { }
        }

        private string ObjectToXml(SchemaSettings value)
        {
            var stringwriter = new System.IO.StringWriter();
            var serializer = new XmlSerializer(this.GetType());
            serializer.Serialize(stringwriter, value);
            return stringwriter.ToString();
        }

        private static SchemaSettings LoadFromXMLString(string xmlText)
        {
            var stringReader = new System.IO.StringReader(xmlText);
            var serializer = new XmlSerializer(typeof(SettingsSerialisation));
            return serializer.Deserialize(stringReader) as SchemaSettings;
        }

        public void Dispose()
        {
            SchemaSettings = null;
        }
    }
}