namespace GBRead.Base
{
    public class Symbol
    {
        public bool IsConstant = false;
        public string ExportScopeName = "";
        public string GlobalScopeName = "";
        public string LocalScopeName = "";
        public string Name = "";
        public long Value = 0;

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Symbol)
            {
                var other = (Symbol)obj;
                return this.ExportScopeName.Equals(other.ExportScopeName)
                    && this.GlobalScopeName.Equals(other.GlobalScopeName)
                    && this.LocalScopeName.Equals(other.LocalScopeName)
                    && this.Name.Equals(other.Name);
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            return ExportScopeName
                + "!" + GlobalScopeName
                + "!" + LocalScopeName
                + "!" + Name;
        }
    }
}