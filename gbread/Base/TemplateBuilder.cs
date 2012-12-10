namespace GBRead.Base
{
    using System.Collections.Generic;
    using System.Text;
    using Antlr.Runtime;
    using Antlr.Runtime.Tree;

    internal class TemplateBuilder
    {
        private CompError currentError;
        private StringBuilder sb = new StringBuilder();

        private void Initialize()
        {
            sb.Clear();
        }

        public static List<FormatInfo> GetTemplateInfo(string template)
        {
            var tempDict = new List<FormatInfo>();
            var templateIndex = 0;
            if (char.IsDigit(template[templateIndex]))
            {
                var fInfo = new FormatInfo() {
                    FormatType = 'b'
                };
                var ctBuf = "";
                while (templateIndex < template.Length && char.IsDigit(template[templateIndex]))
                {
                    ctBuf += template[templateIndex++];
                }
                fInfo.ArrLenArg = int.Parse(ctBuf);
                tempDict.Add(fInfo);
            }
            else
            {
                while (templateIndex < template.Length)
                {
                    var ctBuf = "";
                    var fInfo = new FormatInfo() {
                        FormatType = template[templateIndex++]
                    };
                    while (templateIndex < template.Length)
                    {
                        if (char.IsDigit(template[templateIndex]))
                        {
                            ctBuf = "";
                            while (templateIndex < template.Length && char.IsDigit(template[templateIndex]))
                            {
                                ctBuf += template[templateIndex++];
                            }
                            fInfo.ArrLenArg = int.Parse(ctBuf);
                        }
                        else if (template[templateIndex] == '!')
                        {
                            ctBuf = "";
                            templateIndex++;
                            while (templateIndex < template.Length && char.IsDigit(template[templateIndex]))
                            {
                                ctBuf += template[templateIndex++];
                            }
                            fInfo.FormatArgs.Add(int.Parse(ctBuf));
                        }
                        else
                        {
                            break;
                        }
                    }
                    tempDict.Add(fInfo);
                }
            }
            return tempDict;
        }

        public static string TemplateToString(string template)
        {
            var sb = new StringBuilder();
            foreach (var fInfo in GetTemplateInfo(template))
            {
                switch (fInfo.FormatType)
                {
                    case 'b':
                        sb.Append("byte");
                        break;

                    case 'w':
                        sb.Append("word");
                        break;

                    case 'd':
                        sb.Append("dword");
                        break;

                    case 'q':
                        sb.Append("qword");
                        break;

                    case 's':
                        sb.Append("string");
                        break;
                }
                if (fInfo.FormatArgs.Count != 0)
                {
                    sb.AppendFormat("({0})", fInfo.FormatArgs[0]);
                }
                if (fInfo.ArrLenArg > 1)
                {
                    sb.AppendFormat("[{0}]", fInfo.ArrLenArg);
                }
                sb.AppendLine(";");
            }
            return sb.ToString();
        }

        public string ValidateTemplate(string input, ref CompError error, out bool success)
        {
            Initialize();
            success = false;
            error = new CompError();

            var syntaxTree = new CommonTree();
            if (!CreateAST(input, out syntaxTree))
            {
                error = currentError;
                return "";
            }

            if (!EvaluateAST(syntaxTree))
            {
                error = currentError;
                return "";
            }
            success = true;
            return sb.ToString();
        }

        private bool CreateAST(string input, out CommonTree syntaxTree)
        {
            var css = new CaseInsensitiveStringStream(input);
            var gblex = new DataTemplateLexer(css);
            var cts = new CommonTokenStream(gblex);
            var gbparse = new DataTemplateParser(cts);
            gbparse.TreeAdaptor = new CommonTreeAdaptor();
            syntaxTree = gbparse.data_temp().Tree;
            var parseErrors = gblex.GetErrors();
            parseErrors.AddRange(gbparse.GetErrors());
            if (parseErrors.Count != 0)
            {
                MakeErrorMessage(parseErrors[0]);
                return false;
            }
            return true;
        }

        private bool EvaluateAST(ITree eval)
        {
            for (int i = 0; i < eval.ChildCount; i++)
            {
                var bTree = eval.GetChild(i);
                switch (bTree.Text.ToLower())
                {
                    case "byte":
                        {
                            sb.Append("b");
                        }
                        break;

                    case "word":
                        {
                            sb.Append("w");
                        }
                        break;

                    case "dword":
                        {
                            sb.Append("d");
                        }
                        break;

                    case "qword":
                        {
                            sb.Append("q");
                        }
                        break;

                    case "string":
                        {
                            sb.Append("s");
                        }
                        break;
                }
                for (int x = 0; x < bTree.ChildCount; x++)
                {
                    var result = 0L;
                    if (!EvaluateExpression(bTree.GetChild(x).GetChild(0).GetChild(0), out result))
                    {
                        return false;
                    }
                    if (result == 0)
                    {
                        continue;
                    }
                    else
                    {
                        switch (bTree.GetChild(x).Text)
                        {
                            case "ARG":
                                {
                                    sb.Append("!" + result.ToString());
                                }
                                break;

                            case "ARRLEN":
                                {
                                    sb.Append(result.ToString());
                                }
                                break;
                        }
                    }
                }
            }
            return true;
        }

        private bool EvaluateExpression(ITree eval, out long result)
        {
            result = 0;
            var res1 = 0L;
            var res2 = 0L;
            var res3 = 0L;
            switch (eval.Text)
            {
                case "LITERAL":
                    return EvaluateLiteral(eval.GetChild(0), out result);
                default:
                    {
                        switch (eval.ChildCount)
                        {
                            case 1:
                                {
                                    if (!EvaluateExpression(eval.GetChild(0), out res1))
                                    {
                                        return false;
                                    }
                                    switch (eval.Text)
                                    {
                                        case "~":
                                            result = ~res1;
                                            return true;
                                        case "-":
                                            result = -res1;
                                            return true;
                                        case "!":
                                            result = (res1 == 0) ? 1 : 0;
                                            return true;
                                        default:
                                            {
                                                MakeErrorMessage(eval, ErrorMessage.Build_UnknownArgument);
                                                return false;
                                            }
                                    }
                                }
                            case 2:
                                {
                                    if (!(EvaluateExpression(eval.GetChild(0), out res1)
                                        && EvaluateExpression(eval.GetChild(1), out res2)))
                                    {
                                        return false;
                                    }
                                    switch (eval.Text)
                                    {
                                        case "+":
                                            result = res1 + res2;
                                            return true;
                                        case "-":
                                            result = res1 - res2;
                                            return true;
                                        case "*":
                                            result = res1 * res2;
                                            return true;
                                        case "/":
                                            result = res1 / res2;
                                            return true;
                                        case "%":
                                            result = res1 % res2;
                                            return true;
                                        case "<<":
                                            result = res1 << (int)res2; // Right side has to be an int.
                                            return true;
                                        case ">>":
                                            result = res1 >> (int)res2; // Right side has to be an int.
                                            return true;
                                        case "<":
                                            result = res1 < res2 ? 1 : 0;
                                            return true;
                                        case ">":
                                            result = res1 > res2 ? 1 : 0;
                                            return true;
                                        case "<=":
                                            result = res1 <= res2 ? 1 : 0;
                                            return true;
                                        case ">=":
                                            result = res1 >= res2 ? 1 : 0;
                                            return true;
                                        case "==":
                                            result = res1 == res2 ? 1 : 0;
                                            return true;
                                        case "&":
                                            result = res1 & res2;
                                            return true;
                                        case "^":
                                            result = res1 ^ res2;
                                            return true;
                                        case "|":
                                            result = res1 | res2;
                                            return true;
                                        case "&&":
                                            result = (res1 != 0) && (res2 != 0) ? 1 : 0;
                                            return true;
                                        case "||":
                                            result = (res1 != 0) || (res2 != 0) ? 1 : 0;
                                            return true;
                                        default:
                                            {
                                                MakeErrorMessage(eval, ErrorMessage.Build_UnknownArgument);
                                                return false;
                                            }
                                    }
                                }
                            case 3:
                                {
                                    if (!(EvaluateExpression(eval.GetChild(0), out res1)
                                        && EvaluateExpression(eval.GetChild(1), out res2)
                                        && EvaluateExpression(eval.GetChild(2), out res3)))
                                    {
                                        return false;
                                    }
                                    switch (eval.Text)
                                    {
                                        case "?":
                                            result = (res1 != 0) ? res2 : res3;
                                            return true;
                                        default:
                                            {
                                                MakeErrorMessage(eval, ErrorMessage.Build_UnknownArgument);
                                                return false;
                                            }
                                    }
                                }
                            default:
                                {
                                    MakeErrorMessage(eval, ErrorMessage.Build_UnknownArgument);
                                    return false;
                                }
                        }
                    }
            }
        }

        private bool EvaluateLiteral(ITree eval, out long result)
        {
            result = 0;
            if (!Utility.StringToLong(eval.Text, out result))
            {
                MakeErrorMessage(eval, ErrorMessage.Build_NumberOverflow);
                return false;
            }
            return true;
        }

        private void MakeErrorMessage(ITree arg, ErrorMessage messageType)
        {
            currentError.lineNumber = arg.Line;
            currentError.characterNumber = arg.CharPositionInLine;
            currentError.errorMessage = messageType;
            currentError.extraInfo1 = arg.Text;
        }

        private void MakeErrorMessage(ErrInfo arg)
        {
            currentError.lineNumber = arg.error.Line;
            currentError.characterNumber = arg.error.CharPositionInLine;
            currentError.errorMessage = ErrorMessage.General_CustomError;
            currentError.extraInfo1 = arg.errText;
        }
    }

    public class FormatInfo
    {
        public char FormatType;
        public int ArrLenArg;
        public List<int> FormatArgs;

        public FormatInfo()
        {
            FormatType = 'b';
            ArrLenArg = 1;
            FormatArgs = new List<int>();
        }
    }
}