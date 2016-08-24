// <copyright file="OF_ChatWindow.xaml.cs" company="POQDavid">
//     Copyright (c) POQDavid. All rights reserved.
// </copyright>
// <author>POQDavid</author>
// <summary>
// This is the OF_ChatWindow class.
// </summary>
namespace OpenFlareClient
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for ChatWindow.xaml
    /// </summary>
    public partial class OF_ChatWindow : Window
    {
        /// <summary>
        /// For chats
        /// </summary>
        private Xmpp.Chats<Xmpp.ChatData> xmppChats;

        /// <summary>
        /// For jid
        /// </summary>
        private Sharp.Xmpp.Jid xmppJid;

        /// <summary>
        /// Initializes a new instance of the OF_ChatWindow class
        /// </summary>
        /// <param name="xmppChats">All chats</param>
        /// <param name="jid">Jid of the chat</param>
        public OF_ChatWindow(Xmpp.Chats<Xmpp.ChatData> xmppChats, Sharp.Xmpp.Jid jid)
        {
            this.InitializeComponent();
            this.xmppChats = new Xmpp.Chats<Xmpp.ChatData>();

            this.xmppJid = jid;
            OF_SendButton.Tag = jid;
            OF_MsgBox.Tag = jid;
            this.Title = this.Title + " > " + this.xmppJid;

            ////this.chatBox.Items. = XC.Single(j => j.Jid == C_jid).Msgs;
            this.xmppChats.CollectionChanged += this.XmppChats_CollectionChanged;
        }

        /// <summary>
        /// Gets or sets xmppChats
        /// </summary>
        public Xmpp.Chats<Xmpp.ChatData> XmppChats
        {
            get
            {
                return this.xmppChats;
            }

            set
            {
                this.xmppChats = value;
            }
        }

        /// <summary>
        /// Gets or sets xmppJid
        /// </summary>
        public Sharp.Xmpp.Jid XmppJid
        {
            get
            {
                return this.xmppJid;
            }

            set
            {
                this.xmppJid = value;
            }
        }

        /// <summary>
        /// SourceUpdated event for OF_ChatBox
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">The DataTransferEventArgs</param>
        private void OF_ChatBox_SourceUpdated(object sender, DataTransferEventArgs e)
        {
        }

        /// <summary>
        /// KeyDown event for OF_MsgBox
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">The KeyEventArgs</param>
        private void OF_MsgBox_KeyDown(object sender, KeyEventArgs e)
        {
        }

        /// <summary>
        /// TextChanged event for OF_MsgBox
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">The TextChangedEventArgs</param>
        private void OF_MsgBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        /// <summary>
        /// Click event for OF_SendButton
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">The RoutedEventArgs</param>
        private void OF_SendButton_Click(object sender, RoutedEventArgs e)
        {
        }

        /// <summary>
        /// CollectionChanged event for XmppChats
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">The NotifyCollectionChangedEventArgs</param>
        private void XmppChats_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //// this.chatBox.ItemsSource = XC2.Single(j => j.Jid == C_jid2).Msgs;
            ////  MessageBox.Show("");
        }
    }
}