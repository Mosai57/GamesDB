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
            GamesDB.BindFunction(
                new SQLiteFunctionAttribute() { FuncType = FunctionType.Collation, Name = "library", },
                new LibraryCollation()
                );
        }

        class LibraryCollation : SQLiteFunction
        {
            public LibraryCollation() { }

            /// <summary>
            /// Finds the first whitespace character starting at <paramref name="index"/>, returning -1 if the remainder of the string
            /// does not contain whitespace characters.
            /// </summary>
            /// <param name="s"></param>
            /// <param name="index"></param>
            /// <returns></returns>
            static int FindWhitespace(string s, int index)
            {
                if (index < 0) return index;
                for (; index < s.Length; index++)
                    if (char.IsWhiteSpace(s, index)) return index;
                return -1;
            }

            /// <summary>
            /// Finds the first non-whitespace character starting at <paramref name="index"/>, returning -1 if the remainder of the string
            /// is composed of whitespace characters.
            /// </summary>
            /// <param name="s"></param>
            /// <param name="index"></param>
            /// <returns></returns>
            static int FindNonWhitespace(string s, int index)
            {
                if (index < 0) return index;
                for (; index < s.Length; index++)
                    if (!char.IsWhiteSpace(s, index)) return index;
                return -1;
            }

            /// <summary>
            /// Finds the first non-space character that occurs after the first space character.
            /// </summary>
            /// <param name="s"></param>
            /// <returns></returns>
            static int FindSecondWord(string s)
            {
                int word1_start = FindNonWhitespace(s, 0);
                int word1_stop = FindWhitespace(s, word1_start);
                int word2_start = FindNonWhitespace(s, word1_stop);
                return word2_start;
            }

            static string NormalizeName(string s)
            {
                if (s.StartsWith("A ", StringComparison.CurrentCultureIgnoreCase) ||
                    s.StartsWith("The ", StringComparison.CurrentCultureIgnoreCase))
                {
                    int word2_start = FindSecondWord(s);
                    if (word2_start < 0) return s;

                    return s.Substring(word2_start);
                }
                return s;
            }

            public override int Compare(string param1, string param2)
            {
                return StringComparer.CurrentCultureIgnoreCase.Compare(
                    NormalizeName(param1),
                    NormalizeName(param2)
                    );
            }
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
            if (GameID != 0 && SystemID != 0 && FormatID != 0)
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

        public List<GameEntry> SearchDB(string Game_SearchTerm, string System_SearchTerm, string Format_SearchTerm)
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
                  ORDER BY Game COLLATE library ASC";
            SQL_Get_Rows.Parameters.Add(new SQLiteParameter("@GameTerm", Game_SearchTerm));
            SQL_Get_Rows.Parameters.Add(new SQLiteParameter("@SystemTerm", System_SearchTerm));
            SQL_Get_Rows.Parameters.Add(new SQLiteParameter("@FormatTerm", Format_SearchTerm));

            SQLiteDataReader Reader = SQL_Get_Rows.ExecuteReader();

            var list = new List<GameEntry>();
            while (Reader.Read())
                list.Add(new GameEntry()
                {
                    Name = Reader.GetString(0),
                    SystemName = Reader.GetString(1),
                    FormatName = Reader.GetString(2),
                });

            return list;
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
            }
            else
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