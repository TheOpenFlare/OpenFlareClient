// <copyright file="GroupChatData.cs" company="POQDavid">
// Copyright (c) POQDavid. All rights reserved.
// </copyright>
// <author>POQDavid</author>
// <summary>This is the GroupChatData class.</summary>
namespace OpenFlareClient.Xmpp
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// GroupChatData class
    /// </summary>
    public class GroupChatData : INotifyPropertyChanged
    {
        /// <summary>
        /// Default for Jid
        /// </summary>
        private Sharp.Xmpp.Jid defaultJid;

        /// <summary>
        /// Default for messages
        /// </summary>
        private List<ChatMsg> defaultMsgs = new List<ChatMsg>();

        /// <summary>
        /// Default for name
        /// </summary>
        private string defaultName;

        /// <summary>
        /// PropertyChanged event for GroupChatData
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

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
        /// Gets or sets messages
        /// </summary>
        public List<ChatMsg> Msgs
        {
            get
            {
                return this.defaultMsgs;
            }

            set
            {
                this.defaultMsgs = value;
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