// <copyright file="BuddiesData.cs" company="POQDavid">
// Copyright (c) POQDavid. All rights reserved.
// </copyright>
// <author>POQDavid</author>
// <summary>This is the BuddiesData class.</summary>
namespace OpenFlareClient.Xmpp
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// BuddiesData class
    /// </summary>
    public class BuddiesData : INotifyPropertyChanged
    {
        /// <summary>
        /// Default for avatar
        /// </summary>
        private BitmapImage defaultAvatar = null;

        /// <summary>
        /// Default for jid
        /// </summary>
        private Sharp.Xmpp.Jid defaultJid;

        /// <summary>
        /// Default for user tune
        /// </summary>
        private Sharp.Xmpp.Extensions.TuneInformation defaultmyTune;

        /// <summary>
        /// Default for name
        /// </summary>
        private string defaultName = string.Empty;

        /// <summary>
        /// Default for pending
        /// </summary>
        private bool defaultPending;

        /// <summary>
        /// Default for status
        /// </summary>
        private string defaultStatus = "Offline";

        /// <summary>
        /// Default for StatusColor
        /// </summary>
        private SolidColorBrush defaultStatusColor = new SolidColorBrush(Color.FromArgb(100, 102, 102, 102));

        /// <summary>
        /// Default for StatusMessage
        /// </summary>
        private string defaultStatusMessage = "...";

        /// <summary>
        /// Default for SubscriptionState
        /// </summary>
        private string defaultSubscriptionState = "None";

        /// <summary>
        /// Default for tune text
        /// </summary>
        private string defaultTuneText = "Not Playing any music!";

        /// <summary>
        /// Default for tune text visibility
        /// </summary>
        private Visibility defaultTuneTextVisibility = Visibility.Collapsed;

        /// <summary>
        /// PropertyChanged event for BuddiesData
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets avatar
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
        /// Gets or sets jid
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
        /// Gets or sets user tune
        /// </summary>
        public Sharp.Xmpp.Extensions.TuneInformation MyTune
        {
            get
            {
                return this.defaultmyTune;
            }

            set
            {
                this.defaultmyTune = value;
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
                if (this.defaultName.IsNullOrEmpty())
                {
                    return "@" + this.defaultJid.GetBareJid().ToString().Split('@')[0];
                }
                else
                {
                    return this.defaultName;
                }
            }

            set
            {
                this.defaultName = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the stats is pending or not.
        /// </summary>
        public bool Pending
        {
            get
            {
                return this.defaultPending;
            }

            set
            {
                this.defaultPending = value;
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
        /// Gets or sets status message
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
        /// Gets or sets subscription state
        /// </summary>
        public string SubscriptionState
        {
            get
            {
                return this.defaultSubscriptionState;
            }

            set
            {
                this.defaultSubscriptionState = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets tune text
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
        /// Gets or sets tune text Visibility
        /// </summary>
        public Visibility TuneTextVisibility
        {
            get
            {
                return this.defaultTuneTextVisibility;
            }

            set
            {
                this.defaultTuneTextVisibility = value;
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