// <copyright file="Buddies.cs" company="POQDavid">
// Copyright (c) POQDavid. All rights reserved.
// </copyright>
// <author>POQDavid</author>
// <summary>This is the Buddies class.</summary>
namespace OpenFlareClient.Xmpp
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Windows.Threading;

    /// <summary>
    /// Buddies class
    /// </summary>
    /// <typeparam name="BuddiesData">Buddies data</typeparam>
    public class Buddies<BuddiesData> : ObservableCollection<BuddiesData> where BuddiesData : INotifyPropertyChanged
    {
        /// <summary>
        /// Dispatcher dispatcherUIThread
        /// </summary>
        private Dispatcher dispatcherUIThread;

        /// <summary>
        /// Initializes a new instance of the Buddies class
        /// </summary>
        /// <param name="dispatcher">Dispatcher dispatcher</param>
        public Buddies(Dispatcher dispatcher)
        {
            this.dispatcherUIThread = dispatcher;
        }

        /// <summary>
        /// Initializes a new instance of the Buddies class
        /// </summary>
        public Buddies()
        {
            this.dispatcherUIThread = Dispatcher.CurrentDispatcher;
            this.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(this.Buddies_CollectionChanged);
        }

        /// <summary>
        /// Initializes a new instance of the Buddies class
        /// </summary>
        /// <param name="collection">IEnumerable BuddiesData collection</param>
        public Buddies(IEnumerable<BuddiesData> collection) : this()
        {
            // AddRange(collection);
        }

        /// <summary>
        /// Callback for adding item
        /// </summary>
        /// <param name="item">Item to add</param>
        private delegate void AddItemCallback(BuddiesData item);

        /// <summary>
        /// Callback for clearing items
        /// </summary>
        private delegate void ClearItemsCallback();

        /// <summary>
        /// Callback for inserting item at selected index
        /// </summary>
        /// <param name="index">Item index</param>
        /// <param name="item">Item to add</param>
        private delegate void InsertItemCallback(int index, BuddiesData item);

        /// <summary>
        /// Callback to move item to selected index
        /// </summary>
        /// <param name="oldIndex">Selected index</param>
        /// <param name="newIndex">New index</param>
        private delegate void MoveItemCallback(int oldIndex, int newIndex);

        /// <summary>
        /// Callback to remove item
        /// </summary>
        /// <param name="index">Item index</param>
        private delegate void RemoveItemCallback(int index);

        /// <summary>
        /// Callback to set item
        /// </summary>
        /// <param name="index">Item index</param>
        /// <param name="item">Item to set</param>
        private delegate void SetItemCallback(int index, BuddiesData item);

        /// <summary>
        /// Gets Dispatcher
        /// </summary>
        public Dispatcher Dispatcher
        {
            get
            {
                return this.dispatcherUIThread;
            }
        }

        /// <summary>
        /// EntityViewModelPropertyChanged for Buddies
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event args</param>
        public void EntityViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ////This will get called when the property of an object inside the collection changes - note you must make it a 'reset' - dunno why
            NotifyCollectionChangedEventArgs args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
            this.OnCollectionChanged(args);
        }

        /// <summary>
        /// Clears items
        /// </summary>
        protected override void ClearItems()
        {
            if (this.dispatcherUIThread.CheckAccess())
            {
                base.ClearItems();
            }
            else
            {
                this.dispatcherUIThread.Invoke(DispatcherPriority.Send, new ClearItemsCallback(this.ClearItems));
            }
        }

        /// <summary>
        /// Inserts item
        /// </summary>
        /// <param name="index">Item index</param>
        /// <param name="item">Item to insert</param>
        protected override void InsertItem(int index, BuddiesData item)
        {
            if (this.dispatcherUIThread == null)
            {
                base.InsertItem(index, item);
            }
            else if (this.dispatcherUIThread.CheckAccess())
            {
                base.InsertItem(index, item);
            }
            else
            {
                this.dispatcherUIThread.Invoke(DispatcherPriority.Send, new InsertItemCallback(this.InsertItem), index, new object[] { item });
            }
        }

        /// <summary>
        /// Move item
        /// </summary>
        /// <param name="oldIndex">Selected index</param>
        /// <param name="newIndex">New index</param>
        protected override void MoveItem(int oldIndex, int newIndex)
        {
            if (this.dispatcherUIThread.CheckAccess())
            {
                base.MoveItem(oldIndex, newIndex);
            }
            else
            {
                this.dispatcherUIThread.Invoke(DispatcherPriority.Send, new MoveItemCallback(this.MoveItem), oldIndex, new object[] { newIndex });
            }
        }

        /// <summary>
        /// Removes item
        /// </summary>
        /// <param name="index">Item index</param>
        protected override void RemoveItem(int index)
        {
            if (this.dispatcherUIThread.CheckAccess())
            {
                base.RemoveItem(index);
            }
            else
            {
                this.dispatcherUIThread.Invoke(DispatcherPriority.Send, new RemoveItemCallback(this.RemoveItem), index);
            }
        }

        /// <summary>
        /// Set item
        /// </summary>
        /// <param name="index">Item index</param>
        /// <param name="item">Item to set</param>
        protected override void SetItem(int index, BuddiesData item)
        {
            if (this.dispatcherUIThread.CheckAccess())
            {
                base.SetItem(index, item);
            }
            else
            {
                this.dispatcherUIThread.Invoke(DispatcherPriority.Send, new SetItemCallback(this.SetItem), index, new object[] { item });
            }
        }

        /// <summary>
        /// CollectionChanged event
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event args</param>
        private void Buddies_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (BuddiesData item in e.OldItems)
                {
                    ////Removed items
                    item.PropertyChanged -= this.EntityViewModelPropertyChanged;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (BuddiesData item in e.NewItems)
                {
                    ////Added items
                    item.PropertyChanged += this.EntityViewModelPropertyChanged;
                }
            }
        }
    }
}