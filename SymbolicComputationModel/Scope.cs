using System.Collections.Generic;

namespace SymbolicComputationModel
{
    public class Scope
    {
        public List<Symbol> IndeterminateList = new List<Symbol>();
        public Dictionary<string, Symbol> SymbolRules;

        public Scope()
        {
            SymbolRules = new Dictionary<string, Symbol>();
        }
    }
}