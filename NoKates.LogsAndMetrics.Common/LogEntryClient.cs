using System;
using NoKates.Common.Infrastructure.Client;
using LogEntryDto = NoKates.LogsAndMetrics.Common.DataTransfer.LogEntryDto;

namespace NoKates.LogsAndMetrics.Common
{
    public interface ILogEntryClient:IBaseClient<LogEntryDto>
    {

    }
    public class LogEntryClient:BaseClient<LogEntryDto>, ILogEntryClient
    {
        public LogEntryClient(string baseUrl)
        {
            BaseUrl = baseUrl + "\\Log";
        }
        public new LogEntryDto Update(LogEntryDto entry)
            => throw new Exception("Not Allowed");
        public new bool Delete(LogEntryDto entry)
            => throw new Exception("Not Allowed");
        public new LogEntryDto Update(LogEntryDto entry,string token)
            => throw new Exception("Not Allowed");
        public new bool Delete(LogEntryDto entry,string token)
            => throw new Exception("Not Allowed");
    }
}
