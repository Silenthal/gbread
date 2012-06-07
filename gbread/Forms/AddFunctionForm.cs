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
	public enum LabelEditMode { Add, Edit }
	public partial class AddFunctionLabelForm : Form
	{
		Disassembler dc;
		LabelContainer refContainer;
		LabelEditMode lem;
		FunctionLabel priorLabel;
		ListBox.ObjectCollection oc;
		public AddFunctionLabelForm(Disassembler dcs, LabelContainer lc, ListBox.ObjectCollection ocs, LabelEditMode nlem, FunctionLabel newPriorLabel = null)
		{
			InitializeComponent();
			dc = dcs;
			refContainer = lc;
			oc = ocs;
			if (nlem == LabelEditMode.Edit)
			{
				Text = "Edit Function";
				lem = nlem;
				priorLabel = newPriorLabel;
				if (priorLabel != null)
				{
					refContainer.RemoveFuncLabel(priorLabel);
					oc.Remove(priorLabel);
					nameBox.Text = priorLabel.Name;
					offsetBox.Text = priorLabel.Offset.ToString("X");
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
			if (!RegularValidation.IsWord(nameBox.Text))
			{
				Error.ShowErrorMessage(ErrorMessage.NAME_IS_INVALID);
			}
			else if (refContainer.IsNameDefined(nameBox.Text))
			{
				Error.ShowErrorMessage(ErrorMessage.NAME_ALREADY_DEFINED);
			}
			else if (!InputValidation.TryParseOffsetString(offsetBox.Text, out off))
			{
				Error.ShowErrorMessage(ErrorMessage.OFFSET_IS_INVALID);
			}
			else
			{
				priorLabel = new FunctionLabel(off, nameBox.Text, commentBox.Lines);
				refContainer.AddFuncLabel(priorLabel);
				oc.Add(priorLabel);
				this.DialogResult = System.Windows.Forms.DialogResult.OK;
			}
		}
	}
}
