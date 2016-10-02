// <copyright file="App.xaml.cs" company="POQDavid">
//     Copyright (c) POQDavid. All rights reserved.
// </copyright>
// <author>POQDavid</author>
// <summary>
// This is the OF_MainWindow class.
// </summary>
namespace OpenFlareClient
{
    using System;
    using System.IO;
    using System.Windows;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// This is simply where client stores data and loads them from.
        /// </summary>
        /// <returns>directory for the client's data and settings.</returns>
        private static string settingPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\OpenFlare\\Client", @"Settings.json");

        /// <summary>
        /// Initializes a new instance of the App class
        /// </summary>
        public App()
        {
            if (Environment.GetCommandLineArgs().Length > 0)
            {
                int i = 0;
                foreach (string arg in Environment.GetCommandLineArgs())
                {
                    if (arg == "-setpath")
                    {
                        settingPath = Environment.GetCommandLineArgs()[(i + 1)];
                    }

                    i++;
                }
            }
            else
            {
            }
        }

        /// <summary>
        /// Gets or sets the SettingPath property.
        /// </summary>
        /// <value>Path for the client's settings.</value>
        public static string SettingPath
        {
            get { return settingPath; }
            set { settingPath = value; }
        }
    }
}