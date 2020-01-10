using System.Collections.Generic;
using SymbolicComputationModel.Model;

namespace SymbolicComputationModel
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
