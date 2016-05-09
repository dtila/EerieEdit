using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using EerieEdit.ERObjects;
using System.Diagnostics;

namespace EerieEdit
{
	public partial class DiagramEditor
	{
		/// <summary>
		/// Obiectul pe care s-a dat click dreapta, folosit pentru modificarea proprietatilor
		/// </summary>
		public ERObject rightClickObj;

		private bool dragged;
		/// <summary>
		/// Atributul care doresc sa-l adaug acum, este identic cu newObjects
		/// </summary>
		public ERObjects.Attribute addedAttribute;

		protected bool testLink(ERObject obj, ERObject final)
		{
			foreach (Link link in obj.adjacentLinks)
			{
				if (link.second == obj)
					if (link.first == final)
						return false;
					else
						return testLink(link.first, final);
			}
			return true;
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
            if (selectedRectangle.HasValue)
			{
				int x1 = Math.Min(lastSelectedObjectsPosition.X, e.X), x2 = Math.Max(lastSelectedObjectsPosition.X, e.X);
				int y1 = Math.Min(lastSelectedObjectsPosition.Y, e.Y), y2 = Math.Max(lastSelectedObjectsPosition.Y, e.Y);

				Invalidate(Rectangle.Inflate(selectedRectangle.Value, 1, 1)); ///Invalideaza dreptunghiul vechi
				selectedRectangle = Rectangle.FromLTRB(x1, y1, x2, y2);
				/// Check for new selected objects
				SelectedObjects.ForEach(li => Invalidate(li.DrawingRectangle));///Invalidez obiectele vechi selectate
				SelectedObjects.Clear();
				foreach (var li in Objects)
					if (li.Visible && li.Intersects(selectedRectangle.Value))
					{
						SelectedObjects.Add(li);
						Invalidate(li.BoundingRectangle);
					}
				Invalidate(Rectangle.Inflate(selectedRectangle.Value, 1, 1));
			}
			else
			{
				if ((e.Button & MouseButtons.Left) != 0 && firstLinkObject == null)
				{
					dragged = true;

					foreach (var li in SelectedObjects)
						li.Offset(e.X - lastSelectedObjectsPosition.X, e.Y - lastSelectedObjectsPosition.Y);
                    if (SelectedObjects.Count == 0)
                        mouseMovingObjectOffset = null;

                    ModifyDocument();
					Invalidate();
				}
				lastSelectedObjectsPosition = e.Location;
			}

			if (newObject != null)
			{
				Invalidate(Rectangle.Inflate(newObject.BoundingRectangle, 1, 1));
				newObject.SetLocation(e.X - newObject.BoundingRectangle.Width / 2, e.Y - newObject.BoundingRectangle.Height / 2);
				var br = newObject.BoundingRectangle;
				Invalidate(Rectangle.Inflate(newObject.BoundingRectangle, 1, 1));
			}

			if (firstLinkObject != null)
			{
				Cursor = Cursors.Cross;
				var center = firstLinkObject.GetCenter();
				Invalidate();
				//var cursor = PointToClient(Cursor.Position);
				//Invalidate(Rectangle.FromLTRB(Math.Min(center.X, cursor.X), Math.Min(center.Y, cursor.Y), Math.Max(center.X, cursor.X) + 1, Math.Max(center.Y, cursor.Y) + 1));
			}
			else
				Cursor = GetUnderlyingObject(e.Location) != null ? (EditMode != DiagramEditorMode.Selecting ? Cursors.Cross : Cursors.SizeAll) : Cursors.Default;
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			var underlyingObject = GetUnderlyingObject(e.Location);

			if ((e.Button & MouseButtons.Left) != 0)
			{
				dragged = false;
				if (EditMode == DiagramEditorMode.Selecting)
				{
					if (underlyingObject == null)
					{
						Cursor.Clip = new Rectangle(PointToScreen(new Point()), Size);
						selectedRectangle = new Rectangle(e.Location, new Size());
						SelectedObjects.Clear();
						EditMode = DiagramEditorMode.Selecting;
					}
					else
					{
                        selectedRectangle = null;

						if (!controlPressed && !SelectedObjects.Contains(underlyingObject))
							SelectedObjects.Clear();

                        foreach (var li in Objects)
                            if (li.Visible)
                                if (li.Intersects(e.Location))
                                    if (!SelectedObjects.Contains(li))
                                    {
                                        SelectedObjects.Add(li);
                                        if (!controlPressed)
                                            break;
                                    }
                                    else
                                        if (controlPressed) 
											SelectedObjects.Remove(li);

						/// Clip mouse
						if (SelectedObjects.Count > 0)
						{
							var rect = SelectedObjects[0].BoundingRectangle;
							SelectedObjects.ForEach(li => rect = Rectangle.Union(rect, Rectangle.Inflate(li.BoundingRectangle, ExtensionMethods.halfSelectedRectangle, ExtensionMethods.halfSelectedRectangle)));
							const int distance = ExtensionMethods.halfSelectedRectangle + 1;
							var ps = PointToScreen(new Point(e.X - rect.Left + distance, e.Y - rect.Top + distance));
							Cursor.Clip = new Rectangle(ps.X, ps.Y, Size.Width - rect.Width - distance, Size.Height - rect.Height - distance);

                            mouseMovingObjectOffset = e.Location;
						}
					}

					/// Check for clicked link
					if (toDeleteLink != null)
					{
						toDeleteLink.Selected = false;
						toDeleteLink = null;
					}
					foreach (var li in links)
						if (li.ContainsPoint(e.Location) && !(li.second is EerieEdit.ERObjects.Attribute))
						{
							toDeleteLink = li;
							li.Selected = true;
							break;
						}

					Invalidate();
				}
				else
				//if(EditMode != DiagramEditorMode.Selecting)
				{
					if (addedAttribute != null && underlyingObject != null && addedAttribute != underlyingObject)
					{
						links.Add(new Link(underlyingObject, firstLinkObject, EditMode == DiagramEditorMode.CreateIerarhicalLink));

						addedAttribute.ParentObject = underlyingObject;
						addedAttribute.Visible = underlyingObject.ChildrenVisible;
						addedAttribute = null;
						firstLinkObject = null;
						EditMode = DiagramEditorMode.Selecting;
						Cursor = Cursors.Default;
					}
					else
					{
						if (underlyingObject != null) /// Deasupra unui obiect
							firstLinkObject = underlyingObject;
					}
				}

				if (underlyingObject == null)
					SelectedObjects.Clear();

				lastSelectedObjectsPosition = e.Location;
				if (newObject != null)
				{
					Add(newObject, true);

					var attribute = newObject as ERObjects.Attribute;
					if (attribute != null)
					{
						EditMode = DiagramEditorMode.CreateSimpleLink;
						firstLinkObject = attribute;
						addedAttribute = attribute;
                        selectedRectangle = null;
					}

					newObject = null;
				}
			}

			OnSelectionChange(null);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);

			var underlyingObject = GetUnderlyingObject(e.Location);
            if (e.Button == MouseButtons.Right)
            {
                if (underlyingObject != null)
                {
                    SelectedObjects.Clear();
                    rightClickObj = underlyingObject;
                    SelectedObjects.Add(underlyingObject);
                    menuObjects.Show(this, e.Location);
                }
                if (toDeleteLink != null)
                {

                }
            }
			Cursor.Clip = Screen.PrimaryScreen.Bounds;

			if (firstLinkObject != null && !(firstLinkObject is ERObjects.Attribute))
			{
				if (underlyingObject != null && firstLinkObject != underlyingObject)
				{
					bool entity_Relationship = firstLinkObject is Entity && underlyingObject is Relationship || firstLinkObject is Relationship && underlyingObject is Entity;
					bool attribute_Entity = firstLinkObject is ERObjects.Attribute && underlyingObject is Entity || firstLinkObject is ERObjects.Entity && underlyingObject is ERObjects.Attribute;
					bool entity_entity = firstLinkObject is ERObjects.Entity && underlyingObject is Entity;
					bool attribute_Relationship = firstLinkObject is ERObjects.Attribute && underlyingObject is Relationship || firstLinkObject is Relationship && underlyingObject is ERObjects.Attribute;
					bool attribute_attribute = firstLinkObject is ERObjects.Attribute && underlyingObject is ERObjects.Attribute;

					if (EditMode == DiagramEditorMode.CreateIerarhicalLink)
					{
						if (entity_entity)
						{
							//legaturile ierarhice
							bool ok = true;

							foreach (Link link in links)
								if (underlyingObject == link.second && link.first == firstLinkObject)
								{ // daca obiectele nu au fost legate in celalalt sens 
									ok = false;
									break;
								}
							if (!ok)
								OnCanNotLinkObjects(this, EventArgs.Empty);
							else
							{
								//verificarea grafului
								if (testLink(underlyingObject, firstLinkObject))
									AddLink(new Link(underlyingObject, firstLinkObject, true));
								else
									OnCanNotLinkObjects(this, EventArgs.Empty);
							}

						}
						else
							OnCanNotLinkObjects(this, EventArgs.Empty);
					}
					else
					{
						if (entity_Relationship)
						{
							AddLink(new Link(underlyingObject, firstLinkObject, EditMode == DiagramEditorMode.CreateIerarhicalLink));
							if (addedAttribute != null)
							{
								addedAttribute.ParentObject = underlyingObject;
								addedAttribute = null;
								firstLinkObject = null;
								EditMode = DiagramEditorMode.Selecting;
								Cursor = Cursors.Default;
							}
						}
						else
							if (attribute_Relationship || attribute_Entity)
							{
								var linkAttribute = firstLinkObject is ERObjects.Attribute ? firstLinkObject : underlyingObject;
								ERObject linkRelationshipOrEntity = null;
								if (attribute_Relationship)
									linkRelationshipOrEntity = firstLinkObject is ERObjects.Relationship ? firstLinkObject : underlyingObject;
								else
									linkRelationshipOrEntity = firstLinkObject is ERObjects.Entity ? firstLinkObject : underlyingObject;

								var listToRemove = new List<List<Link>>();
								listToRemove.Add(links);
								foreach (var item in Objects)
									listToRemove.Add(item.adjacentLinks);

								foreach (var list in listToRemove)
									for (int i = list.Count - 1; i >= 0; i--)
										if (list[i].first == linkAttribute.ParentObject && list[i].second == linkAttribute)
											list.RemoveAt(i);


								// Adaug la atribut, legatura noua cu noua Entitate
								AddLink(new Link(linkRelationshipOrEntity, linkAttribute, EditMode == DiagramEditorMode.CreateIerarhicalLink));
								linkAttribute.ParentObject = linkRelationshipOrEntity;

								EditMode = DiagramEditorMode.Selecting;

								Invalidate();
							}

							else
								if (attribute_attribute)
								{
									//Verific daca atributul care vreau sa-l pun ca printe (underlyingObject are parinte setat, si copii sai
									var currentParent = underlyingObject;
									bool attributeHasValidParent = false;
									while ((currentParent = currentParent.ParentObject) != null && currentParent != firstLinkObject)
										if (!(currentParent is ERObjects.Attribute) && underlyingObject.ParentObject != firstLinkObject)
											attributeHasValidParent = true;

									if (attributeHasValidParent)
									{
										var listLinksToCheck = new List<Link>[]
										{
											links,
											firstLinkObject.adjacentLinks,
											firstLinkObject.ParentObject.adjacentLinks,
										};

										foreach (var list in listLinksToCheck)
											for (int i = list.Count - 1; i >= 0; i--)
												if (list[i].first == firstLinkObject && list[i].second == firstLinkObject.ParentObject ||
													list[i].second == firstLinkObject && list[i].first == firstLinkObject.ParentObject)
													list.RemoveAt(i);

										firstLinkObject.ParentObject = underlyingObject;
										AddLink(new Link(underlyingObject, firstLinkObject, EditMode == DiagramEditorMode.CreateIerarhicalLink));
										EditMode = DiagramEditorMode.Selecting;
										Invalidate();
									}
									else
										OnCanNotLinkObjects(this, EventArgs.Empty);
								}
								else
									OnCanNotLinkObjects(this, EventArgs.Empty);
					}
				}
			}
            else
                if (!selectedRectangle.HasValue && SelectedObjects.Count > 0)
                {
                    if (mouseMovingObjectOffset.HasValue && (mouseMovingObjectOffset.Value.X != e.X || mouseMovingObjectOffset.Value.Y != e.Y))
                    {
                        operations.Move(SelectedObjects.ToArray(), e.X - mouseMovingObjectOffset.Value.X, e.Y - mouseMovingObjectOffset.Value.Y);
                        mouseMovingObjectOffset = null;
                    }
                }

			lastSelectedObjectsPosition = e.Location;

			if ((e.Button & MouseButtons.Left) != 0)
                if (!controlPressed && !dragged && !selectedRectangle.HasValue && underlyingObject != null && addedAttribute == null)
				{
					SelectedObjects.Clear();
					SelectedObjects.Add(underlyingObject);
				}


            selectedRectangle = null;
			if (addedAttribute == null)
				firstLinkObject = null;
			Invalidate();

			OnSelectionChange(null);
		}

		private void DiagramEditor_DoubleClick(object sender, EventArgs e)
		{
			var me = e as MouseEventArgs;
			if (me != null && me.Button == MouseButtons.Left && SelectedObjects.Count > 0)
				SelectedObjects[0].ToggleAttributeVisibility();
		}
	}
}
