using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace EerieEdit.Forms
{
    public partial class GeneratedSQL : Form
    {
        public GeneratedSQL(string sqlString)
        {
            InitializeComponent();

            textBox.Text = sqlString;
        }

		void SetUndoRedoItems()
		{
			undoToolStripMenuItem.Enabled = textBox.CanUndo;
		}

		private void undoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			textBox.Undo();

		}

		private void newToolStripMenuItem_Click(object sender, EventArgs e)
		{
			textBox.Clear();
		}

		private void cutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			textBox.Cut();
		}

		private void copyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			textBox.Copy();
		}

		private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			textBox.Paste();
		}

		private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			textBox.SelectAll();
		}

		bool savedFile = false;
		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var fsd = new SaveFileDialog();
			fsd.Filter = "SQL files|*.sql";
			fsd.FilterIndex = 0;

			if (fsd.ShowDialog(this) == DialogResult.OK)
			{
				using (var writer = new StreamWriter(fsd.OpenFile()))
					foreach (var line in textBox.Lines)
						writer.WriteLine(line);
				savedFile = true;
			}
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void GeneratedSQL_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.UserClosing || e.CloseReason == CloseReason.FormOwnerClosing)
			{
				if (savedFile)
				{
					e.Cancel = false;
					return;
				}

				var dr = MessageBox.Show(this, "The generated file has not been saved\nDo you want to save the file now?", "Save file", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
				if (dr != DialogResult.No)
					e.Cancel = false;

				if (dr == DialogResult.Yes)
					saveToolStripMenuItem_Click(sender, e);
			}
		}
    }
}
