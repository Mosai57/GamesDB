using System;
using System.IO;
using System.Windows.Forms;

namespace GDBAccess
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string filename = "GDBA.sdb";
            string FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"InduljNet\Gdba");
            
            // Initialize path
            System.IO.Directory.CreateDirectory(FilePath);
            System.IO.Directory.CreateDirectory(Path.Combine(FilePath, @"log"));
            string DBFilePath = Path.Combine(FilePath, filename);
            
            // If this is our first time using the program, initialize a new database.
            if (!File.Exists(DBFilePath))
            {
                DatabaseInitializer initializer = new DatabaseInitializer(DBFilePath);
                initializer.InitializeDatabase();
                initializer.Dispose();
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(FilePath, DBFilePath));
        }
    }
}
