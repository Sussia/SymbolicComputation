using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using SymbolicComputation.Model;
using Expression = SymbolicComputation.Model.Expression;

namespace SymbolicComputation
{
	public static class BuildInFunctions
	{

		private static Dictionary<string, Func<Expression, Symbol>> functionsDictionary =
			new Dictionary<string, Func<Expression, Symbol>>
			{
				{"Sum", Sum},
				{"Sub", Sub},
				{"Mul", Mul},
				{"Div", Div},
				{"Rem", Rem},
				{"Divide", Divide}
			};

		private static Symbol Eval(Expression exp, Func<decimal, decimal, decimal> func)
		{
			var arg1 = exp.Args[0];
			var arg2 = exp.Args[1];

			if (arg1 is Constant constant1 && arg2 is Constant constant2)
			{
				return new Constant(func(constant1.Value, constant2.Value));
			}

			return exp;
		}

		public static Symbol Evaluate(Expression exp)
		{
			List<Symbol> newArgs = new List<Symbol>();
			foreach (var arg in exp.Args)
			{
				if (arg is Expression)
				{
					newArgs.Add(Evaluate((Expression)arg));
				}
				else
				{
					newArgs.Add(arg);
				}
			}
			Expression newExp = new Expression(exp.Action, newArgs.ToArray());
			Console.WriteLine($"Evaluating expression: {newExp}");
			return functionsDictionary[exp.Action.ToString()](newExp);
		}

		private static Symbol Sum(Expression exp)
		{
			return Eval(exp, (a, b) => a + b);
		}

		private static Symbol Sub(Expression exp)
		{
			return Eval(exp, (a, b) => a - b);
		}

		private static Symbol Mul(Expression exp)
		{
			return Eval(exp, (a, b) => a * b);
		}

		private static Symbol Divide(Expression exp)
		{
			return Eval(exp, (a, b) => a / b);
		}

		private static Symbol Div(Expression exp)
		{
			return Eval(exp, (a, b) => (int)(a / b));
		}

		private static Symbol Rem(Expression exp)
		{
			return Eval(exp, (a, b) => a % b);
		}
	}
}
