using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using EerieEdit.ERObjects;
using EerieEdit.Forms;

namespace EerieEdit.Forms
{
	public partial class EntityForm : ValidatingForm
	{
		public string EntityName
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

		public EntityForm()
		{
			InitializeComponent();
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
	}
}
