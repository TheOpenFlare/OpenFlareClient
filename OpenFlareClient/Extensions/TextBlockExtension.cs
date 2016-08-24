// <copyright file="TextBlockExtension.cs" company="POQDavid">
//     Copyright (c) POQDavid. All rights reserved.
// </copyright>
// <author>POQDavid</author>
// <summary>
// This is the TextBlockExtension class.
// </summary>
namespace OpenFlareClient
{
    using System;
    using System.Windows.Controls;

    /// <summary>
    /// The TextBlockExtension
    /// </summary>
    public static class TextBlockExtension
    {
        /// <summary>
        /// Set TextBlock text
        /// </summary>
        /// <param name="textBlock">The textblock</param>
        /// <param name="text">Text string</param>
        /// <param name="waitUntilReturn">Wait until return</param>
        public static void SetText(this TextBlock textBlock, string text, bool waitUntilReturn = false)
        {
            Action append = () => textBlock.Text = text;
            if (textBlock.CheckAccess())
            {
                append();
            }
            else if (waitUntilReturn)
            {
                textBlock.Dispatcher.Invoke(append);
            }
            else
            {
                textBlock.Dispatcher.BeginInvoke(append);
            }
        }
    }
}