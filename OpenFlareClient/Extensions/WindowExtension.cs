// <copyright file="WindowExtension.cs" company="POQDavid">
//     Copyright (c) POQDavid. All rights reserved.
// </copyright>
// <author>POQDavid</author>
// <summary>
// This is the WindowExtension class.
// </summary>
namespace OpenFlareClient
{
    using System;
    using System.Windows;

    /// <summary>
    /// The WindowExtension
    /// </summary>
    public static class WindowExtension
    {
        /// <summary>
        /// Set title of window
        /// </summary>
        /// <param name="win">The window</param>
        /// <param name="text">Text string</param>
        /// <param name="waitUntilReturn">Wait until return</param>
        public static void Set_Title(Window win, string text, bool waitUntilReturn = false)
        {
            Action settitle = () => win.Title = "OpenFlare Client" + text;
            if (win.CheckAccess())
            {
                settitle();
            }
            else if (waitUntilReturn)
            {
                win.Dispatcher.Invoke(settitle);
            }
            else
            {
                win.Dispatcher.BeginInvoke(settitle);
            }
        }

        /// <summary>
        /// Set title of window
        /// </summary>
        /// <param name="win">The window</param>
        /// <param name="text">Text string</param>
        /// <param name="waitUntilReturn">Wait until return</param>
        public static void SetTitle(this Window win, string text, bool waitUntilReturn = false)
        {
            Action settitle = () => win.Title = "OpenFlare Client" + text;
            if (win.CheckAccess())
            {
                settitle();
            }
            else if (waitUntilReturn)
            {
                win.Dispatcher.Invoke(settitle);
            }
            else
            {
                win.Dispatcher.BeginInvoke(settitle);
            }
        }
    }
}