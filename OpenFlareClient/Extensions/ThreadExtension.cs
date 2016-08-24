// <copyright file="ThreadExtension.cs" company="POQDavid">
//     Copyright (c) POQDavid. All rights reserved.
// </copyright>
// <author>POQDavid</author>
// <summary>
// This is the ThreadExtension class.
// </summary>
namespace OpenFlareClient
{
    using System;
    using System.Threading;

    /// <summary>
    /// The ThreadExtension
    /// </summary>
    public static class ThreadExtension
    {
        /// <summary>
        /// Starts thread
        /// </summary>
        /// <param name="thread">Our thread</param>
        /// <param name="threadStart">The method that executes on our thread</param>
        public static void SmartStart(this Thread thread, ThreadStart threadStart)
        {
            if (!thread.IsAlive)
            {
                try
                {
                    thread.Start();
                }
                catch (Exception)
                {
                    thread = new Thread(threadStart);
                    thread.Start();
                }
            }
        }
    }
}