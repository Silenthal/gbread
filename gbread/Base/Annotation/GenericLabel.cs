using System;

namespace GBRead.Base.Annotation
{
    public abstract class GenericLabel : IComparable<GenericLabel>
    {
        public string Name = "";
        public int Value { get; protected set; } = 0;
        public string Comment = "";

        protected static int counter = 0;
        protected int m_id;

        public abstract string ToASMString();

        public override string ToString() => $"{Name}({Value:X})";

        public override int GetHashCode() => m_id;

        public int CompareTo(GenericLabel comp)
        {
            if (comp != null)
            {
                return Name.CompareTo(comp.Name);
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
                return (obj as GenericLabel).m_id == m_id;
            }
            else
            {
                return false;
            }
        }
    }
}