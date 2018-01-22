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
        string DatabaseSource;

        public MainForm(string FilePath)
        {
            InitializeComponent();
            DatabaseSource = FilePath;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            DB_Manager = new DatabaseManager(DatabaseSource);
            LoadDatabaseContents();
        }

        private void LoadDatabaseContents(string Game = "%", string System = "%", string Format = "%")
        {
            List<GameEntry> results = DB_Manager.SearchDB(Game, System, Format);
            DBResults = results;
            dgv_Results.DataSource = DBResults;
        }

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
                DB_Manager.AddToDB_Controller(GameName, SystemName, Format);

                tb_GameName.Clear();
                tb_GameName.Focus();

                LoadDatabaseContents("%", cb_System.Text, cb_Format.Text);
            }
            catch (SQLiteException SQL_e)
            {
                MessageBox.Show("An error has occured while adding the entry.\nBe sure that the record does not exist in the database.");//Convert.ToString(SQL_e));
            }
        }

        private void button_Search_Click(object sender, EventArgs e)
        {
            string Game_SearchTerm = tb_GameName.Text;
            if (Game_SearchTerm == "") { Game_SearchTerm = "%"; }
            else { Game_SearchTerm = string.Concat("%", Game_SearchTerm, "%"); } // Allow for loose searching

            string System_SearchTerm = cb_System.Text;
            if (System_SearchTerm == "") { System_SearchTerm = "%"; }

            string Format_SearchTerm = cb_Format.Text;
            if (Format_SearchTerm == "") { Format_SearchTerm = "%"; }

            try
            {
                LoadDatabaseContents(Game_SearchTerm, System_SearchTerm, Format_SearchTerm);
            }
            catch (SQLiteException SQL_e)
            {
                MessageBox.Show(Convert.ToString(SQL_e));
            }
        }

        private void btn_Load_Click(object sender, EventArgs e)
        {
            var selrow = dgv_Results.SelectedRows;
            if (selrow.Count == 0) return;
            var record = (GameEntry)selrow[0].DataBoundItem;
            tb_GameName.Text = record.Name;
            cb_System.Text = record.SystemName;
            cb_Format.Text = record.FormatName;
        }

        private void button_Delete_Click(object sender, EventArgs e)
        {
            if (dgv_Results.SelectedRows.Count > 0)
            {
                var record = (GameEntry) dgv_Results.SelectedRows[0].DataBoundItem;

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

                    Deleted = DB_Manager.DeleteRecord(GameName, SystemName, FormatType);
                    if (Deleted)
                    {
                        string systemText = cb_System.Text;
                        if (systemText == "") { systemText = "%"; }
                        string formatText = cb_Format.Text;
                        if (formatText == "") { formatText = "%"; }

                        LoadDatabaseContents("%", systemText, formatText);
                    }
                    else
                    {
                        MessageBox.Show("Unable to delete record. Error occured.");
                    }
                }

            }
        }

        private void ClearForm()
        {
            tb_GameName.Clear();
            cb_Format.Text = "";
            cb_System.Text = "";
            dgv_Results.DataSource = null;
        }

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
                exportModule.ExportCSV(DBResults, saveFile.FileName);
                MessageBox.Show("Export completed!");
            }
        }

        private string[] SplitRowInfo(string ToSplit)
        {
            var NewArray = new[] { "", "", "" };
            var split = ToSplit.Split('/');
            Array.Copy(split, NewArray, split.Length <= 3 ? split.Length : 3);

            return NewArray;
        }

        private void initializeDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string DatabasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"InduljNet\Gdba", "GDBA.sdb");
            DB_Manager.CloseDatabase();
            DatabaseInitializer Initializer = new DatabaseInitializer(DatabasePath);

            Initializer.ClearDatabase();
            Initializer.InitializeDatabase(false); // false indicating db exists, tables dont.

            DB_Manager = new DatabaseManager(DatabasePath);
            ClearForm();
            LoadDatabaseContents();
        }

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

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string outputString = "";
            List<string> selections = new List<string>();
            foreach(object o in dgv_Results.SelectedRows)
            {
                selections.Add(o.ToString());
            }
            outputString = string.Join("\n", selections.ToArray());
            Clipboard.SetText(outputString);
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dgv_Results.SelectAll();
        }

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
    }
}