using System.IO;
using System.Collections.Generic;

namespace GDBAccess
{
    /// <summary>
    /// Exports to supported file formats.
    /// </summary>
    class Export
    {
        /// <summary>
        /// Exports to CSV
        /// </summary>
        /// <param name="exportList"></param>
        /// <param name="filePath"></param>
        public void CSV(List<GameEntry> exportList, string filePath)
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
