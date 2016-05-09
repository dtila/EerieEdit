namespace EerieEdit
{
	partial class DiagramEditor
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

		#region Component Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.menuObjects = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuItemRename = new System.Windows.Forms.ToolStripMenuItem();
            this.menuObjects.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuObjects
            // 
            this.menuObjects.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuItemRename});
            this.menuObjects.Name = "Meniu";
            this.menuObjects.Size = new System.Drawing.Size(135, 26);
            this.menuObjects.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.Meniu_ItemClicked);
            // 
            // mnuItemRename
            // 
            this.mnuItemRename.Name = "mnuItemRename";
            this.mnuItemRename.Size = new System.Drawing.Size(134, 22);
            this.mnuItemRename.Text = "&P&roperties";
            // 
            // DiagramEditor
            // 
            this.DoubleClick += new System.EventHandler(this.DiagramEditor_DoubleClick);
            this.menuObjects.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

        public System.Windows.Forms.ToolStripMenuItem mnuItemRename;
        public System.Windows.Forms.ContextMenuStrip menuObjects;

    }
}
