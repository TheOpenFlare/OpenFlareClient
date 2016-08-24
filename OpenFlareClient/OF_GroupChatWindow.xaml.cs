// <copyright file="OF_GroupChatWindow.xaml.cs" company="POQDavid">
//     Copyright (c) POQDavid. All rights reserved.
// </copyright>
// <author>POQDavid</author>
// <summary>
// This is the OF_GroupChatWindow class.
// </summary>
namespace OpenFlareClient
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for OF_GroupChatWindow.xaml
    /// </summary>
    public partial class OF_GroupChatWindow : Window
    {
        /// <summary>
        ///  For jid
        /// </summary>
        private Sharp.Xmpp.Jid jid;

        /// <summary>
        ///  For chats
        /// </summary>
        private Xmpp.Chats<Xmpp.ChatData> xmppChats;

        /// <summary>
        /// Initializes a new instance of the OF_GroupChatWindow class
        /// </summary>
        /// <param name="xxmppChats">All chats</param>
        /// <param name="jjid">Group jid</param>
        public OF_GroupChatWindow(Xmpp.Chats<Xmpp.ChatData> xxmppChats, Sharp.Xmpp.Jid jjid)
        {
            this.InitializeComponent();
            this.xmppChats = new Xmpp.Chats<Xmpp.ChatData>();

            this.jid = jjid;
            OF_Send.Tag = jjid;
            OF_Msg.Tag = jjid;
            this.Title = this.Title + " > " + this.xmppChats;

            ////this.chatBox.Items. = XC.Single(j => j.Jid == C_jid).Msgs;
            //// iXC2.CollectionChanged += XC2_CollectionChanged;
        }

        /// <summary>
        /// Gets or sets the xmppChats property.
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
        /// Gets or sets the jid property.
        /// </summary>
        public Sharp.Xmpp.Jid Jid
        {
            get
            {
                return this.jid;
            }

            set
            {
                this.jid = value;
            }
        }

        /// <summary>
        /// Loaded event for Window
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">The RoutedEventArgs</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}