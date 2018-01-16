using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.SQLite;

namespace GDBAccess
{
    class DatabaseManager : IDisposable
    {
        private SQLiteConnection GamesDB;
        public DatabaseManager(string FilePath)
        {
            string DatabaseSource = FilePath;
            GamesDB = new SQLiteConnection("Data Source=" + DatabaseSource + ";Version=3");
            GamesDB.Open();
        }


        public void Dispose()
        {
            if (GamesDB != null)
            {
                GamesDB.Dispose();
                GamesDB = null;
            }
        }

        public void AddToDB_Controller(string GameName, string SystemName, string Format)
        {
            int GameID = Convert.ToInt32(null);
            int SystemID = Convert.ToInt32(null);
            int FormatID = Convert.ToInt32(null);
            string DateAdded = DateTime.Now.ToString("yyyyMMddHHmmss");

            GameID = GetGameID(GameName);
            SystemID = GetSystemID(SystemName);
            FormatID = GetFormatID(Format);

            // Secondary check to ensure no garbage data is added.
            // Above functions do attempt to ensure no 0s are returned.
            if(GameID != 0 && SystemID != 0 && FormatID != 0)
            {
                AddGameSystem(GameID, SystemID, FormatID, DateAdded);
            }
        }

        int AddGame(string GameName)
        {
            int GameID = 0;
            SQLiteParameter Param_GameName = new SQLiteParameter("@GameName", GameName);

            SQLiteCommand SQL_Add_Game = GamesDB.CreateCommand();
            SQL_Add_Game.CommandText = "INSERT INTO Games (Game) VALUES (@GameName); select last_insert_rowid();";
            SQL_Add_Game.Parameters.Add(Param_GameName);
            GameID = (int)(long)SQL_Add_Game.ExecuteScalar();

            return GameID;
        }

        int AddSystem(string SystemName)
        {
            int SystemID = 0;
            SQLiteParameter Param_SystemName = new SQLiteParameter("@SystemName", SystemName);

            SQLiteCommand SQL_Add_System = GamesDB.CreateCommand();
            SQL_Add_System.CommandText = "INSERT INTO Systems (System) VALUES (@SystemName); select last_insert_rowid();";
            SQL_Add_System.Parameters.Add(Param_SystemName);
            SystemID = (int)(long)SQL_Add_System.ExecuteScalar();

            return SystemID;
        }

        void AddGameSystem(int GameID, int SystemID, int FormatID, string DateAdded)
        {
            SQLiteCommand SQL_Add_Entry = GamesDB.CreateCommand();
            SQL_Add_Entry.CommandText = "INSERT INTO GameSystem (GameID, SystemID, FormatID, DateAdded) Values (@GameID, @SystemID, @FormatID, @DateAdded)";
            SQL_Add_Entry.Parameters.Add(new SQLiteParameter("@GameID", GameID));
            SQL_Add_Entry.Parameters.Add(new SQLiteParameter("@SystemID", SystemID));
            SQL_Add_Entry.Parameters.Add(new SQLiteParameter("@FormatID", FormatID));
            SQL_Add_Entry.Parameters.Add(new SQLiteParameter("@DateAdded", DateAdded));

            SQL_Add_Entry.ExecuteNonQuery();
        }

        public List<List<string>> SearchDB(string Game_SearchTerm, string System_SearchTerm, string Format_SearchTerm)
        {
            SQLiteCommand SQL_Get_Rows = GamesDB.CreateCommand();
            SQL_Get_Rows.CommandText = 
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
                  ) WHERE Game LIKE @GameTerm AND Platform LIKE @SystemTerm AND Format LIKE @FormatTerm 
                  ORDER BY Game ASC";
            SQL_Get_Rows.Parameters.Add(new SQLiteParameter("@GameTerm", Game_SearchTerm));
            SQL_Get_Rows.Parameters.Add(new SQLiteParameter("@SystemTerm", System_SearchTerm));
            SQL_Get_Rows.Parameters.Add(new SQLiteParameter("@FormatTerm", Format_SearchTerm));

            SQLiteDataReader Reader = SQL_Get_Rows.ExecuteReader();

            DatabaseOutputFormatter Formatter = new DatabaseOutputFormatter();
            List<List<string>> FormattedQuery = Formatter.SortByGame(Reader);

            return FormattedQuery;
        }

        public bool DeleteRecord(string GameName, string SystemName, string FormatType)
        {
            int GameName_ID = GetGameID(GameName);
            int SystemName_ID = GetSystemID(SystemName);
            int FormatType_ID = GetFormatID(FormatType);

            SQLiteCommand SQL_Delete_Record = GamesDB.CreateCommand();
            SQL_Delete_Record.CommandText = "DELETE FROM GameSystem WHERE GameID=@GameName_ID AND SystemID=@SystemName_ID AND FormatID=@FormatType_ID";

            SQL_Delete_Record.Parameters.Add(new SQLiteParameter("@GameName_ID", GameName_ID));
            SQL_Delete_Record.Parameters.Add(new SQLiteParameter("@SystemName_ID", SystemName_ID));
            SQL_Delete_Record.Parameters.Add(new SQLiteParameter("@FormatType_ID", FormatType_ID));

            SQL_Delete_Record.ExecuteNonQuery();
            return true;
        }

        int GetGameID(string GameName)
        {
            SQLiteCommand SQL_Get_RowID = GamesDB.CreateCommand();
            SQL_Get_RowID.CommandText = "SELECT ID FROM Games WHERE Game=@GameName";
            SQL_Get_RowID.Parameters.Add(new SQLiteParameter("@GameName", GameName));
            SQLiteDataReader RowIDFetch = SQL_Get_RowID.ExecuteReader();

            int RowID = Convert.ToInt32(null);

            while (RowIDFetch.Read())
            {
                RowID = RowIDFetch.GetInt32(0);
                break;
            }

            if (RowID != Convert.ToInt32(null))
            {
                return RowID;
            }else
            {
                return AddGame(GameName);
            }  
        }

        // Fetches the RowID for the System Name
        // If System Name is not found in the database
        // the Name is added and the new RowID is returned
        int GetSystemID(string SystemName)
        {
            SQLiteCommand SQL_Get_RowID = GamesDB.CreateCommand();
            SQL_Get_RowID.CommandText = "SELECT ID FROM Systems WHERE System==@SystemName";
            SQL_Get_RowID.Parameters.Add(new SQLiteParameter("@SystemName", SystemName));
            SQLiteDataReader RowIDFetch = SQL_Get_RowID.ExecuteReader();

            int RowID = Convert.ToInt32(null);

            while (RowIDFetch.Read())
            {
                RowID = RowIDFetch.GetInt32(0);
                break;
            }

            if (RowID != Convert.ToInt32(null))
            {
                return RowID;
            }
            else
            {
                return AddSystem(SystemName);
            }
        }

        int GetFormatID(string Format)
        {
            int FormatID = 0;
            SQLiteCommand SQL_Get_FormatID = GamesDB.CreateCommand();

            SQL_Get_FormatID.CommandText = "SELECT ID FROM Format WHERE Type=@Format";
            SQL_Get_FormatID.Parameters.Add(new SQLiteParameter("@Format", Format));

            SQLiteDataReader Reader = SQL_Get_FormatID.ExecuteReader();
            while (Reader.Read())
            {
                FormatID = Reader.GetInt32(0);
                break;
            }

            return FormatID;
        }

        public void CloseDatabase()
        {
            GamesDB.Close();
            this.Dispose();
            GC.Collect();
        }
    }
}