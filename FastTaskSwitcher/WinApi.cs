using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Text;

namespace FastTaskSwitcher
{
    internal static class WinApi
    {
        // Cleanup: Figure out which of these Windows API calls are not used and remove them

        [DllImport("psapi.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumProcesses([MarshalAs(UnmanagedType.LPArray)] uint[] processIDs, uint size, out uint outSize);


        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(ProcessAccessFlags access, [MarshalAs(UnmanagedType.Bool)] bool inheritHandle, uint processID);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public delegate bool EnumWindowsProc(IntPtr hwnd, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtrW")]
        private static extern IntPtr GetWindowLong64(IntPtr hWnd, WindowLong nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongW")]
        private static extern int GetWindowLong32(IntPtr hWnd, WindowLong nIndex);

        // Deprecated
//        [DllImport("user32.dll")]
//        [return: MarshalAs(UnmanagedType.Bool)]
//        public static extern bool GetWindowRect(IntPtr hWnd, out Rect lpRect);

        [DllImport("user32.dll")]
        public static extern IntPtr MonitorFromWindow(IntPtr hwnd, MonitorFromWindowFlags flags);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindow(IntPtr hwnd, GetWindowMode mode);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetLayeredWindowAttributes(IntPtr hwnd, out uint colorKey, out byte alpha, out LayeredWindowFlags flags);

        [DllImport("user32.dll", EntryPoint = "GetWindowText",
        ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpWindowText, int nMaxCount);

        // Note: Below this are new
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        internal static extern IntPtr GetAncestor(IntPtr hWnd, int flags);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        internal static extern IntPtr GetLastActivePopup(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern IntPtr GetShellWindow();

        [DllImport("user32.dll")]
        internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        internal static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out IntPtr ProcessId);

        [DllImport("user32.dll")]
        internal static extern IntPtr AttachThreadInput(IntPtr idAttach, IntPtr idAttachTo, int fAttach);

        [DllImport("Kernel32.dll")]
        internal static extern IntPtr GetCurrentThreadId();

        [DllImport("user32.dll")]
        internal static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        internal static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        internal static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public static IntPtr GetWindowLongPtr(IntPtr hWnd, WindowLong nIndex)
        {
            if (IntPtr.Size == 8)
            {
                return GetWindowLong64(hWnd, nIndex);
            }
            return new IntPtr(GetWindowLong32(hWnd, nIndex));
        }


        // Deprecated
//        public struct Rect
//        {
//            public int Left;
//            public int Top;
//            public int Right;
//            public int Bottom;
//
//            public Point Location
//            {
//                get { return new Point(this.Left, this.Top); }
//            }
//
//            public int Width
//            {
//                get { return this.Right - this.Left; }
//            }
//
//            public int Height
//            {
//                get { return this.Bottom - this.Top; }
//            }
//
//            public Rect(int x, int y, int x1, int y1)
//            {
//                this.Left = x;
//                this.Top = y;
//                this.Right = x1;
//                this.Bottom = y1;
//            }
//
//            public Rect(Rectangle rect)
//            {
//                this.Left = rect.Left;
//                this.Top = rect.Top;
//                this.Right = rect.Right;
//                this.Bottom = rect.Bottom;
//            }
//
//            public Rectangle ToRectangle()
//            {
//                return Rectangle.FromLTRB(this.Left, this.Top, this.Right, this.Bottom);
//            }
//
//            public RectangleF ToRectangleF()
//            {
//                return RectangleF.FromLTRB((float) this.Left, (float) this.Top, (float) this.Right, (float) this.Bottom);
//            }
//
//            public static bool operator == (Rect x, Rect y)
//            {
//                return x.Left == y.Left && x.Top == y.Top && x.Bottom == y.Bottom && x.Right == y.Right;
//            }
//
//            public static bool operator != (Rect x, Rect y)
//            {
//                return x.Left != y.Left || x.Top != y.Top || x.Bottom != y.Bottom || x.Right != y.Right;
//            }
//
//            public override bool Equals(object obj)
//            {
//                if (obj is Rect)
//                {
//                    Rect y = (Rect) obj;
//                    return this == y;
//                }
//                return false;
//            }
//
//            public override int GetHashCode()
//            {
//                return this.Left ^ this.Top ^ this.Bottom ^ this.Right;
//            }
//        }

        [Flags]
        public enum ProcessAccessFlags : uint
        {
            QueryInformation = 1024u,
            VMRead = 16u
        }

        public enum WindowLong
        {
            WndProc = -4,
            HInstance = -6,
            HwndParent = -8,
            Style = -16,
            ExStyle = -20,
            UserData = -21,
            ID = -12
        }

        public enum WindowStyles : uint
        {
            None = 0u,
            Visible = 268435456u,
            Minimize = 536870912u,
            Disabled = 134217728u,
            MinimizeBox = 131072u
        }

        public enum WindowExStyles : uint
        {
            Transparent = 32u,
            TopMost = 8u,
            AppWindow = 262144u,
            ToolWindow = 128u,
            Layered = 524288u,
            NoActivate = 134217728u
        }

        public enum MonitorFromWindowFlags : uint
        {
            DefaultToNull = 0u
        }

        public enum GetWindowMode : uint
        {
            Owner = 4u
        }

        public enum LayeredWindowFlags : uint
        {
            None = 0u,
            ColorKey = 1u,
            Alpha = 2u,
            Opaque = 4u
        }

        public enum FsModifiers : uint
        {
            MOD_ALT = 0x0001,
            MOD_CONTROL = 0x0002,
            MOD_NONREPEAT = 0x4000,
            MOD_SHIFT = 0x0004,
            MOD_WIN = 0x0008
        }
    }
}