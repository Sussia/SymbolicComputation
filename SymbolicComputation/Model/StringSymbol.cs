using System;
using System.Collections.Generic;
using System.Text;

namespace SymbolicComputation.Model
{
	public class StringSymbol : Symbol
	{
		public string Name;
		public StringSymbol(string name)
		{
			Name = name;
		}

		public override string ToString()
		{
			return Name;
		}

		public override bool Equals(object? obj)
		{
			if (obj is StringSymbol symbol)
			{
				return symbol.Name.Equals(this.Name);
			}
			return false;
		}
	}
}
