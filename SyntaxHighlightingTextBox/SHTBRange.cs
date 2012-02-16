using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace GBRead
{
	/// <summary>
	/// Diapason of text chars
	/// </summary>
	public class SHTBRange : IEnumerable<SHTBPlace>
	{
		SHTBPlace start;
		SHTBPlace end;
		public readonly SyntaxHighlightingTextBox AssociatedTextBox;
		int preferredPos = -1;
		int updating = 0;

		string cachedText;
		List<SHTBPlace> cachedCharIndexToPlace;
		int cachedTextVersion = -1;

		/// <summary>
		/// Constructor
		/// </summary>
		public SHTBRange(SyntaxHighlightingTextBox tb)
		{
			AssociatedTextBox = tb;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public SHTBRange(SyntaxHighlightingTextBox tb, int iStartChar, int iStartLine, int iEndChar, int iEndLine)
			: this(tb)
		{
			start = new SHTBPlace(iStartChar, iStartLine);
			end = new SHTBPlace(iEndChar, iEndLine);
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public SHTBRange(SyntaxHighlightingTextBox tb, SHTBPlace start, SHTBPlace end)
			: this(tb)
		{
			this.start = start;
			this.end = end;
		}

		/// <summary>
		/// Returns intersection with other range,
		/// empty range returned otherwise
		/// </summary>
		/// <param name="range"></param>
		/// <returns></returns>
		public SHTBRange GetIntersectionWith(SHTBRange range)
		{
			SHTBRange r1 = this.Clone();
			SHTBRange r2 = range.Clone();
			r1.Normalize();
			r2.Normalize();
			SHTBPlace newStart = r1.Start > r2.Start ? r1.Start : r2.Start;
			SHTBPlace newEnd = r1.End < r2.End ? r1.End : r2.End;
			if (newEnd < newStart) 
				return new SHTBRange(AssociatedTextBox, start, start);
			return AssociatedTextBox.GetRange(newStart, newEnd);
		}

		/// <summary>
		/// Returns union with other range.
		/// </summary>
		/// <param name="range"></param>
		/// <returns></returns>
		public SHTBRange GetUnionWith(SHTBRange range)
		{
			SHTBRange r1 = this.Clone();
			SHTBRange r2 = range.Clone();
			r1.Normalize();
			r2.Normalize();
			SHTBPlace newStart = r1.Start < r2.Start ? r1.Start : r2.Start;
			SHTBPlace newEnd = r1.End > r2.End ? r1.End : r2.End;

			return AssociatedTextBox.GetRange(newStart, newEnd);
		}

		/// <summary>
		/// Select all chars of control
		/// </summary>
		public void SelectAll()
		{
			Start = new SHTBPlace(0, 0);
			if (AssociatedTextBox.LinesCount == 0)
				Start = new SHTBPlace(0, 0);
			else
			{
				end = new SHTBPlace(0, 0);
				start = new SHTBPlace(AssociatedTextBox[AssociatedTextBox.LinesCount - 1].Count, AssociatedTextBox.LinesCount - 1);
			}
			if (this == AssociatedTextBox.Selection)
				AssociatedTextBox.Invalidate();
		}

		/// <summary>
		/// Start line and char position
		/// </summary>
		public SHTBPlace Start
		{
			get { return start; }
			set
			{
				end = start = value;
				preferredPos = -1;

				OnSelectionChanged();
			}
		}

		/// <summary>
		/// Finish line and char position
		/// </summary>
		public SHTBPlace End
		{
			get
			{
				return end;
			}
			set
			{
				end = value;
				OnSelectionChanged();
			}
		}

		/// <summary>
		/// Text of range
		/// </summary>
		/// <remarks>This property has not 'set' accessor because undo/redo stack works only with 
		/// FastColoredTextBox.Selection range. So, if you want to set text, you need to use FastColoredTextBox.Selection
		/// and FastColoredTextBox.InsertText() mehtod.
		/// </remarks>
		public string Text
		{
			get
			{
				int fromLine = Math.Min(end.LineNumber, start.LineNumber);
				int toLine = Math.Max(end.LineNumber, start.LineNumber);
				int fromChar = FromCharPos;
				int toChar = ToCharPos;
				if (fromLine < 0) return null;
				//
				StringBuilder sb = new StringBuilder();
				for (int y = fromLine; y <= toLine; y++)
				{
					int fromX = y == fromLine ? fromChar : 0;
					int toX = y == toLine ? toChar - 1 : AssociatedTextBox[y].Count - 1;
					for (int x = fromX; x <= toX; x++)
						sb.Append(AssociatedTextBox[y][x].Character);
					if (y != toLine && fromLine != toLine)
						sb.AppendLine();
				}
				return sb.ToString();
			}
		}

		internal void GetText(out string text, out List<SHTBPlace> charIndexToPlace)
		{
			//try get cached text
			if (AssociatedTextBox.TextVersion == cachedTextVersion)
			{
				text = cachedText;
				charIndexToPlace = cachedCharIndexToPlace;
				return;
			}
			//
			int fromLine = Math.Min(end.LineNumber, start.LineNumber);
			int toLine = Math.Max(end.LineNumber, start.LineNumber);
			int fromChar = FromCharPos;
			int toChar = ToCharPos;

			StringBuilder sb = new StringBuilder((toLine - fromLine)*100);
			charIndexToPlace = new List<SHTBPlace>(sb.Capacity);
			if (fromLine >= 0)
			{
				for (int y = fromLine; y <= toLine; y++)
				{
					int fromX = y == fromLine ? fromChar : 0;
					int toX = y == toLine ? toChar - 1 : AssociatedTextBox[y].Count - 1;
					for (int x = fromX; x <= toX; x++)
					{
						sb.Append(AssociatedTextBox[y][x].Character);
						charIndexToPlace.Add(new SHTBPlace(x, y));
					}
					if (y != toLine && fromLine != toLine)
					foreach (char c in Environment.NewLine)
					{
						sb.Append(c);
						charIndexToPlace.Add(new SHTBPlace(AssociatedTextBox[y].Count/*???*/, y));
					}
				}
			}
			text = sb.ToString();
			charIndexToPlace.Add(End > Start ? End : Start);
			//caching
			cachedText = text;
			cachedCharIndexToPlace = charIndexToPlace;
			cachedTextVersion = AssociatedTextBox.TextVersion;
		}

		/// <summary>
		/// Returns first char after Start place
		/// </summary>
		public char CharAfterStart
		{
			get
			{
				if (Start.CharacterPosition >= AssociatedTextBox[Start.LineNumber].Count)
					return '\n';
				else
					return AssociatedTextBox[Start.LineNumber][Start.CharacterPosition].Character;
			}
		}

		/// <summary>
		/// Returns first char before Start place
		/// </summary>
		public char CharBeforeStart
		{
			get
			{
				if (Start.CharacterPosition <= 0)
					return '\n';
				else
					return AssociatedTextBox[Start.LineNumber][Start.CharacterPosition - 1].Character;
			}
		}

		/// <summary>
		/// Clone range
		/// </summary>
		/// <returns></returns>
		public SHTBRange Clone()
		{
			return (SHTBRange)MemberwiseClone();
		}

		/// <summary>
		/// Return minimum of end.CharacterPosition and start.CharacterPosition
		/// </summary>
		internal int FromCharPos
		{
			get
			{
				if (end.LineNumber < start.LineNumber) return end.CharacterPosition;
				if (end.LineNumber > start.LineNumber) return start.CharacterPosition;
				return Math.Min(end.CharacterPosition, start.CharacterPosition);
			}
		}

		/// <summary>
		/// Return maximum of end.CharacterPosition and start.CharacterPosition
		/// </summary>
		internal int ToCharPos
		{
			get
			{
				if (end.LineNumber < start.LineNumber) return start.CharacterPosition;
				if (end.LineNumber > start.LineNumber) return end.CharacterPosition;
				return Math.Max(end.CharacterPosition, start.CharacterPosition);
			}
		}

		public void GoLeft(bool shift)
		{
			if (start.CharacterPosition > 0)
			{
				start.CharacterPosition--;
			}
			else if (start.LineNumber > 0)
			{
				start.CharacterPosition = AssociatedTextBox[start.LineNumber - 1].Count;
				start.LineNumber--;
			}
			else return;
			if (!shift)
			{
				end = start;
			}
			OnSelectionChanged();

			preferredPos = -1;
		}

		public void GoRight(bool shift)
		{
			if (start.CharacterPosition < AssociatedTextBox[start.LineNumber].Count)
			{
				start.CharacterPosition++;
			}
			else if (start.LineNumber < AssociatedTextBox.LinesCount - 1)
			{
				start.CharacterPosition = 0;
				start.LineNumber++;
			}
			else return;
			if (!shift)
			{
				end = start;
			}
			OnSelectionChanged();

			preferredPos = -1;
		}

		internal void GoUp(bool shift)
		{
			int WordWrapStartingIndex = AssociatedTextBox[start.LineNumber].GetWordWrapStringIndex(start.CharacterPosition);
			if (preferredPos < 0)
			{
				preferredPos = start.CharacterPosition - AssociatedTextBox[start.LineNumber].GetWordWrapStringStartPosition(WordWrapStartingIndex);
			}

			if (WordWrapStartingIndex == 0)
			{
				start.LineNumber--;
				WordWrapStartingIndex = AssociatedTextBox[start.LineNumber].WordWrapStringsCount;
			}

			if (WordWrapStartingIndex > 0)
			{
				int finish = AssociatedTextBox[start.LineNumber].GetWordWrapStringFinishPosition(WordWrapStartingIndex - 1);
				start.CharacterPosition = AssociatedTextBox[start.LineNumber].GetWordWrapStringStartPosition(WordWrapStartingIndex - 1) + preferredPos;
				if (start.CharacterPosition > finish + 1)
					start.CharacterPosition = finish + 1;
			}

			if (!shift)
			{
				end = start;
			}
			OnSelectionChanged();
		}

		internal void GoPageUp(bool shift)
		{
			if (preferredPos < 0)
			{
				preferredPos = start.CharacterPosition - AssociatedTextBox[start.LineNumber].GetWordWrapStringStartPosition(AssociatedTextBox[start.LineNumber].GetWordWrapStringIndex(start.CharacterPosition));
			}

			int pageHeight = AssociatedTextBox.ClientRectangle.Height / AssociatedTextBox.CharHeight - 1;

			for (int i = 0; i < pageHeight; i++)
			{
				int iWW = AssociatedTextBox[start.LineNumber].GetWordWrapStringIndex(start.CharacterPosition);
				if (iWW == 0)
				{
					if (start.LineNumber == 0) break;
					start.LineNumber--;
					iWW = AssociatedTextBox[start.LineNumber].WordWrapStringsCount;
				}

				if (iWW > 0)
				{
					int finish = AssociatedTextBox[start.LineNumber].GetWordWrapStringFinishPosition(iWW - 1);
					start.CharacterPosition = AssociatedTextBox[start.LineNumber].GetWordWrapStringStartPosition(iWW - 1) + preferredPos;
					if (start.CharacterPosition > finish + 1)
					{
						start.CharacterPosition = finish + 1;
					}
				}
			}

			if (!shift)
			{
				end = start;
			}

			OnSelectionChanged();
		}

		internal void GoDown(bool shift)
		{
			int WordWrapStartingIndex = AssociatedTextBox[start.LineNumber].GetWordWrapStringIndex(start.CharacterPosition);
			if (preferredPos < 0)
			{
				preferredPos = start.CharacterPosition - AssociatedTextBox[start.LineNumber].GetWordWrapStringStartPosition(WordWrapStartingIndex);
			}

			if (WordWrapStartingIndex >= AssociatedTextBox[start.LineNumber].WordWrapStringsCount - 1)
			{
				if (start.LineNumber >= AssociatedTextBox.LinesCount - 1) return;
				start.LineNumber++;
				WordWrapStartingIndex = -1;
			}

			if (WordWrapStartingIndex < AssociatedTextBox[start.LineNumber].WordWrapStringsCount - 1)
			{
				int finish = AssociatedTextBox[start.LineNumber].GetWordWrapStringFinishPosition(WordWrapStartingIndex + 1);
				start.CharacterPosition = AssociatedTextBox[start.LineNumber].GetWordWrapStringStartPosition(WordWrapStartingIndex + 1) + preferredPos;
				if (start.CharacterPosition > finish + 1)
				{
					start.CharacterPosition = finish + 1;
				}
			}
			if (!shift)
			{
				end = start;
			}
			OnSelectionChanged();
		}

		internal void GoPageDown(bool shift)
		{
			if (preferredPos < 0)
			{
				preferredPos = start.CharacterPosition - AssociatedTextBox[start.LineNumber].GetWordWrapStringStartPosition(AssociatedTextBox[start.LineNumber].GetWordWrapStringIndex(start.CharacterPosition));
			}

			int pageHeight = AssociatedTextBox.ClientRectangle.Height / AssociatedTextBox.CharHeight - 1;

			for (int i = 0; i < pageHeight; i++)
			{
				int iWW = AssociatedTextBox[start.LineNumber].GetWordWrapStringIndex(start.CharacterPosition);
				if (iWW >= AssociatedTextBox[start.LineNumber].WordWrapStringsCount - 1)
				{
					if (start.LineNumber >= AssociatedTextBox.LinesCount - 1) break;
					start.LineNumber++;
					iWW = -1;
				}

				if (iWW < AssociatedTextBox[start.LineNumber].WordWrapStringsCount - 1)
				{
					int finish = AssociatedTextBox[start.LineNumber].GetWordWrapStringFinishPosition(iWW + 1);
					start.CharacterPosition = AssociatedTextBox[start.LineNumber].GetWordWrapStringStartPosition(iWW + 1) + preferredPos;
					if (start.CharacterPosition > finish + 1)
					{
						start.CharacterPosition = finish + 1;
					}
				}
			}

			if (!shift)
			{
				end = start;
			}
			OnSelectionChanged();
		}

		internal void GoHome(bool shift)
		{
			start.CharacterPosition = 0;
			if (!shift)
			{
				end = start;
			}
			OnSelectionChanged();
			preferredPos = -1;
		}

		internal void GoEnd(bool shift)
		{
			start.CharacterPosition = AssociatedTextBox[start.LineNumber].Count;
			if (!shift)
			{
				end = start;
			}
			OnSelectionChanged();
			preferredPos = -1;
		}

		/// <summary>
		/// Set style for range
		/// </summary>
		public void SetStyle(SHTBStyle style)
		{
			//search code for style
			int code = AssociatedTextBox.GetOrSetStyleLayerIndex(style);
			//set code to chars
			SetStyle(ToStyleIndex(code));
			//
			AssociatedTextBox.Invalidate();
		}

		/// <summary>
		/// Set style for given regex
		/// </summary>
		public void SetStyle(SHTBStyle style, Regex regex)
		{
			//search code for style
			StyleIndex layer = ToStyleIndex(AssociatedTextBox.GetOrSetStyleLayerIndex(style));
			SetStyle(layer, regex);
		}

		/// <summary>
		/// Set style for given regex pattern
		/// </summary>
		public void SetStyle(StyleIndex styleLayer, Regex regex)
		{
			foreach (var range in GetRanges(regex))
				range.SetStyle(styleLayer);
			AssociatedTextBox.Invalidate();
		}

		/// <summary>
		/// Appends style to chars of range
		/// </summary>
		public void SetStyle(StyleIndex styleIndex)
		{
			//set code to chars
			int fromLine = Math.Min(End.LineNumber, Start.LineNumber);
			int toLine = Math.Max(End.LineNumber, Start.LineNumber);
			int fromChar = FromCharPos;
			int toChar = ToCharPos;
			if (fromLine < 0) return;
			//
			for (int y = fromLine; y <= toLine; y++)
			{
				int fromX = y == fromLine ? fromChar : 0;
				int toX = y == toLine ? toChar - 1 : AssociatedTextBox[y].Count - 1;
				for (int x = fromX; x <= toX; x++)
				{
					SHTBChar c = AssociatedTextBox[y][x];
					c.Style |= styleIndex;
					AssociatedTextBox[y][x] = c;
				}
			}
		}

		/// <summary>
		/// Finds ranges for given regex
		/// </summary>
		/// <returns>Enumeration of ranges</returns>
		public IEnumerable<SHTBRange> GetRanges(Regex regex)
		{
			//get text
			string text;
			List<SHTBPlace> charIndexToPlace;
			GetText(out text, out charIndexToPlace);
			//
			foreach (Match m in regex.Matches(text))
			{
				SHTBRange r = new SHTBRange(this.AssociatedTextBox);
				//try get 'range' group, otherwise use group 0
				Group group = m.Groups["range"];
				if (!group.Success)
					group = m.Groups[0];
				//
				r.Start = charIndexToPlace[group.Index];
				r.End = charIndexToPlace[group.Index + group.Length];
				yield return r;
			}
		}

		/// <summary>
		/// Clear styles of range
		/// </summary>
		public void ClearStyle(params SHTBStyle[] styles)
		{
			try
			{
			   ClearStyle(AssociatedTextBox.GetStyleIndexMask(styles));
			}
			catch { ;}
		}

		/// <summary>
		/// Clear styles of range
		/// </summary>
		public void ClearStyle(StyleIndex styleIndex)
		{
			//set code to chars
			int fromLine = Math.Min(End.LineNumber, Start.LineNumber);
			int toLine = Math.Max(End.LineNumber, Start.LineNumber);
			int fromChar = FromCharPos;
			int toChar = ToCharPos;
			if (fromLine < 0) return;
			//
			for (int y = fromLine; y <= toLine; y++)
			{
				int fromX = y == fromLine ? fromChar : 0;
				int toX = y == toLine ? toChar - 1 : AssociatedTextBox[y].Count - 1;
				for (int x = fromX; x <= toX; x++)
				{
					SHTBChar c = AssociatedTextBox[y][x];
					c.Style &= ~styleIndex;
					AssociatedTextBox[y][x] = c;
				}
			}
			//
			AssociatedTextBox.Invalidate();
		}

		void OnSelectionChanged()
		{
			//clear cache
			cachedTextVersion = -1;
			cachedText = null;
			cachedCharIndexToPlace = null;
			//
			if (AssociatedTextBox.Selection == this && updating == 0)
			{
				AssociatedTextBox.OnSelectionChanged();
			}
		}

		/// <summary>
		/// Starts selection position updating
		/// </summary>
		public void BeginUpdate()
		{
			updating++;
		}

		/// <summary>
		/// Ends selection position updating
		/// </summary>
		public void EndUpdate()
		{
			updating--;
			if (updating == 0)
				OnSelectionChanged();
		}

		public override string ToString()
		{
			return String.Format("Start: {0} End: {1}", Start, End);
		}

		/// <summary>
		/// Adjusts the Start and End of the Range so that the Start comes first.
		/// </summary>
		public void Normalize()
		{
			if (Start > End)
			{
				var temp = start;
				start = end;
				end = temp;
			}
		}

		/// <summary>
		/// Expands range from first char of Start line to last char of End line
		/// </summary>
		public void Expand()
		{
			Normalize();
			start = new SHTBPlace(0, start.LineNumber);
			end = new SHTBPlace(AssociatedTextBox.GetLineLength(end.LineNumber), end.LineNumber);
		}

		IEnumerator<SHTBPlace> IEnumerable<SHTBPlace>.GetEnumerator()
		{
			int fromLine = Math.Min(end.LineNumber, start.LineNumber);
			int toLine = Math.Max(end.LineNumber, start.LineNumber);
			int fromChar = FromCharPos;
			int toChar = ToCharPos;
			if (fromLine < 0) yield break;
			//
			for (int y = fromLine; y <= toLine; y++)
			{
				int fromX = y == fromLine ? fromChar : 0;
				int toX = y == toLine ? toChar - 1 : AssociatedTextBox[y].Count - 1;
				for (int x = fromX; x <= toX; x++)
					yield return new SHTBPlace(x, y);
			}
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return (this as IEnumerable<SHTBPlace>).GetEnumerator();
		}

		bool IsIdentifierChar(char c)
		{
			return char.IsLetterOrDigit(c) || c == '_';
		}

		internal void GoWordLeft(bool shift)
		{
			SHTBRange range = this.Clone();//for OnSelectionChanged disable

			SHTBPlace prev;
			bool findIdentifier = IsIdentifierChar(range.CharBeforeStart);

			do
			{
				prev = range.Start;
				if (IsIdentifierChar(range.CharBeforeStart) ^ findIdentifier)
					break;

				//move left
				range.GoLeft(shift);
			}
			while (prev != range.Start);

			this.Start = range.Start;
			this.End = range.End;

		}

		internal void GoWordRight(bool shift)
		{
			SHTBRange range = this.Clone();//for OnSelectionChanged disable

			SHTBPlace prev;
			bool findIdentifier = IsIdentifierChar(range.CharAfterStart);

			do
			{
				prev = range.Start;
				if (IsIdentifierChar(range.CharAfterStart) ^ findIdentifier)
					break;

				//move right
				range.GoRight(shift);
			} 
			while (prev != range.Start);

			this.Start = range.Start;
			this.End = range.End;

		}

		internal void GoFirst(bool shift)
		{
			start = new SHTBPlace(0, 0);

			if (!shift)
			{
				end = start;
			}
			OnSelectionChanged();
		}

		internal void GoLast(bool shift)
		{
			start = new SHTBPlace(AssociatedTextBox[AssociatedTextBox.LinesCount - 1].Count, AssociatedTextBox.LinesCount-1);

			if (!shift)
				end = start;

			OnSelectionChanged();
		}

		public static StyleIndex ToStyleIndex(int i)
		{
			return (StyleIndex)(1 << i);
		}
	}
}
