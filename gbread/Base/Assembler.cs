namespace GBRead.Base
{
    using System.Collections.Generic;
    using Antlr.Runtime;
    using Antlr.Runtime.Tree;

    public class Assembler
    {
        private Dictionary<string, int> variableDict = new Dictionary<string, int>();
        private Dictionary<string, int> callDict = new Dictionary<string, int>();
        private List<SymEntry> symFillTable = new List<SymEntry>();

        private CodeGenerator codeGen = new CodeGenerator();

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
            // TODO: stress test assembling.
            success = false;
            error = new CompError();
            int currentOffset = baseOffset;
            int currentPreOffset = baseOffset;
            codeGen.ClearStream();

            #region Dict Initialization

            variableDict.Clear();
            callDict.Clear();
            symFillTable.Clear();

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

            #endregion Dict Initialization

            #region Build AST

            var css = new CaseInsensitiveStringStream(input);
            var gblex = new GBXLexer(css);
            var cts = new CommonTokenStream(gblex);
            var gbparse = new GBXParser(cts);
            gbparse.TreeAdaptor = new CommonTreeAdaptor();
            var syntaxTree = gbparse.program().Tree;
            var parseErrors = gblex.GetErrors();
            parseErrors.AddRange(gbparse.GetErrors());
            if (parseErrors.Count != 0)
            {
                var f = gbparse.GetErrors()[0];
                error.lineNumber = f.error.Line;
                error.characterNumber = f.error.CharPositionInLine;
                error.errorMessage = ErrorMessage.CUSTOM;
                error.extraInfo1 = f.errText;
                return new byte[1];
            }

            #endregion Build AST

            string ExpressionToken = "EXPRESSION";
            string HLRefToken = "RR_REF_HL";
            string MemRefToken = "MEM_REF";
            string AssignmentToken = "ASSIGNMENT";

            // ROOT -> STATEMENT*
            if (syntaxTree.ChildCount != 0)
            {
                foreach (ITree statementTree in syntaxTree.Children)
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
                            return new byte[1];
                        }
                        int result = 0;
                        ErrorMessage emt = Evaluate(idValue, out result);
                        if (emt == ErrorMessage.NO_ERROR)
                        {
                            variableDict.Add(idName, result);
                        }
                        else
                        {
                            error = new CompError();
                            error.lineNumber = idValue.Line;
                            error.characterNumber = idValue.CharPositionInLine;
                            error.errorMessage = emt;
                            return new byte[1];
                        }
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
                                    return new byte[1];
                                }
                                else
                                {
                                    callDict.Add(labelName, (int)(codeGen.Position + baseOffset));
                                }
                            }
                        }
                        var instField = statementTree.GetChild(statementTree.ChildCount - 1);

                        switch (instField.Text)
                        {
                            case "adc":
                                {
                                    var arg = instField.ChildCount == 1 ? instField.GetChild(0) : instField.GetChild(1);
                                    if (arg.Text == ExpressionToken)
                                    {
                                        if (!EvalArithArgFunc(codeGen.EmitAdcN, arg, ref error))
                                        {
                                            return new byte[1];
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
                                            if (!EvalArithArgFunc(codeGen.EmitAddN, arg, ref error))
                                            {
                                                return new byte[1];
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
                                                if (!EvalArithArgFunc(codeGen.EmitAddN, arg2, ref error))
                                                {
                                                    return new byte[1];
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
                                            if (!EvalArithArgFunc(codeGen.EmitAddSPN, arg2, ref error))
                                            {
                                                return new byte[1];
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
                                        if (!EvalArithArgFunc(codeGen.EmitAndN, arg, ref error))
                                        {
                                            return new byte[1];
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
                                    if (!EvalBitFunc(codeGen.EmitBitXR, arg, arg2, ref error))
                                    {
                                        return new byte[1];
                                    }
                                }
                                break;

                            case "call":
                                {
                                    int memLoc = 0;
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
                                        if (!EvalArithArgFunc(codeGen.EmitCpN, arg, ref error))
                                        {
                                            return new byte[1];
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
                                                return new byte[1];
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
                                                return new byte[1];
                                            }
                                    }
                                }
                                break;

                            case "jp":
                                if (instField.ChildCount == 1)
                                {
                                    int memLoc = 0;
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
                                    int memLoc = 0;
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
                                        int memLoc = callDict[arg.Text];
                                        diff = memLoc - (codeGen.Position + 2);
                                        if (diff < -128 || diff > 127)
                                        {
                                            MakeErrorMessage(arg, ErrorMessage.JR_OUT_OF_RANGE, ref error);
                                            return new byte[1];
                                        }
                                    }
                                    else
                                    {
                                        AddSymEntry(arg, true, codeGen.Position, codeGen.Position + 1);
                                    }

                                    if (sel == 0)
                                    {
                                        codeGen.EmitJr((int)diff);
                                    }
                                    else
                                    {
                                        codeGen.EmitJrCCN(instField.GetChild(0).Text, (int)diff);
                                    } 
                                }
                                
                                break;

                            case "ldhl":
                                {
                                    if (!EvalArithArgFunc(codeGen.EmitLdHLSP, instField.GetChild(1), ref error))
                                    {
                                        return new byte[1];
                                    }
                                }
                                break;

                            case "ldio":
                                {
                                    if (instField.GetChild(0).Text == MemRefToken)
                                    {
                                        if (!EvalArithArgFunc(codeGen.EmitLdioNA, instField.GetChild(0).GetChild(0), ref error))
                                        {
                                            return new byte[1];
                                        }
                                    }
                                    else
                                    {
                                        if (!EvalArithArgFunc(codeGen.EmitLdioAN, instField.GetChild(1).GetChild(0), ref error))
                                        {
                                            return new byte[1];
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
                                                    int result = 0;
                                                    ErrorMessage emt = Evaluate(arg2, out result);
                                                    if (emt == ErrorMessage.NO_ERROR)
                                                    {
                                                        codeGen.EmitLdRN("a", result);
                                                    }
                                                    else
                                                    {
                                                        MakeErrorMessage(arg2, emt, ref error);
                                                        return new byte[1];
                                                    }
                                                }
                                                else if (arg2.Text == MemRefToken)
                                                {
                                                    int result = 0;
                                                    ErrorMessage emt = Evaluate(arg2.GetChild(0), out result);
                                                    if (emt == ErrorMessage.NO_ERROR)
                                                    {
                                                        codeGen.EmitLdANRef(result);
                                                    }
                                                    else
                                                    {
                                                        MakeErrorMessage(arg2, emt, ref error);
                                                        return new byte[1];
                                                    }
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
                                                    int result = 0;
                                                    ErrorMessage emt = Evaluate(arg2, out result);
                                                    if (emt == ErrorMessage.NO_ERROR)
                                                    {
                                                        codeGen.EmitLdRN(arg1.Text, result);
                                                    }
                                                    else
                                                    {
                                                        MakeErrorMessage(arg2, emt, ref error);
                                                        return new byte[1];
                                                    }
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
                                                int result = 0;
                                                ErrorMessage emt = Evaluate(arg2, out result);
                                                if (emt == ErrorMessage.NO_ERROR)
                                                {
                                                    codeGen.EmitLdRRN(arg1.Text, result);
                                                }
                                                else
                                                {
                                                    MakeErrorMessage(arg2, emt, ref error);
                                                    return new byte[1];
                                                }
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
                                                    int result = 0;
                                                    ErrorMessage emt = Evaluate(arg2, out result);
                                                    if (emt == ErrorMessage.NO_ERROR)
                                                    {
                                                        codeGen.EmitLdRN(HLRefToken, result);
                                                    }
                                                    else
                                                    {
                                                        MakeErrorMessage(arg2, emt, ref error);
                                                        return new byte[1];
                                                    }
                                                }
                                                else
                                                {
                                                    codeGen.EmitLdRegReg(HLRefToken, arg2.Text);
                                                }
                                            }
                                            break;

                                        case "MEM_REF":
                                            {
                                                int result = 0;
                                                ErrorMessage emt = Evaluate(arg1.GetChild(0), out result);
                                                if (emt == ErrorMessage.NO_ERROR)
                                                {
                                                    if (arg2.Text == "a")
                                                    {
                                                        codeGen.EmitLdNRefA(result);
                                                    }
                                                    else
                                                    {
                                                        codeGen.EmitLdNRefSP(result);
                                                    }
                                                }
                                                else
                                                {
                                                    MakeErrorMessage(arg2, emt, ref error);
                                                    return new byte[1];
                                                }
                                            }
                                            break;

                                        case "sp":
                                            {
                                                if (arg2.Text == ExpressionToken)
                                                {
                                                    int result = 0;
                                                    ErrorMessage emt = Evaluate(arg2, out result);
                                                    if (emt == ErrorMessage.NO_ERROR)
                                                    {
                                                        codeGen.EmitLdRRN(arg1.Text, result);
                                                    }
                                                    else
                                                    {
                                                        MakeErrorMessage(arg2, emt, ref error);
                                                        return new byte[1];
                                                    }
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
                                        if (!EvalArithArgFunc(codeGen.EmitOrN, arg, ref error))
                                        {
                                            return new byte[1];
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
                                    if (!EvalBitFunc(codeGen.EmitResXR, arg, arg2, ref error))
                                    {
                                        return new byte[1];
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
                                    if (!EvalArithArgFunc(codeGen.EmitRst, instField.GetChild(0), ref error))
                                    {
                                        return new byte[1];
                                    }
                                }
                                break;

                            case "sbc":
                                {
                                    var arg = instField.ChildCount == 1 ? instField.GetChild(0) : instField.GetChild(1);
                                    if (arg.Text == ExpressionToken)
                                    {
                                        if (!EvalArithArgFunc(codeGen.EmitSbcN, arg, ref error))
                                        {
                                            return new byte[1];
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
                                    if (!EvalBitFunc(codeGen.EmitSetXR, arg, arg2, ref error))
                                    {
                                        return new byte[1];
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
                                        if (!EvalArithArgFunc(codeGen.EmitSubN, arg, ref error))
                                        {
                                            return new byte[1];
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
                                        if (!EvalArithArgFunc(codeGen.EmitXorN, arg, ref error))
                                        {
                                            return new byte[1];
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
                foreach (SymEntry se in symFillTable)
                {
                    if (callDict.ContainsKey(se.label))
                    {
                        codeGen.Seek(se.offsetToFill);
                        int memLoc = callDict[se.label];
                        if (se.isJR)
                        {
                            long diff = memLoc - (se.instructionPosition + 2);
                            if (diff < -128 || diff > 127)
                            {
                                MakeErrorMessage(se, ErrorMessage.JR_OUT_OF_RANGE, ref error);
                                return new byte[1];
                            }
                            codeGen.EmitByte((int)diff);
                        }
                        else
                        {
                            codeGen.EmitWord(memLoc);
                        }
                    }
                    else
                    {
                        MakeErrorMessage(se, ErrorMessage.UNKNOWN_ARGUMENT, ref error);
                        return new byte[0];
                    }
                }
            }
            success = true;
            return codeGen.StreamToArray();
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

        private bool EvalArithArgFunc(CodeGenerator.ArithmeticFuncDelegate arithFunc, ITree arg, ref CompError error)
        {
            int result = 0;
            ErrorMessage emt = Evaluate(arg, out result);
            if (emt == ErrorMessage.NO_ERROR)
            {
                arithFunc(result);
                return true;
            }
            else
            {
                MakeErrorMessage(arg, emt, ref error);
                return false;
            }
        }

        private bool EvalBitFunc(CodeGenerator.BitFunctionDelegate arithFunc, ITree arg, string reg, ref CompError error)
        {
            int result = 0;
            ErrorMessage emt = Evaluate(arg, out result);
            if (emt == ErrorMessage.NO_ERROR)
            {
                arithFunc(result, reg);
                return true;
            }
            else
            {
                MakeErrorMessage(arg, emt, ref error);
                return false;
            }
        }

        private ErrorMessage Evaluate(ITree eval, out int result)
        {
            result = 0;
            if (eval.Text == "EXPRESSION")
            {
                // If it's on an expression node, then return the evaluation of the result.
                return Evaluate(eval.GetChild(0), out result);
            }
            else if (eval.Text == "VAR")
            {
                // If it's on an ID node, return the ID's value.
                if (!variableDict.ContainsKey(eval.GetChild(0).Text))
                {
                    return ErrorMessage.UNKNOWN_ARGUMENT;
                }
                else
                {
                    result = variableDict[eval.GetChild(0).Text];
                    return ErrorMessage.NO_ERROR;
                }
            }
            else
            {
                // It's either an operator, or a number.
                var res1 = 0;
                var res2 = 0;
                var res3 = 0;
                switch (eval.ChildCount)
                {
                    case 0:
                        {
                            result = Utility.NumStringToInt(eval.Text);
                            return ErrorMessage.NO_ERROR;
                        }
                    case 1:
                        {
                            ErrorMessage res = Evaluate(eval.GetChild(0), out res1);
                            if (res != ErrorMessage.NO_ERROR)
                            {
                                return res;
                            }
                            switch (eval.Text)
                            {
                                case "~":
                                    result = ~res1;
                                    return ErrorMessage.NO_ERROR;
                                case "-":
                                    result = -res1;
                                    return ErrorMessage.NO_ERROR;
                                case "!":
                                    result = (res1 == 0) ? 1 : 0;
                                    return ErrorMessage.NO_ERROR;
                                default:
                                    return ErrorMessage.UNKNOWN_ARGUMENT;
                            }
                        }
                    case 2:
                        {
                            ErrorMessage res = Evaluate(eval.GetChild(0), out res1);
                            if (res != ErrorMessage.NO_ERROR)
                            {
                                return res;
                            }
                            res = Evaluate(eval.GetChild(1), out res2);
                            if (res != ErrorMessage.NO_ERROR)
                            {
                                return res;
                            }
                            switch (eval.Text)
                            {
                                case "+":
                                    result = res1 + res2;
                                    return ErrorMessage.NO_ERROR;
                                case "-":
                                    result = res1 - res2;
                                    return ErrorMessage.NO_ERROR;
                                case "*":
                                    result = res1 * res2;
                                    return ErrorMessage.NO_ERROR;
                                case "/":
                                    result = res1 / res2;
                                    return ErrorMessage.NO_ERROR;
                                case "%":
                                    result = res1 % res2;
                                    return ErrorMessage.NO_ERROR;
                                case "<<":
                                    result = res1 << res2;
                                    return ErrorMessage.NO_ERROR;
                                case ">>":
                                    result = res1 >> res2;
                                    return ErrorMessage.NO_ERROR;
                                case "<":
                                    result = res1 < res2 ? 1 : 0;
                                    return ErrorMessage.NO_ERROR;
                                case ">":
                                    result = res1 > res2 ? 1 : 0;
                                    return ErrorMessage.NO_ERROR;
                                case "<=":
                                    result = res1 <= res2 ? 1 : 0;
                                    return ErrorMessage.NO_ERROR;
                                case ">=":
                                    result = res1 >= res2 ? 1 : 0;
                                    return ErrorMessage.NO_ERROR;
                                case "==":
                                    result = res1 == res2 ? 1 : 0;
                                    return ErrorMessage.NO_ERROR;
                                case "&":
                                    result = res1 & res2;
                                    return ErrorMessage.NO_ERROR;
                                case "^":
                                    result = res1 ^ res2;
                                    return ErrorMessage.NO_ERROR;
                                case "|":
                                    result = res1 | res2;
                                    return ErrorMessage.NO_ERROR;
                                case "&&":
                                    result = (res1 != 0) && (res2 != 0) ? 1 : 0;
                                    return ErrorMessage.NO_ERROR;
                                case "||":
                                    result = (res1 != 0) || (res2 != 0) ? 1 : 0;
                                    return ErrorMessage.NO_ERROR;
                                default:
                                    return ErrorMessage.UNKNOWN_ARGUMENT;
                            }
                        }
                    case 3:
                        {
                            ErrorMessage res = Evaluate(eval.GetChild(0), out res1);
                            if (res != ErrorMessage.NO_ERROR)
                            {
                                return res;
                            }
                            res = Evaluate(eval.GetChild(1), out res2);
                            if (res != ErrorMessage.NO_ERROR)
                            {
                                return res;
                            }
                            res = Evaluate(eval.GetChild(2), out res3);
                            if (res != ErrorMessage.NO_ERROR)
                            {
                                return res;
                            }
                            switch (eval.Text)
                            {
                                case "?":
                                    result = (res1 != 0) ? res2 : res3;
                                    return ErrorMessage.NO_ERROR;
                                default:
                                    return ErrorMessage.UNKNOWN_ARGUMENT;
                            }
                        }
                    default:
                        return ErrorMessage.UNKNOWN_ARGUMENT;
                }
            }
        }
    }
}