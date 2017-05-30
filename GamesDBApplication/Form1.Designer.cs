namespace GamesDBApplication
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.tb_GameName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cb_System = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cb_Format = new System.Windows.Forms.ComboBox();
            this.button_Add = new System.Windows.Forms.Button();
            this.button_Search = new System.Windows.Forms.Button();
            this.lb_Results = new System.Windows.Forms.ListBox();
            this.button_Delete = new System.Windows.Forms.Button();
            this.button_Load = new System.Windows.Forms.Button();
            this.button_Clear = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Game Name:";
            // 
            // tb_GameName
            // 
            this.tb_GameName.Location = new System.Drawing.Point(87, 12);
            this.tb_GameName.Name = "tb_GameName";
            this.tb_GameName.Size = new System.Drawing.Size(196, 20);
            this.tb_GameName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "System:";
            // 
            // cb_System
            // 
            this.cb_System.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_System.FormattingEnabled = true;
            this.cb_System.Items.AddRange(new object[] {
            "",
            "3DS",
            "GB",
            "GBA",
            "GBC",
            "Gamecube",
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
            this.cb_System.Location = new System.Drawing.Point(87, 38);
            this.cb_System.Name = "cb_System";
            this.cb_System.Size = new System.Drawing.Size(121, 21);
            this.cb_System.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(40, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Format";
            // 
            // cb_Format
            // 
            this.cb_Format.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_Format.FormattingEnabled = true;
            this.cb_Format.Items.AddRange(new object[] {
            "",
            "Physical",
            "Digital"});
            this.cb_Format.Location = new System.Drawing.Point(87, 65);
            this.cb_Format.Name = "cb_Format";
            this.cb_Format.Size = new System.Drawing.Size(121, 21);
            this.cb_Format.TabIndex = 5;
            // 
            // button_Add
            // 
            this.button_Add.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_Add.AutoSize = true;
            this.button_Add.Location = new System.Drawing.Point(43, 115);
            this.button_Add.Name = "button_Add";
            this.button_Add.Size = new System.Drawing.Size(64, 23);
            this.button_Add.TabIndex = 6;
            this.button_Add.Text = "Add";
            this.button_Add.UseVisualStyleBackColor = true;
            this.button_Add.Click += new System.EventHandler(this.button_Add_Click);
            // 
            // button_Search
            // 
            this.button_Search.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_Search.AutoSize = true;
            this.button_Search.Location = new System.Drawing.Point(113, 115);
            this.button_Search.Name = "button_Search";
            this.button_Search.Size = new System.Drawing.Size(64, 23);
            this.button_Search.TabIndex = 7;
            this.button_Search.Text = "Search";
            this.button_Search.UseVisualStyleBackColor = true;
            this.button_Search.Click += new System.EventHandler(this.button_Search_Click);
            // 
            // lb_Results
            // 
            this.lb_Results.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lb_Results.FormattingEnabled = true;
            this.lb_Results.Location = new System.Drawing.Point(289, 12);
            this.lb_Results.Name = "lb_Results";
            this.lb_Results.Size = new System.Drawing.Size(459, 160);
            this.lb_Results.TabIndex = 8;
            // 
            // button_Delete
            // 
            this.button_Delete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_Delete.AutoSize = true;
            this.button_Delete.Location = new System.Drawing.Point(43, 149);
            this.button_Delete.Name = "button_Delete";
            this.button_Delete.Size = new System.Drawing.Size(64, 23);
            this.button_Delete.TabIndex = 9;
            this.button_Delete.Text = "Delete";
            this.button_Delete.UseVisualStyleBackColor = true;
            this.button_Delete.Click += new System.EventHandler(this.button_Delete_Click);
            // 
            // button_Load
            // 
            this.button_Load.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_Load.AutoSize = true;
            this.button_Load.Location = new System.Drawing.Point(183, 115);
            this.button_Load.Name = "button_Load";
            this.button_Load.Size = new System.Drawing.Size(64, 23);
            this.button_Load.TabIndex = 10;
            this.button_Load.Text = "Load";
            this.button_Load.UseVisualStyleBackColor = true;
            this.button_Load.Click += new System.EventHandler(this.button_Load_Click);
            // 
            // button_Clear
            // 
            this.button_Clear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_Clear.AutoSize = true;
            this.button_Clear.Location = new System.Drawing.Point(113, 149);
            this.button_Clear.Name = "button_Clear";
            this.button_Clear.Size = new System.Drawing.Size(64, 23);
            this.button_Clear.TabIndex = 12;
            this.button_Clear.Text = "Clear";
            this.button_Clear.UseVisualStyleBackColor = true;
            this.button_Clear.Click += new System.EventHandler(this.button_Clear_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(183, 149);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(64, 23);
            this.button1.TabIndex = 13;
            this.button1.Text = "Export";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(760, 184);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button_Clear);
            this.Controls.Add(this.button_Load);
            this.Controls.Add(this.button_Delete);
            this.Controls.Add(this.lb_Results);
            this.Controls.Add(this.button_Search);
            this.Controls.Add(this.button_Add);
            this.Controls.Add(this.cb_Format);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cb_System);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tb_GameName);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "GDB Access";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_GameName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cb_System;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cb_Format;
        private System.Windows.Forms.Button button_Add;
        private System.Windows.Forms.Button button_Search;
        private System.Windows.Forms.ListBox lb_Results;
        private System.Windows.Forms.Button button_Delete;
        private System.Windows.Forms.Button button_Load;
        private System.Windows.Forms.Button button_Clear;
        private System.Windows.Forms.Button button1;
    }
}

