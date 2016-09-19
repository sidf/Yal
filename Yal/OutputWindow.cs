﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using Utilities;

namespace Yal
{
    public partial class OutputWindow : Form
    {
        Yal MainWindow { get; }

        public OutputWindow(Yal mainWindow)
        {
            InitializeComponent();

            MainWindow = mainWindow;

            CreateLVColumns();

            listViewOutput.ShowItemToolTips = true;
        }

        private void CreateLVColumns()
        {
            //listViewOutput.HeaderStyle = ColumnHeaderStyle.None;
            var c1 = new ColumnHeader() { Name="ColName", Text="Name"};
            var c2 = new ColumnHeader() { Name = "ColFullName", Text = "FullName" };
            listViewOutput.Columns.AddRange(new ColumnHeader[] { c1, c2 });
        }

        internal void ResizeToFitContent()
        {
            int neededRows = Math.Min(Properties.Settings.Default.MaxVisible, listViewOutput.Items.Count);
            this.Size = new Size(this.Size.Width, (neededRows * listViewOutput.TileSize.Height) + 4); // add 4px to the heigh to compensate for borders

            // dynamically change the tile's width based on the number of items (by considering the state of the vert. scrollbar)
            //listViewOutput.TileSize = new Size(listViewOutput.Items.Count <= Properties.Settings.Default.MaxVisible ? 
            //                                   listViewOutput.Size.Width : listViewOutput.Size.Width - 17, 
            //                                   listViewOutput.TileSize.Height);
            listViewOutput.TileSize = new Size(listViewOutput.ClientSize.Width,
                                               listViewOutput.TileSize.Height);
        }

        private void listViewOutput_MouseEnter(object sender, EventArgs e)
        {
            listViewOutput.Focus();
        }

        private void listViewOutput_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                BuildContextMenu(Cursor.Position);
            }
        }

        internal void BuildContextMenu(Point location)
        {
            var contextMenu = new ContextMenuStrip();

            var runItem = new ToolStripMenuItem("Run");
            runItem.Click += RunItem_Click;

            var runAsAdminItem = new ToolStripMenuItem("Run as administrator");
            runAsAdminItem.Click += RunAsAdminItem_Click;

            var copyNameItem = new ToolStripMenuItem("Copy name");
            copyNameItem.Click += CopyNameItem_Click;

            if (File.Exists(listViewOutput.SelectedItems[0].SubItems[1].Text))
            {
                var copyPathItem = new ToolStripMenuItem("Copy path");
                copyPathItem.Click += CopyPathItem_Click;

                var openDirItem = new ToolStripMenuItem("Open containing directory");
                openDirItem.Click += OpenDirItem_Click;
                contextMenu.Items.AddRange(new ToolStripItem[] { openDirItem, copyPathItem });
            }

            contextMenu.Items.AddRange(new ToolStripItem[] { copyNameItem, runItem, runAsAdminItem });
            contextMenu.Show(location);
        }

        private void CopyPathItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(listViewOutput.SelectedItems[0].SubItems[1].Text);
        }

        private void CopyNameItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(listViewOutput.SelectedItems[0].SubItems[0].Text);
        }

        private void OpenDirItem_Click(object sender, EventArgs e)
        {
            Utils.OpenFileDirectory(listViewOutput.SelectedItems[0].SubItems[1].Text);
        }

        private void RunAsAdminItem_Click(object sender, EventArgs e)
        {
            MainWindow.StartSelectedItem(elevatedRights: true);
        }

        private void RunItem_Click(object sender, EventArgs e)
        {
            MainWindow.StartSelectedItem();
        }
    }
}