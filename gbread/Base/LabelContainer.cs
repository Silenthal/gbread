using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GBRead.Base
{
	public class LabelContainer
	{
		private object funcListLock = new object();
		private object dataListLock = new object();
		private object varListLock = new object();
		private object symbolListLock = new object();

		private List<GenericLabel> _funcList;
		private List<GenericLabel> _dataList;
		private List<GenericLabel> _varList;
		private HashSet<string> _symbolList = new HashSet<string>();

		private HashSet<int> dataAddrs;

		#region Default Var List

		private static List<GenericLabel> defaultVars = new List<GenericLabel>()
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
			new VarLabel(0xFF4F, "VBK"),
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

		public IList<GenericLabel> FuncList
		{
			get
			{
				lock (funcListLock)
				{
					return _funcList.AsReadOnly();
				}
			}
		}

		public IList<GenericLabel> DataList
		{
			get
			{
				lock (dataListLock)
				{
					return _dataList.AsReadOnly();
				}
			}
		}

		public IList<GenericLabel> VarList
		{
			get
			{
				lock (varListLock)
				{
					return _varList.AsReadOnly();
				}
			}
		}

		private HashSet<string> SymbolList
		{
			get
			{
				lock (symbolListLock)
				{
					return _symbolList;
				}
			}
		}

		public LabelContainer()
		{
			ClearAllLists();
		}

		public void LoadDefaultLabels(int newFileSize)
		{
			ClearAllLists();
			foreach (VarLabel vls in defaultVars)
			{
				AddVarLabel(vls);
			}
			AddDataLabel(new DataLabel(0x104, 0x4C, "Header"));
		}

		#region Adding, clearing, and removing labels

		public FunctionLabel TryGetFuncLabel(int current)
		{
			lock (funcListLock)
			{
				int x = _funcList.IndexOf(new FunctionLabel(current));
				if (x < 0) return null;
				return (FunctionLabel)_funcList[x];
			}
		}

		public DataLabel TryGetDataLabel(int current)
		{
			lock (dataListLock)
			{
				int x = _dataList.IndexOf(new DataLabel(current));
				if (x < 0) return null;
				return (DataLabel)_dataList[x];
			}
		}

		public VarLabel TryGetVarLabel(ushort current)
		{
			lock (varListLock)
			{
				int x = _varList.IndexOf(new VarLabel(current));
				if (x < 0) return null;
				return (VarLabel)_varList[x];
			}
		}

		public bool IsNameDefined(string name)
		{
			//Note: this function will not help if, in between calling this and
			//AddLabel, another thread adds a label with the same name
			//first.
			lock (symbolListLock)
			{
				if (String.IsNullOrEmpty(name)) return false;
				return _symbolList.Contains(name);
			}
		}

		public void AddFuncLabel(FunctionLabel toBeAdded)
		{
			lock (symbolListLock)
			{
				lock (funcListLock)
				{
					if (_symbolList.Contains(toBeAdded.Name)) return;
					_funcList.Add(toBeAdded);
					_symbolList.Add(toBeAdded.Name);
				}
			}
		}

		public void AddDataLabel(DataLabel toBeAdded)
		{
			lock (symbolListLock)
			{
				lock (dataListLock)
				{
					if (_symbolList.Contains(toBeAdded.Name)) return;
					_dataList.Add(toBeAdded);
					_symbolList.Add(toBeAdded.Name);
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
					if (_symbolList.Contains(toBeAdded.Name)) return;
					_varList.Add(toBeAdded);
					_symbolList.Add(toBeAdded.Name);
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
			lock (funcListLock)
			{
				lock (symbolListLock)
				{
					if (_funcList == null) _funcList = new List<GenericLabel>();
					if (_funcList.Count != 0)
					{
						foreach (GenericLabel l in _funcList)
						{

						}
					}
					_funcList.Clear();
				}
			}
		}

		public void ClearDataList()
		{
			lock (dataListLock)
			{
				if (_dataList == null) _dataList = new List<GenericLabel>();
				else _dataList.Clear();
				if (dataAddrs == null) dataAddrs = new HashSet<int>();
				dataAddrs.Clear();
			}
		}

		public void ClearVarList()
		{
			lock (varListLock)
			{
				if (_varList == null) _varList = new List<GenericLabel>();
				else _varList.Clear();
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
						return;
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

							#region Handler for CRC

							if (currentLine.Equals(".crc", StringComparison.OrdinalIgnoreCase))
							{
								//Check against file CRC here.
								//If mismatch, prompt the user, and continue from there.
								//Options:
								//-Ask to continue or not
								//-Silent fail
								//-Notify that file can't be loaded, and silent fail.
							}

							#endregion Handler for CRC

							#region Handler for labels

							else if (currentLine.Equals(".label", StringComparison.OrdinalIgnoreCase))
							{
								int offset = 0;
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
									FunctionLabel fl = new FunctionLabel(offset, name, cmtBuf.ToArray());
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

							#endregion Handler for labels

							#region Handler for data

							else if (currentLine.Equals(".data", StringComparison.OrdinalIgnoreCase))
							{
								int offset = -1;
								int length = -1;
								int dataDiv = 0;
								string name = String.Empty;
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
											dst = DataSectionType.Image;
											break;
										case 't':
											if (val == "Data")
											{
												dst = DataSectionType.Data;
											}
											else if (val == "Image")
											{
												dst = DataSectionType.Image;
											}
											break;
										default:
											break;
									}
								}
								if (offsetGood && lengthGood)
								{
									DataLabel ds = new DataLabel(offset, length, name, dataDiv, cmtBuf.ToArray(), dst);
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
									VarLabel vl = new VarLabel(variable, name, cmtBuf.ToArray());
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
			foreach (GenericLabel s in FuncList)
			{
				sb.AppendLine(s.ToSaveFileString());
			}
			return sb.ToString();
		}

		private string DataListToSaveFileFormat()
		{
			StringBuilder sb = new StringBuilder(String.Empty);
			foreach (GenericLabel s in DataList)
			{
				sb.AppendLine(s.ToSaveFileString());
			}
			return sb.ToString();
		}

		private string VarListToSaveFileFormat()
		{
			StringBuilder sb = new StringBuilder(String.Empty);
			foreach (GenericLabel s in VarList)
			{
				sb.AppendLine(s.ToSaveFileString());
			}
			return sb.ToString();
		}

		#endregion Loading and Saving Label Files		
	}
}