using System;
using System.Collections.Generic;
using SymbolicComputation.Model;

namespace SymbolicComputation
{
	class Program
	{
		private static void Main(string[] args)
		{
			Symbol Sum = new StringSymbol("Sum");
			Expression exp1 = Sum[5,3];

			Symbol Mul = new StringSymbol("Mul");

			Expression exp2 = Mul[5, exp1];

			Symbol res = exp2.Evaluate();

			Console.WriteLine(res.ToString());
		}
	}
}
