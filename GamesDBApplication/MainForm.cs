using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data.SQLite;

namespace GamesDBApplication
{
    public partial class MainForm : Form
    {
        DatabaseManager DB_Manager;
        List<string> listBoxContents;
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

        private void LoadDatabaseContents()
        {
            List<string> Results = DB_Manager.SearchDB("%", "%", "%");
            listBoxContents = Results;
            lb_Results.DataSource = Results;
            lbl_NoEntries.Text = listBoxContents.Count.ToString();
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

                //MessageBox.Show("Added Record:\n" + GameName + " / " + SystemName + " / " + Format);
                tb_GameName.Clear();
                tb_GameName.Focus();

                List<string> Results = DB_Manager.SearchDB("%", cb_System.Text, cb_Format.Text);
                listBoxContents = Results;
                lb_Results.DataSource = Results;
                lbl_NoEntries.Text = listBoxContents.Count.ToString();
            }
            catch (SQLiteException SQL_e)
            {
                MessageBox.Show(Convert.ToString(SQL_e));
            }
        }

        private void button_Search_Click(object sender, EventArgs e)
        {
            string Game_SearchTerm = tb_GameName.Text;
            if (Game_SearchTerm == "") { Game_SearchTerm = "%"; }
            else { Game_SearchTerm = string.Concat(Game_SearchTerm, "%"); } // Allow for loose searching

            string System_SearchTerm = cb_System.Text;
            if (System_SearchTerm == "") { System_SearchTerm = "%"; }

            string Format_SearchTerm = cb_Format.Text;
            if (Format_SearchTerm == "") { Format_SearchTerm = "%"; }

            try
            {
                List<string> Results = DB_Manager.SearchDB(Game_SearchTerm, System_SearchTerm, Format_SearchTerm);
                //MessageBox.Show("Found " + Results.Count + " records");
                listBoxContents = Results;
                lb_Results.DataSource = Results;
                lbl_NoEntries.Text = listBoxContents.Count.ToString();
            }
            catch (SQLiteException SQL_e)
            {
                MessageBox.Show(Convert.ToString(SQL_e));
            }
        }

        private void btn_Load_Click(object sender, EventArgs e)
        {
            string Highlighted_Data = Convert.ToString(lb_Results.SelectedItem);
            string[] RowInfo = SplitRowInfo(Highlighted_Data);
            tb_GameName.Text = RowInfo[0].Trim();
            cb_System.Text = RowInfo[1].Trim();
            cb_Format.Text = RowInfo[2].Trim();
        }

        private void button_Delete_Click(object sender, EventArgs e)
        {
            if (lb_Results.SelectedItem != null)
            {
                string[] RowInfo = SplitRowInfo(Convert.ToString(lb_Results.SelectedItem));

                DialogResult PerformDelete = MessageBox.Show("Are you sure you want to delete the following record: "
                    + RowInfo[0] + " / " + RowInfo[1] + " / " + RowInfo[2],
                    "Confirm Delete", MessageBoxButtons.YesNo);

                if (PerformDelete == DialogResult.Yes)
                {
                    string GameName = RowInfo[0].Trim();
                    string SystemName = RowInfo[1].Trim();
                    string FormatType = RowInfo[2].Trim();
                    bool Deleted = false;

                    Deleted = DB_Manager.DeleteRecord(GameName, SystemName, FormatType);
                    if (Deleted)
                    {
                        //MessageBox.Show("Record deleted");

                        string systemText = cb_System.Text;
                        if (systemText == "") { systemText = "%"; }
                        string formatText = cb_Format.Text;
                        if (formatText == "") { formatText = "%"; }

                        List<string> Results = DB_Manager.SearchDB("%", systemText, formatText);
                        listBoxContents = Results;
                        lb_Results.DataSource = Results;
                        lbl_NoEntries.Text = listBoxContents.Count.ToString();
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
            lb_Results.DataSource = null;
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
                Export exportModule = new GamesDBApplication.Export();
                exportModule.ExportCSV(listBoxContents, saveFile.FileName);
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

        private void tb_GameName_TextChanged(object sender, EventArgs e)
        {

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
            foreach(object o in lb_Results.SelectedItems)
            {
                selections.Add(o.ToString());
            }
            outputString = string.Join("\n", selections.ToArray());
            Clipboard.SetText(outputString);
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < lb_Results.Items.Count; i++)
            {
                lb_Results.SetSelected(i, true);
            }
        }
    }
}