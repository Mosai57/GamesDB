using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GDBAccess
{
    class Export
    {
        public void ExportCSV(List<GameEntry> exportList, string filePath)
        {
            System.IO.StreamWriter csvFile = new System.IO.StreamWriter(filePath);
            csvFile.WriteLine("Game Name,System Name,Format Type");

            foreach (GameEntry entry in exportList)
            {
                string recordLineOut = "\"" + entry.Name + "\"" + ","
                                     + "\"" + entry.SystemName + "\"" + ","
                                     + "\"" + entry.FormatName + "\"";

                csvFile.WriteLine(recordLineOut);
            }
            csvFile.Close();
        }
    }
}
