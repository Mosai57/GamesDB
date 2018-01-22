using System;
using System.IO;

namespace GDBAccess
{
    /// <summary>
    /// Logs to specified log files in the application's storage path.
    /// Interfaces with LogObjects
    /// </summary>
    public class Logger
    {
        private string BasePath;
        private string EventLogPath;
        private string TransactionLogPath;

        /// <summary>
        /// Initializes the Logger object with the filepath of the application's storage directory.
        /// </summary>
        /// <param name="filepath"></param>
        public Logger(string filepath)
        {
            BasePath = filepath;

            EventLogPath = Path.Combine(BasePath, @"log\events.log");
            TransactionLogPath = Path.Combine(BasePath, @"log\transactions.log");
        }

        /// <summary>
        /// Logs to the proper log file specified by the type of LogObject passed.
        /// </summary>
        /// <param name="logobj"></param>
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

        /// <summary>
        /// Returns the path for the file of the specified LogObject type
        /// </summary>
        /// <param name="logobj"></param>
        /// <returns></returns>
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
