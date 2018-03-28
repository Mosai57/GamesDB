using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDBAccess
{
    class ReportExporter
    {
        public string ReportFileOutput;
        public ReportExporter(List<ReportEntry> ReportResults)
        {
            FormatResults(ReportResults);
            ExportResults();
            OpenResults();
        }

        private void FormatResults(List<ReportEntry> ReportResults)
        {
            foreach(IEnumerable<ReportEntry> Line in ReportResults)
            {
                foreach(var Item in Line)
                {
                    ReportFileOutput += String.Format("{0,-10}", Item);
                }
                ReportFileOutput += "\n";
            }
        }
        
        private void ExportResults()
        {
            File.WriteAllText("Report.txt", ReportFileOutput);
        }

        private void OpenResults()
        {
            // System.Diagnostics.Process.Start(@"Report.txt");
        }
    }
}
