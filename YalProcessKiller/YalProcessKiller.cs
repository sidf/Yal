﻿using System;
using System.Linq;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

using Utilities;
using PluginInterfaces;

namespace YalProcessKiller
{
    public class YalProcessKiller : IPlugin
    {
        public string Name { get; } = "YalProcessKiller";
        public string Version { get; } = "1.0";
        public string Description { get; } = "Easily stop processes through Yal";
        public string Activator { get; } = "kill";
        public PluginItemSortingOption SortingOption { get; } = PluginItemSortingOption.ByNameLength;

        public Icon PluginIcon { get; }
        public string HelpText { get; }

        public UserControl PluginUserControl
        {
            get
            {
                if (pluginUserControl == null || pluginUserControl.IsDisposed)
                {
                    pluginUserControl = new YalProcessKillerUC();
                }
                return pluginUserControl;
            }
        }

        private YalProcessKillerUC pluginUserControl;

        public YalProcessKiller()
        {
            PluginIcon = Utils.GetPluginIcon(Name);

            HelpText = $@"To list killable processes simply type the '{Activator}' keyword. 
It will give you a list of processes and their names. Hitting
Enter or double clicking an item will try killing the underlying
process";
        }

        public void SaveSettings()
        {
            pluginUserControl.SaveSettings();
        }

        public string[] GetItems(string input, out string[] itemInfo)
        {
            itemInfo = null;
            return Process.GetProcesses().Select(process => string.Join(" ", Activator, process.ProcessName)).ToArray();
        }

        public void HandleExecution(string input)
        {
            var processName = input.Split()[1];

            foreach (var process in Process.GetProcessesByName(processName))
            {
                try
                {
                    if (!Properties.Settings.Default.WarnBeforeKill || MessageBox.Show($@"Kill '{process.ProcessName}' (ID - {process.Id})", $"{Name}",
                                                                                       MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                    {
                        process.Kill();
                    }
                }
                catch (InvalidOperationException)
                {
                    // process already exited
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
