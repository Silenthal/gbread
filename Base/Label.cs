using System;
using System.Text;
using System.Collections.Generic;

namespace GBRead.Base
{
	public enum DataSectionType { Data, Img, Text }

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

	public abstract class GenericLabel
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
	}

	public class FunctionLabel : GenericLabel, IComparable<FunctionLabel>
	{
		private int _length;
		public int Offset { get { return _value; } set { _value = value; } }
		public int Length { get { return _length; } set { _length = value; } }
		public int Bank { get { return _value >> 14; } }
		public FunctionLabel(int newOffset, string labelName = "", int labelLength = 0, string[] commentLines = null)
		{
			_value = newOffset;
			_name = labelName.Equals(String.Empty) ? String.Format("F_{0:X6}", newOffset) : labelName;
			_length = labelLength;
			if (commentLines != null)
			{
				_comment = new string[commentLines.Length];
				Array.Copy(commentLines, _comment, commentLines.Length);
			}
		}

		public FunctionLabel(FunctionLabel prev) : this(prev._value, prev._name, prev._length, prev._comment) { }

		public override string ToSaveFileString()
		{
			StringBuilder returned = new StringBuilder(".label");
			returned.Append(Environment.NewLine + "_n:" + _name);
			returned.Append(Environment.NewLine + "_o:" + _value.ToString("X"));
			if (_length != 0) returned.Append(Environment.NewLine + "_l:" + _length.ToString("X"));
			if (_comment != null)
			{
				foreach (string x in _comment)
				{
					returned.Append(Environment.NewLine + "_c:" + x);
				}
			}
			return returned.ToString();
		}

		public override string ToASMCommentString()
		{
			StringBuilder returned = new StringBuilder();
			returned.Append(_name + ":");
			if (_length != 0)
			{
				returned.AppendFormat("{0};Size: 0x{1:X} bytes", Environment.NewLine, _length);
			}
			if (_comment != null)
			{
				foreach (string commentLine in _comment)
				{
					if (!commentLine.Equals(String.Empty)) returned.AppendFormat("{0};{1}", Environment.NewLine, commentLine);
				}
			}
			return returned.ToString();
		}

		public int CompareTo(FunctionLabel comp)
		{
			if (comp != null)
			{
				return _value.CompareTo(comp._value);
			}
			else throw new ArgumentException("Object is not a proper Code Label.");
		}

		public override bool Equals(object obj)
		{
			if (obj is FunctionLabel)
			{
				return ((FunctionLabel)obj)._value.Equals(_value);
			}
			else return false;
		}

		public override int GetHashCode()
		{
			return _value.GetHashCode();
		}
	}

	public class DataLabel : GenericLabel, IComparable<DataLabel>
	{
		private int _length;
		private int _dataLineLength;
		private DataSectionType dataSectType;
		public GBPalette DataPalette;
		public int Offset { get { return _value; } set { _value = value; } }
		public int Length { get { return _length; } set { _length = value; } }
		public int DataLineLength { get { return _dataLineLength; } set { _dataLineLength = value; if (_dataLineLength < 0) _dataLineLength = 8; } }
		public DataSectionType DSectionType { get { return dataSectType; } set { dataSectType = value; } }
		public DataLabel(int newOffset, int newLength = 1, string labelName = "", int dataLen = 8, string[] cmt = null, DataSectionType dst = DataSectionType.Data, GBPalette pal = null)
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
			DataPalette = pal == null ? new GBPalette() : pal;
		}

		public DataLabel(DataLabel prev) : this(prev._value, prev._length, prev._name, prev._dataLineLength, prev._comment, prev.dataSectType, prev.DataPalette) { }

		public override string ToASMCommentString()
		{
			StringBuilder returned = new StringBuilder();
			returned.AppendLine(_name + ":");
			returned.AppendFormat(";Size: 0x{0:X} bytes", _length);
			if (_comment != null)
			{
				foreach (string commentLine in _comment)
				{
					if (!commentLine.Equals(String.Empty)) returned.AppendFormat("{0};{1}", Environment.NewLine, commentLine);
				}
			}
			return returned.ToString();
		}

		public override string ToSaveFileString()
		{
			StringBuilder returned = new StringBuilder(".data");
			returned.AppendLine(Environment.NewLine + "_n:" + _name);
			returned.AppendLine("_o:" + _value.ToString("X"));
			returned.AppendLine("_l:" + _length.ToString("X"));
			returned.Append("_d:" + _dataLineLength.ToString("X"));
			if (_comment != null)
			{
				foreach (string x in _comment)
				{
					returned.Append(Environment.NewLine + "_c:" + x);
				}
			}
			if (dataSectType == DataSectionType.Img)
			{
				returned.Append(Environment.NewLine + "_p1:" + DataPalette.Col_1.ToString("X"));
				returned.Append(Environment.NewLine + "_p2:" + DataPalette.Col_2.ToString("X"));
				returned.Append(Environment.NewLine + "_p3:" + DataPalette.Col_3.ToString("X"));
				returned.Append(Environment.NewLine + "_p4:" + DataPalette.Col_4.ToString("X"));
			}
			return returned.ToString();
		}

		public int CompareTo(DataLabel comp)
		{
			if (comp != null)
			{
				return _value.CompareTo(comp._value);
			}
			else throw new ArgumentException("Object is not a Data Label.");
		}

		public override bool Equals(object obj)
		{
			if (obj is DataLabel)
			{
				return ((DataLabel)obj)._value.Equals(_value);
			}
			else return false;
		}

		public override int GetHashCode()
		{
			return _value.GetHashCode();
		}
	}

	public class VarLabel : GenericLabel, IComparable<VarLabel>
	{
		public int Variable { get { return _value; } set { _value = value & 0xFFFF; } }
		public VarLabel(int a, string n = "", string[] cmt = null)
		{
			_name = n.Equals(String.Empty) ? String.Format("V_{0:X4}", a) : n;
			_value = a;
			if (cmt != null)
			{
				_comment = new string[cmt.Length];
				Array.Copy(cmt, _comment, cmt.Length);
			}
		}

		public VarLabel(VarLabel prev) : this(prev._value, prev._name, prev._comment) { }

		public override string ToASMCommentString()
		{
			StringBuilder returned = new StringBuilder(String.Empty);
			if (_comment != null)
			{
				foreach (string commentLine in _comment)
				{
					if (!commentLine.Equals(String.Empty))
					{
						returned.AppendLine(";" + commentLine);
					}
				}
			}
			returned.Append(_name + " EQU $" + _value.ToString("X"));
			return returned.ToString();
		}

		public override string ToSaveFileString()
		{
			StringBuilder returned = new StringBuilder(".var");
			returned.Append(Environment.NewLine + "_n:" + _name);
			returned.Append(Environment.NewLine + "_v:" + _value.ToString("X"));
			if (_comment != null)
			{
				foreach (string x in _comment)
				{
					returned.Append(Environment.NewLine + "_c:" + x);
				}
			}
			return returned.ToString();
		}

		public string ToDisplayString()
		{
			StringBuilder returned = new StringBuilder(";Name: " + _name + Environment.NewLine);
			returned.Append(";Value: " + _value.ToString("X4"));
			if (_comment != null)
			{
				foreach (string x in _comment)
				{
					if (x != "")
						returned.Append(Environment.NewLine + ";" + x);
				}
			}
			returned.AppendLine();
			return returned.ToString();
		}

		public int CompareTo(VarLabel comp)
		{
			if (comp != null)
			{
				return _name.CompareTo(comp._name);
			}
			else throw new ArgumentException("Object is not a proper Variable Label.");
		}

		public override bool Equals(object obj)
		{
			if (obj is VarLabel)
			{
				return ((VarLabel)obj)._value.Equals(_value);
			}
			else return false;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
