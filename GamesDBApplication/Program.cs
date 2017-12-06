using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace GamesDBApplication
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
            FilePath = Path.Combine(FilePath, filename);

            if (!File.Exists(FilePath))
            {
                MessageBox.Show("Database not found. Initializing.", "Initialization");

                DatabaseInitializer initializer = new DatabaseInitializer(FilePath);
                initializer.InitializeDatabase();
                initializer.Dispose();

                MessageBox.Show("Database initialized.", "Initialization");
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(FilePath));
        }
    }
}
