using System;
using System.Collections.Generic;
using System.Text;

namespace SymbolicComputation.Model
{
	public class Expression : Symbol
	{
		public readonly Symbol Action;

		public readonly Symbol[] Args;

		//public Symbol Visit(Visitor visitor)
		//{
		//	return visitor.Evaluate(this);
		//}

		public Symbol Evaluate()
		{
			Console.WriteLine($"Evaluating expression: {this}");
			return BuildInFunctions.Evaluate(this);
		}

		public Expression(Symbol action, Symbol[] args)
		{
			Action = action;
			Args = args;
		}
		
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append(Action);
			sb.Append($"[ ");
			foreach (var term in Args)
			{
				sb.Append(term.ToString());
				sb.Append(", ");
			}

			sb.Remove(sb.Length - 2, 2);
			sb.Append(']');
			return sb.ToString();
		}
	}
}
