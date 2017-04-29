using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows.Forms;

namespace GamesDBApplication
{
    class DatabaseManager
    {
        private SQLiteConnection GamesDB;
        public DatabaseManager()
        {
            GamesDB = new SQLiteConnection("Data Source=C:\\Users\\Mosai\\db\\Games.sdb;Version=3");
            GamesDB.Open();
        }

        ~DatabaseManager()
        {
            GamesDB.Close();
        }

        public void AddToDB_Controller(string GameName, string SystemName, string Format)
        {
            int GameID = Convert.ToInt32(null);
            int SystemID = Convert.ToInt32(null);
            int FormatID = Convert.ToInt32(null);

            GameID = CheckExists("Games", "Game", GameName);
            if (GameID == 0)
            {
                GameID = AddGame(GameName);
            }

            SystemID = CheckExists("Systems", "System", SystemName);
            if (SystemID == 0)
            {
                SystemID = AddSystem(SystemName);
            }

            FormatID = DetermineFormatID(Format);

            if(GameID != null && SystemID != null && FormatID != null)
            {
                AddGameSystem(GameID, SystemID, FormatID);
            }
        }

        int CheckExists(string Table, string SearchField, string SearchTerm)
        {
            string SQL_COMMAND_SEARCH = ("SELECT COUNT(*) FROM " + Table + " WHERE " + SearchField + "==\"" + SearchTerm + "\"");
            SQLiteCommand CheckExistsCommand = new SQLiteCommand(SQL_COMMAND_SEARCH, GamesDB);
            SQLiteDataReader ExistReader = CheckExistsCommand.ExecuteReader();

            int Entities = 0;
            while (ExistReader.Read())
            {
                Entities = ExistReader.GetInt32(0);
            }
            if (Entities > 0)
            {
                // If the entity exists, fetch the row ID for it for later use.
                string SQL_GET_ROWID = ("SELECT ID FROM " + Table + " WHERE " + SearchField + "==\"" + SearchTerm + "\"");
                SQLiteCommand GetRowID = new SQLiteCommand(SQL_GET_ROWID, GamesDB);
                SQLiteDataReader RowIDFetch = GetRowID.ExecuteReader();

                int RowID = 0;
                while (RowIDFetch.Read())
                {
                    RowID = RowIDFetch.GetInt32(0);
                }

                return RowID;
            }
            else
            {
                return 0;
            }
        }

        int AddGame(string GameName)
        {
            int GameID = 0;

            string SQL_Command_GameTable = "INSERT INTO Games (Game) VALUES (\"" + GameName + "\");";
            SQLiteCommand GameTableCommand = new SQLiteCommand(SQL_Command_GameTable, GamesDB);
            GameTableCommand.ExecuteNonQuery();

            string SQL_Command_GetGameID = "SELECT ID FROM Games WHERE Game==\"" + GameName + "\"";
            GameTableCommand = new SQLiteCommand(SQL_Command_GetGameID, GamesDB);
            SQLiteDataReader GameTableReader = GameTableCommand.ExecuteReader();
            while (GameTableReader.Read())
            {
                GameID = GameTableReader.GetInt32(0);
                break;
            }

            return GameID;
        }

        int AddSystem(string SystemName)
        {
            int SystemID = 0;

            string SQL_Command_SystemTable = "INSERT INTO Systems (System) VALUES (\"" + SystemName + "\");";
            SQLiteCommand SysTableCommand = new SQLiteCommand(SQL_Command_SystemTable, GamesDB);
            SysTableCommand.ExecuteNonQuery();

            string SQL_Command_GetSystemID = "SELECT ID FROM Systems WHERE System==\"" + SystemName + "\"";
            SysTableCommand = new SQLiteCommand(SQL_Command_GetSystemID, GamesDB);
            SQLiteDataReader SysTableReader = SysTableCommand.ExecuteReader();
            while (SysTableReader.Read())
            {
                SystemID = SysTableReader.GetInt32(0);
                break;
            }

            return SystemID;
        }

        void AddGameSystem(int GameID, int SystemID, int FormatID)
        {
            string SQL_Command_GameSystemTable = "INSERT INTO GameSystem (GameID, SystemID, FormatID) Values (" + GameID + ", " + SystemID + ", " + FormatID + ")";
            SQLiteCommand GameSysCommand = new SQLiteCommand(SQL_Command_GameSystemTable, GamesDB);
            GameSysCommand.ExecuteNonQuery();
        }

        public List<string> SearchDB(string Game_SearchTerm, string System_SearchTerm, string Format_SearchTerm)
        {
            List<string> Search_Results = new List<string>();
            string SQL_Search_Command = 
                @"SELECT Game, Platform, Format
                  FROM(
                    SELECT
                        Games.Game AS Game,
                        Systems.System AS Platform,
                        Format.Type AS Format
                    FROM
                        GameSystem
                    INNER JOIN Games ON GameSystem.GameID = Games.ID
                    INNER JOIN Systems ON GameSystem.SystemID = Systems.ID
                    INNER JOIN Format ON GameSystem.FormatID = Format.ID
                  ) WHERE "
                  + "Game LIKE \"" + Game_SearchTerm + "\" AND Platform LIKE \"" + System_SearchTerm + "\" AND Format LIKE \"" + Format_SearchTerm + "\" ORDER BY Game ASC";
            SQLiteCommand GetRows = new SQLiteCommand(SQL_Search_Command, GamesDB);
            SQLiteDataReader MatchReader = GetRows.ExecuteReader();
            while (MatchReader.Read())
            {
                Search_Results.Add(MatchReader.GetString(0) + " / " + MatchReader.GetString(1) + " / " + MatchReader.GetString(2));
            }

            return Search_Results;
        }

        // We know that theres only two possible values for Format, so we're going to play a bit dirty
        int DetermineFormatID(string Format)
        {
            switch (Format)
            {
                case "Physical":
                    return 1;
                case "Digital":
                    return 2;
                default:
                    return 0;
            }
        }
    }
}
