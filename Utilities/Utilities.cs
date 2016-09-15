﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.IO;

namespace Utilities
{
    public static class Utils
    {
        public static string GetOsVersion()
        {
            using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion"))
            {
                object value = rk.GetValue("ProductName");
                return Regex.Match(value.ToString(), @"^Windows (Vista|\d+(\.\d+)?) [\w\s]+$").Groups[1].Value;
            }
        }

        public static void OpenFileDirectory(string filePath)
        {
            if (File.Exists(filePath))
            {
                Process.Start(Path.GetDirectoryName(filePath));
            }
        }
    }
}
