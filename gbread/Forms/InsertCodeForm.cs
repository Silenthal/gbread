namespace GBRead.Forms
{
    using System;
    using System.Windows.Forms;
    using GBRead.Base;
    using ICSharpCode.AvalonEdit;

    public partial class InsertCodeForm : Form
    {
        private BinFile baseFile;
        private Disassembler refFile;
        private Assembler asm;
        public BinFile preComCheck;
        private int insertOffset;
        public bool gcheckSuccess;
        private TextEditor mainTextBox;

        public InsertCodeForm(BinFile existing, Disassembler disassembler, Assembler asnew, int offset = 0)
        {
            InitializeComponent();
            mainTextBox = ((TextBoxHost)elementHost1.Child).mainTextBox;
            mainTextBox.ShowLineNumbers = true;
            refFile = disassembler;
            baseFile = existing;
            gcheckSuccess = false;
            offsetBox.Text = offset.ToString("X");
            asm = asnew;
            insertOffset = -1;
        }

        private void assembleButton_Click(object sender, EventArgs e)
        {
            preComCheck = null;
            CompError c = new CompError();
            int off;
            bool success = Utility.OffsetStringToInt(offsetBox.Text, out off);
            if (success)
            {
                insertOffset = off;
                bool syntaxPass = false;
                preComCheck = new GBBinFile(asm.AssembleASM(off, mainTextBox.Text, ref c, out syntaxPass));

                if (!syntaxPass)
                {
                    Error.ShowErrorMessage(c);
                    gcheckSuccess = false;
                }
                else if (preComCheck.Length != 0)
                {
                    string message = "Are you sure you want to insert this code?";
                    message += "\n" + "Offset: $" + insertOffset.ToString("X") + "  Size: $" + preComCheck.Length.ToString("X") + " byte(s)";
                    if (MessageBox.Show(message, "Confirm Insertion", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        baseFile.ModifyFile(insertOffset, preComCheck);
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    }
                }
            }
        }

        private void offsetBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Keys)e.KeyChar == Keys.Enter)
            {
                assembleButton_Click(new object(), new EventArgs());
                e.Handled = true;
            }
            if (!Char.IsControl(e.KeyChar) && !Char.IsDigit(e.KeyChar) && !(e.KeyChar >= 'A' && e.KeyChar <= 'F') && !(e.KeyChar >= 'a' && e.KeyChar <= 'f') && !(e.KeyChar == ':'))
            {
                e.Handled = true;
            }
        }
    }
}