using System.Collections.Generic;
using SymbolicComputationLib.Model;

namespace SymbolicComputationLib
{
	public class Scope
	{
		public List<Symbol> IndeterminateList;
		public Dictionary<string, Symbol> SymbolRules;

		public Scope()
		{
			SymbolRules = new Dictionary<string, Symbol>();
			IndeterminateList = new List<Symbol>();
		}
	}
}
