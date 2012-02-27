using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using GBRead.Base;

namespace GBRead.Forms
{
	public partial class AddDataSectionForm : Form
	{
		Disassembler dc;
		ListBox.ObjectCollection oc;
		LabelContainer refContainer;
		LabelEditMode lem;
		DataLabel priorLabel;
		public AddDataSectionForm(Disassembler dcs, LabelContainer lc, ListBox.ObjectCollection ocs, LabelEditMode nlem, DataLabel newPriorLabel = null)
		{
			InitializeComponent();
			dc = dcs;
			refContainer = lc;
			oc = ocs;
			if (nlem == LabelEditMode.Edit)
			{
				Text = "Edit Data Section";
				lem = nlem;
				priorLabel = newPriorLabel;
				if (priorLabel != null)
				{
					refContainer.RemoveLabel(priorLabel);
					oc.Remove(priorLabel);
					nameBox.Text = priorLabel.Name;
					offsetBox.Text = priorLabel.Offset.ToString("X");
					lengthBox.Text = priorLabel.Length.ToString("X");
					dataTypeBox.SelectedIndex = (int)priorLabel.DSectionType;
					rowLengthBox.Text = priorLabel.DataLineLength.ToString("X");
					if (priorLabel.Comment != null)
					{
						for (int i = 0; i < priorLabel.Comment.Length; i++)
						{
							commentBox.Text += priorLabel.Comment[i];
							if (i != priorLabel.Comment.Length - 1) commentBox.Text += Environment.NewLine;
						}
					}
				}
			}
			else
			{
				priorLabel = null;
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
			int off = -1;
			int len = 0;
			int rlen = 0;
			if (!RegularValidation.IsWord(nameBox.Text))
			{
				Error.ShowErrorMessage(ErrorMessage.NAME_IS_INVALID);
			}
			else if (refContainer.IsSymbolDefined(nameBox.Text))
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
				priorLabel = new DataLabel(off, len, nameBox.Text, rlen, commentBox.Lines, (DataSectionType)dataTypeBox.SelectedIndex);
				refContainer.AddLabel(priorLabel);
				oc.Add(priorLabel);
				this.DialogResult = System.Windows.Forms.DialogResult.OK;
			}
		}
	}
}
