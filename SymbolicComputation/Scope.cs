using System;
using System.Collections.Generic;
using System.Text;
using SymbolicComputation.Model;

namespace SymbolicComputation
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
