using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GBRead
{
	/// <summary>
	/// Represents the style a SyntaxHighlightingTextBox text element is drawn in.
	/// </summary>
	/// <remarks>This is the base class for all text and design renderers in the SyntaxHighlightingTextBox..</remarks>
	public abstract class SHTBStyle : IDisposable
	{
		/// <summary>
		/// Draws a given range of text.
		/// </summary>
		/// <param name="g">The Graphics object to use.</param>
		/// <param name="position">The position of the range in absolute control coordinates.</param>
		/// <param name="range">The Range of text to draw.</param>
		public abstract void Draw(Graphics g, Point position, SHTBRange range);

		/// <summary>
		/// Returns the drawn size of a given Range.
		/// </summary>
		/// <param name="range">A Range of text.</param>
		/// <returns>The Size of the Range.</returns>
		public static Size GetSizeOfRange(SHTBRange range)
		{
			return new Size((range.End.CharacterPosition - range.Start.CharacterPosition) * range.AssociatedTextBox.CharWidth, range.AssociatedTextBox.CharHeight);
		}

		/// <summary>
		/// Calculates a rounded rectangle, given a starting rectangle.
		/// </summary>
		/// <param name="rect">A System.Drawing.Rectangle that represents the original rectangle to base the new rectangle on.</param>
		/// <param name="d">Represents the width and height of the rectangle bounding the arc on each corner of the rounded rectangle.</param>
		/// <returns>A System.Drawing.Drawing2D.GraphicsPath that represents the rounded rectangle.</returns>
		public static GraphicsPath GetRoundedRectangle(Rectangle rect, int d)
		{
			GraphicsPath gp = new GraphicsPath();
			gp.AddArc(rect.X, rect.Y, d, d, 180, 90);
			gp.AddArc(rect.X + rect.Width - d, rect.Y, d, d, 270, 90);
			gp.AddArc(rect.X + rect.Width - d, rect.Y + rect.Height - d, d, d, 0, 90);
			gp.AddArc(rect.X, rect.Y + rect.Height - d, d, d, 90, 90);
			gp.AddLine(rect.X, rect.Y + rect.Height - d, rect.X, rect.Y + d / 2);

			return gp;
		}

		/// <summary>
		/// Releases all resources used by this SHTBTextStyle object.
		/// </summary>
		public virtual void Dispose()
		{
		}
	}

	/// <summary>
	/// Represents the style an SHTBChar is drawn in.
	/// </summary>
	public class SHTBTextStyle : SHTBStyle
	{
		/// <summary>
		/// The System.Drawing.Brush used to paint the foreground of an SHTBChar.
		/// </summary>
		public Brush ForegroundBrush { get; set; }

		/// <summary>
		/// The System.Drawing.Brush used to paint the background of an SHTBChar.
		/// </summary>
		public Brush BackgroundBrush { get; set; }

		/// <summary>
		/// The System.Drawing.FontStyle the SHTBChar is drawn in.
		/// </summary>
		public FontStyle FontStyle { get; set; }

		/// <summary>
		/// Represents the System.Drawing.StringFormat of the SHTBChar.
		/// </summary>
		public StringFormat stringFormat;

		/// <summary>
		/// Initializes a new instance of the SHTBTextStyle class.
		/// </summary>
		/// <param name="foreBrush">The System.Drawing.Brush used to paint the foreground.</param>
		/// <param name="backgroundBrush">The System.Drawing.Brush used to paint the background.</param>
		/// <param name="fontStyle">The System.Drawing.FontStyle of the text.</param>
		public SHTBTextStyle(Brush foreBrush, Brush backgroundBrush, FontStyle fontStyle = System.Drawing.FontStyle.Regular)
		{
			ForegroundBrush = foreBrush;
			BackgroundBrush = backgroundBrush;
			FontStyle = fontStyle;
			stringFormat = new StringFormat(StringFormatFlags.MeasureTrailingSpaces);
		}

		/// <summary>
		/// Draws the given range.
		/// </summary>
		/// <param name="g"></param>
		/// <param name="position"></param>
		/// <param name="range"></param>
		public override void Draw(Graphics g, Point position, SHTBRange range)
		{
			#region Drawing the background

			if (BackgroundBrush != null)
			{
				g.FillRectangle(BackgroundBrush, position.X, position.Y, (range.End.CharacterPosition - range.Start.CharacterPosition) * range.AssociatedTextBox.CharWidth, range.AssociatedTextBox.CharHeight);
			}

			#endregion Drawing the background

			#region Drawing the characters

			Font drawnFont = new Font(range.AssociatedTextBox.Font, FontStyle);
			SHTBLine line = range.AssociatedTextBox[range.Start.LineNumber];
			float characterWidth = range.AssociatedTextBox.CharWidth;
			float y = position.Y;
			float x = position.X - range.AssociatedTextBox.CharWidth / 3;

			if (ForegroundBrush == null) ForegroundBrush = new SolidBrush(range.AssociatedTextBox.ForeColor);

			#region If IME Mode is allowed

			if (range.AssociatedTextBox.ImeAllowed)
			{
				for (int i = range.Start.CharacterPosition; i < range.End.CharacterPosition; i++)
				{
					SizeF charSize = SyntaxHighlightingTextBox.GetCharSize(drawnFont, line[i].Character);

					var savedGraphicState = g.Save();
					float charScaleFactor = charSize.Width > range.AssociatedTextBox.CharWidth + 1 ? range.AssociatedTextBox.CharWidth / charSize.Width : 1;
					g.TranslateTransform(x, y + (1 - charScaleFactor) * range.AssociatedTextBox.CharHeight / 2);
					g.ScaleTransform(charScaleFactor, (float)Math.Sqrt(charScaleFactor));
					g.DrawString(line[i].Character.ToString(), drawnFont, ForegroundBrush, 0, 0, stringFormat);
					g.Restore(savedGraphicState);
					/*
					if(size.Width>range.tb.CharWidth*1.5f)
						gr.DrawString(line[i].c.ToString(), fHalfSize, foreBrush, x, y+range.tb.CharHeight/4, stringFormat);
					else
						gr.DrawString(line[i].c.ToString(), f, foreBrush, x, y, stringFormat);
					 * */
					x += characterWidth;
				}
			}

			#endregion If IME Mode is allowed

			#region If IME Mode isn't allowed

			else
			{
				for (int i = range.Start.CharacterPosition; i < range.End.CharacterPosition; i++)
				{
					//draw char
					g.DrawString(line[i].Character.ToString(), drawnFont, ForegroundBrush, x, y, stringFormat);
					x += characterWidth;
				}
			}

			#endregion If IME Mode isn't allowed

			#endregion Drawing the characters

			drawnFont.Dispose();
		}

		/// <summary>
		/// Releases all resources used by this SHTBTextStyle object.
		/// </summary>
		public override void Dispose()
		{
			base.Dispose();
			if (ForegroundBrush != null) ForegroundBrush.Dispose();
			if (BackgroundBrush != null) BackgroundBrush.Dispose();
		}
	}

	/// <summary>
	/// Represents the style a a selection is drawn in.
	/// </summary>
	public class SHTBSelectionStyle : SHTBStyle
	{
		public Brush BackgroundBrush { get; set; }

		public SHTBSelectionStyle(Brush backgroundBrush)
		{
			this.BackgroundBrush = backgroundBrush;
		}

		public override void Draw(Graphics g, Point position, SHTBRange range)
		{
			//draw background
			if (BackgroundBrush != null)
			{
				Rectangle rect = new Rectangle(position.X, position.Y, (range.End.CharacterPosition - range.Start.CharacterPosition) * range.AssociatedTextBox.CharWidth, range.AssociatedTextBox.CharHeight);
				if (rect.Width == 0)
					return;
				g.FillRectangle(BackgroundBrush, rect);
			}
		}

		/// <summary>
		/// Releases all resources used by this SHTBSelectionStyle object.
		/// </summary>
		public override void Dispose()
		{
			base.Dispose();

			if (BackgroundBrush != null)
				BackgroundBrush.Dispose();
		}
	}
}