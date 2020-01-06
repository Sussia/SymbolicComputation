using System;
using System.Collections.Generic;
using System.Text;

namespace SymbolicComputation.Model
{
	public class Constant : Symbol
	{

		public decimal Value;

		public Constant(decimal value)
		{
			Value = value;
		}
		public override string ToString()
		{
			return Value.ToString();
		}
	}
}
