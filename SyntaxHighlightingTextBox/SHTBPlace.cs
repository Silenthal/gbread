using System;

namespace GBRead
{
	/// <summary>
	/// Line index and char index
	/// </summary>
	public struct SHTBPlace
	{
		private int charPos;
		private int lineNum;

		public int CharacterPosition
		{
			get
			{
				return charPos;
			}
			set
			{
				charPos = value;
				if (charPos < 0) charPos = 0;
			}
		}
		public int LineNumber
		{
			get
			{
				return lineNum;
			}
			set
			{
				lineNum = value;
				if (lineNum < 0) lineNum = 0;
			}
		}

		public SHTBPlace(int iChar, int iLine)
		{
			charPos = iChar;
			lineNum = iLine;
		}

		/// <summary>
		/// Changes the position of the SHTBPlace to a new location based on given offsets.
		/// </summary>
		/// <param name="CharacterPositionOffset">The offset to the original character position.</param>
		/// <param name="LineNumberOffset">The offset to the original line number.</param>
		public void ChangePosition(int CharacterPositionOffset, int LineNumberOffset)
		{
			charPos += CharacterPositionOffset;
			lineNum += LineNumberOffset;
		}

		public static bool operator !=(SHTBPlace p1, SHTBPlace p2)
		{
			return !p1.Equals(p2);
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public static bool operator ==(SHTBPlace p1, SHTBPlace p2)
		{
			return p1.Equals(p2);
		}

		public static bool operator <(SHTBPlace p1, SHTBPlace p2)
		{
			if (p1.lineNum < p2.lineNum) return true;
			if (p1.lineNum > p2.lineNum) return false;
			if (p1.charPos < p2.charPos) return true;
			return false;
		}

		public static bool operator <=(SHTBPlace p1, SHTBPlace p2)
		{
			if (p1.Equals(p2)) return true;
			if (p1.lineNum < p2.lineNum) return true;
			if (p1.lineNum > p2.lineNum) return false;
			if (p1.charPos < p2.charPos) return true;
			return false;
		}

		public static bool operator >(SHTBPlace p1, SHTBPlace p2)
		{
			if (p1.lineNum > p2.lineNum) return true;
			if (p1.lineNum < p2.lineNum) return false;
			if (p1.charPos > p2.charPos) return true;
			return false;
		}

		public static bool operator >=(SHTBPlace p1, SHTBPlace p2)
		{
			if (p1.Equals(p2)) return true;
			if (p1.lineNum > p2.lineNum) return true;
			if (p1.lineNum < p2.lineNum) return false;
			if (p1.charPos > p2.charPos) return true;
			return false;
		}

		public static SHTBPlace Empty
		{
			get { return new SHTBPlace(); }
		}

		public override string ToString()
		{
			return String.Format("({0},{1})", lineNum, charPos);
		}
	}
}
