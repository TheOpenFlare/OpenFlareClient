// <copyright file="OF_SettingsWindow.xaml.cs" company="POQDavid">
//     Copyright (c) POQDavid. All rights reserved.
// </copyright>
// <author>POQDavid</author>
// <summary>
// This is the OF_SettingsWindow class.
// </summary>
namespace OpenFlareClient
{
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class OF_SettingsWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the OF_SettingsWindow class
        /// </summary>
        public OF_SettingsWindow()
        {
            this.InitializeComponent();
            OF_Pass_Box.Password = OpenFlareClient.OF_MainWindow.Settings.Password.UnsecureString();
        }

        /// <summary>
        /// Gets or sets the Settings property.
        /// </summary>
        public Settings Settings
        {
            get
            {
                return OpenFlareClient.OF_MainWindow.Settings;
            }

            set
            {
                OpenFlareClient.OF_MainWindow.Settings = value;
            }
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
        /// Click event for button_Apply
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">The RoutedEventArgs</param>
        private void OF_Button_Apply_Click(object sender, RoutedEventArgs e)
        {
            OpenFlareClient.OF_MainWindow.Settings.SaveSetting();
        }

        /// <summary>
        /// Click event for button_Cancel
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">The RoutedEventArgs</param>
        private void OF_Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            OpenFlareClient.OF_MainWindow.Settings.LoadSettings();
            this.DialogResult = false;
        }

        /// <summary>
        /// Click event for button_Save
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">The RoutedEventArgs</param>
        private void OF_Button_Save_Click(object sender, RoutedEventArgs e)
        {
            OpenFlareClient.OF_MainWindow.Settings.SaveSetting();
            this.DialogResult = true;
        }

        /// <summary>
        /// PasswordChanged event for OF_Pass_Box
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">The RoutedEventArgs</param>
        private void OF_Pass_Box_PasswordChanged(object sender, RoutedEventArgs e)
        {
            OpenFlareClient.OF_MainWindow.Settings.Password = OF_Pass_Box.SecurePassword;
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