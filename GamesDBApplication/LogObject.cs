namespace GDBAccess
{
    public class LogObject
    {
        public string LogType { get; set; }
        public string Log { get; set; }
        public virtual void CreateLog() { }
        public virtual void CreateLog(object obj) { }
    }
}
