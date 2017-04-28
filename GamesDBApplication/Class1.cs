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
        }

        public void AddToDB_Controller(string GameName, string SystemName, string Format)
        {
            int GameID = Convert.ToInt32(null);
            int SystemID = Convert.ToInt32(null);
            int FormatID = Convert.ToInt32(null);

            GamesDB.Open();

            if (CheckExists("Games", "Game", GameName) == 0)
            {
                GameID = AddGame(GameName);
            }
            if (CheckExists("Systems", "System", SystemName) == 0)
            {
                SystemID = AddSystem(SystemName);
            }
            FormatID = DetermineFormatID(Format);

            MessageBox.Show("Verified all values. Adding to main database");
            if(GameID != null && SystemID != null && FormatID != null)
            {
                AddGameSystem(GameID, SystemID, FormatID);
            }
          
            GamesDB.Close();
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
                return 1;
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
                GameID = Convert.ToInt32(GameTableReader["ID"]);
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
                SystemID = Convert.ToInt32(SysTableReader["ID"]);
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
