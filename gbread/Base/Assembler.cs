using Antlr.Runtime;
using Antlr.Runtime.Tree;
using System.Collections.Generic;
using GBRead.Base.Annotation;

namespace GBRead.Base
{
    public class Assembler
    {
        private Dictionary<string, long> variableDict = new Dictionary<string, long>();
        private Dictionary<string, long> callDict = new Dictionary<string, long>();
        private Dictionary<string, ITree> macroDict = new Dictionary<string, ITree>();
        private List<SymEntry> symFillTable = new List<SymEntry>();
        private CodeGenerator codeGen = new CodeGenerator();
        private Stack<List<long>> macroArgStack = new Stack<List<long>>();
        private CompError currentError = new CompError();

        public string GameboyFormatChars
        {
            get;
            set;
        }

        private const string ExpressionToken = "EXPRESSION";
        private const string HLRefToken = "RR_REF_HL";
        private const string MemRefToken = "MEM_REF";
        private const string AssignmentToken = "ASSIGNMENT";
        private const string MacroDefToken = "MACRO";
        private const string MacroArgToken = "MACRO_ARG";
        private const string VarToken = "VAR";
        private const string LiteralToken = "LITERAL";
        private const string InstructionToken = "INSTRUCTION";
        private const string PseudoInstToken = "PSEUDO_INST";
        private const string DataDefToken = "DATA_DEF";
        private const string IncludeToken = "INCLUDE";
        private const string MacroCallToken = "MACRO_CALL";
        private const string StatementToken = "STATEMENT";
        private const string ExportLabelToken = "EXPORT_LABEL";
        private const string GlobalLabelToken = "GLOBAL_LABEL";
        private const string LocalLabelToken = "LOCAL_LABEL";
        private const string GBFormatStringToken = "GB_NUMBER";

        private string GlobalScopeName = "_";
        private string LocalScopeName = "_";

        private LabelContainer lc;

        private struct SymEntry
        {
            public int line;
            public int charpos;
            public long instructionPosition;
            public long offsetToFill;
            public string label;
            public string fullyQualifiedLabel;
            public bool isJR;
        }

        public Assembler(LabelContainer newlc)
        {
            lc = newlc;
            GameboyFormatChars = "0123";
        }

        public void GetOptions(Options options)
        {
            GameboyFormatChars = options.Assembler_GameboyFormatChars;
        }

        public void SetOptions(Options options)
        {
            options.Assembler_GameboyFormatChars = GameboyFormatChars;
        }

        private void Initialize()
        {
            codeGen.ClearStream();
            variableDict.Clear();
            callDict.Clear();
            symFillTable.Clear();
            macroDict.Clear();
            macroArgStack.Clear();
            currentError = new CompError();
            GlobalScopeName = "_";
            LocalScopeName = "_";
            foreach (FunctionLabel kvp in lc.FuncList)
            {
                callDict.Add(GetGlobalScopedID(kvp.Name), Utility.GetPCAddress(kvp.Value));
            }

            foreach (DataLabel kvp in lc.DataList)
            {
                callDict.Add(GetGlobalScopedID(kvp.Name), Utility.GetPCAddress(kvp.Value));
            }

            foreach (VarLabel kvp in lc.VarList)
            {
                variableDict.Add(GetGlobalScopedID(kvp.Name), kvp.Value);
            }
        }

        public byte[] AssembleASM(int baseOffset, string input, ref CompError error, out bool success)
        {
            success = false;
            error = new CompError();
            int currentOffset = baseOffset;
            int currentPreOffset = baseOffset;
            Initialize();

            var syntaxTree = new CommonTree();
            if (!CreateAST(input, out syntaxTree))
            {
                error = currentError;
                return new byte[1];
            }

            if (!(EvaluateAST(syntaxTree, baseOffset)
                && EvaluateSymbols(baseOffset)))
            {
                error = currentError;
                return new byte[1];
            }
            success = true;
            return codeGen.StreamToArray();
        }

        private bool CreateAST(string input, out CommonTree syntaxTree)
        {
            var css = new CaseInsensitiveStringStream(input);
            var gblex = new GBXLexer(css);
            var cts = new CommonTokenStream(gblex);
            var gbparse = new GBXParser(cts);
            gbparse.TreeAdaptor = new CommonTreeAdaptor();
            syntaxTree = gbparse.program().Tree;
            var parseErrors = gblex.GetErrors();
            parseErrors.AddRange(gbparse.GetErrors());
            if (parseErrors.Count != 0)
            {
                MakeErrorMessage(parseErrors[0]);
                return false;
            }
            return true;
        }

        // TODO: Allow for building from multiple sources("include").
        // TODO: Add "section"
        private bool EvaluateAST(ITree eval, int baseOffset)
        {
            for (int i = 0; i < eval.ChildCount; i++)
            {
                switch (eval.GetChild(i).Text)
                {
                    case AssignmentToken:
                        {
                            if (!EvaluateAssignment(eval.GetChild(i).GetChild(0), baseOffset))
                            {
                                return false;
                            }
                        }
                        break;

                    case MacroDefToken:
                        {
                            if (!EvaluateMacroDef(eval.GetChild(i).GetChild(0)))
                            {
                                return false;
                            }
                        }
                        break;

                    case StatementToken:
                        {
                            if (!EvaluateStatement(eval.GetChild(i).GetChild(0), baseOffset))
                            {
                                return false;
                            }
                        }
                        break;
                }
            }
            return true;
        }

        private bool EvaluateSymbols(int baseOffset)
        {
            foreach (SymEntry se in symFillTable)
            {
                if (callDict.ContainsKey(se.fullyQualifiedLabel))
                {
                    codeGen.Seek(se.offsetToFill);
                    var memLoc = callDict[se.fullyQualifiedLabel];
                    if (se.isJR)
                    {
                        long diff = memLoc - (se.instructionPosition + 2);
                        if (diff < -128 || diff > 127)
                        {
                            MakeErrorMessage(se, ErrorMessage.Build_JROutOfRange);
                            return false;
                        }
                        codeGen.EmitByte(diff);
                    }
                    else
                    {
                        codeGen.EmitWord(memLoc);
                    }
                }
                else
                {
                    MakeErrorMessage(se, ErrorMessage.Build_UnknownLabel);
                    return false;
                }
            }
            return true;
        }

        private bool EvaluateAssignment(ITree eval, int baseOffset)
        {
            var varName = GetGlobalScopedID(eval.Text);
            if (variableDict.ContainsKey(varName))
            {
                MakeErrorMessage(eval, ErrorMessage.Label_VariableAlreadyDefined);
                return false;
            }
            var result = 0L;
            if (!EvaluateExpression(eval.GetChild(0), out result))
            {
                return false;
            }
            variableDict.Add(varName, result);
            return true;
        }

        private bool EvaluateMacroDef(ITree eval)
        {
            var macroName = GetGlobalScopedID(eval.Text);
            if (macroDict.ContainsKey(macroName))
            {
                MakeErrorMessage(eval, ErrorMessage.Build_MacroAlreadyDefined);
                return false;
            }
            macroDict.Add(macroName, eval);
            return true;
        }

        private bool EvaluateStatement(ITree eval, int baseOffset)
        {
            var labelDeclTree = eval.GetChild(0);
            var statementTree = eval.GetChild(1);
            for (int i = 0; i < labelDeclTree.ChildCount; i++)
            {
                if (!EvaluateLabelDecl(labelDeclTree.GetChild(i), baseOffset))
                {
                    return false;
                }
            }
            switch (statementTree.Text)
            {
                case InstructionToken:
                    {
                        return EvaluateInstruction(statementTree.GetChild(0), baseOffset);
                    }
                case PseudoInstToken:
                    {
                        return EvaluatePseudoInst(statementTree.GetChild(0), baseOffset);
                    }
                default:
                    {
                        MakeErrorMessage(eval, ErrorMessage.Build_UnknownError);
                        return false;
                    }
            }
        }

        private bool EvaluateLabelDecl(ITree eval, int baseOffset)
        {
            var labelName = eval.GetChild(0).Text;
            switch (eval.Text)
            {
                case ExportLabelToken:
                    {
                        var exportScopedName = GetExportScopedID(labelName);
                        labelName = GetGlobalScopedID(labelName);
                        if (callDict.ContainsKey(labelName))
                        {
                            MakeErrorMessage(eval.GetChild(0), ErrorMessage.Label_LabelAlreadyDefined);
                            return false;
                        }
                        callDict.Add(exportScopedName, codeGen.Position + baseOffset);
                        callDict.Add(labelName, codeGen.Position + baseOffset);
                    }
                    break;

                case GlobalLabelToken:
                    {
                        SetLocalScope(labelName);
                        labelName = GetGlobalScopedID(labelName);
                        if (callDict.ContainsKey(labelName))
                        {
                            MakeErrorMessage(eval.GetChild(0), ErrorMessage.Label_LabelAlreadyDefined);
                            return false;
                        }
                        callDict.Add(labelName, codeGen.Position + baseOffset);
                    }
                    break;

                case LocalLabelToken:
                    {
                        labelName = GetLocalScopedID(labelName);
                        if (callDict.ContainsKey(labelName))
                        {
                            MakeErrorMessage(eval.GetChild(0), ErrorMessage.Label_LabelAlreadyDefined);
                            return false;
                        }
                        callDict.Add(labelName, codeGen.Position + baseOffset);
                    }
                    break;

                default:
                    {
                        MakeErrorMessage(eval, ErrorMessage.Build_UnknownError);
                        return false;
                    }
            }
            return true;
        }

        private bool EvaluateInstruction(ITree eval, int baseOffset)
        {
            switch (eval.Text)
            {
                case "adc":
                    {
                        var arg = eval.ChildCount == 1 ? eval.GetChild(0) : eval.GetChild(1);
                        if (arg.Text == ExpressionToken)
                        {
                            if (!EvalArithArgFunc(codeGen.EmitAdcN, arg))
                            {
                                return false;
                            }
                        }
                        else
                        {
                            codeGen.EmitAdcR(arg.Text);
                        }
                    }
                    break;

                case "add":
                    {
                        if (eval.ChildCount == 1)
                        {
                            var arg = eval.GetChild(0);
                            if (arg.Text == ExpressionToken)
                            {
                                if (!EvalArithArgFunc(codeGen.EmitAddN, arg))
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                codeGen.EmitAddR(arg.Text);
                            }
                        }
                        else
                        {
                            string arg1 = eval.GetChild(0).Text;
                            var arg2 = eval.GetChild(1);
                            if (arg1 == "a")
                            {
                                if (arg2.Text == ExpressionToken)
                                {
                                    if (!EvalArithArgFunc(codeGen.EmitAddN, arg2))
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    codeGen.EmitAddR(arg2.Text);
                                }
                            }
                            else if (arg1 == "hl")
                            {
                                codeGen.EmitAddRR(arg2.Text);
                            }
                            else
                            {
                                if (!EvalArithArgFunc(codeGen.EmitAddSPN, arg2))
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    break;

                case "and":
                    {
                        var arg = eval.ChildCount == 1 ? eval.GetChild(0) : eval.GetChild(1);
                        if (arg.Text == ExpressionToken)
                        {
                            if (!EvalArithArgFunc(codeGen.EmitAndN, arg))
                            {
                                return false;
                            }
                        }
                        else
                        {
                            codeGen.EmitAndR(arg.Text);
                        }
                    }
                    break;

                case "bit":
                    {
                        var arg = eval.GetChild(0);
                        var arg2 = eval.GetChild(1).Text;
                        if (!EvalBitFunc(codeGen.EmitBitXR, arg, arg2))
                        {
                            return false;
                        }
                    }
                    break;

                case "call":
                    {
                        var memLoc = 0L;
                        if (eval.ChildCount == 1)
                        {
                            var callName = GetGlobalScopedID(eval.GetChild(0).Text);
                            if (callDict.ContainsKey(callName))
                            {
                                memLoc = callDict[callName];
                            }
                            else
                            {
                                AddSymEntry(eval.Line, eval.CharPositionInLine, eval.GetChild(1).Text, callName, false, codeGen.Position, codeGen.Position + 1);
                            }
                            codeGen.EmitCallN(memLoc);
                        }
                        else
                        {
                            var callName = GetGlobalScopedID(eval.GetChild(1).Text);
                            if (callDict.ContainsKey(callName))
                            {
                                memLoc = callDict[callName];
                            }
                            else
                            {
                                AddSymEntry(eval.Line, eval.CharPositionInLine, eval.GetChild(1).Text, callName, false, codeGen.Position, codeGen.Position + 1);
                            }
                            codeGen.EmitCallCCN(eval.GetChild(0).Text, memLoc);
                        }
                    }
                    break;

                case "ccf":
                    codeGen.EmitCCF();
                    break;

                case "cp":
                    {
                        var arg = eval.ChildCount == 1 ? eval.GetChild(0) : eval.GetChild(1);
                        if (arg.Text == ExpressionToken)
                        {
                            if (!EvalArithArgFunc(codeGen.EmitCpN, arg))
                            {
                                return false;
                            }
                        }
                        else
                        {
                            codeGen.EmitCpR(arg.Text);
                        }
                    }
                    break;

                case "cpl":
                    codeGen.EmitCPL();
                    break;

                case "daa":
                    codeGen.EmitDAA();
                    break;

                case "dec":
                    {
                        switch (eval.GetChild(0).Text)
                        {
                            case "bc":
                            case "de":
                            case "hl":
                            case "sp":
                                {
                                    codeGen.EmitDecRR(eval.GetChild(0).Text);
                                }
                                break;

                            case "a":
                            case "b":
                            case "c":
                            case "d":
                            case "e":
                            case "h":
                            case "l":
                            case "RR_REF_HL":
                                {
                                    codeGen.EmitDecR(eval.GetChild(0).Text);
                                }
                                break;

                            default:
                                {
                                    MakeErrorMessage(eval.GetChild(0), ErrorMessage.Build_UnknownArgument);
                                    return false;
                                }
                        }
                    }
                    break;

                case "di":
                    codeGen.EmitDI();
                    break;

                case "ei":
                    codeGen.EmitEI();
                    break;

                case "halt":
                    codeGen.EmitHalt();
                    break;

                case "inc":
                    {
                        switch (eval.GetChild(0).Text)
                        {
                            case "bc":
                            case "de":
                            case "hl":
                            case "sp":
                                {
                                    codeGen.EmitIncRR(eval.GetChild(0).Text);
                                }
                                break;

                            case "a":
                            case "b":
                            case "c":
                            case "d":
                            case "e":
                            case "h":
                            case "l":
                            case "RR_REF_HL":
                                {
                                    codeGen.EmitIncR(eval.GetChild(0).Text);
                                }
                                break;

                            default:
                                {
                                    MakeErrorMessage(eval.GetChild(0), ErrorMessage.Build_UnknownArgument);
                                    return false;
                                }
                        }
                    }
                    break;

                case "jp":
                    if (eval.ChildCount == 1)
                    {
                        if (eval.GetChild(0).Text == "RR_HL")
                        {
                            codeGen.EmitJpHL();
                        }
                        else
                        {
                            var memLoc = 0L;
                            var jumpName = eval.GetChild(0).GetChild(0).Text;
                            jumpName = eval.GetChild(0).Text == GlobalLabelToken ?
                                GetGlobalScopedID(jumpName) :
                                GetLocalScopedID(jumpName);
                            if (callDict.ContainsKey(jumpName))
                            {
                                memLoc = callDict[jumpName];
                            }
                            else
                            {
                                AddSymEntry(eval.Line, eval.CharPositionInLine, eval.GetChild(0).GetChild(0).Text, jumpName, false, codeGen.Position, codeGen.Position + 1);
                            }
                            codeGen.EmitJpN(memLoc);
                        }
                    }
                    else
                    {
                        var memLoc = 0L;
                        var jumpName = eval.GetChild(1).GetChild(0).Text;
                        jumpName = eval.GetChild(1).Text == GlobalLabelToken ?
                            GetGlobalScopedID(jumpName) :
                            GetLocalScopedID(jumpName);
                        if (callDict.ContainsKey(eval.GetChild(1).Text))
                        {
                            memLoc = callDict[jumpName];
                        }
                        else
                        {
                            AddSymEntry(eval.Line, eval.CharPositionInLine, eval.GetChild(1).GetChild(0).Text, jumpName, false, codeGen.Position, codeGen.Position + 1);
                        }
                        codeGen.EmitJpCCN(eval.GetChild(0).Text, memLoc);
                    }
                    break;

                case "jr":
                    {
                        int sel = eval.ChildCount - 1;
                        var arg = eval.GetChild(sel);
                        long diff = 0;
                        var jumpName = arg.Text == GlobalLabelToken ?
                            GetGlobalScopedID(arg.GetChild(0).Text) :
                            GetLocalScopedID(arg.GetChild(0).Text);
                        if (callDict.ContainsKey(jumpName))
                        {
                            var memLoc = callDict[jumpName];
                            diff = memLoc - (codeGen.Position + 2);
                            if (diff < -128 || diff > 127)
                            {
                                MakeErrorMessage(arg, ErrorMessage.Build_JROutOfRange);
                                return false;
                            }
                        }
                        else
                        {
                            AddSymEntry(arg.Line, arg.CharPositionInLine, arg.GetChild(0).Text, jumpName, true, codeGen.Position, codeGen.Position + 1);
                        }

                        if (sel == 0)
                        {
                            codeGen.EmitJr(diff);
                        }
                        else
                        {
                            codeGen.EmitJrCCN(eval.GetChild(0).Text, diff);
                        }
                    }

                    break;

                case "ldhl":
                    {
                        if (!EvalArithArgFunc(codeGen.EmitLdHLSP, eval.GetChild(1)))
                        {
                            return false;
                        }
                    }
                    break;

                case "ldio":
                    {
                        if (eval.GetChild(0).Text == MemRefToken)
                        {
                            if (!EvalArithArgFunc(codeGen.EmitLdioNA, eval.GetChild(0).GetChild(0)))
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (!EvalArithArgFunc(codeGen.EmitLdioAN, eval.GetChild(1).GetChild(0)))
                            {
                                return false;
                            }
                        }
                    }
                    break;

                case "ldi":
                    {
                        if (eval.GetChild(0).Text == "a")
                        {
                            codeGen.EmitLdiAHL();
                        }
                        else
                        {
                            codeGen.EmitLdiHLA();
                        }
                    }
                    break;

                case "ldd":
                    {
                        if (eval.GetChild(0).Text == "a")
                        {
                            codeGen.EmitLddAHL();
                        }
                        else
                        {
                            codeGen.EmitLddHLA();
                        }
                    }
                    break;

                case "ld":
                    {
                        var arg1 = eval.GetChild(0);
                        var arg2 = eval.GetChild(1);
                        switch (arg1.Text)
                        {
                            case "a":
                                {
                                    if (arg2.Text == ExpressionToken)
                                    {
                                        var result = 0L;
                                        if (!EvaluateExpression(arg2.GetChild(0), out result))
                                        {
                                            return false;
                                        }
                                        codeGen.EmitLdRN("a", result);
                                    }
                                    else if (arg2.Text == MemRefToken)
                                    {
                                        var result = 0L;
                                        if (!EvaluateExpression(arg2.GetChild(0), out result))
                                        {
                                            return false;
                                        }
                                        codeGen.EmitLdANRef(result);
                                    }
                                    else
                                    {
                                        codeGen.EmitLdRegReg("a", arg2.Text);
                                    }
                                }
                                break;

                            case "b":
                            case "c":
                            case "d":
                            case "e":
                            case "h":
                            case "l":
                                {
                                    if (arg2.Text == ExpressionToken)
                                    {
                                        var result = 0L;
                                        if (!EvaluateExpression(arg2.GetChild(0), out result))
                                        {
                                            return false;
                                        }
                                        codeGen.EmitLdRN(arg1.Text, result);
                                    }
                                    else
                                    {
                                        codeGen.EmitLdRegReg(arg1.Text, arg2.Text);
                                    }
                                }
                                break;

                            case "bc":
                            case "de":
                            case "hl":
                                {
                                    var result = 0L;
                                    if (!EvaluateExpression(arg2.GetChild(0), out result))
                                    {
                                        return false;
                                    }
                                    codeGen.EmitLdRRN(arg1.Text, result);
                                }
                                break;

                            case "RR_REF_C":
                                {
                                    codeGen.EmitLdCRefA();
                                }
                                break;

                            case "RR_REF_BC":
                                {
                                    codeGen.EmitLdBCRefA();
                                }
                                break;

                            case "RR_REF_DE":
                                {
                                    codeGen.EmitLdDERefA();
                                }
                                break;

                            case "RR_REF_HL":
                                {
                                    if (arg2.Text == ExpressionToken)
                                    {
                                        var result = 0L;
                                        if (!EvaluateExpression(arg2.GetChild(0), out result))
                                        {
                                            return false;
                                        }
                                        codeGen.EmitLdRN(HLRefToken, result);
                                    }
                                    else
                                    {
                                        codeGen.EmitLdRegReg(HLRefToken, arg2.Text);
                                    }
                                }
                                break;

                            case "MEM_REF":
                                {
                                    var result = 0L;
                                    if (!EvaluateExpression(arg1.GetChild(0), out result))
                                    {
                                        return false;
                                    }
                                    if (arg2.Text == "a")
                                    {
                                        codeGen.EmitLdNRefA(result);
                                    }
                                    else
                                    {
                                        codeGen.EmitLdNRefSP(result);
                                    }
                                }
                                break;

                            case "sp":
                                {
                                    if (arg2.Text == ExpressionToken)
                                    {
                                        var result = 0L;
                                        if (!(EvaluateExpression(arg2.GetChild(0), out result)))
                                        {
                                            return false;
                                        }
                                        codeGen.EmitLdRRN(arg1.Text, result);
                                    }
                                    else
                                    {
                                        codeGen.EmitLdSPHL();
                                    }
                                }
                                break;

                            default:
                                break;
                        }
                    }
                    break;

                case "nop":
                    codeGen.EmitNop();
                    break;

                case "or":
                    {
                        var arg = eval.ChildCount == 1 ? eval.GetChild(0) : eval.GetChild(1);
                        if (arg.Text == ExpressionToken)
                        {
                            if (!EvalArithArgFunc(codeGen.EmitOrN, arg))
                            {
                                return false;
                            }
                        }
                        else
                        {
                            codeGen.EmitOrR(arg.Text);
                        }
                    }
                    break;

                case "pop":
                    {
                        codeGen.EmitPopRR(eval.GetChild(0).Text);
                    }
                    break;

                case "push":
                    {
                        codeGen.EmitPushRR(eval.GetChild(0).Text);
                    }
                    break;

                case "res":
                    {
                        var arg = eval.GetChild(0);
                        var arg2 = eval.GetChild(1).Text;
                        if (!EvalBitFunc(codeGen.EmitResXR, arg, arg2))
                        {
                            return false;
                        }
                    }
                    break;

                case "ret":
                    {
                        if (eval.ChildCount != 0)
                        {
                            codeGen.EmitRetCC(eval.GetChild(0).Text);
                        }
                        else
                        {
                            codeGen.EmitRet();
                        }
                    }
                    break;

                case "reti":
                    codeGen.EmitReti();
                    break;

                case "rla":
                    codeGen.EmitRla();
                    break;

                case "rl":
                    codeGen.EmitRl(eval.GetChild(0).Text);
                    break;

                case "rlca":
                    codeGen.EmitRlca();
                    break;

                case "rlc":
                    codeGen.EmitRlc(eval.GetChild(0).Text);
                    break;

                case "rra":
                    codeGen.EmitRra();
                    break;

                case "rr":
                    codeGen.EmitRr(eval.GetChild(0).Text);
                    break;

                case "rrca":
                    codeGen.EmitRrca();
                    break;

                case "rrc":
                    codeGen.EmitRrc(eval.GetChild(0).Text);
                    break;

                case "rst":
                    {
                        if (!EvalArithArgFunc(codeGen.EmitRst, eval.GetChild(0)))
                        {
                            return false;
                        }
                    }
                    break;

                case "sbc":
                    {
                        var arg = eval.ChildCount == 1 ? eval.GetChild(0) : eval.GetChild(1);
                        if (arg.Text == ExpressionToken)
                        {
                            if (!EvalArithArgFunc(codeGen.EmitSbcN, arg))
                            {
                                return false;
                            }
                        }
                        else
                        {
                            codeGen.EmitSbcR(arg.Text);
                        }
                    }
                    break;

                case "scf":
                    codeGen.EmitScf();
                    break;

                case "set":
                    {
                        var arg = eval.GetChild(0);
                        var arg2 = eval.GetChild(1).Text;
                        if (!EvalBitFunc(codeGen.EmitSetXR, arg, arg2))
                        {
                            return false;
                        }
                    }
                    break;

                case "sla":
                    codeGen.EmitSla(eval.GetChild(0).Text);
                    break;

                case "sra":
                    codeGen.EmitSra(eval.GetChild(0).Text);
                    break;

                case "srl":
                    codeGen.EmitSrl(eval.GetChild(0).Text);
                    break;

                case "stop":
                    codeGen.EmitStop();
                    break;

                case "sub":
                    {
                        var arg = eval.ChildCount == 1 ? eval.GetChild(0) : eval.GetChild(1);
                        if (arg.Text == ExpressionToken)
                        {
                            if (!EvalArithArgFunc(codeGen.EmitSubN, arg))
                            {
                                return false;
                            }
                        }
                        else
                        {
                            codeGen.EmitSubR(arg.Text);
                        }
                    }
                    break;

                case "swap":
                    codeGen.EmitSwapR(eval.GetChild(0).Text);
                    break;

                case "xor":
                    {
                        var arg = eval.ChildCount == 1 ? eval.GetChild(0) : eval.GetChild(1);
                        if (arg.Text == ExpressionToken)
                        {
                            if (!EvalArithArgFunc(codeGen.EmitXorN, arg))
                            {
                                return false;
                            }
                        }
                        else
                        {
                            codeGen.EmitXorR(arg.Text);
                        }
                    }
                    break;

                default:
                    break;
            }
            return true;
        }

        private bool EvaluatePseudoInst(ITree eval, int baseOffset)
        {
            switch (eval.Text)
            {
                case DataDefToken:
                    {
                        return EvaluateDataDef(eval.GetChild(0), baseOffset);
                    }
                case MacroCallToken:
                    {
                        return EvaluateMacroCall(eval.GetChild(0), baseOffset);
                    }
                case IncludeToken:
                    {
                        return EvaluateInclude(eval.GetChild(0), baseOffset);
                    }
                default:
                    {
                        MakeErrorMessage(eval, ErrorMessage.Build_UnknownError);
                        return false;
                    }
            }
        }

        private bool EvaluateDataDef(ITree eval, int baseOffset)
        {
            CodeGenerator.DataFuncDelegate dataFunc;
            switch (eval.Text)
            {
                case "db":
                    dataFunc = codeGen.EmitByte;
                    break;

                case "dw":
                    dataFunc = codeGen.EmitWord;
                    break;

                case "dd":
                    dataFunc = codeGen.EmitDWord;
                    break;

                case "dq":
                    dataFunc = codeGen.EmitQWord;
                    break;

                default:
                    return false;
            }

            if (eval.ChildCount == 0)
            {
                dataFunc(0);
            }
            else
            {
                for (int i = 0; i < eval.ChildCount; i++)
                {
                    var arg = eval.GetChild(i).GetChild(0);
                    var result = 0L;
                    if (!EvaluateExpression(arg, out result))
                    {
                        return false;
                    }
                    dataFunc(result);
                }
            }
            return true;
        }

        private bool EvaluateMacroCall(ITree eval, int baseOffset)
        {
            var macroName = GetGlobalScopedID(eval.Text);
            if (!macroDict.ContainsKey(macroName))
            {
                MakeErrorMessage(eval, ErrorMessage.Build_MacroDoesNotExist);
                return false;
            }
            else
            {
                var macArgList = new List<long>();
                var result = 0L;
                for (int i = 0; i < eval.ChildCount; i++)
                {
                    if (!EvaluateExpression(eval.GetChild(i).GetChild(0), out result))
                    {
                        return false;
                    }
                    macArgList.Add(result);
                }
                macroArgStack.Push(macArgList);
                var curScope = GetGlobalScope();
                SetGlobalScope(eval.Text);
                if (!EvaluateAST(macroDict[macroName], baseOffset))
                {
                    return false;
                }
                macroArgStack.Pop();
                SetGlobalScope(curScope);
                return true;
            }
        }

        private bool EvaluateInclude(ITree eval, int baseOffset)
        {
            // TODO: Write this function.
            // Search path for includes:
            // -Directory of binary
            // -Directory of program
            MakeErrorMessage(eval, ErrorMessage.Build_UnknownError);
            return false;
        }

        private bool EvaluateExpression(ITree eval, out long result)
        {
            result = 0;
            var res1 = 0L;
            var res2 = 0L;
            var res3 = 0L;
            if (eval == null)
            {
                MakeErrorMessage(eval, ErrorMessage.Build_UnknownError);
                return false;
            }
            switch (eval.Text)
            {
                case LiteralToken:
                    return EvaluateLiteral(eval.GetChild(0), out result);

                case VarToken:
                    return EvaluateVar(eval.GetChild(0), out result);

                case MacroArgToken:
                    return EvaluateMacroArg(eval.GetChild(0), out result);

                case GBFormatStringToken:
                    return EvaluateGBFormat(eval.GetChild(0), out result);

                case ExpressionToken:
                    return EvaluateExpression(eval.GetChild(0), out result);

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

        private bool EvaluateMacroArg(ITree eval, out long result)
        {
            result = 0;
            int mIndex = 0;
            if (eval == null)
            {
                MakeErrorMessage(eval, ErrorMessage.Build_UnknownError);
                return false;
            }
            else if (macroArgStack.Count == 0)
            {
                MakeErrorMessage(eval, ErrorMessage.Build_MacroArgUsedOutsideOfDef);
                return false;
            }
            else
            {
                switch (eval.Text)
                {
                    case "\\1":
                        mIndex = 0;
                        break;

                    case "\\2":
                        mIndex = 1;
                        break;

                    case "\\3":
                        mIndex = 2;
                        break;

                    case "\\4":
                        mIndex = 3;
                        break;

                    case "\\5":
                        mIndex = 4;
                        break;

                    case "\\6":
                        mIndex = 5;
                        break;

                    case "\\7":
                        mIndex = 6;
                        break;

                    case "\\8":
                        mIndex = 7;
                        break;

                    case "\\9":
                        mIndex = 8;
                        break;

                    default:
                        {
                            MakeErrorMessage(eval, ErrorMessage.Build_UnknownArgument);
                            return false;
                        }
                }
            }
            if (mIndex >= macroArgStack.Peek().Count)
            {
                MakeErrorMessage(eval, ErrorMessage.Build_NotEnoughMacroArgs);
                return false;
            }
            result = macroArgStack.Peek()[mIndex];
            return true;
        }

        private bool EvaluateVar(ITree eval, out long result)
        {
            result = 0;
            var varName = GetGlobalScopedID(eval.Text);
            if (eval == null)
            {
                MakeErrorMessage(eval, ErrorMessage.Build_UnknownError);
                return false;
            }
            else if (!variableDict.ContainsKey(varName))
            {
                MakeErrorMessage(eval, ErrorMessage.Build_UnknownArgument);
                return false;
            }
            else
            {
                result = variableDict[varName];
                return true;
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

        private bool EvaluateGBFormat(ITree eval, out long result)
        {
            result = 0;
            if (!Utility.GameboyFormatStringToWord(eval.Text, GameboyFormatChars, out result))
            {
                MakeErrorMessage(eval, ErrorMessage.Build_NumberOverflow);
                return false;
            }
            return true;
        }

        private void SetGlobalScope(string fileName)
        {
            GlobalScopeName = fileName;
        }

        private void SetLocalScope(string labelName)
        {
            LocalScopeName = labelName;
        }

        private string GetGlobalScope()
        {
            return GlobalScopeName;
        }

        private string GetLocalScope()
        {
            return LocalScopeName;
        }

        private string GetExportScopedID(string id)
        {
            return id;
        }

        private string GetGlobalScopedID(string id)
        {
            return GlobalScopeName + "!" + id;
        }

        private string GetLocalScopedID(string id)
        {
            return GlobalScopeName + "!" + LocalScopeName + "!" + id;
        }

        private void AddSymEntry(int lineNumber, int charPos, string labelName, string fullName, bool isJR, long position, long offsetToFill)
        {
            SymEntry se = new SymEntry();
            se.line = lineNumber;
            se.charpos = charPos;
            se.instructionPosition = position;
            se.label = labelName;
            se.fullyQualifiedLabel = fullName;
            se.isJR = isJR;
            se.offsetToFill = offsetToFill;
            symFillTable.Add(se);
        }

        private void MakeErrorMessage(SymEntry se, ErrorMessage messageType)
        {
            currentError.errorMessage = messageType;
            currentError.lineNumber = se.line;
            currentError.characterNumber = se.charpos;
            currentError.extraInfo1 = se.label;
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

        private bool EvalArithArgFunc(CodeGenerator.ArithmeticFuncDelegate arithFunc, ITree arg)
        {
            var result = 0L;
            if (!EvaluateExpression(arg.GetChild(0), out result))
            {
                return false;
            }
            arithFunc(result);
            return true;
        }

        private bool EvalBitFunc(CodeGenerator.BitFunctionDelegate bitFunc, ITree arg, string reg)
        {
            var result = 0L;
            if (!EvaluateExpression(arg.GetChild(0), out result))
            {
                return false;
            }
            bitFunc(result, reg);
            return true;
        }
    }
}