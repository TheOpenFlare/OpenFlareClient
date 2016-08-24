// <copyright file="ChatData.cs" company="POQDavid">
// Copyright (c) POQDavid. All rights reserved.
// </copyright>
// <author>POQDavid</author>
// <summary>This is the ChatData class.</summary>
namespace OpenFlareClient.Xmpp
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// ChatData class
    /// </summary>
    public class ChatData : INotifyPropertyChanged
    {
        /// <summary>
        /// Default for Jid
        /// </summary>
        private Sharp.Xmpp.Jid defaultJid;

        /// <summary>
        /// Default for Msgs
        /// </summary>
        private List<ChatMsg> defaultMsgs = new List<ChatMsg>();

        /// <summary>
        /// Default for
        /// </summary>
        private object defaultMsgsLock = new object();

        /// <summary>
        /// Initializes a new instance of the ChatData class
        /// </summary>
        public ChatData()
        {
            // BindingOperations.EnableCollectionSynchronization(Msgs, _msgsLock);
        }

        /// <summary>
        /// PropertyChanged event for ChatData
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

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
        /// Gets or sets lock
        /// </summary>
        public object MsgsLock
        {
            get
            {
                return this.defaultMsgsLock;
            }

            set
            {
                this.defaultMsgsLock = value;
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