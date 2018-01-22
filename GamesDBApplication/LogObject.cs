using System;

namespace GDBAccess
{
    /// <summary>
    /// Base class LogObject. 
    /// Child classes specify the type of logging it does and formats its information to be properly logged by the Logger class.
    /// </summary>
    public class LogObject
    {
        public string LogType { get; set; }
        public string Log { get; set; }
        public virtual void CreateLog() { }
        public virtual void CreateLog(object obj) { }
    }
}
