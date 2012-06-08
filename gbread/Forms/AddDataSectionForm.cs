using System;
using System.Globalization;
using System.Windows.Forms;
using GBRead.Base;

namespace GBRead.Forms
{
	public partial class AddDataLabelForm : Form
	{
		DataLabel editedLabel;
		LabelEditMode editingMode;
		LabelContainer labelContainer;
		ListBox.ObjectCollection listBoxLabelCollection;

		public AddDataLabelForm(LabelContainer lblContainer, ListBox.ObjectCollection lbLabelCollection, LabelEditMode editMode, DataLabel newPriorLabel = null)
		{
			InitializeComponent();
			labelContainer = lblContainer;
			listBoxLabelCollection = lbLabelCollection;
			editingMode = editMode;
			editedLabel = newPriorLabel;
			if (editMode == LabelEditMode.Edit)
			{
				Text = "Edit Data Section";
				if (editedLabel != null)
				{
					nameBox.Text = editedLabel.Name;
					offsetBox.Text = editedLabel.Offset.ToString("X");
					lengthBox.Text = editedLabel.Length.ToString("X");
					dataTypeBox.SelectedIndex = (int)editedLabel.DSectionType;
					rowLengthBox.Text = editedLabel.DataLineLength.ToString("X");
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
			int len = 0;
			int rlen = 0;
			if (!RegularValidation.IsWord(nameBox.Text))
			{
				Error.ShowErrorMessage(ErrorMessage.NAME_IS_INVALID);
			}
			else if (checkNameCollision && labelContainer.IsNameDefined(nameBox.Text))
			{
				Error.ShowErrorMessage(ErrorMessage.NAME_ALREADY_DEFINED);
			}
			else if (!InputValidation.TryParseOffsetString(offsetBox.Text, out off))
			{
				Error.ShowErrorMessage(ErrorMessage.OFFSET_IS_INVALID);
			}
			else if (!Int32.TryParse(lengthBox.Text, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out len) || len <= 0)
			{
				Error.ShowErrorMessage(ErrorMessage.LENGTH_IS_INVALID);
			}
			else if (!Int32.TryParse(rowLengthBox.Text, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out rlen) || rlen <= 0)
			{
				Error.ShowErrorMessage(ErrorMessage.ROW_LENGTH_IS_INVALID);
			}
			else
			{
				if (editingMode == LabelEditMode.Edit)
				{
					labelContainer.RemoveDataLabel(editedLabel);
					listBoxLabelCollection.Remove(editedLabel);
				}
				editedLabel = new DataLabel(off, len, nameBox.Text, rlen, commentBox.Lines, (DataSectionType)dataTypeBox.SelectedIndex);
				labelContainer.AddDataLabel(editedLabel);
				listBoxLabelCollection.Add(editedLabel);
				this.DialogResult = System.Windows.Forms.DialogResult.OK;
			}
		}
	}
}