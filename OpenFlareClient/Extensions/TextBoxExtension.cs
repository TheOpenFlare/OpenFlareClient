// <copyright file="TextBoxExtension.cs" company="POQDavid">
//     Copyright (c) POQDavid. All rights reserved.
// </copyright>
// <author>POQDavid</author>
// <summary>
// This is the TextBoxExtension class.
// </summary>
namespace OpenFlareClient
{
    using System;
    using System.Windows.Controls.Primitives;

    /// <summary>
    /// The TextBoxExtension
    /// </summary>
    public static class TextBoxExtension
    {
        /// <summary>
        /// Check and append text
        /// </summary>
        /// <param name="textBox">The textbox</param>
        /// <param name="text">Text string</param>
        /// <param name="waitUntilReturn">Wait until return</param>
        public static void CheckAppendText(this TextBoxBase textBox, string text, bool waitUntilReturn = false)
        {
            Action append = () => textBox.AppendText(text);
            if (textBox.CheckAccess())
            {
                append();
            }
            else if (waitUntilReturn)
            {
                textBox.Dispatcher.Invoke(append);
            }
            else
            {
                textBox.Dispatcher.BeginInvoke(append);
            }
        }
    }
}