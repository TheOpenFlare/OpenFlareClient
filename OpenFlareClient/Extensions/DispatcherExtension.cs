// <copyright file="DispatcherExtension.cs" company="POQDavid">
//     Copyright (c) POQDavid. All rights reserved.
// </copyright>
// <author>POQDavid</author>
// <summary>
// This is the DispatcherExtension class.
// </summary>
namespace OpenFlareClient
{
    using System;
    using System.Windows.Threading;

    /// <summary>
    /// The DispatcherExtension
    /// </summary>
    public static class DispatcherExtension
    {
        /// <summary>
        /// Invoke or execute
        /// </summary>
        /// <param name="dispatcher">The dispatcher</param>
        /// <param name="action">Action object</param>
        public static void InvokeOrExecute(this System.Windows.Threading.Dispatcher dispatcher, Action action)
        {
            if (dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                dispatcher.BeginInvoke(DispatcherPriority.Normal, action);
            }
        }
    }
}