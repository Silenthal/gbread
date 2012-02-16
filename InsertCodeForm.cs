using System;
using System.Windows.Forms;
using System.Collections.Generic;


namespace GBRead
{
	public partial class InsertCodeForm : Form
	{
		private BinFile baseFile;
		private Disassembler refFile;
		private Assembler asm;
		public BinFile preComCheck;
		private int insertOffset;
		public bool gcheckSuccess;

		public InsertCodeForm(BinFile existing, Disassembler disassembler, Assembler asnew, Z80SyntaxHighlighter sh, int offset = 0)
		{
			InitializeComponent();
			refFile = disassembler;
			baseFile = existing;
			gcheckSuccess = false;
			offsetBox.Text = offset.ToString("X");
			asm = asnew;
			codeTextBox.SyntaxHighlighter = sh;
			insertOffset = -1;
		}

		private void assembleButton_Click(object sender, EventArgs e)
		{
			preComCheck = null;
			CompError c = new CompError();
			int off;
			bool success = InputValidation.TryParseOffsetString(offsetBox.Text, out off);
			if (success)
			{
				insertOffset = off;
				bool syntaxPass = false;
				preComCheck = new GBBinFile(asm.AssembleASM(off, codeTextBox.Lines, ref c, out syntaxPass));
				if (!syntaxPass)
				{
					Error.ShowErrorMessage(c);
					gcheckSuccess = false;
				}
				else if (preComCheck.Length == 0)
				{
					gcheckSuccess = false;
				}
				else
				{
					intermediaryTextBox.Text = refFile.PrintASM(preComCheck, off, 0, preComCheck.Length);
					replaceTextBox.Text = refFile.PrintASM(off, preComCheck.Length);
					gcheckSuccess = true;
				}
			}
		}

		private void insertButton_Click(object sender, EventArgs e)
		{
			if (gcheckSuccess)
			{
				if (MessageBox.Show("Are you sure you want to insert this code?", "Confirm Insertion", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
				{
					baseFile.ModifyFile(insertOffset, preComCheck);
					this.DialogResult = System.Windows.Forms.DialogResult.OK;
				}
			}
		}

		private void offsetBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			if ((Keys)e.KeyChar == Keys.Enter)
			{
				insertButton_Click(new object(), new EventArgs());
				e.Handled = true;
			}
			if (!Char.IsControl(e.KeyChar) && !Char.IsDigit(e.KeyChar) && !(e.KeyChar >= 'A' && e.KeyChar <= 'F') && !(e.KeyChar >= 'a' && e.KeyChar <= 'f') && !(e.KeyChar == ':'))
			{
				e.Handled = true;
			}
		}
	}
}