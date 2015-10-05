using System;
using static System.String;
using static System.Threading.Interlocked;
using static System.Environment;

namespace GBRead.Base.Annotation
{
    public class FunctionLabel : GenericLabel
    {
        public int Offset { get { return Value; } set { Value = value; } }

        public FunctionLabel(int offset, string name = "", string comment = "")
        {
            m_id = Increment(ref counter);
            Name = IsNullOrWhiteSpace(name) ? $"F_{offset:X6}" : name;
            Value = offset;
            
            if (!IsNullOrEmpty(comment))
            {
                Comment = comment;
            }
        }

        public FunctionLabel(FunctionLabel prev) : this(prev.Value, prev.Name, prev.Comment)
        {
        }

        public override string ToASMString()
        {
            string returned = Name + ":";
            if (Comment != "")
            {
                returned += NewLine + ";" + Comment.Replace("\n", "\n;");
            }
            return returned;
        }
    }
}