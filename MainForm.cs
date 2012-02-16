using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace GBRead
{
	public partial class MainForm : Form
	{
		string currentFileLoaded = "";

		LabelContainer labelContainer;

		Disassembler disassembler;

		Assembler assembler;

		BinFile romFile;

		class PrintingFunctionsThreadArgumentPack
		{
			public CodeLabel labelToBePrinted { get; set; }
		}

		class PrintingASMThreadArgumentPack
		{
			public int Start { get; set; }
			public int End { get; set; }
		}

		class PrintingFileASMThreadArgumentPack
		{
			public string SavedASMFileName { get; set; }
		}

		MainFormOptions mainFormOptions = new MainFormOptions();

		public MainForm(BinFile cs, Disassembler ds, Assembler ac, LabelContainer lcnew)
		{
			InitializeComponent();
			
			disassembler = ds;
			assembler = ac;
			labelContainer = lcnew;
			romFile = cs;

			dataLabelBox.DataSource = labelContainer.DataList;
			codeLabelBox.DataSource = labelContainer.LabelList;
			varLabelBox.DataSource = labelContainer.VarList;
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			backgroundWorker1.CancelAsync();
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

		#region Code Label Box Handlers
		private void codeLabelBox_DoubleClick(object sender, EventArgs e)
		{
			if (codeLabelBox.SelectedItem != null)
			{
				PrintingFunctionsThreadArgumentPack pfrg = new PrintingFunctionsThreadArgumentPack
				{
					labelToBePrinted = (CodeLabel)codeLabelBox.SelectedItem
				};
				if (!backgroundWorker1.IsBusy) backgroundWorker1.RunWorkerAsync(pfrg);
			}
		}
		private void codeLabelBox_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == System.Windows.Forms.MouseButtons.Right)
			{
				codeLabelBox.SelectedIndex = codeLabelBox.IndexFromPoint(e.X, e.Y);
				if (codeLabelBox.SelectedItem != null) codeLabelBoxContextMenu.Show(MousePosition);
				else codeLabelBoxContextMenu2.Show(MousePosition);
			}
		}
		private void codeLabelBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (codeLabelBox.SelectedIndex != -1)
			{
				if (e.KeyCode == Keys.Enter)
				{
					PrintingFunctionsThreadArgumentPack pfrg = new PrintingFunctionsThreadArgumentPack
					{
						labelToBePrinted = (CodeLabel)codeLabelBox.SelectedItem
					};
					if (!backgroundWorker1.IsBusy) backgroundWorker1.RunWorkerAsync(pfrg);
				}
				else if (e.KeyCode == Keys.Delete)
				{
					labelContainer.RemoveLabel((CodeLabel)codeLabelBox.SelectedItem);
					UpdateLabelBoxView();
				}
			}
		}
		#endregion

		#region Data Label Box Handlers
		private void dataLabelBox_DoubleClick(object sender, EventArgs e)
		{
			if (dataLabelBox.SelectedItem != null)
			{
				if (((DataLabel)dataLabelBox.SelectedItem).DSectionType == DataSectionType.Img)
				{
					ImageDisplayForm img = new ImageDisplayForm(romFile, (DataLabel)dataLabelBox.SelectedItem);
					if (img.ShowDialog() == System.Windows.Forms.DialogResult.OK)
					{
						labelContainer.AddLabel(img.dataSect);
						UpdateDataBoxView();
					}
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
				labelContainer.RemoveLabel((DataLabel)dataLabelBox.SelectedItem);
				UpdateDataBoxView();
			}
		}
		#endregion

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
				labelContainer.RemoveLabel((VarLabel)varLabelBox.SelectedItem);
				UpdateVarBoxView();
			}
		}
		#endregion

		#region Menu Item Handlers

		#region CodeLabelBox Menu

		private void addANewCodeLabelToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (romFile.FileLoaded)
			{
				codeTabAddCancelButton();
			}
			else
			{
				Error.ShowErrorMessage(ErrorMessage.NO_FILE);
			}
		}

		private void renameLabelToolStripMenuItem_Click(object sender, EventArgs e)
		{
			codeTabEditFunction();
		}

		private void removeFunctionMenuItem_Click(object sender, EventArgs e)
		{
			labelContainer.RemoveLabel((CodeLabel)codeLabelBox.SelectedItem);
			UpdateLabelBoxView();
		}

		private void codeTabAddCancelButton()
		{
			if (cancelButton.Visible)
			{
				int offset;
				int length;
				bool offSuccess = InputValidation.TryParseOffsetString(codeLabelOffsetBox.Text, out offset);
				bool lengthSuccess = InputValidation.TryParseOffsetString(codeLabelLengthBox.Text, out length);
				if (!offSuccess)
				{
					MessageBox.Show("Your entered offset was invalid. Make sure to type it properly.", "Error", MessageBoxButtons.OK);
				}
				else if (!lengthSuccess && !codeLabelLengthBox.Text.Equals(String.Empty))
				{
					MessageBox.Show("Your entered length was invalid. Make sure to type it properly.", "Error", MessageBoxButtons.OK);
				}
				else if (offset + length > romFile.Length)
				{
					MessageBox.Show("Your entered offset and length are not in the file. Make sure to type them properly.", "Error", MessageBoxButtons.OK);
				}
				else
				{
					labelContainer.AddCodeLabel(offset, codeLabelNameBox.Text, (length < 1 ? 0 : length), codeLabelCommentBox.Lines);
					UpdateLabelBoxView();
					FlipCodeLabelTabControlVisibility();
					editButton.Visible = true;
				}
			}
			else
			{
				FlipCodeLabelTabControlVisibility();
				editButton.Visible = false;
			}
		}

		private void codeTabEditFunction()
		{
			CodeLabel fetch = (CodeLabel)codeLabelBox.SelectedItem;
			if (cancelButton.Visible)
			{
				int offset;
				int length;
				bool offsetGood = InputValidation.TryParseOffsetString(codeLabelOffsetBox.Text, out offset);
				bool lengthGood = InputValidation.TryParseOffsetString(codeLabelLengthBox.Text, out length);
				if (offset < 0)
				{
					MessageBox.Show("Your entered offset was invalid. Make sure to type it properly.", "Error", MessageBoxButtons.OK);
				}
				else if (length < 0)
				{
					MessageBox.Show("Your entered length was invalid. Make sure to type it properly.", "Error", MessageBoxButtons.OK);
				}
				else if (offset + length > romFile.Length)
				{
					MessageBox.Show("Your entered offset and length are not in the file. Make sure to type them properly.", "Error", MessageBoxButtons.OK);
				}
				else
				{
					labelContainer.RemoveLabel(fetch);
					labelContainer.AddCodeLabel(offset, codeLabelNameBox.Text, (length < 1 ? 0 : length), codeLabelCommentBox.Lines);
					UpdateLabelBoxView();
					FlipCodeLabelTabControlVisibility();
					addButton.Visible = true;
				}
			}
			else
			{
				FlipCodeLabelTabControlVisibility();
				addButton.Visible = false;
				codeLabelNameBox.Text = fetch.Name;
				codeLabelOffsetBox.Text = fetch.Offset.ToString("X");
				codeLabelLengthBox.Text = fetch.Length.ToString("X");
				if (fetch.Comment != null)
				{
					if (fetch.Comment.Length == 1)
					{
						codeLabelCommentBox.Text = fetch.Comment[0];
					}
					else
					{
						for (int i = 0; i < fetch.Comment.Length; i++)
						{
							if (i == 0) codeLabelCommentBox.Text = fetch.Comment[i];
							else codeLabelCommentBox.AppendText(Environment.NewLine + fetch.Comment[i]);
						}
					}
				}
			}
		}

		public void FlipCodeLabelTabControlVisibility()
		{
			codeLabelNameLabel.Visible = !codeLabelNameLabel.Visible;
			codeLabelOffsetLabel.Visible = !codeLabelOffsetLabel.Visible;
			codeLabelLengthLabel.Visible = !codeLabelLengthLabel.Visible;
			codeLabelCommentLabel.Visible = !codeLabelCommentLabel.Visible;
			codeLabelBox.Visible = !codeLabelBox.Visible;
			codeLabelNameBox.Visible = !codeLabelNameBox.Visible;
			codeLabelOffsetBox.Visible = !codeLabelOffsetBox.Visible;
			codeLabelLengthBox.Visible = !codeLabelLengthBox.Visible;
			codeLabelCommentBox.Visible = !codeLabelCommentBox.Visible;
			cancelButton.Visible = !cancelButton.Visible;
			codeLabelNameBox.Text = "";
			codeLabelOffsetBox.Text = "";
			codeLabelLengthBox.Text = "";
			codeLabelCommentBox.Text = "";
			if (codeLabelOffsetBox.Visible) codeLabelOffsetBox.Focus();
		}

		#endregion

		#region DataLabelBox Menus

		private void addANewDataSectionToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (romFile.FileLoaded)
			{
				dataTabAddCancelButton();
			}
			else
			{
				Error.ShowErrorMessage(ErrorMessage.NO_FILE);
			}
		}

		private void renameADataSectionToolStripMenuItem_Click(object sender, EventArgs e)
		{
			dataTabEditFunction();
		}

		private void removeToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			labelContainer.RemoveLabel((DataLabel)dataLabelBox.SelectedItem);
			UpdateDataBoxView();
		}

		public void dataTabAddCancelButton()
		{
			if (cancelButton.Visible)
			{
				int offset;
				int length;
				int dataDiv;
				bool offsetGood = InputValidation.TryParseOffsetString(dataLabelOffsetBox.Text, out offset);
				bool lengthGood = InputValidation.TryParseOffsetString(dataLabelLengthBox.Text, out length);
				bool dataDivGood = InputValidation.TryParseOffsetString(dataLabelRowLengthBox.Text, out dataDiv);
				if (offset < 0)
				{
					MessageBox.Show("Your entered offset was invalid. Make sure to type it properly.", "Error", MessageBoxButtons.OK);
				}
				else if (length < 1)
				{
					MessageBox.Show("Your entered length was invalid. Make sure to type it properly.", "Error", MessageBoxButtons.OK);
				}
				else if (offset + length > romFile.Length)
				{
					MessageBox.Show("Your entered offset and length are not in the file. Make sure to type them properly.", "Error", MessageBoxButtons.OK);
				}
				else
				{
					DataSectionType dst = (DataSectionType)dataLabelDataTypeBox.SelectedIndex;
					labelContainer.AddDataLabel(dst, offset, dataLabelNameBox.Text, length, dataDiv, dataLabelCommentBox.Lines);
					UpdateDataBoxView();
					FlipDataLabelTabControlVisibility();
					editButton.Visible = true;
				}
			}
			else
			{
				FlipDataLabelTabControlVisibility();
				editButton.Visible = false;
			}
		}

		private void dataTabEditFunction()
		{
			DataLabel fetch = (DataLabel)dataLabelBox.SelectedItem;
			if (cancelButton.Visible)
			{
				int offset;
				int length;
				int dataDiv;
				bool offsetGood = InputValidation.TryParseOffsetString(dataLabelOffsetBox.Text, out offset);
				bool lengthGood = InputValidation.TryParseOffsetString(dataLabelLengthBox.Text, out length);
				bool dataDivGood = InputValidation.TryParseOffsetString(dataLabelRowLengthBox.Text, out dataDiv);
				if (offset < 0)
				{
					MessageBox.Show("Your entered offset was invalid. Make sure to type it properly.", "Error", MessageBoxButtons.OK);
				}
				else if (length < 1)
				{
					MessageBox.Show("Your entered length was invalid. Make sure to type it properly.", "Error", MessageBoxButtons.OK);
				}
				else if (offset + length > romFile.Length)
				{
					MessageBox.Show("Your combined offset and length are not in the file. Make sure to type them properly.", "Error", MessageBoxButtons.OK);
				}
				else
				{
					labelContainer.RemoveLabel(fetch);
					DataSectionType dst = (DataSectionType)dataLabelDataTypeBox.SelectedIndex;
					labelContainer.AddDataLabel(dst, offset, dataLabelNameBox.Text, length, dataDiv, dataLabelCommentBox.Lines);
					UpdateDataBoxView();
					FlipDataLabelTabControlVisibility();
					addButton.Visible = true;
				}
			}
			else
			{
				FlipDataLabelTabControlVisibility();
				addButton.Visible = false;
				dataLabelNameBox.Text = fetch.Name;
				dataLabelOffsetBox.Text = fetch.Offset.ToString("X");
				dataLabelLengthBox.Text = fetch.Length.ToString("X");
				dataLabelRowLengthBox.Text = fetch.DataLineLength.ToString("X");
				dataLabelDataTypeBox.SelectedIndex = (int)fetch.DSectionType;
				if (fetch.Comment != null)
				{
					if (fetch.Comment.Length == 1)
					{
						dataLabelCommentBox.Text = fetch.Comment[0];
					}
					else
					{
						for (int i = 0; i < fetch.Comment.Length; i++)
						{
							if (i == 0) dataLabelCommentBox.Text = fetch.Comment[i];
							else dataLabelCommentBox.AppendText(Environment.NewLine + fetch.Comment[i]);
						}
					}
				}
			}
		}

		public void FlipDataLabelTabControlVisibility()
		{
			dataLabelNameLabel.Visible = !dataLabelNameLabel.Visible;
			dataLabelOffsetLabel.Visible = !dataLabelOffsetLabel.Visible;
			dataLabelLengthLabel.Visible = !dataLabelLengthLabel.Visible;
			dataLabelDataTypeLabel.Visible = !dataLabelDataTypeLabel.Visible;
			dataLabelCommentLabel.Visible = !dataLabelCommentLabel.Visible;
			dataLabelRowLengthLabel.Visible = !dataLabelRowLengthLabel.Visible;

			dataLabelBox.Visible = !dataLabelBox.Visible;
			dataLabelNameBox.Visible = !dataLabelNameBox.Visible;
			dataLabelOffsetBox.Visible = !dataLabelOffsetBox.Visible;
			dataLabelLengthBox.Visible = !dataLabelLengthBox.Visible;
			dataLabelDataTypeBox.Visible = !dataLabelDataTypeBox.Visible;
			dataLabelCommentBox.Visible = !dataLabelCommentBox.Visible;
			dataLabelRowLengthBox.Visible = !dataLabelRowLengthBox.Visible;

			cancelButton.Visible = !cancelButton.Visible;
			dataLabelNameBox.Text = "";
			dataLabelOffsetBox.Text = "";
			dataLabelLengthBox.Text = "";
			dataLabelCommentBox.Text = "";
			dataLabelDataTypeBox.SelectedIndex = 0;
			dataLabelRowLengthBox.Text = "8";
			if (dataLabelOffsetBox.Visible) dataLabelOffsetBox.Focus();
		}

		#endregion

		#region VarLabelBox Menus

		private void addVariableToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (romFile.FileLoaded)
			{
				varTabAddCancelButton();
			}
			else
			{
				Error.ShowErrorMessage(ErrorMessage.NO_FILE);
			}
		}

		private void editVariableToolStripMenuItem2_Click(object sender, EventArgs e)
		{
			varTabEditFunction();
		}

		private void removeVariableToolStripMenuItem3_Click(object sender, EventArgs e)
		{
			labelContainer.RemoveLabel((VarLabel)varLabelBox.SelectedItem);
			UpdateVarBoxView();
		}

		public void varTabAddCancelButton()
		{
			if (cancelButton.Visible)
			{
				int offset;
				bool offsetGood = InputValidation.TryParseOffsetString(varLabelOffsetBox.Text, out offset);
				if (offset < 0)
				{
					MessageBox.Show("Your entered variable was invalid. Make sure to type it properly.", "Error", MessageBoxButtons.OK);
				}
				else
				{
					labelContainer.AddVarLabel(offset, varLabelNameBox.Text, varLabelCommentBox.Lines);
					UpdateVarBoxView();
					FlipVarLabelTabControlVisibility();
					editButton.Visible = true;
				}
			}
			else
			{
				FlipVarLabelTabControlVisibility();
				editButton.Visible = false;
			}
		}

		private void varTabEditFunction()
		{
			VarLabel fetch = (VarLabel)varLabelBox.SelectedItem;
			if (cancelButton.Visible)
			{
				int offset;
				int length;
				bool offsetGood = InputValidation.TryParseOffsetString(varLabelOffsetBox.Text, out offset);
				bool lengthGood = InputValidation.TryParseOffsetString(varLabelOffsetBox.Text, out length);
				if (offset == -1)
				{
					MessageBox.Show("Your entered offset was invalid. Make sure to type it properly.", "Error", MessageBoxButtons.OK);
				}
				else if (length == -1 && varLabelOffsetBox.Text.Equals(String.Empty))
				{
					MessageBox.Show("Your entered value was invalid. Make sure to type it properly.", "Error", MessageBoxButtons.OK);
				}
				else
				{
					labelContainer.RemoveLabel(fetch);
					labelContainer.AddVarLabel(offset, varLabelNameBox.Text, varLabelCommentBox.Lines);
					UpdateVarBoxView();
					FlipVarLabelTabControlVisibility();
					addButton.Visible = true;
				}
			}
			else
			{
				FlipVarLabelTabControlVisibility();
				addButton.Visible = false;
				varLabelNameBox.Text = fetch.Name;
				varLabelOffsetBox.Text = fetch.Variable.ToString("X");
				if (fetch.Comment != null)
				{
					if (fetch.Comment.Length == 1)
					{
						varLabelCommentBox.Text = fetch.Comment[0];
					}
					else
					{
						for (int i = 0; i < fetch.Comment.Length; i++)
						{
							if (i == 0) varLabelCommentBox.Text = fetch.Comment[i];
							else varLabelCommentBox.AppendText(Environment.NewLine + fetch.Comment[i]);
						}
					}
				}
			}
		}

		public void FlipVarLabelTabControlVisibility()
		{
			varLabelNameLabel.Visible = !varLabelNameLabel.Visible;
			varLabelOffsetLabel.Visible = !varLabelOffsetLabel.Visible;
			varLabelCommentLabel.Visible = !varLabelCommentLabel.Visible;
			varLabelBox.Visible = !varLabelBox.Visible;
			varLabelNameBox.Visible = !varLabelNameBox.Visible;
			varLabelOffsetBox.Visible = !varLabelOffsetBox.Visible;
			varLabelCommentBox.Visible = !varLabelCommentBox.Visible;
			cancelButton.Visible = !cancelButton.Visible;
			varLabelNameBox.Text = "";
			varLabelOffsetBox.Text = "";
			varLabelCommentBox.Text = "";
			if (varLabelOffsetBox.Visible) varLabelOffsetBox.Focus();
		}

		#endregion

		#region Toolstrip Menus
		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			openFileDialog1.Title = "Open GB/GBC File...";
			openFileDialog1.Filter = "GB/GBC Files|*.gb;*.gbc|All Files|*";
			openFileDialog1.FileName = "";
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				FileInfo fs = new FileInfo(openFileDialog1.FileName);
				romFile.LoadFile(openFileDialog1.FileName);
				this.Text = "GBRead - " + openFileDialog1.SafeFileName;
				UpdateMainTextBox(romFile.GetBinInfo(), TextBoxWriteMode.Overwrite);
				currentFileLoaded = openFileDialog1.FileName;
				labelContainer.LoadDefaultLabels(romFile.Length);
				UpdateDataBoxView();
				UpdateLabelBoxView();
				UpdateVarBoxView();
				startBox.Focus();
			}
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void loadFunctionListToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (romFile.FileLoaded)
			{
				openFileDialog1.Title = "Load Function/Data/Variable List...";
				openFileDialog1.Filter = "Function/Data/Variable List|*.txt|All Files|*";
				openFileDialog1.FileName = "";
				if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					labelContainer.LoadLabelFile(openFileDialog1.FileName);
					UpdateLabelBoxView();
					UpdateVarBoxView();
					UpdateDataBoxView();
				}
			}
			else
			{
				Error.ShowErrorMessage(ErrorMessage.NO_FILE);
			}
		}

		private void saveCalledFunctionsListToolStripMenuItem_Click(object sender, EventArgs e)
		{
			saveFileDialog1.Title = "Save Function/Data/Variable List...";
			saveFileDialog1.FileName = "";
			saveFileDialog1.Filter = "Function/Data/Variable List|*.txt|All Files|*";
			if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				labelContainer.SaveLabelFile(saveFileDialog1.FileName);
				MessageBox.Show(String.Format("Saved Functions and Data to{0}{1}", Environment.NewLine, saveFileDialog1.FileName), "Success", MessageBoxButtons.OK);
			}
		}

		private void saveEntireFileASMToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (romFile.FileLoaded)
			{
				saveFileDialog1.Title = "Save All ASM...";
				saveFileDialog1.Filter = "Text File|*.txt|All Files|*";
				saveFileDialog1.FileName = "";
				if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					PrintingFileASMThreadArgumentPack pfap = new PrintingFileASMThreadArgumentPack
					{
						SavedASMFileName = saveFileDialog1.FileName
					};
					if (!backgroundWorker1.IsBusy) backgroundWorker1.RunWorkerAsync(pfap);
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
				saveFileDialog1.Title = "Save GB/GBC File...";
				saveFileDialog1.Filter = "GB/GBC Files|*.gb;*.gbc|All Files|*";
				saveFileDialog1.FileName = "";
				if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					if (MessageBox.Show("Do you want to correct the header checksum and complement bytes as well?", "Fix Header", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
					{
						if (romFile is GBBinFile)
						{
							((GBBinFile)romFile).FixGBHeader();
						}
					}
					if (romFile.SaveFile(saveFileDialog1.FileName))
					{
						MessageBox.Show("File successfuly written to " + Environment.NewLine + saveFileDialog1.FileName);
					}
					else
					{
						MessageBox.Show("File could not be written to " + Environment.NewLine + saveFileDialog1.FileName);
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
			UpdateLabelBoxView();
			UpdateVarBoxView();
			UpdateDataBoxView();
		}

		private void exportToBinaryFileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			saveFileDialog1.Title = "Export Data Section To Binary...";
			saveFileDialog1.Filter = "Binary File|*.bin|All Files|*";
			saveFileDialog1.FileName = ((DataLabel)dataLabelBox.SelectedItem).Name;
			if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				DataLabel ds = (DataLabel)dataLabelBox.SelectedItem;
				if (romFile.SaveFilePortion(saveFileDialog1.FileName, ds.Offset, ds.Length))
				{
					MessageBox.Show("Binary file successfuly written to " + Environment.NewLine + saveFileDialog1.FileName);
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
				UpdateLabelBoxView();
			}
			else
			{
				Error.ShowErrorMessage(ErrorMessage.NO_FILE);
			}
		}
		#endregion

		private void printASMButton_Click(object sender, EventArgs e)
		{
			int start;
			int end;
			InputValidation.TryParseOffsetString(startBox.Text, out start);
			InputValidation.TryParseOffsetString(endBox.Text, out end);
			PrintingASMThreadArgumentPack parp = new PrintingASMThreadArgumentPack
			{
				Start = start, 
				End = end
			};
			if (!backgroundWorker1.IsBusy) backgroundWorker1.RunWorkerAsync(parp);
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

		private void offsetBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			if ((Keys)e.KeyChar == Keys.Enter)
			{
				if (addButton.Visible) addButton_Click(new object(), new EventArgs());
				else editButton_Click(new object(), new EventArgs());
				e.Handled = true;
			}
			if (!Char.IsControl(e.KeyChar) && !Char.IsDigit(e.KeyChar) && !(e.KeyChar >= 'A' && e.KeyChar <= 'F') && !(e.KeyChar >= 'a' && e.KeyChar <= 'f') && !(e.KeyChar == ':'))
			{
				e.Handled = true;
			}
		}

		private void nameBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			if ((Keys)e.KeyChar == Keys.Enter)
			{
				if (addButton.Visible) addButton_Click(new object(), new EventArgs());
				else editButton_Click(new object(), new EventArgs());
				e.Handled = true;
			}
		}

		private void lengthBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			if ((Keys)e.KeyChar == Keys.Enter)
			{
				if (addButton.Visible) addButton_Click(new object(), new EventArgs());
				else editButton_Click(new object(), new EventArgs());
				e.Handled = true;
			}
			if (!Char.IsControl(e.KeyChar) && !Char.IsDigit(e.KeyChar) && !(e.KeyChar >= 'A' && e.KeyChar <= 'F') && !(e.KeyChar >= 'a' && e.KeyChar <= 'f'))
			{
				e.Handled = true;
			}
		}

		private void dataLabelRowLengthBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			if ((Keys)e.KeyChar == Keys.Enter)
			{
				if (addButton.Visible) addButton_Click(new object(), new EventArgs());
				else editButton_Click(new object(), new EventArgs());
				e.Handled = true;
			}
			if (!Char.IsControl(e.KeyChar) && !Char.IsDigit(e.KeyChar) && !(e.KeyChar >= 'A' && e.KeyChar <= 'F') && !(e.KeyChar >= 'a' && e.KeyChar <= 'f') && !(e.KeyChar == ':'))
			{
				e.Handled = true;
			}
		}

		private void codeLabelBox_DrawItem(object sender, DrawItemEventArgs e)
		{
			e.DrawBackground();
			if (codeLabelBox.Items.Count > 0)
			{
				CodeLabel ds = (CodeLabel)codeLabelBox.Items[e.Index];
				Brush itemBrush = Brushes.Black;
				StringFormat sf = new StringFormat();
				sf.Alignment = StringAlignment.Near;
				sf.LineAlignment = StringAlignment.Center;
				if (ds.Length > 0) itemBrush = Brushes.Green;
				e.Graphics.DrawString(ds.ToString(), e.Font, itemBrush, codeLabelBox.GetItemRectangle(e.Index), sf);
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
					case DataSectionType.Img:
						itemBrush = Brushes.DeepPink;
						break;
					case DataSectionType.Text:
						itemBrush = Brushes.ForestGreen;
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

		private void addButton_Click(object sender, EventArgs e)
		{
			if (romFile.FileLoaded)
			{
				switch (LabelTabControl.SelectedIndex)
				{
					case 0:
						codeTabAddCancelButton();
						break;
					case 1:
						dataTabAddCancelButton();
						break;
					case 2:
						varTabAddCancelButton();
						break;
					default:
						break;
				}
			}
			else
			{
				Error.ShowErrorMessage(ErrorMessage.NO_FILE);
			}
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			if (romFile.FileLoaded)
			{
				switch (LabelTabControl.SelectedIndex)
				{
					case 0:
						if (editButton.Visible) addButton.Visible = true;
						else editButton.Visible = true;
						FlipCodeLabelTabControlVisibility();
						break;
					case 1:
						if (editButton.Visible) addButton.Visible = true;
						else editButton.Visible = true;
						FlipDataLabelTabControlVisibility();
						break;
					case 2:
						if (editButton.Visible) addButton.Visible = true;
						else editButton.Visible = true;
						FlipVarLabelTabControlVisibility();
						break;
					default:
						break;
				}
			}
			else
			{
				Error.ShowErrorMessage(ErrorMessage.NO_FILE);
			}
		}

		private void editButton_Click(object sender, EventArgs e)
		{
			if (romFile.FileLoaded)
			{
				switch (LabelTabControl.SelectedIndex)
				{
					case 0:
						if (codeLabelBox.SelectedItem != null) codeTabEditFunction();
						break;
					case 1:
						if (dataLabelBox.SelectedItem != null) dataTabEditFunction();
						break;
					case 2:
						if (varLabelBox.SelectedItem != null) varTabEditFunction();
						break;
					default:
						break;
				}
			}
			else
			{
				Error.ShowErrorMessage(ErrorMessage.NO_FILE);
			}
		}

		private void LabelTabControl_Selecting(object sender, TabControlCancelEventArgs e)
		{
			if (cancelButton.Visible)
			{
				e.Cancel = true;
			}
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
				UpdateMainTextBox(disassembler.SearchForFunctionCall((CodeLabel)codeLabelBox.SelectedItem), TextBoxWriteMode.Overwrite);
			}
		}

		#endregion

		#region List Management
		private void UpdateDataBoxView()
		{
			((CurrencyManager)dataLabelBox.BindingContext[labelContainer.DataList]).Refresh();
		}

		private void UpdateLabelBoxView()
		{
			((CurrencyManager)codeLabelBox.BindingContext[labelContainer.LabelList]).Refresh();
		}

		private void UpdateVarBoxView()
		{
			((CurrencyManager)varLabelBox.BindingContext[labelContainer.VarList]).Refresh();
		}

		#endregion

		#region Asynchronous Operations
		private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
		{
			System.ComponentModel.BackgroundWorker worker = sender as System.ComponentModel.BackgroundWorker;
			if (e.Argument is PrintingFunctionsThreadArgumentPack)
			{
				PrintFunctions_Threaded(worker, e);
			}
			else if (e.Argument is PrintingASMThreadArgumentPack)
			{
				PrintASM_Threaded(worker, e);
			}
			else if (e.Argument is PrintingFileASMThreadArgumentPack)
			{
				PrintASMToFile_Threaded(worker, e);
			}
		}

		private void PrintASM_Threaded(System.ComponentModel.BackgroundWorker worker, System.ComponentModel.DoWorkEventArgs e)
		{
			int start = (e.Argument as PrintingASMThreadArgumentPack).Start;
			int end = (e.Argument as PrintingASMThreadArgumentPack).End;
			if (!worker.CancellationPending && romFile.FileLoaded)
			{
				if (start < 0 || end < 0)
				{
					Error.ShowErrorMessage(ErrorMessage.START_OR_END_INVALID);
					e.Cancel = true;
				}
				else if (start < 0 || start > romFile.Length)
				{
					Error.ShowErrorMessage(ErrorMessage.START_INVALID);
					e.Cancel = true;
				}
				else if (end <= 0 || end >= romFile.Length)
				{
					Error.ShowErrorMessage(ErrorMessage.END_INVALID);
					e.Cancel = true;
				}
				else if (end < start)
				{
					Error.ShowErrorMessage(ErrorMessage.START_AFTER_END);
					e.Cancel = true;
				}
				else
				{
					String tempText = String.Empty;
					worker.ReportProgress(0);
					tempText = disassembler.PrintASM(start, end - start);
					e.Result = tempText;
				}
			}
			else
			{
				Error.ShowErrorMessage(ErrorMessage.NO_FILE);
				e.Cancel = true;
			}
		}

		private void PrintFunctions_Threaded(System.ComponentModel.BackgroundWorker worker, System.ComponentModel.DoWorkEventArgs e)
		{
			if (!worker.CancellationPending)
			{
				CodeLabel c = (e.Argument as PrintingFunctionsThreadArgumentPack).labelToBePrinted;
				worker.ReportProgress(0);
				string result = String.Empty;
				result = disassembler.ShowCodeLabel(c);
				e.Result = result;
			}
		}

		private void PrintASMToFile_Threaded(System.ComponentModel.BackgroundWorker worker, System.ComponentModel.DoWorkEventArgs e)
		{
			worker.ReportProgress(0);
			if (!worker.CancellationPending && romFile.FileLoaded)
			{
				bool tempHFVal = disassembler.HideDefinedFunctions;
				bool tempHDVal = disassembler.HideDefinedData;
				disassembler.HideDefinedFunctions = false;
				disassembler.HideDefinedData = false;
				using (StreamWriter ot = new StreamWriter(((PrintingFileASMThreadArgumentPack)e.Argument).SavedASMFileName, false))
				{
					foreach (VarLabel v in labelContainer.VarList)
					{
						ot.WriteLine(v.Name + "\tEQU\t$" + v.Variable.ToString("X"));
					}
					ot.WriteLine(disassembler.PrintASM(0, romFile.Length));
				}
				//StringBuilder ot = new StringBuilder();
				//foreach (VarLabel v in labelContainer.VarList)
				//{
				//    ot.AppendLine(v.Name + "\tEQU\t$" + v.Variable.ToString("X"));
				//}
				//ot.Append(disassembler.PrintASM(0, romFile.ROMSize));
				//File.WriteAllText((e.Argument as PrintingFileASMThreadArgumentPack).SavedASMFileName, ot.ToString());
				MessageBox.Show(String.Format("Saved All ASM to{0}{1}", Environment.NewLine, saveFileDialog1.FileName), "Success", MessageBoxButtons.OK);
				disassembler.HideDefinedFunctions = tempHFVal;
				disassembler.HideDefinedData = tempHDVal;
			}
		}

		private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
		{
			UpdateProgressLabel("Working...");
		}

		private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
		{
			progressLabel.Text = String.Empty;
			if (!e.Cancelled) UpdateMainTextBox((string)e.Result, TextBoxWriteMode.Overwrite);
		}

		delegate void UpdateTextBoxDelegate(string updateText, TextBoxWriteMode overwriteExistingText);
		
		private void UpdateProgressLabel(string newLabel, TextBoxWriteMode overwriteExistingText = TextBoxWriteMode.Append)
		{
			if (progressLabel.InvokeRequired)
			{
				UpdateTextBoxDelegate del = new UpdateTextBoxDelegate(UpdateProgressLabel);
				progressLabel.Invoke(del, new object[] { newLabel, false });
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
				progressLabel.Invoke(del, new object[] { text, overwriteExistingText });
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

		#endregion	

		private void findReferencesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DataLabel selectedLabel = (DataLabel)dataLabelBox.SelectedItem;
			UpdateMainTextBox(disassembler.SearchForReference(selectedLabel), TextBoxWriteMode.Overwrite);
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
