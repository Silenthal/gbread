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
                lock (commentListLock)
                {
                    _commentList.Clear();
                }
                _symbolList.Clear();
            }
            if (File.Exists("default.txt"))
            {
                LoadLabelFile("default.txt");
            }
        }

        #region Adding, clearing, and removing labels

        public bool TryGetFuncLabel(int current, out FunctionLabel label)
        {
            lock (funcListLock)
            {
                var s = from item in _funcList where item.Value == current select item;
                var success = s.Count() != 0;
                label = success ? s.First() : new FunctionLabel(current);
                return success;
            }
        }

        public bool TryGetDataLabel(int current, out DataLabel label)
        {
            lock (dataListLock)
            {
                var s = from item in _dataList where item.Value == current select item;
                var success = s.Count() != 0;
                label = success ? s.First() : new DataLabel(current);
                return success;
            }
        }

        public bool TryGetVarLabel(ushort current, out VarLabel label)
        {
            lock (varListLock)
            {
                var s = from item in _varList where item.Value == current select item;
                var success = s.Count() != 0;
                label = success ? s.First() : new VarLabel(current);
                return success;
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
            int offset = address;
            lock (dataListLock)
            {
                while (dataAddrs.Contains(offset++)) { }
            }
            return offset;
        }

        public bool IsNameDefined(string name)
        {
            //Note: this function will not help if, in between calling this and
            //AddLabel, another thread adds a label with the same name
            //first.
            lock (symbolListLock)
            {
                if (String.IsNullOrEmpty(name))
                    return false;
                return _symbolList.ContainsKey(name);
            }
        }

        public void AddFuncLabel(FunctionLabel toBeAdded)
        {
            lock (symbolListLock)
            {
                lock (funcListLock)
                {
                    if (_symbolList.ContainsKey(toBeAdded.Name) || _funcList.Contains(toBeAdded))
                        return;
                    _funcList.Add(toBeAdded);
                    _symbolList.Add(toBeAdded.Name, toBeAdded.Offset);
                }
            }
        }

        public void AddDataLabel(DataLabel toBeAdded)
        {
            lock (symbolListLock)
            {
                lock (dataListLock)
                {
                    if (_symbolList.ContainsKey(toBeAdded.Name) || _dataList.Contains(toBeAdded))
                        return;
                    _dataList.Add(toBeAdded);
                    _symbolList.Add(toBeAdded.Name, toBeAdded.Value);
                    RegisterDataAddresses(toBeAdded.Offset, toBeAdded.Length);
                }
            }
        }

        public void AddVarLabel(VarLabel toBeAdded)
        {
            lock (symbolListLock)
            {
                lock (varListLock)
                {
                    if (_symbolList.ContainsKey(toBeAdded.Name) || _varList.Contains(toBeAdded))
                        return;
                    _varList.Add(toBeAdded);
                    _symbolList.Add(toBeAdded.Name, toBeAdded.Value);
                }
            }
        }

        public void AddComment(int offset, string comment)
        {
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
                    DeregisterDataAddresses(toBeRemoved.Offset, toBeRemoved.Length);
                    _symbolList.Remove(toBeRemoved.Name);
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

        private void RegisterDataAddresses(int offset, int length)
        {
            for (int i = offset; i < offset + length; i++)
            {
                dataAddrs.Add(i);
            }
        }

        private void DeregisterDataAddresses(int offset, int length)
        {
            dataAddrs.RemoveWhere(x => x >= offset && x < offset + length);
        }

        #endregion Adding, clearing, and removing labels

        #region Loading and Saving Label Files

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
                        string currentLine;
                        while ((currentLine = tr.ReadLine()) != null)
                        {
                            List<string> buf = new List<string>();
                            while (tr.Peek() != '.')
                            {
                                string s = tr.ReadLine();
                                if (s != "")
                                {
                                    buf.Add(tr.ReadLine());
                                }
                            }
                            switch (currentLine.ToLower())
                            {
                                case ".label":
                                    {
                                        int offset = 0;
                                        string name = "";
                                        List<string> cmtBuf = new List<string>();
                                        bool offsetGood = false;
                                        foreach (string x in buf)
                                        {
                                            string[] opt = x.Split(':');
                                            if (opt.Length == 1)
                                            {
                                                cmtBuf.Add(x);
                                            }
                                            switch (opt[0].ToLower())
                                            {
                                                case "_o":
                                                    {
                                                        offsetGood = InputValidation.TryParseOffsetString(opt[1], out offset);
                                                    }

                                                    break;

                                                case "_n":
                                                    {
                                                        name = opt[1];
                                                    }

                                                    break;

                                                case "_c":
                                                    {
                                                        cmtBuf.Add(x.Substring(0, 3));
                                                    }

                                                    break;
                                                default:
                                                    {
                                                        cmtBuf.Add(x);
                                                    }

                                                    break;
                                            }
                                        }
                                        if (offsetGood)
                                        {
                                            FunctionLabel fl = new FunctionLabel(offset, name, string.Join(Environment.NewLine, cmtBuf.ToArray()));
                                            AddFuncLabel(fl);
                                        }
                                        else
                                        {
                                            tw.WriteLine("Unrecognized section:");
                                            foreach (string x in buf)
                                            {
                                                tw.WriteLine(x);
                                            }
                                        }
                                    }
                                    break;

                                case ".data":
                                    {
                                        int offset = -1;
                                        int length = -1;
                                        int dataDiv = 0;
                                        string name = String.Empty;
                                        DataSectionType dst = DataSectionType.Data;
                                        List<string> cmtBuf = new List<string>();
                                        GBPalette gbp = new GBPalette();
                                        bool offsetGood = false;
                                        bool lengthGood = false;
                                        bool dataDivGood = false;
                                        foreach (string x in buf)
                                        {
                                            string[] opt = x.Split(':');
                                            if (opt.Length == 1)
                                            {
                                                cmtBuf.Add(x);
                                            }
                                            switch (opt[0].ToLower())
                                            {
                                                case "_o":
                                                    {
                                                        offsetGood = InputValidation.TryParseOffsetString(opt[1], out offset);
                                                    }

                                                    break;

                                                case "_l":
                                                    {
                                                        lengthGood = InputValidation.TryParseOffsetString(opt[1], out length);
                                                    }
                                                    break;

                                                case "_d":
                                                    {
                                                        dataDivGood = InputValidation.TryParseOffsetString(opt[1], out dataDiv);
                                                    }
                                                    break;

                                                case "_n":
                                                    if (RegularValidation.IsWord(opt[1]))
                                                    {
                                                        name = opt[1];
                                                    }

                                                    break;

                                                case "_c":
                                                    {
                                                        cmtBuf.Add(x.Substring(0, 3));
                                                    }

                                                    break;

                                                case "_p1":
                                                    {
                                                        dst = DataSectionType.Image;
                                                        InputValidation.TryParseOffsetString(opt[1], out gbp.Col_1);
                                                    }

                                                    break;

                                                case "_p2":
                                                    {
                                                        dst = DataSectionType.Image;
                                                        InputValidation.TryParseOffsetString(opt[1], out gbp.Col_2);
                                                    }

                                                    break;

                                                case "_p3":
                                                    {
                                                        dst = DataSectionType.Image;
                                                        InputValidation.TryParseOffsetString(opt[1], out gbp.Col_3);
                                                    }

                                                    break;

                                                case "_p4":
                                                    {
                                                        dst = DataSectionType.Image;
                                                        InputValidation.TryParseOffsetString(opt[1], out gbp.Col_4);
                                                    }

                                                    break;
                                                default:
                                                    {
                                                        cmtBuf.Add(x);
                                                    }
                                                    break;
                                            }
                                        }
                                        if (offsetGood && lengthGood)
                                        {
                                            DataLabel ds = new DataLabel(offset, length, name, dataDiv, string.Join(Environment.NewLine, cmtBuf.ToArray()), dst, gbp);
                                            AddDataLabel(ds);
                                        }
                                        else
                                        {
                                            tw.WriteLine("Unrecognized section:");
                                            foreach (string x in buf)
                                            {
                                                tw.WriteLine(x);
                                            }
                                        }
                                    }

                                    break;

                                case ".var":
                                    {
                                        int variable = -1;
                                        string name = String.Empty;
                                        List<string> cmtBuf = new List<string>();
                                        bool variableGood = false;
                                        foreach (string x in buf)
                                        {
                                            string code = x.Substring(0, 3);
                                            string val = x.Substring(3, x.Length - 3);
                                            switch (code[1])
                                            {
                                                case 'v':
                                                    variableGood = InputValidation.TryParseOffsetString(val, out variable);
                                                    break;

                                                case 'n':
                                                    if (RegularValidation.IsWord(val))
                                                        name = val;
                                                    break;

                                                case 'c':
                                                    cmtBuf.Add(val);
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }
                                        if (variableGood)
                                        {
                                            VarLabel vl = new VarLabel(variable, name, string.Join(Environment.NewLine, cmtBuf.ToArray()));
                                            AddVarLabel(vl);
                                        }
                                        else
                                        {
                                            tw.WriteLine("Unrecognized section:");
                                            foreach (string x in buf)
                                            {
                                                tw.WriteLine(x);
                                            }
                                        }
                                    }

                                    break;

                                case ".comment":
                                    {
                                        int offset = 0;
                                        string name = String.Empty;
                                        List<string> cmtBuf = new List<string>();
                                        bool offsetGood = false;
                                        foreach (string x in buf)
                                        {
                                            string code = x.Substring(0, 3).ToLower();
                                            string val = x.Substring(3, x.Length - 3);
                                            switch (code)
                                            {
                                                case "_o:":
                                                    offsetGood = InputValidation.TryParseOffsetString(val, out offset);
                                                    break;
                                                default:
                                                    cmtBuf.Add(val);
                                                    break;
                                            }
                                        }
                                        if (offsetGood)
                                        {
                                            AddComment(offset, string.Join(Environment.NewLine, cmtBuf.ToArray()));
                                        }
                                        else
                                        {
                                            tw.WriteLine("Unrecognized section:");
                                            foreach (string x in buf)
                                            {
                                                tw.WriteLine(x);
                                            }
                                        }
                                    }
                                    break;
                                default:
                                    tw.WriteLine("Unrecognized section heading: " + currentLine);
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
                if (FuncList.Count != 0)
                {
                    functions.WriteLine(FunctionListToSaveFileFormat());
                }
                if (DataList.Count != 0)
                {
                    functions.WriteLine(DataListToSaveFileFormat());
                }
                if (VarList.Count != 0)
                {
                    functions.WriteLine(VarListToSaveFileFormat());
                }
                if (_commentList.Count != 0)
                {
                    functions.WriteLine(CommentListToSaveFileFormat());
                }
                functions.Close();
            }
        }

        // TODO: Make sure everything saves and loads properly.

        private string FunctionListToSaveFileFormat()
        {
            StringBuilder sb = new StringBuilder(String.Empty);
            foreach (FunctionLabel s in FuncList)
            {
                sb.AppendLine(".label");
                sb.AppendLine("_n:" + s.Name);
                sb.AppendLine("_o:" + s.Offset);
            }
            return sb.ToString();
        }

        private string DataListToSaveFileFormat()
        {
            StringBuilder sb = new StringBuilder(String.Empty);
            foreach (DataLabel s in DataList)
            {
                sb.AppendLine(".data");
                sb.AppendLine("_n:" + s.Name);
                sb.AppendLine("_o:" + s.Offset.ToString("X"));
                sb.AppendLine("_l:" + s.Length.ToString("X"));
                sb.AppendLine("_t:" + s.DSectionType);
                sb.AppendLine("_d:" + s.DataLineLength.ToString("X"));
                if (s.DSectionType == DataSectionType.Image)
                {
                    sb.AppendLine("_p1:" + s.Palette.Col_1.ToString("X"));
                    sb.AppendLine("_p2:" + s.Palette.Col_2.ToString("X"));
                    sb.AppendLine("_p3:" + s.Palette.Col_3.ToString("X"));
                    sb.AppendLine("_p4:" + s.Palette.Col_4.ToString("X"));
                }
            }
            return sb.ToString();
        }

        private string VarListToSaveFileFormat()
        {
            StringBuilder sb = new StringBuilder(String.Empty);
            foreach (VarLabel s in VarList)
            {
                sb.AppendLine(".var");
                sb.AppendLine("_n:" + s.Name);
                sb.AppendLine("_v:" + s.Variable.ToString("X"));
            }
            return sb.ToString();
        }

        private string CommentListToSaveFileFormat()
        {
            if (_commentList.Count == 0)
            {
                return "";
            }
            StringBuilder ret = new StringBuilder();
            foreach (KeyValuePair<int, string> kvp in _commentList)
            {
                ret.AppendLine(".comment");
                ret.AppendLine("_o:" + kvp.Key.ToString("X"));
                ret.AppendLine("_c:" + kvp.Value);
            }
            return ret.ToString();
        }

        #endregion Loading and Saving Label Files
    }
}