

namespace HelpProvider2Example
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    public static class HelpExtensions
    {
        public static void ShowPopup2(Control parent, string caption, Point location, Font font = null, Color? backColor = null, Color? foreColor = null)
        {
            font = font ?? SystemFonts.CaptionFont;
            backColor = backColor ?? Color.FromKnownColor(KnownColor.Window);
            foreColor = foreColor ?? Color.FromKnownColor(KnownColor.WindowText);

            var popup = new HH_POPUP();
            popup.clrBackground = new COLORREF(backColor.Value);
            popup.clrForeground = new COLORREF(foreColor.Value);
            popup.pt = new POINT(location);
            var pszText = Marshal.StringToCoTaskMemAuto(caption);
            popup.pszText = pszText;
            var pszFont = Marshal.StringToCoTaskMemAuto(
                $"{font.Name}, {font.Size}, , " +
                $"{(font.Bold ? "BOLD" : "")}" +
                $"{(font.Italic ? "ITALIC" : "")}" +
                $"{(font.Underline ? "UNDERLINE" : "")}");
            popup.pszFont = pszFont;
            try
            {
                HtmlHelp(parent.Handle, null, HTMLHelpCommand.HH_DISPLAY_TEXT_POPUP, popup);
            }
            finally
            {
                Marshal.FreeCoTaskMem(pszText);
                Marshal.FreeCoTaskMem(pszFont);
            }
        }

        [Flags()]
        public enum HTMLHelpCommand : uint
        {
            HH_DISPLAY_TOPIC = 0,
            HH_DISPLAY_TOC = 1,
            HH_DISPLAY_INDEX = 2,
            HH_DISPLAY_SEARCH = 3,
            HH_DISPLAY_TEXT_POPUP = 0x000E,
            HH_HELP_CONTEXT = 0x000F,
            HH_CLOSE_ALL = 0x0012
        }

        [DllImport("hhctrl.ocx", SetLastError = true, EntryPoint = "HtmlHelpW", CharSet = CharSet.Unicode)]
        static extern int HtmlHelp(IntPtr hWndCaller,
            [MarshalAs(UnmanagedType.LPWStr)] string pszFile,
            HTMLHelpCommand uCommand,
            [MarshalAs(UnmanagedType.LPStruct)] HH_POPUP dwData);

        [StructLayout(LayoutKind.Sequential)]
        struct COLORREF
        {
            int ColorRef;

            public COLORREF(int lRGB)
            {
                ColorRef = lRGB & 0x00ffffff;
            }
            public COLORREF(Color color) : this(color.ToArgb())
            {
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        class POINT
        {
            public int x;
            public int y;
            public POINT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
            public POINT(Point p) : this(p.X, p.Y)
            {
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
            public RECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }
            public RECT(Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom)
            {
            }
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        class HH_POPUP
        {
            internal int cbStruct = Marshal.SizeOf(typeof(HH_POPUP));
            internal IntPtr hinst = IntPtr.Zero;
            internal int idString = 0;
            internal IntPtr pszText;
            internal POINT pt;
            internal COLORREF clrForeground = new COLORREF(-1);
            internal COLORREF clrBackground = new COLORREF(-1);
            internal RECT rcMargins = new RECT(-1, -1, -1, -1);
            internal IntPtr pszFont;
        }
    }


}
