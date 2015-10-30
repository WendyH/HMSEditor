/* This code is released under WTFPL Version 2 (http://www.wtfpl.net/) * Created by WendyH. Copyleft. */
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Drawing;
using System.Security;

namespace HMSEditorNS {
	[SuppressUnmanagedCodeSecurityAttribute]
	internal static class UnsafeNativeMethods {
		internal delegate void   WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);
		internal delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

		[DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool UnhookWindowsHookEx(IntPtr hhk);

		[DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll")]
		internal static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

		[DllImport("user32.dll")]
		internal static extern bool UnhookWinEvent(IntPtr hWinEventHook);

		internal class EventHook: IDisposable {
			IntPtr unmanagedResource;
			bool disposed = false;  // recommended by MS

			public const uint EVENT_MIN = 0x00000001;
			public const uint EVENT_MAX = 0x7FFFFFFF;
			public const uint EVENT_SYSTEM_FOREGROUND  = 3;
			public const uint EVENT_SYSTEM_MOVESIZEEND = 0x000B;
			public const uint EVENT_OBJECT_CREATE         = 0x8000;
			public const uint EVENT_OBJECT_LOCATIONCHANGE = 0x800B;
			public const uint EVENT_OBJECT_NAMECHANGE     = 0x800C;
			public const uint EVENT_OBJECT_VALUECHANGE    = 0x800E;
			public const uint EVENT_OBJECT_DESTROY        = 0x8001;
			public const uint EVENT_OBJECT_SHOW           = 0x8002;
			public const uint EVENT_OBJECT_HIDE           = 0x8003;
			public const uint WINEVENT_OUTOFCONTEXT   = 0;
			public const uint WINEVENT_INCONTEXT      = 0x0004;
			public const uint WINEVENT_SKIPOWNPROCESS = 0x0001;
			public const uint WINEVENT_SKIPOWNTHREAD  = 0x0002;

			readonly WinEventDelegate _procDelegate;
			readonly IntPtr _hWinEventHook;

			public EventHook(WinEventDelegate handler, uint processID, IntPtr hWnd) {
				_procDelegate = handler;
				_hWinEventHook = SetWinEventHook(EVENT_MIN, EVENT_MAX, hWnd, handler, processID, 0, WINEVENT_OUTOFCONTEXT);
			}

			~EventHook() {
				Stop();
				Dispose(false);
			}

			public void Stop() {
				if (_hWinEventHook != IntPtr.Zero) UnhookWinEvent(_hWinEventHook);
			}

			public void Dispose() {
				Dispose(true);
				GC.SuppressFinalize(this);
			}

			protected virtual void Dispose(bool disposing) {
				if (!disposed) {
					if (disposing) {
						// Release managed resources.
					}
					// Free the unmanaged resource ...
					unmanagedResource = IntPtr.Zero;
					disposed = true;
				}
			}

		}
	}

	internal static class NativeMethods {
		private const int timeout = 1000; // 1 sec timeout for SendMessageTimeout function

		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		public static extern int GetWindowText(IntPtr hWnd, StringBuilder title, int size);

		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		public static extern int GetClassName(IntPtr hWnd, StringBuilder name, int size);

		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, [Out] StringBuilder lParam); // for get and set text

		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);              // work with not text

		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, out int wParam, out int lParam);        // work with not text

		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		public static extern IntPtr SendMessageTimeout(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam, uint fuFlags, uint uTimeout, [Out] IntPtr lpdwResult); // 4 get text

		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		public static extern IntPtr SendMessageTimeout(IntPtr hWnd, uint Msg, IntPtr wParam, string lParam, uint fuFlags, uint uTimeout, [Out] IntPtr lpdwResult); // 4 set text

		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		public static extern IntPtr SendMessageTimeout(IntPtr hWnd, uint Msg, IntPtr wParam, [Out] StringBuilder lParam, uint fuFlags, uint uTimeout, [Out] IntPtr lpdwResult);

		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		public static extern bool SendNotifyMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		public static extern bool SendNotifyMessage(IntPtr hWnd, uint Msg, IntPtr wParam, [Out] StringBuilder lParam);

		public delegate void SendAsyncProc(IntPtr hwnd, uint uMsg, UIntPtr dwData, IntPtr lResult);

		[DllImport("User32.dll")]
		public static extern bool CreateCaret(IntPtr hWnd, int hBitmap, int nWidth, int nHeight);

		[DllImport("User32.dll")]
		public static extern bool SetCaretPos(int x, int y);

		[DllImport("User32.dll")]
		public static extern bool DestroyCaret();

		[DllImport("User32.dll")]
		public static extern bool ShowCaret(IntPtr hWnd);

		[DllImport("User32.dll")]
		public static extern bool HideCaret(IntPtr hWnd);

		[DllImport("user32.dll")]
		public static extern IntPtr GetOpenClipboardWindow();

		[DllImport("user32.dll")]
		public static extern IntPtr CloseClipboard();

		[DllImport("Imm32.dll")]
		public static extern IntPtr ImmGetContext(IntPtr hWnd);

		[DllImport("Imm32.dll")]
		public static extern IntPtr ImmAssociateContext(IntPtr hWnd, IntPtr hIMC);

		[DllImport("kernel32.dll")]
		static extern void GetNativeSystemInfo(ref SYSTEM_INFO lpSystemInfo);

		[DllImport("user32.dll")]
		static extern IntPtr GetWindowDC(IntPtr hWnd);

		[DllImport("user32.dll")]
		static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

		[DllImport("gdi32.dll")]
		static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

		[DllImport("gdi32.dll")]
		static extern uint SetPixel(IntPtr hdc, int nXPos, int nYPos, int color);

		[DllImport("kernel32.dll")]
		static extern void GetSystemInfo(ref SYSTEM_INFO lpSystemInfo);

		[StructLayout(LayoutKind.Sequential)]
		struct SYSTEM_INFO {
			public ushort wProcessorArchitecture;
			public ushort wReserved;
			public uint dwPageSize;
			public IntPtr lpMinimumApplicationAddress;
			public IntPtr lpMaximumApplicationAddress;
			public UIntPtr dwActiveProcessorMask;
			public uint dwNumberOfProcessors;
			public uint dwProcessorType;
			public uint dwAllocationGranularity;
			public ushort wProcessorLevel;
			public ushort wProcessorRevision;
		}

		[DllImport("user32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
		public static extern void NotifyWinEvent(int winEvent, IntPtr hwnd, uint objType, int objID);

		[DllImport("user32.dll")]
		public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		[DllImport("user32.dll")]
		public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		public static extern bool SetCursorPos(int X, int Y);

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);
		[StructLayout(LayoutKind.Sequential)]
		public struct RECT {
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;
		}

		[DllImport("User32", CharSet = CharSet.Unicode, ExactSpelling = true)]
		public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndParent);

		[DllImport("user32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
		public static extern IntPtr GetParent(IntPtr hWnd);

		[DllImport("user32.dll")]
		public static extern int GetCaretPos(ref POINT lpPoint);

		[DllImport("user32.dll")]
		public static extern IntPtr GetDlgItem(IntPtr hWnd, int nIDDlgItem);

		[StructLayout(LayoutKind.Sequential)]
		public struct POINT {
			public int X;
			public int Y;

			public static implicit operator Point(POINT point) {
				return new Point(point.X, point.Y);
			}
		}

		[DllImport("user32.dll")]
		public static extern bool GetCursorPos(out POINT lpPoint);

		public static Point GetCursorPosition() {
			POINT lpPoint;
			GetCursorPos(out lpPoint);
			return lpPoint;
		}

		[DllImport("User32.dll", CharSet = CharSet.Unicode)]
		public static extern int SendMessageTimeout(
						  IntPtr hWnd,
						  [MarshalAs(UnmanagedType.U4)] int Msg,
						  IntPtr wParam,
						  IntPtr lParam,
						  [MarshalAs(UnmanagedType.U4)] int fuFlags,
						  [MarshalAs(UnmanagedType.U4)] int uTimeout,
						  [MarshalAs(UnmanagedType.U4)] ref int lpdwResult);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr lParam);

		[DllImport("user32.dll")]
		public static extern bool CloseWindow(IntPtr hWnd);

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern IntPtr GetModuleHandle(string lpModuleName);

		[DllImport("user32.dll", EntryPoint = "WindowFromPoint", CharSet = CharSet.Unicode, ExactSpelling = true)]
		public static extern IntPtr WindowFromPoint(POINT pt);

		[DllImport("Wintrust.dll", PreserveSig = true, SetLastError = false)]
		public static extern uint WinVerifyTrust(IntPtr hWnd, IntPtr pgActionID, IntPtr pWinTrustData);

		public delegate bool EnumWindowProc(IntPtr hwnd, IntPtr lParam);

		const ushort PROCESSOR_ARCHITECTURE_INTEL = 0;
		const ushort PROCESSOR_ARCHITECTURE_IA64 = 6;
		const ushort PROCESSOR_ARCHITECTURE_AMD64 = 9;
		const ushort PROCESSOR_ARCHITECTURE_UNKNOWN = 0xFFFF;

		[StructLayout(LayoutKind.Sequential)]
		internal struct MSLLHOOKSTRUCT {
			public POINT  pt;
			public uint   mouseData;
			public uint   flags;
			public uint   time;
			public IntPtr dwExtraInfo;
		}

		public enum MouseMessages {
			WM_LBUTTONDOWN = 0x0201,
			WM_LBUTTONUP   = 0x0202,
			WM_MOUSEMOVE   = 0x0200,
			WM_MOUSEWHEEL  = 0x020A,
			WM_RBUTTONDOWN = 0x0204,
			WM_RBUTTONUP   = 0x0205
		}

		public enum Platform { X86, X64, Unknown }

		public static Platform GetOperationSystemPlatform() {
			var sysInfo = new SYSTEM_INFO();

			// WinXP and older - use GetNativeSystemInfo
			if (Environment.OSVersion.Version.Major > 5 ||
				(Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor >= 1)) {
				GetNativeSystemInfo(ref sysInfo);
			} else {
				GetSystemInfo(ref sysInfo);
			}

			switch (sysInfo.wProcessorArchitecture) {
				case PROCESSOR_ARCHITECTURE_IA64:
				case PROCESSOR_ARCHITECTURE_AMD64:
					return Platform.X64;

				case PROCESSOR_ARCHITECTURE_INTEL:
					return Platform.X86;

				default:
					return Platform.Unknown;
			}
		}

		public static string GetTextFromControl(IntPtr hwnd) {
			StringBuilder sb = new StringBuilder(265535); // needs to be big enough for the whole text
			IntPtr lpdwResult = IntPtr.Zero;
			SendMessageTimeout(hwnd, WM_GETTEXT, (IntPtr)sb.Capacity, sb, 0, timeout, lpdwResult);
			return sb.ToString();
		}

		public static void SetTextOfControl(IntPtr hwnd, string text) {
			IntPtr lpdwResult = IntPtr.Zero;
			SendMessageTimeout(hwnd, NativeMethods.WM_SETTEXT, (IntPtr)0, text, 0, timeout, lpdwResult);
		}

		public static void SendKeyDown(IntPtr hwnd, int key) {
			IntPtr lpdwResult = IntPtr.Zero;
			SendMessageTimeout(hwnd, WM_KEYDOWN, (IntPtr)key, IntPtr.Zero, 0, timeout, lpdwResult);
		}

		public static void SendNotifyKey(IntPtr hwnd, int key) {
			NativeMethods.SendNotifyMessage(hwnd, NativeMethods.WM_KEYDOWN, (IntPtr)key, IntPtr.Zero);
		}

		public static void SendPosition(IntPtr hwnd, int position) {
			IntPtr lpdwResult = IntPtr.Zero;
			SendMessageTimeout(hwnd, EM_SETSEL, (IntPtr)position, (IntPtr)position, 0, timeout, lpdwResult);
		}

		public static void SendNotifyMessage(IntPtr hWnd, uint Msg) {
			SendNotifyMessage(hWnd, Msg, IntPtr.Zero, IntPtr.Zero);
		}

		public static IntPtr SendMessage(IntPtr hWnd, uint Msg) { return SendMessage(hWnd, Msg, IntPtr.Zero, IntPtr.Zero); }

		public static int GetCaretPosition(IntPtr hWnd) {
			int start = 0; int end = 0;
			SendMessage(hWnd, NativeMethods.EM_GETSEL, out start, out end);
			return start;
        }

		public static Color GetPixelColor(IntPtr hWnd, int x, int y) {
			IntPtr hdc = GetWindowDC(hWnd);
			uint pixel = GetPixel(hdc, x, y);
			ReleaseDC(hWnd, hdc);
			Color color = Color.FromArgb((int)(pixel & 0x000000FF),
						 (int)(pixel & 0x0000FF00) >> 8,
						 (int)(pixel & 0x00FF0000) >> 16);
			return color;
		}

		public static void SetPixelColor(IntPtr hWnd, int x, int y) {
			IntPtr hdc = GetWindowDC(hWnd);
			uint pixel = SetPixel(hdc, x, y, 255);
			ReleaseDC(hWnd, hdc);
		}

		public const int HWND_BROADCAST          = 0xffff;
		public const int WM_SETTINGCHANGE        = 0x001A;
		public const int SMTO_NORMAL             = 0x0000;
		public const int SMTO_BLOCK              = 0x0001;
		public const int SMTO_ABORTIFHUNG        = 0x0002;
		public const int SMTO_NOTIMEOUTIFNOTHUNG = 0x0008;
		public const uint EVENT_MIN = 0x00000001;
		public const uint EVENT_MAX = 0x7FFFFFFF;
		public const uint EVENT_SYSTEM_FOREGROUND  = 0x0003;
		public const uint EVENT_SYSTEM_MOVESIZEEND = 0x000B;
		public const uint EVENT_OBJECT_LOCATIONCHANGE = 0x800B;
		public const uint EVENT_OBJECT_NAMECHANGE     = 0x800C;
		public const uint EVENT_OBJECT_VALUECHANGE    = 0x800E;
		public const uint EVENT_OBJECT_CREATE         = 0x8000;
		public const uint EVENT_OBJECT_DESTROY        = 0x8001;
		public const uint EVENT_OBJECT_SHOW           = 0x8002;
		public const uint EVENT_OBJECT_HIDE           = 0x8003;
		public const uint EVENT_OBJECT_FOCUS          = 0x8005;
		public const uint EVENT_SYSTEM_CAPTURESTART   = 0x0008;
		public const uint EVENT_SYSTEM_CAPTUREEND     = 0x0009;
		public const uint WINEVENT_OUTOFCONTEXT   = 0;
		public const uint WINEVENT_INCONTEXT      = 4;
		public const uint WINEVENT_SKIPOWNPROCESS = 1;
		public const uint WINEVENT_SKIPOWNTHREAD  = 2;
		public const uint EM_GETSEL = 0x00B0;
		public const uint EM_SETSEL = 0x00B1;
		public const uint EM_LINEFROMCHAR = 0x00C9;
		public const uint EM_LINEINDEX    = 0x00BB;
		public const  int SW_HIDE = 0;
		public const  int SW_SHOW = 5;
		public const uint OBJID_CLIENT = 0xFFFFFFFC;
		public const  int SWP_ASYNCWINDOWPOS = 0x4000;
		public const  int SWP_NOREDRAW       = 0x0008;
		public const  int SWP_NOSENDCHANGING = 0x0400;
		public const  int SWP_SHOWWINDOW     = 0x0040;
		public const uint BM_CLICK   = 0x00F5;
		public const uint WM_KEYDOWN    = 0x100;
		public const uint WM_GETTEXT    = 0x0D;
		public const uint WM_SETTEXT    = 0x0C;
		public const uint WM_SETREDRAW  = 0x0B;
		public const uint WM_DESTROY    = 0x02;
		public const uint WM_SHOWWINDOW = 0x18;
		public const uint WM_CLOSE      = 0x10;
		public const uint WM_ACTIVATE   = 0x06;
		public const uint WM_SETFOCUS   = 0x07;
		// https://msdn.microsoft.com/ru-ru/library/windows/desktop/dd375731(v=vs.85).aspx
		public const int VK_SHIFT   = 0x10;
		public const int VK_CONTROL = 0x11;
		public const int VK_MENU    = 0x12; // Alt-Key
		public const int VK_RETURN  = 0x0D;
		public const int VK_F5 = 0x74;
		public const int VK_F6 = 0x75;
		public const int VK_F7 = 0x76;
		public const int VK_F8 = 0x77;
		public const int VK_F9 = 0x78;
		public const int WH_MOUSE_LL = 14;
	}
}
