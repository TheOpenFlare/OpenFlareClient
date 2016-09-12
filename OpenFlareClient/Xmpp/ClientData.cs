// <copyright file="ClientData.cs" company="POQDavid">
// Copyright (c) POQDavid. All rights reserved.
// </copyright>
// <author>POQDavid</author>
// <summary>This is the ClientData class.</summary>
namespace OpenFlareClient.Xmpp
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// ClientData class
    /// </summary>
    public class ClientData : INotifyPropertyChanged
    {
        /// <summary>
        /// Default for Avatar
        /// </summary>
        private BitmapImage defaultAvatar = null;

        /// <summary>
        /// Default for Jid
        /// </summary>
        private Sharp.Xmpp.Jid defaultJid;

        /// <summary>
        /// Default for MyTune
        /// </summary>
        private Sharp.Xmpp.Extensions.TuneInformation defaultMyTune;

        /// <summary>
        /// Default for Name
        /// </summary>
        private string defaultName = "Loading...";

        /// <summary>
        /// Default for Status
        /// </summary>
        private string defaultStatus = "Offline";

        /// <summary>
        /// Default for StatusColor
        /// </summary>
        private SolidColorBrush defaultStatusColor = new SolidColorBrush(Color.FromArgb(100, 102, 102, 102));

        /// <summary>
        /// Default for StatusMessage
        /// </summary>
        private string defaultStatusMessage = string.Empty;

        /// <summary>
        /// Default for TuneText
        /// </summary>
        private string defaultTuneText = "Not Playing any music!";

        /// <summary>
        /// PropertyChanged event for ChatData
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets Avatar
        /// </summary>
        public BitmapImage Avatar
        {
            get
            {
                return this.defaultAvatar;
            }

            set
            {
                this.defaultAvatar = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets Jid
        /// </summary>
        public Sharp.Xmpp.Jid Jid
        {
            get
            {
                return this.defaultJid;
            }

            set
            {
                this.defaultJid = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets MyTune
        /// </summary>
        public Sharp.Xmpp.Extensions.TuneInformation MyTune
        {
            get
            {
                return this.defaultMyTune;
            }

            set
            {
                this.defaultMyTune = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets name
        /// </summary>
        public string Name
        {
            get
            {
                return this.defaultName;
            }

            set
            {
                this.defaultName = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets status
        /// </summary>
        public string Status
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
        /// Gets or sets StatusColor
        /// </summary>
        public SolidColorBrush StatusColor
        {
            get
            {
                
                return this.defaultStatusColor;
            }

            set
            {
                this.defaultStatusColor = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets StatusMessage
        /// </summary>
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
        /// Gets or sets TuneText
        /// </summary>
        public string TuneText
        {
            get
            {
                return this.defaultTuneText;
            }

            set
            {
                this.defaultTuneText = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Notifies objects registered to receive this event that a property value has changed.
        /// </summary>
        /// <param name="propertyName">The name of the property that was changed.</param>
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}