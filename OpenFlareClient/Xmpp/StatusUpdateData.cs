// <copyright file="StatusUpdateData.cs" company="POQDavid">
// Copyright (c) POQDavid. All rights reserved.
// </copyright>
// <author>POQDavid</author>
// <summary>This is the StatusUpdateData class.</summary>
namespace OpenFlareClient.Xmpp
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// StatusUpdateData class
    /// </summary>
    public class StatusUpdateData : INotifyPropertyChanged
    {
        /// <summary>
        /// Default for Data
        /// </summary>
        private object defaultData = null;

        /// <summary>
        /// Default for DataType
        /// </summary>
        private DataTypes defaultDataType = DataTypes.None;

        /// <summary>
        /// Default for jid
        /// </summary>
        private Sharp.Xmpp.Jid defaultJid;

        /// <summary>
        /// PropertyChanged event for StatusUpdateData
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// enum DataTypes
        /// </summary>
        public enum DataTypes
        {
            /// <summary>
            /// None type
            /// </summary>
            None,

            /// <summary>
            /// Status type
            /// </summary>
            Status,

            /// <summary>
            /// UserTune type
            /// </summary>
            UserTune
        }

        /// <summary>
        /// Gets or sets data
        /// </summary>
        public object Data
        {
            get
            {
                return this.defaultData;
            }

            set
            {
                this.defaultData = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets DataType
        /// </summary>
        public DataTypes DataType
        {
            get
            {
                return this.defaultDataType;
            }

            set
            {
                this.defaultDataType = value;
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
        /// Notifies objects registered to receive this event that a property value has changed.
        /// </summary>
        /// <param name="propertyName">The name of the property that was changed.</param>
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}