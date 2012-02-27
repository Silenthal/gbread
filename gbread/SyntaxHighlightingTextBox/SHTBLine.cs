using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GBRead
{
	/// <summary>
	/// Line of text
	/// </summary>
	public class SHTBLine : List<SHTBChar>
	{
		List<int> cutOffPositions;

		/// <summary>
		/// Text of line was changed
		/// </summary>
		public bool IsChanged { get; set; }

		//Y coordinate of line on screen
		internal int startY = -1;

		/// <summary>
		/// AutoIndent level for this line
		/// </summary>
		public int IndentLevel { get; set; }

		/// <summary>
		/// Background brush.
		/// </summary>
		public Brush BackgroundBrush { get; set; }

		/// <summary>
		/// Unique ID
		/// </summary>
		public int UniqueId { get; private set; }

		internal SHTBLine(int uid)
		{
			this.UniqueId = uid;
		}

		/// <summary>
		/// Clears style of chars.
		/// </summary>
		public void ClearStyle(StyleIndex styleIndex)
		{
			for (int i = 0; i < Count; i++)
			{
				SHTBChar c = this[i];
				c.Style &= ~styleIndex;
				this[i] = c;
			}
		}

		/// <summary>
		/// Text of the line
		/// </summary>
		public string Text
		{
			get
			{
				StringBuilder sb = new StringBuilder(Count);
				foreach (SHTBChar c in this)
					sb.Append(c.Character);
				return sb.ToString();
			}
		}

		/// <summary>
		/// Positions for wordwrap cutoffs
		/// </summary>
		public List<int> CutOffPositions
		{
			get
			{
				if (cutOffPositions == null)
					cutOffPositions = new List<int>();
				return cutOffPositions;
			}
		}

		/// <summary>
		/// Count of wordwrap string count for this line
		/// </summary>
		public int WordWrapStringsCount
		{
			get
			{
				if (cutOffPositions == null)
					return 1;
				return cutOffPositions.Count + 1;
			}
		}

		internal int GetWordWrapStringStartPosition(int iWordWrapLine)
		{
			return iWordWrapLine == 0 ? 0 : CutOffPositions[iWordWrapLine - 1];
		}

		internal int GetWordWrapStringFinishPosition(int iWordWrapLine)
		{
			if (WordWrapStringsCount <= 0)
				return 0;
			return iWordWrapLine == WordWrapStringsCount - 1 ? Count - 1 : CutOffPositions[iWordWrapLine] - 1;
		}

		/// <summary>
		/// Gets index of wordwrap string for given char position
		/// </summary>
		public int GetWordWrapStringIndex(int iChar)
		{
			if (cutOffPositions == null || cutOffPositions.Count == 0) return 0;
			for (int i = 0; i < cutOffPositions.Count; i++)
				if (cutOffPositions[i] > iChar)
					return i;
			return cutOffPositions.Count;
		}

		/// <summary>
		/// Calculates wordwrap cutoffs
		/// </summary>
		internal void CalcCutOffs(int maxCharsPerLine, bool allowIME)
		{
			int segmentLength = 0;
			int cutOff = 0;
			CutOffPositions.Clear();

			for (int i = 0; i < Count; i++)
			{
				char c = this[i].Character;
				//word wrapping
				if (allowIME && isCJKLetter(c))//in CJK languages cutoff can be in any letter
				{
					cutOff = i;
				}
				else
				{
					if (!char.IsLetterOrDigit(c) && c != '_')
					{
						cutOff = Math.Min(i + 1, Count - 1);
					}
				}
				segmentLength++;

				if (segmentLength == maxCharsPerLine)
				{
					if (cutOff == 0 || (cutOffPositions.Count > 0 && cutOff == cutOffPositions[cutOffPositions.Count - 1]))
						cutOff = i + 1;
					CutOffPositions.Add(cutOff);
					segmentLength = 1 + i - cutOff;
				}
			}
		}

		private bool isCJKLetter(char c)
		{
			int code = Convert.ToInt32(c);
			return
			(code >= 0x3300 && code <= 0x33FF) ||
			(code >= 0xFE30 && code <= 0xFE4F) ||
			(code >= 0xF900 && code <= 0xFAFF) ||
			(code >= 0x2E80 && code <= 0x2EFF) ||
			(code >= 0x31C0 && code <= 0x31EF) ||
			(code >= 0x4E00 && code <= 0x9FFF) ||
			(code >= 0x3400 && code <= 0x4DBF) ||
			(code >= 0x3200 && code <= 0x32FF) ||
			(code >= 0x2460 && code <= 0x24FF) ||
			(code >= 0x3040 && code <= 0x309F) ||
			(code >= 0x2F00 && code <= 0x2FDF) ||
			(code >= 0x31A0 && code <= 0x31BF) ||
			(code >= 0x4DC0 && code <= 0x4DFF) ||
			(code >= 0x3100 && code <= 0x312F) ||
			(code >= 0x30A0 && code <= 0x30FF) ||
			(code >= 0x31F0 && code <= 0x31FF) ||
			(code >= 0x2FF0 && code <= 0x2FFF) ||
			(code >= 0x1100 && code <= 0x11FF) ||
			(code >= 0xA960 && code <= 0xA97F) ||
			(code >= 0xD7B0 && code <= 0xD7FF) ||
			(code >= 0x3130 && code <= 0x318F) ||
			(code >= 0xAC00 && code <= 0xD7AF);
		}

		/// <summary>
		/// Count of start spaces
		/// </summary>
		public int StartSpacesCount
		{
			get
			{
				int spacesCount = 0;
				for (int i = 0; i < Count; i++)
					if (this[i].Character == ' ')
						spacesCount++;
					else
						break;
				return spacesCount;
			}
		}
	}
}