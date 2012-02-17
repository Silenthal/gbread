namespace GBRead.Forms
{
    partial class InsertCodeForm
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
			this.assembleButton = new System.Windows.Forms.Button();
			this.offsetBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.insertButton = new System.Windows.Forms.Button();
			this.replaceTextBox = new System.Windows.Forms.RichTextBox();
			this.intermediaryTextBox = new System.Windows.Forms.RichTextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.cancelButton = new System.Windows.Forms.Button();
			this.codeTextBox = new GBRead.SyntaxHighlightingTextBox();
			this.SuspendLayout();
			// 
			// assembleButton
			// 
			this.assembleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.assembleButton.Location = new System.Drawing.Point(12, 417);
			this.assembleButton.Name = "assembleButton";
			this.assembleButton.Size = new System.Drawing.Size(251, 23);
			this.assembleButton.TabIndex = 2;
			this.assembleButton.Text = "Assemble Code";
			this.assembleButton.UseVisualStyleBackColor = true;
			this.assembleButton.Click += new System.EventHandler(this.assembleButton_Click);
			// 
			// offsetBox
			// 
			this.offsetBox.Location = new System.Drawing.Point(163, 391);
			this.offsetBox.Name = "offsetBox";
			this.offsetBox.Size = new System.Drawing.Size(100, 20);
			this.offsetBox.TabIndex = 1;
			this.offsetBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.offsetBox_KeyPress);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(9, 392);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(107, 16);
			this.label1.TabIndex = 4;
			this.label1.Text = "Insert at position:";
			// 
			// insertButton
			// 
			this.insertButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.insertButton.Location = new System.Drawing.Point(289, 417);
			this.insertButton.Name = "insertButton";
			this.insertButton.Size = new System.Drawing.Size(247, 23);
			this.insertButton.TabIndex = 3;
			this.insertButton.Text = "Insert ASM";
			this.insertButton.UseVisualStyleBackColor = true;
			this.insertButton.Click += new System.EventHandler(this.insertButton_Click);
			// 
			// replaceTextBox
			// 
			this.replaceTextBox.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.replaceTextBox.Location = new System.Drawing.Point(562, 25);
			this.replaceTextBox.Name = "replaceTextBox";
			this.replaceTextBox.ReadOnly = true;
			this.replaceTextBox.Size = new System.Drawing.Size(258, 360);
			this.replaceTextBox.TabIndex = 6;
			this.replaceTextBox.Text = "";
			this.replaceTextBox.WordWrap = false;
			// 
			// intermediaryTextBox
			// 
			this.intermediaryTextBox.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.intermediaryTextBox.Location = new System.Drawing.Point(289, 25);
			this.intermediaryTextBox.Name = "intermediaryTextBox";
			this.intermediaryTextBox.ReadOnly = true;
			this.intermediaryTextBox.Size = new System.Drawing.Size(247, 360);
			this.intermediaryTextBox.TabIndex = 5;
			this.intermediaryTextBox.Text = "";
			this.intermediaryTextBox.WordWrap = false;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(264, 152);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(24, 17);
			this.label3.TabIndex = 10;
			this.label3.Text = "->";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(537, 152);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(24, 17);
			this.label4.TabIndex = 11;
			this.label4.Text = "->";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(97, 9);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(57, 13);
			this.label2.TabIndex = 12;
			this.label2.Text = "ASM Input";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(378, 9);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(65, 13);
			this.label5.TabIndex = 13;
			this.label5.Text = "ASM Output";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(634, 9);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(109, 13);
			this.label6.TabIndex = 14;
			this.label6.Text = "Data Being Replaced";
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cancelButton.Location = new System.Drawing.Point(562, 417);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(258, 23);
			this.cancelButton.TabIndex = 4;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// codeTextBox
			// 
			this.codeTextBox.AutoScrollMinSize = new System.Drawing.Size(25, 14);
			this.codeTextBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.codeTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.codeTextBox.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.codeTextBox.Location = new System.Drawing.Point(12, 25);
			this.codeTextBox.Name = "codeTextBox";
			this.codeTextBox.Size = new System.Drawing.Size(251, 360);
			this.codeTextBox.TabIndex = 15;
			// 
			// InsertCodeForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(834, 452);
			this.Controls.Add(this.codeTextBox);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.intermediaryTextBox);
			this.Controls.Add(this.replaceTextBox);
			this.Controls.Add(this.insertButton);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.offsetBox);
			this.Controls.Add(this.assembleButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "InsertCodeForm";
			this.Text = "Insert ASM";
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.Button assembleButton;
        private System.Windows.Forms.TextBox offsetBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button insertButton;
        private System.Windows.Forms.RichTextBox replaceTextBox;
        private System.Windows.Forms.RichTextBox intermediaryTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button cancelButton;
		private SyntaxHighlightingTextBox codeTextBox;
    }
}