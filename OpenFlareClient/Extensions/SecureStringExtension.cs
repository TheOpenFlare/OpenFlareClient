// <copyright file="SecureStringExtension.cs" company="POQDavid">
//     Copyright (c) POQDavid. All rights reserved.
// </copyright>
// <author>POQDavid</author>
// <summary>
// This is the SecureStringExtension class.
// </summary>
namespace OpenFlareClient
{
    using System.Security;

    /// <summary>
    /// The SecureStringExtension
    /// </summary>
    public static class SecureStringExtension
    {
        /// <summary>
        /// Appends SecureString
        /// </summary>
        /// <param name="str">The main string</param>
        /// <param name="str2">String to append</param>
        /// <returns>Returns SecureString</returns>
        public static SecureString AppendSecureString(this SecureString str, SecureString str2)
        {
            var secureStr = new SecureString();
            if (str.Length > 0)
            {
                foreach (var c in str2.UnsecureString().ToCharArray())
                {
                    secureStr.AppendChar(c);
                }
            }

            return secureStr;
        }

        /// <summary>
        /// Checks if SecureString is Null or Empty
        /// </summary>
        /// <param name="value">SecureString to check</param>
        /// <returns>True or false</returns>
        public static bool IsNullOrEmpty(this SecureString value)
        {
            return null == value || 0 == value.Length;
        }

        /// <summary>
        /// Checks if SecureString is Null or WhiteSpace
        /// </summary>
        /// <param name="value">SecureString to check</param>
        /// <returns>True or false</returns>
        public static bool IsNullOrWhiteSpace(this SecureString value)
        {
            if (value != null)
            {
                for (int i = 0; i < value.Length; i++)
                {
                    if (!char.IsWhiteSpace(value.UnsecureString()[i]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Converts string to SecureString
        /// </summary>
        /// <param name="str">The string to be converted</param>
        /// <returns>Returns SecureString</returns>
        public static SecureString SecureString(this string str)
        {
            var secureStr = new SecureString();

            if (str.Length > 0)
            {
                foreach (var c in str.ToCharArray())
                {
                    secureStr.AppendChar(c);
                }
            }

            str = null;
            return secureStr;
        }
    }
}