using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using EerieEdit.ERObjects;
using EerieEdit.Forms;
using System.Collections.Generic;

namespace EerieEdit.Forms
{
	public partial class RelationshipForm : ValidatingForm
	{
		private Relationship relationship;
		private Dictionary<Link, Multiplicity> multiplicityChanges;

		public string RelationshipName
		{
			get
			{
				return txtName.Text;
			}

			set
			{
				txtName.Text = value;
			}
		}

		public bool Identifying
		{
			get
			{
				return cbIdentifying.Checked;
			}
			
			set
			{
				cbIdentifying.Checked = value;
			}
		}
        /// <summary>
        /// Get the current mutiplicity relation between entities and the current Relationship
        /// </summary>
        public Dictionary<Link, Multiplicity> MultiplicityChanges
        {
            get
            {
                return new Dictionary<Link,Multiplicity>(multiplicityChanges);
            }
        }

		public RelationshipForm(Relationship relationship)
		{
			InitializeComponent();

			multiplicityChanges = new Dictionary<Link, Multiplicity>();

			this.relationship = relationship;

			RelationshipName = relationship.Name;
			Identifying = relationship.Identifying;

            foreach (var entity in relationship.GetNeighbours<Entity>())
            {
                lstEntities.Items.Add(entity);
                var relatedLink = GetLink(entity);
                if(relatedLink != null)
                    multiplicityChanges.Add(relatedLink, relatedLink.Multiplicity);
            }
		}

		private void txtName_Validating(object sender, CancelEventArgs e)
		{
			if (e.Cancel = string.IsNullOrEmpty(txtName.Text))
				errorProvider.SetError(txtName, "Type an attribute name");
		}

		private void txtName_Validated(object sender, EventArgs e)
		{
			errorProvider.SetError(txtName, string.Empty);
		}

		private void RelationshipForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			e.Cancel = false;

			if (DialogResult == DialogResult.OK)
				foreach (var pair in multiplicityChanges)
					pair.Key.Multiplicity = pair.Value;
		}

        private void btnMultiplicity_Click(object sender, EventArgs e)
        {
            if (Width == 270)
            {
                Width = 480;
                btnMultiplicity.Text = "&Multiplicity <<";
            }
            else
            {
                Width = 270;
                btnMultiplicity.Text = "&Multiplicity >>";
            }
        }

		private void lstEntities_SelectedValueChanged(object sender, EventArgs e)
		{
			var entity = lstEntities.SelectedItem as Entity;
			if (entity == null)
				return;

			var multiplicity = multiplicityChanges[GetLink(entity)];
            rbOne.Checked = multiplicity == Multiplicity.One;
            rbMany.Checked = multiplicity == Multiplicity.Many;
		}

		private void rbOneOrMany_Click(object sender, EventArgs e)
		{
			var entity = lstEntities.SelectedItem as Entity;
			if (entity == null)
				return;

			var link = GetLink(entity);
			//multiplicityChanges.Remove(link);
            multiplicityChanges[link] = rbOne.Checked ? Multiplicity.One : Multiplicity.Many;
		}

		private Link GetLink(Entity entity)
		{
			return entity.adjacentLinks.Find(l => l.first == relationship || l.second == relationship);
		}
	}
}
