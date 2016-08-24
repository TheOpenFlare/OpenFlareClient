using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace System.Security
{
    internal static partial class SecureStringExtension
    {
        public static SecureString SecureString(string strPassword)
        {

            var secureStr = new SecureString();
            if (strPassword.Length > 0)
            {
                foreach (var c in strPassword.ToCharArray()) secureStr.AppendChar(c);
            }
            return secureStr;
        }


        public static bool IsNullOrEmpty(SecureString value)
        {
            bool temp = true;

            if (value == null || value.Length == 0)
            {
                temp = true;
            }
            {
                temp = false;
            }

            return temp;
        }

        public static bool IsNullOrWhiteSpace(SecureString value) { return true; }
    }
}
