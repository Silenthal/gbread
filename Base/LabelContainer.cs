using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GBRead
{
	public class LabelContainer
	{
		private object labelListLock = new object();
		private object dataListLock = new object();
		private object varListLock = new object();

		private List<Label> _funcList;
		private List<Label> _dataList;
		private List<Label> _varList;

		private HashSet<int> dataAddrs;
		private HashSet<int> varTableDict;
		private HashSet<int> funcTableDict;
		private HashSet<int> dataTableDict;


		private bool isFuncListSorted = false;
		private bool isDataListSorted = false;
		private bool isVarListSorted = true;

		private ListSortOrder _funcListSortOrder;
		private ListSortOrder _dataListSortOrder;
		private ListSortOrder _varListSortOrder;

		private BinFile _coreFile;

		#region Default Var List

		private List<Label> defaultVars = new List<Label>()
		{
			new VarLabel(0xFF00, "JOYP"),
			new VarLabel(0xFF01, "SB"),
			new VarLabel(0xFF02, "SC"),
			new VarLabel(0xFF04, "DIV"),
			new VarLabel(0xFF05, "TIMA"),
			new VarLabel(0xFF06, "TMA"),
			new VarLabel(0xFF07, "TAC"),
			new VarLabel(0xFF0F, "IF"),
			new VarLabel(0xFF10, "NR10"),
			new VarLabel(0xFF11, "NR11"),
			new VarLabel(0xFF12, "NR12"),
			new VarLabel(0xFF13, "NR13"),
			new VarLabel(0xFF14, "NR14"),
			new VarLabel(0xFF16, "NR21"),
			new VarLabel(0xFF17, "NR22"),
			new VarLabel(0xFF18, "NR23"),
			new VarLabel(0xFF19, "NR24"),
			new VarLabel(0xFF1A, "NR30"),
			new VarLabel(0xFF1B, "NR31"),
			new VarLabel(0xFF1C, "NR32"),
			new VarLabel(0xFF1D, "NR33"),
			new VarLabel(0xFF1E, "NR34"),
			new VarLabel(0xFF20, "NR41"),
			new VarLabel(0xFF21, "NR42"),
			new VarLabel(0xFF22, "NR43"),
			new VarLabel(0xFF23, "NR44"),
			new VarLabel(0xFF24, "NR50"),
			new VarLabel(0xFF25, "NR51"),
			new VarLabel(0xFF26, "NR52"),
			new VarLabel(0xFF40, "LCDC"),
			new VarLabel(0xFF41, "STAT"),
			new VarLabel(0xFF42, "SCY"),
			new VarLabel(0xFF43, "SCX"),
			new VarLabel(0xFF44, "LY"),
			new VarLabel(0xFF45, "LYC"),
			new VarLabel(0xFF46, "DMA"),
			new VarLabel(0xFF47, "BGP"),
			new VarLabel(0xFF48, "OBP0"),
			new VarLabel(0xFF49, "OBP1"),
			new VarLabel(0xFF4A, "WY"),
			new VarLabel(0xFF4B, "WX"),
			new VarLabel(0xFF4D, "KEY1"),
			new VarLabel(0xFF4F, "VBNK"),
			new VarLabel(0xFF51, "HDMA1", new string[1]{"New DMA Source - High"}),
			new VarLabel(0xFF52, "HDMA2", new string[1]{"New DMA Source - Low"}),
			new VarLabel(0xFF53, "HDMA3", new string[1]{"New DMA Destination - High"}),
			new VarLabel(0xFF54, "HDMA4", new string[1]{"New DMA Destination - Low"}),
			new VarLabel(0xFF55, "HDMA5", new string[1]{"New DMA Length/Mode/Start"}),
			new VarLabel(0xFF56, "RP"),
			new VarLabel(0xFF68, "BGPI"),
			new VarLabel(0xFF69, "BGPD"),
			new VarLabel(0xFF6A, "OBPI"),
			new VarLabel(0xFF6B, "OBPD"),
			new VarLabel(0xFF70, "SVBK"),
			new VarLabel(0xFFFF, "IE"),
		};

		#endregion Default Var List

		public List<Label> FuncList
		{
			get
			{
				lock (labelListLock)
				{
					if (!isFuncListSorted)
					{
						_funcList.Sort(new LabelComparer(FuncListSortOrder));
						isFuncListSorted = true;
					}
					return _funcList;
				}
			}
		}
		public List<Label> DataList
		{
			get
			{
				lock (dataListLock)
				{
					if (!isDataListSorted)
					{
						_dataList.Sort(new LabelComparer(DataListSortOrder));
						isDataListSorted = true;
					}
					return _dataList;
				}
			}
		}
		public List<Label> VarList
		{
			get
			{
				lock (varListLock)
				{
					if (!isVarListSorted)
					{
						_varList.Sort(new LabelComparer(VarListSortOrder));
						isVarListSorted = true;
					}
					return _varList;
				}
			}
		}

		public ListSortOrder FuncListSortOrder 
		{
			get
			{
				lock (labelListLock)
				{
					return _funcListSortOrder;
				}
			}
			set
			{
				lock (labelListLock)
				{
					if (value != _funcListSortOrder)
					{
						_funcListSortOrder = value;
						isFuncListSorted = false;
					}
				}
			}
		}
		public ListSortOrder DataListSortOrder
		{
			get
			{
				lock (dataListLock)
				{
					return _dataListSortOrder;
				}
			}
			set
			{
				lock (dataListLock)
				{
					if (value != _dataListSortOrder)
					{
						_dataListSortOrder = value;
						isDataListSorted = false;
					}
				}
			}
		}
		public ListSortOrder VarListSortOrder 
		{
			get
			{
				lock (varListLock)
				{
					return _varListSortOrder;
				}
			}
			set
			{
				lock (varListLock)
				{
					if (value != _varListSortOrder)
					{
						_varListSortOrder = value;
						isVarListSorted = false;
					}
				}
			}
		}

		public BinFile CoreFile { get { return _coreFile; } set { _coreFile = value; } }

		public LabelContainer()
		{
			ClearAllLists();
		}

		public void LoadDefaultLabels(int newFileSize)
		{
			ClearFuncList();
			ClearVarList();
			foreach (VarLabel vls in defaultVars)
			{
				AddLabel(vls);
			}
			ClearDataList();
			AddLabel(new DataLabel(0x104, 0x4C, "Header"));
		}

		public void GetOptions(Options options)
		{
			_funcListSortOrder = options.LabelContainer_FuncListSortOrder;
			_dataListSortOrder = options.LabelContainer_DataListSortOrder;
			_varListSortOrder = options.LabelContainer_VarListSortOrder;
		}

		public void SetOptions(ref Options options)
		{
			options.LabelContainer_FuncListSortOrder = _funcListSortOrder;
			options.LabelContainer_DataListSortOrder = _dataListSortOrder;
			options.LabelContainer_VarListSortOrder = _varListSortOrder;
		}

		#region Adding, clearing, and removing labels

		public bool AddFuncLabel(int offset, string labelName = "", int labelLength = 0, string[] comment = null)
		{
			FunctionLabel toBeAdded = new FunctionLabel(offset, labelName, labelLength, comment);
			bool contains = false;
			lock (labelListLock)
			{
				contains = funcTableDict.Contains(toBeAdded.Value);
			}
			if (contains)
			{
				RemoveLabel(toBeAdded);
			}
			lock (labelListLock)
			{
				_funcList.Add(toBeAdded);
				funcTableDict.Add(toBeAdded.Value);
				isFuncListSorted = false;
			}
			return true;
		}

		public bool AddDataLabel(DataSectionType dsType, int offset, string labelName, int labelLength, int dataDivisionLength, string[] comment = null, GBPalette newPalette = null)
		{
			DataLabel dsAdded = new DataLabel(offset, labelLength, labelName, dataDivisionLength, comment, dsType, newPalette);
			return AddLabel(dsAdded);
		}

		public bool AddVarLabel(int variable, string name, string[] comment)
		{
			if (variable < 0) return false;
			VarLabel vlAdded = new VarLabel(variable, name, comment);
			return AddLabel(vlAdded);
		}

		public bool AddLabel(Label toBeAdded)
		{
			if (toBeAdded == null) return false;
			bool contains = false;
			if (toBeAdded is VarLabel)
			{
				lock (varListLock)
				{
					contains = varTableDict.Contains(toBeAdded.Value);
				}
				if (contains)
				{
					RemoveLabel(toBeAdded);
				}
				lock (varListLock)
				{
					_varList.Add(toBeAdded);
					varTableDict.Add(toBeAdded.Value);
					isVarListSorted = false;
				}
				return true;
			}
			else if (toBeAdded is FunctionLabel)
			{
				lock (labelListLock)
				{
					contains = funcTableDict.Contains(toBeAdded.Value);
				}
				if (contains)
				{
					RemoveLabel(toBeAdded);
				}
				lock (labelListLock)
				{
					_funcList.Add(toBeAdded);
					funcTableDict.Add(toBeAdded.Value);
					isFuncListSorted = false;
				}
				return true;
			}
			else if (toBeAdded is DataLabel)
			{
				lock (dataListLock)
				{
					contains = dataTableDict.Contains(toBeAdded.Value);
				}
				if (contains)
				{
					RemoveLabel(toBeAdded);
				}
				lock (dataListLock)
				{
					_dataList.Add(toBeAdded);
					dataTableDict.Add(toBeAdded.Value);
					RegisterDataAddresses(((DataLabel)toBeAdded).Offset, ((DataLabel)toBeAdded).Length);
					isDataListSorted = false;
				}
				return true;
			}
			else return false;
		}

		public void RemoveLabel(Label toBeRemoved)
		{
			if (toBeRemoved is FunctionLabel)
			{
				lock (labelListLock)
				{
					_funcList.Remove(toBeRemoved);
					funcTableDict.Remove(toBeRemoved.Value);
				}
			}
			else if (toBeRemoved is DataLabel)
			{
				lock (dataListLock)
				{
					_dataList.Remove(toBeRemoved);
					dataTableDict.Remove(toBeRemoved.Value);
					DeregisterDataAddresses(((DataLabel)toBeRemoved).Offset, ((DataLabel)toBeRemoved).Length);
				}
			}
			else if (toBeRemoved is VarLabel)
			{
				lock (varListLock)
				{
					_varList.Remove(toBeRemoved);
					varTableDict.Remove(toBeRemoved.Value);
				}
			}
		}

		private void RegisterDataAddresses(int offset, int length)
		{
			for (int i = offset; i < offset + length; i++)
			{
				if (!dataAddrs.Contains(i)) dataAddrs.Add(i);
			}
		}

		private void DeregisterDataAddresses(int offset, int length)
		{
			dataAddrs.RemoveWhere(x => x > offset && x < offset + length);
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

		public void ClearAllLists()
		{
			ClearFuncList();
			ClearDataList();
			ClearVarList();
		}

		public void ClearFuncList()
		{
			lock (labelListLock)
			{
				if (_funcList == null) _funcList = new List<Label>();
				_funcList.Clear();
				if (funcTableDict == null) funcTableDict = new HashSet<int>();
				funcTableDict.Clear();
			}
		}

		public void ClearDataList()
		{
			lock (dataListLock)
			{
				if (_dataList == null) _dataList = new List<Label>();
				else _dataList.Clear();
				if (dataTableDict == null) dataTableDict = new HashSet<int>();
				else dataTableDict.Clear();
				if (dataAddrs == null) dataAddrs = new HashSet<int>();
				dataAddrs.Clear();
			}
		}

		public void ClearVarList()
		{
			lock (varListLock)
			{
				if (_varList == null) _varList = new List<Label>();
				else _varList.Clear();
				if (varTableDict == null) varTableDict = new HashSet<int>();
				else varTableDict.Clear();
			}
		}

		public bool Contains(Label ls)
		{
			if (ls == null) return false;
			if (ls is FunctionLabel)
			{
				lock (labelListLock)
				{
					return funcTableDict.Contains(ls.Value);
				}
			}
			if (ls is VarLabel)
			{
				lock (varListLock)
				{
					return varTableDict.Contains(ls.Value);
				}
			}
			if (ls is DataLabel)
			{
				lock (dataListLock)
				{
					return dataTableDict.Contains(ls.Value);
				}
			}
			return false;
		}

		public Label TryGetFuncLabel(int value)
		{
			lock (labelListLock)
			{
				if (funcTableDict.Contains(value)) return _funcList.Find(x => x.Value == value);
				else return null;
			}
		}

		public Label TryGetDataLabel(int value)
		{
			lock (dataListLock)
			{
				if (dataTableDict.Contains(value)) return _dataList.Find(x => x.Value == value);
				else return null;
			}
		}

		public Label TryGetVarLabel(int value)
		{
			lock (varListLock)
			{
				if (varTableDict.Contains(value))
				{
					if (varTableDict.Contains(value)) return _varList.Find(x => x.Value == value);
					else return null;
				}
				else return null;
			}
		}

		#endregion Adding, clearing, and removing labels

		#region Loading and Saving Label Files

		public void LoadLabelFile(string fileName)
		{
			using (TextReader tr = new StreamReader(fileName))
			{
				using (TextWriter tw = new StreamWriter("err.txt"))
				{
					char[] test = new char[3];
					tr.Read(test, 0, 3);
					if (test[0] != 'g' || test[1] != 'b' || test[2] != 'r')
					{
						LoadLabelFile_Old(tr, tw);
					}
					else
					{
						string currentLine;
						while ((currentLine = tr.ReadLine()) != null)
						{
							List<string> buf = new List<string>();
							while (tr.Peek() == '_')
							{
								buf.Add(tr.ReadLine());
							}

							#region Handler for labels

							if (currentLine.Equals(".label", StringComparison.OrdinalIgnoreCase))
							{
								int offset = 0;
								int length = 0;
								string name = String.Empty;
								List<string> cmtBuf = new List<string>();
								bool offsetGood = false;
								foreach (string x in buf)
								{
									string code = x.Substring(0, 3);
									string val = x.Substring(3, x.Length - 3);
									if (code.Equals("_o:", StringComparison.OrdinalIgnoreCase))
									{
										offsetGood = InputValidation.TryParseOffsetString(val, out offset);
									}
									else if (code.Equals("_l:", StringComparison.OrdinalIgnoreCase))
									{
										if (!InputValidation.TryParseOffsetString(val, out length))
										{
											length = 0;
										}
									}
									else if (code.Equals("_n:", StringComparison.OrdinalIgnoreCase) && RegularValidation.IsWord(val))
									{
										name = val;
									}
									else if (code.Equals("_c:", StringComparison.OrdinalIgnoreCase))
									{
										cmtBuf.Add(val);
									}
								}
								if (offsetGood)
								{
									AddFuncLabel(offset, name, length, cmtBuf.ToArray());
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

							#endregion Handler for labels

							#region Handler for data

							else if (currentLine.Equals(".data", StringComparison.OrdinalIgnoreCase))
							{
								int offset = -1;
								int length = -1;
								int dataDiv = 0;
								string name = String.Empty;
								GBPalette dataPalette = new GBPalette();
								DataSectionType dst = DataSectionType.Data;
								List<string> cmtBuf = new List<string>();
								bool offsetGood = false;
								bool lengthGood = false;
								bool dataDivGood = false;
								foreach (string x in buf)
								{
									string code = x.Substring(0, 3);
									string val = x.Substring(3, x.Length - 3);
									switch (code[1])
									{
										case 'o':
											offsetGood = InputValidation.TryParseOffsetString(val, out offset);
											break;
										case 'l':
											lengthGood = InputValidation.TryParseOffsetString(val, out length);
											break;
										case 'd':
											dataDivGood = InputValidation.TryParseOffsetString(val, out dataDiv);
											break;
										case 'n':
											if (RegularValidation.IsWord(val)) name = val;
											break;
										case 'c':
											cmtBuf.Add(val);
											break;
										case 'p':
											{
												dst = DataSectionType.Img;
												int colval;
												bool colvalGood = InputValidation.TryParseOffsetString(val.Substring(1, val.Length - 1), out colval);
												if (colvalGood)
												{
													switch (code[2])
													{
														case '1':
															dataPalette.Col_1 = colval;
															break;
														case '2':
															dataPalette.Col_2 = colval;
															break;
														case '3':
															dataPalette.Col_3 = colval;
															break;
														case '4':
															dataPalette.Col_4 = colval;
															break;
														default:
															break;
													}
												}
											}
											break;
										default:
											break;
									}
								}
								if (offsetGood && lengthGood)
								{
									AddDataLabel(dst, offset, name, length, dataDiv, cmtBuf.ToArray(), dataPalette);
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

							#endregion Handler for data

							#region Handler for variables

							else if (currentLine.Equals(".var"))
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
											if (RegularValidation.IsWord(val)) name = val;
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
									AddVarLabel(variable, name, cmtBuf.ToArray());
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

							#endregion Handler for variables

							else
							{
								tw.WriteLine("Unrecognized section heading: " + currentLine);
							}
						}
					}
				}
			}
		}

		public void LoadLabelFile_Old(TextReader tr, TextWriter tw)
		{
			string currentline;
			while ((currentline = tr.ReadLine()) != null)
			{
				string[] line = currentline.Split(new Char[] { ',' });

				#region Loading Labels

				if (line[0].Equals("label", StringComparison.OrdinalIgnoreCase) && line.Length >= 5)
				{
					int off;
					int len;
					bool offGood = InputValidation.TryParseOffsetString(line[1], out off);
					bool lenGood = InputValidation.TryParseOffsetString(line[3], out len);
					if (!offGood)
					{
						tw.WriteLine("One of the lines of your function list ({0}) has an invalid offset.", currentline);
					}
					else if (!lenGood)
					{
						tw.WriteLine("One of the lines of your function list ({0}) has an invalid length.", currentline);
					}
					else
					{
						List<string> comment = null;
						if (line.Length >= 5)
						{
							comment = new List<string>();
							for (int i = 4; i < line.Length; i++)
							{
								comment.Add(line[i]);
							}
						}
						AddFuncLabel(off, line[2], len, comment.ToArray());
					}
				}

				#endregion Loading Labels

				#region Loading Data

				else if (line[0].Equals("data", StringComparison.OrdinalIgnoreCase))
				{
					int off;
					int len;
					bool offGood = InputValidation.TryParseOffsetString(line[1], out off);
					bool lenGood = InputValidation.TryParseOffsetString(line[3], out len);
					if (off < 0)
					{
						tw.WriteLine("One of the lines of your function list ({0}) has an invalid offset.", currentline);
					}
					else if (len < 1)
					{
						tw.WriteLine("One of the lines of your function list ({0}) has an invalid length.", currentline);
					}
					else
					{
						List<string> comment = null;
						if (line.Length >= 5)
						{
							comment = new List<string>();
							for (int i = 4; i < line.Length; i++)
							{
								comment.Add(line[i]);
							}
						}
						AddDataLabel(DataSectionType.Data, off, line[2], len, 0, comment.ToArray());
					}
				}

				#endregion Loading Data

				#region Loading Vars

				else if (line[0].Equals("var", StringComparison.OrdinalIgnoreCase))
				{
					if (line.Length < 2)
					{
						tw.WriteLine("One of the lines of your function list ({0}) can't be loaded.", currentline);
					}
					else
					{
						int off;
						bool offGood = InputValidation.TryParseOffsetString(line[1], out off);
						if (off < 0)
						{
							tw.WriteLine("One of the lines of your function list ({0}) has an invalid offset.", currentline);
						}
						else if (off > 0xFFFF)
						{
							tw.WriteLine("One of the lines of your function list ({0}) has an offset larget than 0xFFFF.", currentline);
						}
						else
						{
							List<string> comment = null;
							if (line.Length >= 4)
							{
								comment = new List<string>();
								for (int i = 3; i < line.Length; i++)
								{
									comment.Add(line[i]);
								}
							}
							AddVarLabel(off, line[2], comment.ToArray());
						}
					}
				}

				#endregion Loading Vars
			}
		}

		public void SaveLabelFile(string fileName)
		{
			using (TextWriter functions = new StreamWriter(fileName, false, Encoding.UTF8))
			{
				functions.WriteLine("gbr");
				functions.WriteLine(FunctionListToSaveFileFormat());
				functions.WriteLine(DataListToSaveFileFormat());
				functions.WriteLine(VarListToSaveFileFormat());
				functions.Close();
			}
		}

		private string FunctionListToSaveFileFormat()
		{
			StringBuilder sb = new StringBuilder(String.Empty);
			lock (labelListLock)
			{
				foreach (Label s in _funcList)
				{
					sb.AppendLine(s.ToSaveFileString());
				}
			}
			return sb.ToString();
		}

		private string DataListToSaveFileFormat()
		{
			StringBuilder sb = new StringBuilder(String.Empty);
			lock (dataListLock)
			{
				foreach (Label s in _dataList)
				{
					sb.AppendLine(s.ToSaveFileString());
				}
			}
			return sb.ToString();
		}

		private string VarListToSaveFileFormat()
		{
			StringBuilder sb = new StringBuilder(String.Empty);
			lock (varListLock)
			{
				foreach (Label s in _varList)
				{
					sb.AppendLine(s.ToSaveFileString());
				}
			}
			return sb.ToString();
		}

		#endregion Loading and Saving Label Files
	}
}