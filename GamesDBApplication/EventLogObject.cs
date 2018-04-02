namespace GDBAccess
{
    /// <summary>
    /// Logs an event
    /// </summary>
    class EventLogObject : LogObject
    {
        public EventLogObject(object obj)
        {
            LogType = "E";
            CreateLog(obj);
        }

        public override void CreateLog(object eventOutput)
        {
            Log = eventOutput.ToString().Replace(System.Environment.NewLine, " ");
        }
    }
}
