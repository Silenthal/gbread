using System;

namespace GBRead.Base
{
	public enum DataSectionType { Data, Image }

	public enum VariableType { Byte, Word }

	public enum OffsetFormat { Hex, Decimal, BBOO }

	public class GBPalette
	{
		public int Color_1_Red { get { return (Col_1 & 0x1F) << 3; } set { Col_1 = (Col_1 & 0x7FE0) | ((value >> 3) & 0x1F); } }
		public int Color_1_Green { get { return ((Col_1 >> 5) & 0x1F) << 3; } set { Col_1 = (Col_1 & 0x7C1F) | (((value >> 3) & 0x1F) << 5); } }
		public int Color_1_Blue { get { return ((Col_1 >> 10) & 0x1F) << 3; } set { Col_1 = (Col_1 & 0x3FF) | (((value >> 3) & 0x1F) << 10); } }
		public int Color_2_Red { get { return (Col_2 & 0x1F) << 3; } set { Col_2 = (Col_2 & 0x7FE0) | ((value >> 3) & 0x1F); } }
		public int Color_2_Green { get { return ((Col_2 >> 5) & 0x1F) << 3; } set { Col_2 = (Col_2 & 0x7C1F) | (((value >> 3) & 0x1F) << 5); } }
		public int Color_2_Blue { get { return ((Col_2 >> 10) & 0x1F) << 3; } set { Col_2 = (Col_2 & 0x3FF) | (((value >> 3) & 0x1F) << 10); } }
		public int Color_3_Red { get { return (Col_3 & 0x1F) << 3; } set { Col_3 = (Col_3 & 0x7FE0) | ((value >> 3) & 0x1F); } }
		public int Color_3_Green { get { return ((Col_3 >> 5) & 0x1F) << 3; } set { Col_3 = (Col_3 & 0x7C1F) | (((value >> 3) & 0x1F) << 5); } }
		public int Color_3_Blue { get { return ((Col_3 >> 10) & 0x1F) << 3; } set { Col_3 = (Col_3 & 0x3FF) | (((value >> 3) & 0x1F) << 10); } }
		public int Color_4_Red { get { return (Col_4 & 0x1F) << 3; } set { Col_4 = (Col_4 & 0x7FE0) | ((value >> 3) & 0x1F); } }
		public int Color_4_Green { get { return ((Col_4 >> 5) & 0x1F) << 3; } set { Col_4 = (Col_4 & 0x7C1F) | (((value >> 3) & 0x1F) << 5); } }
		public int Color_4_Blue { get { return ((Col_4 >> 10) & 0x1F) << 3; } set { Col_4 = (Col_4 & 0x3FF) | (((value >> 3) & 0x1F) << 10); } }
		public int Col_1 = 0x7FFF;
		public int Col_2 = 0x5AD6;
		public int Col_3 = 0x2D6B;
		public int Col_4 = 0;
	}

	public abstract class GenericLabel : IComparable<GenericLabel>
	{
		protected int _value;
		public int Value { get { return _value; } }
		protected string _name;
		public string Name { get { return _name; } set { _name = value; } }
		protected string[] _comment;
		public string[] Comment { get { return _comment; } set { _comment = value; } }
		public abstract string ToASMCommentString();
		public abstract string ToSaveFileString();
		public override string ToString()
		{
			return String.Format("{0}({1:X})", _name, _value);
		}

		public override int GetHashCode()
		{
			return _value.GetHashCode();
		}

		public int CompareTo(GenericLabel comp)
		{
			if (comp != null)
			{
				return _name.CompareTo(comp._name);
			}
			else throw new ArgumentException("Object is not initialized.");
		}

	}

	public class FunctionLabel : GenericLabel
	{
		public int Offset { get { return _value; } set { _value = value; } }
		public FunctionLabel(int newOffset, string labelName = "", string[] commentLines = null)
		{
			_value = newOffset;
			_name = labelName.Equals(String.Empty) ? String.Format("F_{0:X6}", newOffset) : labelName;
			if (commentLines != null)
			{
				_comment = new string[commentLines.Length];
				Array.Copy(commentLines, _comment, commentLines.Length);
			}
		}

		public FunctionLabel(FunctionLabel prev) : this(prev._value, prev._name, prev._comment) { }

		public override string ToSaveFileString()
		{
			string returned = ".label";
			returned += Environment.NewLine + "_n:" + _name;
			returned += Environment.NewLine + "_o:" + _value.ToString("X");
			if (_comment != null)
			{
				foreach (string x in _comment)
				{
					returned += Environment.NewLine + "_c:" + x;
				}
			}
			return returned;
		}

		public override string ToASMCommentString()
		{
			string returned = "";
			returned += _name + ":";
			if (_comment != null)
			{
				foreach (string commentLine in _comment)
				{
					if (!commentLine.Equals(String.Empty)) returned += String.Format("{0};{1}", Environment.NewLine, commentLine);
				}
			}
			return returned;
		}

		public override bool Equals(object obj)
		{
			if (obj is FunctionLabel)
			{
				return ((FunctionLabel)obj)._value == _value;
			}
			else return false;
		}

		public override int GetHashCode()
		{
			return _value.GetHashCode();
		}
	}

	public class DataLabel : GenericLabel
	{
		private int _length;
		private int _dataLineLength;
		private DataSectionType dataSectType;
		public int Offset { get { return _value; } set { _value = value; } }
		public int Length { get { return _length; } set { _length = value; } }
		public int DataLineLength { get { return _dataLineLength; } set { _dataLineLength = value; if (_dataLineLength < 0) _dataLineLength = 8; } }
		public DataSectionType DSectionType { get { return dataSectType; } set { dataSectType = value; } }
		public DataLabel(int newOffset, int newLength = 1, string labelName = "", int dataLen = 8, string[] cmt = null, DataSectionType dst = DataSectionType.Data)
		{
			_value = newOffset;
			_length = newLength;
			_name = labelName == String.Empty ? String.Format("DS_{0:X6}", newOffset) : labelName;
			if (cmt != null)
			{
				_comment = new string[cmt.Length];
				Array.Copy(cmt, _comment, cmt.Length);
			}
			if (dataLen <= 0) dataLen = 8;
			_dataLineLength = dataLen;
			dataSectType = dst;
		}

		public DataLabel(DataLabel prev) : this(prev._value, prev._length, prev._name, prev._dataLineLength, prev._comment, prev.dataSectType) { }

		public override string ToASMCommentString()
		{
			string returned = "";
			returned += _name + ":" + Environment.NewLine;
			returned+= String.Format("{0};Size: 0x{1:X} bytes", Environment.NewLine, _length);
			if (_comment != null)
			{
				foreach (string commentLine in _comment)
				{
					if (!commentLine.Equals(String.Empty)) returned += String.Format("{0};{1}", Environment.NewLine, commentLine);
				}
			}
			return returned;
		}

		public override string ToSaveFileString()
		{
			string returned = ".data" + Environment.NewLine;
			returned += "_n:" + _name + Environment.NewLine;
			returned += "_o:" + _value.ToString("X") + Environment.NewLine;
			returned += "_l:" + _length.ToString("X") + Environment.NewLine;
			returned += "_t:" + DSectionType.ToString() + Environment.NewLine;
			returned += "_d:" + _dataLineLength.ToString("X");
			if (_comment != null)
			{
				foreach (string x in _comment)
				{
					returned += Environment.NewLine + "_c:" + x;
				}
			}
			return returned;
		}

		public override bool Equals(object obj)
		{
			if (obj is DataLabel)
			{
				return ((DataLabel)obj)._value == _value;
			}
			else return false;
		}

		public override int GetHashCode()
		{
			return _value.GetHashCode();
		}
	}

	public class VarLabel : GenericLabel
	{
		public int Variable { get { return _value; } set { _value = value & 0xFFFF; } }
		private VariableType varType;
		public VariableType VarType { get { return varType; } set { varType = value; } }
		public VarLabel(int a, string n = "", VariableType vType = VariableType.Byte, string[] cmt = null)
		{
			_name = n.Equals(String.Empty) ? String.Format("V_{0:X4}", a) : n;
			_value = a;
			varType = vType;
			if (cmt != null)
			{
				_comment = new string[cmt.Length];
				Array.Copy(cmt, _comment, cmt.Length);
			}
		}

		public VarLabel(VarLabel prev) : this(prev._value, prev._name, prev.varType, prev._comment) { }

		public override string ToASMCommentString()
		{
			string returned = "";
			if (_comment != null)
			{
				foreach (string commentLine in _comment)
				{
					if (!commentLine.Equals(String.Empty))
					{
						returned += ";" + commentLine + Environment.NewLine;
					}
				}
			}
			returned += _name + " EQU $" + _value.ToString("X");
			return returned;
		}

		public override string ToSaveFileString()
		{
			string returned = ".var" + Environment.NewLine;
			returned += "_n:" + _name + Environment.NewLine;
			returned += "_v:" + _value.ToString("X") + Environment.NewLine;
			returned += "_t:" + varType.ToString();
			if (_comment != null)
			{
				foreach (string x in _comment)
				{
					returned += Environment.NewLine + "_c:" + x;
				}
			}
			return returned;
		}

		public string ToDisplayString()
		{
			string returned = ";Name: " + _name + Environment.NewLine;
			returned += ";Value: " + _value.ToString("X4");
			if (_comment != null)
			{
				foreach (string x in _comment)
				{
					if (x != "")
						returned += Environment.NewLine + ";" + x;
				}
			}
			returned += Environment.NewLine;
			return returned;
		}

		public override bool Equals(object obj)
		{
			if (obj is VarLabel)
			{
				return ((VarLabel)obj)._value == _value;
			}
			else return false;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
