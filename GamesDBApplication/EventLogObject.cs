using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDBAccess
{
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
