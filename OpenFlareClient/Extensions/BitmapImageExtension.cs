// <copyright file="BitmapImageExtension.cs" company="POQDavid">
//     Copyright (c) POQDavid. All rights reserved.
// </copyright>
// <author>POQDavid</author>
// <summary>
// This is the BitmapImageExtension class.
// </summary>
namespace OpenFlareClient
{
    using System;
    using System.IO;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// The BitmapImageExtension
    /// </summary>
    public static class BitmapImageExtension
    {
        /// <summary>
        /// Convert Base64 string to bitmap
        /// </summary>
        /// <param name="base64String">Base64 string</param>
        /// <returns>Returns BitmapImage</returns>
        public static BitmapImage Base64StringToBitmap(string base64String)
        {
            byte[] byteBuffer = Convert.FromBase64String(base64String);
            MemoryStream memoryStream = new MemoryStream(byteBuffer);
            memoryStream.Position = 0;

            BitmapImage bitmapImage = new BitmapImage();

            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.StreamSource = memoryStream;
            bitmapImage.EndInit();

            memoryStream.Close();
            memoryStream = null;
            byteBuffer = null;

            return bitmapImage;
        }

        /// <summary>
        /// Converts byte to Bitmap
        /// </summary>
        /// <param name="imagebyte">Bitmap's byte</param>
        /// <returns>Returns BitmapImage</returns>
        public static BitmapImage ByteToBitmap(byte[] imagebyte)
        {
            MemoryStream memoryStream = new MemoryStream(imagebyte);
            memoryStream.Position = 0;

            BitmapImage bitmapImage = new BitmapImage();

            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.StreamSource = memoryStream;
            bitmapImage.EndInit();

            memoryStream.Close();
            memoryStream = null;

            return bitmapImage;
        }
    }
}