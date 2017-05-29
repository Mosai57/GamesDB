using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GamesDBApplication
{
    class Export
    {
        public void ExportCSV(List<string> exportList, string filePath)
        {
            Regex splitter = new Regex(@"\s/\s");

            System.IO.StreamWriter csvFile = new System.IO.StreamWriter(filePath);
            csvFile.WriteLine("Game Name,System Name,Format Type");

            foreach (string line in exportList)
            {
                string[] lineInfo = splitter.Split(line).ToArray();

                string recordLineOut = "\"" + lineInfo[0] + "\"" + ","
                                     + "\"" + lineInfo[1] + "\"" + ","
                                     + "\"" + lineInfo[2] + "\"";

                csvFile.WriteLine(recordLineOut);
            }
            csvFile.Close();
        }
    }
}
