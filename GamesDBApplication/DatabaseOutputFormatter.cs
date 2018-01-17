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

        public List<GameEntry> SortByGame(SQLiteDataReader RawQuery)
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

            List<GameEntry> QueryResults = ReturnQuery(OrderedSearchResults.ToList());

            return QueryResults;
        }

        private List<GameEntry> ReturnQuery(List<string> QueryResults)
        {
            List<GameEntry> QueryReturn = new List<GameEntry>();
            foreach (String entry in QueryResults)
            {
                string[] line = entry.Split('|');
                QueryReturn.Add(new GameEntry()
                {
                    Name = line[0],
                    SystemName = line[1],
                    FormatName = line[2],
                });
            }
            return QueryReturn;
        }

    }
}
