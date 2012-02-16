namespace GBRead
{
    partial class MainForm
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
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveChangedFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.loadFunctionListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveCalledFunctionsListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.saveEntireFileASMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.optionsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.insertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.insertASMAtLocationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.insertExternalBinaryAtLocationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.searchToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.addAllCalledFunctionsToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.startBox = new System.Windows.Forms.TextBox();
			this.endBox = new System.Windows.Forms.TextBox();
			this.printASMButton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.codeLabelBoxContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.renameLabelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.searchForFunctionsThatCallThisOneToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.codeLabelBoxContextMenu2 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.addANewLabelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.dataLabelBoxContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.renameADataSectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.removeToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.findReferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exportToBinaryFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.dataLabelContextMenu2 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.addANewDataSectionToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
			this.progressLabel = new System.Windows.Forms.Label();
			this.varLabelBoxContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.editVariableToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.removeVariableToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
			this.varLabelBoxContextMenu2 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.addVariableToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.addButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.editButton = new System.Windows.Forms.Button();
			this.mainTextBox = new GBRead.SyntaxHighlightingTextBox();
			this.varLabelTabPage = new System.Windows.Forms.TabPage();
			this.varLabelCommentLabel = new System.Windows.Forms.Label();
			this.varLabelCommentBox = new System.Windows.Forms.TextBox();
			this.varLabelNameBox = new System.Windows.Forms.TextBox();
			this.varLabelOffsetBox = new System.Windows.Forms.TextBox();
			this.varLabelOffsetLabel = new System.Windows.Forms.Label();
			this.varLabelNameLabel = new System.Windows.Forms.Label();
			this.varLabelBox = new System.Windows.Forms.ListBox();
			this.dataLabelTabPage = new System.Windows.Forms.TabPage();
			this.dataLabelRowLengthLabel = new System.Windows.Forms.Label();
			this.dataLabelRowLengthBox = new System.Windows.Forms.TextBox();
			this.dataLabelCommentBox = new System.Windows.Forms.TextBox();
			this.dataLabelOffsetBox = new System.Windows.Forms.TextBox();
			this.dataLabelNameBox = new System.Windows.Forms.TextBox();
			this.dataLabelLengthBox = new System.Windows.Forms.TextBox();
			this.dataLabelDataTypeBox = new System.Windows.Forms.ComboBox();
			this.dataLabelDataTypeLabel = new System.Windows.Forms.Label();
			this.dataLabelOffsetLabel = new System.Windows.Forms.Label();
			this.dataLabelNameLabel = new System.Windows.Forms.Label();
			this.dataLabelLengthLabel = new System.Windows.Forms.Label();
			this.dataLabelCommentLabel = new System.Windows.Forms.Label();
			this.dataLabelBox = new System.Windows.Forms.ListBox();
			this.functionLabelTabPage = new System.Windows.Forms.TabPage();
			this.codeLabelCommentLabel = new System.Windows.Forms.Label();
			this.codeLabelLengthLabel = new System.Windows.Forms.Label();
			this.codeLabelOffsetLabel = new System.Windows.Forms.Label();
			this.codeLabelNameLabel = new System.Windows.Forms.Label();
			this.codeLabelOffsetBox = new System.Windows.Forms.TextBox();
			this.codeLabelCommentBox = new System.Windows.Forms.TextBox();
			this.codeLabelLengthBox = new System.Windows.Forms.TextBox();
			this.codeLabelNameBox = new System.Windows.Forms.TextBox();
			this.codeLabelBox = new System.Windows.Forms.ListBox();
			this.LabelTabControl = new System.Windows.Forms.TabControl();
			this.menuStrip1.SuspendLayout();
			this.codeLabelBoxContextMenu.SuspendLayout();
			this.codeLabelBoxContextMenu2.SuspendLayout();
			this.dataLabelBoxContextMenu.SuspendLayout();
			this.dataLabelContextMenu2.SuspendLayout();
			this.varLabelBoxContextMenu.SuspendLayout();
			this.varLabelBoxContextMenu2.SuspendLayout();
			this.varLabelTabPage.SuspendLayout();
			this.dataLabelTabPage.SuspendLayout();
			this.functionLabelTabPage.SuspendLayout();
			this.LabelTabControl.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.optionsToolStripMenuItem1,
            this.insertToolStripMenuItem,
            this.searchToolStripMenuItem2});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(704, 24);
			this.menuStrip1.TabIndex = 0;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveChangedFileToolStripMenuItem,
            this.toolStripSeparator2,
            this.loadFunctionListToolStripMenuItem,
            this.saveCalledFunctionsListToolStripMenuItem,
            this.toolStripSeparator3,
            this.saveEntireFileASMToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.openToolStripMenuItem.Size = new System.Drawing.Size(262, 22);
			this.openToolStripMenuItem.Text = "&Open GB/C File";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
			// 
			// saveChangedFileToolStripMenuItem
			// 
			this.saveChangedFileToolStripMenuItem.Name = "saveChangedFileToolStripMenuItem";
			this.saveChangedFileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.saveChangedFileToolStripMenuItem.Size = new System.Drawing.Size(262, 22);
			this.saveChangedFileToolStripMenuItem.Text = "&Save GB/C File...";
			this.saveChangedFileToolStripMenuItem.Click += new System.EventHandler(this.saveChangedFileToolStripMenuItem_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(259, 6);
			// 
			// loadFunctionListToolStripMenuItem
			// 
			this.loadFunctionListToolStripMenuItem.Name = "loadFunctionListToolStripMenuItem";
			this.loadFunctionListToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
			this.loadFunctionListToolStripMenuItem.Size = new System.Drawing.Size(262, 22);
			this.loadFunctionListToolStripMenuItem.Text = "&Load Function/Data/Var List";
			this.loadFunctionListToolStripMenuItem.Click += new System.EventHandler(this.loadFunctionListToolStripMenuItem_Click);
			// 
			// saveCalledFunctionsListToolStripMenuItem
			// 
			this.saveCalledFunctionsListToolStripMenuItem.Name = "saveCalledFunctionsListToolStripMenuItem";
			this.saveCalledFunctionsListToolStripMenuItem.Size = new System.Drawing.Size(262, 22);
			this.saveCalledFunctionsListToolStripMenuItem.Text = "Save &Function/Data/Var List";
			this.saveCalledFunctionsListToolStripMenuItem.Click += new System.EventHandler(this.saveCalledFunctionsListToolStripMenuItem_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(259, 6);
			// 
			// saveEntireFileASMToolStripMenuItem
			// 
			this.saveEntireFileASMToolStripMenuItem.Name = "saveEntireFileASMToolStripMenuItem";
			this.saveEntireFileASMToolStripMenuItem.Size = new System.Drawing.Size(262, 22);
			this.saveEntireFileASMToolStripMenuItem.Text = "Save &Entire File ASM";
			this.saveEntireFileASMToolStripMenuItem.Click += new System.EventHandler(this.saveEntireFileASMToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(259, 6);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(262, 22);
			this.exitToolStripMenuItem.Text = "E&xit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// optionsToolStripMenuItem1
			// 
			this.optionsToolStripMenuItem1.Name = "optionsToolStripMenuItem1";
			this.optionsToolStripMenuItem1.Size = new System.Drawing.Size(61, 20);
			this.optionsToolStripMenuItem1.Text = "&Options";
			this.optionsToolStripMenuItem1.Click += new System.EventHandler(this.optionsToolStripMenuItem1_Click);
			// 
			// insertToolStripMenuItem
			// 
			this.insertToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.insertASMAtLocationToolStripMenuItem,
            this.insertExternalBinaryAtLocationToolStripMenuItem});
			this.insertToolStripMenuItem.Name = "insertToolStripMenuItem";
			this.insertToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
			this.insertToolStripMenuItem.Text = "&Insert";
			// 
			// insertASMAtLocationToolStripMenuItem
			// 
			this.insertASMAtLocationToolStripMenuItem.Name = "insertASMAtLocationToolStripMenuItem";
			this.insertASMAtLocationToolStripMenuItem.Size = new System.Drawing.Size(256, 22);
			this.insertASMAtLocationToolStripMenuItem.Text = "Insert &ASM At Location...";
			this.insertASMAtLocationToolStripMenuItem.Click += new System.EventHandler(this.insertASMAtLocationToolStripMenuItem_Click);
			// 
			// insertExternalBinaryAtLocationToolStripMenuItem
			// 
			this.insertExternalBinaryAtLocationToolStripMenuItem.Name = "insertExternalBinaryAtLocationToolStripMenuItem";
			this.insertExternalBinaryAtLocationToolStripMenuItem.Size = new System.Drawing.Size(256, 22);
			this.insertExternalBinaryAtLocationToolStripMenuItem.Text = "Insert &External Binary At Location...";
			this.insertExternalBinaryAtLocationToolStripMenuItem.Click += new System.EventHandler(this.insertExternalBinaryAtLocationToolStripMenuItem_Click);
			// 
			// searchToolStripMenuItem2
			// 
			this.searchToolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addAllCalledFunctionsToolStripMenuItem2});
			this.searchToolStripMenuItem2.Name = "searchToolStripMenuItem2";
			this.searchToolStripMenuItem2.Size = new System.Drawing.Size(54, 20);
			this.searchToolStripMenuItem2.Text = "&Search";
			// 
			// addAllCalledFunctionsToolStripMenuItem2
			// 
			this.addAllCalledFunctionsToolStripMenuItem2.Name = "addAllCalledFunctionsToolStripMenuItem2";
			this.addAllCalledFunctionsToolStripMenuItem2.Size = new System.Drawing.Size(188, 22);
			this.addAllCalledFunctionsToolStripMenuItem2.Text = "Find Called Functions";
			this.addAllCalledFunctionsToolStripMenuItem2.Click += new System.EventHandler(this.addAllCallsMadeInCodeToolStripMenuItem_Click);
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.Filter = "GB Files|*.gb|GBC Files|*.gbc|All Files|*";
			// 
			// startBox
			// 
			this.startBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.startBox.Location = new System.Drawing.Point(12, 288);
			this.startBox.Name = "startBox";
			this.startBox.Size = new System.Drawing.Size(106, 20);
			this.startBox.TabIndex = 0;
			this.startBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.startEndBox_KeyPress);
			// 
			// endBox
			// 
			this.endBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.endBox.Location = new System.Drawing.Point(124, 288);
			this.endBox.Name = "endBox";
			this.endBox.Size = new System.Drawing.Size(103, 20);
			this.endBox.TabIndex = 1;
			this.endBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.startEndBox_KeyPress);
			// 
			// printASMButton
			// 
			this.printASMButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.printASMButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.printASMButton.Location = new System.Drawing.Point(233, 285);
			this.printASMButton.Name = "printASMButton";
			this.printASMButton.Size = new System.Drawing.Size(62, 23);
			this.printASMButton.TabIndex = 2;
			this.printASMButton.Text = "Print ASM";
			this.printASMButton.UseVisualStyleBackColor = true;
			this.printASMButton.Click += new System.EventHandler(this.printASMButton_Click);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 272);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(106, 13);
			this.label1.TabIndex = 12;
			this.label1.Text = "Start Position (in hex)";
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(124, 272);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(103, 13);
			this.label2.TabIndex = 13;
			this.label2.Text = "End Position (in hex)";
			// 
			// saveFileDialog1
			// 
			this.saveFileDialog1.Filter = ".txt File|*.txt|All Files|*";
			// 
			// codeLabelBoxContextMenu
			// 
			this.codeLabelBoxContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.renameLabelToolStripMenuItem,
            this.removeToolStripMenuItem,
            this.searchForFunctionsThatCallThisOneToolStripMenuItem1});
			this.codeLabelBoxContextMenu.Name = "functionBoxContextMenu";
			this.codeLabelBoxContextMenu.Size = new System.Drawing.Size(167, 70);
			// 
			// renameLabelToolStripMenuItem
			// 
			this.renameLabelToolStripMenuItem.Name = "renameLabelToolStripMenuItem";
			this.renameLabelToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
			this.renameLabelToolStripMenuItem.Text = "Edit";
			this.renameLabelToolStripMenuItem.Click += new System.EventHandler(this.renameLabelToolStripMenuItem_Click);
			// 
			// removeToolStripMenuItem
			// 
			this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
			this.removeToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
			this.removeToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
			this.removeToolStripMenuItem.Text = "Remove";
			this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeFunctionMenuItem_Click);
			// 
			// searchForFunctionsThatCallThisOneToolStripMenuItem1
			// 
			this.searchForFunctionsThatCallThisOneToolStripMenuItem1.Name = "searchForFunctionsThatCallThisOneToolStripMenuItem1";
			this.searchForFunctionsThatCallThisOneToolStripMenuItem1.Size = new System.Drawing.Size(166, 22);
			this.searchForFunctionsThatCallThisOneToolStripMenuItem1.Text = "Find References...";
			this.searchForFunctionsThatCallThisOneToolStripMenuItem1.Click += new System.EventHandler(this.searchForFunctionsThatCallThisOneToolStripMenuItem_Click);
			// 
			// codeLabelBoxContextMenu2
			// 
			this.codeLabelBoxContextMenu2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addANewLabelToolStripMenuItem});
			this.codeLabelBoxContextMenu2.Name = "functionBoxContextMenu2";
			this.codeLabelBoxContextMenu2.Size = new System.Drawing.Size(174, 26);
			// 
			// addANewLabelToolStripMenuItem
			// 
			this.addANewLabelToolStripMenuItem.Name = "addANewLabelToolStripMenuItem";
			this.addANewLabelToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
			this.addANewLabelToolStripMenuItem.Text = "Add New Function";
			this.addANewLabelToolStripMenuItem.Click += new System.EventHandler(this.addANewCodeLabelToolStripMenuItem_Click);
			// 
			// dataLabelBoxContextMenu
			// 
			this.dataLabelBoxContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.renameADataSectionToolStripMenuItem,
            this.removeToolStripMenuItem1,
            this.findReferencesToolStripMenuItem,
            this.exportToBinaryFileToolStripMenuItem});
			this.dataLabelBoxContextMenu.Name = "dataSectionBoxContextMenu";
			this.dataLabelBoxContextMenu.Size = new System.Drawing.Size(167, 92);
			// 
			// renameADataSectionToolStripMenuItem
			// 
			this.renameADataSectionToolStripMenuItem.Name = "renameADataSectionToolStripMenuItem";
			this.renameADataSectionToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
			this.renameADataSectionToolStripMenuItem.Text = "Edit";
			this.renameADataSectionToolStripMenuItem.Click += new System.EventHandler(this.renameADataSectionToolStripMenuItem_Click);
			// 
			// removeToolStripMenuItem1
			// 
			this.removeToolStripMenuItem1.Name = "removeToolStripMenuItem1";
			this.removeToolStripMenuItem1.Size = new System.Drawing.Size(166, 22);
			this.removeToolStripMenuItem1.Text = "Remove";
			this.removeToolStripMenuItem1.Click += new System.EventHandler(this.removeToolStripMenuItem1_Click);
			// 
			// findReferencesToolStripMenuItem
			// 
			this.findReferencesToolStripMenuItem.Name = "findReferencesToolStripMenuItem";
			this.findReferencesToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
			this.findReferencesToolStripMenuItem.Text = "Find References...";
			this.findReferencesToolStripMenuItem.Click += new System.EventHandler(this.findReferencesToolStripMenuItem_Click);
			// 
			// exportToBinaryFileToolStripMenuItem
			// 
			this.exportToBinaryFileToolStripMenuItem.Name = "exportToBinaryFileToolStripMenuItem";
			this.exportToBinaryFileToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
			this.exportToBinaryFileToolStripMenuItem.Text = "Export As Binary";
			this.exportToBinaryFileToolStripMenuItem.Click += new System.EventHandler(this.exportToBinaryFileToolStripMenuItem_Click);
			// 
			// dataLabelContextMenu2
			// 
			this.dataLabelContextMenu2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addANewDataSectionToolStripMenuItem1});
			this.dataLabelContextMenu2.Name = "dataSectionContextMenu2";
			this.dataLabelContextMenu2.Size = new System.Drawing.Size(193, 26);
			// 
			// addANewDataSectionToolStripMenuItem1
			// 
			this.addANewDataSectionToolStripMenuItem1.Name = "addANewDataSectionToolStripMenuItem1";
			this.addANewDataSectionToolStripMenuItem1.Size = new System.Drawing.Size(192, 22);
			this.addANewDataSectionToolStripMenuItem1.Text = "Add New Data Section";
			this.addANewDataSectionToolStripMenuItem1.Click += new System.EventHandler(this.addANewDataSectionToolStripMenuItem_Click);
			// 
			// backgroundWorker1
			// 
			this.backgroundWorker1.WorkerReportsProgress = true;
			this.backgroundWorker1.WorkerSupportsCancellation = true;
			this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
			this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
			this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
			// 
			// progressLabel
			// 
			this.progressLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.progressLabel.AutoSize = true;
			this.progressLabel.BackColor = System.Drawing.SystemColors.Control;
			this.progressLabel.Location = new System.Drawing.Point(301, 290);
			this.progressLabel.Name = "progressLabel";
			this.progressLabel.Size = new System.Drawing.Size(0, 13);
			this.progressLabel.TabIndex = 22;
			// 
			// varLabelBoxContextMenu
			// 
			this.varLabelBoxContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editVariableToolStripMenuItem2,
            this.removeVariableToolStripMenuItem3});
			this.varLabelBoxContextMenu.Name = "functionBoxContextMenu";
			this.varLabelBoxContextMenu.Size = new System.Drawing.Size(118, 48);
			// 
			// editVariableToolStripMenuItem2
			// 
			this.editVariableToolStripMenuItem2.Name = "editVariableToolStripMenuItem2";
			this.editVariableToolStripMenuItem2.Size = new System.Drawing.Size(117, 22);
			this.editVariableToolStripMenuItem2.Text = "Edit";
			this.editVariableToolStripMenuItem2.Click += new System.EventHandler(this.editVariableToolStripMenuItem2_Click);
			// 
			// removeVariableToolStripMenuItem3
			// 
			this.removeVariableToolStripMenuItem3.Name = "removeVariableToolStripMenuItem3";
			this.removeVariableToolStripMenuItem3.Size = new System.Drawing.Size(117, 22);
			this.removeVariableToolStripMenuItem3.Text = "Remove";
			this.removeVariableToolStripMenuItem3.Click += new System.EventHandler(this.removeVariableToolStripMenuItem3_Click);
			// 
			// varLabelBoxContextMenu2
			// 
			this.varLabelBoxContextMenu2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addVariableToolStripMenuItem2});
			this.varLabelBoxContextMenu2.Name = "functionBoxContextMenu2";
			this.varLabelBoxContextMenu2.Size = new System.Drawing.Size(169, 26);
			// 
			// addVariableToolStripMenuItem2
			// 
			this.addVariableToolStripMenuItem2.Name = "addVariableToolStripMenuItem2";
			this.addVariableToolStripMenuItem2.Size = new System.Drawing.Size(168, 22);
			this.addVariableToolStripMenuItem2.Text = "Add New Variable";
			this.addVariableToolStripMenuItem2.Click += new System.EventHandler(this.addVariableToolStripMenuItem_Click);
			// 
			// addButton
			// 
			this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.addButton.BackColor = System.Drawing.SystemColors.Control;
			this.addButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.addButton.Location = new System.Drawing.Point(426, 285);
			this.addButton.Name = "addButton";
			this.addButton.Size = new System.Drawing.Size(83, 23);
			this.addButton.TabIndex = 5;
			this.addButton.Text = "Add Label";
			this.addButton.UseVisualStyleBackColor = false;
			this.addButton.Click += new System.EventHandler(this.addButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.BackColor = System.Drawing.SystemColors.Control;
			this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.cancelButton.Location = new System.Drawing.Point(608, 285);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(83, 23);
			this.cancelButton.TabIndex = 7;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = false;
			this.cancelButton.Visible = false;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// editButton
			// 
			this.editButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.editButton.BackColor = System.Drawing.SystemColors.Control;
			this.editButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.editButton.Location = new System.Drawing.Point(517, 285);
			this.editButton.Name = "editButton";
			this.editButton.Size = new System.Drawing.Size(83, 23);
			this.editButton.TabIndex = 6;
			this.editButton.Text = "Edit Label";
			this.editButton.UseVisualStyleBackColor = false;
			this.editButton.Click += new System.EventHandler(this.editButton_Click);
			// 
			// mainTextBox
			// 
			this.mainTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.mainTextBox.AutoScrollMinSize = new System.Drawing.Size(2, 14);
			this.mainTextBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.mainTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.mainTextBox.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.mainTextBox.Location = new System.Drawing.Point(12, 27);
			this.mainTextBox.Name = "mainTextBox";
			this.mainTextBox.ShowLineNumbers = false;
			this.mainTextBox.Size = new System.Drawing.Size(408, 240);
			this.mainTextBox.TabIndex = 3;
			// 
			// varLabelTabPage
			// 
			this.varLabelTabPage.Controls.Add(this.varLabelCommentLabel);
			this.varLabelTabPage.Controls.Add(this.varLabelCommentBox);
			this.varLabelTabPage.Controls.Add(this.varLabelNameBox);
			this.varLabelTabPage.Controls.Add(this.varLabelOffsetBox);
			this.varLabelTabPage.Controls.Add(this.varLabelOffsetLabel);
			this.varLabelTabPage.Controls.Add(this.varLabelNameLabel);
			this.varLabelTabPage.Controls.Add(this.varLabelBox);
			this.varLabelTabPage.Location = new System.Drawing.Point(4, 22);
			this.varLabelTabPage.Name = "varLabelTabPage";
			this.varLabelTabPage.Size = new System.Drawing.Size(258, 214);
			this.varLabelTabPage.TabIndex = 2;
			this.varLabelTabPage.Text = "Variables";
			this.varLabelTabPage.UseVisualStyleBackColor = true;
			// 
			// varLabelCommentLabel
			// 
			this.varLabelCommentLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.varLabelCommentLabel.AutoSize = true;
			this.varLabelCommentLabel.Location = new System.Drawing.Point(3, 78);
			this.varLabelCommentLabel.Name = "varLabelCommentLabel";
			this.varLabelCommentLabel.Size = new System.Drawing.Size(51, 13);
			this.varLabelCommentLabel.TabIndex = 25;
			this.varLabelCommentLabel.Text = "Comment";
			this.varLabelCommentLabel.Visible = false;
			// 
			// varLabelCommentBox
			// 
			this.varLabelCommentBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.varLabelCommentBox.Location = new System.Drawing.Point(3, 94);
			this.varLabelCommentBox.Multiline = true;
			this.varLabelCommentBox.Name = "varLabelCommentBox";
			this.varLabelCommentBox.Size = new System.Drawing.Size(252, 114);
			this.varLabelCommentBox.TabIndex = 3;
			this.varLabelCommentBox.Visible = false;
			// 
			// varLabelNameBox
			// 
			this.varLabelNameBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.varLabelNameBox.Location = new System.Drawing.Point(128, 16);
			this.varLabelNameBox.Name = "varLabelNameBox";
			this.varLabelNameBox.Size = new System.Drawing.Size(124, 20);
			this.varLabelNameBox.TabIndex = 2;
			this.varLabelNameBox.Visible = false;
			this.varLabelNameBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.nameBox_KeyPress);
			// 
			// varLabelOffsetBox
			// 
			this.varLabelOffsetBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.varLabelOffsetBox.Location = new System.Drawing.Point(3, 16);
			this.varLabelOffsetBox.Name = "varLabelOffsetBox";
			this.varLabelOffsetBox.Size = new System.Drawing.Size(119, 20);
			this.varLabelOffsetBox.TabIndex = 1;
			this.varLabelOffsetBox.Visible = false;
			this.varLabelOffsetBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.lengthBox_KeyPress);
			// 
			// varLabelOffsetLabel
			// 
			this.varLabelOffsetLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.varLabelOffsetLabel.AutoSize = true;
			this.varLabelOffsetLabel.Location = new System.Drawing.Point(3, 3);
			this.varLabelOffsetLabel.Name = "varLabelOffsetLabel";
			this.varLabelOffsetLabel.Size = new System.Drawing.Size(60, 13);
			this.varLabelOffsetLabel.TabIndex = 28;
			this.varLabelOffsetLabel.Text = "Value (hex)";
			this.varLabelOffsetLabel.Visible = false;
			// 
			// varLabelNameLabel
			// 
			this.varLabelNameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.varLabelNameLabel.AutoSize = true;
			this.varLabelNameLabel.Location = new System.Drawing.Point(125, 3);
			this.varLabelNameLabel.Name = "varLabelNameLabel";
			this.varLabelNameLabel.Size = new System.Drawing.Size(35, 13);
			this.varLabelNameLabel.TabIndex = 27;
			this.varLabelNameLabel.Text = "Name";
			this.varLabelNameLabel.Visible = false;
			// 
			// varLabelBox
			// 
			this.varLabelBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.varLabelBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.varLabelBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.varLabelBox.FormattingEnabled = true;
			this.varLabelBox.ItemHeight = 11;
			this.varLabelBox.Location = new System.Drawing.Point(3, 3);
			this.varLabelBox.Name = "varLabelBox";
			this.varLabelBox.Size = new System.Drawing.Size(252, 202);
			this.varLabelBox.TabIndex = 0;
			this.varLabelBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.varLabelBox_MouseClick);
			this.varLabelBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.varLabelBox_DrawItem);
			this.varLabelBox.DoubleClick += new System.EventHandler(this.varLabelBox_DoubleClick);
			this.varLabelBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.varLabelBox_KeyDown);
			this.varLabelBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.varLabelBox_MouseClick);
			// 
			// dataLabelTabPage
			// 
			this.dataLabelTabPage.Controls.Add(this.dataLabelRowLengthLabel);
			this.dataLabelTabPage.Controls.Add(this.dataLabelRowLengthBox);
			this.dataLabelTabPage.Controls.Add(this.dataLabelCommentBox);
			this.dataLabelTabPage.Controls.Add(this.dataLabelOffsetBox);
			this.dataLabelTabPage.Controls.Add(this.dataLabelNameBox);
			this.dataLabelTabPage.Controls.Add(this.dataLabelLengthBox);
			this.dataLabelTabPage.Controls.Add(this.dataLabelDataTypeBox);
			this.dataLabelTabPage.Controls.Add(this.dataLabelDataTypeLabel);
			this.dataLabelTabPage.Controls.Add(this.dataLabelOffsetLabel);
			this.dataLabelTabPage.Controls.Add(this.dataLabelNameLabel);
			this.dataLabelTabPage.Controls.Add(this.dataLabelLengthLabel);
			this.dataLabelTabPage.Controls.Add(this.dataLabelCommentLabel);
			this.dataLabelTabPage.Controls.Add(this.dataLabelBox);
			this.dataLabelTabPage.Location = new System.Drawing.Point(4, 22);
			this.dataLabelTabPage.Name = "dataLabelTabPage";
			this.dataLabelTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.dataLabelTabPage.Size = new System.Drawing.Size(258, 214);
			this.dataLabelTabPage.TabIndex = 1;
			this.dataLabelTabPage.Text = "Data";
			this.dataLabelTabPage.UseVisualStyleBackColor = true;
			// 
			// dataLabelRowLengthLabel
			// 
			this.dataLabelRowLengthLabel.AutoSize = true;
			this.dataLabelRowLengthLabel.Location = new System.Drawing.Point(162, 39);
			this.dataLabelRowLengthLabel.Name = "dataLabelRowLengthLabel";
			this.dataLabelRowLengthLabel.Size = new System.Drawing.Size(78, 13);
			this.dataLabelRowLengthLabel.TabIndex = 31;
			this.dataLabelRowLengthLabel.Text = "Data Row Size";
			this.dataLabelRowLengthLabel.Visible = false;
			// 
			// dataLabelRowLengthBox
			// 
			this.dataLabelRowLengthBox.Location = new System.Drawing.Point(165, 55);
			this.dataLabelRowLengthBox.Name = "dataLabelRowLengthBox";
			this.dataLabelRowLengthBox.Size = new System.Drawing.Size(87, 20);
			this.dataLabelRowLengthBox.TabIndex = 5;
			this.dataLabelRowLengthBox.Visible = false;
			this.dataLabelRowLengthBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dataLabelRowLengthBox_KeyPress);
			// 
			// dataLabelCommentBox
			// 
			this.dataLabelCommentBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dataLabelCommentBox.Location = new System.Drawing.Point(3, 94);
			this.dataLabelCommentBox.Multiline = true;
			this.dataLabelCommentBox.Name = "dataLabelCommentBox";
			this.dataLabelCommentBox.Size = new System.Drawing.Size(252, 114);
			this.dataLabelCommentBox.TabIndex = 6;
			this.dataLabelCommentBox.Visible = false;
			// 
			// dataLabelOffsetBox
			// 
			this.dataLabelOffsetBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.dataLabelOffsetBox.Location = new System.Drawing.Point(3, 16);
			this.dataLabelOffsetBox.Name = "dataLabelOffsetBox";
			this.dataLabelOffsetBox.Size = new System.Drawing.Size(119, 20);
			this.dataLabelOffsetBox.TabIndex = 1;
			this.dataLabelOffsetBox.Visible = false;
			this.dataLabelOffsetBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.offsetBox_KeyPress);
			// 
			// dataLabelNameBox
			// 
			this.dataLabelNameBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.dataLabelNameBox.Location = new System.Drawing.Point(128, 16);
			this.dataLabelNameBox.Name = "dataLabelNameBox";
			this.dataLabelNameBox.Size = new System.Drawing.Size(124, 20);
			this.dataLabelNameBox.TabIndex = 2;
			this.dataLabelNameBox.Visible = false;
			this.dataLabelNameBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.nameBox_KeyPress);
			// 
			// dataLabelLengthBox
			// 
			this.dataLabelLengthBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.dataLabelLengthBox.Location = new System.Drawing.Point(3, 55);
			this.dataLabelLengthBox.Name = "dataLabelLengthBox";
			this.dataLabelLengthBox.Size = new System.Drawing.Size(74, 20);
			this.dataLabelLengthBox.TabIndex = 3;
			this.dataLabelLengthBox.Visible = false;
			this.dataLabelLengthBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.lengthBox_KeyPress);
			// 
			// dataLabelDataTypeBox
			// 
			this.dataLabelDataTypeBox.FormattingEnabled = true;
			this.dataLabelDataTypeBox.Items.AddRange(new object[] {
            "Data",
            "Image"});
			this.dataLabelDataTypeBox.Location = new System.Drawing.Point(83, 54);
			this.dataLabelDataTypeBox.Name = "dataLabelDataTypeBox";
			this.dataLabelDataTypeBox.Size = new System.Drawing.Size(76, 21);
			this.dataLabelDataTypeBox.TabIndex = 4;
			this.dataLabelDataTypeBox.Visible = false;
			// 
			// dataLabelDataTypeLabel
			// 
			this.dataLabelDataTypeLabel.AutoSize = true;
			this.dataLabelDataTypeLabel.Location = new System.Drawing.Point(80, 39);
			this.dataLabelDataTypeLabel.Name = "dataLabelDataTypeLabel";
			this.dataLabelDataTypeLabel.Size = new System.Drawing.Size(57, 13);
			this.dataLabelDataTypeLabel.TabIndex = 29;
			this.dataLabelDataTypeLabel.Text = "Data Type";
			this.dataLabelDataTypeLabel.Visible = false;
			// 
			// dataLabelOffsetLabel
			// 
			this.dataLabelOffsetLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.dataLabelOffsetLabel.AutoSize = true;
			this.dataLabelOffsetLabel.Location = new System.Drawing.Point(3, 3);
			this.dataLabelOffsetLabel.Name = "dataLabelOffsetLabel";
			this.dataLabelOffsetLabel.Size = new System.Drawing.Size(61, 13);
			this.dataLabelOffsetLabel.TabIndex = 26;
			this.dataLabelOffsetLabel.Text = "Offset (hex)";
			this.dataLabelOffsetLabel.Visible = false;
			// 
			// dataLabelNameLabel
			// 
			this.dataLabelNameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.dataLabelNameLabel.AutoSize = true;
			this.dataLabelNameLabel.Location = new System.Drawing.Point(125, 3);
			this.dataLabelNameLabel.Name = "dataLabelNameLabel";
			this.dataLabelNameLabel.Size = new System.Drawing.Size(35, 13);
			this.dataLabelNameLabel.TabIndex = 25;
			this.dataLabelNameLabel.Text = "Name";
			this.dataLabelNameLabel.Visible = false;
			// 
			// dataLabelLengthLabel
			// 
			this.dataLabelLengthLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.dataLabelLengthLabel.AutoSize = true;
			this.dataLabelLengthLabel.Location = new System.Drawing.Point(3, 39);
			this.dataLabelLengthLabel.Name = "dataLabelLengthLabel";
			this.dataLabelLengthLabel.Size = new System.Drawing.Size(40, 13);
			this.dataLabelLengthLabel.TabIndex = 27;
			this.dataLabelLengthLabel.Text = "Length";
			this.dataLabelLengthLabel.Visible = false;
			// 
			// dataLabelCommentLabel
			// 
			this.dataLabelCommentLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dataLabelCommentLabel.AutoSize = true;
			this.dataLabelCommentLabel.Location = new System.Drawing.Point(3, 78);
			this.dataLabelCommentLabel.Name = "dataLabelCommentLabel";
			this.dataLabelCommentLabel.Size = new System.Drawing.Size(51, 13);
			this.dataLabelCommentLabel.TabIndex = 28;
			this.dataLabelCommentLabel.Text = "Comment";
			this.dataLabelCommentLabel.Visible = false;
			// 
			// dataLabelBox
			// 
			this.dataLabelBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dataLabelBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.dataLabelBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.dataLabelBox.FormattingEnabled = true;
			this.dataLabelBox.ItemHeight = 11;
			this.dataLabelBox.Location = new System.Drawing.Point(3, 3);
			this.dataLabelBox.Name = "dataLabelBox";
			this.dataLabelBox.Size = new System.Drawing.Size(252, 202);
			this.dataLabelBox.TabIndex = 0;
			this.dataLabelBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dataLabelBox_MouseClick);
			this.dataLabelBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.dataLabelBox_DrawItem);
			this.dataLabelBox.DoubleClick += new System.EventHandler(this.dataLabelBox_DoubleClick);
			this.dataLabelBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataLabelBox_KeyDown);
			this.dataLabelBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataLabelBox_MouseClick);
			// 
			// functionLabelTabPage
			// 
			this.functionLabelTabPage.Controls.Add(this.codeLabelCommentLabel);
			this.functionLabelTabPage.Controls.Add(this.codeLabelLengthLabel);
			this.functionLabelTabPage.Controls.Add(this.codeLabelOffsetLabel);
			this.functionLabelTabPage.Controls.Add(this.codeLabelNameLabel);
			this.functionLabelTabPage.Controls.Add(this.codeLabelOffsetBox);
			this.functionLabelTabPage.Controls.Add(this.codeLabelCommentBox);
			this.functionLabelTabPage.Controls.Add(this.codeLabelLengthBox);
			this.functionLabelTabPage.Controls.Add(this.codeLabelNameBox);
			this.functionLabelTabPage.Controls.Add(this.codeLabelBox);
			this.functionLabelTabPage.Location = new System.Drawing.Point(4, 22);
			this.functionLabelTabPage.Name = "functionLabelTabPage";
			this.functionLabelTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.functionLabelTabPage.Size = new System.Drawing.Size(258, 214);
			this.functionLabelTabPage.TabIndex = 0;
			this.functionLabelTabPage.Text = "Functions";
			this.functionLabelTabPage.UseVisualStyleBackColor = true;
			// 
			// codeLabelCommentLabel
			// 
			this.codeLabelCommentLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.codeLabelCommentLabel.AutoSize = true;
			this.codeLabelCommentLabel.Location = new System.Drawing.Point(3, 78);
			this.codeLabelCommentLabel.Name = "codeLabelCommentLabel";
			this.codeLabelCommentLabel.Size = new System.Drawing.Size(51, 13);
			this.codeLabelCommentLabel.TabIndex = 14;
			this.codeLabelCommentLabel.Text = "Comment";
			this.codeLabelCommentLabel.Visible = false;
			// 
			// codeLabelLengthLabel
			// 
			this.codeLabelLengthLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.codeLabelLengthLabel.AutoSize = true;
			this.codeLabelLengthLabel.Location = new System.Drawing.Point(3, 39);
			this.codeLabelLengthLabel.Name = "codeLabelLengthLabel";
			this.codeLabelLengthLabel.Size = new System.Drawing.Size(40, 13);
			this.codeLabelLengthLabel.TabIndex = 13;
			this.codeLabelLengthLabel.Text = "Length";
			this.codeLabelLengthLabel.Visible = false;
			// 
			// codeLabelOffsetLabel
			// 
			this.codeLabelOffsetLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.codeLabelOffsetLabel.AutoSize = true;
			this.codeLabelOffsetLabel.Location = new System.Drawing.Point(3, 3);
			this.codeLabelOffsetLabel.Name = "codeLabelOffsetLabel";
			this.codeLabelOffsetLabel.Size = new System.Drawing.Size(61, 13);
			this.codeLabelOffsetLabel.TabIndex = 12;
			this.codeLabelOffsetLabel.Text = "Offset (hex)";
			this.codeLabelOffsetLabel.Visible = false;
			// 
			// codeLabelNameLabel
			// 
			this.codeLabelNameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.codeLabelNameLabel.AutoSize = true;
			this.codeLabelNameLabel.Location = new System.Drawing.Point(125, 3);
			this.codeLabelNameLabel.Name = "codeLabelNameLabel";
			this.codeLabelNameLabel.Size = new System.Drawing.Size(35, 13);
			this.codeLabelNameLabel.TabIndex = 11;
			this.codeLabelNameLabel.Text = "Name";
			this.codeLabelNameLabel.Visible = false;
			// 
			// codeLabelOffsetBox
			// 
			this.codeLabelOffsetBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.codeLabelOffsetBox.Location = new System.Drawing.Point(3, 16);
			this.codeLabelOffsetBox.Name = "codeLabelOffsetBox";
			this.codeLabelOffsetBox.Size = new System.Drawing.Size(119, 20);
			this.codeLabelOffsetBox.TabIndex = 1;
			this.codeLabelOffsetBox.Visible = false;
			this.codeLabelOffsetBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.offsetBox_KeyPress);
			// 
			// codeLabelCommentBox
			// 
			this.codeLabelCommentBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.codeLabelCommentBox.Location = new System.Drawing.Point(3, 94);
			this.codeLabelCommentBox.Multiline = true;
			this.codeLabelCommentBox.Name = "codeLabelCommentBox";
			this.codeLabelCommentBox.Size = new System.Drawing.Size(252, 114);
			this.codeLabelCommentBox.TabIndex = 4;
			this.codeLabelCommentBox.Visible = false;
			// 
			// codeLabelLengthBox
			// 
			this.codeLabelLengthBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.codeLabelLengthBox.Location = new System.Drawing.Point(3, 55);
			this.codeLabelLengthBox.Name = "codeLabelLengthBox";
			this.codeLabelLengthBox.Size = new System.Drawing.Size(119, 20);
			this.codeLabelLengthBox.TabIndex = 3;
			this.codeLabelLengthBox.Visible = false;
			this.codeLabelLengthBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.lengthBox_KeyPress);
			// 
			// codeLabelNameBox
			// 
			this.codeLabelNameBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.codeLabelNameBox.Location = new System.Drawing.Point(128, 16);
			this.codeLabelNameBox.Name = "codeLabelNameBox";
			this.codeLabelNameBox.Size = new System.Drawing.Size(124, 20);
			this.codeLabelNameBox.TabIndex = 2;
			this.codeLabelNameBox.Visible = false;
			this.codeLabelNameBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.nameBox_KeyPress);
			// 
			// codeLabelBox
			// 
			this.codeLabelBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.codeLabelBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.codeLabelBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.codeLabelBox.FormattingEnabled = true;
			this.codeLabelBox.ItemHeight = 11;
			this.codeLabelBox.Location = new System.Drawing.Point(3, 3);
			this.codeLabelBox.Name = "codeLabelBox";
			this.codeLabelBox.Size = new System.Drawing.Size(252, 202);
			this.codeLabelBox.TabIndex = 0;
			this.codeLabelBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.codeLabelBox_MouseClick);
			this.codeLabelBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.codeLabelBox_DrawItem);
			this.codeLabelBox.DoubleClick += new System.EventHandler(this.codeLabelBox_DoubleClick);
			this.codeLabelBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.codeLabelBox_KeyDown);
			this.codeLabelBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.codeLabelBox_MouseClick);
			// 
			// LabelTabControl
			// 
			this.LabelTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.LabelTabControl.Controls.Add(this.functionLabelTabPage);
			this.LabelTabControl.Controls.Add(this.dataLabelTabPage);
			this.LabelTabControl.Controls.Add(this.varLabelTabPage);
			this.LabelTabControl.Location = new System.Drawing.Point(426, 27);
			this.LabelTabControl.Name = "LabelTabControl";
			this.LabelTabControl.SelectedIndex = 0;
			this.LabelTabControl.Size = new System.Drawing.Size(266, 240);
			this.LabelTabControl.TabIndex = 4;
			this.LabelTabControl.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.LabelTabControl_Selecting);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(704, 322);
			this.Controls.Add(this.menuStrip1);
			this.Controls.Add(this.progressLabel);
			this.Controls.Add(this.mainTextBox);
			this.Controls.Add(this.LabelTabControl);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.editButton);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.startBox);
			this.Controls.Add(this.addButton);
			this.Controls.Add(this.printASMButton);
			this.Controls.Add(this.endBox);
			this.Controls.Add(this.label2);
			this.MainMenuStrip = this.menuStrip1;
			this.MinimumSize = new System.Drawing.Size(658, 300);
			this.Name = "MainForm";
			this.Text = "GBRead";
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.codeLabelBoxContextMenu.ResumeLayout(false);
			this.codeLabelBoxContextMenu2.ResumeLayout(false);
			this.dataLabelBoxContextMenu.ResumeLayout(false);
			this.dataLabelContextMenu2.ResumeLayout(false);
			this.varLabelBoxContextMenu.ResumeLayout(false);
			this.varLabelBoxContextMenu2.ResumeLayout(false);
			this.varLabelTabPage.ResumeLayout(false);
			this.varLabelTabPage.PerformLayout();
			this.dataLabelTabPage.ResumeLayout(false);
			this.dataLabelTabPage.PerformLayout();
			this.functionLabelTabPage.ResumeLayout(false);
			this.functionLabelTabPage.PerformLayout();
			this.LabelTabControl.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.TextBox startBox;
        private System.Windows.Forms.TextBox endBox;
        private System.Windows.Forms.Button printASMButton;
        private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStripMenuItem saveCalledFunctionsListToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ContextMenuStrip codeLabelBoxContextMenu;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadFunctionListToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem renameLabelToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip codeLabelBoxContextMenu2;
        private System.Windows.Forms.ToolStripMenuItem addANewLabelToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ContextMenuStrip dataLabelBoxContextMenu;
        private System.Windows.Forms.ToolStripMenuItem renameADataSectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem1;
        private System.Windows.Forms.ContextMenuStrip dataLabelContextMenu2;
        private System.Windows.Forms.ToolStripMenuItem addANewDataSectionToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem exportToBinaryFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveEntireFileASMToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
		private System.Windows.Forms.Label progressLabel;
		private System.Windows.Forms.ContextMenuStrip varLabelBoxContextMenu;
        private System.Windows.Forms.ToolStripMenuItem editVariableToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem removeVariableToolStripMenuItem3;
        private System.Windows.Forms.ContextMenuStrip varLabelBoxContextMenu2;
        private System.Windows.Forms.ToolStripMenuItem addVariableToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem insertToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertASMAtLocationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveChangedFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem addAllCalledFunctionsToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem insertExternalBinaryAtLocationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem searchForFunctionsThatCallThisOneToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem1;
        private System.Windows.Forms.Button addButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button editButton;
		private SyntaxHighlightingTextBox mainTextBox;
		private System.Windows.Forms.TabPage varLabelTabPage;
		private System.Windows.Forms.Label varLabelCommentLabel;
		private System.Windows.Forms.TextBox varLabelCommentBox;
		private System.Windows.Forms.TextBox varLabelNameBox;
		private System.Windows.Forms.TextBox varLabelOffsetBox;
		private System.Windows.Forms.Label varLabelOffsetLabel;
		private System.Windows.Forms.Label varLabelNameLabel;
		private System.Windows.Forms.ListBox varLabelBox;
		private System.Windows.Forms.TabPage dataLabelTabPage;
		private System.Windows.Forms.Label dataLabelRowLengthLabel;
		private System.Windows.Forms.TextBox dataLabelRowLengthBox;
		private System.Windows.Forms.TextBox dataLabelCommentBox;
		private System.Windows.Forms.TextBox dataLabelOffsetBox;
		private System.Windows.Forms.TextBox dataLabelNameBox;
		private System.Windows.Forms.TextBox dataLabelLengthBox;
		private System.Windows.Forms.ComboBox dataLabelDataTypeBox;
		private System.Windows.Forms.Label dataLabelDataTypeLabel;
		private System.Windows.Forms.Label dataLabelOffsetLabel;
		private System.Windows.Forms.Label dataLabelNameLabel;
		private System.Windows.Forms.Label dataLabelLengthLabel;
		private System.Windows.Forms.Label dataLabelCommentLabel;
		private System.Windows.Forms.ListBox dataLabelBox;
		private System.Windows.Forms.TabPage functionLabelTabPage;
		private System.Windows.Forms.Label codeLabelCommentLabel;
		private System.Windows.Forms.Label codeLabelLengthLabel;
		private System.Windows.Forms.Label codeLabelOffsetLabel;
		private System.Windows.Forms.Label codeLabelNameLabel;
		private System.Windows.Forms.TextBox codeLabelOffsetBox;
		private System.Windows.Forms.TextBox codeLabelCommentBox;
		private System.Windows.Forms.TextBox codeLabelLengthBox;
		private System.Windows.Forms.TextBox codeLabelNameBox;
		private System.Windows.Forms.ListBox codeLabelBox;
		private System.Windows.Forms.TabControl LabelTabControl;
		private System.Windows.Forms.ToolStripMenuItem findReferencesToolStripMenuItem;
    }
}

