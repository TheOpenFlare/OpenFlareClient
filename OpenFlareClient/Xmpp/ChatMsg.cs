// <copyright file="ChatMsg.cs" company="POQDavid">
// Copyright (c) POQDavid. All rights reserved.
// </copyright>
// <author>POQDavid</author>
// <summary>This is the ChatMsg class.</summary>
namespace OpenFlareClient.Xmpp
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// ChatMsg class
    /// </summary>
    public class ChatMsg : INotifyPropertyChanged
    {
        /// <summary>
        /// Default for Avatar
        /// </summary>
        private BitmapImage defaultAvatar = null;

        /// <summary>
        /// Default for AvatarColumn
        /// </summary>
        private int defaultAvatarColumn = 0;

        /// <summary>
        /// Default for Text
        /// </summary>
        private string defaultText;

        /// <summary>
        /// Default for Name
        /// </summary>
        private string defaultName;

        /// <summary>
        /// Default for TextColumn
        /// </summary>
        private int defaultTextColumn = 1;

        /// <summary>
        /// Default for TextHA
        /// </summary>
        private HorizontalAlignment defaultTextHA = HorizontalAlignment.Center;

        /// <summary>
        /// PropertyChanged event for ChatMsg
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
        /// Gets or sets AvatarColumn
        /// </summary>
        public int AvatarColumn
        {
            get
            {
                return this.defaultAvatarColumn;
            }

            set
            {
                this.defaultAvatarColumn = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets Text
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
        /// Gets or sets Text
        /// </summary>
        public string Text
        {
            get
            {
                return this.defaultText;
            }

            set
            {
                this.defaultText = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets TextColumn
        /// </summary>
        public int TextColumn
        {
            get
            {
                return this.defaultTextColumn;
            }

            set
            {
                this.defaultTextColumn = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets TextHA
        /// </summary>
        public HorizontalAlignment TextHA
        {
            get
            {
                return this.defaultTextHA;
            }

            set
            {
                this.defaultTextHA = value;
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