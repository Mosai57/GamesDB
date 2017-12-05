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
            string UserName = Environment.UserName;
            string FilePath = "C:\\Users\\" + UserName + "\\db\\Games.sdb";

            if (!File.Exists(FilePath))
            {
                MessageBox.Show("Database not found. Initializing.", "Initialization");

                DatabaseInitializer initializer = new DatabaseInitializer();
                initializer.InitializeDatabase();
                initializer.Dispose();

                MessageBox.Show("Database initialized.", "Initialization");
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
