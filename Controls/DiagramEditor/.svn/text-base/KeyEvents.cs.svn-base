using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace EerieEdit
{
    public partial class DiagramEditor
    {
        protected override void OnKeyDown(KeyEventArgs e)
        {
			switch (e.KeyCode)
			{
				case Keys.Delete:
                    if (toDeleteLink != null)
                    {
                        var eventArgs = new global::EerieEdit.CancelEventsArgs(true);
                        if(OnDeleteSelectedLink != null)
                            OnDeleteSelectedLink(this, eventArgs);
                        if (!eventArgs.Cancel)
                        {
                            links.Remove(toDeleteLink);
                            toDeleteLink.first.adjacentLinks.Remove(toDeleteLink);
                            toDeleteLink.second.adjacentLinks.Remove(toDeleteLink);

                            operations.DeleteLink(toDeleteLink);

                            toDeleteLink = null;
                            ModifyDocument();
                            Invalidate();

                        }
                    }
                    else
                        if (OnDeleteSelectedObjects != null)
					    {
						    var eventArgs = new global::EerieEdit.CancelEventsArgs(true);
						    OnDeleteSelectedObjects(this, eventArgs);
                            if (!eventArgs.Cancel)
                            {
                                operations.Delete(SelectedObjects.ToArray());
                                DeleteObjects(SelectedObjects);
                            }
					    }
					break;
				case Keys.ControlKey:
					controlPressed = e.Control;
					break;
				case Keys.Escape:
					if (addedAttribute != null)
					{
						Objects.Remove(addedAttribute);
						addedAttribute = null;
						firstLinkObject = null;
						EditMode = DiagramEditorMode.Selecting;
						Cursor = Cursors.Default;
						Invalidate();
					}
					break;
			}

            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
                controlPressed = e.Control;

            base.OnKeyUp(e);
        }
    }
}
