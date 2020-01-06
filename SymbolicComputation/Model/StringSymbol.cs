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
	}
}
