namespace GoogleAccountMerge
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
            
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.TargetButton = new System.Windows.Forms.Button();
            this.TargetTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SourceButton = new System.Windows.Forms.Button();
            this.SourceAccount = new System.Windows.Forms.Label();
            this.SourceTitle = new System.Windows.Forms.Label();
            this.ExecuteButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.TargetLog = new System.Windows.Forms.Label();
            this.panel7 = new System.Windows.Forms.Panel();
            this.SourceLog = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.RefreshButton = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.NewTargetListBox = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.OldTargetListBox = new System.Windows.Forms.ListBox();
            this.OldTargetListTile = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.SourceListBox = new System.Windows.Forms.ListBox();
            this.SourceListTile = new System.Windows.Forms.Label();
            TargetAccount = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1024, 110);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel2.Controls.Add(this.TargetButton);
            this.panel2.Controls.Add(TargetAccount);
            this.panel2.Controls.Add(this.TargetTitle);
            this.panel2.Location = new System.Drawing.Point(515, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(506, 104);
            this.panel2.TabIndex = 1;
            // 
            // TargetButton
            // 
            this.TargetButton.AutoSize = true;
            this.TargetButton.Location = new System.Drawing.Point(203, 67);
            this.TargetButton.Name = "TargetButton";
            this.TargetButton.Size = new System.Drawing.Size(94, 34);
            this.TargetButton.TabIndex = 1;
            this.TargetButton.Text = "Connect";
            this.TargetButton.UseVisualStyleBackColor = true;
            this.TargetButton.Click += new System.EventHandler(this.TargetButton_Click);
            // 
            // TargetAccount
            // 
            TargetAccount.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            TargetAccount.Location = new System.Drawing.Point(0, 32);
            TargetAccount.Name = "TargetAccount";
            TargetAccount.Size = new System.Drawing.Size(503, 20);
            TargetAccount.TabIndex = 1;
            TargetAccount.Text = "No account";
            TargetAccount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TargetTitle
            // 
            this.TargetTitle.AutoSize = true;
            this.TargetTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TargetTitle.Location = new System.Drawing.Point(172, 6);
            this.TargetTitle.Name = "TargetTitle";
            this.TargetTitle.Size = new System.Drawing.Size(158, 26);
            this.TargetTitle.TabIndex = 0;
            this.TargetTitle.Text = "Target Account";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Controls.Add(this.SourceButton);
            this.panel1.Controls.Add(this.SourceAccount);
            this.panel1.Controls.Add(this.SourceTitle);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(506, 104);
            this.panel1.TabIndex = 0;
            // 
            // SourceButton
            // 
            this.SourceButton.AutoSize = true;
            this.SourceButton.Location = new System.Drawing.Point(206, 67);
            this.SourceButton.Name = "SourceButton";
            this.SourceButton.Size = new System.Drawing.Size(94, 34);
            this.SourceButton.TabIndex = 1;
            this.SourceButton.Text = "Connect";
            this.SourceButton.UseVisualStyleBackColor = true;
            this.SourceButton.Click += new System.EventHandler(this.SourceButton_Click);
            // 
            // SourceAccount
            // 
            this.SourceAccount.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SourceAccount.Location = new System.Drawing.Point(-1, 32);
            this.SourceAccount.Name = "SourceAccount";
            this.SourceAccount.Size = new System.Drawing.Size(503, 20);
            this.SourceAccount.TabIndex = 1;
            this.SourceAccount.Text = "No account";
            this.SourceAccount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SourceTitle
            // 
            this.SourceTitle.AutoSize = true;
            this.SourceTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SourceTitle.Location = new System.Drawing.Point(169, 6);
            this.SourceTitle.Name = "SourceTitle";
            this.SourceTitle.Size = new System.Drawing.Size(166, 26);
            this.SourceTitle.TabIndex = 0;
            this.SourceTitle.Text = "Source Account";
            // 
            // ExecuteButton
            // 
            this.ExecuteButton.BackColor = System.Drawing.Color.ForestGreen;
            this.ExecuteButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExecuteButton.ForeColor = System.Drawing.Color.White;
            this.ExecuteButton.Location = new System.Drawing.Point(239, -1);
            this.ExecuteButton.Name = "ExecuteButton";
            this.ExecuteButton.Size = new System.Drawing.Size(88, 29);
            this.ExecuteButton.TabIndex = 2;
            this.ExecuteButton.Text = "Execute";
            this.ExecuteButton.UseVisualStyleBackColor = false;
            this.ExecuteButton.Click += new System.EventHandler(this.ExecuteButton_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 341F));
            this.tableLayoutPanel2.Controls.Add(this.panel8, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.panel7, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.panel6, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.panel5, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel4, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel3, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 116);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 93.50649F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.493506F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1018, 522);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.TargetLog);
            this.panel8.Location = new System.Drawing.Point(341, 491);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(332, 28);
            this.panel8.TabIndex = 5;
            // 
            // TargetLog
            // 
            this.TargetLog.Location = new System.Drawing.Point(6, 4);
            this.TargetLog.Name = "TargetLog";
            this.TargetLog.Size = new System.Drawing.Size(323, 29);
            this.TargetLog.TabIndex = 0;
            this.TargetLog.Text = "Log:";
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.SourceLog);
            this.panel7.Location = new System.Drawing.Point(3, 491);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(332, 28);
            this.panel7.TabIndex = 4;
            // 
            // SourceLog
            // 
            this.SourceLog.Location = new System.Drawing.Point(6, 4);
            this.SourceLog.Name = "SourceLog";
            this.SourceLog.Size = new System.Drawing.Size(323, 29);
            this.SourceLog.TabIndex = 0;
            this.SourceLog.Text = "Log:";
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.RefreshButton);
            this.panel6.Controls.Add(this.ExecuteButton);
            this.panel6.Location = new System.Drawing.Point(679, 491);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(332, 28);
            this.panel6.TabIndex = 2;
            // 
            // RefreshButton
            // 
            this.RefreshButton.BackColor = System.Drawing.SystemColors.ControlDark;
            this.RefreshButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RefreshButton.ForeColor = System.Drawing.Color.White;
            this.RefreshButton.Location = new System.Drawing.Point(99, -1);
            this.RefreshButton.Name = "RefreshButton";
            this.RefreshButton.Size = new System.Drawing.Size(134, 29);
            this.RefreshButton.TabIndex = 3;
            this.RefreshButton.Text = "Refresh Lists";
            this.RefreshButton.UseVisualStyleBackColor = false;
            this.RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.NewTargetListBox);
            this.panel5.Controls.Add(this.label2);
            this.panel5.Location = new System.Drawing.Point(679, 3);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(332, 482);
            this.panel5.TabIndex = 3;
            // 
            // NewTargetListBox
            // 
            this.NewTargetListBox.FormattingEnabled = true;
            this.NewTargetListBox.ItemHeight = 16;
            this.NewTargetListBox.Location = new System.Drawing.Point(0, 20);
            this.NewTargetListBox.Name = "NewTargetListBox";
            this.NewTargetListBox.Size = new System.Drawing.Size(329, 500);
            this.NewTargetListBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(96, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(133, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "New target contacts";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.OldTargetListBox);
            this.panel4.Controls.Add(this.OldTargetListTile);
            this.panel4.Location = new System.Drawing.Point(341, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(332, 482);
            this.panel4.TabIndex = 2;
            // 
            // OldTargetListBox
            // 
            this.OldTargetListBox.FormattingEnabled = true;
            this.OldTargetListBox.ItemHeight = 16;
            this.OldTargetListBox.Location = new System.Drawing.Point(0, 20);
            this.OldTargetListBox.Name = "OldTargetListBox";
            this.OldTargetListBox.Size = new System.Drawing.Size(329, 500);
            this.OldTargetListBox.TabIndex = 1;
            // 
            // OldTargetListTile
            // 
            this.OldTargetListTile.AutoSize = true;
            this.OldTargetListTile.Location = new System.Drawing.Point(96, 0);
            this.OldTargetListTile.Name = "OldTargetListTile";
            this.OldTargetListTile.Size = new System.Drawing.Size(128, 17);
            this.OldTargetListTile.TabIndex = 0;
            this.OldTargetListTile.Text = "Old target contacts";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.SourceListBox);
            this.panel3.Controls.Add(this.SourceListTile);
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(332, 482);
            this.panel3.TabIndex = 0;
            // 
            // SourceListBox
            // 
            this.SourceListBox.FormattingEnabled = true;
            this.SourceListBox.ItemHeight = 16;
            this.SourceListBox.Location = new System.Drawing.Point(0, 20);
            this.SourceListBox.Name = "SourceListBox";
            this.SourceListBox.Size = new System.Drawing.Size(329, 500);
            this.SourceListBox.TabIndex = 1;
            // 
            // SourceListTile
            // 
            this.SourceListTile.AutoSize = true;
            this.SourceListTile.Location = new System.Drawing.Point(96, 0);
            this.SourceListTile.Name = "SourceListTile";
            this.SourceListTile.Size = new System.Drawing.Size(110, 17);
            this.SourceListTile.TabIndex = 0;
            this.SourceListTile.Text = "Source contacts";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1021, 640);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label SourceTitle;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button TargetButton;
        private System.Windows.Forms.Label TargetTitle;
        private System.Windows.Forms.Button SourceButton;
        private System.Windows.Forms.Label SourceAccount;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label SourceListTile;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.ListBox NewTargetListBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.ListBox OldTargetListBox;
        private System.Windows.Forms.Label OldTargetListTile;
        private System.Windows.Forms.ListBox SourceListBox;
        private System.Windows.Forms.Button ExecuteButton;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label SourceLog;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button RefreshButton;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Label TargetLog;
        private System.Windows.Forms.Label TargetAccount;
    }
}