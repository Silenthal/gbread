using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GBRead
{
	public class SHTBLineCollection
	{
		private List<SHTBLine> lines = new List<SHTBLine>();
		public int DisplayedLineWidth { get; set; }
		public int DisplayedLineHeight { get; set; }

		/// <summary>
		/// Gets Line
		/// </summary>
		public SHTBLine this[int iLine]
		{
			get { return lines[iLine]; }
		}

		public SHTBLineCollection()
		{
			lines = new List<SHTBLine>();
		}
	}
}
