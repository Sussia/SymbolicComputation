using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using SymbolicComputationLib.Model;
using static SymbolicComputationLib.PredefinedSymbols;

namespace SymbolicComputationLib
{
	public static class Parser
	{
		public static object ParseInput(string jsonInput, Scope context)
		{
			dynamic a = JsonConvert.DeserializeObject<dynamic>(jsonInput);
			// Check if it is an Expression
			try
			{
				string temp = a.Action.ToString();
			}
			catch (RuntimeBinderException e)
			{
				try
				{
					return new Constant(Convert.ToDecimal(a.Value.ToString()));
				}
				catch (RuntimeBinderException ex)
				{
					context.IndeterminateList.Add(new StringSymbol(a.Name.ToString()));
					return new StringSymbol(a.Name.ToString());
				}
			}

			//Console.WriteLine(a.Action.Name.ToString());
			Symbol action = new StringSymbol(a.Action.Name.ToString());
			List<Symbol> argumentList = new List<Symbol>();
			for (int i = 0; i < Int32.MaxValue; i++)
			{
				try
				{
					argumentList.Add(ParseInput(a.Args[i].ToString(), context));
				}
				catch (ArgumentOutOfRangeException e)
				{
					break;
				}
			}


			return new Expression(action, argumentList.ToArray());
		}

		public static string ToMathML(Expression expression)
		{
			var sb = new StringBuilder();
			sb.Append("<math>\r\n<mrow>\r\n");
			ToMathML(expression, sb);
			sb.Append("</mrow>\r\n</math>");
			return sb.ToString();
		}

		private static void ToMathML(Symbol symbol, StringBuilder sb)
		{
			switch (symbol)
			{
				case Constant constant:
					sb.Append($"<mn>{constant.Value}</mn>\r\n");
					break;
				case StringSymbol stringSymbol:
					sb.Append($"<mi>{stringSymbol}</mi>\r\n");
					break;
				case Expression expression:
					{
						if (expression.Action.Equals(Mul))
						{
							foreach (var expressionArg in expression.Args)
							{
								ToMathML(expressionArg, sb);
							}
						}

						if (expression.Action.Equals(Pow))
						{
							sb.Append($"<msup>");
							ToMathML(expression.Args[0], sb);
							ToMathML(expression.Args[1], sb);
							sb.Append($"</msup>\r\n");
						}

						if (expression.Action.Equals(Sum))
						{
							sb.Append("<mo>(</mo>\r\n");
							int lastOperatorLength = "<mo>+</mo>\r\n".Length;
							foreach (var expressionArg in expression.Args)
							{
								ToMathML(expressionArg, sb);
								sb.Append("<mo>+</mo>\r\n");
							}
							sb.Remove(sb.Length - lastOperatorLength - 1, lastOperatorLength);
							sb.Append("<mo>)</mo>\r\n");
						}

						if (expression.Action.Equals(Equal))
						{

							ToMathML(expression.Args[0], sb);
							sb.Append("<mo>=</mo>\r\n");
							ToMathML(expression.Args[1], sb);
						}
						break;
					}
			}
		}
	}
}