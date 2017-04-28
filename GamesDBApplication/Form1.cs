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
            string GameName = textBox1tb_GameName.Text;
            string SystemName = cb_System.Text;
            string Format = cb_Format.Text;

            // Check to make sure no idiots left a necessary spot blank
            if (GameName == "" || SystemName == "" || Format == "")
            {
                MessageBox.Show("No fields may be left blank when adding an entry.", "Missing Fields", MessageBoxButtons.OK);
                return;
            }

            DB_Manager.AddToDB_Controller(GameName, SystemName, Format);
        }
    }
}
