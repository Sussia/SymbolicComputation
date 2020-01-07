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
			Symbol List = new StringSymbol("List");
			Symbol Set = new StringSymbol("Set");
			Symbol Mul = new StringSymbol("Mul");
			Symbol Delayed = new StringSymbol("Delayed");

			Expression p1Func = Sum["t", 1];

			Symbol P1 = new StringSymbol("P1");

			Expression delExp = Delayed[P1, "t", p1Func];
			Expression testDelExp = List[delExp, P1[2]];

			Expression exp1 = Sum[5, 3];
			Expression exp2 = Mul[5, exp1];

			Expression exp3 = List[Set["y", 10], Mul["x", Sum["y", 1]]];



			Expression setExp = Set["x", 2];
			Expression setExp2 = Set["y", Mul["x", 4]];

			//Expression repExp = BuildInFunctions.ReplaceVariable(exp3, new StringSymbol( "x"), new Constant(2));


			Console.WriteLine(testDelExp.Evaluate().ToString());

		}
	}
}
