using System.IO;
using System.Collections.Generic;

namespace GDBAccess
{
    class Export
    {
        public void ExportCSV(List<GameEntry> exportList, string filePath)
        {
            StreamWriter csvFile = new StreamWriter(filePath);
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
