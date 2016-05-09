using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace EerieEdit.Forms
{
	public class ValidatingForm : Form
	{
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			const int WM_KEYDOWN = 0x100;

			if (msg.Msg == WM_KEYDOWN && keyData == Keys.Escape)
			{
				DialogResult = DialogResult.Cancel;
				return true;
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}
	}
}
