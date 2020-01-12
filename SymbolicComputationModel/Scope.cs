using System.Collections.Generic;
using SymbolicComputationLib.Model;
using static SymbolicComputationLib.PredefinedSymbols;

namespace SymbolicComputationLib
{
	public class Scope
	{
		public Dictionary<string, Symbol> SymbolRules;
		public Dictionary<string, Symbol> AttributeDictionary;

		public Scope()
		{
			SymbolRules = new Dictionary<string, Symbol>();
			AttributeDictionary = new Dictionary<string, Symbol>()
			{
				{Set.ToString(), HoldFirst},
				{Prepend.ToString(), HoldAll},
				{SetDelayed.ToString(), HoldAll},
				{SetAttribute.ToString(), HoldAll},
				{Delayed.ToString(), HoldAll},
				//{If.ToString(), HoldRest}
			};
		}
	}
}
