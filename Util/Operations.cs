using System;
using System.Collections.Generic;
using System.Text;
using EerieEdit.ERObjects;
using System.Drawing;
using System.Diagnostics;

namespace EerieEdit.Util
{
    /// <summary>
    /// Provide operations linke undo or redo for all operations in the DiagramEditor
    /// </summary>
    public class Operations
    {
        DiagramEditor editor;
        int operationIndex = -1;
        List<Operation> history = new List<Operation>();

        public Operations(DiagramEditor editor)
        {
            this.editor = editor;
        }

        // Empties the redo list
        // OperationIndex is the last added operation
        internal void Truncate()
        {
            if (CanRedo)
            {
                history.RemoveRange(OperationIndex + 1, history.Count - 1 - OperationIndex);
                OperationIndex = OperationIndex;
            }
        }

        public bool CanUndo
        {
            get
            {
                return OperationIndex >= 0 && OperationIndex < history.Count;
            }
        }
        public bool CanRedo
        {
            get
            {
                return OperationIndex < history.Count - 1;
            }
        }
        /// <summary>
        /// The current operation index in the history operation list
        /// </summary>
        public int OperationIndex
        {
            get { return operationIndex; }
            private set
            {
                operationIndex = value;
                editor.InvokeUndoEvent(CanUndo, CanUndo ? history[operationIndex].Name : null);
                editor.InvokeRedoEvent(CanRedo, CanRedo ? history[operationIndex + 1].Name : null);
            }
        }

		/// <summary>
		/// Clear the operation list history
		/// </summary>
        public void Clear()
        {
            history.Clear();
            OperationIndex = -1;
        }

        /// <summary>
        /// This function is called when undo operation must be performed
        /// </summary>
        internal void InvokeUndo()
        {
            Debug.Assert(history.Count > 0 && OperationIndex < history.Count && OperationIndex >= 0);
            history[OperationIndex--].Undo();
        }

        /// <summary>
        /// Function called when a Redo operation must be performed
        /// </summary>
        internal void InvokeRedo()
        {
            Debug.Assert(history.Count > 0 && OperationIndex < history.Count);
            history[++OperationIndex].Redo();
        }

        void Add(Operation operation)
        {
            //System.Media.SystemSounds.Beep.Play();
            Truncate();
            history.Add(operation);
            OperationIndex++;
        }

        /// <summary>
        /// Post a Add operation in the history list
        /// </summary>
        public void Add(ERObject obj)
        {
            Add(new Operation("add '" + obj.Name + "'")
            {
                Undo = () =>
                {
                    editor.DeleteObject(obj);
                    editor.Invalidate(obj.DrawingRectangle);
                },
                Redo = () =>
                {
                    editor.Add(obj, false);
                    editor.Invalidate(obj.DrawingRectangle);
                }
            });
        }

        public void Delete(ERObject[] objects)
        {
            Add(new Operation(string.Format("delete {0}", objects.Length == 1 ? "object '" + objects[0].Name + "'" : objects.Length + " objects"))
            {
                Undo = () =>
                {
                    // Link the existing objects that were linked with the deleted objects
                    foreach (var erObject in objects)
                        foreach (var link in erObject.adjacentLinks)
                        {
                            link.GetOtherEnd(erObject).adjacentLinks.Add(link);
                            if (!editor.links.Contains(link))
                                editor.links.Add(link);

                            if (link.first == erObject && link.second is ERObjects.Attribute)
                                editor.Add(link.second, false);
                        }
                    // Add objects deleted
                    editor.Objects.AddRange(objects);
                    editor.SelectedObjects.AddRange(objects);
					foreach (var obj in editor.Objects)
					{
						var ent = obj as Entity;
						if (ent != null)
							ent.UpdateStrength();
					}
                    editor.Invalidate();
                },
                Redo = () => editor.DeleteObjects(new List<ERObject>(objects))
            });
        }

        public void Move(ERObject[] objects, int xoffset, int yoffset)
        {
			Add(new Operation(string.Format("move {0}", objects.Length == 1 ? "object '" + objects[0].Name + "'" : objects.Length + " objects"))
			{
				Undo = () =>
				{	
					foreach (var obj in objects)
						obj.Offset(-xoffset, -yoffset);
                    editor.Invalidate();
  				},
				Redo = () =>
				{
					foreach (var obj in objects)
						obj.Offset(xoffset, yoffset);
                    editor.Invalidate();
				}
			});
		}

        public void PropertiesAttribute(ERObjects.Attribute erAttrinute)
        {
        }

        void deleteLink(Link link)
        {
            editor.links.Remove(link);
            link.first.adjacentLinks.Remove(link);
            link.second.adjacentLinks.Remove(link);
            link.first.ComputeBounds();
            link.second.ComputeBounds();
            editor.Invalidate();
        }
        void addLink(Link link)
        {
            editor.links.Add(link);
            link.first.adjacentLinks.Add(link);
            link.second.adjacentLinks.Add(link);
            link.first.ComputeBounds();
            link.second.ComputeBounds();
            editor.Invalidate();
        }
        public void CreateLink(Link link)
        {
            Add(new Operation("create link")
            {
                Undo = () => deleteLink(link),
                Redo = () => addLink(link)
            });
        }
        public void DeleteLink(Link link)
        {
            Add(new Operation("delete link")
            {
                Undo = () => addLink(link),
                Redo = () => deleteLink(link)
            });
        }

        internal void ChangeAttribute(ERObjects.Attribute attribute, object[] oldProperties, object[] newProperties)
        {
            Add(new Operation("modify '" + attribute.Name + '\'')
            {
                Undo = () =>
                {
                    attribute.PrimaryKey = (bool)oldProperties[1];
                    attribute.CanBeNull = (bool)oldProperties[2];
                    attribute.AutoIncrement = (bool)oldProperties[3];
                    attribute.Type = (AttributeType)oldProperties[4];
                    attribute.AttributeValueType = (string)oldProperties[5];
                    attribute.Name = (string)oldProperties[0];
                    editor.Invalidate();
                },
                Redo = () =>
                {
                    attribute.PrimaryKey = (bool)newProperties[1];
                    attribute.CanBeNull = (bool)newProperties[2];
                    attribute.AutoIncrement = (bool)newProperties[3];
                    attribute.Type = (AttributeType)newProperties[4];
                    attribute.AttributeValueType = (string)newProperties[4];
                    attribute.Name = (string)newProperties[0];
                    editor.Invalidate();
                }
            });
        }

        internal void ChangeEntity(Entity entity, object[] oldProperties, object[] newProperties)
        {
            Add(new Operation("modify '" + entity.Name + '\'')
            {
                Undo = () =>
                {
                    entity.Type = (EntityType)oldProperties[1];
                    entity.Name = (string)oldProperties[0];
                    editor.Invalidate();
                },
                Redo = () =>
                {
                    entity.Type = (EntityType)newProperties[1];
                    entity.Name = (string)newProperties[0];
                    editor.Invalidate();
                }
            });
        }


        internal void ChangeRelationship(Relationship relationship, object[] oldProperties, object[] newProperties, 
            Dictionary<Link, Multiplicity> initialMultiplicity, Dictionary<Link, Multiplicity> modifiedMultiplicity)
        {
            Add(new Operation("modify '" + relationship.Name + '\'')
            {
                Undo = () =>
                {
                    relationship.Identifying = (bool)oldProperties[1];
                    relationship.Name = (string)oldProperties[0];
                    foreach (var kvp in initialMultiplicity)
                        kvp.Key.Multiplicity = kvp.Value;
                    editor.Invalidate();
                },
                Redo = () =>
                {
                    relationship.Identifying = (bool)newProperties[1];
                    relationship.Name = (string)newProperties[0];
                    foreach (var kvp in modifiedMultiplicity)
                        kvp.Key.Multiplicity = kvp.Value;
                    editor.Invalidate();
                }
            });
        }
    }


    public class OperationEventArgs : EventArgs
    {
        /// <summary>
        /// Get the status of the current operation
        /// </summary>
        public bool Enabled { get; set; }
        public string OperationName { get; set; }

        public OperationEventArgs(bool enable, string operationName)
        {
            Enabled = enable;
            this.OperationName = operationName;
        }
    }

    /// <summary>
    /// Represents a operation result, with a Undo and a Redo functions
    /// </summary>
    public class Operation
    {
        string name;
        public Operation() { }
        public Operation(string operationName)
        {
            this.name = operationName;
        }
        /// <summary>
        /// Represent the Undo action asociated with this action
        /// </summary>
        public Action Undo { get; set; }

        /// <summary>
        /// Represent the Redo action asociated with this action        
        /// </summary>
        public Action Redo { get; set; }

        /// <summary>
        /// Get the operation name.
        /// </summary>
        public string Name
        {
            get { return name; }
        }
    }
}
