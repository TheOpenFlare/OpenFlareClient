// <copyright file="OF_SetStatusWindow.xaml.cs" company="POQDavid">
//     Copyright (c) POQDavid. All rights reserved.
// </copyright>
// <author>POQDavid</author>
// <summary>
// This is the OF_SetStatusWindow class.
// </summary>
namespace OpenFlareClient
{
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for OF_SetStatus_Window.xaml
    /// </summary>
    public partial class OF_SetStatusWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the OF_SetStatusWindow class
        /// </summary>
        public OF_SetStatusWindow()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the AllGroupChats property.
        /// </summary>
        public string Status { get; set; }

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
        /// Click event for OF_CancelStatus_Button
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">RoutedEvent args</param>
        private void OF_CancelStatus_Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        /// <summary>
        /// Click event for OF_SetStatus_Button
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">RoutedEvent args</param>
        private void OF_SetStatus_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Status = this.OF_Status_TextBox.Text;
            this.DialogResult = true;
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