using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GBRead.Base
{
    public enum ListSortOrder { BY_NAME, BY_VALUE }

    public class LabelComparer : Comparer<GenericLabel>
    {
        public ListSortOrder listOrder = ListSortOrder.BY_NAME;

        public LabelComparer(ListSortOrder cLL)
        {
            listOrder = cLL;
        }

        public override int Compare(GenericLabel x, GenericLabel y)
        {
            switch (listOrder)
            {
                case ListSortOrder.BY_NAME:
                    return x.Name.CompareTo(y.Name);
                case ListSortOrder.BY_VALUE:
                    return x.Value.CompareTo(y.Value);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
