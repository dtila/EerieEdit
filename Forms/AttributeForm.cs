using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using EerieEdit.ERObjects;
using EerieEdit.Forms;
using System.Collections;

namespace EerieEdit.Forms
{
	public partial class AttributeForm : ValidatingForm
	{
		public string AttributeName
		{
			get	{ return txtName.Text;	}
			set	{ txtName.Text = value; }
		}
		public bool Primary
		{
			get { return cbPrimary.Checked;	}
			set	{ cbPrimary.Checked = value; }
		}

        public bool CanBeNull
        {
            get { return cbNull.Checked; }
            set { cbNull.Checked = value; }
        }
        public bool AutoIncrement
        {
            get { return cbAutoIncrement.Checked; }
            set { cbAutoIncrement.Checked = value; }
        }

		public AttributeType Type
		{
			get
			{
				if (rbSimple.Checked)
					return AttributeType.Simple;
				if (rbMultivalued.Checked)
					return AttributeType.Multivalued;
				return AttributeType.Derived;
			}

			set
			{
				rbSimple.Checked = value == AttributeType.Simple;
				rbMultivalued.Checked = value == AttributeType.Multivalued;
				rbDerived.Checked = value == AttributeType.Derived;
			}
		}

        public string ValueType
        {
            get
            {
                return boxAttributeValueType.Text;// == null ? string.Empty : (string)boxAttributeValueType.SelectedValue;
            }
            set
            {
                boxAttributeValueType.Text = value.ToLower();
            }
        }

		public AttributeForm()
		{
			InitializeComponent();

            var attributes = new string[] 
            {
                "bigint",
                "binary(50)",
                "binary(max)",
                "bit",
                "char",
                "datetime",
                "float",
                "image",
                "int",
                "money",
                "nchar(10)",
                "nvarchar(10)",
                "nvarchar(max)",
                "real",
                "smalldatetime",
                "smallmoney",
                "smallint"
            };

            foreach (var item in attributes)
                boxAttributeValueType.Items.Add(item);
		}

		private void txtLenght_Validating(object sender, CancelEventArgs e)
		{
			if (e.Cancel = string.IsNullOrEmpty(txtName.Text))
				errorProvider.SetError(txtName, "Type an attribute name");
		}

		private void controlValidated_Validated(object sender, EventArgs e)
		{
			errorProvider.SetError((sender as Control), string.Empty);
		}

        private void cbPrimary_CheckedChanged(object sender, EventArgs e)
        {
            if (!cbPrimary.Checked)
                cbAutoIncrement.Checked = false;
            else
                cbNull.Checked = false;

            cbNull.Enabled = !cbPrimary.Checked;
            cbAutoIncrement.Enabled = cbPrimary.Checked;
        }
	}
}
