// <copyright file="OF_AddFriendWindow.xaml.cs" company="POQDavid">
//     Copyright (c) POQDavid. All rights reserved.
// </copyright>
// <author>POQDavid</author>
// <summary>
// This is the OF_AddFriendWindow class.
// </summary>
namespace OpenFlareClient
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for OF_AddFriendWindow.xaml
    /// </summary>
    public partial class OF_AddFriendWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the OF_AddFriendWindow class
        /// </summary>
        public OF_AddFriendWindow()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Click event for OF_Add_Button
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">The RoutedEventArgs</param>
        private void OF_Add_Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        /// <summary>
        /// Click event for OF_Cancel_Button
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">The RoutedEventArgs</param>
        private void OF_Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}