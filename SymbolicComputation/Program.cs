using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using SymbolicComputation.Model;

namespace SymbolicComputation
{
	class Program
	{
		private static void Main(string[] args)
		{
			string filepath = "../../../input.json";

			using (StreamReader sr = new StreamReader(filepath))
			{
				Expression asd = (Expression)(Parser.ParseInput(sr.ReadToEnd()));
				Console.WriteLine($"Got expression: {asd}");
			}

			Symbol Sum = new StringSymbol("Sum");
			Symbol Mul = new StringSymbol("Mul");
			Symbol List = new StringSymbol("List");
			Symbol Set = new StringSymbol("Set");
			Symbol Pow = new StringSymbol("Pow");
			Symbol Delayed = new StringSymbol("Delayed");

			Expression p1Func = Sum["t", 1];

			Symbol P1 = new StringSymbol("P1");

			Expression delExp = Delayed["P1", "t", p1Func];
			Expression testDelExp = List[delExp, P1[2]];

			Expression exp1 = Pow[5, 3];
			Expression exp2 = Mul[5, exp1];

			Expression exp3 = List[Set["y", 10], Mul["x", Sum["y", 1]]];

			Expression setExp = Set["x", 2];
			Expression setExp2 = Set["y", Mul["x", 4]];

			Console.WriteLine(exp1.Evaluate().ToString());

		}
	}
}