namespace GBRead.Base
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class LabelContainer
    {
        #region Private members

        private object funcListLock = new object();
        private object dataListLock = new object();
        private object varListLock = new object();
        private object commentListLock = new object();
        private object symbolListLock = new object();
        private object dataAddrLock = new object();


        private List<FunctionLabel> _funcList = new List<FunctionLabel>();
        private List<DataLabel> _dataList = new List<DataLabel>();
        private List<VarLabel> _varList = new List<VarLabel>();
        private Dictionary<int, string> _commentList = new Dictionary<int, string>();
        private Dictionary<string, int> _symbolList = new Dictionary<string, int>();
        private HashSet<int> dataAddrs = new HashSet<int>();

        #endregion Private members

        #region Public Properties

        public IList<FunctionLabel> FuncList
        {
            get
            {
                lock (funcListLock)
                {
                    return _funcList;
                }
            }
        }

        public IList<DataLabel> DataList
        {
            get
            {
                lock (dataListLock)
                {
                    return _dataList;
                }
            }
        }

        public IList<VarLabel> VarList
        {
            get
            {
                lock (varListLock)
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

        public Dictionary<string, int> SymbolList
        {
            get
            {
                lock (symbolListLock)
                {
                    return _symbolList;
                }
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
                lock (funcListLock)
                {
                    _funcList.Clear();
                }
                lock (dataListLock)
                {
                    _dataList.Clear();
                    dataAddrs.Clear();
                }
                lock (varListLock)
                {
                    _varList.Clear();
                }
                _symbolList.Clear();
            }
            lock (commentListLock)
            {
                _commentList.Clear();
            }
            if (File.Exists("default.txt"))
            {
                LoadLabelFile("default.txt");
            }
        }

        #region Adding, clearing, and removing labels

        // TODO: Adjust behavior so that labels belonging to the same offset get printed.

        public bool TryGetFuncLabel(int current, out FunctionLabel label)
        {
            lock (funcListLock)
            {
                var s = from item in _funcList where item.Offset == current select item;
                var success = s.Count() != 0;
                label = success ? s.First() : new FunctionLabel(current);
                return success;
            }
        }

        public bool TryGetDataLabel(int current, out DataLabel label)
        {
            lock (dataListLock)
            {
                var s = from item in _dataList where item.Offset == current select item;
                var success = s.Count() != 0;
                label = success ? s.First() : new DataLabel(current);
                return success;
            }
        }

        public bool TryGetVarLabel(ushort current, out VarLabel label)
        {
            lock (varListLock)
            {
                var s = from item in _varList where item.Variable == current select item;
                var success = s.Count() != 0;
                label = success ? s.First() : new VarLabel(current);
                return success;
            }
        }
        
        public void AddFuncLabel(FunctionLabel toBeAdded)
        {
            lock (symbolListLock)
            {
                lock (funcListLock)
                {
                    if (!_symbolList.ContainsKey(toBeAdded.Name))
                    {
                        _funcList.Add(toBeAdded);
                        _symbolList.Add(toBeAdded.Name, toBeAdded.Offset); 
                    }
                }
            }
        }

        public void AddDataLabel(DataLabel toBeAdded)
        {
            lock (symbolListLock)
            {
                lock (dataListLock)
                {
                    if (!_symbolList.ContainsKey(toBeAdded.Name))
                    {
                        _dataList.Add(toBeAdded);
                        _symbolList.Add(toBeAdded.Name, toBeAdded.Value);
                        for (int i = toBeAdded.Offset; i < toBeAdded.Offset + toBeAdded.Length; i++)
                        {
                            dataAddrs.Add(i);
                        } 
                    }
                }
            }
        }

        public void AddVarLabel(VarLabel toBeAdded)
        {
            lock (symbolListLock)
            {
                lock (varListLock)
                {
                    if (!_symbolList.ContainsKey(toBeAdded.Name))
                    {
                        _varList.Add(toBeAdded);
                        _symbolList.Add(toBeAdded.Name, toBeAdded.Value);
                    }
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
                lock (funcListLock)
                {
                    _funcList.Remove(toBeRemoved);
                    _symbolList.Remove(toBeRemoved.Name);
                }
            }
        }

        public void RemoveDataLabel(DataLabel toBeRemoved)
        {
            lock (symbolListLock)
            {
                lock (dataListLock)
                {
                    _dataList.Remove(toBeRemoved);
                    _symbolList.Remove(toBeRemoved.Name);
                    dataAddrs.RemoveWhere(x => x >= toBeRemoved.Offset && x < toBeRemoved.Offset + toBeRemoved.Length);
                }
            }
        }

        public void RemoveVarLabel(VarLabel toBeRemoved)
        {
            lock (symbolListLock)
            {
                lock (varListLock)
                {
                    _varList.Remove(toBeRemoved);
                    _symbolList.Remove(toBeRemoved.Name);
                }
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
            lock (dataListLock)
            {
                return dataAddrs.Contains(address);
            }
        }

        public int GetNextNonDataAddress(int address)
        {
            lock (dataListLock)
            {
                int offset = address;
                while (dataAddrs.Contains(offset++)) { }
                return offset;
            }
        }

        #endregion Adding, clearing, and removing labels

        #region Loading and Saving Label Files

        // TODO: Make sure everything saves and loads properly.

        public void LoadLabelFile(string fileName)
        {
            using (TextReader tr = new StreamReader(fileName))
            {
                using (TextWriter tw = new StreamWriter("err.txt"))
                {
                    if (tr.ReadLine() != "gbr")
                    {
                        return;
                    }
                    else
                    {
                        List<Dictionary<string, string>> items = new List<Dictionary<string, string>>();
                        int curItem = -1;
                        string currentLine;
                        while ((currentLine = tr.ReadLine()) != null)
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
                                    string[] opt = currentLine.Split(':');
                                    if (opt.Length == 1)
                                    {
                                        if (items[curItem].ContainsKey("_c"))
                                        {
                                            items[curItem]["_c"] = currentLine;
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
                        foreach(Dictionary<string, string> currentItem in items)
                        {
                            switch (currentItem["tag"])
                            {
                                case "label":
                                    {
                                        int offset = 0;
                                        string name = "";
                                        string comment = "";
                                        bool offsetGood = false;
                                        foreach (KeyValuePair<string, string> kvp in currentItem)
                                        {
                                            switch(kvp.Key)
                                            {
                                                case "_o":
                                                    {
                                                        offsetGood = InputValidation.TryParseOffsetString(kvp.Value, out offset);
                                                    }

                                                    break;

                                                case "_n":
                                                    {
                                                        name = kvp.Value;
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
                                            tw.WriteLine("Label #" + items.IndexOf(currentItem) + " was unrecognized.");
                                        }
                                    }

                                    break;

                                case "data":
                                    {
                                        int offset = -1;
                                        int length = -1;
                                        int dataDiv = 0;
                                        string name = "";
                                        string comment = "";
                                        DataSectionType dst = DataSectionType.Data;
                                        GBPalette gbp = new GBPalette();
                                        bool offsetGood = false;
                                        bool lengthGood = false;
                                        bool dataDivGood = false;
                                        foreach (KeyValuePair<string, string> kvp in currentItem)
                                        {
                                            switch (kvp.Key)
                                            {
                                                case "_o":
                                                    {
                                                        offsetGood = InputValidation.TryParseOffsetString(kvp.Value, out offset);
                                                    }

                                                    break;

                                                case "_l":
                                                    {
                                                        lengthGood = InputValidation.TryParseOffsetString(kvp.Value, out length);
                                                    }
                                                    break;

                                                case "_n":
                                                    {
                                                        if (RegularValidation.IsWord(kvp.Value))
                                                        {
                                                            name = kvp.Value;
                                                        }
                                                    }

                                                    break;
                                                case "_d":
                                                    {
                                                        dataDivGood = InputValidation.TryParseOffsetString(kvp.Value, out dataDiv);
                                                    }
                                                    break;

                                                case "_p1":
                                                    {
                                                        dst = DataSectionType.Image;
                                                        InputValidation.TryParseOffsetString(kvp.Value, out gbp.Col_1);
                                                    }

                                                    break;

                                                case "_p2":
                                                    {
                                                        dst = DataSectionType.Image;
                                                        InputValidation.TryParseOffsetString(kvp.Value, out gbp.Col_2);
                                                    }

                                                    break;

                                                case "_p3":
                                                    {
                                                        dst = DataSectionType.Image;
                                                        InputValidation.TryParseOffsetString(kvp.Value, out gbp.Col_3);
                                                    }

                                                    break;

                                                case "_p4":
                                                    {
                                                        dst = DataSectionType.Image;
                                                        InputValidation.TryParseOffsetString(kvp.Value, out gbp.Col_4);
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
                                            DataLabel ds = new DataLabel(offset, length, name, dataDiv, comment, dst, gbp);
                                            AddDataLabel(ds);
                                        }
                                        else
                                        {
                                            tw.WriteLine("Label #" + items.IndexOf(currentItem) + " was unrecognized.");
                                        }
                                    }

                                    break;

                                case "var":
                                    {
                                        int variable = -1;
                                        string name = "";
                                        string comment = "";
                                        bool variableGood = false;

                                        foreach (KeyValuePair<string, string> kvp in currentItem)
                                        {
                                            switch (kvp.Key)
                                            {
                                                case "_v":
                                                    {
                                                        variableGood = InputValidation.TryParseOffsetString(kvp.Value, out variable);
                                                    }

                                                    break;

                                                case "_n":
                                                    {
                                                        if (RegularValidation.IsWord(kvp.Value))
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
                                            tw.WriteLine("Label #" + items.IndexOf(currentItem) + " was unrecognized.");
                                        }
                                    }

                                    break;

                                case "comment":
                                    {
                                        int offset = 0;
                                        string name = String.Empty;
                                        string comment = "";
                                        bool offsetGood = false;

                                        foreach (KeyValuePair<string, string> kvp in currentItem)
                                        {
                                            switch (kvp.Key)
                                            {
                                                case "_o":
                                                    {
                                                        offsetGood = InputValidation.TryParseOffsetString(kvp.Value, out offset);
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
                                            tw.WriteLine("Label #" + items.IndexOf(currentItem) + " was unrecognized.");
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
                    functions.WriteLine("_o:" + s.Offset);
                }
                foreach (DataLabel s in DataList)
                {
                    functions.WriteLine(".data");
                    functions.WriteLine("_n:" + s.Name);
                    functions.WriteLine("_o:" + s.Offset.ToString("X"));
                    functions.WriteLine("_l:" + s.Length.ToString("X"));
                    functions.WriteLine("_t:" + s.DSectionType);
                    functions.WriteLine("_d:" + s.DataLineLength.ToString("X"));
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