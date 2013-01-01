namespace GBRead.Forms
{
    using System;
    using System.Windows.Forms;
    using GBRead.Base;

    public partial class OptionsForm : Form
    {
        private Disassembler disassembler;
        private Assembler assembler;
        private LabelContainer lcs;
        private MainFormOptions mfo;

        public OptionsForm(Disassembler op, Assembler ap, LabelContainer lc, MainFormOptions mf)
        {
            InitializeComponent();
            disassembler = op;
            assembler = ap;
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
            dsmColor00Box.Text = op.GameboyFormatChars.Length > 0 ? op.GameboyFormatChars[0].ToString() : "0";
            dsmColor01Box.Text = op.GameboyFormatChars.Length > 1 ? op.GameboyFormatChars[1].ToString() : "1";
            dsmColor10Box.Text = op.GameboyFormatChars.Length > 2 ? op.GameboyFormatChars[2].ToString() : "2";
            dsmColor11Box.Text = op.GameboyFormatChars.Length > 3 ? op.GameboyFormatChars[3].ToString() : "3";

            asmColor00Box.Text = ap.GameboyFormatChars.Length > 0 ? ap.GameboyFormatChars[0].ToString() : "0";
            asmColor01Box.Text = ap.GameboyFormatChars.Length > 1 ? ap.GameboyFormatChars[1].ToString() : "1";
            asmColor10Box.Text = ap.GameboyFormatChars.Length > 2 ? ap.GameboyFormatChars[2].ToString() : "2";
            asmColor11Box.Text = ap.GameboyFormatChars.Length > 3 ? ap.GameboyFormatChars[3].ToString() : "3";
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
            string fString = "";
            if (dsmColor00Box.Text.Length > 0 && dsmColor00Box.Text != " ")
            {
                fString += dsmColor00Box.Text;
            }
            if (dsmColor01Box.Text.Length > 0 && dsmColor01Box.Text != " ")
            {
                fString += dsmColor01Box.Text;
            }
            if (dsmColor10Box.Text.Length > 0 && dsmColor10Box.Text != " ")
            {
                fString += dsmColor10Box.Text;
            }
            if (dsmColor11Box.Text.Length > 0 && dsmColor11Box.Text != " ")
            {
                fString += dsmColor11Box.Text;
            }
            disassembler.GameboyFormatChars = fString;
            fString = "";
            if (asmColor00Box.Text.Length > 0 && asmColor00Box.Text != " ")
            {
                fString += asmColor00Box.Text;
            }
            if (asmColor01Box.Text.Length > 0 && asmColor01Box.Text != " ")
            {
                fString += asmColor01Box.Text;
            }
            if (asmColor10Box.Text.Length > 0 && asmColor10Box.Text != " ")
            {
                fString += asmColor10Box.Text;
            }
            if (asmColor11Box.Text.Length > 0 && asmColor11Box.Text != " ")
            {
                fString += asmColor11Box.Text;
            }
            assembler.GameboyFormatChars = fString;
        }
    }
}