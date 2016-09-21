﻿using System;
using System.IO;
using System.Linq;
using System.Drawing;
using Microsoft.Win32;
using System.Data.SQLite;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

using Utilities;
using PluginInterfaces;

namespace YalBookmark
{
    struct BrowserInfo
    {
        public string name;
        public string queryString;
        public string executableName;
        public Func<string> GetDbPath;
        public Action QueryDatabase;

        public BrowserInfo(string name, string query, string exeName, Func<string> GetDbPath,
                           Action QueryDatabase)
        {
            this.name = name;
            queryString = query;
            executableName = exeName;
            this.GetDbPath = GetDbPath;
            this.QueryDatabase = QueryDatabase;
        }
    }

    public class YalBookmark : IPlugin
    {
        public string Name { get; } = "YalBookmark";
        public string Version { get; } = "1.0";
        public string Description { get; } = "Yal plugin that allows you to easily visit your browser bookmarks";

        public Icon PluginIcon { get; }
        public bool FileLikeOutput { get; } = true;

        private YalBookmarkUC BookmarkPluginInstance { get; set; }

        internal static Dictionary<string, BrowserInfo> browsers;
        private const string dbConnectionString = "Data Source={0};Version=3;";
        private Dictionary<string, string[]> localQueryCache = new Dictionary<string, string[]>();

        private static string roamingAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private static string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        private static string[] rootRegistryKeys = new string[] { "LOCAL_MACHINE", "CURRENT_USER" };
        private static string appPathsTemplate = @"HKEY_{0}\Software\Microsoft\Windows\CurrentVersion\App Paths\{1}";

        public YalBookmark()
        {
            PluginIcon = Utils.GetPluginIcon(Name);

            browsers = new Dictionary<string, BrowserInfo>()
            {
                { "Firefox",  new BrowserInfo("Firefox", @"select bookmarks.TITLE, places.URL from moz_bookmarks as bookmarks, 
                                                         moz_places as places where bookmarks.fk = places.id and places.REV_HOST is not null",
                                              GetExecutablePath("firefox.exe"), GetFirefoxDbPath, QueryFirefoxDb) },
                { "Chrome", new BrowserInfo("Chrome", "", GetExecutablePath("chrome.exe"), GetChromeDbPath, QueryChromeDb) }
            };

            if (Properties.Settings.Default.FirstRun)
            {
                Properties.Settings.Default.EnabledBackends = new System.Collections.Specialized.StringCollection();
                foreach (var browser in browsers.Keys)
                {
                    Properties.Settings.Default.EnabledBackends.Add(browser);
                }
                Properties.Settings.Default.FirstRun = false;
            }
        }

        private static string GetExecutablePath(string programName)
        {
            foreach (string rootKey in rootRegistryKeys)
            {
                var path = (string)Registry.GetValue(string.Format(appPathsTemplate, rootKey, programName), string.Empty, null);
                if (path != null && File.Exists(path))
                {
                    return path;
                }
            }
            return null;
        }

        private static string GetFirefoxDbPath()
        {
            var firefoxPath = string.Concat(roamingAppData, @"\Mozilla\Firefox");
            var profilesPath = string.Concat(firefoxPath, @"\profiles.ini");
            if (File.Exists(profilesPath))
            {
                string profilePath = string.Empty;
                var content = File.ReadAllLines(profilesPath);

                for (int i = 0; i < content.Length; i++)
                {
                    if (content[i].StartsWith("Path=") && content[i + 1] == "Default=1")
                    {
                        // found the default profile's directory
                        profilePath = content[i].Split('=')[1];
                        break;
                    }
                }

                string placesPath = string.Concat(Path.Combine(firefoxPath, profilePath), @"\places.sqlite");
                if (File.Exists(placesPath))
                {
                    return placesPath;
                }
            }
            return null;
        }

        private static string GetChromeDbPath()
        {
            var bookmarksPath = string.Concat(localAppData, @"\Google\Chrome\User Data\Default\Bookmarks");
            return File.Exists(bookmarksPath) ? bookmarksPath : null;
        }

        private void QueryFirefoxDb()
        {
            var browserInfo = browsers["Firefox"];
            var databasePath = browserInfo.GetDbPath();

            if (databasePath == null)
            {
                return;
            }

            using (var connection = new SQLiteConnection(string.Format(dbConnectionString, databasePath)))
            {
                connection.Open();
                var command = new SQLiteCommand(browserInfo.queryString, connection);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var title = reader["TITLE"].ToString();
                    if (!localQueryCache.ContainsKey(title))
                    {
                        localQueryCache.Add(title, new string[] { browserInfo.name, reader["URL"].ToString() });
                    }
                }
            }
        }

        private void QueryChromeDb()
        {
            var browserInfo = browsers["Chrome"];
            string databasePath = browserInfo.GetDbPath();
            
            if (databasePath == null)
            {
                return;
            }

            string database = File.ReadAllText(databasePath);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            dynamic parsedBookmarks = serializer.DeserializeObject(database);

            foreach (var bookmark in parsedBookmarks["roots"]["bookmark_bar"]["children"])
            {
                var bookmarkName = bookmark["name"];
                if (bookmark["type"] == "url" && !localQueryCache.ContainsKey(bookmarkName))
                {
                    localQueryCache.Add(bookmarkName, new string[] { browserInfo.name, bookmark["url"] });
                }
            }
        }

        public void SaveSettings()
        {
            BookmarkPluginInstance.SaveSettings();
        }

        public UserControl GetUserControl()
        {
            if (BookmarkPluginInstance == null || BookmarkPluginInstance.IsDisposed)
            {
                BookmarkPluginInstance = new YalBookmarkUC();
            }
            return BookmarkPluginInstance;
        }

        public string[] GetResults(string input, out string[] itemInfo)
        {
            itemInfo = null;
            localQueryCache.Clear();

            foreach (var browser in browsers)
            {
                if (!Properties.Settings.Default.EnabledBackends.Contains(browser.Key))
                {
                    continue;
                }
                browser.Value.QueryDatabase();
            }
            return localQueryCache.Keys.ToArray();
        }

        public void HandleExecution(string input)
        {
            var providingBrowser = browsers[localQueryCache[input][0]];
            var url = localQueryCache[input][1];
            var proc = new Process();

            var providingBrowserPath = GetExecutablePath(providingBrowser.executableName);
            if (Properties.Settings.Default.OpenWithProvider && providingBrowserPath != null)
            {
                proc.StartInfo.FileName = providingBrowserPath;
                proc.StartInfo.Arguments = url;
            }
            else
            {
                proc.StartInfo.FileName = url;
            }

            try
            {
                proc.Start();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}