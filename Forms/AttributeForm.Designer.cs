namespace EerieEdit.Forms
{
    partial class AttributeForm
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
			this.txtName = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.cbPrimary = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.cbAutoIncrement = new System.Windows.Forms.CheckBox();
			this.cbNull = new System.Windows.Forms.CheckBox();
			this.boxAttributeValueType = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.rbDerived = new System.Windows.Forms.RadioButton();
			this.rbMultivalued = new System.Windows.Forms.RadioButton();
			this.rbSimple = new System.Windows.Forms.RadioButton();
			this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
			this.SuspendLayout();
			// 
			// txtName
			// 
			this.txtName.Location = new System.Drawing.Point(15, 35);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(219, 20);
			this.txtName.TabIndex = 0;
			this.txtName.Validated += new System.EventHandler(this.controlValidated_Validated);
			this.txtName.Validating += new System.ComponentModel.CancelEventHandler(this.txtLenght_Validating);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 19);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(149, 13);
			this.label1.TabIndex = 6;
			this.label1.Text = "Type the name of the attribute";
			// 
			// btnOk
			// 
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(67, 235);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 5;
			this.btnOk.Text = "OK";
			this.btnOk.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.CausesValidation = false;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(158, 235);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 6;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// cbPrimary
			// 
			this.cbPrimary.AutoSize = true;
			this.cbPrimary.CausesValidation = false;
			this.cbPrimary.Location = new System.Drawing.Point(121, 30);
			this.cbPrimary.Name = "cbPrimary";
			this.cbPrimary.Size = new System.Drawing.Size(60, 17);
			this.cbPrimary.TabIndex = 1;
			this.cbPrimary.Text = "Primary";
			this.cbPrimary.UseVisualStyleBackColor = true;
			this.cbPrimary.CheckedChanged += new System.EventHandler(this.cbPrimary_CheckedChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.CausesValidation = false;
			this.groupBox1.Controls.Add(this.cbAutoIncrement);
			this.groupBox1.Controls.Add(this.cbNull);
			this.groupBox1.Controls.Add(this.boxAttributeValueType);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.rbDerived);
			this.groupBox1.Controls.Add(this.rbMultivalued);
			this.groupBox1.Controls.Add(this.rbSimple);
			this.groupBox1.Controls.Add(this.cbPrimary);
			this.groupBox1.Location = new System.Drawing.Point(15, 66);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(219, 163);
			this.groupBox1.TabIndex = 9;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Attribute properties";
			// 
			// cbAutoIncrement
			// 
			this.cbAutoIncrement.AutoSize = true;
			this.cbAutoIncrement.CausesValidation = false;
			this.cbAutoIncrement.Enabled = false;
			this.cbAutoIncrement.Location = new System.Drawing.Point(121, 75);
			this.cbAutoIncrement.Name = "cbAutoIncrement";
			this.cbAutoIncrement.Size = new System.Drawing.Size(97, 17);
			this.cbAutoIncrement.TabIndex = 14;
			this.cbAutoIncrement.Text = "Auto increment";
			this.cbAutoIncrement.UseVisualStyleBackColor = true;
			// 
			// cbNull
			// 
			this.cbNull.AutoSize = true;
			this.cbNull.CausesValidation = false;
			this.cbNull.Location = new System.Drawing.Point(121, 53);
			this.cbNull.Name = "cbNull";
			this.cbNull.Size = new System.Drawing.Size(79, 17);
			this.cbNull.TabIndex = 13;
			this.cbNull.Text = "Can be null";
			this.cbNull.UseVisualStyleBackColor = true;
			// 
			// boxAttributeValueType
			// 
			this.boxAttributeValueType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.boxAttributeValueType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.boxAttributeValueType.DisplayMember = "Key";
			this.boxAttributeValueType.FormattingEnabled = true;
			this.boxAttributeValueType.Location = new System.Drawing.Point(29, 130);
			this.boxAttributeValueType.Name = "boxAttributeValueType";
			this.boxAttributeValueType.Size = new System.Drawing.Size(126, 21);
			this.boxAttributeValueType.TabIndex = 11;
			this.boxAttributeValueType.ValueMember = "Value";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(16, 114);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(139, 13);
			this.label2.TabIndex = 12;
			this.label2.Text = "Choose attribute value type:";
			// 
			// rbDerived
			// 
			this.rbDerived.AutoSize = true;
			this.rbDerived.CausesValidation = false;
			this.rbDerived.Location = new System.Drawing.Point(19, 75);
			this.rbDerived.Name = "rbDerived";
			this.rbDerived.Size = new System.Drawing.Size(62, 17);
			this.rbDerived.TabIndex = 4;
			this.rbDerived.TabStop = true;
			this.rbDerived.Text = "Derived";
			this.rbDerived.UseVisualStyleBackColor = true;
			// 
			// rbMultivalued
			// 
			this.rbMultivalued.AutoSize = true;
			this.rbMultivalued.CausesValidation = false;
			this.rbMultivalued.Location = new System.Drawing.Point(19, 52);
			this.rbMultivalued.Name = "rbMultivalued";
			this.rbMultivalued.Size = new System.Drawing.Size(79, 17);
			this.rbMultivalued.TabIndex = 3;
			this.rbMultivalued.TabStop = true;
			this.rbMultivalued.Text = "Multivalued";
			this.rbMultivalued.UseVisualStyleBackColor = true;
			// 
			// rbSimple
			// 
			this.rbSimple.AutoSize = true;
			this.rbSimple.CausesValidation = false;
			this.rbSimple.Checked = true;
			this.rbSimple.Location = new System.Drawing.Point(19, 29);
			this.rbSimple.Name = "rbSimple";
			this.rbSimple.Size = new System.Drawing.Size(56, 17);
			this.rbSimple.TabIndex = 2;
			this.rbSimple.TabStop = true;
			this.rbSimple.Text = "Simple";
			this.rbSimple.UseVisualStyleBackColor = true;
			// 
			// errorProvider
			// 
			this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.AlwaysBlink;
			this.errorProvider.ContainerControl = this;
			// 
			// AttributeForm
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(249, 266);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.txtName);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnOk);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AttributeForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Attribute";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox cbPrimary;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.RadioButton rbSimple;
        private System.Windows.Forms.RadioButton rbMultivalued;
        private System.Windows.Forms.RadioButton rbDerived;
        private System.Windows.Forms.ComboBox boxAttributeValueType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox cbNull;
        private System.Windows.Forms.CheckBox cbAutoIncrement;
    }
}