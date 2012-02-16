namespace GBRead
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
			System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Main Window");
			System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Highlighting");
			System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Labels");
			this.printOffsetsCheckBox = new System.Windows.Forms.CheckBox();
			this.hideFunctionsCheckBox = new System.Windows.Forms.CheckBox();
			this.hideDataSectionsCheckBox = new System.Windows.Forms.CheckBox();
			this.printCommentsCheckBox = new System.Windows.Forms.CheckBox();
			this.cancelButton = new System.Windows.Forms.Button();
			this.saveButton = new System.Windows.Forms.Button();
			this.printBitPatternCheckBox = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.offsetNumberFormatBox = new System.Windows.Forms.ComboBox();
			this.instructionNumberFormatBox = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.codeLabelNameSortButton = new System.Windows.Forms.RadioButton();
			this.codeLabelOffsetSortButton = new System.Windows.Forms.RadioButton();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.dataLabelNameSortButton = new System.Windows.Forms.RadioButton();
			this.dataLabelOffsetSortButton = new System.Windows.Forms.RadioButton();
			this.label3 = new System.Windows.Forms.Label();
			this.panel3 = new System.Windows.Forms.Panel();
			this.varLabelNameSortButton = new System.Windows.Forms.RadioButton();
			this.varLabelValueSortButton = new System.Windows.Forms.RadioButton();
			this.label4 = new System.Windows.Forms.Label();
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.mainWindowPanel = new System.Windows.Forms.Panel();
			this.wordWrapCheckBox = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label5 = new System.Windows.Forms.Label();
			this.labelPanel = new System.Windows.Forms.Panel();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.highlightingPanel = new System.Windows.Forms.Panel();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.highlightRegistersCheckBox = new System.Windows.Forms.CheckBox();
			this.highlightCommentsCheckBox = new System.Windows.Forms.CheckBox();
			this.highlightLabelsCheckBox = new System.Windows.Forms.CheckBox();
			this.highlightInstructionsCheckBox = new System.Windows.Forms.CheckBox();
			this.highlightOffsetsCheckBox = new System.Windows.Forms.CheckBox();
			this.highlightNumbersCheckBox = new System.Windows.Forms.CheckBox();
			this.lineSeparator1 = new GBRead.LineSeparator();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel3.SuspendLayout();
			this.mainWindowPanel.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.labelPanel.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.highlightingPanel.SuspendLayout();
			this.groupBox3.SuspendLayout();
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
			// hideFunctionsCheckBox
			// 
			this.hideFunctionsCheckBox.AutoSize = true;
			this.hideFunctionsCheckBox.Location = new System.Drawing.Point(3, 95);
			this.hideFunctionsCheckBox.Name = "hideFunctionsCheckBox";
			this.hideFunctionsCheckBox.Size = new System.Drawing.Size(137, 17);
			this.hideFunctionsCheckBox.TabIndex = 4;
			this.hideFunctionsCheckBox.Text = "Hide Defined Functions";
			this.hideFunctionsCheckBox.UseVisualStyleBackColor = true;
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
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(109, 13);
			this.label2.TabIndex = 15;
			this.label2.Text = "Sort Code Labels By: ";
			// 
			// codeLabelNameSortButton
			// 
			this.codeLabelNameSortButton.AutoSize = true;
			this.codeLabelNameSortButton.Location = new System.Drawing.Point(3, 4);
			this.codeLabelNameSortButton.Name = "codeLabelNameSortButton";
			this.codeLabelNameSortButton.Size = new System.Drawing.Size(53, 17);
			this.codeLabelNameSortButton.TabIndex = 16;
			this.codeLabelNameSortButton.TabStop = true;
			this.codeLabelNameSortButton.Text = "Name";
			this.codeLabelNameSortButton.UseVisualStyleBackColor = true;
			// 
			// codeLabelOffsetSortButton
			// 
			this.codeLabelOffsetSortButton.AutoSize = true;
			this.codeLabelOffsetSortButton.Location = new System.Drawing.Point(62, 3);
			this.codeLabelOffsetSortButton.Name = "codeLabelOffsetSortButton";
			this.codeLabelOffsetSortButton.Size = new System.Drawing.Size(53, 17);
			this.codeLabelOffsetSortButton.TabIndex = 17;
			this.codeLabelOffsetSortButton.TabStop = true;
			this.codeLabelOffsetSortButton.Text = "Offset";
			this.codeLabelOffsetSortButton.UseVisualStyleBackColor = true;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.codeLabelNameSortButton);
			this.panel1.Controls.Add(this.codeLabelOffsetSortButton);
			this.panel1.Location = new System.Drawing.Point(119, 10);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(123, 24);
			this.panel1.TabIndex = 18;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.dataLabelNameSortButton);
			this.panel2.Controls.Add(this.dataLabelOffsetSortButton);
			this.panel2.Location = new System.Drawing.Point(119, 40);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(123, 24);
			this.panel2.TabIndex = 20;
			// 
			// dataLabelNameSortButton
			// 
			this.dataLabelNameSortButton.AutoSize = true;
			this.dataLabelNameSortButton.Location = new System.Drawing.Point(3, 4);
			this.dataLabelNameSortButton.Name = "dataLabelNameSortButton";
			this.dataLabelNameSortButton.Size = new System.Drawing.Size(53, 17);
			this.dataLabelNameSortButton.TabIndex = 16;
			this.dataLabelNameSortButton.TabStop = true;
			this.dataLabelNameSortButton.Text = "Name";
			this.dataLabelNameSortButton.UseVisualStyleBackColor = true;
			// 
			// dataLabelOffsetSortButton
			// 
			this.dataLabelOffsetSortButton.AutoSize = true;
			this.dataLabelOffsetSortButton.Location = new System.Drawing.Point(62, 3);
			this.dataLabelOffsetSortButton.Name = "dataLabelOffsetSortButton";
			this.dataLabelOffsetSortButton.Size = new System.Drawing.Size(53, 17);
			this.dataLabelOffsetSortButton.TabIndex = 17;
			this.dataLabelOffsetSortButton.TabStop = true;
			this.dataLabelOffsetSortButton.Text = "Offset";
			this.dataLabelOffsetSortButton.UseVisualStyleBackColor = true;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(6, 46);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(107, 13);
			this.label3.TabIndex = 19;
			this.label3.Text = "Sort Data Labels By: ";
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.varLabelNameSortButton);
			this.panel3.Controls.Add(this.varLabelValueSortButton);
			this.panel3.Location = new System.Drawing.Point(119, 70);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(123, 24);
			this.panel3.TabIndex = 20;
			// 
			// varLabelNameSortButton
			// 
			this.varLabelNameSortButton.AutoSize = true;
			this.varLabelNameSortButton.Location = new System.Drawing.Point(3, 4);
			this.varLabelNameSortButton.Name = "varLabelNameSortButton";
			this.varLabelNameSortButton.Size = new System.Drawing.Size(53, 17);
			this.varLabelNameSortButton.TabIndex = 16;
			this.varLabelNameSortButton.TabStop = true;
			this.varLabelNameSortButton.Text = "Name";
			this.varLabelNameSortButton.UseVisualStyleBackColor = true;
			// 
			// varLabelValueSortButton
			// 
			this.varLabelValueSortButton.AutoSize = true;
			this.varLabelValueSortButton.Location = new System.Drawing.Point(62, 3);
			this.varLabelValueSortButton.Name = "varLabelValueSortButton";
			this.varLabelValueSortButton.Size = new System.Drawing.Size(52, 17);
			this.varLabelValueSortButton.TabIndex = 17;
			this.varLabelValueSortButton.TabStop = true;
			this.varLabelValueSortButton.Text = "Value";
			this.varLabelValueSortButton.UseVisualStyleBackColor = true;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(6, 76);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(100, 13);
			this.label4.TabIndex = 19;
			this.label4.Text = "Sort Var Labels By: ";
			// 
			// treeView1
			// 
			this.treeView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.treeView1.Location = new System.Drawing.Point(12, 12);
			this.treeView1.Name = "treeView1";
			treeNode1.Name = "mainWindowNode";
			treeNode1.Text = "Main Window";
			treeNode2.Name = "highlightingNode";
			treeNode2.Text = "Highlighting";
			treeNode3.Name = "labelNode";
			treeNode3.Text = "Labels";
			this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3});
			this.treeView1.ShowLines = false;
			this.treeView1.Size = new System.Drawing.Size(128, 248);
			this.treeView1.TabIndex = 21;
			this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
			// 
			// mainWindowPanel
			// 
			this.mainWindowPanel.Controls.Add(this.wordWrapCheckBox);
			this.mainWindowPanel.Controls.Add(this.groupBox1);
			this.mainWindowPanel.Controls.Add(this.printCommentsCheckBox);
			this.mainWindowPanel.Controls.Add(this.hideFunctionsCheckBox);
			this.mainWindowPanel.Controls.Add(this.hideDataSectionsCheckBox);
			this.mainWindowPanel.Controls.Add(this.printBitPatternCheckBox);
			this.mainWindowPanel.Controls.Add(this.printOffsetsCheckBox);
			this.mainWindowPanel.Location = new System.Drawing.Point(146, 12);
			this.mainWindowPanel.Name = "mainWindowPanel";
			this.mainWindowPanel.Size = new System.Drawing.Size(281, 237);
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
			this.groupBox1.Size = new System.Drawing.Size(274, 93);
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
			// labelPanel
			// 
			this.labelPanel.Controls.Add(this.groupBox2);
			this.labelPanel.Location = new System.Drawing.Point(146, 12);
			this.labelPanel.Name = "labelPanel";
			this.labelPanel.Size = new System.Drawing.Size(281, 237);
			this.labelPanel.TabIndex = 23;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Controls.Add(this.panel1);
			this.groupBox2.Controls.Add(this.panel2);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.panel3);
			this.groupBox2.Location = new System.Drawing.Point(3, 3);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(274, 104);
			this.groupBox2.TabIndex = 21;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Sorting";
			// 
			// highlightingPanel
			// 
			this.highlightingPanel.Controls.Add(this.groupBox3);
			this.highlightingPanel.Location = new System.Drawing.Point(146, 12);
			this.highlightingPanel.Name = "highlightingPanel";
			this.highlightingPanel.Size = new System.Drawing.Size(281, 237);
			this.highlightingPanel.TabIndex = 24;
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.highlightRegistersCheckBox);
			this.groupBox3.Controls.Add(this.highlightCommentsCheckBox);
			this.groupBox3.Controls.Add(this.highlightLabelsCheckBox);
			this.groupBox3.Controls.Add(this.highlightInstructionsCheckBox);
			this.groupBox3.Controls.Add(this.highlightOffsetsCheckBox);
			this.groupBox3.Controls.Add(this.highlightNumbersCheckBox);
			this.groupBox3.Location = new System.Drawing.Point(3, 3);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(274, 154);
			this.groupBox3.TabIndex = 1;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Highlight";
			// 
			// highlightRegistersCheckBox
			// 
			this.highlightRegistersCheckBox.AutoSize = true;
			this.highlightRegistersCheckBox.Location = new System.Drawing.Point(6, 65);
			this.highlightRegistersCheckBox.Name = "highlightRegistersCheckBox";
			this.highlightRegistersCheckBox.Size = new System.Drawing.Size(70, 17);
			this.highlightRegistersCheckBox.TabIndex = 5;
			this.highlightRegistersCheckBox.Text = "Registers";
			this.highlightRegistersCheckBox.UseVisualStyleBackColor = true;
			// 
			// highlightCommentsCheckBox
			// 
			this.highlightCommentsCheckBox.AutoSize = true;
			this.highlightCommentsCheckBox.Location = new System.Drawing.Point(6, 88);
			this.highlightCommentsCheckBox.Name = "highlightCommentsCheckBox";
			this.highlightCommentsCheckBox.Size = new System.Drawing.Size(75, 17);
			this.highlightCommentsCheckBox.TabIndex = 4;
			this.highlightCommentsCheckBox.Text = "Comments";
			this.highlightCommentsCheckBox.UseVisualStyleBackColor = true;
			// 
			// highlightLabelsCheckBox
			// 
			this.highlightLabelsCheckBox.AutoSize = true;
			this.highlightLabelsCheckBox.Location = new System.Drawing.Point(6, 111);
			this.highlightLabelsCheckBox.Name = "highlightLabelsCheckBox";
			this.highlightLabelsCheckBox.Size = new System.Drawing.Size(57, 17);
			this.highlightLabelsCheckBox.TabIndex = 3;
			this.highlightLabelsCheckBox.Text = "Labels";
			this.highlightLabelsCheckBox.UseVisualStyleBackColor = true;
			// 
			// highlightInstructionsCheckBox
			// 
			this.highlightInstructionsCheckBox.AutoSize = true;
			this.highlightInstructionsCheckBox.Location = new System.Drawing.Point(6, 19);
			this.highlightInstructionsCheckBox.Name = "highlightInstructionsCheckBox";
			this.highlightInstructionsCheckBox.Size = new System.Drawing.Size(80, 17);
			this.highlightInstructionsCheckBox.TabIndex = 2;
			this.highlightInstructionsCheckBox.Text = "Instructions";
			this.highlightInstructionsCheckBox.UseVisualStyleBackColor = true;
			// 
			// highlightOffsetsCheckBox
			// 
			this.highlightOffsetsCheckBox.AutoSize = true;
			this.highlightOffsetsCheckBox.Location = new System.Drawing.Point(6, 134);
			this.highlightOffsetsCheckBox.Name = "highlightOffsetsCheckBox";
			this.highlightOffsetsCheckBox.Size = new System.Drawing.Size(59, 17);
			this.highlightOffsetsCheckBox.TabIndex = 1;
			this.highlightOffsetsCheckBox.Text = "Offsets";
			this.highlightOffsetsCheckBox.UseVisualStyleBackColor = true;
			// 
			// highlightNumbersCheckBox
			// 
			this.highlightNumbersCheckBox.AutoSize = true;
			this.highlightNumbersCheckBox.Location = new System.Drawing.Point(6, 42);
			this.highlightNumbersCheckBox.Name = "highlightNumbersCheckBox";
			this.highlightNumbersCheckBox.Size = new System.Drawing.Size(68, 17);
			this.highlightNumbersCheckBox.TabIndex = 0;
			this.highlightNumbersCheckBox.Text = "Numbers";
			this.highlightNumbersCheckBox.UseVisualStyleBackColor = true;
			// 
			// lineSeparator1
			// 
			this.lineSeparator1.Location = new System.Drawing.Point(147, 259);
			this.lineSeparator1.Name = "lineSeparator1";
			this.lineSeparator1.Size = new System.Drawing.Size(276, 2);
			this.lineSeparator1.TabIndex = 25;
			// 
			// OptionsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(435, 303);
			this.Controls.Add(this.highlightingPanel);
			this.Controls.Add(this.lineSeparator1);
			this.Controls.Add(this.labelPanel);
			this.Controls.Add(this.mainWindowPanel);
			this.Controls.Add(this.treeView1);
			this.Controls.Add(this.saveButton);
			this.Controls.Add(this.cancelButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "OptionsForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "Options";
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.panel3.ResumeLayout(false);
			this.panel3.PerformLayout();
			this.mainWindowPanel.ResumeLayout(false);
			this.mainWindowPanel.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.labelPanel.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.highlightingPanel.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox printOffsetsCheckBox;
        private System.Windows.Forms.CheckBox hideFunctionsCheckBox;
        private System.Windows.Forms.CheckBox hideDataSectionsCheckBox;
        private System.Windows.Forms.CheckBox printCommentsCheckBox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.CheckBox printBitPatternCheckBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox offsetNumberFormatBox;
        private System.Windows.Forms.ComboBox instructionNumberFormatBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton codeLabelNameSortButton;
        private System.Windows.Forms.RadioButton codeLabelOffsetSortButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton dataLabelNameSortButton;
        private System.Windows.Forms.RadioButton dataLabelOffsetSortButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.RadioButton varLabelNameSortButton;
        private System.Windows.Forms.RadioButton varLabelValueSortButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Panel mainWindowPanel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel labelPanel;
        private System.Windows.Forms.GroupBox groupBox2;
        private LineSeparator lineSeparator1;
        private System.Windows.Forms.CheckBox wordWrapCheckBox;
		private System.Windows.Forms.Panel highlightingPanel;
		private System.Windows.Forms.CheckBox highlightNumbersCheckBox;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.CheckBox highlightRegistersCheckBox;
		private System.Windows.Forms.CheckBox highlightCommentsCheckBox;
		private System.Windows.Forms.CheckBox highlightLabelsCheckBox;
		private System.Windows.Forms.CheckBox highlightInstructionsCheckBox;
		private System.Windows.Forms.CheckBox highlightOffsetsCheckBox;
    }
}