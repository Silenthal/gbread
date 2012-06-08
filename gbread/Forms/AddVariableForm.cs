﻿using System;
using System.Globalization;
using System.Windows.Forms;
using GBRead.Base;

namespace GBRead.Forms
{
	public partial class AddVarLabelForm : Form
	{
		VarLabel editedLabel;
		LabelEditMode editingMode;
		LabelContainer labelContainer;
		ListBox.ObjectCollection listBoxLabelCollection;

		public AddVarLabelForm(LabelContainer lblContainer, ListBox.ObjectCollection lbLabelCollection, LabelEditMode editMode, VarLabel newPriorLabel = null)
		{
			InitializeComponent();
			labelContainer = lblContainer;
			listBoxLabelCollection = lbLabelCollection;
			editingMode = editMode;
			editedLabel = newPriorLabel;
			if (editMode == LabelEditMode.Edit)
			{
				Text = "Edit Variable";
				if (editedLabel != null)
				{
					nameBox.Text = editedLabel.Name;
					offsetBox.Text = editedLabel.Value.ToString("X");
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
			int val = 0;
			if (!RegularValidation.IsWord(nameBox.Text))
			{
				Error.ShowErrorMessage(ErrorMessage.NAME_IS_INVALID);
			}
			else if (checkNameCollision && labelContainer.IsNameDefined(nameBox.Text))
			{
				Error.ShowErrorMessage(ErrorMessage.NAME_ALREADY_DEFINED);
			}
			else if (!Int32.TryParse(offsetBox.Text, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out val))
			{
				Error.ShowErrorMessage(ErrorMessage.VARIABLE_IS_INVALID);
			}
			else
			{
				if (editingMode == LabelEditMode.Edit)
				{
					labelContainer.RemoveVarLabel(editedLabel);
					listBoxLabelCollection.Remove(editedLabel);
				}
				editedLabel = new VarLabel(val, nameBox.Text, commentBox.Lines);
				labelContainer.AddVarLabel(editedLabel);
				listBoxLabelCollection.Add(editedLabel);
				this.DialogResult = System.Windows.Forms.DialogResult.OK;
			}
		}
	}
}