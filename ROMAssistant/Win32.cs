using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;

namespace ROMAssistant
{
    /// <summary>
    /// Summary description for Win32.
    /// </summary>
    public class Win32
    {
        // The WM_COMMAND message is sent when the user selects a command item from 
        // a menu, when a control sends a notification message to its parent window, 
        // or when an accelerator keystroke is translated.
        public const int WM_KEYDOWN = 0x100;
        public const int WM_KEYUP = 0x101;
        public const int WM_COMMAND = 0x111;
        public const int WM_LBUTTONDOWN = 0x201;
        public const int WM_LBUTTONUP = 0x202;
        public const int WM_LBUTTONDBLCLK = 0x203;
        public const int WM_RBUTTONDOWN = 0x204;
        public const int WM_RBUTTONUP = 0x205;
        public const int WM_RBUTTONDBLCLK = 0x206;
        private const int WM_MOUSEMOVE = 0x0200;
        public const int WM_NCHITTEST = 0x84;
        const uint WM_MOUSEWHEEL = 0x020A;

        [Flags]
        public enum WinMsgMouseKey : int
        {
            MK_NONE = 0x0000,
            MK_LBUTTON = 0x0001,
            MK_RBUTTON = 0x0002,
            MK_SHIFT = 0x0004,
            MK_CONTROL = 0x0008,
            MK_MBUTTON = 0x0010,
            MK_XBUTTON1 = 0x0020,
            MK_XBUTTON2 = 0x0040
        }
        // The FindWindow function retrieves a handle to the top-level window whose
        // class name and window name match the specified strings.
        // This function does not search child windows.
        // This function does not perform a case-sensitive search.
        [DllImport("User32.dll")]
        public static extern int FindWindow(string strClassName, string strWindowName);

        // The FindWindowEx function retrieves a handle to a window whose class name 
        // and window name match the specified strings.
        // The function searches child windows, beginning with the one following the
        // specified child window.
        // This function does not perform a case-sensitive search.
        [DllImport("User32.dll")]
        public static extern int FindWindowEx(
            IntPtr hwndParent,
            IntPtr hwndChildAfter,
            string strClassName,
            string strWindowName);

        // The SendMessage function sends the specified message to a window or windows. 
        // It calls the window procedure for the specified window and does not return
        // until the window procedure has processed the message. 
        [DllImport("User32.dll")]
        public static extern Int32 SendMessage(
            int hWnd,               // handle to destination window
            int Msg,                // message
            int wParam,             // first message parameter
            [MarshalAs(UnmanagedType.LPStr)] string lParam); // second message parameter

        [DllImport("User32.dll")]
        public static extern Int32 SendMessage(
            int hWnd,               // handle to destination window
            int Msg,                // message
            int wParam,             // first message parameter
            int lParam);            // second message parameter

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool PostMessage(int hWnd, uint Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, IntPtr dwExtraInfo);
        private static int MakeLParam(int LoWord, int HiWord)
        {
            return (int)((HiWord << 16) | (LoWord & 0xFFFF));
        }
        public static void Click(int hWnd, int x, int y)
        {
            Win32.SendMessage(hWnd, WM_LBUTTONDOWN, 1, MakeLParam(x, y));
            Win32.SendMessage(hWnd, WM_LBUTTONUP, 0, MakeLParam(x, y));
            Win32.SendMessage(hWnd, WM_NCHITTEST, 0, MakeLParam(x, y));
        }

        public static void SendY(int hWnd)
        {
            Win32.PostMessage(hWnd, WM_KEYDOWN, (int)Keys.Y, 0);
            Win32.PostMessage(hWnd, WM_KEYUP, (int)Keys.Y, 0);
        }

        public static void SendU(int hWnd)
        {
            Win32.PostMessage(hWnd, WM_KEYDOWN, (int)Keys.U, 0);
            Win32.PostMessage(hWnd, WM_KEYUP, (int)Keys.U, 0);
        }

        public static void ScrollDown(int hWnd, int x, int y)
        {
            //Win32.SendMessage(hWnd, WM_LBUTTONDOWN, 1, MakeLParam(x, y));
            //Win32.SendMessage(hWnd, WM_MOUSEMOVE, 0, MakeLParam(x, y ));
            ////Win32.SendMessage(hWnd, WM_LBUTTONUP, 0, MakeLParam(x, y));
            //////Win32.SendMessage(hWnd, WM_NCHITTEST, 0, MakeLParam(x, y));
            //Win32.SendMessage(hWnd, MOUSEEVENTF_WHEEL, 0, MakeLParam(x, y - 30 ));
            //mouse_event(MOUSEEVENTF_WHEEL, x, y, 120, (UIntPtr)hWnd);
            int directionUp = 1;
            int directionDown = -1;

            IntPtr wParam = MAKEWPARAM(directionDown, .5f, WinMsgMouseKey.MK_LBUTTON);
            IntPtr lParam = MAKELPARAM(x, y);
            Win32.PostMessage(hWnd, WM_MOUSEWHEEL, 120, 10);
        }
        internal static IntPtr MAKEWPARAM(int direction, float multiplier, WinMsgMouseKey button)
        {
            int delta = 120;//(int)(SystemInformation.MouseWheelScrollDelta * multiplier);
            return (IntPtr)(((delta << 16) * Math.Sign(direction) | (ushort)button));
        }

        internal static IntPtr MAKELPARAM(int low, int high)
        {
            return (IntPtr)((high << 16) | (low & 0xFFFF));
        }

        //[DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        //public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        //static void Main(string[] args)
        //{
        //    Process[] anotherApps = Process.GetProcessesByName("AnotherApp");
        //    if (anotherApps.Length == 0) return;
        //    if (anotherApps[0] != null)
        //    {
        //        var allChildWindows = new WindowHandleInfo(anotherApps[0].MainWindowHandle).GetAllChildHandles();
        //    }
        //}

    }
    public class WindowHandleInfo
    {
        private delegate bool EnumWindowProc(IntPtr hwnd, IntPtr lParam);

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr lParam);

        private IntPtr _MainHandle;

        public WindowHandleInfo(IntPtr handle)
        {
            this._MainHandle = handle;
        }

        public List<IntPtr> GetAllChildHandles()
        {
            List<IntPtr> childHandles = new List<IntPtr>();

            GCHandle gcChildhandlesList = GCHandle.Alloc(childHandles);
            IntPtr pointerChildHandlesList = GCHandle.ToIntPtr(gcChildhandlesList);

            try
            {
                EnumWindowProc childProc = new EnumWindowProc(EnumWindow);
                EnumChildWindows(this._MainHandle, childProc, pointerChildHandlesList);
            }
            finally
            {
                gcChildhandlesList.Free();
            }

            return childHandles;
        }

        private bool EnumWindow(IntPtr hWnd, IntPtr lParam)
        {
            GCHandle gcChildhandlesList = GCHandle.FromIntPtr(lParam);

            if (gcChildhandlesList == null || gcChildhandlesList.Target == null)
            {
                return false;
            }

            List<IntPtr> childHandles = gcChildhandlesList.Target as List<IntPtr>;
            childHandles.Add(hWnd);

            return true;
        }
    }
}