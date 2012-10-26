using GBRead.FormElements;

namespace GBRead.Forms
{
    partial class OptionsForm
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
            this.printOffsetsCheckBox = new System.Windows.Forms.CheckBox();
            this.hideDataSectionsCheckBox = new System.Windows.Forms.CheckBox();
            this.printCommentsCheckBox = new System.Windows.Forms.CheckBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.printBitPatternCheckBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.offsetNumberFormatBox = new System.Windows.Forms.ComboBox();
            this.instructionNumberFormatBox = new System.Windows.Forms.ComboBox();
            this.mainWindowPanel = new System.Windows.Forms.Panel();
            this.wordWrapCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lineSeparator1 = new GBRead.FormElements.LineSeparator();
            this.mainWindowPanel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // printOffsetsCheckBox
            // 
            this.printOffsetsCheckBox.AutoSize = true;
            this.printOffsetsCheckBox.Location = new System.Drawing.Point(3, 26);
            this.printOffsetsCheckBox.Name = "printOffsetsCheckBox";
            this.printOffsetsCheckBox.Size = new System.Drawing.Size(83, 17);
            this.printOffsetsCheckBox.TabIndex = 0;
            this.printOffsetsCheckBox.Text = "Print Offsets";
            this.printOffsetsCheckBox.UseVisualStyleBackColor = true;
            // 
            // hideDataSectionsCheckBox
            // 
            this.hideDataSectionsCheckBox.AutoSize = true;
            this.hideDataSectionsCheckBox.Location = new System.Drawing.Point(3, 118);
            this.hideDataSectionsCheckBox.Name = "hideDataSectionsCheckBox";
            this.hideDataSectionsCheckBox.Size = new System.Drawing.Size(158, 17);
            this.hideDataSectionsCheckBox.TabIndex = 5;
            this.hideDataSectionsCheckBox.Text = "Hide Defined Data Sections";
            this.hideDataSectionsCheckBox.UseVisualStyleBackColor = true;
            // 
            // printCommentsCheckBox
            // 
            this.printCommentsCheckBox.AutoSize = true;
            this.printCommentsCheckBox.Location = new System.Drawing.Point(3, 72);
            this.printCommentsCheckBox.Name = "printCommentsCheckBox";
            this.printCommentsCheckBox.Size = new System.Drawing.Size(173, 17);
            this.printCommentsCheckBox.TabIndex = 6;
            this.printCommentsCheckBox.Text = "Print Extended ASM Comments";
            this.printCommentsCheckBox.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelButton.Location = new System.Drawing.Point(342, 267);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(81, 23);
            this.cancelButton.TabIndex = 7;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // saveButton
            // 
            this.saveButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.saveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveButton.Location = new System.Drawing.Point(255, 267);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(81, 23);
            this.saveButton.TabIndex = 8;
            this.saveButton.Text = "Save Choices";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // printBitPatternCheckBox
            // 
            this.printBitPatternCheckBox.AutoSize = true;
            this.printBitPatternCheckBox.Location = new System.Drawing.Point(3, 49);
            this.printBitPatternCheckBox.Name = "printBitPatternCheckBox";
            this.printBitPatternCheckBox.Size = new System.Drawing.Size(76, 17);
            this.printBitPatternCheckBox.TabIndex = 9;
            this.printBitPatternCheckBox.Text = "Print Bytes";
            this.printBitPatternCheckBox.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Instruction Number Format";
            // 
            // offsetNumberFormatBox
            // 
            this.offsetNumberFormatBox.FormattingEnabled = true;
            this.offsetNumberFormatBox.Items.AddRange(new object[] {
            "BB:OOOO",
            "Hexadecimal",
            "Numeral"});
            this.offsetNumberFormatBox.Location = new System.Drawing.Point(143, 22);
            this.offsetNumberFormatBox.Name = "offsetNumberFormatBox";
            this.offsetNumberFormatBox.Size = new System.Drawing.Size(121, 21);
            this.offsetNumberFormatBox.TabIndex = 13;
            // 
            // instructionNumberFormatBox
            // 
            this.instructionNumberFormatBox.FormattingEnabled = true;
            this.instructionNumberFormatBox.Items.AddRange(new object[] {
            "Hexadecimal",
            "Numeral"});
            this.instructionNumberFormatBox.Location = new System.Drawing.Point(143, 49);
            this.instructionNumberFormatBox.Name = "instructionNumberFormatBox";
            this.instructionNumberFormatBox.Size = new System.Drawing.Size(121, 21);
            this.instructionNumberFormatBox.TabIndex = 14;
            // 
            // mainWindowPanel
            // 
            this.mainWindowPanel.Controls.Add(this.wordWrapCheckBox);
            this.mainWindowPanel.Controls.Add(this.groupBox1);
            this.mainWindowPanel.Controls.Add(this.printCommentsCheckBox);
            this.mainWindowPanel.Controls.Add(this.hideDataSectionsCheckBox);
            this.mainWindowPanel.Controls.Add(this.printBitPatternCheckBox);
            this.mainWindowPanel.Controls.Add(this.printOffsetsCheckBox);
            this.mainWindowPanel.Location = new System.Drawing.Point(12, 12);
            this.mainWindowPanel.Name = "mainWindowPanel";
            this.mainWindowPanel.Size = new System.Drawing.Size(415, 237);
            this.mainWindowPanel.TabIndex = 22;
            // 
            // wordWrapCheckBox
            // 
            this.wordWrapCheckBox.AutoSize = true;
            this.wordWrapCheckBox.Location = new System.Drawing.Point(3, 3);
            this.wordWrapCheckBox.Name = "wordWrapCheckBox";
            this.wordWrapCheckBox.Size = new System.Drawing.Size(81, 17);
            this.wordWrapCheckBox.TabIndex = 11;
            this.wordWrapCheckBox.Text = "Word Wrap";
            this.wordWrapCheckBox.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.offsetNumberFormatBox);
            this.groupBox1.Controls.Add(this.instructionNumberFormatBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(3, 141);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(408, 93);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Number Formats";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(110, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Offset Number Format";
            // 
            // lineSeparator1
            // 
            this.lineSeparator1.Location = new System.Drawing.Point(12, 259);
            this.lineSeparator1.Name = "lineSeparator1";
            this.lineSeparator1.Size = new System.Drawing.Size(415, 2);
            this.lineSeparator1.TabIndex = 25;
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 300);
            this.Controls.Add(this.lineSeparator1);
            this.Controls.Add(this.mainWindowPanel);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "OptionsForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Options";
            this.mainWindowPanel.ResumeLayout(false);
            this.mainWindowPanel.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

		private System.Windows.Forms.CheckBox printOffsetsCheckBox;
        private System.Windows.Forms.CheckBox hideDataSectionsCheckBox;
        private System.Windows.Forms.CheckBox printCommentsCheckBox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.CheckBox printBitPatternCheckBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox offsetNumberFormatBox;
        private System.Windows.Forms.ComboBox instructionNumberFormatBox;
        private System.Windows.Forms.Panel mainWindowPanel;
        private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label5;
        private LineSeparator lineSeparator1;
        private System.Windows.Forms.CheckBox wordWrapCheckBox;
    }
}