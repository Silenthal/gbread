using System.Drawing;
using System.Text.RegularExpressions;

namespace GBRead
{
	public class Z80SyntaxHighlighter
	{
		#region Text Styles
		private static SHTBStyle LightBlueStyle = new SHTBTextStyle(Brushes.LightBlue, null, FontStyle.Regular);
		private static SHTBStyle BlueStyle = new SHTBTextStyle(Brushes.Blue, null, FontStyle.Regular);
		private static SHTBStyle BoldStyle = new SHTBTextStyle(null, null, FontStyle.Bold);
		private static SHTBStyle GrayStyle = new SHTBTextStyle(Brushes.Gray, null, FontStyle.Regular);
		private static SHTBStyle MagentaStyle = new SHTBTextStyle(Brushes.Magenta, null, FontStyle.Regular);
		private static SHTBStyle GreenStyle = new SHTBTextStyle(Brushes.Green, null, FontStyle.Regular);
		private static SHTBStyle BrownStyle = new SHTBTextStyle(Brushes.Brown, null, FontStyle.Regular);
		private static SHTBStyle RedStyle = new SHTBTextStyle(Brushes.Red, null, FontStyle.Regular);
		private static SHTBStyle MaroonStyle = new SHTBTextStyle(Brushes.Maroon, null, FontStyle.Regular);
		private static SHTBStyle RegularStyle = new SHTBTextStyle(Brushes.Black, null, FontStyle.Regular);
		#endregion

		#region Regexes for each variable type
		private static Regex Z80CommentRegex = new Regex(@";.*$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);
		private static Regex Z80KeywordRegex = new Regex(@"\b(adc|add|and|bit|call|ccf|cp|cpl|daa|dec|di|ei|halt|inc|jp|jr|ld|ldd|ldi|ldhl|nop|or|pop|push|res|ret|reti|rl|rla|rlca|rr|rra|rrca|rst|sbc|scf|set|sla|sra|srl|sub|swap|stop|xor)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
		private static Regex Z80DataKeywordRegex = new Regex(@"\b(db|dw)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);
		private static Regex Z80NumberRegex = new Regex(@"((\B\$|\b0x)[0-9a-f]+|\b[0-9]+)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
		private static Regex Z80BankNumberRegex = new Regex(@"^[0-9a-f]{2}:[0-9a-f]{4}\b", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);
		private static Regex Z80LabelRegex = new Regex(@"^\w+:( |\n|\r|$)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);
		private static Regex Z80RegisterRegex = new Regex(@"\b(a|b|c|d|e|h|l|af|bc|de|hl|sp)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
		#endregion

		public bool HighlightComments { get; set; }
		public bool HighlightOffsets { get; set; }
		public bool HighlightNumbers { get; set; }
		public bool HighlightKeywords { get; set; }
		public bool HighlightLabels { get; set; }
		public bool HighlightRegisters { get; set; }

		public Z80SyntaxHighlighter()
		{
			HighlightComments = true;
			HighlightLabels = true;
			HighlightOffsets = false;
			HighlightNumbers = false;
			HighlightKeywords = true;	
			HighlightRegisters = false;
		}

		/// <summary>
		/// Highlights Z80 code.
		/// </summary>
		/// <param name="range">The range to highlight.</param>
		public void HighlightSyntax(SHTBRange range)
		{
			range.ClearStyle(BlueStyle, BoldStyle, GrayStyle, MagentaStyle, RedStyle, GreenStyle, BrownStyle);
			if (HighlightComments) range.SetStyle(GreenStyle, Z80CommentRegex);
			if (HighlightLabels) range.SetStyle(BrownStyle, Z80LabelRegex);
			if (HighlightOffsets) range.SetStyle(BoldStyle, Z80BankNumberRegex);
			if (HighlightNumbers) range.SetStyle(MagentaStyle, Z80NumberRegex);
			if (HighlightRegisters) range.SetStyle(RedStyle, Z80RegisterRegex);
			if (HighlightKeywords)
			{
				range.SetStyle(BlueStyle, Z80KeywordRegex);
				range.SetStyle(LightBlueStyle, Z80DataKeywordRegex);
			}
			
		}
	}
}