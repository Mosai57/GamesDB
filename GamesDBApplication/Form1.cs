using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace GamesDBApplication
{
    public partial class Form1 : Form
    {
        DatabaseManager DB_Manager;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DB_Manager = new DatabaseManager();
        }

        private void button_Add_Click(object sender, EventArgs e)
        {
            string GameName = tb_GameName.Text;
            string SystemName = cb_System.Text;
            string Format = cb_Format.Text;

            // Check to make sure no idiots left a necessary spot blank
            if (GameName == "" || SystemName == "" || Format == "")
            {
                MessageBox.Show("No fields may be left blank when adding an entry.", "Missing Fields", MessageBoxButtons.OK);
                return;
            }

            try
            {
                DB_Manager.AddToDB_Controller(GameName, SystemName, Format);
            } catch(SQLiteException SQL_e)
            {
                MessageBox.Show(Convert.ToString(SQL_e));
            }
        }

        private void button_Search_Click(object sender, EventArgs e)
        {
            string Game_SearchTerm = tb_GameName.Text;
            if(Game_SearchTerm == "") { Game_SearchTerm = "%"; }

            string System_SearchTerm = cb_System.Text;
            if(System_SearchTerm == "") { System_SearchTerm = "%"; }

            string Format_SearchTerm = cb_Format.Text;
            if(Format_SearchTerm == "") { Format_SearchTerm = "%"; }

            try
            {
                List<string> Results = DB_Manager.SearchDB(Game_SearchTerm, System_SearchTerm, Format_SearchTerm);
                MessageBox.Show("Found " + Results.Count + " records");
                lb_Results.DataSource = Results;
            } catch(SQLiteException SQL_e)
            {
                MessageBox.Show(Convert.ToString(SQL_e));
            }
        }

        private void button_Load_Click(object sender, EventArgs e)
        {
            string Highlighted_Data = Convert.ToString(lb_Results.SelectedItem);
            var RowInfo = new[] { "", "", "" };
            var split = Highlighted_Data.Split('/');
            Array.Copy(split, RowInfo, split.Length <= 3 ? split.Length : 3);
            tb_GameName.Text = RowInfo[0].Trim();
            cb_System.Text = RowInfo[1].Trim();
            cb_Format.Text = RowInfo[2].Trim();
        }

        private void button_Clear_Click(object sender, EventArgs e)
        {
            tb_GameName.Clear();
            cb_Format.Text = "";
            cb_System.Text = "";
            lb_Results.DataSource = null;
        }
    }
}
