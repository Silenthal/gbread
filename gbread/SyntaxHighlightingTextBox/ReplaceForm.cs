using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace GBRead
{
	public partial class ReplaceForm : Form
	{
		SyntaxHighlightingTextBox tb;
		bool firstSearch = true;
		SHTBPlace startPlace;

		public ReplaceForm(SyntaxHighlightingTextBox tb)
		{
			InitializeComponent();
			this.tb = tb;
		}

		private void btClose_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void btFindNext_Click(object sender, EventArgs e)
		{
			try
			{
				if(!Find())
					MessageBox.Show("Not found");
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		List<SHTBRange> FindAll()
		{
			string pattern = tbFind.Text;
			RegexOptions opt = cbMatchCase.Checked ? RegexOptions.None : RegexOptions.IgnoreCase;
			if (!cbRegex.Checked)
			{
				pattern = Regex.Replace(pattern, GBRead.FindForm.RegexSpecSymbolsPattern, "\\$0");
			} 
			if (cbWholeWord.Checked)
			{
				pattern = "\\b" + pattern + "\\b";
			}//
			SHTBRange range = tb.Selection.Clone();
			range.Normalize();
			range.Start = range.End;
			range.End = new SHTBPlace(tb.GetLineLength(tb.LinesCount - 1), tb.LinesCount - 1);
			//
			List<SHTBRange> list = new List<SHTBRange>();
			Regex reg = new Regex(pattern, opt);
			foreach (var r in range.GetRanges(reg))
			{
				list.Add(r);
			}
			return list;
		}

		bool Find()
		{
			string pattern = tbFind.Text;
			RegexOptions opt = cbMatchCase.Checked ? RegexOptions.None : RegexOptions.IgnoreCase;
			if (!cbRegex.Checked)
				pattern = Regex.Replace(pattern, GBRead.FindForm.RegexSpecSymbolsPattern, "\\$0");
			if (cbWholeWord.Checked)
				pattern = "\\b" + pattern + "\\b";
			//
			SHTBRange range = tb.Selection.Clone();
			range.Normalize();
			//
			if (firstSearch)
			{
				startPlace = range.Start;
				firstSearch = false;
			}
			//
			range.Start = range.End;
			if (range.Start >= startPlace)
			{
				range.End = new SHTBPlace(tb.GetLineLength(tb.LinesCount - 1), tb.LinesCount - 1);
			}
			else
			{
				range.End = startPlace;
			}//
			Regex reg = new Regex(pattern, opt);
			foreach (var r in range.GetRanges(reg))
			{
				tb.Selection.Start = r.Start;
				tb.Selection.End = r.End;
				tb.DoSelectionVisible();
				tb.Invalidate();
				return true;
			}
			if (range.Start >= startPlace && startPlace > SHTBPlace.Empty)
			{
				tb.Selection.Start = new SHTBPlace(0, 0);
				return Find();
			}
			return false;
		}

		private void tbFind_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == '\r')
			{
				btFindNext_Click(sender, null);
			} 
			if (e.KeyChar == '\x1b')
			{
				Hide(); 
			}
		}

		private void FindForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.UserClosing)
			{
				e.Cancel = true;
				Hide();
			}
		}

		private void btReplace_Click(object sender, EventArgs e)
		{
			try
			{
				if (tb.SelectionLength != 0)
				{
					tb.InsertText(tbReplace.Text);
				}
				btFindNext_Click(sender, null);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void btReplaceAll_Click(object sender, EventArgs e)
		{
			try
			{
				tb.Selection.BeginUpdate();
				tb.Selection.Start = new SHTBPlace(0, 0);
				//search
				var ranges = FindAll();
				//replace
				if (ranges.Count > 0)
				{
					tb.manager.ExecuteCommand(new ReplaceTextCommand(tb, ranges, tbReplace.Text));
					tb.Selection.Start = new SHTBPlace(0, 0);
				}
				//
				tb.Invalidate();
				MessageBox.Show(ranges.Count + " occurrence(s) replaced");
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			tb.Selection.EndUpdate();
		}

		protected override void OnActivated(EventArgs e)
		{
			tbFind.Focus();
			ResetSerach();
		}

		void ResetSerach()
		{
			firstSearch = true;
		}

		private void cbMatchCase_CheckedChanged(object sender, EventArgs e)
		{
			ResetSerach();
		}
	}
}
