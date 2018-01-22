using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
