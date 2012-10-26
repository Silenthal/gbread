namespace GBRead.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using LibGBasm;

    public class Disassembler
    {
        private BinFile CoreFile;

        private LabelContainer lc;

        #region Options

        public bool PrintOffsets { get; set; }

        public bool PrintBitPattern { get; set; }

        public OffsetFormat PrintedOffsetFormat { get; set; }

        public OffsetFormat InstructionNumberFormat { get; set; }

        public bool PrintComments { get; set; }

        public bool HideDefinedFunctions { get; set; }

        public bool HideDefinedData { get; set; }

        #endregion Options

        public Disassembler(BinFile cs, LabelContainer lcs)
        {
            CoreFile = cs;
            lc = lcs;
            PrintOffsets = true;
            PrintedOffsetFormat = OffsetFormat.BankOffset;
            InstructionNumberFormat = OffsetFormat.Hex;
            PrintBitPattern = true;
            PrintComments = false;
            HideDefinedData = false;
            HideDefinedFunctions = false;
        }

        #region Getting and Setting Options

        public void GetOptions(Options options)
        {
            PrintOffsets = options.Disassembler_PrintOffsets;
            PrintBitPattern = options.Disassembler_PrintBitPattern;
            PrintedOffsetFormat = options.Disassembler_PrintedOffsetFormat;
            InstructionNumberFormat = options.Disassembler_InstructionNumberFormat;
            PrintComments = options.Disassembler_PrintComments;
            HideDefinedFunctions = options.Disassembler_HideDefinedFunctions;
            HideDefinedData = options.Disassembler_HideDefinedData;
        }

        public void SetOptions(ref Options options)
        {
            options.Disassembler_PrintOffsets = PrintOffsets;
            options.Disassembler_PrintBitPattern = PrintBitPattern;
            options.Disassembler_PrintedOffsetFormat = PrintedOffsetFormat;
            options.Disassembler_InstructionNumberFormat = InstructionNumberFormat;
            options.Disassembler_PrintComments = PrintComments;
            options.Disassembler_HideDefinedFunctions = HideDefinedFunctions;
            options.Disassembler_HideDefinedData = HideDefinedData;
        }

        #endregion Getting and Setting Options

        #region Label Display

        // TODO: Handle the case where the DL goes out of bounds.
        public string ShowDataLabel(DataLabel dataLabel)
        {
            StringBuilder ret = new StringBuilder(String.Empty);
            ret.AppendLine(dataLabel.ToASMCommentString());
            var labelsIn = (
                from s in lc.FuncList
                where s.Offset >= dataLabel.Offset && s.Offset < dataLabel.Offset + dataLabel.Length
                select new { Name = s.Name, Offset = s.Offset, Comment = s.ToASMCommentString() })
                .Concat(
                from s in lc.DataList
                where s.Offset > dataLabel.Offset && s.Offset < dataLabel.Offset + dataLabel.Length
                select new { Name = s.Name, Offset = s.Offset, Comment = s.ToASMCommentString() })
                .OrderBy(s => s.Offset)
                .ThenBy(s => s.Name);
            int currentLoc = dataLabel.Offset;
            foreach (var label in labelsIn)
            {
                ret.Append(GetDBSection(currentLoc, label.Offset - currentLoc, dataLabel.DataLineLength));
                ret.AppendLine(label.Comment);
                currentLoc = label.Offset;
            }
            ret.Append(GetDBSection(currentLoc, dataLabel.Length, dataLabel.DataLineLength));
            return ret.ToString();
        }

        public string ShowFuncLabel(FunctionLabel cLabel)
        {
            return PrintASM(cLabel.Offset, GuessLabelPrintLength(cLabel.Offset));
        }

        public string ShowVarLabel(VarLabel vLabel)
        {
            StringBuilder ret = new StringBuilder();
            ret.AppendLine(vLabel.ToDisplayString());
            ret.AppendLine(SearchForReference(vLabel));
            return ret.ToString();
        }

        private string GetDBSection(int off, int len, int lineSize)
        {
            if (len == 0)
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            int afterLast = off + len;
            for (int currentOffset = off; currentOffset < afterLast; currentOffset += lineSize)
            {
                int dbCount = currentOffset + lineSize > afterLast ? afterLast - currentOffset : lineSize;
                int last = currentOffset + dbCount - 1;
                sb.Append("    db ");
                for (int curOff = currentOffset; curOff < last; curOff++)
                {
                    byte curByte = (byte)CoreFile.ReadByte(curOff);
                    switch (InstructionNumberFormat)
                    {
                        case OffsetFormat.Decimal:
                            sb.Append(curByte.ToString() + ", ");
                            break;

                        default:
                            sb.Append("$" + curByte.ToString("X2") + ", ");
                            break;
                    }
                }
                switch (InstructionNumberFormat)
                {
                    case OffsetFormat.Decimal:
                        sb.Append(((byte)CoreFile.ReadByte(last)).ToString());
                        break;

                    default:
                        sb.Append("$" + ((byte)CoreFile.ReadByte(last)).ToString("X2"));
                        break;
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        #endregion Label Display

        #region Disassembly

        public string PrintASM(int start, int length)
        {
            return PrintASM(CoreFile, 0, start, length);
        }

        public string PrintASM(BinFile file, int baseOffset, int start, int length)
        {
            StringBuilder output = new StringBuilder();
            GBInstruction isu = new GBInstruction();
            int current = start;
            while (current < start + length)
            {
                var labelsAt = from s in lc.FuncList
                               where s.Offset == current
                               orderby s.Name
                               select s;
                var dataAt = from s in lc.DataList
                             where s.Offset == current
                             select s;
                foreach (var label in labelsAt)
                {
                    output.AppendLine(label.ToASMCommentString());
                }
                int advanceBy = 0;
                if (dataAt.Count() != 0)
                {
                    if (HideDefinedData)
                    {
                        output.AppendLine(String.Format("INCBIN \"{0}.bin\"", dataAt.First().Name));
                    }
                    else
                    {
                        output.Append(ShowDataLabel(dataAt.First()));
                    }
                    advanceBy = dataAt.First().Length;
                }
                else
                {
                    output.AppendLine(GetInstruction(file, baseOffset, current, ref isu));
                    advanceBy = isu.InstSize;
                }
                if (lc.Comments.ContainsKey(current))
                {
                    output.AppendLine(";" + lc.Comments[current].Replace("\n", "\n;"));
                }
                current += advanceBy;
            }
            return output.ToString();
        }

        private string GetInstruction(BinFile refFile, int OrgOffset, int BinaryOffset, ref GBInstruction isu)
        {
            int currentAddress = OrgOffset + BinaryOffset;
            if (lc.isAddressMarkedAsData(currentAddress))
            {
                GBASM.CreateDBInstruction(refFile.MainFile, OrgOffset, BinaryOffset, ref isu);
            }
            else
            {
                GBASM.GetInstruction(refFile.MainFile, OrgOffset, BinaryOffset, ref isu);
            }
            StringBuilder returned = new StringBuilder();

            if (PrintOffsets || PrintBitPattern)
            {
                returned.Append("/*");
                if (PrintOffsets)
                {
                    returned.Append(AddressToString(isu));
                }
                if (PrintBitPattern)
                {
                    var bp = new string[] { "  ", "  ", "  ", "  " };
                    for (int i = 0; i < isu.InstSize; i++)
                    {
                        bp[i] = refFile.ReadByte(BinaryOffset + i).ToString("X2");
                    }
                    returned.Append(string.Join("", bp));
                }
                returned.Append("*/ ");
            }
            else
            {
                returned.Append("    ");
            }

            returned.Append(isu.InstType.ToString());
            string numArg = "";
            if (isu.ArgCount > 0)
            {
                returned.Append(" ");
                numArg = ArgumentToString(isu.Bank, isu.Address, isu.InstType, isu.Arg1);
                returned.Append(numArg);
            }
            if (isu.ArgCount == 2)
            {
                returned.Append(",");
                numArg = ArgumentToString(isu.Bank, isu.Address, isu.InstType, isu.Arg2);
                returned.Append(numArg);
            }

            #region Check comments

            if (PrintComments)
            {
                returned.Append("  ;");
                int x = refFile.ReadByte(BinaryOffset);
                if (refFile.ReadByte(BinaryOffset) == 0xCB)
                {
                    returned.AppendFormat(CBLongInst[x], numArg);
                }
                else
                {
                    returned.AppendFormat(longInst[x], numArg);
                }
            }

            #endregion Check comments

            return returned.ToString();
        }

        public string GetFullASM()
        {
            StringBuilder ot = new StringBuilder();
            bool tempHFVal, tempHDVal, tempPBVal, tempPOVal;
            tempHFVal = HideDefinedFunctions;
            tempHDVal = HideDefinedData;
            tempPBVal = PrintBitPattern;
            tempPOVal = PrintOffsets;

            HideDefinedFunctions = false;
            HideDefinedData = false;
            PrintBitPattern = false;
            PrintOffsets = false;

            foreach (VarLabel v in lc.VarList)
            {
                ot.AppendLine(v.Name + " EQU $" + v.Variable.ToString("X"));
            }
            ot.AppendLine(PrintASM(0, CoreFile.Length));

            HideDefinedFunctions = tempHFVal;
            HideDefinedData = tempHDVal;
            PrintBitPattern = tempPBVal;
            PrintOffsets = tempPOVal;

            return ot.ToString();
        }

        #endregion Disassembly

        #region Searching

        public enum SearchOptions { InFunctions, InFile }

        [Flags]
        private enum FuncRefType { None = 0x0, Call = 0x1, Jump = 0x2 }

        [Flags]
        private enum VarRefType { None = 0x0, Read = 0x1, Write = 0x2, Ref = 0x4 };

        public void AutoPopulateFunctionList()
        {
            int currentOffset = 0;
            while (currentOffset < CoreFile.Length)
            {
                if (lc.isAddressMarkedAsData(currentOffset))
                {
                    currentOffset = lc.GetNextNonDataAddress(currentOffset);
                    continue;
                }

                GBInstruction isu = new GBInstruction();
                if (!GBASM.GetInstruction(CoreFile.MainFile, 0, currentOffset, ref isu))
                {
                    return;
                }

                if (isu.InstType == InstructionType.call)
                {
                    ushort curCallAddress = (isu.ArgCount == 1) ? isu.Arg1.NumArg : isu.Arg2.NumArg;
                    if (curCallAddress < 0x4000)
                    {
                        lc.AddFuncLabel(new FunctionLabel(curCallAddress));
                    }
                    else if (currentOffset >= 0x4000)
                    {
                        int curAdjustedCallAddress = Utility.GetRealAddress(isu.Bank, curCallAddress);
                        lc.AddFuncLabel(new FunctionLabel(curAdjustedCallAddress));
                    }
                }

                currentOffset += isu.InstSize;
            }
        }

        public string SearchForFunctionCall(FunctionLabel labelName)
        {
            StringBuilder returned = new StringBuilder();
            var ifResult = SearchForFunctionCall(labelName, SearchOptions.InFunctions);
            var ofResult = SearchForFunctionCall(labelName, SearchOptions.InFile);

            string functionUsedMessage = "{0} was referred to near these labels:{1}";
            string offsetUsedMessage = "{0} was referred to at these offsets:{1}";

            if (ifResult.Count == 0)
            {
                returned.AppendLine(labelName.Name + " was not referred to near any of these labels.");
            }
            else
            {
                returned.AppendFormat(functionUsedMessage, labelName.Name, Environment.NewLine);
                foreach (var kvp in ifResult)
                {
                    returned.AppendFormat("{0} ({1}){2}", kvp.Key, kvp.Value.ToString(), Environment.NewLine);
                }
            }

            if (ofResult.Count == 0)
            {
                returned.AppendLine(labelName.Name + " was not referred to at any offset in disassembly.");
            }
            else
            {
                returned.AppendFormat(offsetUsedMessage, labelName.Name, Environment.NewLine);
                foreach (var kvp in ofResult)
                {
                    returned.AppendFormat("{0} ({1}){2}", kvp.Key, kvp.Value.ToString(), Environment.NewLine);
                }
            }
            return returned.ToString();
        }

        private Dictionary<string, FuncRefType> SearchForFunctionCall(FunctionLabel searchLabel, SearchOptions options)
        {
            var results = new Dictionary<string, FuncRefType>();
            var searched = new HashSet<int>();
            if (options == SearchOptions.InFile)
            {
                SearchFileRangeForFunctionCall(results, 0x0, CoreFile.Length, searchLabel);
                return results;
            }
            else
            {
                foreach (FunctionLabel c in lc.FuncList)
                {
                    FuncRefType refType = FuncRefType.None;
                    int currentOffset = c.Offset;
                    if (searched.Contains(currentOffset))
                    {
                        continue;
                    }
                    else
                    {
                        searched.Add(currentOffset);
                    }
                    int curLen = GuessLabelPrintLength(c.Offset);
                    while (currentOffset < c.Offset + curLen)
                    {
                        if (currentOffset < 0x4000 && searchLabel.Offset > 0x3FFF)
                        {
                            break;
                        }
                        if (lc.isAddressMarkedAsData(currentOffset))
                        {
                            currentOffset = lc.GetNextNonDataAddress(currentOffset);
                            continue;
                        }
                        GBInstruction isu = new GBInstruction();
                        if (!GBASM.GetInstruction(CoreFile.MainFile, 0, currentOffset, ref isu))
                            break;
                        if (isu.InstType == InstructionType.call || isu.InstType == InstructionType.jp || isu.InstType == InstructionType.jr)
                        {
                            ushort calledAddress = isu.ArgCount == 1 ? isu.Arg1.NumArg : isu.Arg2.NumArg;
                            if (!(currentOffset < 0x4000 && calledAddress > 0x3FFF))
                            {
                                int calledNum = Utility.GetRealAddress(isu.Bank, calledAddress);
                                if (searchLabel.Offset == calledNum)
                                {
                                    if (isu.InstType == InstructionType.call)
                                        refType |= FuncRefType.Call;
                                    else
                                        refType |= FuncRefType.Jump;
                                }
                            }
                        }
                        currentOffset += isu.InstSize;
                    }
                    if (refType != FuncRefType.None)
                        results.Add(c.Name, refType);
                }
                return results;
            }
        }

        private void SearchFileRangeForFunctionCall(Dictionary<string, FuncRefType> results, int startingOffset, int length, FunctionLabel searchLabel)
        {
            if (startingOffset < 0x4000 && searchLabel.Offset > 0x3FFF)
            {
                return;
            }
            int currentOffset = startingOffset;
            while (currentOffset < startingOffset + length)
            {
                if (lc.isAddressMarkedAsData(currentOffset))
                {
                    currentOffset = lc.GetNextNonDataAddress(currentOffset);
                    continue;
                }
                GBInstruction isu = new GBInstruction();
                if (!GBASM.GetInstruction(CoreFile.MainFile, 0, currentOffset, ref isu))
                    break;
                if (isu.InstType == InstructionType.call || isu.InstType == InstructionType.jp || isu.InstType == InstructionType.jr)
                {
                    ushort calledAddress = isu.ArgCount == 1 ? isu.Arg1.NumArg : isu.Arg2.NumArg;
                    if (!(calledAddress < 0x4000 && currentOffset > 0x3FFF))
                    {
                        int calledNum = Utility.GetRealAddress(isu.Bank, calledAddress);
                        if (searchLabel.Offset == calledNum)
                        {
                            if (isu.InstType == InstructionType.call)
                                results.Add(currentOffset.ToString("X"), FuncRefType.Call);
                            else
                                results.Add(currentOffset.ToString("X"), FuncRefType.Call);
                        }
                    }
                }
                currentOffset += isu.InstSize;
            }
        }

        public string SearchForReference(GenericLabel search)
        {
            StringBuilder returned = new StringBuilder(String.Empty);
            var funcRefsNew = SearchFileForVarReference(search.Value, SearchOptions.InFunctions);
            var offRefsNew = SearchFileForVarReference(search.Value, SearchOptions.InFile);
            string functionUsedMessage = "{0} was used near these labels:{1}";
            string offsetUsedMessage = "{0} was used at these offsets:{1}";

            #region Var Refs in Functions

            if (funcRefsNew.Count == 0)
            {
                returned.AppendFormat("{0} is not referred to in any of the defined functions.{1}", search.Name, Environment.NewLine);
            }
            else
            {
                returned.AppendFormat(functionUsedMessage, search.Name, Environment.NewLine);
                foreach (KeyValuePair<string, VarRefType> kvp in funcRefsNew)
                {
                    returned.AppendFormat("{0} ({1}){2}", kvp.Key, kvp.Value.ToString("G"), Environment.NewLine);
                }
            }

            #endregion Var Refs in Functions

            #region Var Refs out of functions

            if (offRefsNew.Count == 0)
            {
                returned.AppendFormat("{0} could not be found at any offset.{1}", search.Name, Environment.NewLine);
            }
            else
            {
                returned.AppendFormat(offsetUsedMessage, search.Name, Environment.NewLine);
                foreach (KeyValuePair<string, VarRefType> kvp in offRefsNew)
                {
                    returned.AppendFormat("{0} ({1}){2}", kvp.Key, kvp.Value.ToString("G"), Environment.NewLine);
                }
            }

            #endregion Var Refs out of functions

            return returned.ToString();
        }

        private Dictionary<string, VarRefType> SearchFileForVarReference(int searchWord, SearchOptions options)
        {
            Dictionary<string, VarRefType> results = new Dictionary<string, VarRefType>();
            if (options == SearchOptions.InFile)
            {
                SearchFileRangeForVarReference(results, 0, CoreFile.Length, searchWord);
            }
            else
            {
                foreach (FunctionLabel c in lc.FuncList)
                {
                    VarRefType result = VarRefType.None;
                    int currentOffset = c.Offset;
                    int curLen = GuessLabelPrintLength(c.Offset);
                    while (currentOffset < c.Offset + curLen)
                    {
                        if (lc.isAddressMarkedAsData(currentOffset))
                        {
                            currentOffset = lc.GetNextNonDataAddress(currentOffset);
                            continue;
                        }
                        GBInstruction isu = new GBInstruction();
                        if (!GBASM.GetInstruction(CoreFile.MainFile, 0, currentOffset, ref isu))
                        {
                            break;
                        }

                        if (isu.InstType == InstructionType.ld && isu.ArgCount == 2)
                        {
                            if (isu.Arg1.ArgType == GBArgumentType.MemMapWord && isu.Arg1.NumArg == searchWord)
                            {
                                result |= VarRefType.Write;
                            }
                            else if (isu.Arg2.ArgType == GBArgumentType.MemMapWord && isu.Arg2.NumArg == searchWord)
                            {
                                result |= VarRefType.Read;
                            }
                            else if (isu.Arg1.ArgType == GBArgumentType.RegisterDouble && isu.Arg2.NumArg == searchWord)
                            {
                                result |= VarRefType.Ref;
                            }
                        }
                        currentOffset += isu.InstSize;
                    }
                    if (result != VarRefType.None)
                        results.Add(c.Name, result);
                }
            }
            return results;
        }

        private void SearchFileRangeForVarReference(Dictionary<string, VarRefType> results, int startingOffset, int length, int searchWord)
        {
            int currentOffset = startingOffset;
            while (currentOffset < startingOffset + length)
            {
                if (lc.isAddressMarkedAsData(currentOffset))
                {
                    currentOffset = lc.GetNextNonDataAddress(currentOffset);
                    continue;
                }
                GBInstruction isu = new GBInstruction();
                if (!GBASM.GetInstruction(CoreFile.MainFile, 0, currentOffset, ref isu))
                    break;
                if (isu.InstType == InstructionType.ld && isu.ArgCount == 2)
                {
                    if (isu.Arg1.ArgType == GBArgumentType.MemMapWord && isu.Arg1.NumArg == searchWord)
                    {
                        results.Add(currentOffset.ToString("X"), VarRefType.Write);
                    }
                    else if (isu.Arg2.ArgType == GBArgumentType.MemMapWord && isu.Arg2.NumArg == searchWord)
                    {
                        results.Add(currentOffset.ToString("X"), VarRefType.Read);
                    }
                    else if (isu.Arg1.ArgType == GBArgumentType.RegisterDouble && isu.Arg2.NumArg == searchWord)
                    {
                        results.Add(currentOffset.ToString("X"), VarRefType.Ref);
                    }
                }
                currentOffset += isu.InstSize;
            }
        }

        #endregion Searching

        #region Utility Methods

        public int GuessLabelPrintLength(int offset)
        {
            int currentOffset = offset;
            int maxLength = 0x4000 - (currentOffset & 0x3FFF);
            bool done = false;
            int nopCount = 0;
            var jumpLocs = new List<int>();
            bool endpoint = false;
            while (!done)
            {
                if (currentOffset >= CoreFile.Length)
                {
                    break;
                }

                GBInstruction isu = new GBInstruction();
                GBASM.GetInstruction(CoreFile.MainFile, 0, currentOffset, ref isu);
                currentOffset += isu.InstSize;

                if (currentOffset - offset >= maxLength)
                {
                    break;
                }

                switch (isu.InstType)
                {
                    case InstructionType.nop:
                        nopCount++;
                        if (nopCount > 3)
                        {
                            endpoint = true;
                        }
                        break;

                    case InstructionType.ret:
                    case InstructionType.reti:
                        if (isu.ArgCount == 0)
                        {
                            endpoint = true;
                        }

                        break;

                    case InstructionType.jp:
                    case InstructionType.jr:
                        {
                            int jumpedOffset = 0;
                            if (isu.ArgCount == 1)
                            {
                                jumpedOffset = Utility.GetRealAddress(isu.Bank, isu.Arg1.NumArg);
                            }
                            else if (isu.ArgCount == 2)
                            {
                                jumpedOffset = Utility.GetRealAddress(isu.Bank, isu.Arg2.NumArg);
                            }
                            if (jumpedOffset > currentOffset)
                            {
                                jumpLocs.Add(jumpedOffset);
                            }
                            if (isu.ArgCount == 1)
                            {
                                endpoint = true;
                            }
                            break;
                        }
                    default:
                        break;
                }
                if (endpoint)
                {
                    if (jumpLocs.Contains(currentOffset))
                    {
                        jumpLocs.Remove(currentOffset);
                        endpoint = false;
                    }
                    else
                    {
                        done = true;
                    }
                }
            }
            return currentOffset - offset;
        }

        private string AddressToString(GBInstruction isu)
        {
            switch (PrintedOffsetFormat)
            {
                case OffsetFormat.BankOffset:
                    {
                        return isu.Bank.ToString("X2") + ":" + isu.Address.ToString("X4") + "  ";
                    }
                case OffsetFormat.Hex:
                case OffsetFormat.Decimal:
                    {
                        int address = Utility.GetRealAddress(isu.Bank, isu.Address);
                        if (PrintedOffsetFormat == OffsetFormat.Hex)
                        {
                            return address.ToString("X6") + "   ";
                        }
                        else
                        {
                            return address.ToString("D7") + "  "; 
                        }
                    }
                default:
                    return "";
            }
        }

        private string ArgumentToString(byte bank, ushort address, InstructionType instType, GBArgument arg)
        {
            switch (arg.ArgType)
            {
                case GBArgumentType.Bit:
                    return arg.NumArg.ToString();
                case GBArgumentType.Byte:
                    return NumberToASMString(arg.NumArg, NumberType.Byte);
                case GBArgumentType.MemMapWord:
                    {
                        string vName = lc.GetSymbolName(SymbolType.Variable, arg.NumArg);
                        if (vName != "")
                        {
                            return "[" + vName + "]";
                        }
                        else
                        {
                            return "[" + NumberToASMString(arg.NumArg, NumberType.Word) + "]";
                        }
                    }
                case GBArgumentType.Word:
                    {
                        string vName = lc.GetSymbolName(SymbolType.Generic, Utility.GetRealAddress(bank, arg.NumArg));
                        if (arg.NumArg < 0x8000 && vName != "")
                        {
                            return vName;
                        }
                        if (instType == InstructionType.ld)
                        {
                            vName = lc.GetSymbolName(SymbolType.Variable, arg.NumArg);
                            if (vName != "")
                            {
                                return vName;
                            }
                        }
                        return NumberToASMString(arg.NumArg, NumberType.Word);
                    }

                case GBArgumentType.MemMapRegisterSingle:
                    return "[" + arg.RegSingleArg.ToString() + "]";
                case GBArgumentType.RegisterSingle:
                    return arg.RegSingleArg.ToString();
                case GBArgumentType.MemMapRegisterDouble:
                    return "[" + arg.RegDoubleArg.ToString() + "]";
                case GBArgumentType.RegisterDouble:
                    return arg.RegDoubleArg.ToString();
                case GBArgumentType.Conditional:
                    return arg.CondArg.ToString();
                default:
                    return "";
            }
        }

        private enum NumberType { Byte, Word }

        private string NumberToASMString(int intArg, NumberType numType)
        {
            switch (InstructionNumberFormat)
            {
                case OffsetFormat.Decimal:
                    return intArg.ToString();
                default:
                    if (numType == NumberType.Byte)
                        return "$" + intArg.ToString("X2");
                    else
                        return "$" + intArg.ToString("X4");
            }
        }

        #endregion Utility Methods

        #region Instructions

        #region Regular Instructions

        public static string[] longInst = new string[256]
        {
            "NOP",
            "Load BC with {0}",
            "Load [BC] with A",
            "Increment BC",
            "Increment B",
            "Decrement B",
            "Load B, {0}",
            "Rotate A Left Into Carry",
            "Load {0} With Stack Pointer",
            "Add BC to HL",
            "Load A With [BC]",
            "Decrement BC",
            "Increment C",
            "Decrement C",
            "Load C With {0}",
            "Rotate A Right Into Carry",
            "Stop",
            "Load DE with {0}",
            "Load [DE] With A",
            "Increment DE",
            "Increment D",
            "Decrement E",
            "Load D With {0}",
            "Rotate A Left",
            "Jump Relative To {0}",
            "Add DE To HL",
            "Load A with [DE]",
            "Decrement DE",
            "Increment E",
            "Decrement E",
            "Load E With {0}",
            "Rotate A Right",
            "Jump Relative (If Not Zero) To {0}",
            "Load HL With {0}",
            "Load [HL] With A, Increment HL",
            "Increment HL",
            "Increment H",
            "Decrement H",
            "Load H With {0}",
            "Change A To Binary Coded Decimal (BCD)",
            "Jump Relative (If Zero) to {0}",
            "Add HL To HL",
            "Load A with [HL], Increment HL",
            "Decrement HL",
            "Increment L",
            "Decrement L",
            "Load L With {0}",
            "Complement A",
            "Jump Relative (If No Carry) To {0}",
            "Load Stack Pointer With {0}",
            "Load [HL] With A, Decrement HL",
            "Increment Stack Pointer",
            "Increment [HL]",
            "Decrement [HL]",
            "Load [HL] With {0}",
            "Set Carry Flag",
            "Jump Relative (If Carry) To {0}",
            "Add Stack Pointer to HL",
            "Load A With [HL], Decrement HL",
            "Decrement Stack Pointer",
            "Increment A",
            "Decrement A",
            "Load A With {0}",
            "Complement Carry Flag",
            "Load B With B",
            "Load B With C",
            "Load B With D",
            "Load B With E",
            "Load B With H",
            "Load B With L",
            "Load B With [HL]",
            "Load B With A",
            "Load C With B",
            "Load C With C",
            "Load C With D",
            "Load C With E",
            "Load C With H",
            "Load C With L",
            "Load C With [HL]",
            "Load C With A",
            "Load D With B",
            "Load D With C",
            "Load D With D",
            "Load D With E",
            "Load D With H",
            "Load D With L",
            "Load D With [HL]",
            "Load D With A",
            "Load E With B",
            "Load E With C",
            "Load E With D",
            "Load E With E",
            "Load E With H",
            "Load E With L",
            "Load E With [HL]",
            "Load E With A",
            "Load H With B",
            "Load H With C",
            "Load H With D",
            "Load H With E",
            "Load H With H",
            "Load H With L",
            "Load H With [HL]",
            "Load H With A",
            "Load L With B",
            "Load L With C",
            "Load L With D",
            "Load L With E",
            "Load L With H",
            "Load L With L",
            "Load L With [HL]",
            "Load L With A",
            "Load [HL] With B",
            "Load [HL] With C",
            "Load [HL] With D",
            "Load [HL] With E",
            "Load [HL] With H",
            "Load [HL] With L",
            "Halt",
            "Load [HL] With A",
            "Load A With B",
            "Load A With C",
            "Load A With D",
            "Load A With E",
            "Load A With H",
            "Load A With L",
            "Load A With [HL]",
            "Load A With A",
            "Add B To A",
            "Add C To A",
            "Add D To A",
            "Add E To A",
            "Add H To A",
            "Add L To A",
            "Add [HL] To A",
            "Add A To A",
            "Add B Plus Carry to A",
            "Add C Plus Carry to A",
            "Add D Plus Carry to A",
            "Add E Plus Carry to A",
            "Add H Plus Carry to A",
            "Add L Plus Carry to A",
            "Add [HL] Plus Carry to A",
            "Add A Plus Carry to A",
            "Subtract B From A",
            "Subtract C From A",
            "Subtract D From A",
            "Subtract E From A",
            "Subtract H From A",
            "Subtract L From A",
            "Subtract [HL] From A",
            "Subtract A from A",
            "Subtract B Plus Carry from A",
            "Subtract C Plus Carry from A",
            "Subtract D Plus Carry from A",
            "Subtract E Plus Carry from A",
            "Subtract H Plus Carry from A",
            "Subtract L Plus Carry from A",
            "Subtract [HL] Plus Carry from A",
            "Subtract A Plus Carry from A",
            "A And B",
            "A And C",
            "A And D",
            "A And E",
            "A And H",
            "A And L",
            "A And [HL]",
            "A And A",
            "A Xor B",
            "A Xor C",
            "A Xor D",
            "A Xor e",
            "A Xor H",
            "A Xor L",
            "A Xor [HL]",
            "A Xor A",
            "A Or B",
            "A Or C",
            "A Or D",
            "A Or E",
            "A Or H",
            "A Or L",
            "A Or [HL]",
            "A Or A",
            "Test A - B",
            "Test A - C",
            "Test A - D",
            "Test A - E",
            "Test A - H",
            "Test A - L",
            "Test A - [HL]",
            "Test A - A",
            "Return If Not Zero",
            "Pop BC",
            "Jump (If Not Zero) To {0}",
            "Jump To {0}",
            "Call (If Not Zero) {0}",
            "Push BC",
            "Add {0} To A",
            "Reset To Offset $00",
            "Return If Zero",
            "Return",
            "Jump (If Zero) To {0}",
            "0xCB",
            "Call (If Zero) {0}",
            "Call {0}",
            "Add {0} Plus Carry to A",
            "Reset To Offset $08",
            "Return If No Carry",
            "Pop DE",
            "Jump (If No Carry) To {0}",
            "0xD3",
            "Call (If No Carry) {0}",
            "Push DE",
            "Subtract {0} From A",
            "Reset To Offset $10",
            "Return If Carry",
            "Return, Enable Interrupts",
            "Jump (If Carry) To {0}",
            "0xDB",
            "Call (If Carry) {0}",
            "0xDB",
            "Subtract {0} Plus Carry from A",
            "Reset To Offset $18",
            "Load {0} With A",
            "Pop HL",
            "Load [$FFCC] with A",
            "0xE3",
            "0xE4",
            "Push HL",
            "A And {0}",
            "Reset To Offset $20",
            "Add {0} To Stack Pointer",
            "Jump To HL",
            "Load {0} With A",
            "0xEB",
            "0xEC",
            "0xED",
            "A Xor {0}",
            "Reset To Offset $28",
            "Load A With {0}",
            "Pop AF",
            "Load A With [$FFCC]",
            "Disable Interrupts",
            "0xF4",
            "Push AF",
            "A Or {0}",
            "Reset To Offset $30",
            "Load HL With Address Stack Pointer + {0}",
            "Load Stack Pointer With HL",
            "Load A With {0}",
            "Enable Interrupts",
            "0xFC",
            "0xFD",
            "Compare A With {0}",
            "Reset To Offset $38"
        };

        #endregion Regular Instructions

        #region CB Instructions

        public static string[] CBLongInst = new string[256]
        {
            "Rotate Left Carry B",
            "Rotate Left Carry C",
            "Rotate Left Carry D",
            "Rotate Left Carry E",
            "Rotate Left Carry H",
            "Rotate Left Carry L",
            "Rotate Left Carry [HL]",
            "Rotate Left Carry A",
            "Rotate Right Carry B",
            "Rotate Right Carry C",
            "Rotate Right Carry D",
            "Rotate Right Carry E",
            "Rotate Right Carry H",
            "Rotate Right Carry L",
            "Rotate Right Carry [HL]",
            "Rotate Right Carry A",
            "Rotate Left B",
            "Rotate Left C",
            "Rotate Left D",
            "Rotate Left E",
            "Rotate Left H",
            "Rotate Left L",
            "Rotate Left [HL]",
            "Rotate Left A",
            "Rotate Right B",
            "Rotate Right C",
            "Rotate Right D",
            "Rotate Right E",
            "Rotate Right H",
            "Rotate Right L",
            "Rotate Right [HL]",
            "Rotate Right A",
            "Shift Left Arithmetic B",
            "Shift Left Arithmetic C",
            "Shift Left Arithmetic D",
            "Shift Left Arithmetic E",
            "Shift Left Arithmetic H",
            "Shift Left Arithmetic L",
            "Shift Left Arithmetic [HL]",
            "Shift Left Arithmetic A",
            "Shift Right Arithmetic B",
            "Shift Right Arithmetic C",
            "Shift Right Arithmetic D",
            "Shift Right Arithmetic E",
            "Shift Right Arithmetic H",
            "Shift Right Arithmetic L",
            "Shift Right Arithmetic [HL]",
            "Shift Right Arithmetic A",
            "Swap Upper and Lower Half of B",
            "Swap Upper and Lower Half of C",
            "Swap Upper and Lower Half of D",
            "Swap Upper and Lower Half of E",
            "Swap Upper and Lower Half of H",
            "Swap Upper and Lower Half of L",
            "Swap Upper and Lower Half of [HL]",
            "Swap Upper and Lower Half of A",
            "Shift Right Logical B",
            "Shift Right Logical C",
            "Shift Right Logical D",
            "Shift Right Logical E",
            "Shift Right Logical H",
            "Shift Right Logical L",
            "Shift Right Logical [HL]",
            "Shift Right Logical A",
            "Test Bit 0 of B",
            "Test Bit 0 of C",
            "Test Bit 0 of D",
            "Test Bit 0 of E",
            "Test Bit 0 of H",
            "Test Bit 0 of L",
            "Test Bit 0 of [HL]",
            "Test Bit 0 of A",
            "Test Bit 1 of B",
            "Test Bit 1 of C",
            "Test Bit 1 of D",
            "Test Bit 1 of E",
            "Test Bit 1 of H",
            "Test Bit 1 of L",
            "Test Bit 1 of [HL]",
            "Test Bit 1 of A",
            "Test Bit 2 of B",
            "Test Bit 2 of C",
            "Test Bit 2 of D",
            "Test Bit 2 of E",
            "Test Bit 2 of H",
            "Test Bit 2 of L",
            "Test Bit 2 of [HL]",
            "Test Bit 2 of A",
            "Test Bit 3 of B",
            "Test Bit 3 of C",
            "Test Bit 3 of D",
            "Test Bit 3 of E",
            "Test Bit 3 of H",
            "Test Bit 3 of L",
            "Test Bit 3 of [HL]",
            "Test Bit 3 of A",
            "Test Bit 4 of B",
            "Test Bit 4 of C",
            "Test Bit 4 of D",
            "Test Bit 4 of E",
            "Test Bit 4 of H",
            "Test Bit 4 of L",
            "Test Bit 4 of [HL]",
            "Test Bit 4 of A",
            "Test Bit 5 of B",
            "Test Bit 5 of C",
            "Test Bit 5 of D",
            "Test Bit 5 of E",
            "Test Bit 5 of H",
            "Test Bit 5 of L",
            "Test Bit 5 of [HL]",
            "Test Bit 5 of A",
            "Test Bit 6 of B",
            "Test Bit 6 of C",
            "Test Bit 6 of D",
            "Test Bit 6 of E",
            "Test Bit 6 of H",
            "Test Bit 6 of L",
            "Test Bit 6 of [HL]",
            "Test Bit 6 of A",
            "Test Bit 7 of B",
            "Test Bit 7 of C",
            "Test Bit 7 of D",
            "Test Bit 7 of E",
            "Test Bit 7 of H",
            "Test Bit 7 of L",
            "Test Bit 7 of [HL]",
            "Test Bit 7 of A",
            "Reset Bit 0 of B",
            "Reset Bit 0 of C",
            "Reset Bit 0 of D",
            "Reset Bit 0 of E",
            "Reset Bit 0 of H",
            "Reset Bit 0 of L",
            "Reset Bit 0 of [HL]",
            "Reset Bit 0 of A",
            "Reset Bit 1 of B",
            "Reset Bit 1 of C",
            "Reset Bit 1 of D",
            "Reset Bit 1 of E",
            "Reset Bit 1 of H",
            "Reset Bit 1 of L",
            "Reset Bit 1 of [HL]",
            "Reset Bit 1 of A",
            "Reset Bit 2 of B",
            "Reset Bit 2 of C",
            "Reset Bit 2 of D",
            "Reset Bit 2 of E",
            "Reset Bit 2 of H",
            "Reset Bit 2 of L",
            "Reset Bit 2 of [HL]",
            "Reset Bit 2 of A",
            "Reset Bit 3 of B",
            "Reset Bit 3 of C",
            "Reset Bit 3 of D",
            "Reset Bit 3 of E",
            "Reset Bit 3 of H",
            "Reset Bit 3 of L",
            "Reset Bit 3 of [HL]",
            "Reset Bit 3 of A",
            "Reset Bit 4 of B",
            "Reset Bit 4 of C",
            "Reset Bit 4 of D",
            "Reset Bit 4 of E",
            "Reset Bit 4 of H",
            "Reset Bit 4 of L",
            "Reset Bit 4 of [HL]",
            "Reset Bit 4 of A",
            "Reset Bit 5 of B",
            "Reset Bit 5 of C",
            "Reset Bit 5 of D",
            "Reset Bit 5 of E",
            "Reset Bit 5 of H",
            "Reset Bit 5 of L",
            "Reset Bit 5 of [HL]",
            "Reset Bit 5 of A",
            "Reset Bit 6 of B",
            "Reset Bit 6 of C",
            "Reset Bit 6 of D",
            "Reset Bit 6 of E",
            "Reset Bit 6 of H",
            "Reset Bit 6 of L",
            "Reset Bit 6 of [HL]",
            "Reset Bit 6 of A",
            "Reset Bit 7 of B",
            "Reset Bit 7 of C",
            "Reset Bit 7 of D",
            "Reset Bit 7 of E",
            "Reset Bit 7 of H",
            "Reset Bit 7 of L",
            "Reset Bit 7 of [HL]",
            "Reset Bit 7 of A",
            "Set Bit 0 of B",
            "Set Bit 0 of C",
            "Set Bit 0 of D",
            "Set Bit 0 of E",
            "Set Bit 0 of H",
            "Set Bit 0 of L",
            "Set Bit 0 of [HL]",
            "Set Bit 0 of A",
            "Set Bit 1 of B",
            "Set Bit 1 of C",
            "Set Bit 1 of D",
            "Set Bit 1 of E",
            "Set Bit 1 of H",
            "Set Bit 1 of L",
            "Set Bit 1 of [HL]",
            "Set Bit 1 of A",
            "Set Bit 2 of B",
            "Set Bit 2 of C",
            "Set Bit 2 of D",
            "Set Bit 2 of E",
            "Set Bit 2 of H",
            "Set Bit 2 of L",
            "Set Bit 2 of [HL]",
            "Set Bit 2 of A",
            "Set Bit 3 of B",
            "Set Bit 3 of C",
            "Set Bit 3 of D",
            "Set Bit 3 of E",
            "Set Bit 3 of H",
            "Set Bit 3 of L",
            "Set Bit 3 of [HL]",
            "Set Bit 3 of A",
            "Set Bit 4 of B",
            "Set Bit 4 of C",
            "Set Bit 4 of D",
            "Set Bit 4 of E",
            "Set Bit 4 of H",
            "Set Bit 4 of L",
            "Set Bit 4 of [HL]",
            "Set Bit 4 of A",
            "Set Bit 5 of B",
            "Set Bit 5 of C",
            "Set Bit 5 of D",
            "Set Bit 5 of E",
            "Set Bit 5 of H",
            "Set Bit 5 of L",
            "Set Bit 5 of [HL]",
            "Set Bit 5 of A",
            "Set Bit 6 of B",
            "Set Bit 6 of C",
            "Set Bit 6 of D",
            "Set Bit 6 of E",
            "Set Bit 6 of H",
            "Set Bit 6 of L",
            "Set Bit 6 of [HL]",
            "Set Bit 6 of A",
            "Set Bit 7 of B",
            "Set Bit 7 of C",
            "Set Bit 7 of D",
            "Set Bit 7 of E",
            "Set Bit 7 of H",
            "Set Bit 7 of L",
            "Set Bit 7 of [HL]",
            "Set Bit 7 of A"
        };

        #endregion CB Instructions

        #endregion Instructions
    }
}