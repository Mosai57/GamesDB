using System;
using System.Collections.Generic;
using System.IO;
using System.Data.SQLite;

namespace GamesDBApplication
{
    class DatabaseInitializer : IDisposable
    {
        private SQLiteConnection GamesDB;

        public DatabaseInitializer()
        {
            string UserName = Environment.UserName;
            string FilePath = "C:\\Users\\" + UserName + "\\db\\Games.sdb";
            GamesDB = new SQLiteConnection("Data Source=" + FilePath + ";version=3;");
            //GamesDB.Open();
        }

        public void Dispose()
        {
            if (GamesDB != null)
            {
                GamesDB.Dispose();
                GamesDB = null;
            }
        }

        public void InitializeDatabase()
        {
            GamesDB.Open();

            SQLiteCommand EnablePragmas = GamesDB.CreateCommand();
            string PragmaDefinitons = @"
                PRAGMA foreign_keys = ON;
            ";
            EnablePragmas.CommandText = PragmaDefinitons;

            SQLiteCommand DefineGameTable = GamesDB.CreateCommand();
            string GameTableDefinition = @"
                CREATE TABLE Games(
                    ID INTEGER PRIMARY KEY NOT NULL,
                    Game TEXT UNIQUE NOT NULL
                );";
            DefineGameTable.CommandText = GameTableDefinition;

            SQLiteCommand DefineSystemTable = GamesDB.CreateCommand();
            string SystemTableDefinition = @"
                CREATE TABLE Systems (
                    ID INTEGER PRIMARY KEY NOT NULL,
                    System TEXT UNUQIE NOT NULL
                );";
            DefineSystemTable.CommandText = SystemTableDefinition;

            SQLiteCommand DefineFormatTable = GamesDB.CreateCommand();
            string FormatTableDefinition = @"
                CREATE TABLE Format (
                    ID INTEGER PRIMARY KEY NOT NULL,
                    Type TEXT UNIQUE NOT NULL
                );";
            DefineFormatTable.CommandText = FormatTableDefinition;

            SQLiteCommand SetupFormatTable = GamesDB.CreateCommand();
            string FormatTableEntries = @"
                INSERT INTO Format (ID, Type) VALUES (1, 'Physical');
                INSERT INTO Format(ID, Type) VALUES(2, 'Digital'); 
            ";
            SetupFormatTable.CommandText = FormatTableEntries;

            SQLiteCommand DefineGameSystemTable = GamesDB.CreateCommand();
            string GameSystemTableDefinition = @"
                CREATE TABLE GameSystem (
                    GameID INTEGER NOT NULL,
                    SystemID INTEGER NOT NULL,
                    FormatID INTEGER NOT NULL,
                    CONSTRAINT fk_GameSystem_Game FOREIGN KEY (GameID) REFERENCES Games(ID),
                    CONSTRAINT fk_GameSystem_System FOREIGN KEY (SystemID) REFERENCES Systems(ID),
                    CONSTRAINT fk_GameSystem_Format FOREIGN KEY (FormatID) REFERENCES Format(ID),
                    CONSTRAINT pk_GameSystem PRIMARY KEY (GameID, SystemID, FormatID)
                );";
            DefineGameSystemTable.CommandText = GameSystemTableDefinition;

            // Execute commands to build tables
            EnablePragmas.ExecuteNonQuery();
            DefineGameTable.ExecuteNonQuery();
            DefineSystemTable.ExecuteNonQuery();
            DefineFormatTable.ExecuteNonQuery();
            SetupFormatTable.ExecuteNonQuery();
            DefineGameSystemTable.ExecuteNonQuery();

            GamesDB.Close();
        }

    }
}
