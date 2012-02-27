using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GBRead.Base;
using System.Globalization;

namespace GBRead.Forms
{
	public partial class AddVariableForm : Form
	{
		Disassembler dc;
		LabelContainer refContainer;
		LabelEditMode lem;
		VarLabel priorLabel;
		ListBox.ObjectCollection oc;
		public AddVariableForm(Disassembler dcs, LabelContainer lc, ListBox.ObjectCollection ocs, LabelEditMode nlem, VarLabel newPriorLabel = null)
		{
			InitializeComponent();
			dc = dcs;
			refContainer = lc;
			oc = ocs;
			if (nlem == LabelEditMode.Edit)
			{
				Text = "Edit Variable";
				lem = nlem;
				priorLabel = newPriorLabel;

				if (priorLabel != null)
				{
					refContainer.RemoveLabel(priorLabel);
					oc.Remove(priorLabel);
					nameBox.Text = priorLabel.Name;
					offsetBox.Text = priorLabel.Value.ToString("X");
					dataTypeBox.SelectedIndex = (int)priorLabel.VarType;
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
			int val = 0;
			if (!RegularValidation.IsWord(nameBox.Text))
			{
				Error.ShowErrorMessage(ErrorMessage.NAME_IS_INVALID);
			}
			else if (refContainer.IsSymbolDefined(nameBox.Text))
			{
				Error.ShowErrorMessage(ErrorMessage.NAME_ALREADY_DEFINED);
			}
			else if (!Int32.TryParse(offsetBox.Text, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out val))
			{
				Error.ShowErrorMessage(ErrorMessage.VARIABLE_IS_INVALID);
			}
			else
			{
				priorLabel = new VarLabel(val, nameBox.Text, (VariableType)dataTypeBox.SelectedIndex, commentBox.Lines);
				refContainer.AddLabel(priorLabel);
				oc.Add(priorLabel);
				this.DialogResult = System.Windows.Forms.DialogResult.OK;
			}
		}
	}
}
