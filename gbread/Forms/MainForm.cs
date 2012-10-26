using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using GBRead.Base;

namespace GBRead.Forms
{
    public partial class MainForm : Form
    {
        private string currentFileLoaded = "";

        private LabelContainer labelContainer;

        private Disassembler disassembler;

        private Assembler assembler;

        private BinFile romFile;

        private MainFormOptions mainFormOptions = new MainFormOptions();

        public MainForm(BinFile cs, Disassembler ds, Assembler ac, LabelContainer lcnew)
        {
            InitializeComponent();
            disassembler = ds;
            assembler = ac;
            labelContainer = lcnew;
            romFile = cs;
            funcLabelBox.DataSource = labelContainer.FuncList;
            dataLabelBox.DataSource = labelContainer.DataList;
            varLabelBox.DataSource = labelContainer.VarList;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
        }

        public void GetOptions(Options options)
        {
            mainTextBox.WordWrap = options.MainForm_WordWrap;
            mainTextBox.SyntaxHighlighter.HighlightComments = options.MainForm_HighlightComments;
            mainTextBox.SyntaxHighlighter.HighlightKeywords = options.MainForm_HighlightKeywords;
            mainTextBox.SyntaxHighlighter.HighlightLabels = options.MainForm_HighlightLabels;
            mainTextBox.SyntaxHighlighter.HighlightNumbers = options.MainForm_HighlightNumbers;
            mainTextBox.SyntaxHighlighter.HighlightOffsets = options.MainForm_HighlightOffsets;
            mainTextBox.SyntaxHighlighter.HighlightRegisters = options.MainForm_HighlightRegisters;
        }

        public void SetOptions(ref Options options)
        {
            options.MainForm_WordWrap = mainTextBox.WordWrap;
            options.MainForm_HighlightComments = mainTextBox.SyntaxHighlighter.HighlightComments;
            options.MainForm_HighlightKeywords = mainTextBox.SyntaxHighlighter.HighlightKeywords;
            options.MainForm_HighlightLabels = mainTextBox.SyntaxHighlighter.HighlightLabels;
            options.MainForm_HighlightNumbers = mainTextBox.SyntaxHighlighter.HighlightNumbers;
            options.MainForm_HighlightOffsets = mainTextBox.SyntaxHighlighter.HighlightOffsets;
            options.MainForm_HighlightRegisters = mainTextBox.SyntaxHighlighter.HighlightRegisters;
        }

        #region Menu Item Handlers

        #region FuncLabelBox Menu

        private void addFuncLabelMenuItem_Click(object sender, EventArgs e)
        {
            if (romFile.FileLoaded)
            {
                AddFunctionLabelForm af = new AddFunctionLabelForm(labelContainer, LabelEditMode.Add);
                af.ShowDialog();
                UpdateFuncBoxView();
            }
            else
            {
                Error.ShowErrorMessage(ErrorMessage.NO_FILE);
            }
        }

        private void renameFuncLabelMenuItem_Click(object sender, EventArgs e)
        {
            AddFunctionLabelForm af = new AddFunctionLabelForm(labelContainer, LabelEditMode.Edit, (FunctionLabel)funcLabelBox.SelectedItem);
            af.ShowDialog();
            UpdateFuncBoxView();
        }

        private void removeFuncLabelMenuItem_Click(object sender, EventArgs e)
        {
            labelContainer.RemoveFuncLabel((FunctionLabel)funcLabelBox.SelectedItem);
            UpdateFuncBoxView();
        }

        #endregion FuncLabelBox Menu

        #region DataLabelBox Menus

        private void addDataLabelMenuItem_Click(object sender, EventArgs e)
        {
            if (romFile.FileLoaded)
            {
                AddDataLabelForm ad = new AddDataLabelForm(labelContainer, LabelEditMode.Add);
                ad.ShowDialog();
                UpdateDataBoxView();
            }
            else
            {
                Error.ShowErrorMessage(ErrorMessage.NO_FILE);
            }
        }

        private void renameDataLabelMenuItem_Click(object sender, EventArgs e)
        {
            AddDataLabelForm ad = new AddDataLabelForm(labelContainer, LabelEditMode.Edit, (DataLabel)dataLabelBox.SelectedItem);
            ad.ShowDialog();
            UpdateDataBoxView();
        }

        private void removeDataLabelMenuItem_Click(object sender, EventArgs e)
        {
            labelContainer.RemoveDataLabel((DataLabel)dataLabelBox.SelectedItem);
            UpdateDataBoxView();
        }

        private void findReferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataLabel selectedLabel = (DataLabel)dataLabelBox.SelectedItem;
            UpdateMainTextBox(disassembler.SearchForReference(selectedLabel), TextBoxWriteMode.Overwrite);
        }

        #endregion DataLabelBox Menus

        #region VarLabelBox Menus

        private void addVariableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (romFile.FileLoaded)
            {
                AddVarLabelForm av = new AddVarLabelForm(labelContainer, LabelEditMode.Add);
                av.ShowDialog();
                UpdateVarBoxView();
            }
            else
            {
                Error.ShowErrorMessage(ErrorMessage.NO_FILE);
            }
        }

        private void editVariableToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            AddVarLabelForm av = new AddVarLabelForm(labelContainer, LabelEditMode.Edit, (VarLabel)varLabelBox.SelectedItem);
            av.ShowDialog();
            UpdateVarBoxView();
        }

        private void removeVariableToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            labelContainer.RemoveVarLabel((VarLabel)varLabelBox.SelectedItem);
            UpdateVarBoxView();
        }

        #endregion VarLabelBox Menus

        #region Toolstrip Menus

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Open GB/GBC File...";
            ofd.Filter = "GB/GBC Files|*.gb;*.gbc|All Files|*";
            ofd.FileName = "";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                FileInfo fs = new FileInfo(ofd.FileName);
                romFile.LoadFile(ofd.FileName);
                this.Text = "GBRead - " + ofd.SafeFileName;
                UpdateMainTextBox(romFile.GetBinInfo(), TextBoxWriteMode.Overwrite);
                currentFileLoaded = ofd.FileName;
                labelContainer.Initialize();
                UpdateFuncBoxView();
                UpdateDataBoxView();
                UpdateVarBoxView();
                startBox.Focus();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void loadLabelsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (romFile.FileLoaded)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Title = "Load Labels...";
                ofd.Filter = "Label File|*.txt|All Files|*";
                ofd.FileName = "";
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    labelContainer.LoadLabelFile(ofd.FileName);
                    UpdateFuncBoxView();
                    UpdateDataBoxView();
                    UpdateVarBoxView();
                }
            }
            else
            {
                Error.ShowErrorMessage(ErrorMessage.NO_FILE);
            }
        }

        private void saveLabelsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Save Labels...";
            sfd.FileName = "";
            sfd.Filter = "Label File|*.txt|All Files|*";
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                labelContainer.SaveLabelFile(sfd.FileName);
                MessageBox.Show(String.Format("Saved Labels to{0}{1}", Environment.NewLine, sfd.FileName), "Success", MessageBoxButtons.OK);
            }
        }

        private void saveEntireFileASMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (romFile.FileLoaded)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "Save All ASM...";
                sfd.Filter = "Text File|*.txt|All Files|*";
                sfd.FileName = "";
                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    new Thread(new ThreadStart(() =>
                    {
                        if (romFile.FileLoaded)
                        {
                            UpdateProgressLabel("Working...", TextBoxWriteMode.Overwrite);
                            using (StreamWriter sw = new StreamWriter(sfd.FileName, false))
                            {
                                sw.Write(disassembler.GetFullASM());
                            }
                            UpdateProgressLabel("", TextBoxWriteMode.Overwrite);
                            MessageBox.Show(String.Format("Saved All ASM to{0}{1}", Environment.NewLine, sfd.FileName), "Success", MessageBoxButtons.OK);
                        }
                    })).Start();
                }
            }
            else
            {
                Error.ShowErrorMessage(ErrorMessage.NO_FILE);
            }
        }

        private void saveChangedFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (romFile.FileLoaded)
            {
                SaveFileDialog afd = new SaveFileDialog();
                afd.Title = "Save GB/GBC File...";
                afd.Filter = "GB/GBC Files|*.gb;*.gbc|All Files|*";
                afd.FileName = "";
                if (afd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (MessageBox.Show("Do you want to correct the header checksum and complement bytes as well?", "Fix Header", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        if (romFile is GBBinFile)
                        {
                            ((GBBinFile)romFile).FixGBHeader();
                        }
                    }
                    if (romFile.SaveFile(afd.FileName))
                    {
                        MessageBox.Show("File successfuly written to " + Environment.NewLine + afd.FileName);
                    }
                    else
                    {
                        MessageBox.Show("File could not be written to " + Environment.NewLine + afd.FileName);
                    }
                }
            }
            else
            {
                Error.ShowErrorMessage(ErrorMessage.NO_FILE);
            }
        }

        private void optionsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OptionsForm op = new OptionsForm(disassembler, labelContainer, mainFormOptions, mainTextBox.SyntaxHighlighter);
            op.ShowDialog();
            mainTextBox.WordWrap = mainFormOptions.isWordWrap;
        }

        private void exportToBinaryFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Export Data Section To Binary...";
            sfd.Filter = "Binary File|*.bin|All Files|*";
            sfd.FileName = ((DataLabel)dataLabelBox.SelectedItem).Name;
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DataLabel ds = (DataLabel)dataLabelBox.SelectedItem;
                if (romFile.SaveFilePortion(sfd.FileName, ds.Offset, ds.Length))
                {
                    MessageBox.Show("Binary file successfuly written to " + Environment.NewLine + sfd.FileName);
                }
            }
        }

        private void insertExternalBinaryAtLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (romFile.FileLoaded)
            {
                InsertBinaryForm ib = new InsertBinaryForm(romFile);
                ib.ShowDialog();
            }
            else
            {
                Error.ShowErrorMessage(ErrorMessage.NO_FILE);
            }
        }

        private void addAllCallsMadeInCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (romFile.FileLoaded)
            {
                disassembler.AutoPopulateFunctionList();
                UpdateFuncBoxView();
            }
            else
            {
                Error.ShowErrorMessage(ErrorMessage.NO_FILE);
            }
        }

        #endregion Toolstrip Menus

        private void printASMButton_Click(object sender, EventArgs e)
        {
            int start;
            int end;
            InputValidation.TryParseOffsetString(startBox.Text, out start);
            InputValidation.TryParseOffsetString(endBox.Text, out end);
            new Thread(new ThreadStart(() =>
            {
                if (romFile.FileLoaded)
                {
                    if (start < 0 || end < 0)
                    {
                        Error.ShowErrorMessage(ErrorMessage.START_OR_END_INVALID);
                    }
                    else if (start < 0 || start > romFile.Length)
                    {
                        Error.ShowErrorMessage(ErrorMessage.START_INVALID);
                    }
                    else if (end <= 0 || end >= romFile.Length)
                    {
                        Error.ShowErrorMessage(ErrorMessage.END_INVALID);
                    }
                    else if (end < start)
                    {
                        Error.ShowErrorMessage(ErrorMessage.START_AFTER_END);
                    }
                    else
                    {
                        UpdateMainTextBox(disassembler.PrintASM(start, end - start), TextBoxWriteMode.Overwrite);
                    }
                }
                else
                {
                    Error.ShowErrorMessage(ErrorMessage.NO_FILE);
                }
            })).Start();
        }

        private void startEndBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Keys)e.KeyChar == Keys.Enter)
            {
                printASMButton_Click(new object(), new EventArgs());
                e.Handled = true;
            }
            if (!Char.IsControl(e.KeyChar) && !Char.IsDigit(e.KeyChar) && !(e.KeyChar >= 'A' && e.KeyChar <= 'F') && !(e.KeyChar >= 'a' && e.KeyChar <= 'f') && !(e.KeyChar == ':'))
            {
                e.Handled = true;
            }
        }

        private void funcLabelBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (funcLabelBox.Items.Count > 0)
            {
                FunctionLabel ds = (FunctionLabel)funcLabelBox.Items[e.Index];
                Brush itemBrush = Brushes.Black;
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Near;
                sf.LineAlignment = StringAlignment.Center;
                e.Graphics.DrawString(ds.ToString(), e.Font, itemBrush, funcLabelBox.GetItemRectangle(e.Index), sf);
            }
            e.DrawFocusRectangle();
        }

        private void dataLabelBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (dataLabelBox.Items.Count > 0)
            {
                DataLabel ds = (DataLabel)dataLabelBox.Items[e.Index];
                Brush itemBrush = Brushes.Black;
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Near;
                sf.LineAlignment = StringAlignment.Center;
                switch (ds.DSectionType)
                {
                    case DataSectionType.Image:
                        itemBrush = Brushes.DeepPink;
                        break;
                    default:
                        break;
                }
                e.Graphics.DrawString(ds.ToString(), e.Font, itemBrush, dataLabelBox.GetItemRectangle(e.Index), sf);
            }
            e.DrawFocusRectangle();
        }

        private void varLabelBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (varLabelBox.Items.Count > 0)
            {
                VarLabel ds = (VarLabel)varLabelBox.Items[e.Index];
                Brush itemBrush = Brushes.Black;
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Near;
                sf.LineAlignment = StringAlignment.Center;
                e.Graphics.DrawString(ds.ToString(), e.Font, itemBrush, varLabelBox.GetItemRectangle(e.Index), sf);
            }
            e.DrawFocusRectangle();
        }

        private void insertASMAtLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (romFile.FileLoaded)
            {
                InsertCodeForm iC = new InsertCodeForm(romFile, disassembler, assembler, mainTextBox.SyntaxHighlighter);
                iC.ShowDialog();
            }
            else
            {
                Error.ShowErrorMessage(ErrorMessage.NO_FILE);
            }
        }

        private void searchForFunctionsThatCallThisOneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (romFile.FileLoaded)
            {
                UpdateMainTextBox(disassembler.SearchForFunctionCall((FunctionLabel)funcLabelBox.SelectedItem), TextBoxWriteMode.Overwrite);
            }
        }

        #endregion Menu Item Handlers

        #region Function Label Box Handlers

        private void funcLabelBox_DoubleClick(object sender, EventArgs e)
        {
            if (funcLabelBox.SelectedItem != null)
            {
                UpdateMainTextBox(disassembler.ShowFuncLabel((FunctionLabel)funcLabelBox.SelectedItem), TextBoxWriteMode.Overwrite);
            }
        }

        private void funcLabelBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                funcLabelBox.SelectedIndex = funcLabelBox.IndexFromPoint(e.X, e.Y);
                if (funcLabelBox.SelectedItem != null) funcLabelBoxContextMenu.Show(MousePosition);
                else funcLabelBoxContextMenu2.Show(MousePosition);
            }
        }

        private void funcLabelBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (funcLabelBox.SelectedIndex != -1)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    UpdateMainTextBox(disassembler.ShowFuncLabel((FunctionLabel)funcLabelBox.SelectedItem), TextBoxWriteMode.Overwrite);
                }
                else if (e.KeyCode == Keys.Delete)
                {
                    labelContainer.RemoveFuncLabel((FunctionLabel)funcLabelBox.SelectedItem);
                    UpdateFuncBoxView();
                }
            }
        }

        #endregion Function Label Box Handlers

        #region Data Label Box Handlers

        private void dataLabelBox_DoubleClick(object sender, EventArgs e)
        {
            if (dataLabelBox.SelectedItem != null)
            {
                if (((DataLabel)dataLabelBox.SelectedItem).DSectionType == DataSectionType.Image)
                {
                    ImageDisplayForm img = new ImageDisplayForm(romFile, (DataLabel)dataLabelBox.SelectedItem);
                    img.ShowDialog();
                }
                else
                {
                    UpdateMainTextBox(disassembler.ShowDataLabel((DataLabel)dataLabelBox.SelectedItem), TextBoxWriteMode.Overwrite);
                }
            }
        }

        private void dataLabelBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                dataLabelBox.SelectedIndex = dataLabelBox.IndexFromPoint(e.X, e.Y);
                if (dataLabelBox.SelectedItem != null) dataLabelBoxContextMenu.Show(MousePosition);
                else dataLabelContextMenu2.Show(MousePosition);
            }
        }

        private void dataLabelBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                UpdateMainTextBox(disassembler.ShowDataLabel((DataLabel)dataLabelBox.SelectedItem), TextBoxWriteMode.Overwrite);
            }
            else if (e.KeyCode == Keys.Delete)
            {
                labelContainer.RemoveDataLabel((DataLabel)dataLabelBox.SelectedItem);
                UpdateDataBoxView();
            }
        }

        #endregion Data Label Box Handlers

        #region Variable Label Box Handlers

        private void varLabelBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                varLabelBox.SelectedIndex = varLabelBox.IndexFromPoint(e.X, e.Y);
                if (varLabelBox.SelectedItem != null) varLabelBoxContextMenu.Show(MousePosition);
                else varLabelBoxContextMenu2.Show(MousePosition);
            }
        }

        private void varLabelBox_DoubleClick(object sender, EventArgs e)
        {
            if (varLabelBox.SelectedItem != null)
            {
                VarLabel selectedLabel = (VarLabel)varLabelBox.SelectedItem;
                UpdateMainTextBox(disassembler.ShowVarLabel(selectedLabel), TextBoxWriteMode.Overwrite);
            }
        }

        private void varLabelBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                UpdateMainTextBox(disassembler.ShowVarLabel((VarLabel)varLabelBox.SelectedItem), TextBoxWriteMode.Overwrite);
            }
            else if (e.KeyCode == Keys.Delete)
            {
                labelContainer.RemoveVarLabel((VarLabel)varLabelBox.SelectedItem);
                UpdateVarBoxView();
            }
        }

        #endregion Variable Label Box Handlers

        #region List Management

        private void UpdateFuncBoxView()
        {
            ((CurrencyManager)funcLabelBox.BindingContext[labelContainer.FuncList]).Refresh();
        }

        private void UpdateDataBoxView()
        {
            ((CurrencyManager)dataLabelBox.BindingContext[labelContainer.DataList]).Refresh();
        }

        private void UpdateVarBoxView()
        {
            ((CurrencyManager)varLabelBox.BindingContext[labelContainer.VarList]).Refresh();
        }

        #endregion List Management

        #region Box Update Invocations

        private delegate void UpdateTextBoxDelegate(string updateText, TextBoxWriteMode overwriteExistingText);

        private void UpdateProgressLabel(string newLabel, TextBoxWriteMode overwriteExistingText = TextBoxWriteMode.Append)
        {
            if (progressLabel.InvokeRequired)
            {
                UpdateTextBoxDelegate del = new UpdateTextBoxDelegate(UpdateProgressLabel);
                progressLabel.Invoke(del, new object[] { newLabel, overwriteExistingText });
            }
            else
            {
                if (overwriteExistingText == TextBoxWriteMode.Overwrite) progressLabel.Text = newLabel;
                else progressLabel.Text += newLabel;
            }
        }

        public enum TextBoxWriteMode { Append, Overwrite }

        private void UpdateMainTextBox(string text, TextBoxWriteMode overwriteExistingText)
        {
            if (mainTextBox.InvokeRequired)
            {
                UpdateTextBoxDelegate del = new UpdateTextBoxDelegate(UpdateMainTextBox);
                mainTextBox.Invoke(del, new object[] { text, overwriteExistingText });
            }
            else
            {
                if (overwriteExistingText == TextBoxWriteMode.Overwrite)
                {
                    mainTextBox.Clear();
                }
                mainTextBox.AppendText(text);
            }
        }

        #endregion Box Update Invocations

        private void addNewButton_Click(object sender, EventArgs e)
        {
            switch (LabelTabControl.SelectedIndex)
            {
                case 0:
                    addFuncLabelMenuItem_Click(sender, e);
                    return;
                case 1:
                    addDataLabelMenuItem_Click(sender, e);
                    return;
                case 2:
                    addVariableToolStripMenuItem_Click(sender, e);
                    return;
            }
        }

        private void addCommentButton_Click(object sender, EventArgs e)
        {
            if (romFile.FileLoaded)
            {
                var af = new AddCommentForm(labelContainer, LabelEditMode.Add);
                af.ShowDialog();
            }
            else
            {
                Error.ShowErrorMessage(ErrorMessage.NO_FILE);
            }
        }
    }

    public class MainFormOptions
    {
        public bool isWordWrap;

        public MainFormOptions()
        {
            isWordWrap = false;
        }
    }
}