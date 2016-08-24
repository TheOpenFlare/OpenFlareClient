// <copyright file="Json.cs" company="POQDavid">
//     Copyright (c) POQDavid. All rights reserved.
// </copyright>
// <author>POQDavid</author>
// <summary>
// This is the Json class.
// </summary>
namespace OpenFlareClient
{
    using System;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Description of Json.
    /// </summary>
    public class Json
    {
        /// <summary>
        /// Initializes a new instance of the Json class
        /// </summary>
        public Json()
        {
        }

        /// <summary>
        /// To generate Json string from object
        /// </summary>
        /// <param name="jsonobj">Our Json object</param>
        /// <returns>Json string</returns>
        public static string GenerateString(object jsonobj)
        {
            var s = new JsonSerializerSettings();
            s.ObjectCreationHandling = ObjectCreationHandling.Replace; // without this, you end up with duplicates.

            return JsonConvert.SerializeObject(jsonobj, Formatting.None, s);
        }

        /// <summary>
        /// Gets property from json int.
        /// </summary>
        /// <param name="json_string">Json string</param>
        /// <param name="key">The key we want to read</param>
        /// <returns>Returns property as int</returns>
        internal static int GetInt(string json_string, string key)
        {
            int temp = 0;
            try
            {
                if (IsValid(json_string))
                {
                    JToken token = JObject.Parse(json_string);
                    token = token[key];

                    temp = int.Parse(token.ToString());
                }
                else
                {
                }
            }
            catch (Exception)
            {
            }

            return temp;
        }

        /// <summary>
        /// Gets property from json string.
        /// </summary>
        /// <param name="json_string">Json string</param>
        /// <param name="key">The key we want to read</param>
        /// <returns>Returns property as string</returns>
        internal static string GetStr(string json_string, string key)
        {
            string temp = string.Empty;
            try
            {
                if (IsValid(json_string))
                {
                    JToken token = JObject.Parse(json_string);
                    token = token[key];

                    temp = token.ToString();
                }
                else
                {
                }
            }
            catch (Exception)
            {
            }

            return temp;
        }

        /// <summary>
        /// Given the JSON string, validates if it's a correct JSON string.
        /// </summary>
        /// <param name="json_string">JSON string to validate.</param>
        /// <returns>true or false.</returns>
        internal static bool IsValid(string json_string)
        {
            try
            {
                JToken.Parse(json_string);
                return true;
            }
            catch (JsonReaderException)
            {
                return false;
            }
        }
    }
}