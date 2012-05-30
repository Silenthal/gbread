namespace GBRead.Forms
{
	partial class AddFunctionLabelForm
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
			this.label1 = new System.Windows.Forms.Label();
			this.nameBox = new System.Windows.Forms.TextBox();
			this.offsetBox = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.isFunctionCheckBox = new System.Windows.Forms.CheckBox();
			this.label3 = new System.Windows.Forms.Label();
			this.lengthBox = new System.Windows.Forms.TextBox();
			this.guessLengthButton = new System.Windows.Forms.Button();
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.commentBox = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Name";
			// 
			// nameBox
			// 
			this.nameBox.Location = new System.Drawing.Point(53, 6);
			this.nameBox.Name = "nameBox";
			this.nameBox.Size = new System.Drawing.Size(164, 20);
			this.nameBox.TabIndex = 1;
			this.nameBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.formBox_keyDown);
			// 
			// offsetBox
			// 
			this.offsetBox.Location = new System.Drawing.Point(53, 32);
			this.offsetBox.Name = "offsetBox";
			this.offsetBox.Size = new System.Drawing.Size(91, 20);
			this.offsetBox.TabIndex = 2;
			this.offsetBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.formBox_keyDown);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 35);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(35, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Offset";
			// 
			// isFunctionCheckBox
			// 
			this.isFunctionCheckBox.AutoSize = true;
			this.isFunctionCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.isFunctionCheckBox.Location = new System.Drawing.Point(11, 58);
			this.isFunctionCheckBox.Name = "isFunctionCheckBox";
			this.isFunctionCheckBox.Size = new System.Drawing.Size(67, 17);
			this.isFunctionCheckBox.TabIndex = 4;
			this.isFunctionCheckBox.Text = "Function";
			this.isFunctionCheckBox.UseVisualStyleBackColor = true;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(85, 59);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(77, 13);
			this.label3.TabIndex = 5;
			this.label3.Text = "Length (in hex)";
			// 
			// lengthBox
			// 
			this.lengthBox.Location = new System.Drawing.Point(168, 56);
			this.lengthBox.Name = "lengthBox";
			this.lengthBox.Size = new System.Drawing.Size(49, 20);
			this.lengthBox.TabIndex = 6;
			this.lengthBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.formBox_keyDown);
			// 
			// guessLengthButton
			// 
			this.guessLengthButton.Location = new System.Drawing.Point(223, 54);
			this.guessLengthButton.Name = "guessLengthButton";
			this.guessLengthButton.Size = new System.Drawing.Size(81, 23);
			this.guessLengthButton.TabIndex = 7;
			this.guessLengthButton.Text = "Guess Length";
			this.guessLengthButton.UseVisualStyleBackColor = true;
			this.guessLengthButton.Click += new System.EventHandler(this.guessLengthButton_Click);
			// 
			// okButton
			// 
			this.okButton.Location = new System.Drawing.Point(15, 294);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 10;
			this.okButton.Text = "OK";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(223, 294);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(81, 23);
			this.cancelButton.TabIndex = 11;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// commentBox
			// 
			this.commentBox.Location = new System.Drawing.Point(15, 97);
			this.commentBox.Multiline = true;
			this.commentBox.Name = "commentBox";
			this.commentBox.Size = new System.Drawing.Size(289, 191);
			this.commentBox.TabIndex = 50;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(12, 81);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(51, 13);
			this.label4.TabIndex = 49;
			this.label4.Text = "Comment";
			// 
			// AddFunctionForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(320, 332);
			this.Controls.Add(this.commentBox);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.guessLengthButton);
			this.Controls.Add(this.lengthBox);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.isFunctionCheckBox);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.offsetBox);
			this.Controls.Add(this.nameBox);
			this.Controls.Add(this.label1);
			this.Name = "AddFunctionForm";
			this.Text = "Add Label";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox nameBox;
		private System.Windows.Forms.TextBox offsetBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckBox isFunctionCheckBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox lengthBox;
		private System.Windows.Forms.Button guessLengthButton;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.TextBox commentBox;
		private System.Windows.Forms.Label label4;
	}
}