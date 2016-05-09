using System;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using EerieEdit.ERObjects;
using System.Diagnostics;
using EerieEdit.Forms;
using System.Text;
using System.Collections.Generic;
using EerieEdit.Util;

namespace EerieEdit
{
	public partial class MainForm : Form
	{
		public static StringFormat stringFormat = new StringFormat();

		public MainForm()
		{
			InitializeComponent();
            if (Environment.OSVersion.Version.Major <= 5)
                ToolStripManager.RenderMode = ToolStripManagerRenderMode.System;
			var icon = NativeMethods.LoadIcon(0x7f00, 0, 0, LoadImageFlags.DefaultSize);
			if (icon != null)
				Icon = icon;
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			stringFormat.Alignment = StringAlignment.Center;
			stringFormat.LineAlignment = StringAlignment.Center;

			var args = Environment.GetCommandLineArgs();
			if (args.Length > 1)
			{
				FileStream stream = null;
				try
				{
					stream = File.OpenRead(args[1]);
					diagramEditor.Load(new BinaryFormatter(), stream, stream.Name);
					Text = string.Format("E-R Editor - [{0}]", Path.GetFileNameWithoutExtension(diagramEditor.DocumentFullPath));
				}
				catch (Exception)
				{
					Text = string.Format("E-R Editor - [{0}]", "untitled");
					diagramEditor.NewDocument();
					MessageBox.Show(this, "The current file is invalid", "Load file", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				finally
				{
					if (stream != null)
						stream.Dispose();
				}
			}
		}



		//File operations
		private void cmdNewDocument(object sender, EventArgs e)
		{
			if (diagramEditor.Objects.Count > 0)
				switch (MessageBox.Show(this, "Save current document?", "Save", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
				{
					case DialogResult.Yes: cmdSaveDocument(null, EventArgs.Empty); break;
					case DialogResult.No: diagramEditor.NewDocument(); break;
				}
		}

		private void cmdOpenDocument(object sender, EventArgs e)
		{
			openFileDialog.FileName = String.Empty;
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                var stream = File.OpenRead(openFileDialog.FileName);
                try
                {
                    diagramEditor.Load(new BinaryFormatter(), stream, stream.Name);
					Text = string.Format("E-R Editor - [{0}]", Path.GetFileNameWithoutExtension(diagramEditor.DocumentFullPath));
                }
                catch (Exception)
                {
					Text = string.Format("E-R Editor - [{0}]", "untitled");
					diagramEditor.NewDocument();
                    MessageBox.Show(this, "The current file is invalid", "Load file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    stream.Dispose();
                }
            }
		}

		private void cmdSaveDocument(object sender, EventArgs e)
		{
			var formatter = new BinaryFormatter();
			formatter.AssemblyFormat = FormatterAssemblyStyle.Simple;
			formatter.TypeFormat = FormatterTypeStyle.TypesWhenNeeded;

			saveFileDialog.FileName = String.Empty;
			if(diagramEditor.DocumentFullPath == null)
				saveAsDocumentToolStripButton_Click(sender, e);
			else
				using (var stream = File.OpenWrite(diagramEditor.DocumentFullPath))
				{
					diagramEditor.SaveAs(formatter, stream);
					Text = string.Format("E-R Editor - [{0}]", Path.GetFileNameWithoutExtension(diagramEditor.DocumentFullPath));
				}
		}

		private void saveAsDocumentToolStripButton_Click(object sender, EventArgs e)
		{
			var formatter = new BinaryFormatter();
			formatter.AssemblyFormat = FormatterAssemblyStyle.Simple;
			formatter.TypeFormat = FormatterTypeStyle.TypesWhenNeeded;

			saveFileDialog.FileName = String.Empty;
			if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
			{
				using (var stream = File.Create(saveFileDialog.FileName))
					diagramEditor.Save(formatter, stream, saveFileDialog.FileName);
				Text = string.Format("E-R Editor - [{0}]", Path.GetFileNameWithoutExtension(diagramEditor.DocumentFullPath));
			}
		}

		private void cmdSelectMode(object sender, EventArgs e)
		{
			diagramEditor.EditMode = DiagramEditorMode.Selecting;
		}

		private void cmdCreateSimpleLink(object sender, EventArgs e)
		{
			diagramEditor.EditMode = DiagramEditorMode.CreateSimpleLink;
		}

		private void cmdCreateIerarhicalLink(object sender, EventArgs e)
		{
			diagramEditor.EditMode = DiagramEditorMode.CreateIerarhicalLink;
		}

		private void drawAttributeToolStripButton_Click(object sender, EventArgs e)
		{
			using (var form = new AttributeForm())
				if (form.ShowDialog() == DialogResult.OK)
					diagramEditor.AppendNewObject(new ERObjects.Attribute(diagramEditor, form.AttributeName, form.Primary, form.CanBeNull, form.AutoIncrement, form.Type, form.ValueType));
		}

		private void drawEntityToolStripButton_Click(object sender, EventArgs e)
		{
			using (var form = new EntityForm())
				if (form.ShowDialog() == DialogResult.OK)
					diagramEditor.AppendNewObject(new Entity(diagramEditor, form.EntityName));
		}

		private void drawRelationshipToolStripButton_Click(object sender, EventArgs e)
		{
			var relationship = new Relationship(diagramEditor, String.Empty, false);
			using (var form = new RelationshipForm(relationship))
				if (form.ShowDialog() == DialogResult.OK)
				{
					relationship.Name = form.RelationshipName;
					relationship.Identifying = form.Identifying;
					diagramEditor.AppendNewObject(relationship);
				}
		}

		private void menuItem9_Click(object sender, EventArgs e)
		{
			using (var form = new AboutBox())
				form.ShowDialog(this);
		}

		private void menuItem6_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void btnCreateTables_Click(object sender, EventArgs e)
		{
            var form = btnCreateTables.Tag == null ? new Tables(diagramEditor) : (Tables)btnCreateTables.Tag;
            form.CreateTables();
            if (btnCreateTables.Tag == null)
            {
                btnCreateTables.Tag = form;
                form.FormClosed += (s, ee) => btnCreateTables.Tag = null;
                form.Show(this);
            }
            else
                form.Focus();
		}

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(diagramEditor.IsModified)
                switch (MessageBox.Show(this, "The document has been modified. Do you want to save it?", "Save?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                {
                    case DialogResult.OK: cmdSaveDocument(sender, e); break;
                    case DialogResult.Cancel: e.Cancel = true; break;
                }
        }



        private void menuEdit_Undo_Click(object sender, EventArgs e)
        {
            diagramEditor.Undo();
        }

        private void menuEdit_Redo_Click(object sender, EventArgs e)
        {
            diagramEditor.Redo();
        }

		private void cmdSelectAll_Click(object sender, EventArgs e)
		{
			diagramEditor.SelectAll();
		}



		private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
		{
			diagramEditor.Invalidate();
		}

		private void mnuExport_Click(object sender, EventArgs e)
		{
			using (var bmp = new Bitmap(diagramEditor.ClientSize.Width, diagramEditor.ClientSize.Height))
			{
				diagramEditor.DrawToBitmap(bmp, new Rectangle(new Point(), bmp.Size));
				bmp.MakeTransparent(BackColor);

				using (var sfd = new SaveFileDialog())
				{
					sfd.DefaultExt = "png";
					sfd.Filter = "PNG (*.png)|*.png";

					if (sfd.ShowDialog() == DialogResult.OK)
						using (var stream = sfd.OpenFile())
							bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
				}
			}
		}

		#region Editor events
		private void diagramEditor_OnCanNotLinkObjects(object sender, EventArgs e)
		{
			statusLbl.Text = "These objects can not be linked";
		}

		private void diagramEditor_OnDeleteSelectedObjects(object t, CancelEventsArgs t2)
		{
			t2.Cancel = MessageBox.Show(this, "Are you sure to delete selected objects?", "Delete objects", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes;
		}

		private void diagramEditor_OnDeleteSelectedLink(object t, CancelEventsArgs t2)
		{
			t2.Cancel = MessageBox.Show(this, "Are you sure to delete selected link ?", "Delete link", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes;
		}

		private void diagramEditor_OnModeChanged(object sender, EventArgs e)
		{
			selectToolStripButton.Checked = diagramEditor.EditMode == DiagramEditorMode.Selecting;
			createLinkToolStripButton.Checked = diagramEditor.EditMode == DiagramEditorMode.CreateSimpleLink;
			createHierarchicalLinkToolStripButton.Checked = diagramEditor.EditMode == DiagramEditorMode.CreateIerarhicalLink;
		}

		private void diagramEditor_OnUndoChanged(object sender, EerieEdit.Util.OperationEventArgs e)
		{
			menuEdit_Undo.Enabled = e.Enabled;
			menuEdit_Undo.Text = "Undo" + (e.Enabled ? " " + e.OperationName : string.Empty);
			Text = string.Format("E-R Editor - [{0}{1}]", 
				diagramEditor.DocumentFullPath != null ? Path.GetFileNameWithoutExtension(diagramEditor.DocumentFullPath) : "untitled",
				diagramEditor.IsModified ? "*" : string.Empty);
		}

		private void diagramEditor_OnRedoChanged(object sender, EerieEdit.Util.OperationEventArgs e)
		{
			menuEdit_Redo.Enabled = e.Enabled;
			menuEdit_Redo.Text = "Redo" + (e.Enabled ? " " + e.OperationName : string.Empty);
		}

		private void diagramEditor_SelectionChange(object sender, EventArgs e)
		{
			propertyGrid.SelectedObjects = diagramEditor.PropertyGridObjects;
		}

		private void diagramEditor_OnDocumentModified(object sender, EventArgs e)
		{
			Text = string.Format("E-R Editor - [{0}]", diagramEditor.DocumentFullPath != null ? Path.GetFileNameWithoutExtension(diagramEditor.DocumentFullPath) + '*' : "untitled");
		}
		#endregion
	}
}
