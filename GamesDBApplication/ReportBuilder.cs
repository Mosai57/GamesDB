using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDBAccess
{
    class ReportContents
    {
        public int ColumnCount { get; set; }
        public string SystemParameters { get; set; }
        public string FormatParameters { get; set; }
        public string ColumnParameters { get; set; }
        public string ReportQuery { get; set; }
    }

    class ReportBuilder
    {
        List<string> Systems = new List<string>();
        List<string> Formats = new List<string>();
        List<string> Columns = new List<string>();
        
        string SystemParameter;
        string FormatParameter;
        string ColumnParameter;

        string ReportQuery;

        public ReportContents ReportPackage;

        public ReportBuilder(List<string> SystemsList, List<string> FormatList, List<string> ColumnList)
        {
            Systems = SystemsList;
            Formats = FormatList;
            Columns = ColumnList;

            BuildSystemParameter();
            BuildFormatParameter();
            BuildColumnParameter();

            BuildReport();

            PackageReport();
        }

        private void BuildSystemParameter()
        {
            for(int ctr = 0; ctr < Systems.Count; ctr++)
            {
                Systems[ctr] = "\"" + Systems[ctr] + "\"";
            }
            SystemParameter = String.Join(", ", Systems.ToArray());
        }

        private void BuildFormatParameter()
        {
            for(int ctr = 0; ctr < Formats.Count; ctr++)
            {
                Formats[ctr] = "\"" + Formats[ctr] + "\"";
            }
            FormatParameter = String.Join(", ", Formats.ToArray());
        }

        private void BuildColumnParameter()
        {
            ColumnParameter = String.Join(", ", Columns.ToArray());
        }

        private void BuildReport()
        {
            ReportQuery =
              @"SELECT @Columns
                FROM (
                    SELECT
                        Games.Game AS Game,
                        Systems.System AS System,
                        Format.Type AS Format,
                        DateAdded AS Date
                    FROM
                        GameSystem
                    INNER JOIN Games ON GameSystem.GameID = Games.ID
                    INNER JOIN Systems ON GameSystem.SystemID = Systems.ID
                    INNER JOIN Format ON GameSystem.FormatID = Format.ID
                ) WHERE System IN (@Systems) AND Format IN (@Formats)
                ORDER BY Game COLLATE library ASC";
        }

        private void PackageReport()
        {
            ReportPackage = new ReportContents()
            {
                ColumnCount = Columns.Count,
                SystemParameters = SystemParameter,
                FormatParameters = FormatParameter,
                ColumnParameters = ColumnParameter,
                ReportQuery = ReportQuery
            };
        }
    }
}
