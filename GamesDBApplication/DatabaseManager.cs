using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace GDBAccess
{
    /// <summary>
    /// Manages database transactions for Adding, Searching, and Deleting
    /// </summary>
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

        public void Dispose()
        {
            if (GamesDB != null)
            {
                GamesDB.Dispose();
                GamesDB = null;
            }
        }

        /// <summary>
        /// Closes the database and disposes the object. Forces Garbage Collection to ensure that database is fully unloaded.
        /// </summary>
        public void CloseDatabase()
        {
            GamesDB.Close();
            this.Dispose();
            GC.Collect();
        }

        /// <summary>
        /// Custom collation library for GDBA. Normalizes searches results.
        /// </summary>
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

            /// <summary>
            /// Sorts alphabetically, ignoring leading As or Thes in a title.
            /// </summary>
            /// <param name="s"></param>
            /// <returns></returns>
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

        /// <summary>
        /// Controls the local functions necessary to add a new entry to the database.
        /// </summary>
        /// <param name="Parameters"></param>
        public void Add(QueryParameters Parameters)
        {
            int GameID = Convert.ToInt32(null);
            int SystemID = Convert.ToInt32(null);
            int FormatID = Convert.ToInt32(null);
            string DateAdded = DateTime.Now.ToString("yyyyMMddHHmmss");

            GameID = GetGameID(Parameters.GameName);
            SystemID = GetSystemID(Parameters.System);
            FormatID = GetFormatID(Parameters.Format);

            // Secondary check to ensure no garbage data is added.
            // Above functions do attempt to ensure no 0s are returned.
            if (GameID != 0 && SystemID != 0 && FormatID != 0)
            {
                AddGameSystem(GameID, SystemID, FormatID, DateAdded);
            }
        }

        /// <summary>
        /// Queries the database and returns a list of GameEntry objects.
        /// </summary>
        /// <param name="QueryParameters"></param>
        /// <returns></returns>
        public List<GameEntry> Search(QueryParameters QueryParameters)
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
            SQL_Get_Rows.Parameters.Add(new SQLiteParameter("@GameTerm", QueryParameters.GameName));
            SQL_Get_Rows.Parameters.Add(new SQLiteParameter("@SystemTerm", QueryParameters.System));
            SQL_Get_Rows.Parameters.Add(new SQLiteParameter("@FormatTerm", QueryParameters.Format));

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

        /// <summary>
        /// Removes the record in the GameSystems table that matches the parameters.
        /// </summary>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public bool Delete(QueryParameters Parameters)
        {
            int GameName_ID = GetGameID(Parameters.GameName);
            int SystemName_ID = GetSystemID(Parameters.System);
            int FormatType_ID = GetFormatID(Parameters.Format);

            SQLiteCommand SQL_Delete_Record = GamesDB.CreateCommand();
            SQL_Delete_Record.CommandText = "DELETE FROM GameSystem WHERE GameID=@GameName_ID AND SystemID=@SystemName_ID AND FormatID=@FormatType_ID";

            SQL_Delete_Record.Parameters.Add(new SQLiteParameter("@GameName_ID", GameName_ID));
            SQL_Delete_Record.Parameters.Add(new SQLiteParameter("@SystemName_ID", SystemName_ID));
            SQL_Delete_Record.Parameters.Add(new SQLiteParameter("@FormatType_ID", FormatType_ID));

            SQL_Delete_Record.ExecuteNonQuery();
            return true;
        }

        /// <summary>
        /// Adds GameName to the Games table and returns the new RowID
        /// </summary>
        /// <param name="GameName"></param>
        /// <returns></returns>
        private int AddGame(string GameName)
        {
            int GameID = 0;
            SQLiteParameter Param_GameName = new SQLiteParameter("@GameName", GameName);

            SQLiteCommand SQL_Add_Game = GamesDB.CreateCommand();
            SQL_Add_Game.CommandText = "INSERT INTO Games (Game) VALUES (@GameName); select last_insert_rowid();";
            SQL_Add_Game.Parameters.Add(Param_GameName);
            GameID = (int)(long)SQL_Add_Game.ExecuteScalar();

            return GameID;
        }

        /// <summary>
        /// Adds SystemName to the Systems table and returns the new RowID
        /// </summary>
        /// <param name="SystemName"></param>
        /// <returns></returns>
        private int AddSystem(string SystemName)
        {
            int SystemID = 0;
            SQLiteParameter Param_SystemName = new SQLiteParameter("@SystemName", SystemName);

            SQLiteCommand SQL_Add_System = GamesDB.CreateCommand();
            SQL_Add_System.CommandText = "INSERT INTO Systems (System) VALUES (@SystemName); select last_insert_rowid();";
            SQL_Add_System.Parameters.Add(Param_SystemName);
            SystemID = (int)(long)SQL_Add_System.ExecuteScalar();

            return SystemID;
        }

        /// <summary>
        /// Adds a new entry in the GameSystems table using the RowIDs for the specified parameters.
        /// </summary>
        /// <param name="GameID"></param>
        /// <param name="SystemID"></param>
        /// <param name="FormatID"></param>
        /// <param name="DateAdded"></param>
        private void AddGameSystem(int GameID, int SystemID, int FormatID, string DateAdded)
        {
            SQLiteCommand SQL_Add_Entry = GamesDB.CreateCommand();
            SQL_Add_Entry.CommandText = "INSERT INTO GameSystem (GameID, SystemID, FormatID, DateAdded) Values (@GameID, @SystemID, @FormatID, @DateAdded)";
            SQL_Add_Entry.Parameters.Add(new SQLiteParameter("@GameID", GameID));
            SQL_Add_Entry.Parameters.Add(new SQLiteParameter("@SystemID", SystemID));
            SQL_Add_Entry.Parameters.Add(new SQLiteParameter("@FormatID", FormatID));
            SQL_Add_Entry.Parameters.Add(new SQLiteParameter("@DateAdded", DateAdded));

            SQL_Add_Entry.ExecuteNonQuery();
        }

        /// <summary>
        /// Returns the RowID for GameName from the Games table.
        /// </summary>
        /// <param name="GameName"></param>
        /// <returns></returns>
        private int GetGameID(string GameName)
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

        /// <summary>
        /// Returns the RowID for SystemName from the Systems table.
        /// </summary>
        /// <param name="SystemName"></param>
        /// <returns></returns>
        private int GetSystemID(string SystemName)
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

        /// <summary>
        /// Returns the RowID for Format from the Format table.
        /// </summary>
        /// <param name="Format"></param>
        /// <returns></returns>
        private int GetFormatID(string Format)
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
    }
}