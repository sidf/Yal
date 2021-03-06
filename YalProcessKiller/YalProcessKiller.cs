﻿using System;
using System.Linq;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;

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
Enter or double clicking on an item will try killing the underlying
process";
        }

        public void SaveSettings()
        {
            pluginUserControl.SaveSettings();
        }

        public List<PluginItem> GetItems(string userInput)
        {
            var processes = Process.GetProcesses();
            return processes.Length > 0 ? Process.GetProcesses().Select(process => new PluginItem()
            {
                Item = process.ProcessName, Info = string.Join(" ", Activator, process.ProcessName),
                IconLocation = Properties.Settings.Default.GetProcessIcons ? Utils.GetProcessFileLocation(process) : null
            }).ToList() : null;
        }

        public void HandleExecution(string input)
        {
            var processName = input.Substring(Activator.Length + 1);

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

        public bool CanHandle(string input)
        {
            return Process.GetProcessesByName(input.Substring(Activator.Length + 1)).Length > 0;
        }
    }
}
