// <copyright file="OF_SR_Window.xaml.cs" company="POQDavid">
//     Copyright (c) POQDavid. All rights reserved.
// </copyright>
// <author>POQDavid</author>
// <summary>
// This is the OF_SR_Window class.
// </summary>
namespace OpenFlareClient
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for OF_SR_Window.xaml
    /// </summary>
    public partial class OF_SR_Window : Window
    {
        /// <summary>
        /// Initializes a new instance of the OF_SR_Window class
        /// </summary>
        /// <param name="msg">Request message</param>
        public OF_SR_Window(string msg)
        {
            this.InitializeComponent();
            OF_SR_Label.Content = msg;
        }

        /// <summary>
        /// Click event for OF_Approve_Button
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">The RoutedEventArgs</param>
        private void OF_Approve_Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        /// <summary>
        /// Click event for OF_Refuse_Button
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">The RoutedEventArgs</param>
        private void OF_Refuse_Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}