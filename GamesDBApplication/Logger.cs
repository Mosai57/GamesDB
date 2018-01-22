using System;
using System.IO;

namespace GDBAccess
{
    public class Logger
    {
        private string BasePath;
        private string EventLogPath;
        private string TransactionLogPath;

        public Logger(string filepath)
        {
            BasePath = filepath;

            EventLogPath = Path.Combine(BasePath, @"log\events.log");
            TransactionLogPath = Path.Combine(BasePath, @"log\transactions.log");
        }

        public void Log(LogObject logobj)
        {
            string typePath = GetLogType(logobj);
            string LogOutput = string.Concat(GetTimestamp(), " | ", logobj.Log, '\n');
            using (StreamWriter w = File.AppendText(typePath))
            {
                w.WriteLine(LogOutput);
            }
        }

        private string GetTimestamp()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        private string GetLogType(LogObject logobj)
        {
            switch (logobj.LogType)
            {
                case "E": return EventLogPath;
                case "T": return TransactionLogPath;
                default: return null;
            }
        }
    }
}
