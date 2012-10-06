using System;
using System.Windows.Forms;
using GBRead.Base;

namespace GBRead.Forms
{
	public enum LabelEditMode { Add, Edit }

	public partial class AddFunctionLabelForm : Form
	{
		FunctionLabel editedLabel;
		LabelEditMode editingMode;
		LabelContainer labelContainer;

		public AddFunctionLabelForm(LabelContainer lblContainer, LabelEditMode editMode, FunctionLabel newPriorLabel = null)
		{
			InitializeComponent();
			labelContainer = lblContainer;
			editingMode = editMode;
			editedLabel = newPriorLabel;
			if (editingMode == LabelEditMode.Edit)
			{
				Text = "Edit Label";
				if (editedLabel != null)
				{
					nameBox.Text = editedLabel.Name;
					offsetBox.Text = editedLabel.Offset.ToString("X");
					if (editedLabel.Comment != null)
					{
						for (int i = 0; i < editedLabel.Comment.Length; i++)
						{
							commentBox.Text += editedLabel.Comment[i];
							if (i != editedLabel.Comment.Length - 1) commentBox.Text += Environment.NewLine;
						}
					}
				}
			}
		}

		private void formBox_keyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				okButton_Click(new object(), EventArgs.Empty);
			}
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			bool checkNameCollision = editingMode == LabelEditMode.Add ?
				true :
				!nameBox.Text.Equals(editedLabel.Name, StringComparison.Ordinal);
			int off = -1;
			if (!RegularValidation.IsWord(nameBox.Text))
			{
				Error.ShowErrorMessage(ErrorMessage.NAME_IS_INVALID);
			}
			else if (checkNameCollision && labelContainer.SymbolList.ContainsKey(nameBox.Text))
			{
				Error.ShowErrorMessage(ErrorMessage.NAME_ALREADY_DEFINED);
			}
			else if (!InputValidation.TryParseOffsetString(offsetBox.Text, out off))
			{
				Error.ShowErrorMessage(ErrorMessage.OFFSET_IS_INVALID);
			}
			else
			{
				if (editingMode == LabelEditMode.Edit)
				{
					labelContainer.RemoveFuncLabel(editedLabel);
				}
				editedLabel = new FunctionLabel(off, nameBox.Text, commentBox.Text);
				labelContainer.AddFuncLabel(editedLabel);
				this.DialogResult = System.Windows.Forms.DialogResult.OK;
			}
		}
	}
}