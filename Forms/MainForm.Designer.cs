namespace EerieEdit
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.MainMenu mainMenu;
			System.Windows.Forms.MenuItem menuItem1;
			System.Windows.Forms.MenuItem menuItem7;
			System.Windows.Forms.MenuItem menuItem2;
			System.Windows.Forms.MenuItem menuItem3;
			System.Windows.Forms.MenuItem menuItem4;
			System.Windows.Forms.MenuItem menuItem13;
			System.Windows.Forms.MenuItem mnuExport;
			System.Windows.Forms.MenuItem menuItem6;
			System.Windows.Forms.MenuItem menuItem10;
			System.Windows.Forms.MenuItem menuItem12;
			System.Windows.Forms.MenuItem cmdSelectAll;
			System.Windows.Forms.MenuItem menuItem11;
			System.Windows.Forms.MenuItem cmdSelect;
			System.Windows.Forms.MenuItem cmdAttribute;
			System.Windows.Forms.MenuItem cmdEntity;
			System.Windows.Forms.MenuItem cmdRelationship;
			System.Windows.Forms.MenuItem menuItem8;
			System.Windows.Forms.MenuItem menuItem9;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.menuEdit_Undo = new System.Windows.Forms.MenuItem();
			this.menuEdit_Redo = new System.Windows.Forms.MenuItem();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusLbl = new System.Windows.Forms.ToolStripStatusLabel();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.toolStrip = new System.Windows.Forms.ToolStrip();
			this.newDocumentToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.openDocumentToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.saveDocumentToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.selectToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.createLinkToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.createHierarchicalLinkToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.drawAttributeToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.drawEntityToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.drawRelationshipToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.btnCreateTables = new System.Windows.Forms.ToolStripButton();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.splitContainer = new System.Windows.Forms.SplitContainer();
			this.diagramEditor = new EerieEdit.DiagramEditor();
			this.propertyGrid = new System.Windows.Forms.PropertyGrid();
			mainMenu = new System.Windows.Forms.MainMenu(this.components);
			menuItem1 = new System.Windows.Forms.MenuItem();
			menuItem7 = new System.Windows.Forms.MenuItem();
			menuItem2 = new System.Windows.Forms.MenuItem();
			menuItem3 = new System.Windows.Forms.MenuItem();
			menuItem4 = new System.Windows.Forms.MenuItem();
			menuItem13 = new System.Windows.Forms.MenuItem();
			mnuExport = new System.Windows.Forms.MenuItem();
			menuItem6 = new System.Windows.Forms.MenuItem();
			menuItem10 = new System.Windows.Forms.MenuItem();
			menuItem12 = new System.Windows.Forms.MenuItem();
			cmdSelectAll = new System.Windows.Forms.MenuItem();
			menuItem11 = new System.Windows.Forms.MenuItem();
			cmdSelect = new System.Windows.Forms.MenuItem();
			cmdAttribute = new System.Windows.Forms.MenuItem();
			cmdEntity = new System.Windows.Forms.MenuItem();
			cmdRelationship = new System.Windows.Forms.MenuItem();
			menuItem8 = new System.Windows.Forms.MenuItem();
			menuItem9 = new System.Windows.Forms.MenuItem();
			this.statusStrip1.SuspendLayout();
			this.toolStrip.SuspendLayout();
			this.splitContainer.Panel1.SuspendLayout();
			this.splitContainer.Panel2.SuspendLayout();
			this.splitContainer.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainMenu
			// 
			mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            menuItem1,
            menuItem10,
            menuItem11,
            menuItem8});
			// 
			// menuItem1
			// 
			menuItem1.Index = 0;
			menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            menuItem7,
            menuItem2,
            menuItem3,
            menuItem4,
            menuItem13,
            mnuExport,
            this.menuItem5,
            menuItem6});
			menuItem1.Text = "&File";
			// 
			// menuItem7
			// 
			menuItem7.Index = 0;
			menuItem7.Shortcut = System.Windows.Forms.Shortcut.CtrlN;
			menuItem7.Text = "&New";
			menuItem7.Click += new System.EventHandler(this.cmdNewDocument);
			// 
			// menuItem2
			// 
			menuItem2.Index = 1;
			menuItem2.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
			menuItem2.Text = "&Open...";
			menuItem2.Click += new System.EventHandler(this.cmdOpenDocument);
			// 
			// menuItem3
			// 
			menuItem3.Index = 2;
			menuItem3.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
			menuItem3.Text = "&Save";
			menuItem3.Click += new System.EventHandler(this.cmdSaveDocument);
			// 
			// menuItem4
			// 
			menuItem4.Index = 3;
			menuItem4.Text = "Save &As...";
			menuItem4.Click += new System.EventHandler(this.saveAsDocumentToolStripButton_Click);
			// 
			// menuItem13
			// 
			menuItem13.Index = 4;
			menuItem13.Text = "-";
			// 
			// mnuExport
			// 
			mnuExport.Index = 5;
			mnuExport.Text = "&Export...";
			mnuExport.Click += new System.EventHandler(this.mnuExport_Click);
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 6;
			this.menuItem5.Text = "-";
			// 
			// menuItem6
			// 
			menuItem6.Index = 7;
			menuItem6.Text = "E&xit";
			menuItem6.Click += new System.EventHandler(this.menuItem6_Click);
			// 
			// menuItem10
			// 
			menuItem10.Index = 1;
			menuItem10.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuEdit_Undo,
            this.menuEdit_Redo,
            menuItem12,
            cmdSelectAll});
			menuItem10.Text = "&Edit";
			// 
			// menuEdit_Undo
			// 
			this.menuEdit_Undo.Enabled = false;
			this.menuEdit_Undo.Index = 0;
			this.menuEdit_Undo.Shortcut = System.Windows.Forms.Shortcut.CtrlZ;
			this.menuEdit_Undo.Text = "Undo";
			this.menuEdit_Undo.Click += new System.EventHandler(this.menuEdit_Undo_Click);
			// 
			// menuEdit_Redo
			// 
			this.menuEdit_Redo.Enabled = false;
			this.menuEdit_Redo.Index = 1;
			this.menuEdit_Redo.Shortcut = System.Windows.Forms.Shortcut.CtrlY;
			this.menuEdit_Redo.Text = "Redo";
			this.menuEdit_Redo.Click += new System.EventHandler(this.menuEdit_Redo_Click);
			// 
			// menuItem12
			// 
			menuItem12.Index = 2;
			menuItem12.Text = "-";
			// 
			// cmdSelectAll
			// 
			cmdSelectAll.Index = 3;
			cmdSelectAll.Shortcut = System.Windows.Forms.Shortcut.CtrlA;
			cmdSelectAll.Text = "Select &All";
			cmdSelectAll.Click += new System.EventHandler(this.cmdSelectAll_Click);
			// 
			// menuItem11
			// 
			menuItem11.Index = 2;
			menuItem11.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            cmdSelect,
            cmdAttribute,
            cmdEntity,
            cmdRelationship});
			menuItem11.Text = "&Tool";
			// 
			// cmdSelect
			// 
			cmdSelect.Index = 0;
			cmdSelect.Text = "&Select";
			cmdSelect.Click += new System.EventHandler(this.cmdSelectMode);
			// 
			// cmdAttribute
			// 
			cmdAttribute.Index = 1;
			cmdAttribute.Text = "&Attribute";
			cmdAttribute.Click += new System.EventHandler(this.drawAttributeToolStripButton_Click);
			// 
			// cmdEntity
			// 
			cmdEntity.Index = 2;
			cmdEntity.Text = "&Entity";
			cmdEntity.Click += new System.EventHandler(this.drawEntityToolStripButton_Click);
			// 
			// cmdRelationship
			// 
			cmdRelationship.Index = 3;
			cmdRelationship.Text = "&Relationship";
			cmdRelationship.Click += new System.EventHandler(this.drawRelationshipToolStripButton_Click);
			// 
			// menuItem8
			// 
			menuItem8.Index = 3;
			menuItem8.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            menuItem9});
			menuItem8.Text = "&Help";
			// 
			// menuItem9
			// 
			menuItem9.Index = 0;
			menuItem9.Text = "&About";
			menuItem9.Click += new System.EventHandler(this.menuItem9_Click);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.statusLbl});
			this.statusStrip1.Location = new System.Drawing.Point(0, 536);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(780, 22);
			this.statusStrip1.TabIndex = 0;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.Margin = new System.Windows.Forms.Padding(10, 3, 5, 2);
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(42, 17);
			this.toolStripStatusLabel1.Text = "Status:";
			// 
			// statusLbl
			// 
			this.statusLbl.Name = "statusLbl";
			this.statusLbl.Size = new System.Drawing.Size(76, 17);
			this.statusLbl.Text = "draw anything";
			// 
			// imageList1
			// 
			this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
			this.imageList1.ImageSize = new System.Drawing.Size(24, 24);
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// toolStrip
			// 
			this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newDocumentToolStripButton,
            this.openDocumentToolStripButton,
            this.saveDocumentToolStripButton,
            this.toolStripSeparator1,
            this.selectToolStripButton,
            this.createLinkToolStripButton,
            this.createHierarchicalLinkToolStripButton,
            this.toolStripSeparator2,
            this.drawAttributeToolStripButton,
            this.drawEntityToolStripButton,
            this.drawRelationshipToolStripButton,
            this.toolStripSeparator3,
            this.btnCreateTables});
			this.toolStrip.Location = new System.Drawing.Point(0, 0);
			this.toolStrip.Name = "toolStrip";
			this.toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.toolStrip.Size = new System.Drawing.Size(780, 25);
			this.toolStrip.TabIndex = 4;
			this.toolStrip.Text = "toolStrip1";
			// 
			// newDocumentToolStripButton
			// 
			this.newDocumentToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.newDocumentToolStripButton.Image = global::EerieEdit.Properties.Resources.page;
			this.newDocumentToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.newDocumentToolStripButton.Name = "newDocumentToolStripButton";
			this.newDocumentToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.newDocumentToolStripButton.Text = "New";
			this.newDocumentToolStripButton.ToolTipText = "New document";
			this.newDocumentToolStripButton.Click += new System.EventHandler(this.cmdNewDocument);
			// 
			// openDocumentToolStripButton
			// 
			this.openDocumentToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.openDocumentToolStripButton.Image = global::EerieEdit.Properties.Resources.folder;
			this.openDocumentToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.openDocumentToolStripButton.Name = "openDocumentToolStripButton";
			this.openDocumentToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.openDocumentToolStripButton.Text = "Open";
			this.openDocumentToolStripButton.ToolTipText = "Open file";
			this.openDocumentToolStripButton.Click += new System.EventHandler(this.cmdOpenDocument);
			// 
			// saveDocumentToolStripButton
			// 
			this.saveDocumentToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.saveDocumentToolStripButton.Image = global::EerieEdit.Properties.Resources.saveHS;
			this.saveDocumentToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.saveDocumentToolStripButton.Name = "saveDocumentToolStripButton";
			this.saveDocumentToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.saveDocumentToolStripButton.Text = "Save";
			this.saveDocumentToolStripButton.ToolTipText = "Save document";
			this.saveDocumentToolStripButton.Click += new System.EventHandler(this.cmdSaveDocument);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// selectToolStripButton
			// 
			this.selectToolStripButton.Checked = true;
			this.selectToolStripButton.CheckState = System.Windows.Forms.CheckState.Checked;
			this.selectToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.selectToolStripButton.Image = global::EerieEdit.Properties.Resources.PointerHS;
			this.selectToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.selectToolStripButton.Name = "selectToolStripButton";
			this.selectToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.selectToolStripButton.Text = "toolStripButton4";
			this.selectToolStripButton.ToolTipText = "Switch to selecting mode";
			this.selectToolStripButton.Click += new System.EventHandler(this.cmdSelectMode);
			// 
			// createLinkToolStripButton
			// 
			this.createLinkToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.createLinkToolStripButton.Image = global::EerieEdit.Properties.Resources.linie_editor;
			this.createLinkToolStripButton.ImageTransparentColor = System.Drawing.Color.White;
			this.createLinkToolStripButton.Name = "createLinkToolStripButton";
			this.createLinkToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.createLinkToolStripButton.Text = "toolStripButton1";
			this.createLinkToolStripButton.ToolTipText = "Create link";
			this.createLinkToolStripButton.Click += new System.EventHandler(this.cmdCreateSimpleLink);
			// 
			// createHierarchicalLinkToolStripButton
			// 
			this.createHierarchicalLinkToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.createHierarchicalLinkToolStripButton.Image = global::EerieEdit.Properties.Resources.sageata_editor1;
			this.createHierarchicalLinkToolStripButton.ImageTransparentColor = System.Drawing.Color.White;
			this.createHierarchicalLinkToolStripButton.Name = "createHierarchicalLinkToolStripButton";
			this.createHierarchicalLinkToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.createHierarchicalLinkToolStripButton.Text = "toolStripButton1";
			this.createHierarchicalLinkToolStripButton.ToolTipText = "Create hierarchical link";
			this.createHierarchicalLinkToolStripButton.Click += new System.EventHandler(this.cmdCreateIerarhicalLink);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// drawAttributeToolStripButton
			// 
			this.drawAttributeToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.drawAttributeToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("drawAttributeToolStripButton.Image")));
			this.drawAttributeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.drawAttributeToolStripButton.Name = "drawAttributeToolStripButton";
			this.drawAttributeToolStripButton.Size = new System.Drawing.Size(54, 22);
			this.drawAttributeToolStripButton.Text = "Attribute";
			this.drawAttributeToolStripButton.Click += new System.EventHandler(this.drawAttributeToolStripButton_Click);
			// 
			// drawEntityToolStripButton
			// 
			this.drawEntityToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.drawEntityToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("drawEntityToolStripButton.Image")));
			this.drawEntityToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.drawEntityToolStripButton.Name = "drawEntityToolStripButton";
			this.drawEntityToolStripButton.Size = new System.Drawing.Size(39, 22);
			this.drawEntityToolStripButton.Text = "Entity";
			this.drawEntityToolStripButton.Click += new System.EventHandler(this.drawEntityToolStripButton_Click);
			// 
			// drawRelationshipToolStripButton
			// 
			this.drawRelationshipToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.drawRelationshipToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("drawRelationshipToolStripButton.Image")));
			this.drawRelationshipToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.drawRelationshipToolStripButton.Name = "drawRelationshipToolStripButton";
			this.drawRelationshipToolStripButton.Size = new System.Drawing.Size(69, 22);
			this.drawRelationshipToolStripButton.Text = "Relationship";
			this.drawRelationshipToolStripButton.Click += new System.EventHandler(this.drawRelationshipToolStripButton_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
			// 
			// btnCreateTables
			// 
			this.btnCreateTables.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.btnCreateTables.Image = ((System.Drawing.Image)(resources.GetObject("btnCreateTables.Image")));
			this.btnCreateTables.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnCreateTables.Name = "btnCreateTables";
			this.btnCreateTables.Size = new System.Drawing.Size(76, 22);
			this.btnCreateTables.Text = "Create tables";
			this.btnCreateTables.Click += new System.EventHandler(this.btnCreateTables_Click);
			// 
			// openFileDialog
			// 
			this.openFileDialog.DefaultExt = "erd";
			this.openFileDialog.Filter = "ER diagram|*.erd";
			// 
			// saveFileDialog
			// 
			this.saveFileDialog.DefaultExt = "erd";
			this.saveFileDialog.Filter = "ER diagram|*.erd";
			// 
			// splitContainer
			// 
			this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer.Location = new System.Drawing.Point(0, 25);
			this.splitContainer.Name = "splitContainer";
			// 
			// splitContainer.Panel1
			// 
			this.splitContainer.Panel1.Controls.Add(this.diagramEditor);
			// 
			// splitContainer.Panel2
			// 
			this.splitContainer.Panel2.Controls.Add(this.propertyGrid);
			this.splitContainer.Size = new System.Drawing.Size(780, 511);
			this.splitContainer.SplitterDistance = 559;
			this.splitContainer.TabIndex = 5;
			// 
			// diagramEditor
			// 
			this.diagramEditor.BackColor = System.Drawing.Color.WhiteSmoke;
			this.diagramEditor.Dock = System.Windows.Forms.DockStyle.Fill;
			this.diagramEditor.EditMode = EerieEdit.DiagramEditorMode.Selecting;
			this.diagramEditor.Location = new System.Drawing.Point(0, 0);
			this.diagramEditor.Name = "diagramEditor";
			this.diagramEditor.Size = new System.Drawing.Size(559, 511);
			this.diagramEditor.TabIndex = 1;
			this.diagramEditor.SelectionChange += new System.EventHandler(this.diagramEditor_SelectionChange);
			this.diagramEditor.OnDeleteSelectedObjects += new EerieEdit.Action<object, EerieEdit.CancelEventsArgs>(this.diagramEditor_OnDeleteSelectedObjects);
			this.diagramEditor.OnUndoChanged += new System.EventHandler<EerieEdit.Util.OperationEventArgs>(this.diagramEditor_OnUndoChanged);
			this.diagramEditor.OnCanNotLinkObjects += new System.EventHandler(this.diagramEditor_OnCanNotLinkObjects);
			this.diagramEditor.OnDocumentModified += new System.EventHandler(this.diagramEditor_OnDocumentModified);
			this.diagramEditor.OnDeleteSelectedLink += new System.EventHandler<EerieEdit.CancelEventsArgs>(this.diagramEditor_OnDeleteSelectedLink);
			this.diagramEditor.OnRedoChanged += new System.EventHandler<EerieEdit.Util.OperationEventArgs>(this.diagramEditor_OnRedoChanged);
			this.diagramEditor.OnModeChanged += new System.EventHandler(this.diagramEditor_OnModeChanged);
			// 
			// propertyGrid
			// 
			this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyGrid.Location = new System.Drawing.Point(0, 0);
			this.propertyGrid.Name = "propertyGrid";
			this.propertyGrid.Size = new System.Drawing.Size(217, 511);
			this.propertyGrid.TabIndex = 0;
			this.propertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid_PropertyValueChanged);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(780, 558);
			this.Controls.Add(this.splitContainer);
			this.Controls.Add(this.toolStrip);
			this.Controls.Add(this.statusStrip1);
			this.Menu = mainMenu;
			this.Name = "MainForm";
			this.Text = "E-R Editor - [untitled]";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.toolStrip.ResumeLayout(false);
			this.toolStrip.PerformLayout();
			this.splitContainer.Panel1.ResumeLayout(false);
			this.splitContainer.Panel2.ResumeLayout(false);
			this.splitContainer.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuItem menuItem5;
        private DiagramEditor diagramEditor;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel statusLbl;
        private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.ToolStrip toolStrip;
		private System.Windows.Forms.ToolStripButton newDocumentToolStripButton;
		private System.Windows.Forms.ToolStripButton openDocumentToolStripButton;
        private System.Windows.Forms.ToolStripButton saveDocumentToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton selectToolStripButton;
        private System.Windows.Forms.ToolStripButton drawAttributeToolStripButton;
        private System.Windows.Forms.ToolStripButton drawEntityToolStripButton;
        private System.Windows.Forms.ToolStripButton drawRelationshipToolStripButton;
        private System.Windows.Forms.ToolStripButton createLinkToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ToolStripButton createHierarchicalLinkToolStripButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton btnCreateTables;
        private System.Windows.Forms.MenuItem menuEdit_Undo;
        private System.Windows.Forms.MenuItem menuEdit_Redo;
		private System.Windows.Forms.SplitContainer splitContainer;
		private System.Windows.Forms.PropertyGrid propertyGrid;
	}
}

