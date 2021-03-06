﻿// <copyright file="OF_AddFriendWindow.xaml.cs" company="POQDavid">
//     Copyright (c) POQDavid. All rights reserved.
// </copyright>
// <author>POQDavid</author>
// <summary>
// This is the OF_AddFriendWindow class.
// </summary>
namespace OpenFlareClient
{
    using System.Windows;
    using System.Windows.Input;

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
        /// CanExecute for Command binding.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments for the CanExecute events.</param>
        private void CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        /// <summary>
        /// CanMinimizeWindow for Command binding.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments for the CanExecute events.</param>
        private void CanMinimizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.ResizeMode != ResizeMode.NoResize;
        }

        /// <summary>
        /// CanResizeWindow for Command binding.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments for the CanExecute events.</param>
        private void CanResizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.ResizeMode == ResizeMode.CanResize || this.ResizeMode == ResizeMode.CanResizeWithGrip;
        }

        /// <summary>
        /// CloseWindow for Command binding.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments for the Executed events.</param>
        private void CloseWindow(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }

        /// <summary>
        /// MaximizeWindow for Command binding.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments for the Executed events.</param>
        private void MaximizeWindow(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MaximizeWindow(this);
        }

        /// <summary>
        /// MinimizeWindow for Command binding.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments for the Executed events.</param>
        private void MinimizeWindow(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
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

        /// <summary>
        /// RestoreWindow for Command binding.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments for the Executed events.</param>
        private void RestoreWindow(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(this);
        }
    }
}