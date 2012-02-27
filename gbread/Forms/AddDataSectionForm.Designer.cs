﻿namespace GBRead.Forms
{
	partial class AddDataSectionForm
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
			this.cancelButton = new System.Windows.Forms.Button();
			this.okButton = new System.Windows.Forms.Button();
			this.lengthBox = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.offsetBox = new System.Windows.Forms.TextBox();
			this.nameBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.dataLabelRowLengthLabel = new System.Windows.Forms.Label();
			this.rowLengthBox = new System.Windows.Forms.TextBox();
			this.dataTypeBox = new System.Windows.Forms.ComboBox();
			this.dataLabelDataTypeLabel = new System.Windows.Forms.Label();
			this.commentBox = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(176, 349);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(81, 23);
			this.cancelButton.TabIndex = 21;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// okButton
			// 
			this.okButton.Location = new System.Drawing.Point(15, 349);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 20;
			this.okButton.Text = "OK";
			this.okButton.UseVisualStyleBackColor = true;
			// 
			// lengthBox
			// 
			this.lengthBox.Location = new System.Drawing.Point(95, 58);
			this.lengthBox.Name = "lengthBox";
			this.lengthBox.Size = new System.Drawing.Size(49, 20);
			this.lengthBox.TabIndex = 18;
			this.lengthBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.formBox_keyDown);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 61);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(77, 13);
			this.label3.TabIndex = 17;
			this.label3.Text = "Length (in hex)";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 35);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(35, 13);
			this.label2.TabIndex = 15;
			this.label2.Text = "Offset";
			// 
			// offsetBox
			// 
			this.offsetBox.Location = new System.Drawing.Point(53, 32);
			this.offsetBox.Name = "offsetBox";
			this.offsetBox.Size = new System.Drawing.Size(91, 20);
			this.offsetBox.TabIndex = 14;
			this.offsetBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.formBox_keyDown);
			// 
			// nameBox
			// 
			this.nameBox.Location = new System.Drawing.Point(53, 6);
			this.nameBox.Name = "nameBox";
			this.nameBox.Size = new System.Drawing.Size(204, 20);
			this.nameBox.TabIndex = 13;
			this.nameBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.formBox_keyDown);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 13);
			this.label1.TabIndex = 12;
			this.label1.Text = "Name";
			// 
			// dataLabelRowLengthLabel
			// 
			this.dataLabelRowLengthLabel.AutoSize = true;
			this.dataLabelRowLengthLabel.Location = new System.Drawing.Point(12, 114);
			this.dataLabelRowLengthLabel.Name = "dataLabelRowLengthLabel";
			this.dataLabelRowLengthLabel.Size = new System.Drawing.Size(78, 13);
			this.dataLabelRowLengthLabel.TabIndex = 35;
			this.dataLabelRowLengthLabel.Text = "Data Row Size";
			this.dataLabelRowLengthLabel.Visible = false;
			// 
			// rowLengthBox
			// 
			this.rowLengthBox.Location = new System.Drawing.Point(96, 111);
			this.rowLengthBox.Name = "rowLengthBox";
			this.rowLengthBox.Size = new System.Drawing.Size(87, 20);
			this.rowLengthBox.TabIndex = 33;
			this.rowLengthBox.Visible = false;
			this.rowLengthBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.formBox_keyDown);
			// 
			// dataTypeBox
			// 
			this.dataTypeBox.FormattingEnabled = true;
			this.dataTypeBox.Items.AddRange(new object[] {
            "Data",
            "Image"});
			this.dataTypeBox.Location = new System.Drawing.Point(75, 84);
			this.dataTypeBox.Name = "dataTypeBox";
			this.dataTypeBox.Size = new System.Drawing.Size(76, 21);
			this.dataTypeBox.TabIndex = 32;
			this.dataTypeBox.Visible = false;
			// 
			// dataLabelDataTypeLabel
			// 
			this.dataLabelDataTypeLabel.AutoSize = true;
			this.dataLabelDataTypeLabel.Location = new System.Drawing.Point(12, 87);
			this.dataLabelDataTypeLabel.Name = "dataLabelDataTypeLabel";
			this.dataLabelDataTypeLabel.Size = new System.Drawing.Size(57, 13);
			this.dataLabelDataTypeLabel.TabIndex = 34;
			this.dataLabelDataTypeLabel.Text = "Data Type";
			this.dataLabelDataTypeLabel.Visible = false;
			// 
			// commentBox
			// 
			this.commentBox.Location = new System.Drawing.Point(15, 152);
			this.commentBox.Multiline = true;
			this.commentBox.Name = "commentBox";
			this.commentBox.Size = new System.Drawing.Size(242, 191);
			this.commentBox.TabIndex = 50;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(12, 136);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(51, 13);
			this.label4.TabIndex = 49;
			this.label4.Text = "Comment";
			// 
			// AddDataSectionForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(268, 387);
			this.Controls.Add(this.commentBox);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.dataLabelRowLengthLabel);
			this.Controls.Add(this.rowLengthBox);
			this.Controls.Add(this.dataTypeBox);
			this.Controls.Add(this.dataLabelDataTypeLabel);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.lengthBox);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.offsetBox);
			this.Controls.Add(this.nameBox);
			this.Controls.Add(this.label1);
			this.Name = "AddDataSectionForm";
			this.Text = "AddDataSectionForm";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.TextBox lengthBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox offsetBox;
		private System.Windows.Forms.TextBox nameBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label dataLabelRowLengthLabel;
		private System.Windows.Forms.TextBox rowLengthBox;
		private System.Windows.Forms.ComboBox dataTypeBox;
		private System.Windows.Forms.Label dataLabelDataTypeLabel;
		private System.Windows.Forms.TextBox commentBox;
		private System.Windows.Forms.Label label4;
	}
}