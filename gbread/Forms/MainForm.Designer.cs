namespace GBRead.Forms
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
            this.startBox = new System.Windows.Forms.TextBox();
            this.endBox = new System.Windows.Forms.TextBox();
            this.printASMButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.funcLabelBoxContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameLabelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchForFunctionsThatCallThisOneToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.funcLabelBoxContextMenu2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addANewLabelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataLabelBoxContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.renameADataSectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.findReferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToBinaryFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataLabelContextMenu2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addANewDataSectionToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.progressLabel = new System.Windows.Forms.Label();
            this.varLabelBoxContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.editVariableToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.removeVariableToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.varLabelBoxContextMenu2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addVariableToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.dataLabelTabPage = new System.Windows.Forms.TabPage();
            this.dataLabelBox = new System.Windows.Forms.ListBox();
            this.functionLabelTabPage = new System.Windows.Forms.TabPage();
            this.funcLabelBox = new System.Windows.Forms.ListBox();
            this.LabelTabControl = new System.Windows.Forms.TabControl();
            this.varLabelTabPage = new System.Windows.Forms.TabPage();
            this.varLabelBox = new System.Windows.Forms.ListBox();
            this.addNewButton = new System.Windows.Forms.Button();
            this.mainTextBox = new GBRead.SyntaxHighlightingTextBox();
            this.menuStrip1.SuspendLayout();
            this.funcLabelBoxContextMenu.SuspendLayout();
            this.funcLabelBoxContextMenu2.SuspendLayout();
            this.dataLabelBoxContextMenu.SuspendLayout();
            this.dataLabelContextMenu2.SuspendLayout();
            this.varLabelBoxContextMenu.SuspendLayout();
            this.varLabelBoxContextMenu2.SuspendLayout();
            this.dataLabelTabPage.SuspendLayout();
            this.functionLabelTabPage.SuspendLayout();
            this.LabelTabControl.SuspendLayout();
            this.varLabelTabPage.SuspendLayout();
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
            this.openToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
            this.openToolStripMenuItem.Text = "&Open GB/C File";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveChangedFileToolStripMenuItem
            // 
            this.saveChangedFileToolStripMenuItem.Name = "saveChangedFileToolStripMenuItem";
            this.saveChangedFileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveChangedFileToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
            this.saveChangedFileToolStripMenuItem.Text = "&Save GB/C File...";
            this.saveChangedFileToolStripMenuItem.Click += new System.EventHandler(this.saveChangedFileToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(246, 6);
            // 
            // loadFunctionListToolStripMenuItem
            // 
            this.loadFunctionListToolStripMenuItem.Name = "loadFunctionListToolStripMenuItem";
            this.loadFunctionListToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.loadFunctionListToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
            this.loadFunctionListToolStripMenuItem.Text = "&Load Labels and Variables";
            this.loadFunctionListToolStripMenuItem.Click += new System.EventHandler(this.loadLabelsToolStripMenuItem_Click);
            // 
            // saveCalledFunctionsListToolStripMenuItem
            // 
            this.saveCalledFunctionsListToolStripMenuItem.Name = "saveCalledFunctionsListToolStripMenuItem";
            this.saveCalledFunctionsListToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
            this.saveCalledFunctionsListToolStripMenuItem.Text = "Save Labels and Variables";
            this.saveCalledFunctionsListToolStripMenuItem.Click += new System.EventHandler(this.saveLabelsToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(246, 6);
            // 
            // saveEntireFileASMToolStripMenuItem
            // 
            this.saveEntireFileASMToolStripMenuItem.Name = "saveEntireFileASMToolStripMenuItem";
            this.saveEntireFileASMToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
            this.saveEntireFileASMToolStripMenuItem.Text = "Save &Entire File ASM";
            this.saveEntireFileASMToolStripMenuItem.Click += new System.EventHandler(this.saveEntireFileASMToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(246, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
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
            // startBox
            // 
            this.startBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.startBox.Location = new System.Drawing.Point(12, 329);
            this.startBox.Name = "startBox";
            this.startBox.Size = new System.Drawing.Size(106, 20);
            this.startBox.TabIndex = 0;
            this.startBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.startEndBox_KeyPress);
            // 
            // endBox
            // 
            this.endBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.endBox.Location = new System.Drawing.Point(124, 329);
            this.endBox.Name = "endBox";
            this.endBox.Size = new System.Drawing.Size(103, 20);
            this.endBox.TabIndex = 1;
            this.endBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.startEndBox_KeyPress);
            // 
            // printASMButton
            // 
            this.printASMButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.printASMButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.printASMButton.Location = new System.Drawing.Point(233, 326);
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
            this.label1.Location = new System.Drawing.Point(12, 313);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Start Position (in hex)";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(121, 313);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "End Position (in hex)";
            // 
            // funcLabelBoxContextMenu
            // 
            this.funcLabelBoxContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.renameLabelToolStripMenuItem,
            this.removeToolStripMenuItem,
            this.searchForFunctionsThatCallThisOneToolStripMenuItem1});
            this.funcLabelBoxContextMenu.Name = "functionBoxContextMenu";
            this.funcLabelBoxContextMenu.Size = new System.Drawing.Size(167, 92);
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.addToolStripMenuItem.Text = "Add New Label";
            this.addToolStripMenuItem.Click += new System.EventHandler(this.addFuncLabelMenuItem_Click);
            // 
            // renameLabelToolStripMenuItem
            // 
            this.renameLabelToolStripMenuItem.Name = "renameLabelToolStripMenuItem";
            this.renameLabelToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.renameLabelToolStripMenuItem.Text = "Edit";
            this.renameLabelToolStripMenuItem.Click += new System.EventHandler(this.renameFuncLabelMenuItem_Click);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeFuncLabelMenuItem_Click);
            // 
            // searchForFunctionsThatCallThisOneToolStripMenuItem1
            // 
            this.searchForFunctionsThatCallThisOneToolStripMenuItem1.Name = "searchForFunctionsThatCallThisOneToolStripMenuItem1";
            this.searchForFunctionsThatCallThisOneToolStripMenuItem1.Size = new System.Drawing.Size(166, 22);
            this.searchForFunctionsThatCallThisOneToolStripMenuItem1.Text = "Find References...";
            this.searchForFunctionsThatCallThisOneToolStripMenuItem1.Click += new System.EventHandler(this.searchForFunctionsThatCallThisOneToolStripMenuItem_Click);
            // 
            // funcLabelBoxContextMenu2
            // 
            this.funcLabelBoxContextMenu2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addANewLabelToolStripMenuItem});
            this.funcLabelBoxContextMenu2.Name = "functionBoxContextMenu2";
            this.funcLabelBoxContextMenu2.Size = new System.Drawing.Size(155, 26);
            // 
            // addANewLabelToolStripMenuItem
            // 
            this.addANewLabelToolStripMenuItem.Name = "addANewLabelToolStripMenuItem";
            this.addANewLabelToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.addANewLabelToolStripMenuItem.Text = "Add New Label";
            this.addANewLabelToolStripMenuItem.Click += new System.EventHandler(this.addFuncLabelMenuItem_Click);
            // 
            // dataLabelBoxContextMenu
            // 
            this.dataLabelBoxContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem1,
            this.renameADataSectionToolStripMenuItem,
            this.removeToolStripMenuItem1,
            this.findReferencesToolStripMenuItem,
            this.exportToBinaryFileToolStripMenuItem});
            this.dataLabelBoxContextMenu.Name = "dataSectionBoxContextMenu";
            this.dataLabelBoxContextMenu.Size = new System.Drawing.Size(193, 114);
            // 
            // addToolStripMenuItem1
            // 
            this.addToolStripMenuItem1.Name = "addToolStripMenuItem1";
            this.addToolStripMenuItem1.Size = new System.Drawing.Size(192, 22);
            this.addToolStripMenuItem1.Text = "Add New Data Section";
            this.addToolStripMenuItem1.Click += new System.EventHandler(this.addDataLabelMenuItem_Click);
            // 
            // renameADataSectionToolStripMenuItem
            // 
            this.renameADataSectionToolStripMenuItem.Name = "renameADataSectionToolStripMenuItem";
            this.renameADataSectionToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.renameADataSectionToolStripMenuItem.Text = "Edit";
            this.renameADataSectionToolStripMenuItem.Click += new System.EventHandler(this.renameDataLabelMenuItem_Click);
            // 
            // removeToolStripMenuItem1
            // 
            this.removeToolStripMenuItem1.Name = "removeToolStripMenuItem1";
            this.removeToolStripMenuItem1.Size = new System.Drawing.Size(192, 22);
            this.removeToolStripMenuItem1.Text = "Remove";
            this.removeToolStripMenuItem1.Click += new System.EventHandler(this.removeDataLabelMenuItem_Click);
            // 
            // findReferencesToolStripMenuItem
            // 
            this.findReferencesToolStripMenuItem.Name = "findReferencesToolStripMenuItem";
            this.findReferencesToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.findReferencesToolStripMenuItem.Text = "Find References...";
            this.findReferencesToolStripMenuItem.Click += new System.EventHandler(this.findReferencesToolStripMenuItem_Click);
            // 
            // exportToBinaryFileToolStripMenuItem
            // 
            this.exportToBinaryFileToolStripMenuItem.Name = "exportToBinaryFileToolStripMenuItem";
            this.exportToBinaryFileToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
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
            this.addANewDataSectionToolStripMenuItem1.Click += new System.EventHandler(this.addDataLabelMenuItem_Click);
            // 
            // progressLabel
            // 
            this.progressLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.progressLabel.AutoSize = true;
            this.progressLabel.BackColor = System.Drawing.SystemColors.Control;
            this.progressLabel.Location = new System.Drawing.Point(301, 329);
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.Size = new System.Drawing.Size(0, 13);
            this.progressLabel.TabIndex = 22;
            // 
            // varLabelBoxContextMenu
            // 
            this.varLabelBoxContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem2,
            this.editVariableToolStripMenuItem2,
            this.removeVariableToolStripMenuItem3});
            this.varLabelBoxContextMenu.Name = "functionBoxContextMenu";
            this.varLabelBoxContextMenu.Size = new System.Drawing.Size(142, 70);
            // 
            // addToolStripMenuItem2
            // 
            this.addToolStripMenuItem2.Name = "addToolStripMenuItem2";
            this.addToolStripMenuItem2.Size = new System.Drawing.Size(141, 22);
            this.addToolStripMenuItem2.Text = "Add Variable";
            this.addToolStripMenuItem2.Click += new System.EventHandler(this.addVariableToolStripMenuItem_Click);
            // 
            // editVariableToolStripMenuItem2
            // 
            this.editVariableToolStripMenuItem2.Name = "editVariableToolStripMenuItem2";
            this.editVariableToolStripMenuItem2.Size = new System.Drawing.Size(141, 22);
            this.editVariableToolStripMenuItem2.Text = "Edit";
            this.editVariableToolStripMenuItem2.Click += new System.EventHandler(this.editVariableToolStripMenuItem2_Click);
            // 
            // removeVariableToolStripMenuItem3
            // 
            this.removeVariableToolStripMenuItem3.Name = "removeVariableToolStripMenuItem3";
            this.removeVariableToolStripMenuItem3.Size = new System.Drawing.Size(141, 22);
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
            // dataLabelTabPage
            // 
            this.dataLabelTabPage.Controls.Add(this.dataLabelBox);
            this.dataLabelTabPage.Location = new System.Drawing.Point(4, 22);
            this.dataLabelTabPage.Name = "dataLabelTabPage";
            this.dataLabelTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.dataLabelTabPage.Size = new System.Drawing.Size(258, 253);
            this.dataLabelTabPage.TabIndex = 1;
            this.dataLabelTabPage.Text = "Data";
            this.dataLabelTabPage.UseVisualStyleBackColor = true;
            // 
            // dataLabelBox
            // 
            this.dataLabelBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataLabelBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.dataLabelBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataLabelBox.FormattingEnabled = true;
            this.dataLabelBox.HorizontalScrollbar = true;
            this.dataLabelBox.ItemHeight = 11;
            this.dataLabelBox.Location = new System.Drawing.Point(3, 3);
            this.dataLabelBox.Name = "dataLabelBox";
            this.dataLabelBox.Size = new System.Drawing.Size(252, 247);
            this.dataLabelBox.Sorted = true;
            this.dataLabelBox.TabIndex = 0;
            this.dataLabelBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dataLabelBox_MouseClick);
            this.dataLabelBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.dataLabelBox_DrawItem);
            this.dataLabelBox.DoubleClick += new System.EventHandler(this.dataLabelBox_DoubleClick);
            this.dataLabelBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataLabelBox_KeyDown);
            this.dataLabelBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataLabelBox_MouseClick);
            // 
            // functionLabelTabPage
            // 
            this.functionLabelTabPage.Controls.Add(this.funcLabelBox);
            this.functionLabelTabPage.Location = new System.Drawing.Point(4, 22);
            this.functionLabelTabPage.Name = "functionLabelTabPage";
            this.functionLabelTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.functionLabelTabPage.Size = new System.Drawing.Size(258, 253);
            this.functionLabelTabPage.TabIndex = 0;
            this.functionLabelTabPage.Text = "Functions";
            this.functionLabelTabPage.UseVisualStyleBackColor = true;
            // 
            // funcLabelBox
            // 
            this.funcLabelBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.funcLabelBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.funcLabelBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.funcLabelBox.FormattingEnabled = true;
            this.funcLabelBox.HorizontalScrollbar = true;
            this.funcLabelBox.ItemHeight = 11;
            this.funcLabelBox.Location = new System.Drawing.Point(3, 3);
            this.funcLabelBox.Name = "funcLabelBox";
            this.funcLabelBox.Size = new System.Drawing.Size(252, 247);
            this.funcLabelBox.Sorted = true;
            this.funcLabelBox.TabIndex = 0;
            this.funcLabelBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.funcLabelBox_MouseClick);
            this.funcLabelBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.funcLabelBox_DrawItem);
            this.funcLabelBox.DoubleClick += new System.EventHandler(this.funcLabelBox_DoubleClick);
            this.funcLabelBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.funcLabelBox_KeyDown);
            this.funcLabelBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.funcLabelBox_MouseClick);
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
            this.LabelTabControl.Size = new System.Drawing.Size(266, 279);
            this.LabelTabControl.TabIndex = 4;
            // 
            // varLabelTabPage
            // 
            this.varLabelTabPage.Controls.Add(this.varLabelBox);
            this.varLabelTabPage.Location = new System.Drawing.Point(4, 22);
            this.varLabelTabPage.Name = "varLabelTabPage";
            this.varLabelTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.varLabelTabPage.Size = new System.Drawing.Size(258, 253);
            this.varLabelTabPage.TabIndex = 3;
            this.varLabelTabPage.Text = "Variables";
            this.varLabelTabPage.UseVisualStyleBackColor = true;
            // 
            // varLabelBox
            // 
            this.varLabelBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.varLabelBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.varLabelBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.varLabelBox.FormattingEnabled = true;
            this.varLabelBox.HorizontalScrollbar = true;
            this.varLabelBox.ItemHeight = 11;
            this.varLabelBox.Location = new System.Drawing.Point(3, 3);
            this.varLabelBox.Name = "varLabelBox";
            this.varLabelBox.Size = new System.Drawing.Size(252, 247);
            this.varLabelBox.Sorted = true;
            this.varLabelBox.TabIndex = 1;
            this.varLabelBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.varLabelBox_MouseClick);
            this.varLabelBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.varLabelBox_DrawItem);
            this.varLabelBox.DoubleClick += new System.EventHandler(this.varLabelBox_DoubleClick);
            this.varLabelBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.varLabelBox_KeyDown);
            this.varLabelBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.varLabelBox_MouseClick);
            // 
            // addNewButton
            // 
            this.addNewButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.addNewButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addNewButton.Location = new System.Drawing.Point(426, 326);
            this.addNewButton.Name = "addNewButton";
            this.addNewButton.Size = new System.Drawing.Size(75, 23);
            this.addNewButton.TabIndex = 23;
            this.addNewButton.Text = "Add New...";
            this.addNewButton.UseVisualStyleBackColor = true;
            this.addNewButton.Click += new System.EventHandler(this.addNewButton_Click);
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
            this.mainTextBox.ReadOnly = true;
            this.mainTextBox.ShowLineNumbers = false;
            this.mainTextBox.Size = new System.Drawing.Size(408, 279);
            this.mainTextBox.TabIndex = 3;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 361);
            this.Controls.Add(this.addNewButton);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.progressLabel);
            this.Controls.Add(this.mainTextBox);
            this.Controls.Add(this.LabelTabControl);
            this.Controls.Add(this.printASMButton);
            this.Controls.Add(this.endBox);
            this.Controls.Add(this.startBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(658, 300);
            this.Name = "MainForm";
            this.Text = "GBRead";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.funcLabelBoxContextMenu.ResumeLayout(false);
            this.funcLabelBoxContextMenu2.ResumeLayout(false);
            this.dataLabelBoxContextMenu.ResumeLayout(false);
            this.dataLabelContextMenu2.ResumeLayout(false);
            this.varLabelBoxContextMenu.ResumeLayout(false);
            this.varLabelBoxContextMenu2.ResumeLayout(false);
            this.dataLabelTabPage.ResumeLayout(false);
            this.functionLabelTabPage.ResumeLayout(false);
            this.LabelTabControl.ResumeLayout(false);
            this.varLabelTabPage.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.TextBox startBox;
        private System.Windows.Forms.TextBox endBox;
        private System.Windows.Forms.Button printASMButton;
        private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ToolStripMenuItem saveCalledFunctionsListToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip funcLabelBoxContextMenu;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadFunctionListToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem renameLabelToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip funcLabelBoxContextMenu2;
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
		private SyntaxHighlightingTextBox mainTextBox;
		private System.Windows.Forms.TabPage dataLabelTabPage;
		private System.Windows.Forms.ListBox dataLabelBox;
		private System.Windows.Forms.TabPage functionLabelTabPage;
		private System.Windows.Forms.ListBox funcLabelBox;
		private System.Windows.Forms.TabControl LabelTabControl;
		private System.Windows.Forms.ToolStripMenuItem findReferencesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem2;
		private System.Windows.Forms.TabPage varLabelTabPage;
		private System.Windows.Forms.ListBox varLabelBox;
		private System.Windows.Forms.Button addNewButton;
    }
}

