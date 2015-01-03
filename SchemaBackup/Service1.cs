using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using SchemaBackup.Service.Properties;
using SchemaBackup.Core;
using Quartz;
using Quartz.Impl;
using SchemaBackup.Definitions;

namespace SchemaBackup
{
    public partial class Service1 : ServiceBase
    {
        public string[] DbConnectionStrings;
        public SettingsSerialisation SettingsSerialisation;
        IScheduler sched;

        public Service1()
        {
            InitializeComponent();
            // load test connection strings
            string[] strArray = new string[Settings.Default.ConnectionStrings.Count];
            DbConnectionStrings.CopyTo(strArray, 0);
            // load settings
            SettingsSerialisation = new SettingsSerialisation(Settings.Default.SettingFileLocation);
            SettingsSerialisation.Load();
            // setup scheduler
            LoadSchedulingDetails();
        }

        public void LoadSchedulingDetails()
        {
            // load scheduling details
            ISchedulerFactory sf = new StdSchedulerFactory();
            sched = sf.GetScheduler();

            DateTimeOffset runTime = DateBuilder.EvenMinuteDate(DateTime.UtcNow);
            DateTimeOffset startTime = DateBuilder.NextGivenSecondDate(null, 10);
            const string groupname = "SchemaCopiers";
            IJobDetail job = JobBuilder.Create<SchemaJob>()
                    .WithIdentity("SchemaCopyJob", groupname)
                    .Build();
            // set up triggers for each schema
            var triggers = new Quartz.Collection.HashSet<ITrigger>();
            foreach (var setting in SettingsSerialisation.SchemaSettings.Settings)
            {    
                // build trigger name
                string triggername = String.Format("{0}_trigger", setting.Name);
                // build and add trigger
                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity(triggername, groupname)
                    .StartAt(runTime)
                    .WithCronSchedule(setting.CheckFrequency.Frequency) //"5 0/1 * * * ?")
                    .Build();
                triggers.Add(trigger);
            }
            sched.ScheduleJob(job, triggers, true);
        }

        protected override void OnStart(string[] args)
        {
            sched.Start();
        }

        protected override void OnStop()
        {
            sched.Shutdown();
            SettingsSerialisation.Dispose();
        }   
    }

    public class SchemaJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            // load job to work with
            
            // get schema setting
            //SchemaSetting
        }

        public void ProcessSchema(SchemaSetting schemaSetting)
        {

        }

        public void Dispose()
        {
            
        }
    }
}
