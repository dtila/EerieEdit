using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Runtime.ConstrainedExecution;
using System.Windows.Forms;
using System.Diagnostics;

namespace EerieEdit
{
	internal enum ImageType
	{
		Bitmap,
		Icon,
		Cursor,
		EnhMetafile
	}

	public enum StandardCursor
	{
		Arrow = 32512,
		IBeam = 32513,
		Wait = 32514,
		Cross = 32515,
		UpArrow = 32516,
		SizeNWSE = 32642,
		SizeNESW = 32643,
		SizeWE = 32644,
		SizeNS = 32645,
		SizeAll = 32646,
		No = 32648,
		Hand = 32649,
		AppStarting = 32650,
		Help = 32651
	}

	public enum LoadImageFlags
	{
		DefaultSize = 0x0040
	}

	static class NativeMethods
	{
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool DestroyIcon(IntPtr hIcon);

		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail), DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr LoadImage([In] IntPtr hinst, [In] int lpszName, [In] ImageType uType, [In] int cxDesired, [In] int cyDesired, [In] int fuLoad);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr LoadCursor(IntPtr hInstance, int lpCursorName);

		public static Icon LoadIcon(int name, int width, int height, LoadImageFlags flags)
		{
			try
			{
				IntPtr handle = NativeMethods.LoadImage(Process.GetCurrentProcess().MainModule.BaseAddress, name, ImageType.Icon, 0, 0, (int) flags);
				var icon = Icon.FromHandle(handle).Clone() as Icon;
				NativeMethods.DestroyIcon(handle);
				return icon;
			}
			catch
			{
				return null;
			}
		}

		public static Cursor LoadCursor(StandardCursor cursor)
		{
			var handle = LoadCursor(IntPtr.Zero, (int) cursor);
			return new Cursor(handle);
		}
	}
}
