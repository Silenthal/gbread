namespace GBRead.Base
{
    using System.Collections.Generic;
    using Antlr.Runtime;
    using Antlr.Runtime.Tree;

    public class Assembler
    {
        private Dictionary<string, long> variableDict = new Dictionary<string, long>();
        private Dictionary<string, long> callDict = new Dictionary<string, long>();
        private List<SymEntry> symFillTable = new List<SymEntry>();
        private CodeGenerator codeGen = new CodeGenerator();
        private Dictionary<string, ITree> macroDict = new Dictionary<string, ITree>();

        private static string ExpressionToken = "EXPRESSION";
        private static string HLRefToken = "RR_REF_HL";
        private static string MemRefToken = "MEM_REF";
        private static string AssignmentToken = "ASSIGNMENT";
        private static string MacroToken = "MACRO";
        private static string MacroArgToken = "MACRO_ARG";
        private static string VarToken = "VAR";

        private LabelContainer lc;

        private struct SymEntry
        {
            public int line;
            public int charpos;
            public long instructionPosition;
            public long offsetToFill;
            public string label;
            public bool isJR;
        }

        public Assembler(LabelContainer newlc)
        {
            lc = newlc;
        }

        public byte[] AssembleASM(int baseOffset, string input, ref CompError error, out bool success)
        {
            #region Initialization

            // TODO: stress test assembling.
            success = false;
            error = new CompError();
            int currentOffset = baseOffset;
            int currentPreOffset = baseOffset;
            codeGen.ClearStream();
            variableDict.Clear();
            callDict.Clear();
            symFillTable.Clear();
            macroDict.Clear();

            foreach (FunctionLabel kvp in lc.FuncList)
            {
                callDict.Add(kvp.Name, Utility.GetPCAddress(kvp.Value));
            }

            foreach (DataLabel kvp in lc.DataList)
            {
                callDict.Add(kvp.Name, Utility.GetPCAddress(kvp.Value));
            }

            foreach (VarLabel kvp in lc.VarList)
            {
                variableDict.Add(kvp.Name, Utility.GetPCAddress(kvp.Value));
            }

            #endregion Initialization

            var syntaxTree = new CommonTree();
            if (!CreateAST(input, out syntaxTree, ref error))
            {
                return new byte[1];
            }

            if (!(EvaluateAST(syntaxTree, baseOffset, ref error)
                && EvaluateSymbols(baseOffset, ref error)))
            {
                return new byte[1];
            }
            success = true;
            return codeGen.StreamToArray();
        }

        private bool CreateAST(string input, out CommonTree syntaxTree, ref CompError error)
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
                MakeErrorMessage(gbparse.GetErrors()[0], ref error);
                return false;
            }
            return true;
        }

        // TODO: Split up symbol filling and tree evaluation.
        private bool EvaluateAST(ITree syntaxTree, int baseOffset, ref CompError error, bool isMacro = false, List<ITree> macroArgs = null)
        {
            // ROOT -> STATEMENT*
            if (syntaxTree.ChildCount != 0)
            {
                foreach (ITree statementTree in ((CommonTree)syntaxTree).Children)
                {
                    if (statementTree.Text == AssignmentToken)
                    {
                        // ASSIGNMENT -> ID expression
                        string idName = statementTree.GetChild(0).Text;
                        var idValue = statementTree.GetChild(1);
                        if (variableDict.ContainsKey(idName))
                        {
                            error.errorMessage = ErrorMessage.VARIABLE_ALREADY_DEFINED;
                            error.lineNumber = statementTree.GetChild(0).Line;
                            error.characterNumber = statementTree.GetChild(0).CharPositionInLine;
                            error.extraInfo1 = idName;
                            return false;
                        }
                        var result = 0L;
                        if (!EvaluateArgument(idValue, out result, ref error, isMacro, macroArgs))
                        {
                            return false;
                        }
                        variableDict.Add(idName, result);
                    }
                    else if (statementTree.Text == MacroToken)
                    {
                        var macroName = statementTree.GetChild(0).GetChild(0).Text;

                        if (macroDict.ContainsKey(macroName))
                        {
                            MakeErrorMessage(statementTree.GetChild(0), ErrorMessage.Build_MacroAlreadyDefined, ref error);
                            return false;
                        }
                        macroDict.Add(macroName, statementTree);
                    }
                    else
                    {
                        // STATEMENT -> label* (instruction|data_def)
                        int iCount = statementTree.ChildCount;
                        if (iCount > 1)
                        {
                            for (int i = 0; i < iCount - 1; i++)
                            {
                                // GLOBAL_LABEL|LOCAL_LABEL -> ID
                                var labelTree = statementTree.GetChild(i);
                                var labelType = labelTree.Text;
                                var labelName = labelTree.GetChild(0).Text;
                                if (callDict.ContainsKey(labelName))
                                {
                                    error = new CompError("", labelTree.Line, ErrorMessage.LABEL_ALREADY_DEFINED, labelName);
                                    return false;
                                }
                                else
                                {
                                    callDict.Add(labelName, codeGen.Position + baseOffset);
                                }
                            }
                        }
                        var instField = statementTree.GetChild(statementTree.ChildCount - 1);

                        switch (instField.Text)
                        {
                            case "MACRO_CALL":
                                {
                                    if (!macroDict.ContainsKey(instField.GetChild(0).Text))
                                    {
                                        MakeErrorMessage(instField.GetChild(0), ErrorMessage.Build_MacroDoesNotExist, ref error);
                                        return false;
                                    }
                                    else
                                    {
                                        List<ITree> macArgList = null;
                                        if (instField.ChildCount > 1)
                                        {
                                            macArgList = new List<ITree>();
                                            for (int i = 1; i < instField.ChildCount; i++)
                                            {
                                                macArgList.Add(instField.GetChild(i));
                                            }
                                        }
                                        if (!EvaluateAST(macroDict[instField.GetChild(0).Text], baseOffset, ref error, true, macArgList))
                                        {
                                            return false;
                                        }
                                    }
                                }
                                break;

                            case "adc":
                                {
                                    var arg = instField.ChildCount == 1 ? instField.GetChild(0) : instField.GetChild(1);
                                    if (arg.Text == ExpressionToken)
                                    {
                                        if (!EvalArithArgFunc(codeGen.EmitAdcN, arg, ref error, isMacro, macroArgs))
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
                                    if (instField.ChildCount == 1)
                                    {
                                        var arg = instField.GetChild(0);
                                        if (arg.Text == ExpressionToken)
                                        {
                                            if (!EvalArithArgFunc(codeGen.EmitAddN, arg, ref error, isMacro, macroArgs))
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
                                        string arg1 = instField.GetChild(0).Text;
                                        var arg2 = instField.GetChild(1);
                                        if (arg1 == "a")
                                        {
                                            if (arg2.Text == ExpressionToken)
                                            {
                                                if (!EvalArithArgFunc(codeGen.EmitAddN, arg2, ref error, isMacro, macroArgs))
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
                                            if (!EvalArithArgFunc(codeGen.EmitAddSPN, arg2, ref error, isMacro, macroArgs))
                                            {
                                                return false;
                                            }
                                        }
                                    }
                                }
                                break;

                            case "and":
                                {
                                    var arg = instField.ChildCount == 1 ? instField.GetChild(0) : instField.GetChild(1);
                                    if (arg.Text == ExpressionToken)
                                    {
                                        if (!EvalArithArgFunc(codeGen.EmitAndN, arg, ref error, isMacro, macroArgs))
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
                                    var arg = instField.GetChild(0);
                                    var arg2 = instField.GetChild(1).Text;
                                    if (!EvalBitFunc(codeGen.EmitBitXR, arg, arg2, ref error, isMacro, macroArgs))
                                    {
                                        return false;
                                    }
                                }
                                break;

                            case "call":
                                {
                                    var memLoc = 0L;
                                    if (instField.ChildCount == 1)
                                    {
                                        if (callDict.ContainsKey(instField.GetChild(0).Text))
                                        {
                                            memLoc = callDict[instField.GetChild(0).Text];
                                        }
                                        else
                                        {
                                            AddSymEntry(instField.GetChild(0), false, codeGen.Position, codeGen.Position + 1);
                                        }
                                        codeGen.EmitCallN(memLoc);
                                    }
                                    else
                                    {
                                        if (callDict.ContainsKey(instField.GetChild(1).Text))
                                        {
                                            memLoc = callDict[instField.GetChild(1).Text];
                                        }
                                        else
                                        {
                                            AddSymEntry(instField.GetChild(1), false, codeGen.Position, codeGen.Position + 1);
                                        }
                                        codeGen.EmitCallCCN(instField.GetChild(0).Text, memLoc);
                                    }
                                }
                                break;

                            case "ccf":
                                codeGen.EmitCCF();
                                break;

                            case "cp":
                                {
                                    var arg = instField.ChildCount == 1 ? instField.GetChild(0) : instField.GetChild(1);
                                    if (arg.Text == ExpressionToken)
                                    {
                                        if (!EvalArithArgFunc(codeGen.EmitCpN, arg, ref error, isMacro, macroArgs))
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

                            case "db":
                                {
                                    if (!EvalDataFunc(codeGen.EmitByte, instField, ref error, isMacro, macroArgs))
                                    {
                                        return false;
                                    }
                                }
                                break;

                            case "dw":
                                {
                                    if (!EvalDataFunc(codeGen.EmitWord, instField, ref error, isMacro, macroArgs))
                                    {
                                        return false;
                                    }
                                }
                                break;

                            case "dd":
                                {
                                    if (!EvalDataFunc(codeGen.EmitDWord, instField, ref error, isMacro, macroArgs))
                                    {
                                        return false;
                                    }
                                }
                                break;

                            case "dq":
                                {
                                    if (!EvalDataFunc(codeGen.EmitQWord, instField, ref error, isMacro, macroArgs))
                                    {
                                        return false;
                                    }
                                }
                                break;

                            case "dec":
                                {
                                    switch (instField.GetChild(0).Text)
                                    {
                                        case "bc":
                                        case "de":
                                        case "hl":
                                        case "sp":
                                            {
                                                codeGen.EmitDecRR(instField.GetChild(0).Text);
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
                                                codeGen.EmitDecR(instField.GetChild(0).Text);
                                            }
                                            break;

                                        default:
                                            {
                                                MakeErrorMessage(instField.GetChild(0), ErrorMessage.UNKNOWN_ARGUMENT, ref error);
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
                                    switch (instField.GetChild(0).Text)
                                    {
                                        case "bc":
                                        case "de":
                                        case "hl":
                                        case "sp":
                                            {
                                                codeGen.EmitIncRR(instField.GetChild(0).Text);
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
                                                codeGen.EmitIncR(instField.GetChild(0).Text);
                                            }
                                            break;

                                        default:
                                            {
                                                MakeErrorMessage(instField.GetChild(0), ErrorMessage.UNKNOWN_ARGUMENT, ref error);
                                                return false;
                                            }
                                    }
                                }
                                break;

                            case "jp":
                                if (instField.ChildCount == 1)
                                {
                                    var memLoc = 0L;
                                    if (callDict.ContainsKey(instField.GetChild(0).Text))
                                    {
                                        memLoc = callDict[instField.GetChild(0).Text];
                                    }
                                    else
                                    {
                                        AddSymEntry(instField.GetChild(0), false, codeGen.Position, codeGen.Position + 1);
                                    }
                                    codeGen.EmitJpN(memLoc);
                                }
                                else
                                {
                                    var memLoc = 0L;
                                    if (callDict.ContainsKey(instField.GetChild(1).Text))
                                    {
                                        memLoc = callDict[instField.GetChild(1).Text];
                                    }
                                    else
                                    {
                                        AddSymEntry(instField.GetChild(1), false, codeGen.Position, codeGen.Position + 1);
                                    }
                                    codeGen.EmitJpCCN(instField.GetChild(0).Text, memLoc);
                                }
                                break;

                            case "jr":
                                {
                                    int sel = instField.ChildCount - 1;
                                    var arg = instField.GetChild(sel);
                                    long diff = 0;
                                    if (callDict.ContainsKey(arg.Text))
                                    {
                                        var memLoc = callDict[arg.Text];
                                        diff = memLoc - (codeGen.Position + 2);
                                        if (diff < -128 || diff > 127)
                                        {
                                            MakeErrorMessage(arg, ErrorMessage.Build_JROutOfRange, ref error);
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        AddSymEntry(arg, true, codeGen.Position, codeGen.Position + 1);
                                    }

                                    if (sel == 0)
                                    {
                                        codeGen.EmitJr(diff);
                                    }
                                    else
                                    {
                                        codeGen.EmitJrCCN(instField.GetChild(0).Text, diff);
                                    }
                                }

                                break;

                            case "ldhl":
                                {
                                    if (!EvalArithArgFunc(codeGen.EmitLdHLSP, instField.GetChild(1), ref error, isMacro, macroArgs))
                                    {
                                        return false;
                                    }
                                }
                                break;

                            case "ldio":
                                {
                                    if (instField.GetChild(0).Text == MemRefToken)
                                    {
                                        if (!EvalArithArgFunc(codeGen.EmitLdioNA, instField.GetChild(0).GetChild(0), ref error, isMacro, macroArgs))
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        if (!EvalArithArgFunc(codeGen.EmitLdioAN, instField.GetChild(1).GetChild(0), ref error, isMacro, macroArgs))
                                        {
                                            return false;
                                        }
                                    }
                                }
                                break;

                            case "ldi":
                                {
                                    if (instField.GetChild(0).Text == "a")
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
                                    if (instField.GetChild(0).Text == "a")
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
                                    var arg1 = instField.GetChild(0);
                                    var arg2 = instField.GetChild(1);
                                    switch (arg1.Text)
                                    {
                                        case "a":
                                            {
                                                if (arg2.Text == ExpressionToken)
                                                {
                                                    var result = 0L;
                                                    if (!EvaluateArgument(arg2, out result, ref error, isMacro, macroArgs))
                                                    {
                                                        return false;
                                                    }
                                                    codeGen.EmitLdRN("a", result);
                                                }
                                                else if (arg2.Text == MemRefToken)
                                                {
                                                    var result = 0L;
                                                    if (!EvaluateArgument(arg2.GetChild(0), out result, ref error, isMacro, macroArgs))
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
                                                    if (!EvaluateArgument(arg2, out result, ref error, isMacro, macroArgs))
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
                                                if (!EvaluateArgument(arg2, out result, ref error, isMacro, macroArgs))
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
                                                    if (!EvaluateArgument(arg2, out result, ref error, isMacro, macroArgs))
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
                                                if (!EvaluateArgument(arg1.GetChild(0), out result, ref error, isMacro, macroArgs))
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
                                                    if (!(EvaluateArgument(arg2, out result, ref error, isMacro, macroArgs)))
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
                                    var arg = instField.ChildCount == 1 ? instField.GetChild(0) : instField.GetChild(1);
                                    if (arg.Text == ExpressionToken)
                                    {
                                        if (!EvalArithArgFunc(codeGen.EmitOrN, arg, ref error, isMacro, macroArgs))
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
                                    codeGen.EmitPopRR(instField.GetChild(0).Text);
                                }
                                break;

                            case "push":
                                {
                                    codeGen.EmitPushRR(instField.GetChild(0).Text);
                                }
                                break;

                            case "res":
                                {
                                    var arg = instField.GetChild(0);
                                    var arg2 = instField.GetChild(1).Text;
                                    if (!EvalBitFunc(codeGen.EmitResXR, arg, arg2, ref error, isMacro, macroArgs))
                                    {
                                        return false;
                                    }
                                }
                                break;

                            case "ret":
                                {
                                    if (instField.ChildCount != 0)
                                    {
                                        codeGen.EmitRetCC(instField.GetChild(0).Text);
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
                                codeGen.EmitRl(instField.GetChild(0).Text);
                                break;

                            case "rlca":
                                codeGen.EmitRlca();
                                break;

                            case "rlc":
                                codeGen.EmitRlc(instField.GetChild(0).Text);
                                break;

                            case "rra":
                                codeGen.EmitRra();
                                break;

                            case "rr":
                                codeGen.EmitRr(instField.GetChild(0).Text);
                                break;

                            case "rrca":
                                codeGen.EmitRrca();
                                break;

                            case "rrc":
                                codeGen.EmitRrc(instField.GetChild(0).Text);
                                break;

                            case "rst":
                                {
                                    if (!EvalArithArgFunc(codeGen.EmitRst, instField.GetChild(0), ref error, isMacro, macroArgs))
                                    {
                                        return false;
                                    }
                                }
                                break;

                            case "sbc":
                                {
                                    var arg = instField.ChildCount == 1 ? instField.GetChild(0) : instField.GetChild(1);
                                    if (arg.Text == ExpressionToken)
                                    {
                                        if (!EvalArithArgFunc(codeGen.EmitSbcN, arg, ref error, isMacro, macroArgs))
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
                                    var arg = instField.GetChild(0);
                                    var arg2 = instField.GetChild(1).Text;
                                    if (!EvalBitFunc(codeGen.EmitSetXR, arg, arg2, ref error, isMacro, macroArgs))
                                    {
                                        return false;
                                    }
                                }
                                break;

                            case "sla":
                                codeGen.EmitSla(instField.GetChild(0).Text);
                                break;

                            case "sra":
                                codeGen.EmitSra(instField.GetChild(0).Text);
                                break;

                            case "srl":
                                codeGen.EmitSrl(instField.GetChild(0).Text);
                                break;

                            case "stop":
                                codeGen.EmitStop();
                                break;

                            case "sub":
                                {
                                    var arg = instField.ChildCount == 1 ? instField.GetChild(0) : instField.GetChild(1);
                                    if (arg.Text == ExpressionToken)
                                    {
                                        if (!EvalArithArgFunc(codeGen.EmitSubN, arg, ref error, isMacro, macroArgs))
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
                                codeGen.EmitSwapR(instField.GetChild(0).Text);
                                break;

                            case "xor":
                                {
                                    var arg = instField.ChildCount == 1 ? instField.GetChild(0) : instField.GetChild(1);
                                    if (arg.Text == ExpressionToken)
                                    {
                                        if (!EvalArithArgFunc(codeGen.EmitXorN, arg, ref error, isMacro, macroArgs))
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
                    }
                }
            }
            return true;
        }

        private bool EvaluateSymbols(int baseOffset, ref CompError error, bool isMacro = false, List<ITree> macroArgs = null)
        {
            foreach (SymEntry se in symFillTable)
            {
                if (callDict.ContainsKey(se.label))
                {
                    codeGen.Seek(se.offsetToFill);
                    var memLoc = callDict[se.label];
                    if (se.isJR)
                    {
                        long diff = memLoc - (se.instructionPosition + 2);
                        if (diff < -128 || diff > 127)
                        {
                            MakeErrorMessage(se, ErrorMessage.Build_JROutOfRange, ref error);
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
                    MakeErrorMessage(se, ErrorMessage.Build_UnknownArgument, ref error);
                    return false;
                }
            }
            return true;
        }

        private void AddSymEntry(ITree arg, bool isJR, long position, long offsetToFill)
        {
            SymEntry se = new SymEntry();
            se.line = arg.Line;
            se.charpos = arg.CharPositionInLine;
            se.instructionPosition = position;
            se.label = arg.Text;
            se.isJR = isJR;
            se.offsetToFill = offsetToFill;
            symFillTable.Add(se);
        }

        private void MakeErrorMessage(SymEntry se, ErrorMessage messageType, ref CompError error)
        {
            error.errorMessage = messageType;
            error.lineNumber = se.line;
            error.characterNumber = se.charpos;
            error.extraInfo1 = se.label;
        }

        private void MakeErrorMessage(ITree arg, ErrorMessage messageType, ref CompError error)
        {
            error.lineNumber = arg.Line;
            error.characterNumber = arg.CharPositionInLine;
            error.errorMessage = messageType;
            error.extraInfo1 = arg.Text;
        }

        private void MakeErrorMessage(ErrInfo arg, ref CompError error)
        {
            error.lineNumber = arg.error.Line;
            error.characterNumber = arg.error.CharPositionInLine;
            error.errorMessage = ErrorMessage.General_CustomError;
            error.extraInfo1 = arg.errText;
        }

        #region Evaluation

        private bool EvalDataFunc(CodeGenerator.DataFuncDelegate dataFunc, ITree instField, ref CompError error, bool isMacro, List<ITree> macroArgs)
        {
            if (instField.ChildCount == 0)
            {
                dataFunc(0);
            }
            else
            {
                for (int i = 0; i < instField.ChildCount; i++)
                {
                    var arg = instField.GetChild(i);
                    var result = 0L;
                    if (!EvaluateArgument(arg, out result, ref error, isMacro, macroArgs))
                    {
                        return false;
                    }
                    dataFunc(result);
                }
            }
            return true;
        }

        private bool EvalArithArgFunc(CodeGenerator.ArithmeticFuncDelegate arithFunc, ITree arg, ref CompError error, bool isMacro, List<ITree> macroArgs)
        {
            var result = 0L;
            if (!EvaluateArgument(arg, out result, ref error, isMacro, macroArgs))
            {
                return false;
            }
            arithFunc(result);
            return true;
        }

        private bool EvalBitFunc(CodeGenerator.BitFunctionDelegate bitFunc, ITree arg, string reg, ref CompError error, bool isMacro, List<ITree> macroArgs)
        {
            var result = 0L;
            if (!EvaluateArgument(arg, out result, ref error, isMacro, macroArgs))
            {
                return false;
            }
            bitFunc(result, reg);
            return true;
        }

        private bool EvaluateExpression(ITree eval, out long result, ref CompError error, bool isMacro, List<ITree> macroArgs)
        {
            result = 0;
            if (eval.Text == MacroArgToken)
            {
                return EvaluateMacroArg(eval, out result, ref error, isMacro, macroArgs);
            }
            else if (eval.Text == VarToken)
            {
                return EvaluateVar(eval, out result, ref error, isMacro, macroArgs);
            }
            else
            {
                // It's either an operator, or a number.
                var res1 = 0L;
                var res2 = 0L;
                var res3 = 0L;
                switch (eval.ChildCount)
                {
                    case 0:
                        {
                            return EvaluateArgument(eval, out result, ref error, isMacro, macroArgs);
                        }

                    case 1:
                        {
                            if (!EvaluateArgument(eval.GetChild(0), out res1, ref error, isMacro, macroArgs))
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
                                        MakeErrorMessage(eval, ErrorMessage.Build_UnknownArgument, ref error);
                                        return false;
                                    }
                            }
                        }
                    case 2:
                        {
                            if (!(EvaluateArgument(eval.GetChild(0), out res1, ref error, isMacro, macroArgs)
                                && EvaluateArgument(eval.GetChild(1), out res2, ref error, isMacro, macroArgs)))
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
                                        MakeErrorMessage(eval, ErrorMessage.Build_UnknownArgument, ref error);
                                        return false;
                                    }
                            }
                        }
                    case 3:
                        {
                            if (!(EvaluateArgument(eval.GetChild(0), out res1, ref error, isMacro, macroArgs)
                                && EvaluateArgument(eval.GetChild(1), out res2, ref error, isMacro, macroArgs)
                                && EvaluateArgument(eval.GetChild(2), out res3, ref error, isMacro, macroArgs)))
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
                                        MakeErrorMessage(eval, ErrorMessage.Build_UnknownArgument, ref error);
                                        return false;
                                    }
                            }
                        }
                    default:
                        {
                            MakeErrorMessage(eval, ErrorMessage.Build_UnknownArgument, ref error);
                            return false;
                        }
                }
            }
        }

        private bool EvaluateMacroArg(ITree eval, out long result, ref CompError error, bool isMacro, List<ITree> macroArgs)
        {
            result = 0;
            if (eval == null)
            {
                MakeErrorMessage(eval, ErrorMessage.Build_UnknownError, ref error);
                return false;
            }
            else if (!isMacro)
            {
                MakeErrorMessage(eval, ErrorMessage.Build_MacroArgUsedOutsideOfDef, ref error);
                return false;
            }
            else if (macroArgs == null)
            {
                MakeErrorMessage(eval, ErrorMessage.Build_NoMacroArgsPresent, ref error);
                return false;
            }
            else
            {
                switch (eval.Text)
                {
                    case "\\1":
                        if (macroArgs.Count < 1)
                        {
                            MakeErrorMessage(eval, ErrorMessage.Build_NotEnoughMacroArgs, ref error);
                            return false;
                        }
                        else
                        {
                            return EvaluateArgument(macroArgs[0], out result, ref error, isMacro, macroArgs);
                        }

                    case "\\2":
                        if (macroArgs.Count < 2)
                        {
                            MakeErrorMessage(eval, ErrorMessage.Build_NotEnoughMacroArgs, ref error);
                            return false;
                        }
                        else
                        {
                            return EvaluateArgument(macroArgs[1], out result, ref error, isMacro, macroArgs);
                        }

                    case "\\3":
                        if (macroArgs.Count < 3)
                        {
                            MakeErrorMessage(eval, ErrorMessage.Build_NotEnoughMacroArgs, ref error);
                            return false;
                        }
                        else
                        {
                            return EvaluateArgument(macroArgs[2], out result, ref error, isMacro, macroArgs);
                        }

                    case "\\4":
                        if (macroArgs.Count < 4)
                        {
                            MakeErrorMessage(eval, ErrorMessage.Build_NotEnoughMacroArgs, ref error);
                            return false;
                        }
                        else
                        {
                            return EvaluateArgument(macroArgs[3], out result, ref error, isMacro, macroArgs);
                        }

                    case "\\5":
                        if (macroArgs.Count < 5)
                        {
                            MakeErrorMessage(eval, ErrorMessage.Build_NotEnoughMacroArgs, ref error);
                            return false;
                        }
                        else
                        {
                            return EvaluateArgument(macroArgs[4], out result, ref error, isMacro, macroArgs);
                        }

                    case "\\6":
                        if (macroArgs.Count < 6)
                        {
                            MakeErrorMessage(eval, ErrorMessage.Build_NotEnoughMacroArgs, ref error);
                            return false;
                        }
                        else
                        {
                            return EvaluateArgument(macroArgs[5], out result, ref error, isMacro, macroArgs);
                        }

                    case "\\7":
                        if (macroArgs.Count < 7)
                        {
                            MakeErrorMessage(eval, ErrorMessage.Build_NotEnoughMacroArgs, ref error);
                            return false;
                        }
                        else
                        {
                            return EvaluateArgument(macroArgs[6], out result, ref error, isMacro, macroArgs);
                        }

                    case "\\8":
                        if (macroArgs.Count < 8)
                        {
                            MakeErrorMessage(eval, ErrorMessage.Build_NotEnoughMacroArgs, ref error);
                            return false;
                        }
                        else
                        {
                            return EvaluateArgument(macroArgs[7], out result, ref error, isMacro, macroArgs);
                        }

                    case "\\9":
                        if (macroArgs.Count < 9)
                        {
                            MakeErrorMessage(eval, ErrorMessage.Build_NotEnoughMacroArgs, ref error);
                            return false;
                        }
                        else
                        {
                            return EvaluateArgument(macroArgs[8], out result, ref error, isMacro, macroArgs);
                        }
                    default:
                        {
                            MakeErrorMessage(eval, ErrorMessage.Build_UnknownArgument, ref error);
                            return false;
                        }
                }
            }
        }

        private bool EvaluateVar(ITree eval, out long result, ref CompError error, bool isMacro, List<ITree> macroArgs)
        {
            result = 0;
            if (eval == null)
            {
                MakeErrorMessage(eval, ErrorMessage.Build_UnknownError, ref error);
                return false;
            }
            else if (!variableDict.ContainsKey(eval.Text))
            {
                MakeErrorMessage(eval, ErrorMessage.Build_UnknownArgument, ref error);
                return false;
            }
            else
            {
                result = variableDict[eval.Text];
                return true;
            }
        }

        private bool EvaluateArgument(ITree eval, out long result, ref CompError error, bool isMacro, List<ITree> macroArgs)
        {
            result = 0;
            if (eval == null)
            {
                MakeErrorMessage(eval, ErrorMessage.Build_UnknownError, ref error);
                return false;
            }
            else if (eval.Text == ExpressionToken)
            {
                return EvaluateExpression(eval.GetChild(0), out result, ref error, isMacro, macroArgs);
            }
            else if (eval.Text == MacroArgToken)
            {
                return EvaluateMacroArg(eval.GetChild(0), out result, ref error, isMacro, macroArgs);
            }
            else if (!Utility.NumStringToInt(eval.Text, out result))
            {
                MakeErrorMessage(eval, ErrorMessage.NumberOverflow, ref error);
                return false;
            }
            return true;
        }

        #endregion Evaluation
    }
}