namespace GBRead.Base
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System;

    /// <summary>
    /// Contains methods for tokenizing input.
    /// </summary>
    public class Tokenizer
    {
        #region Private Members

        #region Regexes

        private static Regex equalRegex = new Regex(@"\.?(equ)|=", RegexOptions.IgnoreCase);
        private static Regex assignmentRegex = new Regex(@"^(\w+)\s+((equ)|=)\s+(.*)$", RegexOptions.IgnoreCase);
        private static Regex labelGlobalRegex = new Regex(@"^[\w][\w\d_]*:$");
        private static Regex labelExportRegex = new Regex(@"^[\w][\w\d_]*::$");
        private static Regex labelLocalRegex = new Regex(@"^\.[\w][\w\d_]*$");
        private static Regex refRegex = new Regex(@"^[\w][\w\d_]*$", RegexOptions.IgnoreCase);
        private static Regex numberRegex = new Regex(@"^\d+$");
        private static Regex hexDollarNumberRegex = new Regex(@"^\$[\da-f]+$", RegexOptions.IgnoreCase);
        private static Regex hexZeroXNumberRegex = new Regex(@"^0x[\da-f]+$", RegexOptions.IgnoreCase);
        private static Regex hexSuffixNumberRegex = new Regex(@"^[\da-f]+h$", RegexOptions.IgnoreCase);
        private static Regex binaryPercentNumberRegex = new Regex(@"^%[01]+$", RegexOptions.IgnoreCase);
        private static Regex binaryZeroBNumberRegex = new Regex(@"^0b[01]+$", RegexOptions.IgnoreCase);
        private static Regex binarySuffixNumberRegex = new Regex(@"^[01]+b$", RegexOptions.IgnoreCase);
        private static Regex octalZeroONumberRegex = new Regex(@"^0o[0-7]+$", RegexOptions.IgnoreCase);
        private static Regex hexRegex = new Regex(@"[a-f]", RegexOptions.IgnoreCase);

        #endregion Regexes

        #region Keywords

        private static string[] keywords = new string[]
        {
            "adc",
            "add",
            "and",
            "bit",
            "call",
            "ccf",
            "cb",
            "cp",
            "cpl",
            "db",
            "ddw",
            "dq",
            "dw",
            "ei",
            "daa",
            "dec",
            "di",
            "halt",
            "inc",
            "jp",
            "jr",
            "ld",
            "ldi",
            "ldd",
            "ldhl",
            "nop",
            "or",
            "pop",
            "push",
            "res",
            "ret",
            "reti",
            "rl",
            "rla",
            "rlc",
            "rlca",
            "rot",
            "rr",
            "rra",
            "rrc",
            "rrca",
            "rst",
            "sbc",
            "scf",
            "set",
            "sla",
            "sra",
            "srl",
            "stop",
            "sub",
            "swap",
            "xor"
        };

        private static Regex registerSingleRegex = new Regex(@"^(a|b|c|d|e|h|l)$",RegexOptions.IgnoreCase);
        private static Regex registerDoubleRegex = new Regex(@"^(af|bc|de|hl|sp)$", RegexOptions.IgnoreCase);
        private static Regex conditionRegex = new Regex(@"^(nc|nz|z)$", RegexOptions.IgnoreCase);

        #endregion Keywords

        #endregion Private Members

        public static List<Token> Tokenize(int lineNumber, string input)
        {
            var ret = new List<Token>();
            InputStack inputStack = new InputStack(lineNumber, input);
            while (inputStack.Count > 0)
            {
                Token currentToken = new Token() { Line = inputStack.LineNumber };

                // First, skip any spaces.
                while (inputStack.Peek() == ' ')
                {
                    inputStack.Pop();
                }

                // If EOL is reached, then continue/break.
                if (inputStack.Count == 0)
                {
                    continue;
                }

                currentToken.StartingIndex = inputStack.CharOffset;
                // Recognize the sequence based on the first letter.
                currentToken.Lexeme.Append(inputStack.Pop());

                if (char.IsLetter(currentToken.Lexeme[0]))
                {
                    //This is the start of either a keyword, a label, a special directive, or a reference.
                    while (inputStack.Count > 0 && (char.IsLetter(inputStack.Peek()) || char.IsNumber(inputStack.Peek()) || inputStack.Peek() == '_'))
                    {
                        currentToken.Lexeme.Append(inputStack.Pop());
                    }
                    while (inputStack.Peek() == ':')
                    {
                        currentToken.Lexeme.Append(inputStack.Pop());
                    }
                    if (labelGlobalRegex.IsMatch(currentToken.Lexeme.ToString()))
                    {
                        currentToken.Lexeme.Remove(currentToken.Lexeme.Length - 2, 1);
                        currentToken.Type = TokenType.GlobalLabel;
                    }
                    else if (labelExportRegex.IsMatch(currentToken.Lexeme.ToString()))
                    {
                        currentToken.Lexeme.Remove(currentToken.Lexeme.Length - 3, 2);
                        currentToken.Type = TokenType.ExportLabel;
                    }
                    else if (keywords.Contains(currentToken.Lexeme.ToString()))
                    {
                        currentToken.Type = (TokenType)Enum.Parse(typeof(TokenType), "Keyword_" + currentToken.Lexeme.ToString(), true);
                    }
                    else if (registerSingleRegex.IsMatch(currentToken.Lexeme.ToString()))
                    {
                        currentToken.Type = (TokenType)Enum.Parse(typeof(TokenType), "Reg_" + currentToken.Lexeme.ToString(), true);
                    }
                    else if (registerDoubleRegex.IsMatch(currentToken.Lexeme.ToString()))
                    {
                        currentToken.Type = (TokenType)Enum.Parse(typeof(TokenType), "RD_" + currentToken.Lexeme.ToString(), true);
                    }
                    else if (conditionRegex.IsMatch(currentToken.Lexeme.ToString()))
                    {
                        currentToken.Type = (TokenType)Enum.Parse(typeof(TokenType), "CC_" + currentToken.Lexeme.ToString(), true);
                    }
                    else if (currentToken.Lexeme.ToString().Equals("equ", StringComparison.InvariantCultureIgnoreCase))
                    {
                        currentToken.Type = TokenType.Equal;
                    }
                    else if (refRegex.IsMatch(currentToken.Lexeme.ToString()))
                    {
                        currentToken.Type = TokenType.Identifier;
                    }
                }
                else if (currentToken.Lexeme[0] == '.')
                {
                    // A 'dot type' label.
                    while (inputStack.Count != 0 && (char.IsLetter(inputStack.Peek()) || char.IsNumber(inputStack.Peek()) || inputStack.Peek() == '_'))
                    {
                        currentToken.Lexeme.Append(inputStack.Pop());
                    }
                    if (labelLocalRegex.IsMatch(currentToken.Lexeme.ToString()))
                    {
                        currentToken.Type = TokenType.LocalLabel;
                    }
                }
                else if (char.IsDigit(currentToken.Lexeme[0]))
                {
                    // A number, either regular or containing the binary, hex, or octal specifiers.
                    // 0x32F, 0b0100, 0011b, FFh, 43, 0o733 are all valid matches.
                    while (inputStack.Count != 0 && (char.IsDigit(inputStack.Peek()) || new char[] { 'h', 'b', 'o', 'x' }.Contains(inputStack.Peek())))
                    {
                        currentToken.Lexeme.Append(inputStack.Pop());
                    }
                    if (numberRegex.IsMatch(currentToken.Lexeme.ToString())
                        || hexSuffixNumberRegex.IsMatch(currentToken.Lexeme.ToString())
                        || binarySuffixNumberRegex.IsMatch(currentToken.Lexeme.ToString())
                        || hexZeroXNumberRegex.IsMatch(currentToken.Lexeme.ToString())
                        || binaryZeroBNumberRegex.IsMatch(currentToken.Lexeme.ToString())
                        || octalZeroONumberRegex.IsMatch(currentToken.Lexeme.ToString()))
                    {
                        currentToken.Type = TokenType.Number;
                    }
                }
                else if (currentToken.Lexeme[0] == '$')
                {
                    // Hex number of the form $[a-f]+.
                    while (inputStack.Count != 0 && (char.IsDigit(inputStack.Peek()) || new char[] { 'a', 'b', 'c', 'd', 'e', 'f' }.Contains(char.ToLower(inputStack.Peek()))))
                    {
                        currentToken.Lexeme.Append(inputStack.Pop());
                    }
                    currentToken.Type = TokenType.Number;
                }
                else if (currentToken.Lexeme[0] == '%')
                {
                    // Binary number of the form %[01]+.
                    while (inputStack.Count != 0 && new char[] { '0', '1' }.Contains(inputStack.Peek()))
                    {
                        currentToken.Lexeme.Append(inputStack.Pop());
                    }
                    currentToken.Type = TokenType.Number;
                }
                else if (currentToken.Lexeme[0] == '&')
                {
                    // Octal number of the form &[0-7]+.
                    while (inputStack.Count != 0 && new char[] { '0', '1', '2', '3', '4', '5', '6', '7' }.Contains(inputStack.Peek()))
                    {
                        currentToken.Lexeme.Append(inputStack.Pop());
                    }
                    currentToken.Type = TokenType.Number;
                }
                else if (currentToken.Lexeme[0] == '[')
                {
                    currentToken.Type = TokenType.MemMapStart;
                }
                else if (currentToken.Lexeme[0] == ']')
                {
                    currentToken.Type = TokenType.MemMapEnd;
                }
                else if (currentToken.Lexeme[0] == ',')
                {
                    currentToken.Type = TokenType.Comma;
                }
                else if (currentToken.Lexeme[0] == '#' || currentToken.Lexeme[0] == ';')
                {
                    while(inputStack.Count != 0 && inputStack.Peek() != '\n')
                    {
                        currentToken.Lexeme.Append(inputStack.Pop());
                    }
                    currentToken.Type = TokenType.Comment;
                }
                else if (currentToken.Lexeme[0] == '\n')
                {
                    currentToken.Type = TokenType.EndOfLine;
                }
                else if (currentToken.Lexeme[0] == '+')
                {
                    currentToken.Type = TokenType.Operator_Plus;
                }
                else if (currentToken.Lexeme[0] == '-')
                {
                    currentToken.Type = TokenType.Operator_Minus;
                }
                else if (currentToken.Lexeme[0] == '*')
                {
                    currentToken.Type = TokenType.Operator_Mult;
                }
                else if (currentToken.Lexeme[0] == '/')
                {
                    if (inputStack.Peek() == '/')
                    {
                        while (inputStack.Count != 0 && inputStack.Peek() != '\n')
                        {
                            currentToken.Lexeme.Append(inputStack.Pop());
                        }
                        currentToken.Type = TokenType.Comment;
                    }
                    else
                    {
                        currentToken.Type = TokenType.Operator_Div;
                    }
                }
                else if (currentToken.Lexeme[0] == '=')
                {
                    currentToken.Type = TokenType.Equal;
                }
                else if (currentToken.Lexeme[0] == '\\')
                {
                    if (inputStack.Peek() != '\0' && inputStack.Peek() != '\n')
                    {
                        currentToken.Lexeme.Append(inputStack.Pop());
                        currentToken.Type = TokenType.CharacterEscape;
                    }
                }
                if (currentToken.Type != TokenType.Comment) ret.Add(currentToken);
            }
            return ret;
        }

        private class InputStack
        {
            StringBuilder sb = new StringBuilder();
            int curInd = 0;
            int baseLineNumber = 0;
            int lineNumberOffset = 0;
            int charOffset;

            public int LineNumber { get { return baseLineNumber + lineNumberOffset; } }

            public int CharOffset { get { return charOffset; } }

            public int Count { get { return sb.Length - curInd; } }

            public InputStack(int lineNumber, string input)
            {
                sb.Append(input);
                curInd = 0;
                baseLineNumber = lineNumber;
            }

            public char Pop()
            {
                if (curInd == sb.Length)
                {
                    return '\0';
                }
                while (sb[curInd] == '\r')
                {
                    curInd++;
                    charOffset++;
                }
                if (curInd == sb.Length)
                {
                    return '\0';
                }
                if (sb[curInd] == '\n')
                {
                    lineNumberOffset++;
                    charOffset = 0;
                }
                else
                {
                    charOffset++;
                }
                return sb[curInd++];
            }

            public char Peek()
            {
                if (curInd == sb.Length)
                {
                    return '\0';
                }
                int tempInd = curInd;
                while (sb[tempInd] == '\r')
                {
                    tempInd++;
                }
                return sb[tempInd];
            }
        }
    }
}