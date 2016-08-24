// <copyright file="Win32.cs" company="POQDavid">
//     Copyright (c) POQDavid. All rights reserved.
// </copyright>
// <author>POQDavid</author>
// <summary>
// This is the Win32 class.
// </summary>
namespace OpenFlareClient
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Internal class containing all the Win32 calls used.
    /// </summary>
    internal class Win32
    {
        /// <summary>
        /// Int GWLWNDPROC (-4)
        /// </summary>
        internal const int GWLWNDPROC = -4;

        /// <summary>
        /// Int IPCOFGET (23948)
        /// </summary>
        internal const int IPCOFGET = 23948;

        /// <summary>
        /// Int WMCOMMAND (0x111)
        /// </summary>
        internal const int WMCOMMAND = 0x111;

        /// <summary>
        /// Int WMCOPYDATA (0x4A)
        /// </summary>
        internal const int WMCOPYDATA = 0x4A;

        /// <summary>
        /// Int WMUSER (0x400)
        /// </summary>
        internal const int WMUSER = 0x400;

        /// <summary>
        /// Int Win32WndProc
        /// </summary>
        /// <param name="hWnd">Window handle</param>
        /// <param name="msg">Int msg</param>
        /// <param name="wParam">Additional message-specific information. wParam</param>
        /// <param name="lParam">Additional message-specific information. lParam</param>
        /// <returns>Returns result</returns>
        internal delegate int Win32WndProc(IntPtr hWnd, int msg, int wParam, int lParam);

        /// <summary>
        /// Passes message information to the specified window procedure.
        /// </summary>
        /// <param name="lpPrevWndFunc">The previous window procedure. If this value is obtained by calling the GetWindowLong function with the nIndex parameter set to GWL_WNDPROC or DWL_DLGPROC, it is actually either the address of a window or dialog box procedure, or a special internal value meaningful only to CallWindowProc. </param>
        /// <param name="hwnd">A handle to the window procedure to receive the message. </param>
        /// <param name="msg">The message.</param>
        /// <param name="wParam">Additional message-specific information. The contents of this parameter depend on the value of the Msg parameter. wParam</param>
        /// <param name="lParam">Additional message-specific information. The contents of this parameter depend on the value of the Msg parameter. lParam</param>
        /// <returns>Returns value specifies the result of the message processing and depends on the message sent.</returns>
        [DllImport("user32")]
        internal static extern int CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hwnd, int msg, int wParam, int lParam);

        /// <summary>
        /// Retrieves a handle to the top-level window whose class name and window name match the specified strings. This
        /// function does not search child windows. This function does not perform a case-sensitive search. To search child
        /// windows, beginning with a specified child window, use the
        /// <see cref="!:https://msdn.microsoft.com/en-us/library/windows/desktop/ms633500%28v=vs.85%29.aspx">FindWindowEx</see>
        /// function.
        /// <para>
        /// Go to https://msdn.microsoft.com/en-us/library/windows/desktop/ms633499%28v=vs.85%29.aspx for FindWindow
        /// information or https://msdn.microsoft.com/en-us/library/windows/desktop/ms633500%28v=vs.85%29.aspx for
        /// FindWindowEx
        /// </para>
        /// </summary>
        /// <param name="lpClassName">
        /// C++ ( lpClassName [in, optional]. Type: LPCTSTR )<br />The class name or a class atom created by a previous call to
        /// the RegisterClass or RegisterClassEx function. The atom must be in the low-order word of lpClassName; the
        /// high-order word must be zero.
        /// <para>
        /// If lpClassName points to a string, it specifies the window class name. The class name can be any name
        /// registered with RegisterClass or RegisterClassEx, or any of the predefined control-class names.
        /// </para>
        /// <para>If lpClassName is NULL, it finds any window whose title matches the lpWindowName parameter.</para>
        /// </param>
        /// <param name="lpWindowName">
        /// C++ ( lpWindowName [in, optional]. Type: LPCTSTR )<br />The window name (the window's
        /// title). If this parameter is NULL, all window names match.
        /// </param>
        /// <returns>
        /// C++ ( Type: HWND )<br />If the function succeeds, the return value is a handle to the window that has the
        /// specified class name and window name. If the function fails, the return value is NULL.
        /// <para>To get extended error information, call GetLastError.</para>
        /// </returns>
        /// <remarks>
        /// If the lpWindowName parameter is not NULL, FindWindow calls the <see cref="M:GetWindowText" /> function to
        /// retrieve the window name for comparison. For a description of a potential problem that can arise, see the Remarks
        /// for <see cref="M:GetWindowText" />.
        /// </remarks>
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        /// <summary>
        /// Sends the specified message to a window or windows. The SendMessage function calls the window procedure for the specified window and does not return until the window procedure has processed the message.
        /// </summary>
        /// <param name="hWnd">A handle to the window whose window procedure will receive the message. If this parameter is HWND_BROADCAST ((HWND)0xffff), the message is sent to all top-level windows in the system, including disabled or invisible unowned windows, overlapped windows, and pop-up windows; but the message is not sent to child windows.</param>
        /// <param name="msg">The message to be sent.</param>
        /// <param name="wParam">Additional message-specific information. wParam</param>
        /// <param name="lParam">Additional message-specific information. lParam</param>
        /// <returns>Returns value specifies the result of the message processing; it depends on the message sent.</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// Sends the specified message to a window or windows. The SendMessage function calls the window procedure for the specified window and does not return until the window procedure has processed the message.
        /// </summary>
        /// <param name="hWnd">A handle to the window whose window procedure will receive the message. If this parameter is HWND_BROADCAST ((HWND)0xffff), the message is sent to all top-level windows in the system, including disabled or invisible unowned windows, overlapped windows, and pop-up windows; but the message is not sent to child windows.</param>
        /// <param name="msg">The message to be sent.</param>
        /// <param name="wParam">Additional message-specific information. wParam</param>
        /// <param name="lParam">Additional message-specific information. lParam</param>
        /// <returns>Returns value specifies the result of the message processing; it depends on the message sent.</returns>
        [DllImport("user32.dll")]
        internal static extern IntPtr SendMessage(IntPtr hWnd, uint msg, int wParam, int lParam);

        /// <summary>
        /// Sends the specified message to a window or windows. The SendMessage function calls the window procedure for the specified window and does not return until the window procedure has processed the message.
        /// </summary>
        /// <param name="hWnd">A handle to the window whose window procedure will receive the message. If this parameter is HWND_BROADCAST ((HWND)0xffff), the message is sent to all top-level windows in the system, including disabled or invisible unowned windows, overlapped windows, and pop-up windows; but the message is not sent to child windows.</param>
        /// <param name="msg">The message to be sent.</param>
        /// <param name="wParam">Additional message-specific information. wParam</param>
        /// <param name="lParam">Additional message-specific information. lParam</param>
        /// <returns>Returns value specifies the result of the message processing; it depends on the message sent.</returns>
        [DllImport("user32.dll")]
        internal static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, int lParam);

        /// <summary>
        /// Changes an attribute of the specified window. The function also sets the 32-bit (long) value at the specified offset into the extra window memory.
        /// </summary>
        /// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs.</param>
        /// <param name="nIndex">The zero-based offset to the value to be set. Valid values are in the range zero through the number of bytes of extra window memory, minus the size of an integer.</param>
        /// <param name="newProc">Win32WndProc newProc</param>
        /// <returns>If the previous value of the specified 32-bit integer is zero, and the function succeeds, the return value is zero, but the function does not clear the last error information. This makes it difficult to determine success or failure. To deal with this, you should clear the last error information by calling SetLastError with 0 before calling SetWindowLong. Then, function failure will be indicated by a return value of zero and a GetLastError result that is nonzero.</returns>
        [DllImport("user32")]
        internal static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, Win32WndProc newProc);

        /// <summary>
        /// Changes an attribute of the specified window. The function also sets the 32-bit (long) value at the specified offset into the extra window memory.
        /// </summary>
        /// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs.</param>
        /// <param name="nIndex">The zero-based offset to the value to be set. Valid values are in the range zero through the number of bytes of extra window memory, minus the size of an integer.</param>
        /// <param name="newProc">IntPtr newProc</param>
        /// <returns>If the previous value of the specified 32-bit integer is zero, and the function succeeds, the return value is zero, but the function does not clear the last error information. This makes it difficult to determine success or failure. To deal with this, you should clear the last error information by calling SetLastError with 0 before calling SetWindowLong. Then, function failure will be indicated by a return value of zero and a GetLastError result that is nonzero.</returns>
        [DllImport("user32")]
        internal static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr newProc);

        /// <summary>
        /// Contains data to be passed to another application by the WM_COPYDATA message.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        internal struct CopyDataStruct
        {
            /// <summary>
            /// The data to be passed to the receiving application.
            /// </summary>
            public IntPtr DwData;

            /// <summary>
            /// The size, in bytes, of the data pointed to by the lpData member.
            /// </summary>
            public int CbData;

            /// <summary>
            /// The data to be passed to the receiving application. This member can be NULL.
            /// </summary>
            [MarshalAs(UnmanagedType.LPWStr)]
            public string LpData;
        }
    }
}