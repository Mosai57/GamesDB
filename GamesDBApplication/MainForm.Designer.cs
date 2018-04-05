namespace GDBAccess
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cb_System = new System.Windows.Forms.ComboBox();
            this.cb_Format = new System.Windows.Forms.ComboBox();
            this.button_Add = new System.Windows.Forms.Button();
            this.button_Delete = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.initializeDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backupDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lbl_NoEntries = new System.Windows.Forms.Label();
            this.tb_GameName = new System.Windows.Forms.TextBox();
            this.dgv_Results = new System.Windows.Forms.DataGridView();
            this.GameName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SystemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FormatType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button_Export = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_Entries = new System.Windows.Forms.Label();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.randomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Results)).BeginInit();
            this.SuspendLayout();
            // 
            // cb_System
            // 
            this.cb_System.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cb_System.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_System.FormattingEnabled = true;
            this.cb_System.Items.AddRange(new object[] {
            "",
            "3DS",
            "Gamecube",
            "GB",
            "GBA",
            "GBC",
            "Genesis",
            "N64",
            "NDS",
            "NES",
            "PC",
            "PS1",
            "PS2",
            "PS3",
            "PS4",
            "PSP",
            "PSVita",
            "SNES",
            "Switch",
            "Wii",
            "Wii U",
            "Xbox",
            "Xbox 360",
            "Xbox One"});
            this.cb_System.Location = new System.Drawing.Point(87, 72);
            this.cb_System.Name = "cb_System";
            this.cb_System.Size = new System.Drawing.Size(316, 21);
            this.cb_System.TabIndex = 3;
            this.cb_System.SelectedIndexChanged += new System.EventHandler(this.cb_System_SelectedIndexChanged);
            // 
            // cb_Format
            // 
            this.cb_Format.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cb_Format.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_Format.FormattingEnabled = true;
            this.cb_Format.Items.AddRange(new object[] {
            "",
            "Physical",
            "Digital"});
            this.cb_Format.Location = new System.Drawing.Point(87, 101);
            this.cb_Format.Name = "cb_Format";
            this.cb_Format.Size = new System.Drawing.Size(316, 21);
            this.cb_Format.TabIndex = 5;
            this.cb_Format.SelectedIndexChanged += new System.EventHandler(this.cb_Format_SelectedIndexChanged);
            // 
            // button_Add
            // 
            this.button_Add.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Add.AutoSize = true;
            this.button_Add.Location = new System.Drawing.Point(479, 41);
            this.button_Add.Name = "button_Add";
            this.button_Add.Size = new System.Drawing.Size(105, 23);
            this.button_Add.TabIndex = 2;
            this.button_Add.Text = "Add";
            this.button_Add.UseVisualStyleBackColor = true;
            this.button_Add.Click += new System.EventHandler(this.button_Add_Click);
            // 
            // button_Delete
            // 
            this.button_Delete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Delete.AutoSize = true;
            this.button_Delete.Location = new System.Drawing.Point(479, 70);
            this.button_Delete.Name = "button_Delete";
            this.button_Delete.Size = new System.Drawing.Size(105, 23);
            this.button_Delete.TabIndex = 4;
            this.button_Delete.Text = "Delete";
            this.button_Delete.UseVisualStyleBackColor = true;
            this.button_Delete.Click += new System.EventHandler(this.button_Delete_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(596, 24);
            this.menuStrip1.TabIndex = 11;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.initializeDatabaseToolStripMenuItem,
            this.backupDatabaseToolStripMenuItem,
            this.updateDatabaseToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // initializeDatabaseToolStripMenuItem
            // 
            this.initializeDatabaseToolStripMenuItem.Name = "initializeDatabaseToolStripMenuItem";
            this.initializeDatabaseToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.initializeDatabaseToolStripMenuItem.Text = "Initialize Database";
            this.initializeDatabaseToolStripMenuItem.Click += new System.EventHandler(this.initializeDatabaseToolStripMenuItem_Click);
            // 
            // backupDatabaseToolStripMenuItem
            // 
            this.backupDatabaseToolStripMenuItem.Name = "backupDatabaseToolStripMenuItem";
            this.backupDatabaseToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.backupDatabaseToolStripMenuItem.Text = "Backup Database";
            this.backupDatabaseToolStripMenuItem.Click += new System.EventHandler(this.backupDatabaseToolStripMenuItem_Click_1);
            // 
            // updateDatabaseToolStripMenuItem
            // 
            this.updateDatabaseToolStripMenuItem.Name = "updateDatabaseToolStripMenuItem";
            this.updateDatabaseToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.updateDatabaseToolStripMenuItem.Text = "Update Database";
            this.updateDatabaseToolStripMenuItem.Click += new System.EventHandler(this.updateDatabaseToolStripMenuItem_Click);
            // 
            // lbl_NoEntries
            // 
            this.lbl_NoEntries.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lbl_NoEntries.AutoSize = true;
            this.lbl_NoEntries.Location = new System.Drawing.Point(147, 170);
            this.lbl_NoEntries.Name = "lbl_NoEntries";
            this.lbl_NoEntries.Size = new System.Drawing.Size(0, 13);
            this.lbl_NoEntries.TabIndex = 13;
            // 
            // tb_GameName
            // 
            this.tb_GameName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_GameName.Location = new System.Drawing.Point(87, 43);
            this.tb_GameName.Name = "tb_GameName";
            this.tb_GameName.Size = new System.Drawing.Size(386, 20);
            this.tb_GameName.TabIndex = 1;
            this.tb_GameName.TextChanged += new System.EventHandler(this.tb_GameName_TextChanged);
            // 
            // dgv_Results
            // 
            this.dgv_Results.AllowUserToAddRows = false;
            this.dgv_Results.AllowUserToDeleteRows = false;
            this.dgv_Results.AllowUserToResizeColumns = false;
            this.dgv_Results.AllowUserToResizeRows = false;
            this.dgv_Results.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv_Results.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Results.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.GameName,
            this.SystemName,
            this.FormatType});
            this.dgv_Results.Location = new System.Drawing.Point(12, 157);
            this.dgv_Results.Name = "dgv_Results";
            this.dgv_Results.ReadOnly = true;
            this.dgv_Results.RowHeadersVisible = false;
            this.dgv_Results.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_Results.Size = new System.Drawing.Size(572, 267);
            this.dgv_Results.TabIndex = 8;
            // 
            // GameName
            // 
            this.GameName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.GameName.DataPropertyName = "Name";
            this.GameName.HeaderText = "Game";
            this.GameName.Name = "GameName";
            this.GameName.ReadOnly = true;
            // 
            // SystemName
            // 
            this.SystemName.DataPropertyName = "SystemName";
            this.SystemName.HeaderText = "System";
            this.SystemName.Name = "SystemName";
            this.SystemName.ReadOnly = true;
            // 
            // FormatType
            // 
            this.FormatType.DataPropertyName = "FormatName";
            this.FormatType.HeaderText = "Format";
            this.FormatType.Name = "FormatType";
            this.FormatType.ReadOnly = true;
            // 
            // button_Export
            // 
            this.button_Export.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Export.Location = new System.Drawing.Point(479, 99);
            this.button_Export.Name = "button_Export";
            this.button_Export.Size = new System.Drawing.Size(105, 23);
            this.button_Export.TabIndex = 6;
            this.button_Export.Text = "Export";
            this.button_Export.UseVisualStyleBackColor = true;
            this.button_Export.Click += new System.EventHandler(this.button_Export_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(41, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Format:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(39, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "System:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Game Name:";
            // 
            // lbl_Entries
            // 
            this.lbl_Entries.AutoSize = true;
            this.lbl_Entries.Location = new System.Drawing.Point(14, 141);
            this.lbl_Entries.Name = "lbl_Entries";
            this.lbl_Entries.Size = new System.Drawing.Size(42, 13);
            this.lbl_Entries.TabIndex = 14;
            this.lbl_Entries.Text = "Entries:";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.randomToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // randomToolStripMenuItem
            // 
            this.randomToolStripMenuItem.Name = "randomToolStripMenuItem";
            this.randomToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.randomToolStripMenuItem.Text = "Random";
            this.randomToolStripMenuItem.Click += new System.EventHandler(this.randomToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(596, 436);
            this.Controls.Add(this.lbl_Entries);
            this.Controls.Add(this.dgv_Results);
            this.Controls.Add(this.tb_GameName);
            this.Controls.Add(this.lbl_NoEntries);
            this.Controls.Add(this.button_Export);
            this.Controls.Add(this.button_Delete);
            this.Controls.Add(this.button_Add);
            this.Controls.Add(this.cb_Format);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cb_System);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(456, 463);
            this.Name = "MainForm";
            this.Text = "GDB Access";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Results)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox cb_System;
        private System.Windows.Forms.ComboBox cb_Format;
        private System.Windows.Forms.Button button_Add;
        private System.Windows.Forms.Button button_Delete;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem initializeDatabaseToolStripMenuItem;
        private System.Windows.Forms.Label lbl_NoEntries;
        private System.Windows.Forms.ToolStripMenuItem backupDatabaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateDatabaseToolStripMenuItem;
        private System.Windows.Forms.TextBox tb_GameName;
        private System.Windows.Forms.DataGridView dgv_Results;
        private System.Windows.Forms.Button button_Export;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn GameName;
        private System.Windows.Forms.DataGridViewTextBoxColumn SystemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FormatType;
        private System.Windows.Forms.Label lbl_Entries;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem randomToolStripMenuItem;
    }
}

