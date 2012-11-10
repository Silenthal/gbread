

namespace GBRead.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    class SymbolTable
    {
        private List<Symbol> symbolTable = new List<Symbol>();
        private object symLock = new object();

        public void ClearTable()
        {
            lock (symLock)
            {
                symbolTable.Clear();
            }
        }

        public bool AddSymbol(Symbol symbol)
        {
            lock(symLock)
            {
                if (symbolTable.Contains(symbol))
                {
                    return false;
                }
                symbolTable.Add(symbol);
                return true;
            }
        }

        public void RemoveSymbol(Symbol symName)
        {
            lock (symLock)
            {
                if (symbolTable.Contains(symName))
                {
                    symbolTable.Remove(symName);
                }
            }
        }

        public bool ContainsSymbol(Symbol search)
        {
            lock (symLock)
            {
                return symbolTable.Contains(search);
            }
        }

        public List<Symbol> GetSymbolsByValue(long value, bool isConstant)
        {
            lock (symLock)
            {
                var s = from items in symbolTable
                        where items.Value == value && items.IsConstant == isConstant
                        select items;
                var ret = new List<Symbol>(s);
                return ret;
            }
        }
    }
}
