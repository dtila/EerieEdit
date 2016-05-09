using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.Serialization;
using System.Windows.Forms;
using EerieEdit.ERObjects;
using EerieEdit.Forms;
using EerieEdit.Util;

namespace EerieEdit
{
	public enum DiagramEditorMode
	{
		Selecting,
		CreateSimpleLink,
		CreateIerarhicalLink
	}

	public partial class DiagramEditor : Control
	{
		internal List<ERObject> SelectedObjects = new List<ERObject>();
		internal List<ERObject> Objects = new List<ERObject>();
		internal List<Link> links = new List<Link>();

        /// <summary>
        /// Gets the property grid objects that will be displayed to property grid
        /// </summary>
        internal object[] PropertyGridObjects
        {
            get
            {
                var list = new List<object>();
                if (toDeleteLink != null)
                    list.Add(toDeleteLink);
                foreach (var erObject in SelectedObjects)
                    list.Add(erObject);
                return list.ToArray();
            }
		}

		#region Private members
		/// <summary>
		/// The link that is currently selected, and is possible for deleting
		/// </summary>
		Link toDeleteLink = null;
		/// <summary>
		/// Previous mouse position. With this variable is computed the offset when the objects are moved
		/// </summary>
		Point lastSelectedObjectsPosition;

		/// <summary>
		/// Mouse offset when moving objects. Is set in MouseDown like mouse position when moving selected objects.
		/// When the MouseUp event is fired, the variable is set to null
		/// </summary>
		Point? mouseMovingObjectOffset;


		/// <summary>
		/// Indicates when the Control key is pressed or not
		/// </summary>
		bool controlPressed = false;

		/// <summary>
		/// The first link object that want to link with another object.
		/// </summary>
		ERObject firstLinkObject = null;

		/// <summary>
		/// Contains the new object that is wish to be added to editor
		/// </summary>
		ERObject newObject = null;

		/// <summary>
		/// The selecting rectangle that is drawed when the user wish to select multiple objects.
		/// Is null when  the user didn't select any objects
		/// </summary>
		Rectangle? selectedRectangle;

		/// <summary>
		/// Undo, Redo container for the editor
		/// </summary>
		Operations operations;
		#endregion

		#region Events
		public event EventHandler OnCanNotLinkObjects;
		public event Action<object, CancelEventsArgs> OnDeleteSelectedObjects;
		
		/// <summary>
		/// The event that is called when a user wish to delete selected link
		/// </summary>
		public event EventHandler<CancelEventsArgs> OnDeleteSelectedLink;

		/// <summary>
		/// Event called when the editor mode is changed
		/// </summary>
		public event EventHandler OnModeChanged;

		/// <summary>
		/// Handler called when the document is modified
		/// </summary>
		public event EventHandler OnDocumentModified;
		
		/// <summary>
		/// Occurs when the button asociated to this event must changed the enabled functionality
		/// </summary>
		public event EventHandler<OperationEventArgs> OnUndoChanged;
		public event EventHandler<OperationEventArgs> OnRedoChanged;
		#endregion

        #region Properties
        private DiagramEditorMode editMode = DiagramEditorMode.Selecting;
		public DiagramEditorMode EditMode
		{
			get
			{
				return editMode;
			}
			set
			{
				editMode = value;
				if (OnModeChanged != null)
					OnModeChanged(this, EventArgs.Empty);
			}
		}


        public void ModifyDocument()
        {
            isSaved = false;
			if (OnDocumentModified != null)
				OnDocumentModified(this, EventArgs.Empty);
        }

        bool isSaved = false;
		/// <summary>
		/// Get when the document is modified, and must be saved when user close the editor
		/// </summary>
		public bool IsModified
		{
			get
			{
				return isSaved ? false : operations.CanUndo;
			}
        }

		/// <summary>
		/// Get current document name, or null if is a new document
		/// <see cref="documentFullPathName"/>
		/// </summary>
		public string DocumentFullPath
		{
			get;
			private set;
		}
        #endregion

        public DiagramEditor()
		{
			InitializeComponent();
			operations = new Operations(this);
			SetStyle(ControlStyles.Opaque | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
		}

		#region Document functions
		public void NewDocument()
		{
			Objects.Clear();
			SelectedObjects.Clear();
			links.Clear();
			firstLinkObject = null;
			newObject = null;
            isSaved = true;
			EditMode = DiagramEditorMode.Selecting;
			addedAttribute = null;
            operations.Clear();
            isSaved = true;
			Invalidate();
			DocumentFullPath = null;
		}

		/// <summary>
		/// Load a document from specified stream, with specified formatter, and the full document path
		/// </summary>
		public void Load(IFormatter formatter, Stream stream, string documentFullPathName)
		{
			NewDocument();
			Objects = formatter.Deserialize(stream) as List<ERObject>;

			foreach (var obj in Objects)
			{
				obj.EditorControl = this;
				foreach (var link in obj.adjacentLinks)
					if (!links.Contains(link))
						links.Add(link);
			}
			DocumentFullPath = documentFullPathName;
			Invalidate();
		}

		/// <summary>
		/// Save new document using formatter into stream
		/// </summary>
		public void Save(IFormatter formatter, Stream stream, string documentFullPathName)
		{
			formatter.Serialize(stream, Objects);
			isSaved = true;
			DocumentFullPath = documentFullPathName;
		}

		/// <summary>
		/// Save existing open document into stream using specified formatter
		/// </summary>
		/// <exception cref="ArgumentException">If there is no document specified</exception>
		public void SaveAs(IFormatter formatter, Stream stream)
		{
			if (DocumentFullPath == null)
				throw new ArgumentException("There is no document saved");

			formatter.Serialize(stream, Objects);
			isSaved = true;
		}
		#endregion

		#region ER objects and link functions
		public bool Add(ERObject erObject, bool addToOperations)
		{
			Objects.Add(erObject);

			if (addToOperations)
				operations.Add(erObject);
			else
				Invalidate();

            ModifyDocument();
			return true;
		}
		/// <summary>
		/// Put new object to editor control
		/// </summary>
		/// <param name="obj"></param>
		public void AppendNewObject(ERObject obj)
		{
			newObject = obj;
            ModifyDocument();
		}

		internal void DeleteObjects(List<ERObject> objects)
		{
			foreach (var li in objects)
				foreach (var link in li.adjacentLinks)
					link.GetOtherEnd(li).adjacentLinks.Remove(link);
			objects.ForEach(DeleteObject);

			foreach (var obj in Objects)
			{
				var ent = obj as Entity;
				if (ent != null)
					ent.UpdateStrength();
			}
			ModifyDocument();
			Invalidate();
		}

		internal void DeleteObject(ERObject erObject)
		{
			SelectedObjects.Remove(erObject);
			Objects.Remove(erObject);
			links.RemoveAll(link => link.first == erObject || link.second == erObject);
			foreach (var link in erObject.adjacentLinks)
				if (link.first == erObject && link.second is ERObjects.Attribute)
					DeleteObject(link.second);
		}
		internal void AddLink(Link link)
		{
			links.Add(link);
			operations.CreateLink(link);
		}
		#endregion

		protected override void OnPaint(PaintEventArgs pe)
		{
			base.OnPaint(pe);

			var g = pe.Graphics;
			g.SmoothingMode = SmoothingMode.HighQuality;
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;
			g.PixelOffsetMode = PixelOffsetMode.HighQuality;

			g.Clear(BackColor);

			if (firstLinkObject != null)
				using (var pen = new Pen(Color.Gray, 2.0f))
					g.DrawLine(pen, firstLinkObject.GetCenter(), PointToClient(Cursor.Position));


			Objects.ForEach(li => li.Paint(g));
			using (var pen = new Pen(Color.Black))
				SelectedObjects.ForEach(li => g.DrawRectangles(pen, li.BoundingRectangle.GetSelectionHandles()));

			using (var pen = new Pen(Color.Black))
			{
				if (selectedRectangle.HasValue)
					g.DrawRectangle(pen, selectedRectangle.Value);
				foreach (var link in links)
					if (link.first.Visible && link.second.Visible)
						link.Paint(g);
				if (newObject != null)
					newObject.DrawBorderShape(g);
			}
		}

		private ERObject GetUnderlyingObject(Point p)
		{
			foreach (var li in Objects)
				if (li.Visible && li.Intersects(p))
					return li;
			return null;
		}

		private void Meniu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			if (e.ClickedItem == mnuItemRename)
			{
				var attribute = rightClickObj as ERObjects.Attribute;
				if (attribute != null)
				{
					using (var form = new AttributeForm())
					{
						form.AttributeName = attribute.Name;
						form.Primary = attribute.PrimaryKey;
                        form.CanBeNull = attribute.CanBeNull;
                        form.AutoIncrement = attribute.AutoIncrement;
						form.Type = attribute.Type;
						form.ValueType = attribute.AttributeValueType;
                        
                        var oldValues = new object[] { attribute.Name, attribute.PrimaryKey, attribute.CanBeNull, attribute.AutoIncrement, attribute.Type, attribute.AttributeValueType.Clone() };
						if (form.ShowDialog() == DialogResult.OK)
						{
                            attribute.Name = form.AttributeName;
							attribute.PrimaryKey = form.Primary;
                            attribute.CanBeNull = form.CanBeNull;
                            attribute.AutoIncrement = form.AutoIncrement;
							attribute.Type = form.Type;
							attribute.AttributeValueType = form.ValueType;
							Invalidate();

                            var newValues = new object[] { attribute.Name.Clone(), attribute.PrimaryKey, attribute.CanBeNull, attribute.AutoIncrement, attribute.Type, attribute.AttributeValueType.Clone() };
                            operations.ChangeAttribute(attribute, oldValues, newValues);
                            ModifyDocument();
						}
					}
					return;
				}

				var entity = rightClickObj as Entity;
				if (entity != null)
				{
					using (var form = new EntityForm())
					{
						form.EntityName = entity.Name;

                        var oldValues = new object[] { entity.Name, entity.Type };
						if (form.ShowDialog() == DialogResult.OK)
						{
                            entity.Name = form.EntityName;
                            Invalidate();

                            operations.ChangeEntity(entity, oldValues, new object[] { entity.Name.Clone(), entity.Type });
                            ModifyDocument();
						}
					}
					return;
				}

				var relationship = rightClickObj as Relationship;
				if (relationship != null)
				{
					using (var form = new RelationshipForm(relationship))
					{
                        form.RelationshipName = relationship.Name;
                        form.Identifying = relationship.Identifying;

                        var initialMultiplicity = form.MultiplicityChanges;
                        var oldValues = new object[] { relationship.Name, relationship.Identifying };
						if (form.ShowDialog() == DialogResult.OK)
						{
							relationship.Name = form.RelationshipName;
							relationship.Identifying = form.Identifying;
                            Invalidate();

                            operations.ChangeRelationship(relationship, oldValues, new object[] { relationship.Name.Clone(), relationship.Identifying }, 
                                initialMultiplicity, form.MultiplicityChanges);
                            ModifyDocument();
						}
					}
					return;
				}
			}
		}

		/// <summary>
		/// Invoke the undo event on the editor
		/// </summary>
		/// <param name="enabled"></param>
		/// <param name="operationName">Operation name that will be performed when the undo is pressed.</param>
		internal void InvokeUndoEvent(bool enabled, string operationName)
		{
			if (OnUndoChanged != null)
				OnUndoChanged(this, new OperationEventArgs(enabled, operationName));
		}
		/// <summary>
		/// Invoke the redo event on the editor
		/// </summary>
		/// <param name="enabled"></param>
		/// <param name="operationName">Operation name that will be performed when the redo is pressed.</param>
		internal void InvokeRedoEvent(bool enabled, string operationName)
		{
			if (OnRedoChanged != null)
				OnRedoChanged(this, new OperationEventArgs(enabled, operationName));
		}

		public void Undo()
		{
			operations.InvokeUndo();
		}

		public void Redo()
		{
			operations.InvokeRedo();
		}

		public void SelectAll()
		{
			if (firstLinkObject == null)
			{
				SelectedObjects.Clear();
				SelectedObjects.AddRange(Objects);
				Invalidate();
				OnSelectionChange(null);
			}
		}

		public event EventHandler SelectionChange;

		public virtual void OnSelectionChange(EventArgs e)
		{
			if (SelectionChange != null)
				SelectionChange(this, e);
		}
	}
}
