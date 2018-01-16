using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SQLite;
using System.Text;
using System.Threading.Tasks;

namespace GDBAccess
{
    class DatabaseOutputFormatter
    {
        // This class deals with formatting the query output we get from
        // the SearchDB method in DatabaseManager. By standard I will be
        // using the name RawQuery for any query info passed to any method
        // in this class.
        //
        // Another note is any RawQuery passed to this class will be formatted as
        // follows:
        //      RawQuery[0] == GameName
        //      RawQuery[1] == SystemName
        //      RawQuery[2] == FormatType

        public List<List<string>> SortByGame(SQLiteDataReader RawQuery)
        {
            List<string> FormattedQuery = new List<string>();
            while (RawQuery.Read())
            {
                FormattedQuery.Add(RawQuery.GetString(0) + "|" + RawQuery.GetString(1) + "|" + RawQuery.GetString(2));
            }

            // Properly handle cases where the game name starts with "A " or "The "
            IEnumerable<string> OrderedSearchResults = FormattedQuery.OrderBy(s =>
                s.StartsWith("A ", StringComparison.OrdinalIgnoreCase) || s.StartsWith("The ", StringComparison.OrdinalIgnoreCase) ?
                s.Substring(s.IndexOf(" ") + 1) :
                s);

            List<List<string>> QueryResults = ReturnQuery(OrderedSearchResults.ToList<string>());

            return QueryResults;
        }

        private List<List<string>> ReturnQuery(List<string> QueryResults)
        {
            List<List<string>> QueryReturn = new List<List<string>>();
            foreach(String entry in QueryResults)
            {
                List<string> record = new List<string>();
                string[] line = entry.Split('|');
                foreach(var piece in line)
                {
                    record.Add(piece);
                }
                QueryReturn.Add(record);
            }
            return QueryReturn;
        }

    }
}
