namespace GBRead.Base
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class Assembler
    {
        private int NOT_A_NUMBER = Int32.MaxValue;

        private int NumStringToInt(string check)
        {
            bool good = false;
            int temp;
            bool isHex = false;
            if (check.Length > 1)
            {
                if (check.Substring(0, 2).Equals("0x", StringComparison.OrdinalIgnoreCase))
                {
                    check = check.Substring(2);
                    isHex = true;
                }
                else if (check[0].Equals('$'))
                {
                    check = check.Substring(1);
                    isHex = true;
                }
            }
            if (isHex) good = Int32.TryParse(check, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out temp);
            else good = Int32.TryParse(check, out temp);
            return good ? temp : NOT_A_NUMBER;
        }

        private bool isLabel(string check)
        {
            if (check[check.Length - 1].Equals(':'))
            {
                return isGoodWord(check.Substring(0, check.Length - 1));
            }
            else return false;
        }

        private bool isGoodWord(string check)
        {
            if (!Char.IsLetter(check[0])) return false;
            foreach (char s in check.Substring(1))
            {
                if (!(Char.IsLetterOrDigit(s) || s.Equals('_'))) return false;
            }
            return true;
        }

        private bool isConditionalInst(string check)
        {
            if (check.Equals("call", StringComparison.OrdinalIgnoreCase)) return true;
            else if (check.Equals("jp", StringComparison.OrdinalIgnoreCase)) return true;
            else if (check.Equals("jr", StringComparison.OrdinalIgnoreCase)) return true;
            else if (check.Equals("ret", StringComparison.OrdinalIgnoreCase)) return true;
            else return false;
        }

        private bool isNumber(string check)
        {
            int temp;
            if (Int32.TryParse(check, out temp)) return true;
            else if (check.Length >= 2 && check[0].Equals('$'))
            {
                return Int32.TryParse(check.Substring(1), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out temp);
            }
            else if (check.Length > 2 && check[0].Equals('0') && Char.ToLower(check[1]).Equals('x'))
            {
                return Int32.TryParse(check.Substring(2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out temp);
            }
            else return false;
        }

        private bool isMemoryRef(string check)
        {
            if (check.Length > 2)
                return (check[0].Equals('[') && check[check.Length - 1].Equals(']') && !check.Equals("[hl]", StringComparison.OrdinalIgnoreCase));
            else return false;
        }

        private bool isRegSingle(string check)
        {
            if (check.Equals("a", StringComparison.OrdinalIgnoreCase)) return true;
            else if (check.Equals("b", StringComparison.OrdinalIgnoreCase)) return true;
            else if (check.Equals("c", StringComparison.OrdinalIgnoreCase)) return true;
            else if (check.Equals("d", StringComparison.OrdinalIgnoreCase)) return true;
            else if (check.Equals("e", StringComparison.OrdinalIgnoreCase)) return true;
            else if (check.Equals("h", StringComparison.OrdinalIgnoreCase)) return true;
            else if (check.Equals("l", StringComparison.OrdinalIgnoreCase)) return true;
            else if (check.Equals("[hl]", StringComparison.OrdinalIgnoreCase)) return true;
            else return false;
        }

        private bool isRegDouble(string check)
        {
            if (check.Equals("af", StringComparison.OrdinalIgnoreCase)) return true;
            else if (check.Equals("bc", StringComparison.OrdinalIgnoreCase)) return true;
            else if (check.Equals("de", StringComparison.OrdinalIgnoreCase)) return true;
            else if (check.Equals("hl", StringComparison.OrdinalIgnoreCase)) return true;
            else if (check.Equals("sp", StringComparison.OrdinalIgnoreCase)) return true;
            else return false;
        }

        struct InstructionToken
        {
            public TokenType specifier;
            public string tokenVal;
            public int tokenAsInteger;

            public InstructionToken(TokenType spec, string val = "", int tai = 0)
            {
                specifier = spec;
                tokenVal = val;
                tokenAsInteger = tai;
            }
        };

        private LabelContainer lc;

        #region Regexes

        private Regex equalRegex = new Regex(@"\.?(equ)|=", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private Regex assignmentRegex = new Regex(@"^(\w+)\s+((equ)|=)\s+(.*)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        #endregion


        public Assembler(LabelContainer newlc)
        {
            lc = newlc;
        }

        public byte[] AssembleASM(int baseOffset, string[] lines, ref CompError error, out bool success)
        {
            success = false;
            int currentOffset = baseOffset;
            int currentPreOffset = baseOffset;

            #region Initializing lists

            Dictionary<string, int> variableDict = new Dictionary<string, int>();
            Dictionary<string, int> callDict = new Dictionary<string, int>();

            foreach (FunctionLabel kvp in lc.FuncList)
            {
                callDict.Add(kvp.Name, kvp.Offset);
            }

            foreach (VarLabel vlp in lc.VarList)
            {
                variableDict.Add(vlp.Name, vlp.Variable);
            }

            #endregion Initializing lists

            #region Presizing arrays

            for (int i = 0; i < lines.Length; i++)
            {
                int commentIndex = lines[i].IndexOfAny(commentChars);
                if (commentIndex != -1) lines[i] = lines[i].Substring(0, commentIndex);
                lines[i] = lines[i].Trim();

                #region If line is empty

                if (lines[i].Equals(String.Empty)) continue;

                #endregion If line is empty

                #region If line is assignment

                else if (equalRegex.Match(lines[i]).Success)
                {
                    Match mt = assignmentRegex.Match(lines[i]);
                    string varName = String.Empty;
                    string varVal = String.Empty;
                    if (mt.Success)
                    {
                        varName = mt.Groups[1].Value;
                        varVal = mt.Groups[4].Value;
                        if (isGoodWord(varName))
                        {
                            if (variableDict.ContainsKey(varName))
                            {
                                error = new CompError(lines[i], i + 1, ErrorMessage.VARIABLE_ALREADY_DEFINED, varName);
                                return null;
                            }
                            else if (!isNumber(varVal))
                            {
                                error = new CompError(lines[i], i + 1, ErrorMessage.VARIABLE_NOT_ASSIGNED_NUMBER, varVal);
                                return null;
                            }
                            else
                            {
                                variableDict.Add(varName, NumStringToInt(varVal));
                                lines[i] = String.Empty;
                            }
                        }
                        else
                        {
                            error = new CompError(lines[i], i + 1, ErrorMessage.VARIABLE_NAME_INVALID, varName);
                            return null;
                        }
                    }
                    else
                    {
                        error = new CompError(lines[i], i + 1, ErrorMessage.UNRECOGNIZED_LINE, lines[i]);
                        return null;
                    }
                }

                #endregion If line is assignment

                #region If line is data field

                else if (Regex.Match(lines[i], @"^([Dd][BbWw])").Success)
                {
                    string identifier = lines[i].Substring(0, 2);
                    lines[i] = Regex.Replace(lines[i], @"\s*", @"");
                    string[] argList = lines[i].Substring(2).Split(new Char[] { ',' });
                    if (argList.Length > 0)
                    {
                        Dictionary<string, int> dataWordsDict = new Dictionary<string, int>
                        {
                            {"db", 1},
                            {"dw", 2}
                        };
                        int dataSize = dataWordsDict[identifier];
                        for (int dCount = 0; dCount < argList.Length; dCount++)
                        {
                            currentPreOffset += dataSize;
                        }
                    }
                    else
                    {
                        error = new CompError(lines[i], i + 1, ErrorMessage.DATA_ARGUMENTS_UNRECOGNIZED, lines[i]);
                        return null;
                    }
                }

                #endregion If line is data field

                #region If line is label

                else if (isLabel(lines[i]))
                {
                    if (callDict.ContainsKey(lines[i]))
                    {
                        error = new CompError(lines[i], i + 1, ErrorMessage.LABEL_ALREADY_DEFINED, lines[i]);
                        return null;
                    }
                    else
                    {
                        callDict.Add(lines[i].Substring(0, lines[i].Length - 1), currentPreOffset);
                        lines[i] = String.Empty;
                        continue;
                    }
                }

                #endregion If line is label

                #region If line is instruction, or otherwise.

                else
                {
                    #region Instruction split

                    Match instMatch = Regex.Match(lines[i], @"^(\w+)\s*(([\w\[\]\$\-]+)\s*(,\s*([\w\[\]\$\-]+))?)?");
                    string inst = String.Empty;
                    string arg1 = String.Empty;
                    string arg2 = String.Empty;
                    string instArgKey = String.Empty;

                    #region Assign arg strings.

                    if (instMatch.Success)
                    {
                        inst = instMatch.Groups[1].Value.ToLower();
                        arg1 = instMatch.Groups[3].Value;
                        arg2 = instMatch.Groups[5].Value;
                        if (arithmeticArgs.Contains<string>(inst) && arg1.Equals("a", StringComparison.OrdinalIgnoreCase) && !arg2.Equals(String.Empty))
                        {
                            arg1 = arg2;
                            arg2 = String.Empty;
                        }
                    }
                    else
                    {
                        error = new CompError(lines[i], i + 1, ErrorMessage.UNRECOGNIZED_LINE);
                        return null;
                    }

                    #endregion Assign arg strings.

                    #region Check if inst can be converted right away

                    instArgKey = inst + (arg1.Equals(String.Empty) ? String.Empty : (" " + arg1.ToLower() + (arg2.Equals(String.Empty) ? String.Empty : "," + arg2.ToLower())));
                    if (staticArgs.ContainsKey(instArgKey))
                    {
                        currentPreOffset += staticArgs[instArgKey].Length;
                        continue;
                    }
                    else if (Regex.Match(instArgKey, @"srl [AaBbCcDdEeHhLl][AaBbCcDdEeHhLl],[1-7]").Success)
                    {
                        byte[] b1 = staticArgs["srl " + arg1.Substring(0, 1).ToLower()];
                        byte[] b2 = staticArgs["rr " + arg1.Substring(1, 1).ToLower()];
                        int length = NumStringToInt(arg2);
                        for (int ift = 0; ift < length; ift++)
                        {
                            currentPreOffset += b1.Length + b2.Length;
                        }
                        continue;
                    }

                    #endregion Check if inst can be converted right away

                    InstructionToken arg1Token = new InstructionToken(TokenType.UNKNOWN);
                    InstructionToken arg2Token = new InstructionToken(TokenType.UNKNOWN);

                    #region Arg 1 Handling

                    if (arg1.Equals(String.Empty)) arg1Token = new InstructionToken(TokenType.NONE, arg1);
                    else if (isMemoryRef(arg1))
                    {
                        arg1 = arg1.Substring(1, arg1.Length - 2);
                        if (isNumber(arg1))
                        {
                            arg1Token = new InstructionToken(TokenType.MEMORY_REF_DIRECT, arg1, NumStringToInt(arg1));
                        }
                        else if (variableDict.ContainsKey(arg1))
                        {
                            arg1Token = new InstructionToken(TokenType.MEMORY_REF_DIRECT, arg1, variableDict[arg1]);
                        }
                        else if (callDict.ContainsKey(arg1))
                        {
                            arg1Token = new InstructionToken(TokenType.MEMORY_REF_DIRECT, arg1, callDict[arg1]);
                        }
                        else
                        {
                            error = new CompError(lines[i], i + 1, ErrorMessage.UNKNOWN_MEMORY_REFERENCE, arg1);
                        }
                    }
                    else if (isConditionalInst(inst) && conditionBytes.ContainsKey(arg1)) arg1Token = new InstructionToken(TokenType.CONDITION, arg1, conditionBytes[arg1]);
                    else if (isRegSingle(arg1)) arg1Token = new InstructionToken(TokenType.REG_SINGLE, arg1.ToLowerInvariant(), regSingleBytes[arg1]);
                    else if (isRegDouble(arg1)) arg1Token = new InstructionToken(TokenType.REG_DOUBLE, arg1.ToLowerInvariant(), regDoubleBytes[arg1]);
                    else if (variableDict.ContainsKey(arg1))
                    {
                        arg1Token = new InstructionToken(TokenType.NUMBER, arg1, variableDict[arg1]);
                    }
                    else if (isGoodWord(arg1))
                    {
                        arg1Token = new InstructionToken(TokenType.LABEL, arg1, 0);
                    }
                    else if (isNumber(arg1)) arg1Token = new InstructionToken(TokenType.NUMBER, arg1, 0);
                    else
                    {
                        error = new CompError(lines[i], i + 1, ErrorMessage.UNKNOWN_ARGUMENT, arg1);
                    }

                    #endregion Arg 1 Handling

                    #region Arg 2 Handling

                    if (arg2.Equals(String.Empty)) arg2Token = new InstructionToken(TokenType.NONE, arg2);
                    else if (isMemoryRef(arg2))
                    {
                        arg2 = arg2.Substring(1, arg2.Length - 2);
                        if (isNumber(arg2))
                        {
                            arg2Token = new InstructionToken(TokenType.MEMORY_REF_DIRECT, arg2, NumStringToInt(arg2));
                        }
                        else if (variableDict.ContainsKey(arg2))
                        {
                            arg2Token = new InstructionToken(TokenType.MEMORY_REF_DIRECT, arg2, variableDict[arg2]);
                        }
                        else if (callDict.ContainsKey(arg2))
                        {
                            arg2Token = new InstructionToken(TokenType.MEMORY_REF_DIRECT, arg2, callDict[arg2]);
                        }
                        else
                        {
                            error = new CompError(lines[i], i + 1, ErrorMessage.UNKNOWN_MEMORY_REFERENCE, arg2);
                        }
                    }
                    else if (isConditionalInst(inst) && conditionBytes.ContainsKey(arg2)) arg2Token = new InstructionToken(TokenType.CONDITION, arg2, conditionBytes[arg2]);
                    else if (isRegSingle(arg2)) arg2Token = new InstructionToken(TokenType.REG_SINGLE, arg2.ToLowerInvariant(), regSingleBytes[arg2]);
                    else if (isRegDouble(arg2)) arg2Token = new InstructionToken(TokenType.REG_DOUBLE, arg2.ToLowerInvariant(), regDoubleBytes[arg2]);
                    else if (variableDict.ContainsKey(arg2))
                    {
                        arg2Token = new InstructionToken(TokenType.NUMBER, arg2, variableDict[arg2]);
                    }
                    else if (isGoodWord(arg2))
                    {
                        arg2Token = new InstructionToken(TokenType.LABEL, arg2, 0);
                    }
                    else if (isNumber(arg2)) arg2Token = new InstructionToken(TokenType.NUMBER, arg2, 0);
                    else
                    {
                        error = new CompError(lines[i], i + 1, ErrorMessage.UNKNOWN_ARGUMENT, arg2);
                    }

                    #endregion Arg 2 Handling

                    #endregion Instruction split

                    if (inst.Equals("add", StringComparison.OrdinalIgnoreCase) && arg1Token.tokenVal.Equals("sp", StringComparison.OrdinalIgnoreCase) && arg2Token.specifier == TokenType.NUMBER)
                    {
                        currentPreOffset += 2;
                    }

                    #region One Argument

                    else if (singleIntArgBytes.ContainsKey(inst) && arg1Token.specifier != TokenType.NONE && arg2Token.specifier == TokenType.NONE)
                    {
                        #region Integer Argument

                        if (arg1Token.specifier == TokenType.NUMBER || arg1Token.specifier == TokenType.LABEL)
                        {
                            if (inst.Equals("jp", StringComparison.OrdinalIgnoreCase) || inst.Equals("call", StringComparison.OrdinalIgnoreCase))
                            {
                                currentPreOffset += 3;
                            }
                            else
                            {
                                currentPreOffset += 2;
                            }
                        }

                        #endregion Integer Argument

                        else
                        {
                            error = new CompError(lines[i], i + 1, ErrorMessage.SINGLE_ARG_UNRECOGNIZED, arg1Token.tokenVal);
                            return null;
                        }
                    }

                    #endregion One Argument

                    #region Two Arguments

                    else if (doubleArgumentInst.Contains<string>(inst) && arg1Token.specifier != TokenType.NONE && arg2Token.specifier != TokenType.NONE)
                    {
                        #region LDHL

                        if (inst.Equals("ldhl") && arg1Token.tokenVal.Equals("sp") && arg2Token.specifier == TokenType.NUMBER)
                        {
                            currentPreOffset += 2;
                        }

                        #endregion LDHL

                        #region LD

                        else if (inst.Equals("ld", StringComparison.OrdinalIgnoreCase))
                        {
                            #region LD RegPair w/ Value

                            if (arg1Token.specifier == TokenType.REG_DOUBLE && arg2Token.specifier == TokenType.NUMBER && !arg1Token.tokenVal.Equals("af", StringComparison.OrdinalIgnoreCase))
                            {
                                currentPreOffset += 3;
                            }

                            #endregion LD RegPair w/ Value

                            #region LD [nn],SP

                            else if (arg1Token.specifier == TokenType.MEMORY_REF_DIRECT && arg2Token.tokenVal.Equals("sp", StringComparison.OrdinalIgnoreCase))
                            {
                                currentPreOffset += 3;
                            }

                            #endregion LD [nn],SP

                            #region LD r, n

                            else if (arg1Token.specifier == TokenType.REG_SINGLE && arg2Token.specifier == TokenType.NUMBER)
                            {
                                currentPreOffset += 2;
                            }

                            #endregion LD r, n

                            #region A as first arg

                            else if (arg1Token.tokenVal.Equals("a", StringComparison.OrdinalIgnoreCase))
                            {
                                #region LD A, [number]

                                if (arg2Token.specifier == TokenType.MEMORY_REF_DIRECT)
                                {
                                    if (callDict.ContainsKey(arg2Token.tokenVal) && (arg2Token.tokenAsInteger & 0xFFFF) >= 0xFF00)
                                    {
                                        currentPreOffset += 2;
                                    }
                                    else
                                    {
                                        currentPreOffset += 3;
                                    }
                                }

                                #endregion LD A, [number]

                                else
                                {
                                    error = new CompError(lines[i], i + 1, ErrorMessage.SINGLE_ARG_UNRECOGNIZED, arg2Token.tokenVal);
                                    return null;
                                }
                            }

                            #endregion A as first arg

                            #region A as second arg

                            else if (arg2Token.tokenVal.Equals("a", StringComparison.OrdinalIgnoreCase))
                            {
                                #region LD [number], A

                                if (arg1Token.specifier == TokenType.MEMORY_REF_DIRECT)
                                {
                                    if ((arg1Token.tokenAsInteger & 0xFFFF) < 0xFF00)
                                    {
                                        currentPreOffset += 3;
                                    }
                                    else
                                    {
                                        currentPreOffset += 2;
                                    }
                                }

                                #endregion LD [number], A

                                else
                                {
                                    error = new CompError(lines[i], i + 1, ErrorMessage.SINGLE_ARG_UNRECOGNIZED, arg1Token.tokenVal);
                                    return null;
                                }
                            }

                            #endregion A as second arg

                            else
                            {
                                error = new CompError(lines[i], i + 1, ErrorMessage.DOUBLE_ARG_UNRECOGNIZED, arg1Token.tokenVal, arg2Token.tokenVal);
                                return null;
                            }
                        }

                        #endregion LD

                        #region Call/Jp/Jr

                        else if ((inst.Equals("call", StringComparison.OrdinalIgnoreCase) || inst.Equals("jp", StringComparison.OrdinalIgnoreCase) || inst.Equals("jr", StringComparison.OrdinalIgnoreCase)) && arg1Token.specifier == TokenType.CONDITION && (arg2Token.specifier == TokenType.LABEL || arg2Token.specifier == TokenType.NUMBER))
                        {
                            if (inst.Equals("jr", StringComparison.OrdinalIgnoreCase))
                            {
                                currentPreOffset += 2;
                            }
                            else
                            {
                                currentPreOffset += 3;
                            }
                        }

                        #endregion Call/Jp/Jr

                        else
                        {
                            error = new CompError(lines[i], i + 1, ErrorMessage.DOUBLE_ARG_UNRECOGNIZED, arg1Token.tokenVal, arg2Token.tokenVal);
                            return null;
                        }
                    }

                    #endregion Two Arguments

                    else
                    {
                        error = new CompError(lines[i], i + 1, ErrorMessage.UNRECOGNIZED_LINE);
                        return null;
                    }
                }

                #endregion If line is instruction, or otherwise.
            }

            #endregion Presizing arrays

            byte[] returnedArray = new byte[currentPreOffset - baseOffset];
            int returnedArrayOffset = 0;

            #region Adding Data

            for (int i = 0; i < lines.Length; i++)
            {
                #region If line is empty

                if (lines[i].Equals(String.Empty)) continue;

                #endregion If line is empty

                #region If line is data field

                else if (Regex.Match(lines[i], @"^([Dd][BbWw])").Success)
                {
                    string identifier = lines[i].Substring(0, 2).ToLower();
                    string[] argList = lines[i].Substring(2).Split(new Char[] { ',' });
                    if (argList.Length > 0)
                    {
                        Dictionary<string, int> dataWordsDict = new Dictionary<string, int>
                        {
                            {"db", 1},
                            {"dw", 2}
                        };
                        int dataSize = dataWordsDict[identifier];
                        for (int dCount = 0; dCount < argList.Length; dCount++)
                        {
                            int varValue = -1;
                            if (isNumber(argList[dCount]))
                            {
                                varValue = NumStringToInt(argList[dCount]);
                            }
                            else if (variableDict.ContainsKey(argList[dCount]))
                            {
                                varValue = variableDict[argList[dCount]];
                            }
                            else if (callDict.ContainsKey(argList[dCount]))
                            {
                                varValue = callDict[argList[dCount]];
                            }
                            else
                            {
                                error = new CompError(lines[i], i + 1, ErrorMessage.DATA_SINGLE_ARG_UNRECOGNIZED, argList[dCount]);
                                return null;
                            }
                            for (int ds = 0; ds < dataSize; ds++)
                            {
                                returnedArray[returnedArrayOffset++] = (byte)(varValue >> (8 * ds));
                            }
                        }
                    }
                    else
                    {
                        error = new CompError(lines[i], i + 1, ErrorMessage.DATA_ARGUMENTS_UNRECOGNIZED, lines[i]);
                        return null;
                    }
                }

                #endregion If line is data field

                #region If line is instruction, or otherwise.

                else
                {
                    Match instMatch = Regex.Match(lines[i], @"^\s*(\w+)\s*(([\w\[\]\$\-]+)\s*(,\s*([\w\[\]\$\-]+))?)?");
                    string inst = String.Empty;
                    string arg1 = String.Empty;
                    string arg2 = String.Empty;
                    string instArgKey = String.Empty;

                    #region Assign arg strings.

                    if (instMatch.Success)
                    {
                        inst = instMatch.Groups[1].Value.ToLower();
                        arg1 = instMatch.Groups[3].Value;
                        arg2 = instMatch.Groups[5].Value;
                        if (arithmeticArgs.Contains<string>(inst) && arg1.Equals("a", StringComparison.OrdinalIgnoreCase) && !arg2.Equals(String.Empty))
                        {
                            arg1 = arg2;
                            arg2 = String.Empty;
                        }
                    }
                    else
                    {
                        error = new CompError(lines[i], i + 1, ErrorMessage.UNRECOGNIZED_LINE);
                        return null;
                    }

                    #endregion Assign arg strings.

                    #region Check if inst can be converted right away

                    instArgKey = inst + (arg1.Equals(String.Empty) ? String.Empty : (" " + arg1.ToLower() + (arg2.Equals(String.Empty) ? String.Empty : "," + arg2.ToLower())));
                    if (staticArgs.ContainsKey(instArgKey))
                    {
                        foreach (byte bt in staticArgs[instArgKey])
                        {
                            returnedArray[returnedArrayOffset++] = bt;
                        }
                        currentOffset += staticArgs[instArgKey].Length;
                        continue;
                    }
                    else if (Regex.Match(instArgKey, @"srl [AaBbCcDdEeHhLl][AaBbCcDdEeHhLl],[1-7]").Success)
                    {
                        byte[] b1 = staticArgs["srl " + arg1.Substring(0, 1).ToLower()];
                        byte[] b2 = staticArgs["rr " + arg1.Substring(1, 1).ToLower()];
                        int length = NumStringToInt(arg2);
                        for (int ift = 0; ift < length; ift++)
                        {
                            foreach (byte bt in b1)
                            {
                                returnedArray[returnedArrayOffset++] = bt;
                            }
                            foreach (byte bt in b2)
                            {
                                returnedArray[returnedArrayOffset++] = bt;
                            }
                        }
                        continue;
                    }

                    #endregion Check if inst can be converted right away

                    InstructionToken arg1Token = new InstructionToken(TokenType.UNKNOWN);
                    InstructionToken arg2Token = new InstructionToken(TokenType.UNKNOWN);

                    #region Arg 1 Handling

                    if (arg1.Equals(String.Empty)) arg1Token = new InstructionToken(TokenType.NONE, arg1);
                    else if (isMemoryRef(arg1))
                    {
                        arg1 = arg1.Substring(1, arg1.Length - 2);
                        if (isNumber(arg1))
                        {
                            arg1Token = new InstructionToken(TokenType.MEMORY_REF_DIRECT, arg1, NumStringToInt(arg1));
                        }
                        else if (callDict.ContainsKey(arg1))
                        {
                            int memLoc = callDict[arg1];
                            memLoc = memLoc < 0x4000 ? memLoc : ((memLoc % 0x4000) + 0x4000);
                            arg1Token = new InstructionToken(TokenType.MEMORY_REF_DIRECT, String.Empty, memLoc);
                        }
                        else if (variableDict.ContainsKey(arg1))
                        {
                            arg1Token = new InstructionToken(TokenType.MEMORY_REF_DIRECT, String.Empty, variableDict[arg1]);
                        }
                        else
                        {
                            error = new CompError(lines[i], i + 1, ErrorMessage.UNKNOWN_MEMORY_REFERENCE, arg1);
                        }
                    }
                    else if (isConditionalInst(inst) && conditionBytes.ContainsKey(arg1)) arg1Token = new InstructionToken(TokenType.CONDITION, arg1, conditionBytes[arg1]);
                    else if (isRegSingle(arg1)) arg1Token = new InstructionToken(TokenType.REG_SINGLE, arg1.ToLowerInvariant(), regSingleBytes[arg1]);
                    else if (isRegDouble(arg1)) arg1Token = new InstructionToken(TokenType.REG_DOUBLE, arg1.ToLowerInvariant(), regDoubleBytes[arg1]);
                    else if (callDict.ContainsKey(arg1))
                    {
                        int memLoc = callDict[arg1];
                        memLoc = memLoc < 0x4000 ? memLoc : ((memLoc % 0x4000) + 0x4000);
                        arg1Token = new InstructionToken(TokenType.LABEL, arg1, memLoc);
                    }
                    else if (variableDict.ContainsKey(arg1))
                    {
                        arg1Token = new InstructionToken(TokenType.NUMBER, arg1, variableDict[arg1]);
                    }
                    else if (isNumber(arg1)) arg1Token = new InstructionToken(TokenType.NUMBER, arg1, NumStringToInt(arg1));
                    else
                    {
                        error = new CompError(lines[i], i + 1, ErrorMessage.UNKNOWN_ARGUMENT, arg1);
                    }

                    #endregion Arg 1 Handling

                    #region Arg 2 Handling

                    if (arg2.Equals(String.Empty)) arg2Token = new InstructionToken(TokenType.NONE, arg2);
                    else if (isMemoryRef(arg2))
                    {
                        arg2 = arg2.Substring(1, arg2.Length - 2);
                        if (isNumber(arg2))
                        {
                            arg2Token = new InstructionToken(TokenType.MEMORY_REF_DIRECT, arg2, NumStringToInt(arg2));
                        }
                        else if (callDict.ContainsKey(arg2))
                        {
                            int memLoc = callDict[arg2];
                            memLoc = memLoc < 0x4000 ? memLoc : ((memLoc % 0x4000) + 0x4000);
                            arg2Token = new InstructionToken(TokenType.MEMORY_REF_DIRECT, String.Empty, memLoc);
                        }
                        else if (variableDict.ContainsKey(arg2))
                        {
                            arg2Token = new InstructionToken(TokenType.MEMORY_REF_DIRECT, String.Empty, variableDict[arg2]);
                        }
                        else
                        {
                            error = new CompError(lines[i], i + 1, ErrorMessage.UNKNOWN_MEMORY_REFERENCE, arg2);
                        }
                    }
                    else if (isConditionalInst(inst) && conditionBytes.ContainsKey(arg2)) arg2Token = new InstructionToken(TokenType.CONDITION, arg2, conditionBytes[arg2]);
                    else if (isRegSingle(arg2)) arg2Token = new InstructionToken(TokenType.REG_SINGLE, arg2.ToLowerInvariant(), regSingleBytes[arg2]);
                    else if (isRegDouble(arg2)) arg2Token = new InstructionToken(TokenType.REG_DOUBLE, arg2.ToLowerInvariant(), regDoubleBytes[arg2]);
                    else if (callDict.ContainsKey(arg2))
                    {
                        int memLoc = callDict[arg2];
                        memLoc = memLoc < 0x4000 ? memLoc : ((memLoc % 0x4000) + 0x4000);
                        arg2Token = new InstructionToken(TokenType.LABEL, arg2, memLoc);
                    }
                    else if (variableDict.ContainsKey(arg2))
                    {
                        arg2Token = new InstructionToken(TokenType.NUMBER, arg2, variableDict[arg2]);
                    }
                    else if (isNumber(arg2)) arg2Token = new InstructionToken(TokenType.NUMBER, arg2, NumStringToInt(arg2));
                    else
                    {
                        error = new CompError(lines[i], i + 1, ErrorMessage.UNKNOWN_ARGUMENT, arg2);
                    }

                    #endregion Arg 2 Handling

                    #region ADD SP,N

                    if (inst.Equals("add", StringComparison.OrdinalIgnoreCase) && arg1Token.tokenVal.Equals("sp", StringComparison.OrdinalIgnoreCase) && arg2Token.specifier == TokenType.NUMBER)
                    {
                        returnedArray[returnedArrayOffset++] = 0xE8;
                        returnedArray[returnedArrayOffset++] = (byte)arg2Token.tokenAsInteger;
                        currentOffset += 2;
                    }

                    #endregion ADD SP,N

                    #region One Argument

                    else if (singleIntArgBytes.ContainsKey(inst) && arg1Token.specifier != TokenType.NONE && arg2Token.specifier == TokenType.NONE)
                    {
                        #region Integer Argument

                        if (arg1Token.specifier == TokenType.NUMBER || arg1Token.specifier == TokenType.LABEL)
                        {
                            if (inst.Equals("jr"))
                            {
                                int diff = arg1Token.tokenAsInteger - (currentOffset + 2);
                                if (diff < -128 || diff > 127)
                                {
                                    error = new CompError(lines[i], i + 1, ErrorMessage.JR_OUT_OF_RANGE);
                                    return null;
                                }
                                else
                                {
                                    returnedArray[returnedArrayOffset++] = singleIntArgBytes[inst];
                                    returnedArray[returnedArrayOffset++] = (byte)diff;
                                    currentOffset += 2;
                                }
                            }
                            else if (inst.Equals("jp") || inst.Equals("call"))
                            {
                                returnedArray[returnedArrayOffset++] = singleIntArgBytes[inst];
                                returnedArray[returnedArrayOffset++] = (byte)arg1Token.tokenAsInteger;
                                returnedArray[returnedArrayOffset++] = (byte)(arg1Token.tokenAsInteger >> 8);
                                currentOffset += 3;
                            }
                            else
                            {
                                returnedArray[returnedArrayOffset++] = singleIntArgBytes[inst];
                                returnedArray[returnedArrayOffset++] = (byte)arg1Token.tokenAsInteger;
                                currentOffset += 2;
                            }
                        }

                        #endregion Integer Argument

                        else
                        {
                            error = new CompError(lines[i], i + 1, ErrorMessage.SINGLE_ARG_UNRECOGNIZED, arg1Token.tokenVal);
                            return null;
                        }
                    }

                    #endregion One Argument

                    #region Two Arguments

                    else if (doubleArgumentInst.Contains<string>(inst) && arg1Token.specifier != TokenType.NONE && arg2Token.specifier != TokenType.NONE)
                    {
                        #region LDHL

                        if (inst.Equals("ldhl") && arg1Token.tokenVal.Equals("sp") && arg2Token.specifier == TokenType.NUMBER)
                        {
                            returnedArray[returnedArrayOffset++] = 0xF8;
                            returnedArray[returnedArrayOffset++] = (byte)arg2Token.tokenAsInteger;
                            currentOffset += 2;
                        }

                        #endregion LDHL

                        #region LD

                        else if (inst.Equals("ld", StringComparison.OrdinalIgnoreCase))
                        {
                            #region LD RegPair w/ Value

                            if (arg1Token.specifier == TokenType.REG_DOUBLE && arg2Token.specifier == TokenType.NUMBER && !arg1Token.tokenVal.Equals("af", StringComparison.OrdinalIgnoreCase))
                            {
                                returnedArray[returnedArrayOffset++] = (byte)(1 + (16 * arg1Token.tokenAsInteger));
                                returnedArray[returnedArrayOffset++] = (byte)arg2Token.tokenAsInteger;
                                returnedArray[returnedArrayOffset++] = (byte)(arg2Token.tokenAsInteger >> 8);
                                currentOffset += 3;
                            }

                            #endregion LD RegPair w/ Value

                            #region LD [nn],SP

                            else if (arg1Token.specifier == TokenType.MEMORY_REF_DIRECT && arg2Token.tokenVal.Equals("sp", StringComparison.OrdinalIgnoreCase))
                            {
                                returnedArray[returnedArrayOffset++] = 0x08;
                                returnedArray[returnedArrayOffset++] = (byte)arg1Token.tokenAsInteger;
                                returnedArray[returnedArrayOffset++] = (byte)(arg1Token.tokenAsInteger >> 8);
                                currentOffset += 3;
                            }

                            #endregion LD [nn],SP

                            #region LD r, n

                            else if (arg1Token.specifier == TokenType.REG_SINGLE && arg2Token.specifier == TokenType.NUMBER)
                            {
                                returnedArray[returnedArrayOffset++] = (byte)(0x06 + (8 * arg1Token.tokenAsInteger));
                                returnedArray[returnedArrayOffset++] = (byte)arg2Token.tokenAsInteger;
                                currentOffset += 2;
                            }

                            #endregion LD r, n

                            #region A as first arg

                            else if (arg1Token.tokenVal.Equals("a", StringComparison.OrdinalIgnoreCase))
                            {
                                #region LD A, [number]

                                if (arg2Token.specifier == TokenType.MEMORY_REF_DIRECT)
                                {
                                    if ((arg2Token.tokenAsInteger & 0xFFFF) < 0xFF00)
                                    {
                                        returnedArray[returnedArrayOffset++] = 0xFA;
                                        returnedArray[returnedArrayOffset++] = (byte)arg2Token.tokenAsInteger;
                                        returnedArray[returnedArrayOffset++] = (byte)(arg2Token.tokenAsInteger >> 8);
                                        currentOffset += 3;
                                    }
                                    else
                                    {
                                        returnedArray[returnedArrayOffset++] = 0xF0;
                                        returnedArray[returnedArrayOffset++] = (byte)arg2Token.tokenAsInteger;
                                        currentOffset += 2;
                                    }
                                }

                                #endregion LD A, [number]

                                else
                                {
                                    error = new CompError(lines[i], i + 1, ErrorMessage.SINGLE_ARG_UNRECOGNIZED, arg2Token.tokenVal);
                                    return null;
                                }
                            }

                            #endregion A as first arg

                            #region A as second arg

                            else if (arg2Token.tokenVal.Equals("a", StringComparison.OrdinalIgnoreCase))
                            {
                                #region LD [number], A

                                if (arg1Token.specifier == TokenType.MEMORY_REF_DIRECT)
                                {
                                    if ((arg1Token.tokenAsInteger & 0xFFFF) < 0xFF00)
                                    {
                                        returnedArray[returnedArrayOffset++] = 0xEA;
                                        returnedArray[returnedArrayOffset++] = (byte)arg1Token.tokenAsInteger;
                                        returnedArray[returnedArrayOffset++] = (byte)(arg1Token.tokenAsInteger >> 8);
                                        currentOffset += 3;
                                    }
                                    else
                                    {
                                        returnedArray[returnedArrayOffset++] = 0xE0;
                                        returnedArray[returnedArrayOffset++] = (byte)arg1Token.tokenAsInteger;
                                        currentOffset += 2;
                                    }
                                }

                                #endregion LD [number], A

                                else
                                {
                                    error = new CompError(lines[i], i + 1, ErrorMessage.SINGLE_ARG_UNRECOGNIZED, arg1Token.tokenVal);
                                    return null;
                                }
                            }

                            #endregion A as second arg

                            else
                            {
                                error = new CompError(lines[i], i + 1, ErrorMessage.DOUBLE_ARG_UNRECOGNIZED, arg1Token.tokenVal, arg2Token.tokenVal);
                                return null;
                            }
                        }

                        #endregion LD

                        #region Call/Jp/Jr

                        else if ((inst.Equals("call", StringComparison.OrdinalIgnoreCase) || inst.Equals("jp", StringComparison.OrdinalIgnoreCase) || inst.Equals("jr", StringComparison.OrdinalIgnoreCase)) && arg1Token.specifier == TokenType.CONDITION && (arg2Token.specifier == TokenType.LABEL || arg2Token.specifier == TokenType.NUMBER))
                        {
                            byte instByte;
                            if (inst.Equals("call", StringComparison.OrdinalIgnoreCase)) instByte = 0xC4;
                            else if (inst.Equals("jp", StringComparison.OrdinalIgnoreCase)) instByte = 0xC2;
                            else instByte = 0x20;
                            instByte += (byte)(8 * arg1Token.tokenAsInteger);
                            if (inst.Equals("jr", StringComparison.OrdinalIgnoreCase))
                            {
                                int diff = arg2Token.tokenAsInteger - (currentOffset + 2);
                                if (diff < -128 || diff > 127)
                                {
                                    error = new CompError(lines[i], i + 1, ErrorMessage.JR_OUT_OF_RANGE);
                                    return null;
                                }
                                else
                                {
                                    returnedArray[returnedArrayOffset++] = instByte;
                                    returnedArray[returnedArrayOffset++] = (byte)diff;
                                    currentOffset += 2;
                                }
                            }
                            else
                            {
                                returnedArray[returnedArrayOffset++] = instByte;
                                returnedArray[returnedArrayOffset++] = (byte)arg2Token.tokenAsInteger;
                                returnedArray[returnedArrayOffset++] = (byte)(arg2Token.tokenAsInteger >> 8);
                                currentOffset += 3;
                            }
                        }

                        #endregion Call/Jp/Jr

                        else
                        {
                            error = new CompError(lines[i], i + 1, ErrorMessage.DOUBLE_ARG_UNRECOGNIZED, arg1Token.tokenVal, arg2Token.tokenVal);
                            return null;
                        }
                    }

                    #endregion Two Arguments

                    else
                    {
                        error = new CompError(lines[i], i + 1, ErrorMessage.UNRECOGNIZED_LINE);
                        return null;
                    }
                }

                #endregion If line is instruction, or otherwise.
            }

            #endregion Adding Data

            success = true;
            return returnedArray;
        }

        private static char[] commentChars = new char[2]
        {
                  ';',
                  '#'
        };

        #region Assembly Keywords

        #region Static Arguments

        private static Dictionary<string, byte[]> staticArgs = new Dictionary<string, byte[]>
        {
            {"ccf", new byte[] { 0x3F } },
            {"cpl", new byte[] { 0x2F } },
            {"daa", new byte[] { 0x27 } },
            {"di", new byte[] { 0xF3 } },
            {"ei", new byte[] { 0xFB } },
            {"halt", new byte[] { 0x76 } },
            {"nop", new byte[] { 0x00 } },
            {"rla", new byte[] { 0x17 } },
            {"rlca", new byte[] { 0x07 } },
            {"rra", new byte[] { 0x1F } },
            {"rrca", new byte[] { 0x0F } },
            {"scf", new byte[] { 0x37 } },
            {"jp hl", new byte[] { 0xE9 } },
            {"rst $00", new byte[] { 0xC7 } },
            {"rst $08", new byte[] { 0xCF } },
            {"rst $10", new byte[] { 0xD7 } },
            {"rst $18", new byte[] { 0xDF } },
            {"rst $20", new byte[] { 0xE7 } },
            {"rst $28", new byte[] { 0xEF } },
            {"rst $30", new byte[] { 0xF7 } },
            {"rst $38", new byte[] { 0xFF } },
            {"ldi a,[hl]", new byte[] { 0x2A } },
            {"ldi [hl],a", new byte[] { 0x22 } },
            {"ldd a,[hl]", new byte[] { 0x3A } },
            {"ldd [hl],a", new byte[] { 0x32 } },
            {"ld a,[c]", new byte[] { 0xF2 } },
            {"ld a,[de]", new byte[] { 0x1A } },
            {"ld a,[bc]", new byte[] { 0x0A } },
            {"ld [c],a", new byte[] { 0xE2 } },
            {"ld [de],a", new byte[] { 0x12 } },
            {"ld [bc],a", new byte[] { 0x02 } },
            {"ld b,b", new byte[] { 0x40 } },
            {"ld b,c", new byte[] { 0x41 } },
            {"ld b,d", new byte[] { 0x42 } },
            {"ld b,e", new byte[] { 0x43 } },
            {"ld b,h", new byte[] { 0x44 } },
            {"ld b,l", new byte[] { 0x45 } },
            {"ld b,[hl]", new byte[] { 0x46 } },
            {"ld b,a", new byte[] { 0x47 } },
            {"ld c,b", new byte[] { 0x48 } },
            {"ld c,c", new byte[] { 0x49 } },
            {"ld c,d", new byte[] { 0x4A } },
            {"ld c,e", new byte[] { 0x4B } },
            {"ld c,h", new byte[] { 0x4C } },
            {"ld c,l", new byte[] { 0x4D } },
            {"ld c,[hl]", new byte[] { 0x4E } },
            {"ld c,a", new byte[] { 0x4F } },
            {"ld d,b", new byte[] { 0x50 } },
            {"ld d,c", new byte[] { 0x51 } },
            {"ld d,d", new byte[] { 0x52 } },
            {"ld d,e", new byte[] { 0x53 } },
            {"ld d,h", new byte[] { 0x54 } },
            {"ld d,l", new byte[] { 0x55 } },
            {"ld d,[hl]", new byte[] { 0x56 } },
            {"ld d,a", new byte[] { 0x57 } },
            {"ld e,b", new byte[] { 0x58 } },
            {"ld e,c", new byte[] { 0x59 } },
            {"ld e,d", new byte[] { 0x5A } },
            {"ld e,e", new byte[] { 0x5B } },
            {"ld e,h", new byte[] { 0x5C } },
            {"ld e,l", new byte[] { 0x5D } },
            {"ld e,[hl]", new byte[] { 0x5E } },
            {"ld e,a", new byte[] { 0x5F } },
            {"ld h,b", new byte[] { 0x60 } },
            {"ld h,c", new byte[] { 0x61 } },
            {"ld h,d", new byte[] { 0x62 } },
            {"ld h,e", new byte[] { 0x63 } },
            {"ld h,h", new byte[] { 0x64 } },
            {"ld h,l", new byte[] { 0x65 } },
            {"ld h,[hl]", new byte[] { 0x66 } },
            {"ld h,a", new byte[] { 0x67 } },
            {"ld l,b", new byte[] { 0x68 } },
            {"ld l,c", new byte[] { 0x69 } },
            {"ld l,d", new byte[] { 0x6A } },
            {"ld l,e", new byte[] { 0x6B } },
            {"ld l,h", new byte[] { 0x6C } },
            {"ld l,l", new byte[] { 0x6D } },
            {"ld l,[hl]", new byte[] { 0x6E } },
            {"ld l,a", new byte[] { 0x6F } },
            {"ld [hl],b", new byte[] { 0x70 } },
            {"ld [hl],c", new byte[] { 0x71 } },
            {"ld [hl],d", new byte[] { 0x72 } },
            {"ld [hl],e", new byte[] { 0x73 } },
            {"ld [hl],h", new byte[] { 0x74 } },
            {"ld [hl],l", new byte[] { 0x75 } },
            {"ld [hl],a", new byte[] { 0x77 } },
            {"ld a,b", new byte[] { 0x78 } },
            {"ld a,c", new byte[] { 0x79 } },
            {"ld a,d", new byte[] { 0x7A } },
            {"ld a,e", new byte[] { 0x7B } },
            {"ld a,h", new byte[] { 0x7C } },
            {"ld a,l", new byte[] { 0x7D } },
            {"ld a,[hl]", new byte[] { 0x7E } },
            {"ld a,a", new byte[] { 0x7F } },
            {"ld sp,hl", new byte[] { 0xF9 } },
            {"add b", new byte[] { 0x80 } },
            {"add c", new byte[] { 0x81 } },
            {"add d", new byte[] { 0x82 } },
            {"add e", new byte[] { 0x83 } },
            {"add h", new byte[] { 0x84 } },
            {"add l", new byte[] { 0x85 } },
            {"add [hl]", new byte[] { 0x86 } },
            {"add a", new byte[] { 0x87 } },
            {"add hl,bc", new byte[] { 0x09 } },
            {"add hl,de", new byte[] { 0x19 } },
            {"add hl,hl", new byte[] { 0x29 } },
            {"add hl,sp", new byte[] { 0x39 } },
            {"adc b", new byte[] { 0x88 } },
            {"adc c", new byte[] { 0x89 } },
            {"adc d", new byte[] { 0x8A } },
            {"adc e", new byte[] { 0x8B } },
            {"adc h", new byte[] { 0x8C } },
            {"adc l", new byte[] { 0x8D } },
            {"adc [hl]", new byte[] { 0x8E } },
            {"adc a", new byte[] { 0x8F } },
            {"and b", new byte[] { 0xA0 } },
            {"and c", new byte[] { 0xA1 } },
            {"and d", new byte[] { 0xA2 } },
            {"and e", new byte[] { 0xA3 } },
            {"and h", new byte[] { 0xA4 } },
            {"and l", new byte[] { 0xA5 } },
            {"and [hl]", new byte[] { 0xA6 } },
            {"and a", new byte[] { 0xA7 } },
            {"cp b", new byte[] { 0xB8 } },
            {"cp c", new byte[] { 0xB9 } },
            {"cp d", new byte[] { 0xBA } },
            {"cp e", new byte[] { 0xBB } },
            {"cp h", new byte[] { 0xBC } },
            {"cp l", new byte[] { 0xBD } },
            {"cp [hl]", new byte[] { 0xBE } },
            {"cp a", new byte[] { 0xBF } },
            {"dec b", new byte[] { 0x05 } },
            {"dec c", new byte[] { 0x0D } },
            {"dec d", new byte[] { 0x15 } },
            {"dec e", new byte[] { 0x1D } },
            {"dec h", new byte[] { 0x25 } },
            {"dec l", new byte[] { 0x2D } },
            {"dec [hl]", new byte[] { 0x35 } },
            {"dec a", new byte[] { 0x3D } },
            {"dec bc", new byte[] { 0x0B } },
            {"dec de", new byte[] { 0x1B } },
            {"dec hl", new byte[] { 0x2B } },
            {"dec sp", new byte[] { 0x3B } },
            {"inc b", new byte[] { 0x04 } },
            {"inc c", new byte[] { 0x0C } },
            {"inc d", new byte[] { 0x14 } },
            {"inc e", new byte[] { 0x1C } },
            {"inc h", new byte[] { 0x24 } },
            {"inc l", new byte[] { 0x2C } },
            {"inc [hl]", new byte[] { 0x34 } },
            {"inc a", new byte[] { 0x3C } },
            {"inc bc", new byte[] { 0x03 } },
            {"inc de", new byte[] { 0x13 } },
            {"inc hl", new byte[] { 0x23 } },
            {"inc sp", new byte[] { 0x33 } },
            {"or b", new byte[] { 0xB0 } },
            {"or c", new byte[] { 0xB1 } },
            {"or d", new byte[] { 0xB2 } },
            {"or e", new byte[] { 0xB3 } },
            {"or h", new byte[] { 0xB4 } },
            {"or l", new byte[] { 0xB5 } },
            {"or [hl]", new byte[] { 0xB6 } },
            {"or a", new byte[] { 0xB7 } },
            {"pop bc", new byte[] { 0xC1 } },
            {"pop de", new byte[] { 0xD1 } },
            {"pop hl", new byte[] { 0xE1 } },
            {"pop af", new byte[] { 0xF1 } },
            {"push bc", new byte[] { 0xC5 } },
            {"push de", new byte[] { 0xD5 } },
            {"push hl", new byte[] { 0xE5 } },
            {"push af", new byte[] { 0xF5 } },
            {"ret", new byte[] { 0xC9 } },
            {"ret nz", new byte[] { 0xC0 } },
            {"ret z", new byte[] { 0xC8 } },
            {"ret nc", new byte[] { 0xD0 } },
            {"ret c", new byte[] { 0xD8 } },
            {"reti", new byte[] { 0xD9 } },
            {"sub b", new byte[] { 0x90 } },
            {"sub c", new byte[] { 0x91 } },
            {"sub d", new byte[] { 0x92 } },
            {"sub e", new byte[] { 0x93 } },
            {"sub h", new byte[] { 0x94 } },
            {"sub l", new byte[] { 0x95 } },
            {"sub [hl]", new byte[] { 0x96 } },
            {"sub a", new byte[] { 0x97 } },
            {"sbc b", new byte[] { 0x98 } },
            {"sbc c", new byte[] { 0x99 } },
            {"sbc d", new byte[] { 0x9A } },
            {"sbc e", new byte[] { 0x9B } },
            {"sbc h", new byte[] { 0x9C } },
            {"sbc l", new byte[] { 0x9D } },
            {"sbc [hl]", new byte[] { 0x9E } },
            {"sbc a", new byte[] { 0x9F } },
            {"xor b", new byte[] { 0xA8 } },
            {"xor c", new byte[] { 0xA9 } },
            {"xor d", new byte[] { 0xAA } },
            {"xor e", new byte[] { 0xAB } },
            {"xor h", new byte[] { 0xAC } },
            {"xor l", new byte[] { 0xAD } },
            {"xor [hl]", new byte[] { 0xAE } },
            {"xor a", new byte[] { 0xAF } },
            {"stop", new byte[] { 0x10, 0x00 } },
            {"rlc b", new byte[] { 0xCB, 0x00 } },
            {"rlc c", new byte[] { 0xCB, 0x01 } },
            {"rlc d", new byte[] { 0xCB, 0x02 } },
            {"rlc e", new byte[] { 0xCB, 0x03 } },
            {"rlc h", new byte[] { 0xCB, 0x04 } },
            {"rlc l", new byte[] { 0xCB, 0x05 } },
            {"rlc [hl]", new byte[] { 0xCB, 0x06 } },
            {"rlc a", new byte[] { 0xCB, 0x07 } },
            {"rrc b", new byte[] { 0xCB, 0x08 } },
            {"rrc c", new byte[] { 0xCB, 0x09 } },
            {"rrc d", new byte[] { 0xCB, 0x0A } },
            {"rrc e", new byte[] { 0xCB, 0x0B } },
            {"rrc h", new byte[] { 0xCB, 0x0C } },
            {"rrc l", new byte[] { 0xCB, 0x0D } },
            {"rrc [hl]", new byte[] { 0xCB, 0x0E } },
            {"rrc a", new byte[] { 0xCB, 0x0F } },
            {"rl b", new byte[] { 0xCB, 0x10 } },
            {"rl c", new byte[] { 0xCB, 0x11 } },
            {"rl d", new byte[] { 0xCB, 0x12 } },
            {"rl e", new byte[] { 0xCB, 0x13 } },
            {"rl h", new byte[] { 0xCB, 0x14 } },
            {"rl l", new byte[] { 0xCB, 0x15 } },
            {"rl [hl]", new byte[] { 0xCB, 0x16 } },
            {"rl a", new byte[] { 0xCB, 0x17 } },
            {"rr b", new byte[] { 0xCB, 0x18 } },
            {"rr c", new byte[] { 0xCB, 0x19 } },
            {"rr d", new byte[] { 0xCB, 0x1A } },
            {"rr e", new byte[] { 0xCB, 0x1B } },
            {"rr h", new byte[] { 0xCB, 0x1C } },
            {"rr l", new byte[] { 0xCB, 0x1D } },
            {"rr [hl]", new byte[] { 0xCB, 0x1E } },
            {"rr a", new byte[] { 0xCB, 0x1F } },
            {"sla b", new byte[] { 0xCB, 0x20 } },
            {"sla c", new byte[] { 0xCB, 0x21 } },
            {"sla d", new byte[] { 0xCB, 0x22 } },
            {"sla e", new byte[] { 0xCB, 0x23 } },
            {"sla h", new byte[] { 0xCB, 0x24 } },
            {"sla l", new byte[] { 0xCB, 0x25 } },
            {"sla [hl]", new byte[] { 0xCB, 0x26 } },
            {"sla a", new byte[] { 0xCB, 0x27 } },
            {"sra b", new byte[] { 0xCB, 0x28 } },
            {"sra c", new byte[] { 0xCB, 0x29 } },
            {"sra d", new byte[] { 0xCB, 0x2A } },
            {"sra e", new byte[] { 0xCB, 0x2B } },
            {"sra h", new byte[] { 0xCB, 0x2C } },
            {"sra l", new byte[] { 0xCB, 0x2D } },
            {"sra [hl]", new byte[] { 0xCB, 0x2E } },
            {"sra a", new byte[] { 0xCB, 0x2F } },
            {"swap b", new byte[] { 0xCB, 0x30 } },
            {"swap c", new byte[] { 0xCB, 0x31 } },
            {"swap d", new byte[] { 0xCB, 0x32 } },
            {"swap e", new byte[] { 0xCB, 0x33 } },
            {"swap h", new byte[] { 0xCB, 0x34 } },
            {"swap l", new byte[] { 0xCB, 0x35 } },
            {"swap [hl]", new byte[] { 0xCB, 0x36 } },
            {"swap a", new byte[] { 0xCB, 0x37 } },
            {"srl b", new byte[] { 0xCB, 0x38 } },
            {"srl c", new byte[] { 0xCB, 0x39 } },
            {"srl d", new byte[] { 0xCB, 0x3A } },
            {"srl e", new byte[] { 0xCB, 0x3B } },
            {"srl h", new byte[] { 0xCB, 0x3C } },
            {"srl l", new byte[] { 0xCB, 0x3D } },
            {"srl [hl]", new byte[] { 0xCB, 0x3E } },
            {"srl a", new byte[] { 0xCB, 0x3F } },
            {"bit 0,b", new byte[] { 0xCB, 0x40 } },
            {"bit 0,c", new byte[] { 0xCB, 0x41 } },
            {"bit 0,d", new byte[] { 0xCB, 0x42 } },
            {"bit 0,e", new byte[] { 0xCB, 0x43 } },
            {"bit 0,h", new byte[] { 0xCB, 0x44 } },
            {"bit 0,l", new byte[] { 0xCB, 0x45 } },
            {"bit 0,[hl]", new byte[] { 0xCB, 0x46 } },
            {"bit 0,a", new byte[] { 0xCB, 0x47 } },
            {"bit 1,b", new byte[] { 0xCB, 0x48 } },
            {"bit 1,c", new byte[] { 0xCB, 0x49 } },
            {"bit 1,d", new byte[] { 0xCB, 0x4A } },
            {"bit 1,e", new byte[] { 0xCB, 0x4B } },
            {"bit 1,h", new byte[] { 0xCB, 0x4C } },
            {"bit 1,l", new byte[] { 0xCB, 0x4D } },
            {"bit 1,[hl]", new byte[] { 0xCB, 0x4E } },
            {"bit 1,a", new byte[] { 0xCB, 0x4F } },
            {"bit 2,b", new byte[] { 0xCB, 0x50 } },
            {"bit 2,c", new byte[] { 0xCB, 0x51 } },
            {"bit 2,d", new byte[] { 0xCB, 0x52 } },
            {"bit 2,e", new byte[] { 0xCB, 0x53 } },
            {"bit 2,h", new byte[] { 0xCB, 0x54 } },
            {"bit 2,l", new byte[] { 0xCB, 0x55 } },
            {"bit 2,[hl]", new byte[] { 0xCB, 0x56 } },
            {"bit 2,a", new byte[] { 0xCB, 0x57 } },
            {"bit 3,b", new byte[] { 0xCB, 0x58 } },
            {"bit 3,c", new byte[] { 0xCB, 0x59 } },
            {"bit 3,d", new byte[] { 0xCB, 0x5A } },
            {"bit 3,e", new byte[] { 0xCB, 0x5B } },
            {"bit 3,h", new byte[] { 0xCB, 0x5C } },
            {"bit 3,l", new byte[] { 0xCB, 0x5D } },
            {"bit 3,[hl]", new byte[] { 0xCB, 0x5E } },
            {"bit 3,a", new byte[] { 0xCB, 0x5F } },
            {"bit 4,b", new byte[] { 0xCB, 0x60 } },
            {"bit 4,c", new byte[] { 0xCB, 0x61 } },
            {"bit 4,d", new byte[] { 0xCB, 0x62 } },
            {"bit 4,e", new byte[] { 0xCB, 0x63 } },
            {"bit 4,h", new byte[] { 0xCB, 0x64 } },
            {"bit 4,l", new byte[] { 0xCB, 0x65 } },
            {"bit 4,[hl]", new byte[] { 0xCB, 0x66 } },
            {"bit 4,a", new byte[] { 0xCB, 0x67 } },
            {"bit 5,b", new byte[] { 0xCB, 0x68 } },
            {"bit 5,c", new byte[] { 0xCB, 0x69 } },
            {"bit 5,d", new byte[] { 0xCB, 0x6A } },
            {"bit 5,e", new byte[] { 0xCB, 0x6B } },
            {"bit 5,h", new byte[] { 0xCB, 0x6C } },
            {"bit 5,l", new byte[] { 0xCB, 0x6D } },
            {"bit 5,[hl]", new byte[] { 0xCB, 0x6E } },
            {"bit 5,a", new byte[] { 0xCB, 0x6F } },
            {"bit 6,b", new byte[] { 0xCB, 0x70 } },
            {"bit 6,c", new byte[] { 0xCB, 0x71 } },
            {"bit 6,d", new byte[] { 0xCB, 0x72 } },
            {"bit 6,e", new byte[] { 0xCB, 0x73 } },
            {"bit 6,h", new byte[] { 0xCB, 0x74 } },
            {"bit 6,l", new byte[] { 0xCB, 0x75 } },
            {"bit 6,[hl]", new byte[] { 0xCB, 0x76 } },
            {"bit 6,a", new byte[] { 0xCB, 0x77 } },
            {"bit 7,b", new byte[] { 0xCB, 0x78 } },
            {"bit 7,c", new byte[] { 0xCB, 0x79 } },
            {"bit 7,d", new byte[] { 0xCB, 0x7A } },
            {"bit 7,e", new byte[] { 0xCB, 0x7B } },
            {"bit 7,h", new byte[] { 0xCB, 0x7C } },
            {"bit 7,l", new byte[] { 0xCB, 0x7D } },
            {"bit 7,[hl]", new byte[] { 0xCB, 0x7E } },
            {"bit 7,a", new byte[] { 0xCB, 0x7F } },
            {"res 0,b", new byte[] { 0xCB, 0x80 } },
            {"res 0,c", new byte[] { 0xCB, 0x81 } },
            {"res 0,d", new byte[] { 0xCB, 0x82 } },
            {"res 0,e", new byte[] { 0xCB, 0x83 } },
            {"res 0,h", new byte[] { 0xCB, 0x84 } },
            {"res 0,l", new byte[] { 0xCB, 0x85 } },
            {"res 0,[hl]", new byte[] { 0xCB, 0x86 } },
            {"res 0,a", new byte[] { 0xCB, 0x87 } },
            {"res 1,b", new byte[] { 0xCB, 0x88 } },
            {"res 1,c", new byte[] { 0xCB, 0x89 } },
            {"res 1,d", new byte[] { 0xCB, 0x8A } },
            {"res 1,e", new byte[] { 0xCB, 0x8B } },
            {"res 1,h", new byte[] { 0xCB, 0x8C } },
            {"res 1,l", new byte[] { 0xCB, 0x8D } },
            {"res 1,[hl]", new byte[] { 0xCB, 0x8E } },
            {"res 1,a", new byte[] { 0xCB, 0x8F } },
            {"res 2,b", new byte[] { 0xCB, 0x90 } },
            {"res 2,c", new byte[] { 0xCB, 0x91 } },
            {"res 2,d", new byte[] { 0xCB, 0x92 } },
            {"res 2,e", new byte[] { 0xCB, 0x93 } },
            {"res 2,h", new byte[] { 0xCB, 0x94 } },
            {"res 2,l", new byte[] { 0xCB, 0x95 } },
            {"res 2,[hl]", new byte[] { 0xCB, 0x96 } },
            {"res 2,a", new byte[] { 0xCB, 0x97 } },
            {"res 3,b", new byte[] { 0xCB, 0x98 } },
            {"res 3,c", new byte[] { 0xCB, 0x99 } },
            {"res 3,d", new byte[] { 0xCB, 0x9A } },
            {"res 3,e", new byte[] { 0xCB, 0x9B } },
            {"res 3,h", new byte[] { 0xCB, 0x9C } },
            {"res 3,l", new byte[] { 0xCB, 0x9D } },
            {"res 3,[hl]", new byte[] { 0xCB, 0x9E } },
            {"res 3,a", new byte[] { 0xCB, 0x9F } },
            {"res 4,b", new byte[] { 0xCB, 0xA0 } },
            {"res 4,c", new byte[] { 0xCB, 0xA1 } },
            {"res 4,d", new byte[] { 0xCB, 0xA2 } },
            {"res 4,e", new byte[] { 0xCB, 0xA3 } },
            {"res 4,h", new byte[] { 0xCB, 0xA4 } },
            {"res 4,l", new byte[] { 0xCB, 0xA5 } },
            {"res 4,[hl]", new byte[] { 0xCB, 0xA6 } },
            {"res 4,a", new byte[] { 0xCB, 0xA7 } },
            {"res 5,b", new byte[] { 0xCB, 0xA8 } },
            {"res 5,c", new byte[] { 0xCB, 0xA9 } },
            {"res 5,d", new byte[] { 0xCB, 0xAA } },
            {"res 5,e", new byte[] { 0xCB, 0xAB } },
            {"res 5,h", new byte[] { 0xCB, 0xAC } },
            {"res 5,l", new byte[] { 0xCB, 0xAD } },
            {"res 5,[hl]", new byte[] { 0xCB, 0xAE } },
            {"res 5,a", new byte[] { 0xCB, 0xAF } },
            {"res 6,b", new byte[] { 0xCB, 0xB0 } },
            {"res 6,c", new byte[] { 0xCB, 0xB1 } },
            {"res 6,d", new byte[] { 0xCB, 0xB2 } },
            {"res 6,e", new byte[] { 0xCB, 0xB3 } },
            {"res 6,h", new byte[] { 0xCB, 0xB4 } },
            {"res 6,l", new byte[] { 0xCB, 0xB5 } },
            {"res 6,[hl]", new byte[] { 0xCB, 0xB6 } },
            {"res 6,a", new byte[] { 0xCB, 0xB7 } },
            {"res 7,b", new byte[] { 0xCB, 0xB8 } },
            {"res 7,c", new byte[] { 0xCB, 0xB9 } },
            {"res 7,d", new byte[] { 0xCB, 0xBA } },
            {"res 7,e", new byte[] { 0xCB, 0xBB } },
            {"res 7,h", new byte[] { 0xCB, 0xBC } },
            {"res 7,l", new byte[] { 0xCB, 0xBD } },
            {"res 7,[hl]", new byte[] { 0xCB, 0xBE } },
            {"res 7,a", new byte[] { 0xCB, 0xBF } },
            {"set 0,b", new byte[] { 0xCB, 0xC0 } },
            {"set 0,c", new byte[] { 0xCB, 0xC1 } },
            {"set 0,d", new byte[] { 0xCB, 0xC2 } },
            {"set 0,e", new byte[] { 0xCB, 0xC3 } },
            {"set 0,h", new byte[] { 0xCB, 0xC4 } },
            {"set 0,l", new byte[] { 0xCB, 0xC5 } },
            {"set 0,[hl]", new byte[] { 0xCB, 0xC6 } },
            {"set 0,a", new byte[] { 0xCB, 0xC7 } },
            {"set 1,b", new byte[] { 0xCB, 0xC8 } },
            {"set 1,c", new byte[] { 0xCB, 0xC9 } },
            {"set 1,d", new byte[] { 0xCB, 0xCA } },
            {"set 1,e", new byte[] { 0xCB, 0xCB } },
            {"set 1,h", new byte[] { 0xCB, 0xCC } },
            {"set 1,l", new byte[] { 0xCB, 0xCD } },
            {"set 1,[hl]", new byte[] { 0xCB, 0xCE } },
            {"set 1,a", new byte[] { 0xCB, 0xCF } },
            {"set 2,b", new byte[] { 0xCB, 0xD0 } },
            {"set 2,c", new byte[] { 0xCB, 0xD1 } },
            {"set 2,d", new byte[] { 0xCB, 0xD2 } },
            {"set 2,e", new byte[] { 0xCB, 0xD3 } },
            {"set 2,h", new byte[] { 0xCB, 0xD4 } },
            {"set 2,l", new byte[] { 0xCB, 0xD5 } },
            {"set 2,[hl]", new byte[] { 0xCB, 0xD6 } },
            {"set 2,a", new byte[] { 0xCB, 0xD7 } },
            {"set 3,b", new byte[] { 0xCB, 0xD8 } },
            {"set 3,c", new byte[] { 0xCB, 0xD9 } },
            {"set 3,d", new byte[] { 0xCB, 0xDA } },
            {"set 3,e", new byte[] { 0xCB, 0xDB } },
            {"set 3,h", new byte[] { 0xCB, 0xDC } },
            {"set 3,l", new byte[] { 0xCB, 0xDD } },
            {"set 3,[hl]", new byte[] { 0xCB, 0xDE } },
            {"set 3,a", new byte[] { 0xCB, 0xDF } },
            {"set 4,b", new byte[] { 0xCB, 0xE0 } },
            {"set 4,c", new byte[] { 0xCB, 0xE1 } },
            {"set 4,d", new byte[] { 0xCB, 0xE2 } },
            {"set 4,e", new byte[] { 0xCB, 0xE3 } },
            {"set 4,h", new byte[] { 0xCB, 0xE4 } },
            {"set 4,l", new byte[] { 0xCB, 0xE5 } },
            {"set 4,[hl]", new byte[] { 0xCB, 0xE6 } },
            {"set 4,a", new byte[] { 0xCB, 0xE7 } },
            {"set 5,b", new byte[] { 0xCB, 0xE8 } },
            {"set 5,c", new byte[] { 0xCB, 0xE9 } },
            {"set 5,d", new byte[] { 0xCB, 0xEA } },
            {"set 5,e", new byte[] { 0xCB, 0xEB } },
            {"set 5,h", new byte[] { 0xCB, 0xEC } },
            {"set 5,l", new byte[] { 0xCB, 0xED } },
            {"set 5,[hl]", new byte[] { 0xCB, 0xEE } },
            {"set 5,a", new byte[] { 0xCB, 0xEF } },
            {"set 6,b", new byte[] { 0xCB, 0xF0 } },
            {"set 6,c", new byte[] { 0xCB, 0xF1 } },
            {"set 6,d", new byte[] { 0xCB, 0xF2 } },
            {"set 6,e", new byte[] { 0xCB, 0xF3 } },
            {"set 6,h", new byte[] { 0xCB, 0xF4 } },
            {"set 6,l", new byte[] { 0xCB, 0xF5 } },
            {"set 6,[hl]", new byte[] { 0xCB, 0xF6 } },
            {"set 6,a", new byte[] { 0xCB, 0xF7 } },
            {"set 7,b", new byte[] { 0xCB, 0xF8 } },
            {"set 7,c", new byte[] { 0xCB, 0xF9 } },
            {"set 7,d", new byte[] { 0xCB, 0xFA } },
            {"set 7,e", new byte[] { 0xCB, 0xFB } },
            {"set 7,h", new byte[] { 0xCB, 0xFC } },
            {"set 7,l", new byte[] { 0xCB, 0xFD } },
            {"set 7,[hl]", new byte[] { 0xCB, 0xFE } },
            {"set 7,a", new byte[] { 0xCB, 0xFF } }
        };

        #endregion Static Arguments

        public static string[] arithmeticArgs = new string[]
        {
            "adc",
            "add",
            "and",
            "cp",
            "or",
            "sbc",
            "sub",
            "xor"
        };

        #region Single Argument Instructions

        static Dictionary<string, byte> singleIntArgBytes = new Dictionary<string, byte>
        {
            {"add", 0xC6},
            {"adc", 0xCE},
            {"and", 0xE6},
            {"call", 0xCD},
            {"cp", 0xFE},
            {"jp", 0xC3},
            {"jr", 0x18},
            {"or", 0xF6},
            {"sbc", 0xDE},
            {"sub", 0xD6},
            {"xor", 0xEE}
        };

        #endregion Single Argument Instructions

        #region Two Argument Instructions

        static string[] doubleArgumentInst = new string[]
        {
            "add",
            "call",
            "jp",
            "jr",
            "ld",
            "ldhl"
        };

        #endregion Two Argument Instructions

        #region Register Identification

        static Dictionary<string, byte> regSingleBytes = new Dictionary<string, byte>
        {
            {"b", 0},
            {"c", 1},
            {"d", 2},
            {"e", 3},
            {"h", 4},
            {"l", 5},
            {"[hl]", 6},
            {"a", 7}
        };

        static Dictionary<string, byte> conditionBytes = new Dictionary<string, byte>
        {
            {"nz", 0},
            {"z", 1},
            {"nc", 2},
            {"c", 3}
        };

        static Dictionary<string, byte> regDoubleBytes = new Dictionary<string, byte>
        {
            {"bc", 0},
            {"de", 1},
            {"hl", 2},
            {"af", 3},
            {"sp", 3}
        };

        #endregion Register Identification

        public enum TokenType
        {
            EQUAL, 
            COMMA, 
            CONDITION,
            INSTRUCTION,
            LABEL,
            MEMORY_REF_DIRECT,
            MEMORY_REF_REG,
            MEMORY_REF_HL,
            NUMBER,
            REG_SINGLE,
            REG_DOUBLE,
            NONE,
            UNKNOWN,
            UNKNOWN_MEMORY_REF
        }

        #endregion Assembly Keywords
    }
}