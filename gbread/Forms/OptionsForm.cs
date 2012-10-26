namespace GBRead.Forms
{
    using System;
    using System.Windows.Forms;
    using GBRead.Base;

    public partial class OptionsForm : Form
    {
        private Disassembler disassembler;
        private LabelContainer lcs;
        private MainFormOptions mfo;

        public OptionsForm(Disassembler op, LabelContainer lc, MainFormOptions mf)
        {
            InitializeComponent();
            disassembler = op;
            lcs = lc;
            mfo = mf;
            printOffsetsCheckBox.Checked = op.PrintOffsets;
            hideDataSectionsCheckBox.Checked = op.HideDefinedData;
            printBitPatternCheckBox.Checked = op.PrintBitPattern;
            printCommentsCheckBox.Checked = op.PrintComments;
            wordWrapCheckBox.Checked = mf.isWordWrap;
            switch (op.PrintedOffsetFormat)
            {
                case OffsetFormat.BankOffset:
                    offsetNumberFormatBox.SelectedIndex = 0;
                    break;

                case OffsetFormat.Hex:
                    offsetNumberFormatBox.SelectedIndex = 1;
                    break;

                case OffsetFormat.Decimal:
                    offsetNumberFormatBox.SelectedIndex = 2;
                    break;
            }
            switch (op.InstructionNumberFormat)
            {
                case OffsetFormat.Hex:
                    instructionNumberFormatBox.SelectedIndex = 0;
                    break;

                case OffsetFormat.Decimal:
                    instructionNumberFormatBox.SelectedIndex = 1;
                    break;
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            disassembler.PrintComments = printCommentsCheckBox.Checked;
            switch (offsetNumberFormatBox.SelectedIndex)
            {
                case 0:
                    disassembler.PrintedOffsetFormat = OffsetFormat.BankOffset;
                    break;

                case 1:
                    disassembler.PrintedOffsetFormat = OffsetFormat.Hex;
                    break;

                case 2:
                    disassembler.PrintedOffsetFormat = OffsetFormat.Decimal;
                    break;
            }
            switch (instructionNumberFormatBox.SelectedIndex)
            {
                case 0:
                    disassembler.InstructionNumberFormat = OffsetFormat.Hex;
                    break;

                case 1:
                    disassembler.InstructionNumberFormat = OffsetFormat.Decimal;
                    break;
            }
            disassembler.PrintOffsets = printOffsetsCheckBox.Checked;
            disassembler.HideDefinedData = hideDataSectionsCheckBox.Checked;
            disassembler.PrintBitPattern = printBitPatternCheckBox.Checked;
            mfo.isWordWrap = wordWrapCheckBox.Checked;
        }
    }
}