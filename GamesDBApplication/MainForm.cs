using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace GDBAccess
{
    public partial class MainForm : Form
    {
        DatabaseManager DB_Manager;
        List<GameEntry> DBResults;
        int NumEntries;
        Logger logger;
        string DatabaseSource;
        QueryParameters QueryParams = new QueryParameters();


        public MainForm(string FilePath, string DBFilePath)
        {
            InitializeComponent();
            DatabaseSource = DBFilePath;
            logger = new Logger(FilePath);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Text += " - Version " + typeof(Program).Assembly.GetName().Version;
            DB_Manager = new DatabaseManager(DatabaseSource);
            LoadDatabaseContents();
        }

        /// <summary>
        /// Adds the current QueryParams to the database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Add_Click(object sender, EventArgs e)
        {
            string GameName = tb_GameName.Text;
            string SystemName = cb_System.Text;
            string Format = cb_Format.Text;

            if (GameName == "" || SystemName == "" || Format == "")
            {
                MessageBox.Show("No fields may be left blank when adding an entry.", "Missing Fields", MessageBoxButtons.OK);
                return;
            }

            try
            {
                DB_Manager.Add(QueryParams);
                LogTransaction("Add");

                tb_GameName.Clear();
                tb_GameName.Focus();

                LoadDatabaseContents();
            }
            catch (SQLiteException SQL_e)
            {
                LogEvent(SQL_e);
                MessageBox.Show("An error has occured while adding the entry.\nBe sure that the record does not exist in the database.");//Convert.ToString(SQL_e));
            }
        }

        /// <summary>
        /// Searches the database with the current QueryParams.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Search_Click(object sender, EventArgs e)
        {
            BuildQueryParameters();

            try
            {
                LoadDatabaseContents();
            }
            catch (SQLiteException SQL_e)
            {
                LogEvent(SQL_e);
                MessageBox.Show(Convert.ToString(SQL_e));
            }
        }

        /// <summary>
        /// Deletes the current QueryParams from the database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Delete_Click(object sender, EventArgs e)
        {
            if (dgv_Results.SelectedRows.Count > 0)
            {
                var record = (GameEntry)dgv_Results.SelectedRows[0].DataBoundItem;

                DialogResult PerformDelete = MessageBox.Show($@"Are you sure you want to delete the following record?

                    Name = {record.Name}
                    System = {record.SystemName}
                    Format = {record.FormatName}
                    ",
                    "Confirm Delete", MessageBoxButtons.YesNo);

                if (PerformDelete == DialogResult.Yes)
                {
                    string GameName = record.Name;
                    string SystemName = record.SystemName;
                    string FormatType = record.FormatName;
                    bool Deleted = false;

                    Deleted = DB_Manager.Delete(QueryParams);
                    LogTransaction("Delete");
                    if (Deleted)
                    {
                        string systemText = cb_System.Text;
                        if (systemText == "") { systemText = "%"; }
                        string formatText = cb_Format.Text;
                        if (formatText == "") { formatText = "%"; }

                        LoadDatabaseContents();
                    }
                    else
                    {
                        MessageBox.Show("Unable to delete record. Error occured.");
                    }
                }

            }
        }

        /// <summary>
        /// Prompts the user for an export method and executes the proper export method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Export_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.FileName = "gdbexport.csv";
            saveFile.InitialDirectory = Convert.ToString(Environment.SpecialFolder.MyDocuments);
            saveFile.Filter = "Comma Separated Values (*.csv)|*.csv";
            saveFile.FilterIndex = 1;

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                Export exportModule = new GDBAccess.Export();
                exportModule.CSV(DBResults, saveFile.FileName);
                MessageBox.Show("Export completed!");
            }
        }

        /// <summary>
        /// Calls the DatabaseInitializer and clears the database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void initializeDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string DatabasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"InduljNet\Gdba", "GDBA.sdb");
            DB_Manager.CloseDatabase();
            DatabaseInitializer Initializer = new DatabaseInitializer(DatabasePath);

            Initializer.InitializeDatabase(false); // false indicating db exists, tables dont.

            DB_Manager = new DatabaseManager(DatabasePath);
            ClearForm();
            LoadDatabaseContents();
        }

        /// <summary>
        /// Creates a backup of the database .sdb file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backupDatabaseToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.FileName = "GDBA_Backup.sdb";
            saveFile.InitialDirectory = Convert.ToString(Environment.SpecialFolder.MyDocuments);
            saveFile.Filter = "SQLite Database File (*.sdb)|*.sdb";
            saveFile.FilterIndex = 1;

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                System.IO.File.Copy(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"InduljNet\Gdba", "GDBA.sdb"), saveFile.FileName);
                MessageBox.Show("Backup Complete");
            }
        }

        /// <summary>
        /// Updates the database to the current standard according to the DatabaseInitializer object
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updateDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string DatabasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"InduljNet\Gdba", "GDBA.sdb");
            DB_Manager.CloseDatabase();
            DatabaseInitializer Initializer = new DatabaseInitializer(DatabasePath);

            Initializer.UpdateDatabase();

            DB_Manager = new DatabaseManager(DatabasePath);
            ClearForm();
            LoadDatabaseContents();
        }

        /// <summary>
        /// Selects a random row in the DataGridView element and snaps to it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void randomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Random rng = new Random();
            int index = rng.Next(NumEntries - 1);
            dgv_Results.ClearSelection();
            dgv_Results.Rows[index].Selected = true;
            dgv_Results.CurrentCell = dgv_Results.Rows[index].Cells[0];
        }

        /// <summary>
        /// Allows for the database results to change to immediately match the form elements.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tb_GameName_TextChanged(object sender, EventArgs e)
        {
            BuildQueryParameters();

            try
            {
                LoadDatabaseContents();
            }
            catch (SQLiteException SQL_e)
            {
                LogEvent(SQL_e);
                MessageBox.Show(Convert.ToString(SQL_e));
            }
        }

        /// <summary>
        /// Allows for the database results to change to immediately match the form elements.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cb_System_SelectedIndexChanged(object sender, EventArgs e)
        {
            BuildQueryParameters();

            try
            {
                LoadDatabaseContents();
            }
            catch (SQLiteException SQL_e)
            {
                LogEvent(SQL_e);
                MessageBox.Show(Convert.ToString(SQL_e));
            }
        }

        /// <summary>
        /// Allows for the database results to change to immediately match the form elements.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cb_Format_SelectedIndexChanged(object sender, EventArgs e)
        {
            BuildQueryParameters();

            try
            {
                LoadDatabaseContents();
            }
            catch (SQLiteException SQL_e)
            {
                LogEvent(SQL_e);
                MessageBox.Show(Convert.ToString(SQL_e));
            }
        }

        /// <summary>
        /// Populates dgv_Results with the query results for QueryParams' contents.
        /// </summary>
        private void LoadDatabaseContents()
        {
            BuildQueryParameters();
            List<GameEntry> results = DB_Manager.Search(QueryParams);
            DBResults = results;
            dgv_Results.DataSource = DBResults;
            NumEntries = DBResults.Count;
            lbl_Entries.Text = String.Concat("Entries: ", NumEntries.ToString());
        }

        /// <summary>
        /// Updates QueryParams with the current form information.
        /// </summary>
        private void BuildQueryParameters()
        {
            QueryParams.GameName = tb_GameName.Text;
            if (QueryParams.GameName == "") { QueryParams.GameName = "%"; }
            else { QueryParams.GameName = string.Concat("%", QueryParams.GameName, "%"); } // Allow for loose searching

            QueryParams.System = cb_System.Text;
            if (QueryParams.System == "") { QueryParams.System = "%"; }

            QueryParams.Format = cb_Format.Text;
            if (QueryParams.Format == "") { QueryParams.Format = "%"; }
        }

        /// <summary>
        /// Sets all form elements to blank or empty.
        /// </summary>
        private void ClearForm()
        {
            tb_GameName.Clear();
            cb_Format.Text = "";
            cb_System.Text = "";
            dgv_Results.DataSource = null;
        }

        /// <summary>
        /// Logs a SQL transaction.
        /// </summary>
        /// <param name="TransactionType"></param>
        private void LogTransaction(string TransactionType)
        {
            string gameName = tb_GameName.Text;
            if(gameName == "") { gameName = "%"; }
            string systemName = cb_System.Text;
            if(systemName == "") { systemName = "%"; }
            string formatName = cb_Format.Text;
            if(formatName == "") { formatName = "%"; }

            TransactionLogObject transObj = new TransactionLogObject(TransactionType, gameName, systemName, formatName);
            logger.Log(transObj);
        }

        /// <summary>
        /// Logs a system event.
        /// </summary>
        /// <param name="eventObj"></param>
        private void LogEvent(object eventObj)
        {
            EventLogObject eventLog = new EventLogObject(eventObj);
            logger.Log(eventLog);
        } 
    }
}