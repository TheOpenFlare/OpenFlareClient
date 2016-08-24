// <copyright file="ListBoxExtension.cs" company="POQDavid">
//     Copyright (c) POQDavid. All rights reserved.
// </copyright>
// <author>POQDavid</author>
// <summary>
// This is the ListBoxExtension class.
// </summary>
namespace OpenFlareClient
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Controls;

    /// <summary>
    /// The ListBoxExtension
    /// </summary>
    public static class ListBoxExtension
    {
        /// <summary>
        /// Add item to list box
        /// </summary>
        /// <param name="listBox">The listbox</param>
        /// <param name="text">Text string</param>
        /// <param name="waitUntilReturn">Wait until return</param>
        public static void AddItem(this ListBox listBox, string text, bool waitUntilReturn = false)
        {
            Action additem = () =>
            {
                var listBoxItem = new ListBoxItem();
                listBoxItem.Content = text;
                ////lbit.Tag = index;
                listBox.Items.Add(listBoxItem);
            };
            if (listBox.CheckAccess())
            {
                additem();
            }
            else if (waitUntilReturn)
            {
                listBox.Dispatcher.Invoke(additem);
            }
            else
            {
                listBox.Dispatcher.BeginInvoke(additem);
            }
        }

        /// <summary>
        /// Add item to list box
        /// </summary>
        /// <param name="listBox">The listbox</param>
        /// <param name="text">Text string</param>
        /// <param name="tag">Tag object</param>
        /// <param name="waitUntilReturn">Wait until return</param>
        public static void AddItem(this ListBox listBox, string text, object tag, bool waitUntilReturn = false)
        {
            Action additem = () =>
            {
                var listBoxItem = new ListBoxItem();
                listBoxItem.Content = text;
                listBoxItem.Tag = tag;

                ////lbit.Tag = index;
                listBox.Items.Add(listBoxItem);
            };
            if (listBox.CheckAccess())
            {
                additem();
            }
            else if (waitUntilReturn)
            {
                listBox.Dispatcher.Invoke(additem);
            }
            else
            {
                listBox.Dispatcher.BeginInvoke(additem);
            }
        }

        /// <summary>
        /// Remove item
        /// </summary>
        /// <param name="listBox">The listbox</param>
        /// <param name="text">Text string</param>
        /// <param name="index">Int index</param>
        /// <param name="waitUntilReturn">Wait until return</param>
        public static void RemoveItem(this ListBox listBox, string text, int index, bool waitUntilReturn = false)
        {
            ////ListBoxItem lbit;
            ////lbit.Content = text;
            ////lbit.Tag = index;
            Action additem = () => listBox.Items.Remove(string.Empty);
            if (listBox.CheckAccess())
            {
                additem();
            }
            else if (waitUntilReturn)
            {
                listBox.Dispatcher.Invoke(additem);
            }
            else
            {
                listBox.Dispatcher.BeginInvoke(additem);
            }
        }

        /// <summary>
        /// Set item source
        /// </summary>
        /// <param name="listBox">The listbox</param>
        /// <param name="temp">Temp list</param>
        /// <param name="waitUntilReturn">Wait until return</param>
        public static void SetItemSource(this ListBox listBox, List<string> temp, bool waitUntilReturn = false)
        {
            Action additem = () =>
            {
                ////lbit.Tag = index;
                listBox.ItemsSource = temp;
            };
            if (listBox.CheckAccess())
            {
                additem();
            }
            else if (waitUntilReturn)
            {
                listBox.Dispatcher.Invoke(additem);
            }
            else
            {
                listBox.Dispatcher.BeginInvoke(additem);
            }
        }
    }
}