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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.asmColor11Box = new System.Windows.Forms.TextBox();
            this.asmColor10Box = new System.Windows.Forms.TextBox();
            this.asmColor01Box = new System.Windows.Forms.TextBox();
            this.asmColor00Box = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.dsmColor11Box = new System.Windows.Forms.TextBox();
            this.dsmColor10Box = new System.Windows.Forms.TextBox();
            this.dsmColor01Box = new System.Windows.Forms.TextBox();
            this.dsmColor00Box = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.wordWrapCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lineSeparator1 = new GBRead.FormElements.LineSeparator();
            this.mainWindowPanel.SuspendLayout();
            this.groupBox2.SuspendLayout();
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
            this.hideDataSectionsCheckBox.Location = new System.Drawing.Point(3, 95);
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
            this.cancelButton.Location = new System.Drawing.Point(349, 384);
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
            this.saveButton.Location = new System.Drawing.Point(262, 384);
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
            this.mainWindowPanel.Controls.Add(this.groupBox2);
            this.mainWindowPanel.Controls.Add(this.wordWrapCheckBox);
            this.mainWindowPanel.Controls.Add(this.groupBox1);
            this.mainWindowPanel.Controls.Add(this.printCommentsCheckBox);
            this.mainWindowPanel.Controls.Add(this.hideDataSectionsCheckBox);
            this.mainWindowPanel.Controls.Add(this.printBitPatternCheckBox);
            this.mainWindowPanel.Controls.Add(this.printOffsetsCheckBox);
            this.mainWindowPanel.Location = new System.Drawing.Point(12, 12);
            this.mainWindowPanel.Name = "mainWindowPanel";
            this.mainWindowPanel.Size = new System.Drawing.Size(418, 358);
            this.mainWindowPanel.TabIndex = 22;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.asmColor11Box);
            this.groupBox2.Controls.Add(this.asmColor10Box);
            this.groupBox2.Controls.Add(this.asmColor01Box);
            this.groupBox2.Controls.Add(this.asmColor00Box);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.dsmColor11Box);
            this.groupBox2.Controls.Add(this.dsmColor10Box);
            this.groupBox2.Controls.Add(this.dsmColor01Box);
            this.groupBox2.Controls.Add(this.dsmColor00Box);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(3, 210);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(408, 139);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Gameboy Format String";
            // 
            // asmColor11Box
            // 
            this.asmColor11Box.Location = new System.Drawing.Point(141, 109);
            this.asmColor11Box.MaxLength = 1;
            this.asmColor11Box.Name = "asmColor11Box";
            this.asmColor11Box.Size = new System.Drawing.Size(25, 20);
            this.asmColor11Box.TabIndex = 17;
            // 
            // asmColor10Box
            // 
            this.asmColor10Box.Location = new System.Drawing.Point(141, 83);
            this.asmColor10Box.MaxLength = 1;
            this.asmColor10Box.Name = "asmColor10Box";
            this.asmColor10Box.Size = new System.Drawing.Size(25, 20);
            this.asmColor10Box.TabIndex = 16;
            // 
            // asmColor01Box
            // 
            this.asmColor01Box.Location = new System.Drawing.Point(141, 59);
            this.asmColor01Box.MaxLength = 1;
            this.asmColor01Box.Name = "asmColor01Box";
            this.asmColor01Box.Size = new System.Drawing.Size(25, 20);
            this.asmColor01Box.TabIndex = 15;
            // 
            // asmColor00Box
            // 
            this.asmColor00Box.Location = new System.Drawing.Point(141, 32);
            this.asmColor00Box.MaxLength = 1;
            this.asmColor00Box.Name = "asmColor00Box";
            this.asmColor00Box.Size = new System.Drawing.Size(25, 20);
            this.asmColor00Box.TabIndex = 14;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(89, 112);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(46, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "Color 11";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(89, 86);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(46, 13);
            this.label10.TabIndex = 12;
            this.label10.Text = "Color 10";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(89, 62);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(46, 13);
            this.label11.TabIndex = 11;
            this.label11.Text = "Color 01";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(89, 35);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(46, 13);
            this.label12.TabIndex = 10;
            this.label12.Text = "Color 00";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(89, 16);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(55, 13);
            this.label8.TabIndex = 9;
            this.label8.Text = "Assembler";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(69, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Disassembler";
            // 
            // dsmColor11Box
            // 
            this.dsmColor11Box.Location = new System.Drawing.Point(58, 109);
            this.dsmColor11Box.MaxLength = 1;
            this.dsmColor11Box.Name = "dsmColor11Box";
            this.dsmColor11Box.Size = new System.Drawing.Size(25, 20);
            this.dsmColor11Box.TabIndex = 7;
            // 
            // dsmColor10Box
            // 
            this.dsmColor10Box.Location = new System.Drawing.Point(58, 83);
            this.dsmColor10Box.MaxLength = 1;
            this.dsmColor10Box.Name = "dsmColor10Box";
            this.dsmColor10Box.Size = new System.Drawing.Size(25, 20);
            this.dsmColor10Box.TabIndex = 6;
            // 
            // dsmColor01Box
            // 
            this.dsmColor01Box.Location = new System.Drawing.Point(58, 59);
            this.dsmColor01Box.MaxLength = 1;
            this.dsmColor01Box.Name = "dsmColor01Box";
            this.dsmColor01Box.Size = new System.Drawing.Size(25, 20);
            this.dsmColor01Box.TabIndex = 5;
            // 
            // dsmColor00Box
            // 
            this.dsmColor00Box.Location = new System.Drawing.Point(58, 32);
            this.dsmColor00Box.MaxLength = 1;
            this.dsmColor00Box.Name = "dsmColor00Box";
            this.dsmColor00Box.Size = new System.Drawing.Size(25, 20);
            this.dsmColor00Box.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 112);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Color 11";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 86);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Color 10";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Color 01";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Color 00";
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
            this.groupBox1.Location = new System.Drawing.Point(3, 118);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(408, 86);
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
            this.lineSeparator1.Location = new System.Drawing.Point(15, 376);
            this.lineSeparator1.Name = "lineSeparator1";
            this.lineSeparator1.Size = new System.Drawing.Size(415, 2);
            this.lineSeparator1.TabIndex = 25;
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 420);
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
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
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
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox dsmColor11Box;
        private System.Windows.Forms.TextBox dsmColor10Box;
        private System.Windows.Forms.TextBox dsmColor01Box;
        private System.Windows.Forms.TextBox dsmColor00Box;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox asmColor11Box;
        private System.Windows.Forms.TextBox asmColor10Box;
        private System.Windows.Forms.TextBox asmColor01Box;
        private System.Windows.Forms.TextBox asmColor00Box;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
    }
}