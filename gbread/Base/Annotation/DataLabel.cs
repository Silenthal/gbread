using System;

namespace GBRead.Base.Annotation
{
    public class DataLabel : GenericLabel
    {
        private int _length;
        private string _printTemplate;
        private DataSectionType dataSectType;
        private GBPalette palette = new GBPalette();

        public int Offset { get { return Value; } set { Value = value; } }

        public int Length { get { return _length; } set { _length = value; } }

        public DataSectionType DSectionType { get { return dataSectType; } set { dataSectType = value; } }

        public GBPalette Palette { get { return palette; } set { palette = value; } }

        public string PrintTemplate { get { return _printTemplate; } set { _printTemplate = value; } }

        public DataLabel(int newOffset, int newLength = 1, string labelName = "", string printTemplate = "", string cmt = "", DataSectionType dst = DataSectionType.Data, GBPalette pal = null)
        {
            m_id = System.Threading.Interlocked.Increment(ref counter);
            Value = newOffset;
            _length = newLength;
            Name = labelName == string.Empty ? string.Format("DS_{0:X6}", newOffset) : labelName;
            Comment = "";
            if (!string.IsNullOrEmpty(cmt))
            {
                Comment = cmt;
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

        public DataLabel(DataLabel prev) : this(prev.Value, prev._length, prev.Name, prev.Comment, prev._printTemplate, prev.dataSectType)
        {
        }

        public override string ToASMString()
        {
            string returned = "";
            returned += Name + ":" + Environment.NewLine;
            returned += string.Format(";Size: 0x{0:X} bytes", _length);
            if (Comment != "")
            {
                returned += Environment.NewLine + ";" + Comment.Replace("\n", "\n;");
            }
            return returned;
        }
    }
}