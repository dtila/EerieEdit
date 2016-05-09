using System;
using System.Collections.Generic;
using System.Windows.Forms;
using EerieEdit.Forms;
using Microsoft.Win32;

namespace EerieEdit
{
    public delegate void Action();
 
	static class Program
	{
		public static Random randomInstance = new Random();
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

            var args = Environment.GetCommandLineArgs();
            if(args.Length > 1 && args[1] == "-r")
                RegisterExtension();

			Application.Run(new MainForm());
		}

		public static void RegisterExtension()
		{
			using(var key = Registry.CurrentUser.OpenSubKey(@"Software\Classes", true))
            {
				key.CreateSubKey(".erd").SetValue(string.Empty, "EREditorFile");

				var infoKey = key.CreateSubKey("EREditorFile");
				infoKey.SetValue(string.Empty, "E-R diagram file");
				infoKey.CreateSubKey("DefaultIcon").SetValue(string.Empty, Application.ExecutablePath + ",0");
				{
					var shellkey = infoKey.CreateSubKey("shell");
					shellkey.SetValue(string.Empty, "Open");
					{
						var openKey = shellkey.CreateSubKey("Open");
						openKey.SetValue(string.Empty, "&Open in editor");
						openKey.CreateSubKey("command").SetValue(string.Empty, Application.ExecutablePath + " \"%L\"");
					}
				}
			}
		}
	}
}
