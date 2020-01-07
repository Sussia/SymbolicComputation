using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using SymbolicComputation.Model;
using Expression = SymbolicComputation.Model.Expression;

namespace SymbolicComputation
{
	public static class BuildInFunctions
	{
		private static Scope context = new Scope();

		private static Dictionary<string, Func<Expression, Symbol>> functionsDictionary =
			new Dictionary<string, Func<Expression, Symbol>>
			{
				{"Sum", Sum},
				{"Sub", Sub},
				{"Mul", Mul},
				{"Div", Div},
				{"Rem", Rem},
				{"Divide", Divide},
				{"List", List}
			};

		private static Symbol MathEval(Expression exp, Func<decimal, decimal, decimal> func)
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
			if (exp.Action.ToString() == "Set")
			{
				return Set(exp);
			}
			List<Symbol> newArgs = new List<Symbol>();
			foreach (var arg in exp.Args)
			{
				if (arg is Expression)
				{
					newArgs.Add(Evaluate((Expression)arg));
				}
				else if (arg is StringSymbol symbol)
				{
					newArgs.Add(Substitute(symbol));
				} else
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
			return MathEval(exp, (a, b) => a + b);
		}

		private static Symbol Sub(Expression exp)
		{
			return MathEval(exp, (a, b) => a - b);
		}

		private static Symbol Mul(Expression exp)
		{
			return MathEval(exp, (a, b) => a * b);
		}

		private static Symbol Divide(Expression exp)
		{
			return MathEval(exp, (a, b) => a / b);
		}

		private static Symbol Div(Expression exp)
		{
			return MathEval(exp, (a, b) => (int)(a / b));
		}

		private static Symbol Rem(Expression exp)
		{
			return MathEval(exp, (a, b) => a % b);
		}

		private static Symbol List(Expression exp)
		{
			return exp.Args.Last();
		}

		private static Symbol Set(Expression exp)
		{
			var arg1 = exp.Args[0];
			var arg2 = exp.Args[1];
			if (arg1 is StringSymbol symbol)
			{
				Symbol newArg;
				if (arg2 is Expression exp2)
				{
					newArg = Evaluate(exp2);
				} else if (arg2 is StringSymbol stringSymbol)
				{
					newArg = Substitute(stringSymbol);
				}
				else
				{
					newArg = arg2;
				}
				context.SymbolRules.Add(symbol.Name, newArg);
				return arg1;
			}
			throw new Exception("First parameter of Set isn't string symbol");
		}

		private static Symbol Substitute(StringSymbol symbol)
		{
			return context.SymbolRules[symbol.Name]; //, out var resultSymbol) ? resultSymbol : symbol;
		}
	}
}
