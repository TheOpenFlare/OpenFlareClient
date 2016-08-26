// <copyright file="Settings.cs" company="POQDavid">
//     Copyright (c) POQDavid. All rights reserved.
// </copyright>
// <author>POQDavid</author>
// <summary>
// This is the settings class.
// </summary>
namespace OpenFlareClient
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Security;
    using Newtonsoft.Json;

    /// <summary>
    /// And this part is for the settings :)
    /// <list type="bullet">
    /// <item>
    /// <term>Author</term>
    /// <description>POQDavid</description>
    /// </item>
    /// </list>
    /// </summary>
    [Serializable]
    public class Settings : INotifyPropertyChanged
    {
        /// <summary>
        /// Default value for AutoConnect.
        /// </summary>
        private bool? defaultAutoConnect = false;

        /// <summary>
        /// Default value for Password.
        /// </summary>
        private SecureString defaultPassword = string.Empty.SecureString();

        /// <summary>
        /// Default value for Port.
        /// </summary>
        private string defaultPort = "5222";

        /// <summary>
        /// Default value for Server.
        /// </summary>
        private string defaultServer = "localhost";

        /// <summary>
        /// Default value for Status
        /// </summary>
        private int defaultStatus = 0;

        /// <summary>
        /// Default value for StatusMessage
        /// </summary>
        private string defaultStatusMessage = string.Empty;

        /// <summary>
        /// Default value for UserName.
        /// </summary>
        private string defaultUserName = string.Empty;

        /// <summary>
        /// Number of errors for loading friends list to prevent loop
        /// </summary>
        private int statuserrs = 0;

        /// <summary>
        /// Prevents a default instance of the Settings class from being created.
        /// </summary>
        private Settings()
        {
        }

        /// <summary>
        /// PropertyChanged event for settings
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets a value indicating whether to use AutoConnect or not.
        /// </summary>
        [JsonProperty("AutoConnect")]
        [DefaultValue(false)]
        public bool? AutoConnect
        {
            get
            {
                return this.defaultAutoConnect;
            }

            set
            {
                this.defaultAutoConnect = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets password
        /// </summary>
        [JsonIgnore]
        public SecureString Password
        {
            get
            {
                return Security.AES.Decrypt(this.defaultPassword);
            }

            set
            {
                this.PasswordS = string.Empty;
                this.defaultPassword = Security.AES.Encrypt(value);
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the Port property.
        /// </summary>
        [JsonProperty("Port")]
        [DefaultValue("5222")]
        public string Port
        {
            get
            {
                return this.defaultPort;
            }

            set
            {
                this.defaultPort = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the Server property.
        /// </summary>
        [JsonProperty("Server")]
        [DefaultValue("localhost")]
        public string Server
        {
            get
            {
                return this.defaultServer;
            }

            set
            {
                this.defaultServer = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets Status
        /// </summary>
        [JsonProperty("Status")]
        [DefaultValue(false)]
        public int Status
        {
            get
            {
                return this.defaultStatus;
            }

            set
            {
                this.defaultStatus = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets StatusMessage
        /// </summary>
        [JsonProperty("StatusMessage")]
        [DefaultValue("")]
        public string StatusMessage
        {
            get
            {
                return this.defaultStatusMessage;
            }

            set
            {
                this.defaultStatusMessage = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the UserName property.
        /// </summary>
        [JsonProperty("UserName")]
        [DefaultValue("")]
        public string UserName
        {
            get
            {
                return this.defaultUserName;
            }

            set
            {
                this.defaultUserName = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the Password property.
        /// </summary>
        [JsonProperty("Password")]
        [DefaultValue("")]
        private string PasswordS
        {
            get
            {
                return Security.AES.Encrypt(this.Password).UnsecureString();
            }

            set
            {
                this.defaultPassword = value.SecureString();
                value = string.Empty;
            }
        }

        /// <summary>
        /// This is the public factory method.
        /// </summary>
        /// <returns>Returns settings</returns>
        public static Settings CreateNewSettings()
        {
            return new Settings();
        }

        /// <summary>
        /// Loads the settings from the selected path.
        /// </summary>
        public void LoadSettings()
        {
            try
            {
                if (File.Exists(OpenFlareClient.OF_MainWindow.SettingPath))
                {
                    string json_string = File.ReadAllText(OpenFlareClient.OF_MainWindow.SettingPath);
                    if (Json.IsValid(json_string))
                    {
                        var s = new JsonSerializerSettings();
                        s.NullValueHandling = NullValueHandling.Ignore;
                        s.ObjectCreationHandling = ObjectCreationHandling.Replace; //// without this, you end up with duplicates.

                        //// s.TypeNameHandling = TypeNameHandling.Objects;
                        OpenFlareClient.OF_MainWindow.Settings = JsonConvert.DeserializeObject<Settings>(json_string, s);
                    }
                    else
                    {
                        this.RestSettings();
                    }
                }
                else
                {
                    this.RestSettings();
                }
            }
            catch (Exception)
            {
                if (this.statuserrs < 100)
                {
                    this.RestSettings();
                }
                else
                {
                    throw new System.Exception("Too many errors");
                    ////Status_Errs = 0;
                }
            }
        }

        /// <summary>
        /// Rests the settings in selected path.
        /// </summary>
        public void RestSettings()
        {
            this.SaveSetting();
            this.LoadSettings();
        }

        /// <summary>
        /// Saves the settings in selected path.
        /// </summary>
        public void SaveSetting()
        {
            var s = new JsonSerializerSettings();
            s.ObjectCreationHandling = ObjectCreationHandling.Replace; //// without this, you end up with duplicates.

            File.WriteAllText(OpenFlareClient.OF_MainWindow.SettingPath, JsonConvert.SerializeObject(this, Formatting.Indented, s));
        }

        /// <summary>
        /// Notifies objects registered to receive this event that a property value has changed.
        /// </summary>
        /// <param name="propertyName">The name of the property that was changed.</param>
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}