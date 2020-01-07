using System;
using System.Collections.Generic;
using System.Text;
using SymbolicComputation.Model;

namespace SymbolicComputation
{
	public class Scope
	{
		public Dictionary<string, Symbol> SymbolRules;

		public Scope()
		{
			SymbolRules = new Dictionary<string, Symbol>();
		}
	}
}
