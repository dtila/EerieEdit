using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using EerieEdit.ERObjects;
using System.Diagnostics;

namespace EerieEdit.Forms
{
    public partial class Tables : Form
    {
        DiagramEditor diagramEditor;
        public Tables(DiagramEditor diagramEditor)
        {
            InitializeComponent();
            this.diagramEditor = diagramEditor;

			Icon = (diagramEditor.Parent.Parent.Parent as Form).Icon;
        }

        #region Create tables
        public void CreateTables()
        {
            tableViewer.Clear();

            var emitedTables = new Dictionary<ERObject, Table>();
            foreach (var obj in diagramEditor.Objects)
                if (obj is Entity)
                    emitedTables.Add(obj, EmitTable(obj, emitedTables));

            foreach (var rel in diagramEditor.Objects)
                if (rel is Relationship)
                {
                    var relationshipTable = EmitTable(rel, emitedTables);
                    emitedTables.Add(rel, relationshipTable);

                    foreach (var ent in rel.GetNeighbours<Entity>())
                        foreach (var key in ent.GetPrimaryKey())
                        {
                            Table entTable = null;
                            emitedTables.TryGetValue(key, out entTable);
                            relationshipTable.AddPrimaryItem(key, entTable);
                        }
                }

            foreach (var table in tableViewer)
                foreach (var link in new Dictionary<ERObject, Table>(table.primaryKeysLinkedTables))
                    if (link.Value == null && emitedTables.ContainsKey(link.Key.ParentObject))
                        table.SetPrimaryItem(link.Key, emitedTables[link.Key.ParentObject]);
        }

        private Table EmitTable(ERObject obj, Dictionary<ERObject, Table> emitedTables)
        {
            var table = new Table(obj);
            tableViewer.AddTable(table);

            if (obj is Entity)
                foreach (var link in obj.adjacentLinks)
                    if (link.hierarhical && link.second == obj)
                        foreach (var key in link.first.GetPrimaryKey())
                            table.AddPrimaryItem(key, null);
            EmitAttributes(obj, table, emitedTables);

            return table;
        }

        private void EmitAttributes(ERObject obj, Table table, Dictionary<ERObject, Table> emitedTables)
        {
            foreach (var attribute in obj.Attributes)
                if (attribute != obj.ParentObject && attribute.Type != AttributeType.Derived)
                    if (attribute.Type == AttributeType.Multivalued)
                    {
                        var t = EmitTable(attribute, emitedTables);
                        tableViewer.AddTable(t);
                        foreach (var key in attribute.ParentObject.GetPrimaryKey())
                            t.AddPrimaryItem(key, null);
                    }
                    else
                        if (attribute.IsCompound)
                            EmitAttributes(attribute, table, emitedTables);
                        else if (attribute.PrimaryKey)
                            table.AddPrimaryItem(attribute, null);
                        else
                            table.AddItem(attribute);
        }
        #endregion

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateTables();
        }

        private void generateSQLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sb = tableViewer.GenerateSQL();
            if (sb != null)
				new GeneratedSQL(sb.ToString()).Show(this);
        }

		private void toolStripMenuItem2_Click(object sender, EventArgs e)
		{
			using (var fsd = new SaveFileDialog())
			{
				fsd.Filter = "PNG files|*.png";
				fsd.FilterIndex = 0;

				if (fsd.ShowDialog(this) == DialogResult.OK)
					using (var stream = fsd.OpenFile())
						tableViewer.Save(stream);
			}
		}


    }
}
