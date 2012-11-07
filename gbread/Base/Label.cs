namespace GBRead.Base
{
    // TODO: Verify that changed comments print correctly.
    using System;

    public enum DataSectionType { Data, Image }

    public enum OffsetFormat { Hex, Decimal, BankOffset }

    public class GBPalette
    {
        public static GBPalette defaultPal = new GBPalette();

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
        protected static int counter = 0;

        protected int _id;

        public int ID { get { return _id; } protected set { _id = value; } }

        protected int _value;

        public int Value { get { return _value; } }

        protected string _name;

        public string Name { get { return _name; } set { _name = value; } }

        protected string _comment;

        public string Comment { get { return _comment; } set { _comment = value; } }

        public abstract string ToASMCommentString();

        public override string ToString()
        {
            return String.Format("{0}({1:X})", _name, _value);
        }

        public override int GetHashCode()
        {
            return _id;
        }

        public int CompareTo(GenericLabel comp)
        {
            if (comp != null)
            {
                return _name.CompareTo(comp._name);
            }
            else
            {
                throw new ArgumentException("Object is not initialized.");
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is GenericLabel)
            {
                return (obj as GenericLabel)._id == _id;
            }
            else
            {
                return false;
            }
        }
    }

    public class FunctionLabel : GenericLabel
    {
        public int Offset { get { return _value; } set { _value = value; } }

        public FunctionLabel(int newOffset, string labelName = "", string nComment = "")
        {
            _id = System.Threading.Interlocked.Increment(ref counter);
            _value = newOffset;
            _name = labelName.Equals(String.Empty) ? String.Format("F_{0:X6}", newOffset) : labelName;
            _comment = "";
            if (!String.IsNullOrEmpty(nComment))
            {
                _comment = nComment;
            }
        }

        public FunctionLabel(FunctionLabel prev) : this(prev._value, prev._name, prev._comment) { }

        public override string ToASMCommentString()
        {
            string returned = "";
            returned += _name + ":";
            if (_comment != "")
            {
                returned += Environment.NewLine + ";" + _comment.Replace("\n", "\n;");
            }
            return returned;
        }
    }

    public class DataLabel : GenericLabel
    {
        private int _length;
        private string _printTemplate;
        private DataSectionType dataSectType;
        private GBPalette palette = new GBPalette();

        public int Offset { get { return _value; } set { _value = value; } }

        public int Length { get { return _length; } set { _length = value; } }

        public DataSectionType DSectionType { get { return dataSectType; } set { dataSectType = value; } }

        public GBPalette Palette { get { return palette; } set { palette = value; } }

        public string PrintTemplate { get { return _printTemplate; } set { _printTemplate = value; } }

        public DataLabel(int newOffset, int newLength = 1, string labelName = "", string printTemplate = "", string cmt = "", DataSectionType dst = DataSectionType.Data, GBPalette pal = null)
        {
            _id = System.Threading.Interlocked.Increment(ref counter);
            _value = newOffset;
            _length = newLength;
            _name = labelName == String.Empty ? String.Format("DS_{0:X6}", newOffset) : labelName;
            _comment = "";
            if (!String.IsNullOrEmpty(cmt))
            {
                _comment = cmt;
            }
            dataSectType = dst;
            if (pal != null)
            {
                palette = pal;
            }
            _printTemplate = printTemplate;
            if (_printTemplate == "")
            {
                _printTemplate = "b";
            }
        }

        public DataLabel(DataLabel prev) : this(prev._value, prev._length, prev._name, prev._comment, prev._printTemplate, prev.dataSectType)
        {
        }

        public override string ToASMCommentString()
        {
            string returned = "";
            returned += _name + ":" + Environment.NewLine;
            returned += String.Format(";Size: 0x{0:X} bytes", _length);
            if (_comment != "")
            {
                returned += Environment.NewLine + ";" + _comment.Replace("\n", "\n;");
            }
            return returned;
        }
    }

    public class VarLabel : GenericLabel
    {
        public int Variable { get { return _value; } set { _value = value & 0xFFFF; } }

        public VarLabel(int a, string n = "", string cmt = "")
        {
            _id = System.Threading.Interlocked.Increment(ref counter);
            _name = n.Equals(String.Empty) ? String.Format("V_{0:X4}", a) : n;
            _value = a;
            _comment = "";
            if (!String.IsNullOrEmpty(cmt))
            {
                _comment = cmt;
            }
        }

        public VarLabel(VarLabel prev) : this(prev._value, prev._name, prev._comment) { }

        public override string ToASMCommentString()
        {
            string returned = "";
            if (_comment != "")
            {
                returned += Environment.NewLine + ";" + _comment.Replace("\n", "\n;");
            }
            returned += _name + " EQU $" + _value.ToString("X");
            return returned;
        }

        public string ToDisplayString()
        {
            string returned = ";Name: " + _name + Environment.NewLine;
            returned += ";Value: " + _value.ToString("X4");
            if (_comment != null)
            {
                returned += Environment.NewLine + ";" + _comment.Replace("\n", "\n;");
            }
            returned += Environment.NewLine;
            return returned;
        }
    }
}