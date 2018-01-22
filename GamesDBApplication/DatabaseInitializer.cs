using System;
using System.Data.SQLite;

namespace GDBAccess
{
    class DatabaseInitializer : IDisposable
    {
        private SQLiteConnection GamesDB;

        public DatabaseInitializer(string FilePath)
        {
            GamesDB = new SQLiteConnection("Data Source=" + FilePath + ";version=3;");
        }

        public void Dispose()
        {
            if (GamesDB != null)
            {
                GamesDB.Dispose();
                GamesDB = null;
            }
        }

        public void InitializeDatabase(bool NewDB = true)
        {
            GamesDB.Open();

            if (NewDB)
            {
                /* Enable Pragmas
                 * Pragmas enabled:
                 *      Foreign Keys
                 */
                SQLiteCommand EnablePragmas = GamesDB.CreateCommand();
                string PragmaDefinitons = @"
                    PRAGMA foreign_keys = ON;
                ";
                EnablePragmas.CommandText = PragmaDefinitons;
                EnablePragmas.ExecuteNonQuery();
            }

            /* Define Game Table
             * Game table properties:
             *      ID - Primary Key
             *      Game name - Text, Unique
             */ 
            SQLiteCommand DefineGameTable = GamesDB.CreateCommand();
            string GameTableDefinition = @"
                CREATE TABLE Games(
                    ID INTEGER PRIMARY KEY NOT NULL,
                    Game TEXT UNIQUE NOT NULL
                );";
            DefineGameTable.CommandText = GameTableDefinition;
            DefineGameTable.ExecuteNonQuery();

            /* Define System Table
             * System table properties:
             *      ID - Primary Key
             *      System name - Text, Unique
             */
            SQLiteCommand DefineSystemTable = GamesDB.CreateCommand();
            string SystemTableDefinition = @"
                CREATE TABLE Systems (
                    ID INTEGER PRIMARY KEY NOT NULL,
                    System TEXT UNIQIE NOT NULL
                );";
            DefineSystemTable.CommandText = SystemTableDefinition;
            DefineSystemTable.ExecuteNonQuery();

            /* Define Format Table
             * Format table properties:
             *      ID - Primary key
             *      Format name - Text, Unique
             */
            SQLiteCommand DefineFormatTable = GamesDB.CreateCommand();
            string FormatTableDefinition = @"
                CREATE TABLE Format (
                    ID INTEGER PRIMARY KEY NOT NULL,
                    Type TEXT UNIQUE NOT NULL
                );";
            DefineFormatTable.CommandText = FormatTableDefinition;
            DefineFormatTable.ExecuteNonQuery();

            /* Setup Format Table
             * Inserts the two values we will use in the format table
             */
            SQLiteCommand SetupFormatTable = GamesDB.CreateCommand();
            string FormatTableEntries = @"
                INSERT INTO Format (ID, Type) VALUES (1, 'Physical');
                INSERT INTO Format(ID, Type) VALUES(2, 'Digital'); 
            ";
            SetupFormatTable.CommandText = FormatTableEntries;
            SetupFormatTable.ExecuteNonQuery();

            /* Define Game System Table
             * Game system table properties:
             *      GameID - Integer, FK referencing Game table ID
             *      SystemID - Integer, FK referencing System table ID
             *      FormatID - Integer, FK referencing Format table ID
             *      Primary key consisting of GameID, SystemID, and FormatID
             */
            SQLiteCommand DefineGameSystemTable = GamesDB.CreateCommand();
            string GameSystemTableDefinition = @"
                CREATE TABLE GameSystem (
                    GameID INTEGER NOT NULL,
                    SystemID INTEGER NOT NULL,
                    FormatID INTEGER NOT NULL,
                    DateAdded TEXT NOT NULL,
                    CONSTRAINT fk_GameSystem_Game FOREIGN KEY (GameID) REFERENCES Games(ID),
                    CONSTRAINT fk_GameSystem_System FOREIGN KEY (SystemID) REFERENCES Systems(ID),
                    CONSTRAINT fk_GameSystem_Format FOREIGN KEY (FormatID) REFERENCES Format(ID),
                    CONSTRAINT pk_GameSystem PRIMARY KEY (GameID, SystemID, FormatID)
                );";
            DefineGameSystemTable.CommandText = GameSystemTableDefinition;
            DefineGameSystemTable.ExecuteNonQuery();

            GamesDB.Close();
        }

        public void UpdateDatabase()
        {
            GamesDB.Open();

            /* We rename the GameSystems table to GameSystems_Prev
             * in order to be able to copy the existing data to a new
             * GameSystems table in the new proper format.
             */
            SQLiteCommand BackupGameSystems = GamesDB.CreateCommand();
            string BackupString = @"
                PRAGMA foreign_keys=off;
                ALTER TABLE GameSystem RENAME TO GameSystem_Prev;
            ";
            BackupGameSystems.CommandText = BackupString;
            BackupGameSystems.ExecuteNonQuery();

            SQLiteCommand DefineGameSystems = GamesDB.CreateCommand();
            string UpdateString = @"
                CREATE TABLE GameSystem (
                    GameID INTEGER NOT NULL,
                    SystemID INTEGER NOT NULL,
                    FormatID INTEGER NOT NULL,
                    DateAdded TEXT NULL,
                    CONSTRAINT fk_GameSystem_Game FOREIGN KEY (GameID) REFERENCES Games(ID),
                    CONSTRAINT fk_GameSystem_System FOREIGN KEY (SystemID) REFERENCES Systems(ID),
                    CONSTRAINT fk_GameSystem_Format FOREIGN KEY (FormatID) REFERENCES Format(ID),
                    CONSTRAINT pk_GameSystem PRIMARY KEY (GameID, SystemID, FormatID)
                );";
            DefineGameSystems.CommandText = UpdateString;
            DefineGameSystems.ExecuteNonQuery();

            SQLiteCommand RebuildGameSystems = GamesDB.CreateCommand();
            string RebuildString = @"
                INSERT INTO GameSystem (GameID, SystemID, FormatID)
                SELECT GameID, SystemID, FormatID
                FROM GameSystem_Prev;
                DROP TABLE GameSystem_Prev;
                PRAGMA foreign_keys=on;
            ";
            RebuildGameSystems.CommandText = RebuildString;
            RebuildGameSystems.ExecuteNonQuery();

            GamesDB.Close();
        }

        public void ClearDatabase()
        {
            GamesDB.Open();

            SQLiteCommand DropTables = GamesDB.CreateCommand();
            string DropString = @"
                DROP TABLE IF EXISTS GameSystem; 
                DROP TABLE IF EXISTS Games; 
                DROP TABLE IF EXISTS Systems; 
                DROP TABLE IF EXISTS Format;";
            DropTables.CommandText = DropString;
            DropTables.ExecuteNonQuery();

            GamesDB.Close();
        }
    }
}
