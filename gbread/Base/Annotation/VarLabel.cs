using static System.Environment;
using static System.Threading.Interlocked;

namespace GBRead.Base.Annotation
{
    public class VarLabel : GenericLabel
    {
        public int Variable { get { return Value; } set { Value = value & 0xFFFF; } }

        public VarLabel(int value, string name = "", string comment = "")
        {
            Name = name == "" ? $"V_{value:X4}" : name;
            Value = value;

            m_id = Increment(ref counter);

            if (!string.IsNullOrEmpty(comment))
            {
                Comment = comment;
            }
        }

        public VarLabel(VarLabel prev) : this(prev.Value, prev.Name, prev.Comment)
        {
        }

        public override string ToASMString()
        {
            string returned = "";
            if (Comment != "")
            {
                returned += NewLine + ";" + Comment.Replace("\n", "\n;");
            }
            returned += Name + " EQU $" + Value.ToString("X");
            return returned;
        }

        public string ToDisplayString()
        {
            string returned = ";Name: " + Name + NewLine;
            returned += ";Value: " + Value.ToString("X4");
            if (Comment != null)
            {
                returned += NewLine + ";" + Comment.Replace("\n", "\n;");
            }
            returned += NewLine;
            return returned;
        }
    }
}