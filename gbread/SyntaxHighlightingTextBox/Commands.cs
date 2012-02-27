using System;
using System.Collections.Generic;

namespace GBRead
{
    /// <summary>
    /// Insert single char
    /// </summary>
    /// <remarks>This operation includes also insertion of new line and removing char by backspace</remarks>
    internal class InsertCharCommand: UndoableCommand
    {
        char c;
        char deletedChar = '\x0';

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tb">Underlaying textbox</param>
        /// <param name="c">Inserting char</param>
        public InsertCharCommand(SyntaxHighlightingTextBox tb, char c): base(tb)
        {
            this.c = c;
        }

        /// <summary>
        /// Undo operation
        /// </summary>
        public override void Undo()
        {
            associatedTextBox.OnTextChanging();
            switch (c)
            {
                case '\n': MergeLines(currentSelection.Start.LineNumber, associatedTextBox); break;
                case '\r': break;
                case '\b':
                    associatedTextBox.Selection.Start = lastSelection.Start;
                    char cc = '\x0';
                    if (deletedChar != '\x0')
                    {
                        InsertChar(deletedChar, ref cc, associatedTextBox);
                    }
                    break;
                default:
                    associatedTextBox[currentSelection.Start.LineNumber].RemoveAt(currentSelection.Start.CharacterPosition);
                    associatedTextBox.Selection.Start = currentSelection.Start;
                    break;
            }

            associatedTextBox.needRecalc = true;

            base.Undo();
        }

        /// <summary>
        /// Execute operation
        /// </summary>
        public override void Execute()
        {
            string s = c.ToString();
            associatedTextBox.OnTextChanging(ref s);
            if (s.Length == 1)
                c = s[0];

            if (associatedTextBox.LinesCount == 0)
                InsertLine(associatedTextBox);
            InsertChar(c, ref deletedChar, associatedTextBox);
            associatedTextBox.needRecalc = true;
            base.Execute();
        }

        internal static void InsertChar(char c, ref char deletedChar, SyntaxHighlightingTextBox AssociatedTextBox)
        {
            switch (c)
            {
                case '\n':
                    if (AssociatedTextBox.LinesCount == 0)
                        InsertLine(AssociatedTextBox);
                    InsertLine(AssociatedTextBox);
                    break;
                case '\r': break;
                case '\b'://backspace
                    if (AssociatedTextBox.Selection.Start.CharacterPosition == 0 && AssociatedTextBox.Selection.Start.LineNumber == 0)
                        return;
                    if (AssociatedTextBox.Selection.Start.CharacterPosition == 0)
                    {
                        deletedChar = '\n';
                        MergeLines(AssociatedTextBox.Selection.Start.LineNumber - 1, AssociatedTextBox);
                    }
                    else
                    {
                        deletedChar = AssociatedTextBox[AssociatedTextBox.Selection.Start.LineNumber][AssociatedTextBox.Selection.Start.CharacterPosition - 1].Character;
                        AssociatedTextBox[AssociatedTextBox.Selection.Start.LineNumber].RemoveAt(AssociatedTextBox.Selection.Start.CharacterPosition - 1);
                        AssociatedTextBox.Selection.Start = new SHTBPlace(AssociatedTextBox.Selection.Start.CharacterPosition - 1, AssociatedTextBox.Selection.Start.LineNumber);
                    }
                    break;
                case '\t':
                    for (int i = 0; i < AssociatedTextBox.TabLength;i++ )
                        AssociatedTextBox[AssociatedTextBox.Selection.Start.LineNumber].Insert(AssociatedTextBox.Selection.Start.CharacterPosition, new SHTBChar(' '));
                    AssociatedTextBox.Selection.Start = new SHTBPlace(AssociatedTextBox.Selection.Start.CharacterPosition + AssociatedTextBox.TabLength, AssociatedTextBox.Selection.Start.LineNumber);
                    break;
                default:
                    AssociatedTextBox[AssociatedTextBox.Selection.Start.LineNumber].Insert(AssociatedTextBox.Selection.Start.CharacterPosition, new SHTBChar(c));
                    AssociatedTextBox.Selection.Start = new SHTBPlace(AssociatedTextBox.Selection.Start.CharacterPosition + 1, AssociatedTextBox.Selection.Start.LineNumber);
                    break;
            }
        }

        internal static void InsertLine(SyntaxHighlightingTextBox tb)
        {
            if (!tb.Multiline && tb.LinesCount > 0)
                return;

            if (tb.LinesCount == 0)
                tb.InsertLine(tb.Selection.Start.LineNumber + 1, new SHTBLine(tb.GenerateUniqueLineId()));
            else
                BreakLines(tb.Selection.Start.LineNumber, tb.Selection.Start.CharacterPosition, tb);

            tb.Selection.Start = new SHTBPlace(0, tb.Selection.Start.LineNumber + 1);
            tb.needRecalc = true;
        }

        /// <summary>
        /// Merge lines i and i+1
        /// </summary>
        internal static void MergeLines(int i, SyntaxHighlightingTextBox tb)
        {
            if (i + 1 >= tb.LinesCount)
                return;
            int pos = tb[i].Count;
            //
            if(tb[i].Count == 0)
                tb.RemoveLine(i);
            else
            if (tb[i + 1].Count == 0)
                tb.RemoveLine(i + 1);
            else
            {
                tb[i].AddRange(tb[i + 1]);
                tb.RemoveLine(i + 1);
            }
            tb.Selection.Start = new SHTBPlace(pos, i);
            tb.needRecalc = true;
        }

        internal static void BreakLines(int iLine, int pos, SyntaxHighlightingTextBox tb)
        {
            SHTBLine newLine = new SHTBLine(tb.GenerateUniqueLineId());
            for(int i=pos;i<tb[iLine].Count;i++)
                newLine.Add(tb[iLine][i]);
            tb[iLine].RemoveRange(pos, tb[iLine].Count - pos);
            tb.InsertLine(iLine+1, newLine);
        }
    }

    /// <summary>
    /// Insert text
    /// </summary>
    internal class InsertTextCommand : UndoableCommand
    {
        string insertedText;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tb">Underlaying textbox</param>
        /// <param name="insertedText">Text for inserting</param>
        public InsertTextCommand(SyntaxHighlightingTextBox tb, string insertedText): base(tb)
        {
            this.insertedText = insertedText;
        }

        /// <summary>
        /// Undo operation
        /// </summary>
        public override void Undo()
        {
            associatedTextBox.Selection.Start = currentSelection.Start;
            associatedTextBox.Selection.End = lastSelection.Start;
            associatedTextBox.OnTextChanging();
            ClearSelectedCommand.ClearSelected(associatedTextBox);
            base.Undo();
        }

        /// <summary>
        /// Execute operation
        /// </summary>
        public override void Execute()
        {
            associatedTextBox.OnTextChanging(ref insertedText);
            InsertText(insertedText, associatedTextBox);
            base.Execute();
        }

        internal static void InsertText(string insertedText, SyntaxHighlightingTextBox tb)
        {
            try
            {
                tb.Selection.BeginUpdate();
                char cc = '\x0';
                if (tb.LinesCount == 0)
                    InsertCharCommand.InsertLine(tb);
                foreach (char c in insertedText)
                    InsertCharCommand.InsertChar(c, ref cc, tb);
                tb.needRecalc = true;
            }
            finally {
                tb.Selection.EndUpdate();
            }
        }
    }

    /// <summary>
    /// Insert text into given ranges
    /// </summary>
    /// <remarks>This opertaion is very heavy, because it save ALL text for undo.
    /// Use this class only for ReplaceAll command</remarks>
    internal class ReplaceTextCommand : UndoableCommand
    {
        string insertedText;
        List<SHTBRange> ranges;
        string prevText;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tb">Underlaying textbox</param>
        /// <param name="ranges">List of ranges for replace</param>
        /// <param name="insertedText">Text for inserting</param>
        public ReplaceTextCommand(SyntaxHighlightingTextBox tb, List<SHTBRange> ranges, string insertedText)
            : base(tb)
        {
            this.ranges = ranges;
            this.insertedText = insertedText;
            currentSelection = tb.Selection.Clone();
            currentSelection.SelectAll();
        }

        /// <summary>
        /// Undo operation
        /// </summary>
        public override void Undo()
        {
            associatedTextBox.Text = prevText;
        }

        /// <summary>
        /// Execute operation
        /// </summary>
        public override void Execute()
        {
            associatedTextBox.OnTextChanging(ref insertedText);

            this.prevText = associatedTextBox.Text;

            associatedTextBox.Selection.BeginUpdate();
            for (int i = ranges.Count - 1; i >= 0; i--)
            {
                associatedTextBox.Selection.Start = ranges[i].Start;
                associatedTextBox.Selection.End = ranges[i].End;
                ClearSelectedCommand.ClearSelected(associatedTextBox);
                InsertTextCommand.InsertText(insertedText, associatedTextBox);
            }
            associatedTextBox.Selection.SelectAll();
            associatedTextBox.Selection.EndUpdate();
            associatedTextBox.needRecalc = true;

            lastSelection = associatedTextBox.Selection.Clone();
            associatedTextBox.OnTextChanged(lastSelection.Start.LineNumber, lastSelection.End.LineNumber);
            //base.Execute();
        }
    }

    /// <summary>
    /// Clear selected text
    /// </summary>
    internal class ClearSelectedCommand : UndoableCommand
    {
        string deletedText;

        /// <summary>
        /// Construstor
        /// </summary>
        /// <param name="tb">Underlaying textbox</param>
        public ClearSelectedCommand(SyntaxHighlightingTextBox tb): base(tb)
        {
        }

        /// <summary>
        /// Undo operation
        /// </summary>
        public override void Undo()
        {
            associatedTextBox.Selection.Start = new SHTBPlace(currentSelection.FromCharPos, Math.Min(currentSelection.Start.LineNumber, currentSelection.End.LineNumber));
            associatedTextBox.OnTextChanging();
            InsertTextCommand.InsertText(deletedText, associatedTextBox);
            associatedTextBox.OnTextChanged(currentSelection.Start.LineNumber, currentSelection.End.LineNumber);
        }

        /// <summary>
        /// Execute operation
        /// </summary>
        public override void Execute()
        {
            associatedTextBox.OnTextChanging();
            deletedText = associatedTextBox.Selection.Text;
            ClearSelected(associatedTextBox);
            lastSelection = associatedTextBox.Selection.Clone();
            associatedTextBox.OnTextChanged(lastSelection.Start.LineNumber, lastSelection.Start.LineNumber);
        }

        internal static void ClearSelected(SyntaxHighlightingTextBox tb)
        {
            SHTBPlace start = tb.Selection.Start;
            SHTBPlace end = tb.Selection.End;
            int fromLine = Math.Min(end.LineNumber, start.LineNumber);
            int toLine = Math.Max(end.LineNumber, start.LineNumber);
            int fromChar = tb.Selection.FromCharPos;
            int toChar = tb.Selection.ToCharPos;
            if (fromLine < 0) return;
            //
            if (fromLine == toLine)
                tb[fromLine].RemoveRange(fromChar, toChar - fromChar);
            else
            {
                tb[fromLine].RemoveRange(fromChar, tb[fromLine].Count - fromChar);
                tb[toLine].RemoveRange(0, toChar);
                tb.RemoveLine(fromLine + 1, toLine - fromLine - 1);
                InsertCharCommand.MergeLines(fromLine, tb);
            }
            //
            tb.Selection.Start = new SHTBPlace(fromChar, fromLine);
            //
            tb.needRecalc = true;
        }
    }
}
