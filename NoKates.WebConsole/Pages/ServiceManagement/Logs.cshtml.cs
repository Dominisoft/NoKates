using System;
using System.Collections.Generic;
using System.Diagnostics;
using NoKates.Common.Models;
using NoKates.WebConsole.Clients;
using NoKates.WebConsole.Pages.Shared;

namespace NoKates.WebConsole.Pages.ServiceManagement
{
    public class LogsModel : AuthenticatedPage
    {
        private readonly INoKatesCoreClient _adminClient;
        public List<LogEntry> LogEntries;

        public LogsModel(INoKatesCoreClient adminClient)
        {
            _adminClient = adminClient;
            LogEntries = new List<LogEntry>();
        }

        public override void LoadData(string token)
        {
            var serviceName = Request.Query["ServiceName"];

            if (serviceName == "System_Event_Log")
            {
                LoadEventLog();
                return;
            }
            try
            {
                LogEntries = _adminClient.GetLogEntries(serviceName,token);
            }
            catch (Exception e)
            {
                Response.Redirect("./Logs?ServiceName=System_Event_Log");
            }
        }

        private void LoadEventLog()
        {
            var eventLogName = "Application";
            LogEntries = new List<LogEntry>();
            
            var eventLog = new EventLog();
            eventLog.Log = eventLogName;
            var logs = eventLog.Entries;
            foreach (EventLogEntry eventLogEntry in logs)
            {
                if (
                      eventLogEntry.TimeGenerated > DateTime.Now.AddHours(-4)
                    && eventLogEntry.Source == ".NET Runtime"
                    && eventLogEntry.EntryType == EventLogEntryType.Error
                   )
                    LogEntries.Add(new LogEntry
                    {
                        Date = eventLogEntry.TimeGenerated,
                        Message = eventLogEntry.Message,
                        Source = eventLogEntry.Source
                    });
            }
        }


    }
}
