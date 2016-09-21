﻿namespace YalBookmark
{
    partial class YalBookmarkUC
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.numericUpDownTruncate = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.cbOpenWithProvider = new System.Windows.Forms.CheckBox();
            this.listViewBrowsers = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTruncate)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // numericUpDownTruncate
            // 
            this.numericUpDownTruncate.Location = new System.Drawing.Point(258, 3);
            this.numericUpDownTruncate.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownTruncate.Name = "numericUpDownTruncate";
            this.numericUpDownTruncate.Size = new System.Drawing.Size(46, 20);
            this.numericUpDownTruncate.TabIndex = 0;
            this.numericUpDownTruncate.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(249, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Truncate bookmark names longer than (characters)";
            // 
            // cbOpenWithProvider
            // 
            this.cbOpenWithProvider.AutoSize = true;
            this.cbOpenWithProvider.Location = new System.Drawing.Point(3, 28);
            this.cbOpenWithProvider.Name = "cbOpenWithProvider";
            this.cbOpenWithProvider.Size = new System.Drawing.Size(300, 17);
            this.cbOpenWithProvider.TabIndex = 2;
            this.cbOpenWithProvider.Text = "Try visiting the bookmark via the browser that it belongs to";
            this.cbOpenWithProvider.UseVisualStyleBackColor = true;
            // 
            // listViewBrowsers
            // 
            this.listViewBrowsers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listViewBrowsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewBrowsers.FullRowSelect = true;
            this.listViewBrowsers.Location = new System.Drawing.Point(0, 50);
            this.listViewBrowsers.MultiSelect = false;
            this.listViewBrowsers.Name = "listViewBrowsers";
            this.listViewBrowsers.Size = new System.Drawing.Size(307, 132);
            this.listViewBrowsers.TabIndex = 3;
            this.listViewBrowsers.UseCompatibleStateImageBehavior = false;
            this.listViewBrowsers.View = System.Windows.Forms.View.Details;
            this.listViewBrowsers.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listViewBrowsers_MouseClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Browser";
            this.columnHeader1.Width = 107;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Detected profile path";
            this.columnHeader2.Width = 190;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Backend enabled";
            this.columnHeader3.Width = 96;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbOpenWithProvider);
            this.panel1.Controls.Add(this.numericUpDownTruncate);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(307, 50);
            this.panel1.TabIndex = 4;
            // 
            // YalBookmarkUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listViewBrowsers);
            this.Controls.Add(this.panel1);
            this.Name = "YalBookmarkUC";
            this.Size = new System.Drawing.Size(307, 182);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTruncate)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numericUpDownTruncate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbOpenWithProvider;
        private System.Windows.Forms.ListView listViewBrowsers;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Panel panel1;
    }
}