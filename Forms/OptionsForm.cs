using System;
using System.Windows.Forms;
using GBRead.Base;

namespace GBRead.Forms
{
	public partial class OptionsForm : Form
	{
		private Disassembler disassembler;
		private LabelContainer lcs;
		private MainFormOptions mfo;
		private Z80SyntaxHighlighter shb;
		private Panel[] panellList;
		private TreeNode lastSelectedNode;
		public OptionsForm(Disassembler op, LabelContainer lc, MainFormOptions mf, Z80SyntaxHighlighter sh)
		{
			InitializeComponent();
			disassembler = op;
			lcs = lc;
			mfo = mf;
			shb = sh;
			printOffsetsCheckBox.Checked = op.PrintOffsets;
			hideDataSectionsCheckBox.Checked = op.HideDefinedData;
			hideFunctionsCheckBox.Checked = op.HideDefinedFunctions;
			printBitPatternCheckBox.Checked = op.PrintBitPattern;
			printCommentsCheckBox.Checked = op.PrintComments;
			wordWrapCheckBox.Checked = mf.isWordWrap;
			switch (op.PrintedOffsetFormat)
			{
				case OffsetFormat.BBOO:
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
			highlightCommentsCheckBox.Checked = sh.HighlightComments;
			highlightInstructionsCheckBox.Checked = sh.HighlightKeywords;
			highlightNumbersCheckBox.Checked = sh.HighlightNumbers;
			highlightLabelsCheckBox.Checked = sh.HighlightLabels;
			highlightOffsetsCheckBox.Checked = sh.HighlightOffsets;
			highlightRegistersCheckBox.Checked = sh.HighlightRegisters;
			#region Enable Panel Display
			panellList = new Panel[]
			{
				mainWindowPanel, 
				highlightingPanel
			};
			foreach (Panel p in panellList)
			{
				p.Visible = false;
			}
			
			mainWindowPanel.Visible = true;
			lastSelectedNode = treeView1.TopNode;
			#endregion
		}

		private void saveButton_Click(object sender, EventArgs e)
		{
			disassembler.PrintComments = printCommentsCheckBox.Checked;
			switch (offsetNumberFormatBox.SelectedIndex)
			{
				case 0:
					disassembler.PrintedOffsetFormat = OffsetFormat.BBOO;
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
			disassembler.HideDefinedFunctions = hideFunctionsCheckBox.Checked;
			disassembler.PrintBitPattern = printBitPatternCheckBox.Checked;
			mfo.isWordWrap = wordWrapCheckBox.Checked;
			shb.HighlightComments = highlightCommentsCheckBox.Checked;
			shb.HighlightKeywords = highlightInstructionsCheckBox.Checked;
			shb.HighlightNumbers = highlightNumbersCheckBox.Checked;
			shb.HighlightLabels = highlightLabelsCheckBox.Checked;
			shb.HighlightOffsets = highlightOffsetsCheckBox.Checked;
			shb.HighlightRegisters = highlightRegistersCheckBox.Checked;
		}

		private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			if (e.Node == null) return;
			else
			{
				if (lastSelectedNode == e.Node) return;
				foreach (Panel p in panellList)
				{
					p.Visible = false;
				}
				if (e.Node.Parent == null)
				{
					panellList[e.Node.Index].Visible = true;
					lastSelectedNode = e.Node;
				}
			}
		}
	}
}
