using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace GBRead
{
	public class SyntaxHighlightingTextBox : UserControl
	{
		#region Find/Replace Forms

		FindForm findForm;
		ReplaceForm replaceForm;

		#endregion Find/Replace Forms

		#region Drawing Colors

		Color lineNumberColor;
		Color indentBackColor;
		Color currentLineColor;
		Color changedLineColor;

		#endregion Drawing Colors

		#region Text Box Properties

		readonly List<SHTBLine> lines = new List<SHTBLine>();
		bool wordWrap;
		bool scrollBars;
		bool multiline;
		int wordWrapLinesCount;
		bool showLineNumbers;

		#endregion Text Box Properties

		const int minLeftIndent = 8;
		internal readonly CommandManager manager = new CommandManager();
		Keys lastModifiers;

		SHTBRange selection;

		int charHeight;

		public bool needRecalc;
		bool isChanged;

		int leftIndent;

		uint lineNumberStartValue;
		int leftPadding;
		int lastLineUniqueId;

		/// <summary>
		/// Background color for current line.
		/// </summary>
		[DefaultValue(typeof(Color), "Transparent")]
		[Description("Background color for current line. Set to Color.Transparent to hide current line highlighting")]
		public Color CurrentLineColor
		{
			get { return currentLineColor; }
			set
			{
				currentLineColor = value;
				Invalidate();
			}
		}

		/// <summary>
		/// Background color for highlighting of changed lines.
		/// </summary>
		[DefaultValue(typeof(Color), "Transparent")]
		[Description("Background color for highlighting of changed lines. Set to Color.Transparent to hide changed line highlighting.")]
		public Color ChangedLineColor
		{
			get { return changedLineColor; }
			set
			{
				changedLineColor = value;
				Invalidate();
			}
		}

		/// <summary>
		/// Fore color (default style color).
		/// </summary>
		public override Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
				base.ForeColor = value;
				InitDefaultStyle();
				Invalidate();
			}
		}

		/// <summary>
		/// Height of a char in pixels.
		/// </summary>
		[Description("Height of a char in pixels.")]
		public int CharHeight
		{
			get { return charHeight; }
			private set
			{
				charHeight = value;
				OnCharSizeChanged();
			}
		}

		/// <summary>
		/// Occurs when VisibleRange is changed
		/// </summary>
		public virtual void OnVisibleRangeChanged()
		{
			if (VisibleRangeChanged != null)
				VisibleRangeChanged(this, new EventArgs());
		}

		/// <summary>
		/// Invalidates the entire surface of the control and causes the control to be redrawn.
		/// This method is thread safe and does not require Invoke.
		/// </summary>
		public new void Invalidate()
		{
			if (InvokeRequired)
				BeginInvoke(new MethodInvoker(Invalidate));
			else
				base.Invalidate();
		}

		protected virtual void OnCharSizeChanged()
		{
			VerticalScroll.SmallChange = charHeight;
			VerticalScroll.LargeChange = 10 * charHeight;
			HorizontalScroll.SmallChange = CharWidth;
		}

		/// <summary>
		/// Width of char in pixels
		/// </summary>
		[Description("Width of char in pixels")]
		public int CharWidth { get; private set; }

		/// <summary>
		/// Spaces count for tab
		/// </summary>
		[DefaultValue(4)]
		[Description("Spaces count for tab")]
		public int TabLength { get; set; }

		/// <summary>
		/// Text was changed
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsChanged
		{
			get
			{
				return isChanged;
			}
			set
			{
				if (!value)
				{
					//clear line's IsChanged property
					foreach (var line in lines)
					{
						line.IsChanged = false;
					}
				}
				isChanged = value;
			}
		}

		/// <summary>
		/// Text version
		/// </summary>
		/// <remarks>This counter is incremented each time changes the text</remarks>
		[Browsable(false)]
		public int TextVersion { get; private set; }

		/// <summary>
		/// Shows line numbers.
		/// </summary>
		[DefaultValue(true)]
		[Description("Shows line numbers.")]
		public bool ShowLineNumbers { get { return showLineNumbers; } set { showLineNumbers = value; Invalidate(); } }

		/// <summary>
		/// Color of line numbers.
		/// </summary>
		[DefaultValue(typeof(Color), "Teal")]
		[Description("Color of line numbers.")]
		public Color LineNumberColor { get { return lineNumberColor; } set { lineNumberColor = value; Invalidate(); } }

		/// <summary>
		/// Start value of first line number.
		/// </summary>
		[DefaultValue(typeof(uint), "1")]
		[Description("Start value of first line number.")]
		public uint LineNumberStartValue { get { return lineNumberStartValue; } set { lineNumberStartValue = value; needRecalc = true; Invalidate(); } }

		/// <summary>
		/// Background color of indent area
		/// </summary>
		[DefaultValue(typeof(Color), "White")]
		[Description("Background color of indent area")]
		public Color IndentBackColor { get { return indentBackColor; } set { indentBackColor = value; Invalidate(); } }

		/// <summary>
		/// Left indent in pixels
		/// </summary>
		[Browsable(false)]
		[Description("Left indent in pixels")]
		public int LeftIndent { get { return leftIndent; } private set { leftIndent = value; } }

		/// <summary>
		/// Left padding in pixels
		/// </summary>
		[DefaultValue(0)]
		[Description("Left padding in pixels")]
		public int LeftPadding { get { return leftPadding; } set { leftPadding = value; Invalidate(); } }

		/// <summary>
		/// Styles
		/// Maximum style count is 16
		/// </summary>
		public readonly SHTBStyle[] Styles = new SHTBStyle[sizeof(ushort) * 8];

		/// <summary>
		/// Default text style
		/// This style is using when no one other TextStyle is not defined in Char.style
		/// </summary>
		[Browsable(false)]
		public SHTBTextStyle DefaultStyle { get; set; }

		/// <summary>
		/// Style for rendering Selection area
		/// </summary>
		[Browsable(false)]
		public SHTBSelectionStyle SelectionStyle { get; set; }

		/// <summary>
		/// Comment line prefix.
		/// </summary>
		[DefaultValue(";")]
		[Description("Comment line prefix.")]
		public string CommentPrefix { get; set; }

		/// <summary>
		/// This property specifies which part of the text will be highlighted as you type (by built-in highlighter).
		/// </summary>
		/// <remarks>When a user enters text, a component of rebuilding the highlight (because the text is changed).
		/// This property specifies exactly which section of the text will be re-highlighted.
		/// This can be useful to highlight multi-line comments, for example.</remarks>
		[DefaultValue(typeof(HighlightingRangeType), "ChangedRange")]
		[Description("This property specifies which part of the text will be highlighted as you type.")]
		public HighlightingRangeType HighlightingRangeType { get; set; }

		/// <summary>
		/// TextChanged event.
		/// It occurs after insert, delete, clear, undo and redo operations.
		/// </summary>
		[Browsable(true)]
		[Description("It occurs after insert, delete, clear, undo and redo operations.")]
		public new event EventHandler<TextChangedEventArgs> TextChanged;
		/// <summary>
		/// Fake event for correct data binding
		/// </summary>
		[Browsable(false)]
		internal event EventHandler BindingTextChanged;
		/// <summary>
		/// TextChanging event.
		/// It occurs before insert, delete, clear, undo and redo operations.
		/// </summary>
		[Browsable(true)]
		[Description("It occurs before insert, delete, clear, undo and redo operations.")]
		public event EventHandler<TextChangingEventArgs> TextChanging;
		/// <summary>
		/// SelectionChanged event.
		/// It occurs after changing of selection.
		/// </summary>
		[Browsable(true)]
		[Description("It occurs after changing of selection.")]
		public event EventHandler SelectionChanged;
		/// <summary>
		/// VisibleRangeChanged event.
		/// It occurs after changing of visible range.
		/// </summary>
		[Browsable(true)]
		[Description("It occurs after changing of visible range.")]
		public event EventHandler VisibleRangeChanged;
		/// <summary>
		/// Event that occurs when a visible char is being pressed (alphabetic, digit, punctuation, DEL, BACKSPACE).
		/// </summary>
		/// <remarks>Set Handle to True for cancel key</remarks>
		[Browsable(true)]
		[Description("It occurs when visible char is enetering (alphabetic, digit, punctuation, DEL, BACKSPACE).")]
		public event KeyPressEventHandler KeyPressing;
		/// <summary>
		/// It occurs when visible char is enetered (alphabetic, digit, punctuation, DEL, BACKSPACE)
		/// </summary>
		[Browsable(true)]
		[Description("It occurs when visible char is enetered (alphabetic, digit, punctuation, DEL, BACKSPACE).")]
		public event KeyPressEventHandler KeyPressed;
		/// <summary>
		/// It occurs when line background is painting
		/// </summary>
		[Browsable(true)]
		[Description("It occurs when line background is painting.")]
		public event EventHandler<PaintLineEventArgs> PaintLine;
		/// <summary>
		/// Occurs when line was inserted/added
		/// </summary>
		[Browsable(true)]
		[Description("Occurs when line was inserted/added.")]
		public event EventHandler<LineInsertedEventArgs> LineInserted;
		/// <summary>
		/// Occurs when line was removed
		/// </summary>
		[Browsable(true)]
		[Description("Occurs when line was removed.")]
		public event EventHandler<LineRemovedEventArgs> LineRemoved;

		/// <summary>
		/// Allows text rendering several styles same time.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(false)]
		[Description("Allows text rendering several styles same time.")]
		public bool AllowSeveralTextStyleDrawing { get; set; }

		/// <summary>
		/// Syntax Highlighter.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Z80SyntaxHighlighter SyntaxHighlighter { get; set; }

		/// <summary>
		/// Constructor
		/// </summary>
		public SyntaxHighlightingTextBox()
		{
			try
			{
				//drawing optimization
				SetStyle(ControlStyles.AllPaintingInWmPaint, true);
				SetStyle(ControlStyles.UserPaint, true);
				SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
				SetStyle(ControlStyles.ResizeRedraw, true);
				//append monospace font
				Font = new Font("Consolas", 9.75f, FontStyle.Regular, GraphicsUnit.Point);
				//create one line
				if (lines.Count == 0)
					InsertLine(0, new SHTBLine(GenerateUniqueLineId()));
				selection = new SHTBRange(this) { Start = new SHTBPlace(0, 0) };
				//default settings
				Cursor = Cursors.IBeam;
				BackColor = Color.White;
				LineNumberColor = Color.Teal;
				IndentBackColor = Color.White;
				CurrentLineColor = Color.Transparent;
				ChangedLineColor = Color.Transparent;
				ShowLineNumbers = true;
				TabLength = 4;
				SelectionStyle = new SHTBSelectionStyle(new SolidBrush(Color.FromArgb(50, Color.Blue)));
				AllowSeveralTextStyleDrawing = false;
				SyntaxHighlighter = new Z80SyntaxHighlighter();
				needRecalc = true;
				CommentPrefix = ";";
				lineNumberStartValue = 1;
				multiline = true;
				scrollBars = true;
				//
				base.AutoScroll = true;
				//
				InitDefaultStyle();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}

		static SyntaxHighlightingTextBox()
		{
			TypeDescriptor.AddProvider(new SHTBDescriptionProvider(), typeof(SyntaxHighlightingTextBox));
		}


		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			m_hImc = ImmGetContext(this.Handle);
		}

		private void ResetTimer(System.Windows.Forms.Timer timer)
		{
			timer.Stop();
			timer.Start();
		}

		/// <summary>
		/// Adds new style
		/// </summary>
		/// <returns>Layer index of this style</returns>
		public int AddStyle(SHTBStyle style)
		{
			if (style == null) return -1;

			int i = GetStyleIndex(style);
			if (i >= 0)
				return i;

			for (i = Styles.Length - 1; i >= 0; i--)
				if (Styles[i] != null)
					break;

			i++;
			if (i >= Styles.Length)
				throw new Exception("Maximum count of Styles is exceeded");

			Styles[i] = style;
			return i;
		}

		/// <summary>
		/// Returns current visible range of text
		/// </summary>
		[Browsable(false)]
		public SHTBRange VisibleRange
		{
			get
			{
				return GetRange(
					PointToPlace(new Point(LeftIndent, 0)),
					PointToPlace(new Point(ClientSize.Width, ClientSize.Height))
				);
			}
		}

		/// <summary>
		/// Current selection range
		/// </summary>
		[Browsable(false)]
		public SHTBRange Selection
		{
			get { return selection; }
			set
			{
				selection.BeginUpdate();
				selection.Start = value.Start;
				selection.End = value.End;
				selection.EndUpdate();
				Invalidate();
			}
		}

		/// <summary>
		/// Background color.
		/// </summary>
		[DefaultValue(typeof(Color), "White")]
		[Description("Background color.")]
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = value;
			}
		}

		[Browsable(true)]
		[DefaultValue(true)]
		[Description("Scollbars visibility.")]
		public bool ShowScrollBars
		{
			get { return scrollBars; }
			set
			{
				if (value == scrollBars) return;
				scrollBars = value;
				needRecalc = true;
				Invalidate();
			}
		}

		/// <summary>
		/// Multiline.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(true)]
		[Description("Multiline mode.")]
		public bool Multiline
		{
			get { return multiline; }
			set
			{
				if (multiline == value) return;
				multiline = value;
				needRecalc = true;
				if (multiline)
				{
					base.AutoScroll = true;
					ShowScrollBars = true;
				}
				else
				{
					base.AutoScroll = false;
					ShowScrollBars = false;
					if (lines.Count > 1)
						lines.RemoveRange(1, lines.Count - 1);
					manager.ClearHistory();
				}
				Invalidate();
			}
		}

		/// <summary>
		/// Indicates whether a multiline text box control automatically wraps words to the beginning of the next line when necessary.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(false)]
		[Description("Indicates whether a multiline text box control automatically wraps words to the beginning of the next line when necessary.")]
		public bool WordWrap
		{
			get { return wordWrap; }
			set
			{
				if (wordWrap == value) return;
				wordWrap = value;
				RecalcWordWrap(0, LinesCount - 1);
				Invalidate();
			}
		}

		/// <summary>
		/// Controls whether the text in the edit control can be changed or not.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(false)]
		[Description("Controls whether the text in the edit control can be changed or not.")]
		public bool ReadOnly { get; set; }

		/// <summary>
		/// Count of lines with wordwrap effect
		/// </summary>
		[Browsable(false)]
		public int WordWrapLinesCount
		{
			get
			{
				if (needRecalc)
					Recalc();
				return wordWrapLinesCount;
			}
		}

		/// <summary>
		/// Do not change this property
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override bool AutoScroll
		{
			get { return base.AutoScroll; }
			set { ;}
		}

		/// <summary>
		/// Count of lines
		/// </summary>
		[Browsable(false)]
		public int LinesCount
		{
			get { return lines.Count; }
		}

		/// <summary>
		/// Gets Line
		/// </summary>
		public SHTBLine this[int iLine]
		{
			get { return lines[iLine]; }
		}

		/// <summary>
		/// Shows find dialog
		/// </summary>
		public void ShowFindDialog()
		{
			ShowFindDialog(null);
		}

		/// <summary>
		/// Shows find dialog
		/// </summary>
		public void ShowFindDialog(string findText)
		{
			if (findForm == null)
				findForm = new FindForm(this);

			if (findText != null)
				findForm.tbFind.Text = findText;
			else
				if (Selection.Start != Selection.End && Selection.Start.LineNumber == Selection.End.LineNumber)
					findForm.tbFind.Text = Selection.Text;

			findForm.tbFind.SelectAll();
			findForm.Show();
		}

		/// <summary>
		/// Shows replace dialog
		/// </summary>
		public void ShowReplaceDialog()
		{
			ShowReplaceDialog(null);
		}

		/// <summary>
		/// Shows replace dialog
		/// </summary>
		public void ShowReplaceDialog(string findText)
		{
			if (replaceForm == null)
				replaceForm = new ReplaceForm(this);

			if (findText != null)
				replaceForm.tbFind.Text = findText;
			else
				if (Selection.Start != Selection.End && Selection.Start.LineNumber == Selection.End.LineNumber)
					replaceForm.tbFind.Text = Selection.Text;

			replaceForm.tbFind.SelectAll();
			replaceForm.Show();
		}

		/// <summary>
		/// Gets length of given line
		/// </summary>
		/// <param name="iLine">Line index</param>
		/// <returns>Length of line</returns>
		public int GetLineLength(int iLine)
		{
			if (iLine < 0 || iLine >= lines.Count)
				throw new ArgumentOutOfRangeException("Line index out of range");

			return lines[iLine].Count;
		}

		/// <summary>
		/// Get range of line
		/// </summary>
		/// <param name="iLine">Line index</param>
		public SHTBRange GetLine(int iLine)
		{
			if (iLine < 0 || iLine >= lines.Count)
				throw new ArgumentOutOfRangeException("Line index out of range");

			SHTBRange sel = new SHTBRange(this);
			sel.Start = new SHTBPlace(0, iLine);
			sel.End = new SHTBPlace(lines[iLine].Count, iLine);
			return sel;
		}

		/// <summary>
		/// Copy selected text into Clipboard
		/// </summary>
		public void Copy()
		{
			if (Selection.End != Selection.Start)
			{
				var data = new DataObject();
				data.SetData(DataFormats.UnicodeText, true, Selection.Text);

				Thread thread = new Thread(() => Clipboard.SetDataObject(data, true));
				thread.SetApartmentState(ApartmentState.STA);
				thread.Start();
				thread.Join();
			}
		}

		/// <summary>
		/// Cut selected text into Clipboard
		/// </summary>
		public void Cut()
		{
			if (Selection.End != Selection.Start)
			{
				Copy();
				ClearSelected();
			}
		}

		/// <summary>
		/// Paste text from clipboard into selection position
		/// </summary>
		public void Paste()
		{
			string text = null;
			Thread thread = new Thread(() =>
				{
					if (Clipboard.ContainsText())
						text = Clipboard.GetText();
				}
			);
			thread.SetApartmentState(ApartmentState.STA);
			thread.Start();
			thread.Join();

			if (text != null)
				InsertText(text);
		}

		/// <summary>
		/// Gets or sets the text associated with this control.
		/// </summary>
		[Browsable(true)]
		[Localizable(true)]
		[Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[SettingsBindable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Description("Gets or sets the text associated with this control.")]
		[Bindable(true)]
		public override string Text
		{
			get
			{
				SHTBRange sel = new SHTBRange(this);
				sel.SelectAll();
				return sel.Text;
			}

			set
			{
				Selection.BeginUpdate();
				try
				{
					Selection.SelectAll();
					InsertText(value);
					GoHome();
				}
				finally { Selection.EndUpdate(); }
			}
		}

		/// <summary>
		/// Text lines
		/// </summary>
		[Browsable(false)]
		public string[] Lines
		{
			get
			{
				List<string> sif = new List<string>();
				foreach (SHTBLine sd in lines)
				{
					sif.Add(sd.Text);
				}
				return sif.ToArray();
			}
		}

		[Browsable(false)]
		public new Padding Padding { get { return new Padding(0, 0, 0, 0); } set { ;} }

		/// <summary>
		/// Select all chars of text
		/// </summary>
		public void SelectAll()
		{
			Selection.SelectAll();
		}

		/// <summary>
		/// Text of current selection
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string SelectedText
		{
			get { return Selection.Text; }
			set { InsertText(value); }
		}

		/// <summary>
		/// Start position of selection
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int SelectionStart
		{
			get
			{
				return Math.Min(PlaceToPosition(Selection.Start), PlaceToPosition(Selection.End));
			}
			set
			{
				Selection.Start = PositionToPlace(value);
			}
		}

		/// <summary>
		/// Length of selected text
		/// </summary>
		[Browsable(false)]
		[DefaultValue(0)]
		public int SelectionLength
		{
			get
			{
				return Math.Abs(PlaceToPosition(Selection.Start) - PlaceToPosition(Selection.End));
			}
			set
			{
				if (value > 0)
					Selection.End = PositionToPlace(SelectionStart + value);
			}
		}

		/// <summary>
		/// Move caret to end of text
		/// </summary>
		public void GoEnd()
		{
			if (lines.Count > 0)
				Selection.Start = new SHTBPlace(lines[lines.Count - 1].Count, lines.Count - 1);
			else
				Selection.Start = new SHTBPlace(0, 0);

			DoCaretVisible();
		}

		/// <summary>
		/// Move caret to first position
		/// </summary>
		public void GoHome()
		{
			Selection.Start = new SHTBPlace(0, 0);
			VerticalScroll.Value = 0;
			HorizontalScroll.Value = 0;
		}

		/// <summary>
		/// Clears all text from the text box control, including all styles, text history, and caches.
		/// </summary>
		public void Clear()
		{
			Selection.BeginUpdate();
			try
			{
				Selection.SelectAll();
				ClearSelected();
				manager.ClearHistory();
				Invalidate();
			}
			finally { Selection.EndUpdate(); }
		}

		/// <summary>
		/// Clear buffer of styles
		/// </summary>
		public void ClearStylesBuffer()
		{
			for (int i = 0; i < Styles.Length; i++)
				Styles[i] = null;
		}

		/// <summary>
		/// Clear style of all text
		/// </summary>
		public void ClearStyle(StyleIndex styleIndex)
		{
			foreach (var line in lines)
				line.ClearStyle(styleIndex);
			Invalidate();
		}

		/// <summary>
		/// Clears undo and redo stacks
		/// </summary>
		public void ClearUndo()
		{
			manager.ClearHistory();
		}

		private void InitDefaultStyle()
		{
			DefaultStyle = new SHTBTextStyle(null, null, FontStyle.Regular);
		}

		/// <summary>
		/// Insert text into current selection position
		/// </summary>
		/// <param name="text"></param>
		public void InsertText(string text)
		{
			if (text == null)
				return;

			manager.BeginAutoUndoCommands();
			try
			{
				if (Selection.Start != Selection.End)
					manager.ExecuteCommand(new ClearSelectedCommand(this));

				manager.ExecuteCommand(new InsertTextCommand(this, text));
				if (updating <= 0)
					DoCaretVisible();
			}
			finally { manager.EndAutoUndoCommands(); }
			//
			Invalidate();
		}

		/// <summary>
		/// Insert text into current selection position (with predefined style)
		/// </summary>
		/// <param name="text"></param>
		public void InsertText(string text, SHTBStyle style)
		{
			if (text == null)
				return;

			//remember last caret position
			var last = Selection.Start;
			//insert text
			InsertText(text);
			//get range
			var range = new SHTBRange(this, last, Selection.Start);
			//set style for range
			range.SetStyle(style);
		}

		/// <summary>
		/// Appends text to the current text of a text box.
		/// </summary>
		/// <param name="text">The text to append to the current contents of the text box.</param>
		public void AppendText(string text)
		{
			if (String.IsNullOrEmpty(text)) return;

			var oldStart = Selection.Start;
			var oldEnd = Selection.End;

			Selection.BeginUpdate();
			manager.BeginAutoUndoCommands();
			try
			{
				if (lines.Count > 0)
				{
					Selection.Start = new SHTBPlace(lines[lines.Count - 1].Count, lines.Count - 1);
				}
				else
				{
					Selection.Start = new SHTBPlace(0, 0);
				}
				manager.ExecuteCommand(new InsertTextCommand(this, text));
			}
			finally
			{
				manager.EndAutoUndoCommands();
				Selection.Start = oldStart;
				Selection.End = oldEnd;
				Selection.EndUpdate();
			}
			//
			Invalidate();
		}

		/// <summary>
		/// Gets or sets the font used when displaying text in the control.
		/// </summary>
		/// <remarks>Assign only monospaced fonts.</remarks>
		[DefaultValue(typeof(Font), "Consolas, 9.75")]
		public override Font Font
		{
			get
			{
				return base.Font;
			}
			set
			{
				SizeF sizeM = GetCharSize(value, 'M');

				#region Check whether the set font is monospaced.

				if (sizeM != GetCharSize(value, '.'))
				{
					base.Font = new Font("Courier New", value.SizeInPoints, FontStyle.Regular, GraphicsUnit.Point);
				}
				else base.Font = value;

				#endregion Check whether the set font is monospaced.

				#region Calculate the character size

				CharWidth = (int)sizeM.Width - 1;
				CharHeight = (int)sizeM.Height - 2;

				#endregion Calculate the character size

				Invalidate();
			}
		}

		/// <summary>
		/// Returns index of the style in Styles
		/// -1 otherwise
		/// </summary>
		/// <param name="style"></param>
		/// <returns>Index of the style in Styles</returns>
		public int GetStyleIndex(SHTBStyle style)
		{
			return Array.IndexOf<SHTBStyle>(Styles, style);
		}

		/// <summary>
		/// Returns StyleIndex mask of given styles
		/// </summary>
		/// <param name="styles"></param>
		/// <returns>StyleIndex mask of given styles</returns>
		public StyleIndex GetStyleIndexMask(SHTBStyle[] styles)
		{
			StyleIndex mask = StyleIndex.None;
			foreach (SHTBStyle style in styles)
			{
				int i = GetStyleIndex(style);
				if (i >= 0)
					mask |= SHTBRange.ToStyleIndex(i);
			}

			return mask;
		}

		internal int GetOrSetStyleLayerIndex(SHTBStyle style)
		{
			int i = GetStyleIndex(style);
			if (i < 0)
				i = AddStyle(style);
			return i;
		}

		public static SizeF GetCharSize(Font font, char c)
		{
			Size sz2 = TextRenderer.MeasureText("<" + c.ToString() + ">", font);
			Size sz3 = TextRenderer.MeasureText("<>", font);
			return new SizeF(sz2.Width - sz3.Width + 1, font.Height);
		}

		IntPtr m_hImc;

		const int WM_IME_SETCONTEXT = 0x0281;
		const int WM_HSCROLL = 0x114;
		const int WM_VSCROLL = 0x115;
		const int SB_ENDSCROLL = 0x8;

		[DllImport("Imm32.dll")]
		public static extern IntPtr ImmGetContext(IntPtr hWnd);

		[DllImport("Imm32.dll")]
		public static extern IntPtr ImmAssociateContext(IntPtr hWnd, IntPtr hIMC);

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == WM_HSCROLL || m.Msg == WM_VSCROLL)
				if (m.WParam.ToInt32() != SB_ENDSCROLL)
					Invalidate();

			base.WndProc(ref m);

			if (ImeAllowed)
				if (m.Msg == WM_IME_SETCONTEXT && m.WParam.ToInt32() == 1)
				{
					ImmAssociateContext(this.Handle, m_hImc);
				}
		}

		protected override void OnScroll(ScrollEventArgs se)
		{
			base.OnScroll(se);
			OnVisibleRangeChanged();
			Invalidate();
		}

		private void InsertChar(char c)
		{
			manager.BeginAutoUndoCommands();
			try
			{
				if (Selection.Start != Selection.End)
					manager.ExecuteCommand(new ClearSelectedCommand(this));

				manager.ExecuteCommand(new InsertCharCommand(this, c));
			}
			finally { manager.EndAutoUndoCommands(); }

			Invalidate();
		}

		/// <summary>
		/// Deletes selected chars
		/// </summary>
		public void ClearSelected()
		{
			if (Selection.Start != Selection.End)
			{
				manager.ExecuteCommand(new ClearSelectedCommand(this));
				Invalidate();
			}
		}

		private void Recalc()
		{
			if (!needRecalc)
				return;

			needRecalc = false;
			//calc min left indent
			LeftIndent = LeftPadding;
			var maxLineNumber = this.LinesCount + lineNumberStartValue - 1;
			int charsForLineNumber = 2 + (maxLineNumber > 0 ? (int)Math.Log10(maxLineNumber) : 0);
			if (this.Created)
			{
				if (ShowLineNumbers)
					LeftIndent += charsForLineNumber * CharWidth + minLeftIndent + 1;
			}
			else
				needRecalc = true;
			//calc max line length and count of wordWrapLines
			int maxLineLength = 0;
			wordWrapLinesCount = 0;
			foreach (var line in lines)
			{
				if (line.Count > maxLineLength)
					maxLineLength = line.Count;
				line.startY = wordWrapLinesCount * CharHeight;
				wordWrapLinesCount += line.WordWrapStringsCount;
			}

			//adjust AutoScrollMinSize
			int minWidth = LeftIndent + (maxLineLength) * CharWidth + 2;
			if (wordWrap)
			{
				minWidth = 0;
			}
			AutoScrollMinSize = new Size(minWidth, wordWrapLinesCount * CharHeight);
		}

		private new Size AutoScrollMinSize
		{
			set
			{
				if (scrollBars)
				{
					base.AutoScroll = true;
					base.AutoScrollMinSize = value;
				}
				else
				{
					base.AutoScroll = false;
					base.AutoScrollMinSize = new Size(0, 0);
					VerticalScroll.Visible = false;
					HorizontalScroll.Visible = false;
					HorizontalScroll.Maximum = value.Width;
					VerticalScroll.Maximum = value.Height;
				}
			}

			get
			{
				if (scrollBars)
					return base.AutoScrollMinSize;
				else
					return new Size(HorizontalScroll.Maximum, VerticalScroll.Maximum);
			}
		}

		private void RecalcWordWrap(int fromLine, int toLine)
		{
			int maxCharsPerLine = 0;

			maxCharsPerLine = (ClientSize.Width - LeftIndent) / CharWidth;

			for (int currentLine = fromLine; currentLine <= toLine; currentLine++)
			{
				if (wordWrap)
				{
					lines[currentLine].CalcCutOffs(maxCharsPerLine, ImeAllowed);
				}
				else
				{
					lines[currentLine].CutOffPositions.Clear();
				}
			}
			needRecalc = true;
		}

		protected override void OnClientSizeChanged(EventArgs e)
		{
			base.OnClientSizeChanged(e);
			if (WordWrap)
			{
				RecalcWordWrap(0, lines.Count - 1);
				Invalidate();
			}
			OnVisibleRangeChanged();
		}

		/// <summary>
		/// Scroll control for display defined rectangle
		/// </summary>
		/// <param name="rect"></param>
		private void DoVisibleRectangle(Rectangle rect)
		{
			int oldV = VerticalScroll.Value;
			int v = VerticalScroll.Value;
			int h = HorizontalScroll.Value;

			if (rect.Bottom > ClientRectangle.Height)
				v += rect.Bottom - ClientRectangle.Height;
			else
				if (rect.Top < 0)
					v += rect.Top;

			if (rect.Right > ClientRectangle.Width)
				h += rect.Right - ClientRectangle.Width;
			else
				if (rect.Left < LeftIndent)
					h += rect.Left - LeftIndent;
			//
			if (!Multiline)
				v = 0;
			//
			try
			{
				VerticalScroll.Value = Math.Max(0, v);
				HorizontalScroll.Value = Math.Max(0, h);
			}
			catch (ArgumentOutOfRangeException) { ;}

			if (ShowScrollBars)
			{
				//some magic for update scrolls
				base.AutoScrollMinSize -= new Size(1, 0);
				base.AutoScrollMinSize += new Size(1, 0);
			}
			//
			if (oldV != VerticalScroll.Value)
				OnVisibleRangeChanged();
		}

		/// <summary>
		/// Scroll control for display caret
		/// </summary>
		public void DoCaretVisible()
		{
			Invalidate();
			Recalc();
			Point car = PlaceToPoint(Selection.Start);
			car.Offset(-CharWidth, 0);
			DoVisibleRectangle(new Rectangle(car, new Size(2 * CharWidth, 2 * CharHeight)));
		}

		/// <summary>
		/// Scroll control left
		/// </summary>
		public void ScrollLeft()
		{
			Invalidate();
			HorizontalScroll.Value = 0;
			AutoScrollMinSize -= new Size(1, 0);
			AutoScrollMinSize += new Size(1, 0);
		}

		/// <summary>
		/// Scroll control for display selection area
		/// </summary>
		public void DoSelectionVisible()
		{
			Recalc();
			DoVisibleRectangle(new Rectangle(PlaceToPoint(new SHTBPlace(0, Selection.End.LineNumber)), new Size(2 * CharWidth, 2 * CharHeight)));
			Point car = PlaceToPoint(Selection.Start);
			Point car2 = PlaceToPoint(Selection.End);
			car.Offset(-CharWidth, -ClientSize.Height / 2);
			DoVisibleRectangle(new Rectangle(car, new Size(Math.Abs(car2.X - car.X), /*Math.Abs(car2.Y-car.Y) + 2 * CharHeight*/ClientSize.Height)));
		}

		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);

			if (e.KeyCode == Keys.ShiftKey)
				lastModifiers &= ~Keys.Shift;
			if (e.KeyCode == Keys.Alt)
				lastModifiers &= ~Keys.Alt;
			if (e.KeyCode == Keys.ControlKey)
				lastModifiers &= ~Keys.Control;
		}

		bool handledChar = false;

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			lastModifiers = e.Modifiers;

			handledChar = false;

			if (e.Handled)
			{
				handledChar = true;
				return;
			}

			switch (e.KeyCode)
			{
				case Keys.F://Find
					if (e.Modifiers == Keys.Control)
						ShowFindDialog();
					break;
				case Keys.H://Replace
					if (e.Modifiers == Keys.Control)
						ShowReplaceDialog();
					break;
				case Keys.C://Copy
					if (e.Modifiers == Keys.Control)
						Copy();
					if (e.Modifiers == (Keys.Control | Keys.Shift))
						CommentSelected();
					break;
				case Keys.X://Cut
					if (e.Modifiers == Keys.Control)
						Cut();
					break;
				case Keys.V://Paste
					if (e.Modifiers == Keys.Control)
						Paste();
					break;
				case Keys.A://Select All
					if (e.Modifiers == Keys.Control)
						Selection.SelectAll();
					break;
				case Keys.Z://Undo
					if (e.Modifiers == Keys.Control)
						Undo();
					break;
				case Keys.Y://Redo
					if (e.Modifiers == Keys.Control)
						Redo();
					break;
				case Keys.Tab://Shift Tab -> Decrease Indent
					if (e.Modifiers == Keys.Shift)
						DecreaseIndent();
					break;

				case Keys.Back:
					if (e.Modifiers == Keys.Alt)
						Undo();
					else
						if (e.Modifiers == Keys.None)
						{
							if (OnKeyPressing('\b'))//KeyPress event processed key
								break;
							if (Selection.End != Selection.Start)
								ClearSelected();
							else
								InsertChar('\b');
							OnKeyPressed('\b');
						}

					break;
				case Keys.Delete:
					if (e.Modifiers == Keys.None)
					{
						if (OnKeyPressing((char)0xff))//KeyPress event processed key
							break;
						if (Selection.End != Selection.Start)
							ClearSelected();
						OnKeyPressed((char)0xff);
					}
					break;
				case Keys.Space:
					if (e.Modifiers == Keys.None || e.Modifiers == Keys.Shift)
					{
						if (OnKeyPressing(' '))//KeyPress event processed key
							break;
						if (Selection.End != Selection.Start)
							ClearSelected();
						else
							InsertChar(' ');
						OnKeyPressed(' ');
					}
					break;

				case Keys.Left:
					if (e.Modifiers == Keys.Control || e.Modifiers == (Keys.Control | Keys.Shift))
						Selection.GoWordLeft(e.Shift);
					if (e.Modifiers == Keys.None || e.Modifiers == Keys.Shift)
						Selection.GoLeft(e.Shift);
					break;
				case Keys.Right:
					if (e.Modifiers == Keys.Control || e.Modifiers == (Keys.Control | Keys.Shift))
						Selection.GoWordRight(e.Shift);
					if (e.Modifiers == Keys.None || e.Modifiers == Keys.Shift)
						Selection.GoRight(e.Shift);
					break;
				case Keys.Up:
					if (e.Modifiers == Keys.None || e.Modifiers == Keys.Shift)
					{
						Selection.GoUp(e.Shift);
						ScrollLeft();
					}
					break;
				case Keys.Down:
					if (e.Modifiers == Keys.None || e.Modifiers == Keys.Shift)
					{
						Selection.GoDown(e.Shift);
						ScrollLeft();
					}
					break;
				case Keys.PageUp:
					if (e.Modifiers == Keys.None || e.Modifiers == Keys.Shift)
					{
						Selection.GoPageUp(e.Shift);
						ScrollLeft();
					}
					break;
				case Keys.PageDown:
					if (e.Modifiers == Keys.None || e.Modifiers == Keys.Shift)
					{
						Selection.GoPageDown(e.Shift);
						ScrollLeft();
					}
					break;
				case Keys.Home:
					if (e.Modifiers == Keys.Control || e.Modifiers == (Keys.Control | Keys.Shift))
						Selection.GoFirst(e.Shift);
					if (e.Modifiers == Keys.None || e.Modifiers == Keys.Shift)
					{
						GoHome(e.Shift);
						ScrollLeft();
					}
					break;
				case Keys.End:
					if (e.Modifiers == Keys.Control || e.Modifiers == (Keys.Control | Keys.Shift))
						Selection.GoLast(e.Shift);
					if (e.Modifiers == Keys.None || e.Modifiers == Keys.Shift)
						Selection.GoEnd(e.Shift);
					break;
				default:
					if ((e.Modifiers & Keys.Control) != 0)
						return;
					if ((e.Modifiers & Keys.Alt) != 0)
						return;
					if (e.KeyCode == Keys.ShiftKey)
						return;
					break;
			}

			e.Handled = true;

			DoCaretVisible();
			Invalidate();
		}

		private void GoHome(bool shift)
		{
			Selection.BeginUpdate();
			try
			{
				int iLine = Selection.Start.LineNumber;
				int spaces = this[iLine].StartSpacesCount;
				if (Selection.Start.CharacterPosition <= spaces)
					Selection.GoHome(shift);
				else
				{
					Selection.GoHome(shift);
					for (int i = 0; i < spaces; i++)
						Selection.GoRight(shift);
				}
			}
			finally
			{
				Selection.EndUpdate();
			}
		}

		/// <summary>
		/// Insert/remove comment prefix into selected lines
		/// </summary>
		public void CommentSelected()
		{
			Selection.Normalize();
			if (lines[Selection.Start.LineNumber].Text.TrimStart().StartsWith(";")) RemoveLinePrefix(";");
			else InsertLinePrefix(";");
		}

		public void OnKeyPressing(KeyPressEventArgs args)
		{
			if (KeyPressing != null)
				KeyPressing(this, args);
		}

		private bool OnKeyPressing(char c)
		{
			KeyPressEventArgs args = new KeyPressEventArgs(c);
			OnKeyPressing(args);
			return args.Handled;
		}

		public void OnKeyPressed(char c)
		{
			KeyPressEventArgs args = new KeyPressEventArgs(c);
			if (KeyPressed != null)
				KeyPressed(this, args);
		}

		protected override bool ProcessMnemonic(char charCode)
		{
			return ProcessKeyPress(charCode) || base.ProcessMnemonic(charCode);
		}

		private bool ProcessKeyPress(char c)
		{
			if (handledChar)
				return true;

			if (c == ' ')
				return true;

			if (c == '\b' && (lastModifiers & Keys.Alt) != 0)
				return true;

			if (char.IsControl(c) && c != '\r' && c != '\t')
				return false;

			if (!Enabled)
				return false;

			if (lastModifiers != Keys.None &&
				lastModifiers != Keys.Shift &&
				lastModifiers != (Keys.Control | Keys.Alt) &&//ALT+CTRL is special chars (AltGr)
				lastModifiers != (Keys.Shift | Keys.Control | Keys.Alt) &&//SHIFT + ALT + CTRL is special chars (AltGr)
				(lastModifiers != (Keys.Alt) || char.IsLetterOrDigit(c))//may be ALT+LetterOrDigit is mnemonic code
				)
				return false;//do not process Ctrl+? and Alt+? keys

			char sourceC = c;
			if (OnKeyPressing(sourceC))//KeyPress event processed key
				return true;

			//tab?
			if (c == '\t')
			{
				if (Selection.Start == Selection.End)
				{
					//insert tab as spaces
					int spaces = TabLength - (Selection.Start.CharacterPosition % TabLength);
					InsertText(new String(' ', spaces));
				}
				else
					if ((lastModifiers & Keys.Shift) == 0)
						IncreaseIndent();
			}
			else
			{
				//replace \r on \n
				if (c == '\r')
				{
					c = '\n';
				}
				//insert char
				InsertChar(c);
			}

			DoCaretVisible();
			Invalidate();

			OnKeyPressed(sourceC);

			return true;
		}

		private void RemoveSpacesAfterCaret()
		{
			if (Selection.Start != Selection.End)
				return;
			var end = Selection.Start;
			while (Selection.CharAfterStart == ' ')
				Selection.GoRight(true);
			ClearSelected();
		}

		private int GetMinStartSpacesCount(int fromLine, int toLine)
		{
			if (fromLine > toLine)
				return 0;

			int result = int.MaxValue;
			for (int i = fromLine; i <= toLine; i++)
			{
				int count = lines[i].StartSpacesCount;
				if (count < result)
					result = count;
			}

			return result;
		}

		/// <summary>
		/// Indicates that IME is allowed (for CJK language entering)
		/// </summary>
		[Browsable(false)]
		public bool ImeAllowed
		{
			get
			{
				return ImeMode != System.Windows.Forms.ImeMode.Disable &&
						ImeMode != System.Windows.Forms.ImeMode.Off &&
						ImeMode != System.Windows.Forms.ImeMode.NoControl;
			}
		}

		/// <summary>
		/// Undo last operation
		/// </summary>
		public void Undo()
		{
			manager.Undo();
			Invalidate();
		}

		/// <summary>
		/// Is undo enabled?
		/// </summary>
		[Browsable(false)]
		public bool UndoEnabled
		{
			get { return manager.UndoEnabled; }
		}

		/// <summary>
		/// Redo
		/// </summary>
		public void Redo()
		{
			manager.Redo();
			Invalidate();
		}

		/// <summary>
		/// Is redo enabled?
		/// </summary>
		[Browsable(false)]
		public bool RedoEnabled
		{
			get { return manager.RedoEnabled; }
		}

		protected override bool IsInputChar(char charCode)
		{
			return base.IsInputChar(charCode);
		}

		protected override bool IsInputKey(Keys keyData)
		{
			if ((keyData & Keys.Alt) == Keys.None)
			{
				Keys keys = keyData & Keys.KeyCode;
				if (keys == Keys.Return)
					return true;
			}

			if ((keyData & Keys.Alt) != Keys.Alt)
			{
				switch ((keyData & Keys.KeyCode))
				{
					case Keys.Prior:
					case Keys.Next:
					case Keys.End:
					case Keys.Home:
					case Keys.Left:
					case Keys.Right:
					case Keys.Up:
					case Keys.Down:
						return true;

					case Keys.Escape:
						return false;

					case Keys.Tab:
						return (keyData & Keys.Control) == Keys.None;
				}
			}

			return base.IsInputKey(keyData);
		}

		[DllImport("User32.dll")]
		private static extern bool CreateCaret(IntPtr hWnd, int hBitmap, int nWidth, int nHeight);

		[DllImport("User32.dll")]
		private static extern bool SetCaretPos(int x, int y);

		[DllImport("User32.dll")]
		private static extern bool DestroyCaret();

		[DllImport("User32.dll")]
		private static extern bool ShowCaret(IntPtr hWnd);

		[DllImport("User32.dll")]
		private static extern bool HideCaret(IntPtr hWnd);

		private int LeftIndentLine
		{
			get { return LeftIndent - minLeftIndent / 2 - 3; }
		}

		/// <summary>
		/// Draw control
		/// </summary>
		protected override void OnPaint(PaintEventArgs e)
		{
			if (needRecalc)
			{
				Recalc();
			}
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			//
			Brush lineNumberBrush = new SolidBrush(LineNumberColor);
			Brush changedLineBrush = new SolidBrush(ChangedLineColor);
			Brush indentBrush = new SolidBrush(IndentBackColor);
			Pen currentLinePen = new Pen(CurrentLineColor);
			Brush currentLineBrush = new SolidBrush(Color.FromArgb(50, CurrentLineColor));
			//draw indent area
			e.Graphics.FillRectangle(indentBrush, 0, 0, LeftIndentLine, ClientSize.Height);

			int firstChar = HorizontalScroll.Value / CharWidth;
			int lastChar = (HorizontalScroll.Value + ClientSize.Width) / CharWidth;

			#region Character Drawing

			for (int lineNumber = YtoLineIndex(VerticalScroll.Value); lineNumber < lines.Count; lineNumber++)
			{
				SHTBLine currentLine = lines[lineNumber];
				//Skip line
				if (currentLine.startY > VerticalScroll.Value + ClientSize.Height)
					break;
				if (currentLine.startY + currentLine.WordWrapStringsCount * CharHeight < VerticalScroll.Value)
					continue;

				int y = currentLine.startY - VerticalScroll.Value;
				//
				e.Graphics.SmoothingMode = SmoothingMode.None;
				//draw line background
				if (currentLine.BackgroundBrush != null)
				{
					e.Graphics.FillRectangle(currentLine.BackgroundBrush, new Rectangle(LeftIndent, y, Width, CharHeight * currentLine.WordWrapStringsCount));
				}
				//draw current line background
				if (CurrentLineColor != Color.Transparent && lineNumber == Selection.Start.LineNumber)
				{
					if (Selection.Start == Selection.End)
					{
						e.Graphics.FillRectangle(currentLineBrush, new Rectangle(LeftIndent, y, Width, CharHeight));
					}
					else
					{
						e.Graphics.DrawLine(currentLinePen, LeftIndent, y + CharHeight, Width, y + CharHeight);
					}
				}
				//draw changed line
				if (ChangedLineColor != Color.Transparent && currentLine.IsChanged)
				{
					e.Graphics.FillRectangle(changedLineBrush, new RectangleF(-10, y, LeftIndent - minLeftIndent - 2 + 10, CharHeight + 1));
				}

				e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
				//OnPaint event
				OnPaintLine(new PaintLineEventArgs(lineNumber, new Rectangle(LeftIndent, y, Width, CharHeight * currentLine.WordWrapStringsCount), e.Graphics, e.ClipRectangle));
				//draw line number
				if (ShowLineNumbers)
				{
					e.Graphics.DrawString((lineNumber + lineNumberStartValue).ToString(), Font, lineNumberBrush, new RectangleF(-10, y, LeftIndent - minLeftIndent - 2 + 10, CharHeight), new StringFormat(StringFormatFlags.DirectionRightToLeft));
				}
				//draw wordwrap strings of line
				for (int iWordWrapLine = 0; iWordWrapLine < currentLine.WordWrapStringsCount; iWordWrapLine++)
				{
					y = currentLine.startY + iWordWrapLine * CharHeight - VerticalScroll.Value;
					//draw chars
					DrawLineChars(e, firstChar, lastChar, lineNumber, iWordWrapLine, y);
				}
			}

			#endregion Character Drawing

			e.Graphics.SmoothingMode = SmoothingMode.None;

			#region Caret Drawing

			Point car = PlaceToPoint(Selection.Start);
			if (Focused && car.X >= LeftIndent)
			{
				CreateCaret(this.Handle, 0, 1, CharHeight + 1);
				SetCaretPos(car.X, car.Y);
				ShowCaret(this.Handle);
				e.Graphics.DrawLine(Pens.Black, car.X, car.Y, car.X, car.Y + CharHeight);
			}
			else
				HideCaret(this.Handle);

			#endregion Caret Drawing

			//dispose resources
			lineNumberBrush.Dispose();
			changedLineBrush.Dispose();
			indentBrush.Dispose();
			currentLinePen.Dispose();
			currentLineBrush.Dispose();

			base.OnPaint(e);
		}

		private void DrawLineChars(PaintEventArgs e, int firstChar, int lastChar, int iLine, int iWordWrapLine, int y)
		{
			SHTBLine line = lines[iLine];
			int from = line.GetWordWrapStringStartPosition(iWordWrapLine);
			int to = line.GetWordWrapStringFinishPosition(iWordWrapLine);

			int startX = LeftIndent - HorizontalScroll.Value;
			if (startX < LeftIndent)
				firstChar++;

			lastChar = Math.Min(to - from, lastChar);

			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

			StyleIndex currentStyleIndex = StyleIndex.None;
			int iLastFlushedChar = firstChar - 1;

			for (int iChar = firstChar; iChar <= lastChar; iChar++)
			{
				StyleIndex style = line[from + iChar].Style;
				if (currentStyleIndex != style)
				{
					FlushRendering(e.Graphics, currentStyleIndex, new Point(startX + (iLastFlushedChar + 1) * CharWidth, y), new SHTBRange(this, from + iLastFlushedChar + 1, iLine, from + iChar, iLine));
					iLastFlushedChar = iChar - 1;
					currentStyleIndex = style;
				}
			}
			FlushRendering(e.Graphics, currentStyleIndex, new Point(startX + (iLastFlushedChar + 1) * CharWidth, y), new SHTBRange(this, from + iLastFlushedChar + 1, iLine, from + lastChar + 1, iLine));

			//draw selection
			if (Selection.End != Selection.Start && lastChar >= firstChar)
			{
				e.Graphics.SmoothingMode = SmoothingMode.None;
				SHTBRange textRange = new SHTBRange(this, from + firstChar, iLine, from + lastChar + 1, iLine);
				textRange = Selection.GetIntersectionWith(textRange);
				if (textRange != null && SelectionStyle != null)
					SelectionStyle.Draw(e.Graphics, new Point(startX + (textRange.Start.CharacterPosition - from) * CharWidth, y), textRange);
			}
		}

		private void FlushRendering(Graphics gr, StyleIndex styleIndex, Point pos, SHTBRange range)
		{
			if (range.End > range.Start)
			{
				int mask = 1;
				bool hasTextStyle = false;
				for (int i = 0; i < Styles.Length; i++)
				{
					if (Styles[i] != null && ((int)styleIndex & mask) != 0)
					{
						SHTBStyle style = Styles[i];
						bool isTextStyle = style is SHTBTextStyle;
						if (!hasTextStyle || !isTextStyle || AllowSeveralTextStyleDrawing)//cancelling secondary rendering by TextStyle
							style.Draw(gr, pos, range);//rendering
						hasTextStyle |= isTextStyle;
					}
					mask = mask << 1;
				}
				//draw by default renderer
				if (!hasTextStyle)
					DefaultStyle.Draw(gr, pos, range);
			}
		}

		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);
			mouseIsDrag = false;
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			if (e.Button == System.Windows.Forms.MouseButtons.Left)
			{
				mouseIsDrag = true;
				//click on text
				var oldEnd = Selection.End;
				Selection.BeginUpdate();
				Selection.Start = PointToPlace(e.Location);
				if ((lastModifiers & Keys.Shift) != 0)
					Selection.End = oldEnd;
				Selection.EndUpdate();
				Invalidate();
				return;
			}
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			Invalidate();
			base.OnMouseWheel(e);
			OnVisibleRangeChanged();
		}

		bool mouseIsDrag = false;

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			if (e.Button == System.Windows.Forms.MouseButtons.Left && mouseIsDrag)
			{
				var oldEnd = Selection.End;
				Selection.BeginUpdate();
				Selection.Start = PointToPlace(e.Location);
				Selection.End = oldEnd;
				Selection.EndUpdate();
				DoCaretVisible();
				Invalidate();
				return;
			}

			Cursor = Cursors.IBeam;
		}

		protected override void OnMouseDoubleClick(MouseEventArgs e)
		{
			base.OnMouseDoubleClick(e);

			SHTBPlace p = PointToPlace(e.Location);
			int fromX = p.CharacterPosition;
			int toX = p.CharacterPosition;

			for (int i = p.CharacterPosition; i < lines[p.LineNumber].Count; i++)
			{
				char c = lines[p.LineNumber][i].Character;
				if (char.IsLetterOrDigit(c) || c == '_')
					toX = i + 1;
				else
					break;
			}

			for (int i = p.CharacterPosition - 1; i >= 0; i--)
			{
				char c = lines[p.LineNumber][i].Character;
				if (char.IsLetterOrDigit(c) || c == '_')
					fromX = i;
				else
					break;
			}

			Selection.Start = new SHTBPlace(toX, p.LineNumber);
			Selection.End = new SHTBPlace(fromX, p.LineNumber);

			Invalidate();
		}

		private int YtoLineIndex(int y)
		{
			int i = lines.BinarySearch(null, new LineYComparer(y));
			i = i < 0 ? -i - 2 : i;
			if (i < 0) return 0;
			if (i > lines.Count - 1) return lines.Count - 1;
			return i;
		}

		private class LineYComparer : IComparer<SHTBLine>
		{
			int Y;

			public LineYComparer(int Y)
			{
				this.Y = Y;
			}

			public int Compare(SHTBLine x, SHTBLine y)
			{
				if (x == null)
					return -y.startY.CompareTo(Y);
				else
					return x.startY.CompareTo(Y);
			}
		}

		/// <summary>
		/// Gets nearest line and char position from coordinates
		/// </summary>
		/// <param name="point">Point</param>
		/// <returns>Line and char position</returns>
		public SHTBPlace PointToPlace(Point point)
		{
			point.Offset(HorizontalScroll.Value, VerticalScroll.Value);
			point.Offset(-LeftIndent, 0);
			int iLine = YtoLineIndex(point.Y);
			int y = 0;

			for (; iLine < lines.Count; iLine++)
			{
				y = lines[iLine].startY + lines[iLine].WordWrapStringsCount * CharHeight;
				if (y > point.Y)
					break;
			}
			if (iLine >= lines.Count)
				iLine = lines.Count - 1;
			int iWordWrapLine = lines[iLine].WordWrapStringsCount;
			do
			{
				iWordWrapLine--;
				y -= CharHeight;
			} while (y > point.Y);
			if (iWordWrapLine < 0) iWordWrapLine = 0;
			//
			int start = lines[iLine].GetWordWrapStringStartPosition(iWordWrapLine);
			int finish = lines[iLine].GetWordWrapStringFinishPosition(iWordWrapLine);
			int x = (int)Math.Round((float)point.X / CharWidth);
			x = x < 0 ? start : start + x;
			if (x > finish)
				x = finish + 1;
			if (x > lines[iLine].Count)
				x = lines[iLine].Count;
			return new SHTBPlace(x, iLine);
		}

		/// <summary>
		/// Gets nearest absolute text position for given point
		/// </summary>
		/// <param name="point">Point</param>
		/// <returns>Position</returns>
		public int PointToPosition(Point point)
		{
			return PlaceToPosition(PointToPlace(point));
		}

		/// <summary>
		/// Fires TextChanging event
		/// </summary>
		public virtual void OnTextChanging(ref string text)
		{
			if (TextChanging != null)
			{
				var args = new TextChangingEventArgs() { InsertingText = text };
				TextChanging(this, args);
				text = args.InsertingText;
			};
		}

		public virtual void OnTextChanging()
		{
			string temp = null;
			OnTextChanging(ref temp);
		}

		/// <summary>
		/// Fires TextChanged event
		/// </summary>
		public virtual void OnTextChanged()
		{
			SHTBRange r = new SHTBRange(this);
			r.SelectAll();
			OnTextChanged(new TextChangedEventArgs(r));
		}

		/// <summary>
		/// Fires TextChanged event
		/// </summary>
		public virtual void OnTextChanged(int fromLine, int toLine)
		{
			SHTBRange r = new SHTBRange(this);
			r.Start = new SHTBPlace(0, Math.Min(fromLine, toLine));
			r.End = new SHTBPlace(lines[Math.Max(fromLine, toLine)].Count, Math.Max(fromLine, toLine));
			OnTextChanged(new TextChangedEventArgs(r));
		}

		/// <summary>
		/// Fires TextChanged event
		/// </summary>
		public virtual void OnTextChanged(SHTBRange r)
		{
			OnTextChanged(new TextChangedEventArgs(r));
		}

		int updating;
		SHTBRange updatingRange = null;

		public void BeginUpdate()
		{
			if (updating == 0)
				updatingRange = null;
			updating++;
		}

		public void EndUpdate()
		{
			updating--;

			if (updating == 0 && updatingRange != null)
			{
				updatingRange.Expand();
				OnTextChanged(updatingRange);
			}
		}

		/// <summary>
		/// Fires TextChanged event
		/// </summary>
		protected virtual void OnTextChanged(TextChangedEventArgs args)
		{
			//
			args.ChangedRange.Normalize();
			//
			if (updating > 0)
			{
				if (updatingRange == null)
					updatingRange = args.ChangedRange.Clone();
				else
				{
					if (updatingRange.Start.LineNumber > args.ChangedRange.Start.LineNumber)
						updatingRange.Start = new SHTBPlace(0, args.ChangedRange.Start.LineNumber);
					if (updatingRange.End.LineNumber < args.ChangedRange.End.LineNumber)
						updatingRange.End = new SHTBPlace(lines[args.ChangedRange.End.LineNumber].Count, args.ChangedRange.End.LineNumber);
					updatingRange = updatingRange.GetIntersectionWith(TextRange);
				}
				return;
			}
			//
			IsChanged = true;
			TextVersion++;
			MarkLinesAsChanged(args.ChangedRange);
			//
			if (wordWrap)
				RecalcWordWrap(args.ChangedRange.Start.LineNumber, args.ChangedRange.End.LineNumber);
			//
			base.OnTextChanged(args);

			OnSyntaxHighlight(args);
			//
			if (TextChanged != null)
				TextChanged(this, args);
			//
			if (BindingTextChanged != null)
				BindingTextChanged(this, EventArgs.Empty);
			//
			base.OnTextChanged(EventArgs.Empty);
			//

			OnVisibleRangeChanged();
		}

		private void MarkLinesAsChanged(SHTBRange range)
		{
			for (int iLine = range.Start.LineNumber; iLine <= range.End.LineNumber; iLine++)
				if (iLine >= 0 && iLine < lines.Count)
					lines[iLine].IsChanged = true;
		}

		/// <summary>
		/// Fires SelectionCnaged event
		/// </summary>
		public virtual void OnSelectionChanged()
		{
			if (SelectionChanged != null)
				SelectionChanged(this, new EventArgs());
		}

		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			Invalidate();
		}

		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			Invalidate();
		}

		/// <summary>
		/// Gets absolute text position from line and char position
		/// </summary>
		/// <param name="point">Line and char position</param>
		/// <returns>Index of text char</returns>
		public int PlaceToPosition(SHTBPlace point)
		{
			if (point.LineNumber < 0 || point.LineNumber >= lines.Count || point.CharacterPosition >= lines[point.LineNumber].Count + Environment.NewLine.Length)
				return -1;

			int result = 0;
			for (int i = 0; i < point.LineNumber; i++)
				result += lines[i].Count + Environment.NewLine.Length;
			result += point.CharacterPosition;

			return result;
		}

		/// <summary>
		/// Gets line and char position from absolute text position
		/// </summary>
		/// <param name="pos"></param>
		/// <returns></returns>
		public SHTBPlace PositionToPlace(int pos)
		{
			if (pos < 0)
				return new SHTBPlace(0, 0);

			for (int i = 0; i < lines.Count; i++)
			{
				int lineLength = lines[i].Count + Environment.NewLine.Length;
				if (pos < lines[i].Count)
					return new SHTBPlace(pos, i);
				if (pos < lineLength)
					return new SHTBPlace(lines[i].Count, i);

				pos -= lineLength;
			}

			if (lines.Count > 0)
				return new SHTBPlace(lines[lines.Count - 1].Count, lines.Count - 1);
			else
				return new SHTBPlace(0, 0);
			//throw new ArgumentOutOfRangeException("Position out of range");
		}

		/// <summary>
		/// Gets point for given line and char position
		/// </summary>
		/// <param name="palce">Line and char position</param>
		/// <returns>Coordiantes</returns>
		public Point PlaceToPoint(SHTBPlace place)
		{
			int lineNumber = place.LineNumber;
			if (lineNumber > lines.Count) lineNumber = lines.Count - 1;
			int y = lines[lineNumber].startY;
			//
			int iWordWrapIndex = lines[lineNumber].GetWordWrapStringIndex(place.CharacterPosition);
			y += iWordWrapIndex * CharHeight;
			int x = (place.CharacterPosition - lines[lineNumber].GetWordWrapStringStartPosition(iWordWrapIndex)) * CharWidth;
			//
			y = y - VerticalScroll.Value;
			x = LeftIndent + x - HorizontalScroll.Value;

			return new Point(x, y);
		}

		/// <summary>
		/// Range of all text
		/// </summary>
		[Browsable(false)]
		public SHTBRange TextRange
		{
			get
			{
				return new SHTBRange(this, new SHTBPlace(0, 0), new SHTBPlace(lines[lines.Count - 1].Count, lines.Count - 1));
			}
		}

		/// <summary>
		/// Get range of text
		/// </summary>
		/// <param name="fromPos">Absolute start position</param>
		/// <param name="toPos">Absolute finish position</param>
		/// <returns>Range</returns>
		public SHTBRange GetRange(int fromPos, int toPos)
		{
			var sel = new SHTBRange(this);
			sel.Start = PositionToPlace(fromPos);
			sel.End = PositionToPlace(toPos);
			return sel;
		}

		/// <summary>
		/// Get range of text
		/// </summary>
		/// <param name="fromPlace">Line and char position</param>
		/// <param name="toPlace">Line and char position</param>
		/// <returns>Range</returns>
		public SHTBRange GetRange(SHTBPlace fromPlace, SHTBPlace toPlace)
		{
			return new SHTBRange(this, fromPlace, toPlace);
		}

		/// <summary>
		/// Finds ranges for given regex pattern
		/// </summary>
		/// <param name="regexPattern">Regex pattern</param>
		/// <returns>Enumeration of ranges</returns>
		public IEnumerable<SHTBRange> GetRanges(string regexPattern)
		{
			SHTBRange range = new SHTBRange(this);
			range.SelectAll();
			//
			Regex reg = new Regex(regexPattern);
			foreach (var r in range.GetRanges(reg))
			{
				yield return r;
			}
		}

		/// <summary>
		/// Finds ranges for given regex pattern
		/// </summary>
		/// <param name="regexPattern">Regex pattern</param>
		/// <returns>Enumeration of ranges</returns>
		public IEnumerable<SHTBRange> GetRanges(string regexPattern, RegexOptions options)
		{
			SHTBRange range = new SHTBRange(this);
			range.SelectAll();
			//
			Regex reg = new Regex(regexPattern, options);
			foreach (var r in range.GetRanges(reg))
			{
				yield return r;
			}
		}

		/// <summary>
		/// Get text of given line
		/// </summary>
		/// <param name="iLine">Line index</param>
		/// <returns>Text</returns>
		public string GetLineText(int iLine)
		{
			if (iLine < 0 || iLine >= lines.Count)
				throw new ArgumentOutOfRangeException("Line index out of range");
			StringBuilder sb = new StringBuilder(lines[iLine].Count);
			foreach (SHTBChar c in lines[iLine])
				sb.Append(c.Character);
			return sb.ToString();
		}

		/// <summary>
		/// Insert TAB into front of seletcted lines
		/// </summary>
		public void IncreaseIndent()
		{
			if (Selection.Start == Selection.End)
				return;
			var old = Selection.Clone();
			int from = Math.Min(Selection.Start.LineNumber, Selection.End.LineNumber);
			int to = Math.Max(Selection.Start.LineNumber, Selection.End.LineNumber);
			BeginUpdate();
			Selection.BeginUpdate();
			manager.BeginAutoUndoCommands();
			for (int i = from; i <= to; i++)
			{
				if (lines[i].Count == 0) continue;
				Selection.Start = new SHTBPlace(0, i);
				manager.ExecuteCommand(new InsertTextCommand(this, new String(' ', TabLength)));
			}
			manager.EndAutoUndoCommands();
			Selection.Start = new SHTBPlace(0, from);
			Selection.End = new SHTBPlace(lines[to].Count, to);
			needRecalc = true;
			Selection.EndUpdate();
			EndUpdate();
			Invalidate();
		}

		/// <summary>
		/// Remove TAB from front of seletcted lines
		/// </summary>
		public void DecreaseIndent()
		{
			if (Selection.Start == Selection.End)
				return;
			var old = Selection.Clone();
			int from = Math.Min(Selection.Start.LineNumber, Selection.End.LineNumber);
			int to = Math.Max(Selection.Start.LineNumber, Selection.End.LineNumber);
			BeginUpdate();
			Selection.BeginUpdate();
			manager.BeginAutoUndoCommands();
			for (int i = from; i <= to; i++)
			{
				Selection.Start = new SHTBPlace(0, i);
				Selection.End = new SHTBPlace(Math.Min(lines[i].Count, TabLength), i);
				if (Selection.Text.Trim() == "")
					ClearSelected();
			}
			manager.EndAutoUndoCommands();
			Selection.Start = new SHTBPlace(0, from);
			Selection.End = new SHTBPlace(lines[to].Count, to);
			needRecalc = true;
			EndUpdate();
			Selection.EndUpdate();
		}

		/// <summary>
		/// Insert prefix into front of seletcted lines
		/// </summary>
		public void InsertLinePrefix(string prefix)
		{
			var old = Selection.Clone();
			int from = Math.Min(Selection.Start.LineNumber, Selection.End.LineNumber);
			int to = Math.Max(Selection.Start.LineNumber, Selection.End.LineNumber);
			BeginUpdate();
			Selection.BeginUpdate();
			manager.BeginAutoUndoCommands();
			int spaces = GetMinStartSpacesCount(from, to);
			for (int i = from; i <= to; i++)
			{
				Selection.Start = new SHTBPlace(spaces, i);
				manager.ExecuteCommand(new InsertTextCommand(this, prefix));
			}
			Selection.Start = new SHTBPlace(0, from);
			Selection.End = new SHTBPlace(lines[to].Count, to);
			needRecalc = true;
			manager.EndAutoUndoCommands();
			Selection.EndUpdate();
			EndUpdate();
			Invalidate();
		}

		/// <summary>
		/// Remove prefix from front of seletcted lines
		/// </summary>
		public void RemoveLinePrefix(string prefix)
		{
			var old = Selection.Clone();
			int from = Math.Min(Selection.Start.LineNumber, Selection.End.LineNumber);
			int to = Math.Max(Selection.Start.LineNumber, Selection.End.LineNumber);
			BeginUpdate();
			Selection.BeginUpdate();
			manager.BeginAutoUndoCommands();
			for (int i = from; i <= to; i++)
			{
				string text = lines[i].Text;
				string trimmedText = text.TrimStart();
				if (trimmedText.StartsWith(prefix))
				{
					int spaces = text.Length - trimmedText.Length;
					Selection.Start = new SHTBPlace(spaces, i);
					Selection.End = new SHTBPlace(spaces + prefix.Length, i);
					ClearSelected();
				}
			}
			Selection.Start = new SHTBPlace(0, from);
			Selection.End = new SHTBPlace(lines[to].Count, to);
			needRecalc = true;
			manager.EndAutoUndoCommands();
			Selection.EndUpdate();
			EndUpdate();
		}

		public virtual void OnSyntaxHighlight(TextChangedEventArgs args)
		{
			SHTBRange range;

			switch (HighlightingRangeType)
			{
				case HighlightingRangeType.VisibleRange:
					range = VisibleRange.GetUnionWith(args.ChangedRange);
					break;
				case HighlightingRangeType.AllTextRange:
					range = TextRange;
					break;
				default:
					range = args.ChangedRange;
					break;
			}

			SyntaxHighlighter.HighlightSyntax(range);
		}

		private void InitializeComponent()
		{
			this.SuspendLayout();
			//
			// SyntaxHighlightingTextBox
			//
			this.Name = "SyntaxHighlightingTextBox";
			this.ResumeLayout(false);
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
			{
				if (findForm != null)
					findForm.Dispose();

				if (replaceForm != null)
					replaceForm.Dispose();

				if (Font != null)
					Font.Dispose();
			}
		}

		protected virtual void OnPaintLine(PaintLineEventArgs e)
		{
			if (PaintLine != null)
				PaintLine(this, e);
		}

		internal int GenerateUniqueLineId()
		{
			return lastLineUniqueId++;
		}

		internal virtual void InsertLine(int index, SHTBLine line)
		{
			lines.Insert(index, line);

			if (LineInserted != null)
				LineInserted(this, new LineInsertedEventArgs(index, 1));
		}

		internal virtual void RemoveLine(int index)
		{
			RemoveLine(index, 1);
		}

		internal virtual void RemoveLine(int index, int count)
		{
			List<int> removedLineIds = new List<int>();
			//
			if (count > 0)
				if (LineRemoved != null)
					for (int i = 0; i < count; i++)
						removedLineIds.Add(this[index + i].UniqueId);
			//
			lines.RemoveRange(index, count);

			if (count > 0)
				if (LineRemoved != null)
					LineRemoved(this, new LineRemovedEventArgs(index, count, removedLineIds));
		}
	}

	public class PaintLineEventArgs : PaintEventArgs
	{
		public int LineIndex { get; private set; }

		public Rectangle LineRect { get; private set; }

		public PaintLineEventArgs(int iLine, Rectangle rect, Graphics gr, Rectangle clipRect)
			: base(gr, clipRect)
		{
			LineIndex = iLine;
			LineRect = rect;
		}
	}

	public class LineInsertedEventArgs : EventArgs
	{
		/// <summary>
		/// Inserted line index
		/// </summary>
		public int Index { get; private set; }

		/// <summary>
		/// Count of inserted lines
		/// </summary>
		public int Count { get; private set; }

		public LineInsertedEventArgs(int index, int count)
		{
			this.Index = index;
			this.Count = count;
		}
	}

	public class LineRemovedEventArgs : EventArgs
	{
		/// <summary>
		/// Removed line index
		/// </summary>
		public int Index { get; private set; }

		/// <summary>
		/// Count of removed lines
		/// </summary>
		public int Count { get; private set; }

		/// <summary>
		/// UniqueIds of removed lines
		/// </summary>
		public List<int> RemovedLineUniqueIds { get; private set; }

		public LineRemovedEventArgs(int index, int count, List<int> removedLineIds)
		{
			this.Index = index;
			this.Count = count;
			this.RemovedLineUniqueIds = removedLineIds;
		}
	}

	/// <summary>
	/// TextChanged event argument
	/// </summary>
	public class TextChangedEventArgs : EventArgs
	{
		/// <summary>
		/// This range contains changed area of text
		/// </summary>
		public SHTBRange ChangedRange { get; set; }

		/// <summary>
		/// Constructor
		/// </summary>
		public TextChangedEventArgs(SHTBRange changedRange)
		{
			this.ChangedRange = changedRange;
		}
	}

	public class TextChangingEventArgs : EventArgs
	{
		public string InsertingText { get; set; }
	}

	public class TabChangingEventArgs : EventArgs
	{
		public TabChangingReason Reason { get; private set; }

		public bool Cancel { get; set; }

		public TabChangingEventArgs(TabChangingReason reason)
		{
			this.Reason = reason;
		}
	}

	public enum TabChangingReason
	{
		Programm, User
	}

	public enum WordWrapMode
	{
		/// <summary>
		/// Word wrapping by control width
		/// </summary>
		WordWrapControlWidth,
		/// <summary>
		/// Word wrapping by preferred line width (PreferredLineWidth)
		/// </summary>
		WordWrapPreferredWidth,
		/// <summary>
		/// Char wrapping by control width
		/// </summary>
		CharWrapControlWidth,
		/// <summary>
		/// Char wrapping by preferred line width (PreferredLineWidth)
		/// </summary>
		CharWrapPreferredWidth
	}

	/// <summary>
	/// Type of highlighting
	/// </summary>
	public enum HighlightingRangeType
	{
		/// <summary>
		/// Highlight only changed range of text. Highest performance.
		/// </summary>
		ChangedRange,
		/// <summary>
		/// Highlight visible range of text. Middle performance.
		/// </summary>
		VisibleRange,
		/// <summary>
		/// Highlight all (visible and invisible) text. Lowest performance.
		/// </summary>
		AllTextRange
	}
}