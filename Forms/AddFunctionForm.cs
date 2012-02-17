using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GBRead.Base;

namespace GBRead.Forms
{
	public partial class AddFunctionForm : Form
	{
		LabelContainer refContainer;
		public AddFunctionForm(LabelContainer lc)
		{
			InitializeComponent();
			refContainer = lc;
		}

		private void nameBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{

			}
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			if (nameBox.Text == "")
			{
				MessageBox.Show("Please type a name for the label.", "Error", MessageBoxButtons.OK);
			}
			else if (!RegularValidation.IsWord(nameBox.Text))
			{
				MessageBox.Show("The name is not valid (First letter can be letter, following can be letters");
			}
			else if (offsetBox.Text == "")
			{
				MessageBox.Show("Please type an offset for the label.", "Error", MessageBoxButtons.OK);
			}
		}
	}
}
