using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GBRead.Base.Annotation
{
    public enum SymbolType
    {
        Generic,
        Label,
        DataLabel,
        Variable
    }

    public struct SymValue
    {
        public SymbolType Type;
        public int Value;
    }

    public class LabelContainer
    {
        #region Private members

        private object commentListLock = new object();
        private object symbolListLock = new object();

        private List<FunctionLabel> _funcList = new List<FunctionLabel>();
        private List<DataLabel> _dataList = new List<DataLabel>();
        private List<VarLabel> _varList = new List<VarLabel>();
        private Dictionary<int, string> _commentList = new Dictionary<int, string>();

        //private Dictionary<string, SymValue> _symbolList = new Dictionary<string, SymValue>();
        private HashSet<int> _dataAddrs = new HashSet<int>();

        private Table tableFile = new Table();

        private SymbolTable symbolTable = new SymbolTable();

        #endregion Private members

        #region Public Properties

        public IList<FunctionLabel> FuncList
        {
            get
            {
                lock (symbolListLock)
                {
                    return _funcList;
                }
            }
        }

        public IList<DataLabel> DataList
        {
            get
            {
                lock (symbolListLock)
                {
                    return _dataList;
                }
            }
        }

        public IList<VarLabel> VarList
        {
            get
            {
                lock (symbolListLock)
                {
                    return _varList;
                }
            }
        }

        public Dictionary<int, string> Comments
        {
            get
            {
                lock (symbolListLock)
                {
                    return _commentList;
                }
            }
        }

        public Table TableFile
        {
            get
            {
                return tableFile;
            }
        }

        #endregion Public Properties

        public LabelContainer()
        {
        }

        public void Initialize()
        {
            lock (symbolListLock)
            {
                _funcList.Clear();
                _dataList.Clear();
                _dataAddrs.Clear();
                _varList.Clear();
            }
            lock (commentListLock)
            {
                _commentList.Clear();
            }
            symbolTable.ClearTable();
            if (File.Exists("default.txt"))
            {
                LoadLabelFile("default.txt");
            }
        }

        #region Adding, clearing, and removing labels

        // TODO: Adjust behavior so that labels belonging to the same offset get printed.

        public List<Symbol> GetSymbolsByValue(long value, bool isConstant)
        {
            return symbolTable.GetSymbolsByValue(value, isConstant);
        }

        public bool IsSymbolDefined(Symbol symbol)
        {
            return symbolTable.ContainsSymbol(symbol);
        }

        public bool TryGetFuncLabel(int current, out FunctionLabel label)
        {
            lock (symbolListLock)
            {
                var s = from item in _funcList
                        where item.Offset == current
                        select item;
                var success = s.Count() != 0;
                label = success ? s.First() : new FunctionLabel(current);
                return success;
            }
        }

        public bool TryGetDataLabel(int current, out DataLabel label)
        {
            lock (symbolListLock)
            {
                var s = from item in _dataList
                        where item.Offset == current
                        select item;
                var success = s.Count() != 0;
                label = success ? s.First() : new DataLabel(current);
                return success;
            }
        }

        public bool TryGetVarLabel(ushort current, out VarLabel label)
        {
            lock (symbolListLock)
            {
                var s = from item in _varList
                        where item.Variable == current
                        select item;
                var success = s.Count() != 0;
                label = success ? s.First() : new VarLabel(current);
                return success;
            }
        }

        public void AddFuncLabel(FunctionLabel toBeAdded)
        {
            lock (symbolListLock)
            {
                var sym = new Symbol()
                {
                    Name = toBeAdded.Name,
                    Value = toBeAdded.Offset
                };
                if (!symbolTable.ContainsSymbol(sym))
                {
                    _funcList.Add(toBeAdded);

                    symbolTable.AddSymbol(sym);
                }
            }
        }

        public void AddDataLabel(DataLabel toBeAdded)
        {
            lock (symbolListLock)
            {
                var sym = new Symbol()
                {
                    Name = toBeAdded.Name,
                    Value = toBeAdded.Offset
                };
                if (!symbolTable.ContainsSymbol(sym))
                {
                    _dataList.Add(toBeAdded);
                    symbolTable.AddSymbol(sym);
                    for (int i = toBeAdded.Offset; i < toBeAdded.Offset + toBeAdded.Length; i++)
                    {
                        _dataAddrs.Add(i);
                    }
                }
            }
        }

        public void AddVarLabel(VarLabel toBeAdded)
        {
            lock (symbolListLock)
            {
                var sym = new Symbol()
                {
                    Name = toBeAdded.Name,
                    Value = toBeAdded.Value,
                    IsConstant = true
                };
                if (!symbolTable.ContainsSymbol(sym))
                {
                    _varList.Add(toBeAdded);
                    symbolTable.AddSymbol(sym);
                }
            }
        }

        public void AddComment(int offset, string comment)
        {
            if (String.IsNullOrEmpty(comment))
            {
                return;
            }
            lock (commentListLock)
            {
                if (_commentList.ContainsKey(offset))
                {
                    _commentList[offset] = comment;
                }
                else
                {
                    _commentList.Add(offset, comment);
                }
            }
        }

        public void RemoveFuncLabel(FunctionLabel toBeRemoved)
        {
            lock (symbolListLock)
            {
                _funcList.Remove(toBeRemoved);
                var sym = new Symbol()
                {
                    Name = toBeRemoved.Name
                };
                symbolTable.RemoveSymbol(sym);
            }
        }

        public void RemoveDataLabel(DataLabel toBeRemoved)
        {
            lock (symbolListLock)
            {
                _dataList.Remove(toBeRemoved);
                var sym = new Symbol()
                {
                    Name = toBeRemoved.Name
                };
                symbolTable.RemoveSymbol(sym);
                _dataAddrs.RemoveWhere(x => x >= toBeRemoved.Offset && x < toBeRemoved.Offset + toBeRemoved.Length);
            }
        }

        public void RemoveVarLabel(VarLabel toBeRemoved)
        {
            lock (symbolListLock)
            {
                _varList.Remove(toBeRemoved);
                var sym = new Symbol()
                {
                    Name = toBeRemoved.Name
                };
                symbolTable.RemoveSymbol(sym);
            }
        }

        public void RemoveComment(int offset)
        {
            lock (commentListLock)
            {
                if (_commentList.ContainsKey(offset))
                {
                    _commentList.Remove(offset);
                }
            }
        }

        public bool isAddressMarkedAsData(int address)
        {
            lock (symbolListLock)
            {
                return _dataAddrs.Contains(address);
            }
        }

        public int GetNextNonDataAddress(int address)
        {
            lock (symbolListLock)
            {
                int offset = address;
                while (_dataAddrs.Contains(offset++))
                {
                }
                return offset;
            }
        }

        #endregion Adding, clearing, and removing labels

        #region Loading and Saving Label Files

        // TODO: Make sure everything saves and loads properly.

        public void LoadLabelFile(string fileName)
        {
            using (TextReader labelFile = new StreamReader(fileName))
            {
                using (TextWriter errFile = new StreamWriter("err.txt"))
                {
                    if (labelFile.ReadLine() != "gbr")
                    {
                        return;
                    }
                    else
                    {
                        var items = new List<Dictionary<string, string>>();
                        int curItem = -1;
                        string currentLine;
                        while ((currentLine = labelFile.ReadLine()) != null)
                        {
                            switch (currentLine)
                            {
                                case ".label":
                                    {
                                        items.Add(new Dictionary<string, string>() { { "tag", "label" } });
                                        curItem++;
                                    }
                                    break;

                                case ".data":
                                    {
                                        items.Add(new Dictionary<string, string>() { { "tag", "data" } });
                                        curItem++;
                                    }
                                    break;

                                case ".var":
                                    {
                                        items.Add(new Dictionary<string, string>() { { "tag", "var" } });
                                        curItem++;
                                    }
                                    break;

                                case ".comment":
                                    {
                                        items.Add(new Dictionary<string, string>() { { "tag", "comment" } });
                                        curItem++;
                                    }
                                    break;

                                default:
                                    if (curItem == -1)
                                    {
                                        break;
                                    }
                                    string[] opt = currentLine.Split(new[] { ':' }, 2);
                                    if (opt.Length == 1)
                                    {
                                        if (items[curItem].ContainsKey("_c"))
                                        {
                                            items[curItem]["_c"] += "\n" + currentLine;
                                        }
                                        else
                                        {
                                            items[curItem].Add("_c", currentLine);
                                        }
                                    }
                                    else
                                    {
                                        if (items[curItem].ContainsKey(opt[0]))
                                        {
                                            items[curItem][opt[0]] = opt[1];
                                        }
                                        else
                                        {
                                            items[curItem].Add(opt[0], opt[1]);
                                        }
                                    }
                                    break;
                            }
                        }
                        foreach (var currentItem in items)
                        {
                            switch (currentItem["tag"])
                            {
                                case "label":
                                    {
                                        int offset = 0;
                                        string name = "";
                                        string comment = "";
                                        bool offsetGood = false;
                                        foreach (var kvp in currentItem)
                                        {
                                            switch (kvp.Key)
                                            {
                                                case "_o":
                                                    {
                                                        offsetGood = Utility.OffsetStringToInt(kvp.Value, out offset);
                                                    }

                                                    break;

                                                case "_n":
                                                    {
                                                        if (Utility.IsWord(kvp.Value))
                                                        {
                                                            name = kvp.Value;
                                                        }
                                                    }

                                                    break;

                                                case "_c":
                                                    {
                                                        comment = kvp.Value;
                                                    }

                                                    break;
                                            }
                                        }
                                        if (offsetGood)
                                        {
                                            FunctionLabel fl = new FunctionLabel(offset, name, comment);
                                            AddFuncLabel(fl);
                                        }
                                        else
                                        {
                                            errFile.WriteLine("Label #" + items.IndexOf(currentItem) + " was unrecognized.");
                                        }
                                    }

                                    break;

                                case "data":
                                    {
                                        int offset = -1;
                                        int length = -1;
                                        string printTemp = "";
                                        string name = "";
                                        string comment = "";
                                        DataSectionType dst = DataSectionType.Data;
                                        GBPalette gbp = new GBPalette();
                                        bool offsetGood = false;
                                        bool lengthGood = false;
                                        foreach (var kvp in currentItem)
                                        {
                                            switch (kvp.Key)
                                            {
                                                case "_o":
                                                    {
                                                        offsetGood = Utility.OffsetStringToInt(kvp.Value, out offset);
                                                    }

                                                    break;

                                                case "_l":
                                                    {
                                                        lengthGood = Utility.OffsetStringToInt(kvp.Value, out length);
                                                    }
                                                    break;

                                                case "_n":
                                                    {
                                                        if (Utility.IsWord(kvp.Value))
                                                        {
                                                            name = kvp.Value;
                                                        }
                                                    }

                                                    break;

                                                case "_d":
                                                    {
                                                        printTemp = kvp.Value;
                                                    }
                                                    break;

                                                case "_p1":
                                                    {
                                                        dst = DataSectionType.Image;
                                                        Utility.OffsetStringToInt(kvp.Value, out gbp.Col_1);
                                                    }

                                                    break;

                                                case "_p2":
                                                    {
                                                        dst = DataSectionType.Image;
                                                        Utility.OffsetStringToInt(kvp.Value, out gbp.Col_2);
                                                    }

                                                    break;

                                                case "_p3":
                                                    {
                                                        dst = DataSectionType.Image;
                                                        Utility.OffsetStringToInt(kvp.Value, out gbp.Col_3);
                                                    }

                                                    break;

                                                case "_p4":
                                                    {
                                                        dst = DataSectionType.Image;
                                                        Utility.OffsetStringToInt(kvp.Value, out gbp.Col_4);
                                                    }

                                                    break;

                                                case "_c":
                                                    {
                                                        comment = kvp.Value;
                                                    }

                                                    break;
                                            }
                                        }

                                        if (offsetGood && lengthGood)
                                        {
                                            DataLabel ds = new DataLabel(offset, length, name, printTemp, comment, dst, gbp);
                                            AddDataLabel(ds);
                                        }
                                        else
                                        {
                                            errFile.WriteLine("Label #" + items.IndexOf(currentItem) + " was unrecognized.");
                                        }
                                    }

                                    break;

                                case "var":
                                    {
                                        int variable = -1;
                                        string name = "";
                                        string comment = "";
                                        bool variableGood = false;

                                        foreach (var kvp in currentItem)
                                        {
                                            switch (kvp.Key)
                                            {
                                                case "_v":
                                                    {
                                                        variableGood = Utility.OffsetStringToInt(kvp.Value, out variable);
                                                    }

                                                    break;

                                                case "_n":
                                                    {
                                                        if (Utility.IsWord(kvp.Value))
                                                        {
                                                            name = kvp.Value;
                                                        }
                                                    }

                                                    break;

                                                case "_c":
                                                    {
                                                        comment = kvp.Value;
                                                    }

                                                    break;
                                            }
                                        }

                                        if (variableGood)
                                        {
                                            VarLabel vl = new VarLabel(variable, name, comment);
                                            AddVarLabel(vl);
                                        }
                                        else
                                        {
                                            errFile.WriteLine("Label #" + items.IndexOf(currentItem) + " was unrecognized.");
                                        }
                                    }

                                    break;

                                case "comment":
                                    {
                                        int offset = 0;
                                        string name = String.Empty;
                                        string comment = "";
                                        bool offsetGood = false;

                                        foreach (var kvp in currentItem)
                                        {
                                            switch (kvp.Key)
                                            {
                                                case "_o":
                                                    {
                                                        offsetGood = Utility.OffsetStringToInt(kvp.Value, out offset);
                                                    }

                                                    break;

                                                case "_c":
                                                    {
                                                        comment = kvp.Value;
                                                    }

                                                    break;
                                            }
                                        }
                                        if (offsetGood)
                                        {
                                            AddComment(offset, comment);
                                        }
                                        else
                                        {
                                            errFile.WriteLine("Label #" + items.IndexOf(currentItem) + " was unrecognized.");
                                        }
                                    }
                                    break;

                                default:

                                    break;
                            }
                        }
                    }
                }
            }
        }

        public void SaveLabelFile(string fileName)
        {
            using (TextWriter functions = new StreamWriter(fileName, false, Encoding.UTF8))
            {
                functions.WriteLine("gbr");
                foreach (FunctionLabel s in FuncList)
                {
                    functions.WriteLine(".label");
                    functions.WriteLine("_n:" + s.Name);
                    functions.WriteLine("_o:" + s.Offset.ToString("X"));
                    functions.WriteLine("_c:" + s.Comment);
                }
                foreach (DataLabel s in DataList)
                {
                    functions.WriteLine(".data");
                    functions.WriteLine("_n:" + s.Name);
                    functions.WriteLine("_o:" + s.Offset.ToString("X"));
                    functions.WriteLine("_c:" + s.Comment);
                    functions.WriteLine("_l:" + s.Length.ToString("X"));
                    functions.WriteLine("_t:" + s.DSectionType);
                    functions.WriteLine("_d:" + s.PrintTemplate);
                    if (s.DSectionType == DataSectionType.Image)
                    {
                        functions.WriteLine("_p1:" + s.Palette.Col_1.ToString("X"));
                        functions.WriteLine("_p2:" + s.Palette.Col_2.ToString("X"));
                        functions.WriteLine("_p3:" + s.Palette.Col_3.ToString("X"));
                        functions.WriteLine("_p4:" + s.Palette.Col_4.ToString("X"));
                    }
                }
                foreach (VarLabel s in VarList)
                {
                    functions.WriteLine(".var");
                    functions.WriteLine("_n:" + s.Name);
                    functions.WriteLine("_v:" + s.Variable.ToString("X"));
                    functions.WriteLine("_c:" + s.Comment);
                }
                foreach (KeyValuePair<int, string> kvp in _commentList)
                {
                    functions.WriteLine(".comment");
                    functions.WriteLine("_o:" + kvp.Key.ToString("X"));
                    functions.WriteLine("_c:" + kvp.Value);
                }
                functions.Close();
            }
        }

        #endregion Loading and Saving Label Files
    }
}