// <copyright file="OF_MainWindow.xaml.cs" company="POQDavid">
//     Copyright (c) POQDavid. All rights reserved.
// </copyright>
// <author>POQDavid</author>
// <summary>
// This is the OF_MainWindow class.
// </summary>
namespace OpenFlareClient
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Interop;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Media.Imaging;
    using System.Xml;
    using Gat.Controls;
    using Sharp.Xmpp;
    using Sharp.Xmpp.Client;
    using Sharp.Xmpp.Extensions;
    using Sharp.Xmpp.Im;

    /// <summary>
    /// Interaction logic for OF_MainWindow.xaml
    /// </summary>
    public partial class OF_MainWindow : Window
    {
        /// <summary>
        /// Winamp's title
        /// </summary>
        private const string WinampTitle = "Winamp v1.x";

        /// <summary>
        /// Communication Timeout In milliseconds; Indicates a disconnection if timeout is reached
        /// Default value 30000 ms (30s)
        /// -1 Indicates no timeout
        /// </summary>
        private static int defaultTimeOut = 20000;

        /// <summary>
        /// This is simply where client stores data and loads them from.
        /// </summary>
        /// <returns>directory for the client's data and settings.</returns>
        private static string settingPath = @"Settings.json";

        /// <summary>
        /// Album string
        /// </summary>
        private string album = null;

        /// <summary>
        /// Lock object for chats
        /// </summary>
        private object allchatsLock = new object();

        /// <summary>
        /// Artist string
        /// </summary>
        private string artist = null;

        /// <summary>
        /// List of joined muc (Currently not used)
        /// </summary>
        private List<string> joinedmuc = new List<string>();

        /// <summary>
        /// Music length
        /// </summary>
        private int length = 0;

        /// <summary>
        /// For the last track name
        /// </summary>
        private string oldTrack = string.Empty;

        /// <summary>
        /// The period with which ping messages are sent in msec, 10minX30sec
        /// </summary>
        private int pingPeriod = 5000;

        /// <summary>
        /// Timer for pinging xmpp server
        /// </summary>
        private Timer pingTimer;

        /// <summary>
        /// Player's status
        /// </summary>
        private string playerstatus = string.Empty;

        /// <summary>
        /// Boolean for player status
        /// </summary>
        private bool playingmusic = false;

        /// <summary>
        /// Xmpp Connection Resource
        /// </summary>
        private string resource = "OpenFlareClient";

        /// <summary>
        /// Number of errors for loading friends list to prevent loop
        /// </summary>
        private int statuserrs = 0;

        /// <summary>
        /// Separator for music data from player plugin
        /// </summary>
        private string[] stringSeparators = new string[] { "<DATASEPARATOR>" };

        /// <summary>
        /// Main thread for xmpp connection
        /// </summary>
        private Thread threadxmppconnect;

        /// <summary>
        /// Thread for xmpp loading xmpp data
        /// </summary>
        private Thread threadxmppload;

        /// <summary>
        /// Music title
        /// </summary>
        private string title = null;

        /// <summary>
        /// Track string
        /// </summary>
        private string track = null;

        /// <summary>
        /// Handle for winamp window
        /// </summary>
        private IntPtr winampwindow;

        /// <summary>
        /// XmppClient object
        /// </summary>
        private XmppClient xc;

        /// <summary>
        /// Initializes a new instance of the OF_MainWindow class
        /// </summary>
        public OF_MainWindow()
        {
            Security.Generator.GenerateHWIDValue();
            Settings = Settings.CreateNewSettings();
            Settings.LoadSettings();

            this.XmppClientData = new Xmpp.ClientData();
            this.BuddiesList = new Xmpp.Buddies<Xmpp.BuddiesData>();
            this.AllChats = new Xmpp.Chats<Xmpp.ChatData>();
            this.AllGroupChats = new Xmpp.GroupChats<Xmpp.GroupChatData>();

            Attribute attr = Assembly.GetExecutingAssembly().GetCustomAttribute(typeof(AssemblyProductAttribute));
            string name = attr != null ? ((AssemblyProductAttribute)attr).Product : "OpenFlare";
            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            BindingOperations.EnableCollectionSynchronization(this.AllChats, this.allchatsLock);

            this.InitializeComponent();

            OF_Login_Screen.Visibility = Visibility.Visible;
            OF_Password.Password = Settings.Password.UnsecureString();
            this.AllChats.CollectionChanged += this.AllChats_CollectionChanged;
            this.threadxmppconnect = new Thread(this.Connect);
            this.threadxmppload = new Thread(this.XmppLoad);

            this.CreateXMPP();

            if (Settings.AutoConnect == true)
            {
                OF_Login.IsEnabled = false;
                this.threadxmppconnect.SmartStart(this.Connect);
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

        /// <summary>
        /// Gets or sets the Settings property.
        /// </summary>
        /// <value>Client Settings.</value>
        public static Settings Settings { get; set; }

        /// <summary>
        /// Gets or sets the AllGroupChats property.
        /// </summary>
        public Xmpp.GroupChats<Xmpp.GroupChatData> AllGroupChats { get; set; }

        /// <summary>
        /// Gets or sets the BuddiesList property.
        /// </summary>
        public Xmpp.Buddies<Xmpp.BuddiesData> BuddiesList { get; set; }

        /// <summary>
        /// Gets or sets the XCD property.
        /// </summary>
        public Xmpp.ClientData XmppClientData { get; set; }

        /// <summary>
        /// Gets or sets the AllChats property.
        /// </summary>
        private Xmpp.Chats<Xmpp.ChatData> AllChats { get; set; }

        /// <summary>
        /// Converts BitmapImage to System.Drawing.Icon
        /// </summary>
        /// <param name="bitmapImage">BitmapImage to convert</param>
        /// <returns>Converted BitmapImage</returns>
        public static System.Drawing.Icon Convert(BitmapImage bitmapImage)
        {
            var ms = new MemoryStream();
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
            encoder.Save(ms);
            var bmp = new System.Drawing.Bitmap(ms);
            return System.Drawing.Icon.FromHandle(bmp.GetHicon());
        }

        /// <summary>
        /// Escaping string for use in XML
        /// </summary>
        /// <param name="unescaped">String to escape</param>
        /// <returns>Escaped string</returns>
        public static string XmlEscape(string unescaped)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode node = doc.CreateElement("root");
            node.InnerText = unescaped;
            return node.InnerXml;
        }

        /// <summary>
        /// Unescaping string
        /// </summary>
        /// <param name="escaped">Escaped string</param>
        /// <returns>Unescaped string</returns>
        public static string XmlUnescape(string escaped)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode node = doc.CreateElement("root");
            node.InnerXml = escaped;
            return node.InnerText;
        }

        /// <summary>
        /// To check if Buddies list contain the Jid
        /// </summary>
        /// <param name="j">Jid to check</param>
        /// <returns>True or false</returns>
        public bool BuddiesContains(Jid j)
        {
            bool temp = false;
            foreach (Xmpp.BuddiesData d in this.BuddiesList)
            {
                if (d.Jid.GetBareJid() == j.GetBareJid())
                {
                    temp = true;
                }
            }

            return temp;
        }

        /// <summary>
        /// To close everything properly
        /// </summary>
        public void CloseAll()
        {
            this.XClose();

            if (this.pingTimer != null)
            {
                this.pingTimer.Dispose();
            }

            this.pingTimer = null;

            try
            {
                this.xc.Close();
            }
            catch (Exception)
            {
            }

            this.Close();
        }

        /// <summary>
        /// Connect using parameters provided in the constructor
        /// </summary>
        public void Connect()
        {
            // Connects using the Resource name. The catch block will raise appropriate high level error codes
            try
            {
                this.xc.Password = Settings.Password.UnsecureString();
                this.xc.Username = Settings.UserName;
                this.xc.Connect(this.resource); // Its not async, so we are waiting to return
                                                // Set Status with Presence Online and Priority -1
                this.xc.SetStatus(Availability.Online, null, 1);

                if (this.xc.Connected)
                {
                    this.pingTimer = new Timer((o) => { PingServer(); }, null, this.pingPeriod, this.pingPeriod);

                    this.winampwindow = Win32.FindWindow(WinampTitle, null);

                    Win32.SendMessage(this.winampwindow, Win32.IPCOFGET, 0, 0);

                    this.threadxmppload.SmartStart(this.XmppLoad);

                    this.SetLoginSCV(Visibility.Hidden);
                }
            }
            catch (System.Security.Authentication.AuthenticationException e)
            {
                this.Dispose(true);

                this.SetXMPPSTATUS("Auth Exception, Authenication failed: " + e.Message);
                this.threadxmppconnect.Abort();
            }
            catch (System.IO.IOException e)
            {
                this.Dispose(true);
                this.SetXMPPSTATUS("Net Exception: " + e.Message);
                this.threadxmppconnect.Abort();
            }
            catch (XmppException e)
            {
                this.Dispose(true);

                this.SetXMPPSTATUS("XML Exception: " + e.Message);
                this.threadxmppconnect.Abort();
            }
            catch (Exception e)
            {
                this.Dispose(true);

                this.SetXMPPSTATUS("XML Exception: " + e.Message);
                this.threadxmppconnect.Abort();
            }
        }

        /// <summary>
        /// Int to availability
        /// </summary>
        /// <param name="avb">Availability in int</param>
        /// <returns>Converted int to availability</returns>
        public Availability GetFromInt(int avb)
        {
            Availability temp = Availability.Online;

            if (avb == 1)
            {
                temp = Availability.Online;
            }

            if (avb == 2)
            {
                temp = Availability.Away;
            }

            if (avb == 3)
            {
                temp = Availability.Chat;
            }

            if (avb == 4)
            {
                temp = Availability.Dnd;
            }

            if (avb == 5)
            {
                temp = Availability.Xa;
            }

            return temp;
        }

        /// <summary>
        /// String to availability
        /// </summary>
        /// <param name="avb">Availability in string</param>
        /// <returns>Converted string to availability</returns>
        public Availability GetFromString(string avb)
        {
            Availability temp = Availability.Online;

            if (avb == "Online")
            {
                temp = Availability.Online;
            }

            if (avb == "Away")
            {
                temp = Availability.Away;
            }

            if (avb == "Chat")
            {
                temp = Availability.Chat;
            }

            if (avb == "Dnd")
            {
                temp = Availability.Dnd;
            }

            if (avb == "Xa")
            {
                temp = Availability.Xa;
            }

            return temp;
        }

        /// <summary>
        /// Pings one time to the XMPP server and an appropriate error event is raised if disconnected
        /// </summary>
        public void PingServer()
        {
            try
            {
                var t = this.xc.Ping(this.XmppClientData.Jid); // Pings the server
            }
            catch (XmppDisconnectionException)
            {
            }
            catch (InvalidOperationException ex)
            {
                this.Dispose(true);
                this.SetXMPPSTATUS("Error Pinging, not connected to server: " + ex.Message);
                this.threadxmppconnect.Abort();
            }
            catch (NotSupportedException ex)
            {
                this.Dispose(true);
                this.SetXMPPSTATUS("Error Pinging, not supported by server: " + ex.Message);
                this.threadxmppconnect.Abort();
            }
            catch (IOException ex)
            {
                this.Dispose(true);
                this.SetXMPPSTATUS("Error Pinging, IO exception: " + ex.Message);
                this.threadxmppconnect.Abort();
            }
            catch (XmppException ex)
            {
                this.Dispose(true);
                this.SetXMPPSTATUS("Error Pinging, Generic Exception: " + ex.Message);
                this.threadxmppconnect.Abort();
            }
        }

        /// <summary>
        /// Pings one time to the XMPP server and retures true or fales
        /// </summary>
        /// <returns>True or fales</returns>
        public bool PingServerBool()
        {
            try
            {
                var t = this.xc.Ping(this.XmppClientData.Jid); // Pings the server
            }
            catch (XmppDisconnectionException)
            {
                return false;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
            catch (NotSupportedException)
            {
                return false;
            }
            catch (IOException)
            {
                return false;
            }
            catch (XmppException)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Sends chat to the selected jid
        /// </summary>
        /// <param name="jid">Jid to send the chat to</param>
        public void SendChat(Sharp.Xmpp.Jid jid)
        {
            OF_ChatWindow chatWindow = Application.Current.Windows.OfType<OF_ChatWindow>().Single(x => ((Sharp.Xmpp.Jid)x.Tag).GetBareJid() == jid.GetBareJid());
            if (!string.IsNullOrEmpty(chatWindow.OF_MsgBox.Text))
            {
                Xmpp.ChatMsg xmppChatMsg = new Xmpp.ChatMsg();
                xmppChatMsg.AvatarColumn = 2;
                xmppChatMsg.TextColumn = 1;
                xmppChatMsg.TextHA = HorizontalAlignment.Left;
                xmppChatMsg.Name = this.XmppClientData.Name + ": ";
                xmppChatMsg.Text = " " + chatWindow.OF_MsgBox.Text;
                xmppChatMsg.Avatar = this.XmppClientData.Avatar;

                this.xc.SendMessage(jid.GetBareJid(), chatWindow.OF_MsgBox.Text);
                this.AddChat(jid.GetBareJid(), xmppChatMsg);
                chatWindow.OF_ChatBox.ItemsSource = this.AllChats.Single(j => j.Jid.GetBareJid() == jid.GetBareJid()).Msgs;
                chatWindow.OF_ChatBox.Items.Refresh();
                chatWindow.OF_MsgBox.Text = string.Empty;
            }
        }

        /// <summary>
        /// Sends muc chat to the selected jid
        /// </summary>
        /// <param name="jid">Jid to send the chat to</param>
        public void SendMucChat(Sharp.Xmpp.Jid jid)
        {
            OF_GroupChatWindow groupChatWindow = Application.Current.Windows.OfType<OF_GroupChatWindow>().Single(x => ((Sharp.Xmpp.Jid)x.Tag).GetBareJid() == jid.GetBareJid());
            //// OF_ChatWindow CW = Application.Current.Windows.OfType<OF_ChatWindow>().Single(x =>
            //// ((Sharp.Xmpp.Jid)x.Tag).GetBareJid() == D.GetBareJid());
            if (!string.IsNullOrEmpty(groupChatWindow.OF_Msg.Text))
            {
                Xmpp.ChatMsg xmppChatMsg = new Xmpp.ChatMsg();
                xmppChatMsg.AvatarColumn = 2;
                xmppChatMsg.TextColumn = 1;
                xmppChatMsg.TextHA = HorizontalAlignment.Left;
                xmppChatMsg.Name = this.XmppClientData.Name + ": ";
                xmppChatMsg.Text = " " + groupChatWindow.OF_Msg.Text;
                xmppChatMsg.Avatar = this.XmppClientData.Avatar;

                this.xc.SendMessage(jid, groupChatWindow.OF_Msg.Text, null, null, MessageType.Groupchat);
                this.AddChat(jid, xmppChatMsg);
                //// OFGCW.OF_Chat.ItemsSource = AllChats.Single(j =>
                //// j.Jid.ToString().Contains(D.GetBareJid().ToString())).Msgs; //AllChats.Single(j =>
                //// j.Jid.GetBareJid() == D.GetBareJid()).Msgs;
                groupChatWindow.OF_Chat.Items.Refresh();
                groupChatWindow.OF_Msg.Text = string.Empty;
            }
        }

        /// <summary>
        /// Set login screen visibility
        /// </summary>
        /// <param name="vss">Visibility inpute</param>
        public void SetLoginSCV(Visibility vss)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                OF_Login_Screen.Visibility = vss;
            }));
        }

        /// <summary>
        /// To set the status
        /// </summary>
        public void SetStatus()
        {
            ComboBoxItem s = (ComboBoxItem)OF_Status.SelectedItem;
            if (this.xc.Connected)
            {
                this.xc.SetStatus(this.GetFromString(s.Content.ToString()), OF_StatusText.Text, 1);
            }
        }

        /// <summary>
        /// To set XMPPSTATUS content
        /// </summary>
        /// <param name="content">Status message</param>
        public void SetXMPPSTATUS(string content)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                OF_XMPPSTATUS.Content = content;
                OF_XMPPSTATUS.Foreground = Brushes.Red;
            }));
        }

        /// <summary>
        /// An application-defined function that processes messages sent to a window. The WNDPROC type defines a pointer to this callback function.
        /// </summary>
        /// <param name="hwnd">A handle to the window.</param>
        /// <param name="msg">The message.</param>
        /// <param name="wParam">Additional message information. The contents of this parameter depend on the value of the uMsg parameter.  wParam</param>
        /// <param name="lParam">Additional message information. The contents of this parameter depend on the value of the uMsg parameter.  lParam</param>
        /// <param name="handled">ref bool handled</param>
        /// <returns>The return value is the result of the message processing and depends on the message sent.</returns>
        public IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            try
            {
                if (msg == Win32.WMCOPYDATA)
                {
                    Win32.CopyDataStruct cp = (Win32.CopyDataStruct)Marshal.PtrToStructure(lParam, typeof(Win32.CopyDataStruct));
                    TuneInformation tif = new TuneInformation(null, null, null, 0, 0, null, null);

                    this.playerstatus = cp.LpData.Split(this.stringSeparators, StringSplitOptions.None)[1];
                    if (cp.LpData.Split(this.stringSeparators, StringSplitOptions.None)[0] != this.oldTrack)
                    {
                        this.oldTrack = cp.LpData.Split(this.stringSeparators, StringSplitOptions.None)[0];
                        TagLib.File file = TagLib.File.Create(this.oldTrack);
                        this.title = XmlEscape(file.Tag.Title);
                        this.artist = XmlEscape(file.Tag.FirstPerformer);
                        this.track = XmlEscape(file.Tag.Track.ToString());
                        this.length = file.Properties.Duration.Seconds;
                        this.album = XmlEscape(file.Tag.Album);
                    }

                    switch (this.playerstatus)
                    {
                        case "STATUS:playing":
                            this.playingmusic = true;
                            break;

                        case "STATUS:paused":
                            this.playingmusic = false;

                            break;

                        case "STATUS:not_playing":
                            this.playingmusic = false;
                            break;

                        default:
                            this.playingmusic = false;
                            break;
                    }

                    if (this.playingmusic)
                    {
                        tif = new TuneInformation(this.title, this.artist, this.track, this.length, 0, this.album, null);
                        this.XmppClientData.MyTune = tif;
                        this.XmppClientData.TuneText = XmlUnescape(this.artist) + " - " + XmlUnescape(this.title);
                    }
                    else
                    {
                        tif = new TuneInformation(null, null, null, 0, 0, null, null);
                        this.XmppClientData.MyTune = tif;
                        this.XmppClientData.TuneText = "Not Playing any music!";
                    }

                    //// MessageBox.Show(xArtist);

                    if (this.xc.Connected)
                    {
                        this.xc.SetTune(tif);
                    }
                }
            }
            catch (Exception)
            {
            }

            return IntPtr.Zero;
        }

        /// <summary>
        /// To store chat for each Jid
        /// </summary>
        /// <param name="e">The Jid</param>
        /// <param name="msg">The message</param>
        private void AddChat(Sharp.Xmpp.Jid e, Xmpp.ChatMsg msg)
        {
            if (!this.AllChats.Any(j => j.Jid == e))
            {
                Xmpp.ChatData xmppChatData = new Xmpp.ChatData();
                xmppChatData.Jid = e;

                xmppChatData.Msgs.Add(msg);

                this.AllChats.Add(xmppChatData);
            }
            else
            {
                this.AllChats.Single(j => j.Jid == e).Msgs.Add(msg);
            }
        }

        /// <summary>
        /// CollectionChanged event for AllChats
        /// </summary>
        /// <param name="sender">The sender AllChats</param>
        /// <param name="e">CollectionChanged EventArgs</param>
        private void AllChats_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
        }

        /// <summary>
        /// Callback for vcard data
        /// </summary>
        /// <param name="vcard">VCard Data</param>
        /// <param name="jid">The jid for the vcard</param>
        private void Callback(Sharp.Xmpp.Extensions.VCardsData vcard, Sharp.Xmpp.Jid jid)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                if (jid.GetBareJid() == XmppClientData.Jid.GetBareJid())
                {
                    XmppClientData.Avatar = BitmapImageExtension.ByteToBitmap(vcard.Avatar);
                    if (XmppClientData.Name == "Loading...")
                    {
                        XmppClientData.Name = vcard.NickName;
                    }
                }
                else
                {
                    BuddiesList.Single(j => j.Jid.GetBareJid() == jid.GetBareJid()).Avatar = BitmapImageExtension.ByteToBitmap(vcard.Avatar);
                    if (BuddiesList.Single(j => j.Jid.GetBareJid() == jid.GetBareJid()).Name == "Loading...")
                    {
                        BuddiesList.Single(j => j.Jid.GetBareJid() == jid.GetBareJid()).Name = vcard.NickName;
                    }
                }
            }));
        }

        /// <summary>
        /// Getting everything ready for xmpp
        /// </summary>
        private void CreateXMPP()
        {
            try
            {
                this.xc = new XmppClient(Settings.Server, int.Parse(Settings.Port));
                this.xc.RosterUpdated += this.XC_RosterUpdated;
                this.xc.MoodChanged += this.XC_MoodChanged;
                this.xc.StatusChanged += this.XC_StatusChanged;
                ////this.xc.Im.Status += XC_Status;

                this.xc.DefaultTimeOut = defaultTimeOut;
                this.xc.Message += this.XC_Message;
                this.xc.ActivityChanged += this.XC_ActivityChanged;

                ////xc.DirectMucInvitationReceived += XC_DirectMucInvitationReceived;
                this.xc.Im.SubscriptionApproved += this.XC_SubscriptionApproved;
                this.xc.Im.SubscriptionRefused += this.XC_SubscriptionRefused;
                this.xc.Im.SubscriptionRequest = this.OnSubscriptionRequest;
                this.xc.Tune += this.XC_Tune;
                this.xc.Error += this.XC_Error;
                this.xc.ErrorMessage += this.XC_ErrorMessage;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ErrorX: " + ex.Message);
            }
        }

        /// <summary>
        /// To dispose everything needed to be disposed
        /// </summary>
        /// <param name="recreate">If true it will recreate things have been disposed</param>
        private void Dispose(bool recreate = false)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                OF_Login.IsEnabled = true;

                if (pingTimer != null)
                {
                    pingTimer.Dispose();
                }

                pingTimer = null;

                try
                {
                    this.xc.Close();
                }
                catch (Exception)
                {
                }

                BuddiesList.Clear();
                if (recreate == true)
                {
                    CreateXMPP();
                }
            }));

            this.SetLoginSCV(Visibility.Visible);
        }

        /// <summary>
        /// Closing app
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">Routed event args</param>
        private void ExitApp(object sender, RoutedEventArgs e)
        {
            this.CloseAll();
        }

        /// <summary>
        /// Click event for icon
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">Event args</param>
        private void Icon_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// To animate textblocks left to right
        /// </summary>
        /// <param name="tbc">Canvas object</param>
        /// <param name="tb">TextBlock object</param>
        private void LeftToRightMarquee(Canvas tbc, TextBlock tb)
        {
            double height = tbc.ActualHeight - tb.ActualHeight;
            tb.Margin = new Thickness(0, height / 2, 0, 0);
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = -tb.ActualWidth;
            doubleAnimation.To = tbc.ActualWidth;
            doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
            doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(10));
            tb.BeginAnimation(Canvas.LeftProperty, doubleAnimation);
        }

        /// <summary>
        /// KeyDown event for MsgBox
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">KeyEvent args</param>
        private void MsgBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TextBox textBox = (TextBox)sender;
                this.SendChat(((Sharp.Xmpp.Jid)textBox.Tag).GetBareJid());
            }
        }

        /// <summary>
        /// KeyDown event for MsgMucBox
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">KeyEvent args</param>
        private void MsgMucBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TextBox textBox = (TextBox)sender;
                this.SendMucChat(((Sharp.Xmpp.Jid)textBox.Tag).GetBareJid());
            }
        }

        /// <summary>
        /// Click event for OF_AutoLogin
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">RoutedEvent args</param>
        private void OF_AutoLogin_Click(object sender, RoutedEventArgs e)
        {
            Settings.SaveSetting();
        }

        /// <summary>
        /// MouseDoubleClick event for OF_FriendsList
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">MouseButtonEvent args</param>
        private void OF_FriendsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (OF_FriendsList.SelectedItem != null)
            {
                Xmpp.BuddiesData xmppBuddiesData = (Xmpp.BuddiesData)OF_FriendsList.SelectedItem;

                this.Dispatcher.Invoke((Action)(() =>
                {
                    if (!Application.Current.Windows.OfType<OF_ChatWindow>().Any(x => x.XmppJid == xmppBuddiesData.Jid))
                    {
                        OF_ChatWindow CW = new OF_ChatWindow(AllChats, xmppBuddiesData.Jid);
                        CW.Tag = xmppBuddiesData.Jid;

                        CW.Show();
                        CW.OF_SendButton.Click += SendButton_Click;
                        CW.OF_MsgBox.KeyDown += MsgBox_KeyDown;
                        try
                        {
                            CW.OF_ChatBox.ItemsSource = AllChats.Single(j => j.Jid.GetBareJid() == xmppBuddiesData.Jid.GetBareJid()).Msgs;
                        }
                        catch (Exception)
                        {
                        }

                        CW.OF_ChatBox.Items.Refresh();
                    }
                }));
            }
        }

        /// <summary>
        /// SelectionChanged event for OF_FriendsList
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">SelectionChangedEvent args</param>
        private void OF_FriendsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        /// <summary>
        /// MouseDoubleClick event for OF_GroupsList
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">MouseButtonEvent args</param>
        private void OF_GroupsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
        }

        /// <summary>
        /// Click event for OF_Login
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">RoutedEvent args</param>
        private void OF_Login_Click(object sender, RoutedEventArgs e)
        {
            OF_Login.IsEnabled = false;
            this.threadxmppconnect.SmartStart(this.Connect);

            ////thread_xmpp_load.Start();
        }

        /// <summary>
        /// Click event for OF_Menu_Add_Friend
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">RoutedEvent args</param>
        private void OF_Menu_Add_Friend_Click(object sender, RoutedEventArgs e)
        {
            OF_AddFriendWindow addFriendWindow = new OF_AddFriendWindow();
            addFriendWindow.ShowDialog();
            string[] groups = new string[] { "Buddies" };
            if (addFriendWindow.DialogResult.HasValue && addFriendWindow.DialogResult.Value)
            {
                this.xc.AddContact(new Jid(addFriendWindow.OF_Name_TextBox.Text + "@" + this.xc.Im.Jid.Domain), null, groups);
            }
            else
            {
            }
        }

        /// <summary>
        /// Clicl event for OF_Menu_File_LogOut
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">RoutedEvent args</param>
        private void OF_Menu_File_LogOut_Click(object sender, RoutedEventArgs e)
        {
            this.Dispose(true);
            //// SetXMPPSTATUS("Disconnected from server");
            this.threadxmppconnect.Abort();
        }

        /// <summary>
        /// Click event for OF_Menu_Help_About
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">RoutedEvent args</param>
        private void OF_Menu_Help_About_Click(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.ApplicationLogo = (BitmapImage)Application.Current.FindResource("OpenFlareIcon");
            about.Show();
        }

        /// <summary>
        /// Click event for OF_Menu_Options_Settings
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">RoutedEvent args</param>
        private void OF_Menu_Options_Settings_Click(object sender, RoutedEventArgs e)
        {
            OF_SettingsWindow set = new OF_SettingsWindow();
            set.ShowDialog();
        }

        /// <summary>
        /// Click event for OF_MenuList_Accept_Friend
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">RoutedEvent args</param>
        private void OF_MenuList_Accept_Friend_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            Jid jid = (Jid)menuItem.Tag;
            this.xc.Im.ApproveSubscriptionRequest(jid);
        }

        /// <summary>
        /// Click event for OF_MenuList_Remove_Friend
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">RoutedEvent args</param>
        private void OF_MenuList_Remove_Friend_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            Jid jid = (Jid)menuItem.Tag;
            this.xc.RemoveContact(jid);
        }

        /// <summary>
        /// PasswordChanged event for OF_Password
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">RoutedEvent args</param>
        private void OF_Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Settings.Password = OF_Password.SecurePassword;
            Settings.SaveSetting();
        }

        /// <summary>
        /// SelectionChanged event for OF_Status
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">SelectionChangedEvent args</param>
        private void OF_Status_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Settings.SaveSetting();
            this.SetStatus();
        }

        /// <summary>
        /// LostFocus event for OF_StatusText
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">RoutedEvent args</param>
        private void OF_StatusText_LostFocus(object sender, RoutedEventArgs e)
        {
            Settings.SaveSetting();
            this.SetStatus();
        }

        /// <summary>
        /// TextChanged event for OF_UserName
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">TextChangedEvent args</param>
        private void OF_UserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            Settings.SaveSetting();
        }

        /// <summary>
        /// Xmpp OnSubscriptionRequest
        /// </summary>
        /// <param name="from">From Jid</param>
        /// <returns>True or false</returns>
        private bool OnSubscriptionRequest(Jid from)
        {
            bool temp = false;

            this.Dispatcher.Invoke((Action)(() =>
            {
                OF_SR_Window OFSRW = new OF_SR_Window(from + " wants to subscribe to your presence.");
                OFSRW.ShowDialog();
                if (OFSRW.DialogResult.HasValue && OFSRW.DialogResult.Value)
                {
                    xc.Im.ApproveSubscriptionRequest(from);
                    temp = true;
                }
                else
                {
                    xc.Im.RefuseSubscriptionRequest(from);
                    temp = false;
                }
            }));

            return temp;
        }

        /// <summary>
        /// For opening settings window
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">RoutedEvent args</param>
        private void OpenSettings(object sender, RoutedEventArgs e)
        {
            OF_SettingsWindow set = new OF_SettingsWindow();
            set.ShowDialog();
        }

        /// <summary>
        /// To animate textblock right to left
        /// </summary>
        /// <param name="tbc">Canvas object</param>
        /// <param name="tb">TextBlock object</param>
        private void RightToLeftMarquee(Canvas tbc, TextBlock tb)
        {
            double height = tbc.ActualHeight - tb.ActualHeight;
            tb.Margin = new Thickness(0, height / 2, 0, 0);
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = -tb.ActualWidth;
            doubleAnimation.To = tbc.ActualWidth;
            doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
            doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(10));
            tb.BeginAnimation(Canvas.RightProperty, doubleAnimation);
        }

        /// <summary>
        /// Click event for SendButton
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">RoutedEvent args</param>
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            this.SendChat(((Sharp.Xmpp.Jid)button.Tag).GetBareJid());
        }

        /// <summary>
        /// Click event for SendMucButton
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">RoutedEvent args</param>
        private void SendMucButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            this.SendMucChat(((Sharp.Xmpp.Jid)button.Tag).GetBareJid());
        }

        /// <summary>
        /// UpdateStatus for xmpp client
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">StatusEvent args</param>
        private void UpdateStatus(object sender, Sharp.Xmpp.Im.StatusEventArgs e)
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                try
                {
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        if (e.Jid.GetBareJid() != XmppClientData.Jid.GetBareJid())
                        {
                            this.BuddiesList.Single(j => j.Jid.GetBareJid() == e.Jid.GetBareJid()).Status = e.Status.Availability.ToString();
                            string st = "...";

                            if (!e.Status.Message.IsNullOrEmpty())
                            {
                                st = e.Status.Message;
                            }

                            this.BuddiesList.Single(j => j.Jid.GetBareJid() == e.Jid.GetBareJid()).StatusMessage = st;
                        }
                        else
                        {
                            XmppClientData.Status = e.Status.Availability.ToString();
                            XmppClientData.StatusMessage = e.Status.Message;
                        }
                    }));
                }
                catch (Exception)
                {
                    if (statuserrs < 100)
                    {
                        this.UpdateStatus(sender, e);
                    }
                    else
                    {
                        throw new System.Exception("Too many errors");
                        ////Status_Errs = 0;
                    }

                    statuserrs++;
                }
            }).Start();
        }

        /// <summary>
        /// Closing even for main window
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">CancelEvent args</param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.XClose();
        }

        /// <summary>
        /// Loaded event for main window
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">RoutedEvent args</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            HwndSource src = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
            src.AddHook(new HwndSourceHook(this.WindowProc));
            this.LeftToRightMarquee(this.OF_StatusCanvas, this.OF_StatusText);
            this.LeftToRightMarquee(this.OF_TuneCanvas, this.OF_TuneText);
        }

        /// <summary>
        /// ActivityChanged for xmpp client
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">ActivityChangedEvent args</param>
        private void XC_ActivityChanged(object sender, Sharp.Xmpp.Extensions.ActivityChangedEventArgs e)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                MessageBox.Show("ActivityChanged: " + e.Jid.ToString());
            }));
        }

        /// <summary>
        /// Error event for xmpp client
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">ErrorEvent args</param>
        private void XC_Error(object sender, Sharp.Xmpp.Im.ErrorEventArgs e)
        {
            if (e.Exception.Message.Contains("XmppDisconnectionException"))
            {
                this.Dispose(true);
                this.SetXMPPSTATUS("Disconnected from server");
                this.threadxmppconnect.Abort();
            }

            MessageBox.Show("Error: " + e.Exception.Message);
        }

        /// <summary>
        /// ErrorMessage event for xmpp client
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">MessageEvent args</param>
        private void XC_ErrorMessage(object sender, MessageEventArgs e)
        {
            MessageBox.Show("ErrorMessage: " + e.Message);
        }

        /// <summary>
        /// Message event for xmpp client
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">MessageEvent args</param>
        private void XC_Message(object sender, Sharp.Xmpp.Im.MessageEventArgs e)
        {
            if (e.Jid.ToString().Contains("@conference." + this.xc.Im.Jid.Domain) || e.Jid.ToString().Contains("@muc." + this.xc.Im.Jid.Domain))
            {
                Xmpp.ChatMsg xmppChatMsg = new Xmpp.ChatMsg();

                xmppChatMsg.Text = " " + e.Message.Body;
                xmppChatMsg.Avatar = this.XmppClientData.Avatar;
                xmppChatMsg.AvatarColumn = 0;
                xmppChatMsg.TextColumn = 1;
                xmppChatMsg.TextHA = HorizontalAlignment.Left;
                this.AddChat(e.Jid, xmppChatMsg);

                this.Dispatcher.InvokeAsync((Action)(() =>
                {
                    OF_GroupChatWindow groupChatWindow;
                    if (!Application.Current.Windows.OfType<OF_GroupChatWindow>().Any(x => x.Jid.GetBareJid() == e.Jid.GetBareJid()))
                    {
                        groupChatWindow = new OF_GroupChatWindow(AllChats, e.Jid.GetBareJid());
                        groupChatWindow.Tag = e.Jid.GetBareJid();
                        groupChatWindow.OF_Send.Click += SendMucButton_Click;
                        groupChatWindow.OF_Msg.KeyDown += MsgMucBox_KeyDown;
                        groupChatWindow.Show();
                    }
                    else
                    {
                        groupChatWindow = Application.Current.Windows.OfType<OF_GroupChatWindow>().Single(x => ((Sharp.Xmpp.Jid)x.Tag).GetBareJid() == e.Jid.GetBareJid());
                    }

                    groupChatWindow.OF_Chat.ItemsSource = AllChats.Single(j => j.Jid.ToString().Contains(e.Jid.GetBareJid().ToString())).Msgs;
                    groupChatWindow.OF_Chat.Items.Refresh();
                }));
            }
            else
            {
                Xmpp.ChatMsg xmppChatMsg = new Xmpp.ChatMsg();

                xmppChatMsg.Text = " " + e.Message.Body;
                xmppChatMsg.Avatar = this.BuddiesList.Single(j => j.Jid.GetBareJid() == e.Jid.GetBareJid()).Avatar;
                xmppChatMsg.AvatarColumn = 0;
                xmppChatMsg.TextColumn = 1;
                xmppChatMsg.TextHA = HorizontalAlignment.Left;
                this.AddChat(e.Jid.GetBareJid(), xmppChatMsg);

                this.Dispatcher.InvokeAsync((Action)(() =>
                {
                    if (!Application.Current.Windows.OfType<OF_ChatWindow>().Any(x => x.XmppJid.GetBareJid() == e.Jid.GetBareJid()))
                    {
                        OF_ChatWindow CW = new OF_ChatWindow(AllChats, e.Jid.GetBareJid());
                        CW.Tag = e.Jid.GetBareJid();
                        CW.Show();
                        CW.OF_SendButton.Click += SendButton_Click;

                        CW.OF_MsgBox.KeyDown += MsgBox_KeyDown;
                        CW.OF_ChatBox.ItemsSource = AllChats.Single(j => j.Jid.GetBareJid() == e.Jid.GetBareJid()).Msgs;
                        CW.OF_ChatBox.Items.Refresh();
                    }
                    else
                    {
                        OF_ChatWindow chatWindow = Application.Current.Windows.OfType<OF_ChatWindow>().Single(x => ((Sharp.Xmpp.Jid)x.Tag).GetBareJid() == e.Jid.GetBareJid());
                        chatWindow.OF_ChatBox.ItemsSource = AllChats.Single(j => j.Jid.GetBareJid() == e.Jid.GetBareJid()).Msgs;
                        chatWindow.OF_ChatBox.Items.Refresh();
                    }
                }));
            }
        }

        /// <summary>
        /// MoodChanged event for xmpp client
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">MoodChangedEvent args</param>
        private void XC_MoodChanged(object sender, Sharp.Xmpp.Extensions.MoodChangedEventArgs e)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                MessageBox.Show("MoodChanged: " + e.Jid.ToString());
            }));
        }

        /// <summary>
        /// RosterUpdated event for xmpp client
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">RosterUpdatedEvent args</param>
        private void XC_RosterUpdated(object sender, Sharp.Xmpp.Im.RosterUpdatedEventArgs e)
        {
            if (e.Removed == true)
            {
                this.BuddiesList.Remove(this.BuddiesList.Single(j => j.Jid.GetBareJid() == e.Item.Jid.GetBareJid()));
            }
            else
            {
                Xmpp.BuddiesData xmppBuddiesData = new Xmpp.BuddiesData();
                string xname = "Loading...";

                if (!e.Item.Name.IsNullOrEmpty())
                {
                    xname = e.Item.Name;
                }

                xmppBuddiesData.Name = xname;
                xmppBuddiesData.Jid = e.Item.Jid;

                xmppBuddiesData.Pending = e.Item.Pending;
                xmppBuddiesData.SubscriptionState = e.Item.SubscriptionState.ToString();

                if (!this.BuddiesContains(e.Item.Jid))
                {
                    this.BuddiesList.Add(xmppBuddiesData);
                }

                this.xc.GetvCard(e.Item.Jid.ToString(), this.Callback);
            }
        }

        /// <summary>
        /// Status event for xmpp client
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">StatusEvent args</param>
        private void XC_Status(object sender, StatusEventArgs e)
        {
            this.UpdateStatus(sender, e);
        }

        /// <summary>
        /// StatusChanged event for xmpp client
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">StatusEvent args</param>
        private void XC_StatusChanged(object sender, Sharp.Xmpp.Im.StatusEventArgs e)
        {
            this.UpdateStatus(sender, e);
        }

        /// <summary>
        /// SubscriptionApproved event for xmpp client
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">SubscriptionApprovedEvent args</param>
        private void XC_SubscriptionApproved(object sender, SubscriptionApprovedEventArgs e)
        {
            ////MessageBox.Show("Approved");
        }

        /// <summary>
        /// SubscriptionRefused event for xmpp client
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">SubscriptionRefusedEvent args</param>
        private void XC_SubscriptionRefused(object sender, SubscriptionRefusedEventArgs e)
        {
            ////MessageBox.Show("Refused");
        }

        /// <summary>
        /// Tune event for xmpp client
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">TuneEvent args</param>
        private void XC_Tune(object sender, TuneEventArgs e)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                if (e.Jid.GetBareJid() != this.XmppClientData.Jid.GetBareJid())
                {
                    this.BuddiesList.Single(j => j.Jid.GetBareJid() == e.Jid.GetBareJid()).MyTune = e.Information;
                    this.BuddiesList.Single(j => j.Jid.GetBareJid() == e.Jid.GetBareJid()).TuneText = e.Information.Artist + " - " + e.Information.Title;
                }
            }));
            ////MessageBox.Show("Artist: " + e.Information.Artist + Environment.NewLine + "Album: " + e.Information.Source + Environment.NewLine + "Title: " + e.Information.Title, "File: ");
        }

        /// <summary>
        /// For clearing UserTune
        /// </summary>
        private void XClose()
        {
            TuneInformation tif = new TuneInformation(null, null, null, 0, 0, null, null);
            this.XmppClientData.MyTune = tif;
            this.XmppClientData.TuneText = "Not Playing any music!";

            if (this.xc.Connected)
            {
                this.xc.SetTune(tif);
            }
        }

        /// <summary>
        /// For loading xmpp data
        /// </summary>
        private void XmppLoad()
        {
            try
            {
                this.XmppClientData.Jid = this.xc.Jid;
                this.xc.GetvCard(this.XmppClientData.Jid.GetBareJid().ToString(), this.Callback);

                foreach (Sharp.Xmpp.Im.RosterItem item in this.xc.GetRoster())
                {
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        Xmpp.BuddiesData xmppBuddiesData = new Xmpp.BuddiesData();
                        string xname = "Loading...";

                        if (!item.Name.IsNullOrEmpty())
                        {
                            xname = item.Name;
                        }

                        xmppBuddiesData.Name = xname;
                        xmppBuddiesData.Jid = item.Jid;
                        xmppBuddiesData.Pending = item.Pending;
                        xmppBuddiesData.SubscriptionState = item.SubscriptionState.ToString();

                        if (!this.BuddiesList.Contains(xmppBuddiesData))
                        {
                            this.BuddiesList.Add(xmppBuddiesData);
                        }
                    }));
                }

                foreach (Sharp.Xmpp.Im.RosterItem item in this.xc.GetRoster())
                {
                    ////xc.GetvCardAvatar(item.Jid.ToString(), Environment.CurrentDirectory + @"\avatars\" + item.Jid.GetBareJid().ToString().Split('@')[0] + ".png", Callback);
                    ////xc.GetvCardAvatar(item.Jid.ToString(), Callback);

                    this.xc.GetvCard(item.Jid.ToString(), this.Callback);
                }

                this.xc.SetStatus(this.GetFromInt(Settings.Status), Settings.StatusMessage, 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}