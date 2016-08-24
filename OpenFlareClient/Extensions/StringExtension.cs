// <copyright file="StringExtension.cs" company="POQDavid">
//     Copyright (c) POQDavid. All rights reserved.
// </copyright>
// <author>POQDavid</author>
// <summary>
// This is the StringExtension class.
// </summary>
namespace OpenFlareClient
{
    using System;
    using System.Runtime.InteropServices;
    using System.Security;

    /// <summary>
    /// The StringExtension
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// Checks if string is Null or Empty
        /// </summary>
        /// <param name="value">String to checked</param>
        /// <returns>True or false</returns>
        public static bool IsNullOrEmpty(this string value)
        {
            return null == value || 0 == value.Length;
        }

        /// <summary>
        /// Checks if string is Null or WhiteSpace
        /// </summary>
        /// <param name="value">String to check</param>
        /// <returns>True or false</returns>
        public static bool IsNullOrWhiteSpace(this string value)
        {
            if (value != null)
            {
                for (int i = 0; i < value.Length; i++)
                {
                    if (!char.IsWhiteSpace(value[i]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Converts SecureString to string
        /// </summary>
        /// <param name="str">SecureString to convert</param>
        /// <returns>Converted SecureString</returns>
        public static string UnsecureString(this SecureString str)
        {
            if (str == null)
            {
                throw new ArgumentNullException("securePassword");
            }

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(str);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
    }
}