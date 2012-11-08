namespace GBRead.Base
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    public class Table
    {
        private Dictionary<string, string> charValTable;
        private bool useAscii;

        public Table()
        {
            charValTable = new Dictionary<string, string>();
            useAscii = true;
        }

        public void LoadTableFile(string fileName, bool isShiftJIS = false)
        {
            ClearTable();
            using (StreamReader sr = new StreamReader(fileName, isShiftJIS ? Encoding.GetEncoding("shift_jis") : new UTF8Encoding()))
            {
                string curLine;
                while ((curLine = sr.ReadLine()) != null)
                {
                    if (curLine.StartsWith("//"))
                    {
                        continue;
                    }
                    string[] s = curLine.Split(new[] { '=' }, 2);
                    if (s.Length != 2 || s[1] == "")
                    {
                        continue;
                    }
                    long key = -1;
                    var keyAsString = "";
                    switch (s[0].Length)
                    {
                        case 1:
                        case 2:
                            if (!Utility.StringToLong("$" + s[0], out key))
                            {
                                continue;
                            }
                            keyAsString = s[0];
                            if (s[0].Length == 1)
                            {
                                keyAsString = "0" + keyAsString;
                            }
                            break;

                        case 3:
                        case 4:
                            if (!Utility.StringToLong("$" + s[0], out key))
                            {
                                continue;
                            }
                            keyAsString = s[0];
                            if (s[0].Length == 3)
                            {
                                keyAsString = "0" + keyAsString;
                            }
                            break;

                        case 5:
                        case 6:
                            if (!Utility.StringToLong("$" + s[0], out key))
                            {
                                continue;
                            }
                            keyAsString = s[0];
                            if (s[0].Length == 5)
                            {
                                keyAsString = "0" + keyAsString;
                            }
                            break;

                        default:
                            {
                                continue;
                            }
                    }
                    if (!AddKey(keyAsString, s[1]))
                    {
                        continue;
                    }
                }
            }
            if (charValTable.Count != 0)
            {
                useAscii = false;
            }
        }

        public void ClearTable()
        {
            charValTable.Clear();
            useAscii = true;
        }

        public bool AddKey(string num, string val)
        {
            if (charValTable.ContainsKey(num))
            {
                return false;
            }
            charValTable.Add(num, val);
            return true;
        }

        public string GetVal(byte[] data, long offset, long bitLength)
        {
            StringBuilder sb = new StringBuilder();
            if (offset > data.Length)
            {
                return sb.ToString();
            }
            var off = offset;
            while (off < offset + bitLength && off < data.Length)
            {
                if (useAscii)
                {
                    if (data[off] != 0)
                    {
                        sb.Append((char)data[off]);
                    }
                    else
                    {
                        sb.Append(@"\x00");
                    }
                    off++;
                }
                else
                {
                    var singleCode = "";
                    var doubleCode = "";
                    var tripleCode = "";
                    if (off < data.Length)
                    {
                        singleCode = data[off].ToString("X2");
                    }
                    if (off < data.Length - 1)
                    {
                        doubleCode = data[off].ToString("X2") + data[off + 1].ToString("X2");
                    }
                    if (off < data.Length - 2)
                    {
                        tripleCode = data[off].ToString("X2") + data[off + 1].ToString("X2") + data[off + 2].ToString("X2");
                    }
                    if (tripleCode != "" && charValTable.ContainsKey(tripleCode))
                    {
                        sb.Append(charValTable[tripleCode]);
                        off += 3;
                    }
                    else if (doubleCode != "" && charValTable.ContainsKey(doubleCode))
                    {
                        sb.Append(charValTable[doubleCode]);
                        off += 2;
                    }
                    else if (singleCode != "" && charValTable.ContainsKey(singleCode))
                    {
                        sb.Append(charValTable[singleCode]);
                        off += 1;
                    }
                    else
                    {
                        sb.AppendFormat("\\x{0:X2}", data[off]);
                        off++;
                    }
                }
            }
            return sb.ToString();
        }
    }
}