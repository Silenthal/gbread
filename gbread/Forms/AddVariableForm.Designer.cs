namespace GBRead.Forms
{
	partial class AddVarLabelForm
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
			this.dataTypeBox = new System.Windows.Forms.ComboBox();
			this.dataLabelDataTypeLabel = new System.Windows.Forms.Label();
			this.cancelButton = new System.Windows.Forms.Button();
			this.okButton = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.offsetBox = new System.Windows.Forms.TextBox();
			this.nameBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.commentBox = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// dataTypeBox
			// 
			this.dataTypeBox.FormattingEnabled = true;
			this.dataTypeBox.Items.AddRange(new object[] {
            "Byte",
            "Word"});
			this.dataTypeBox.Location = new System.Drawing.Point(75, 58);
			this.dataTypeBox.Name = "dataTypeBox";
			this.dataTypeBox.Size = new System.Drawing.Size(69, 21);
			this.dataTypeBox.TabIndex = 44;
			this.dataTypeBox.Visible = false;
			// 
			// dataLabelDataTypeLabel
			// 
			this.dataLabelDataTypeLabel.AutoSize = true;
			this.dataLabelDataTypeLabel.Location = new System.Drawing.Point(12, 61);
			this.dataLabelDataTypeLabel.Name = "dataLabelDataTypeLabel";
			this.dataLabelDataTypeLabel.Size = new System.Drawing.Size(57, 13);
			this.dataLabelDataTypeLabel.TabIndex = 46;
			this.dataLabelDataTypeLabel.Text = "Data Type";
			this.dataLabelDataTypeLabel.Visible = false;
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(176, 297);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(81, 23);
			this.cancelButton.TabIndex = 43;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// okButton
			// 
			this.okButton.Location = new System.Drawing.Point(15, 297);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 42;
			this.okButton.Text = "OK";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 35);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(34, 13);
			this.label2.TabIndex = 39;
			this.label2.Text = "Value";
			// 
			// offsetBox
			// 
			this.offsetBox.Location = new System.Drawing.Point(53, 32);
			this.offsetBox.Name = "offsetBox";
			this.offsetBox.Size = new System.Drawing.Size(91, 20);
			this.offsetBox.TabIndex = 38;
			this.offsetBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.formBox_keyDown);
			// 
			// nameBox
			// 
			this.nameBox.Location = new System.Drawing.Point(53, 6);
			this.nameBox.Name = "nameBox";
			this.nameBox.Size = new System.Drawing.Size(204, 20);
			this.nameBox.TabIndex = 37;
			this.nameBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.formBox_keyDown);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 13);
			this.label1.TabIndex = 36;
			this.label1.Text = "Name";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 84);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(51, 13);
			this.label3.TabIndex = 47;
			this.label3.Text = "Comment";
			// 
			// commentBox
			// 
			this.commentBox.Location = new System.Drawing.Point(15, 100);
			this.commentBox.Multiline = true;
			this.commentBox.Name = "commentBox";
			this.commentBox.Size = new System.Drawing.Size(242, 191);
			this.commentBox.TabIndex = 48;
			// 
			// AddVariableForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(269, 333);
			this.Controls.Add(this.commentBox);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.dataTypeBox);
			this.Controls.Add(this.dataLabelDataTypeLabel);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.offsetBox);
			this.Controls.Add(this.nameBox);
			this.Controls.Add(this.label1);
			this.Name = "AddVariableForm";
			this.Text = "AddVariableForm";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox dataTypeBox;
		private System.Windows.Forms.Label dataLabelDataTypeLabel;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox offsetBox;
		private System.Windows.Forms.TextBox nameBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox commentBox;
	}
}