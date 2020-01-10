using System.Text;

namespace SymbolicComputationLib.Model
{
	public class Expression : Symbol
	{
		public readonly Symbol Action;

		public readonly Symbol[] Args;

		//public Symbol Visit(Visitor visitor)
		//{
		//	return visitor.Evaluate(this);
		//}

		public Symbol Evaluate(Scope context)
		{
			return BuildInFunctions.Evaluate(this, context);
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
			sb.Append($"[");
			foreach (var term in Args)
			{
				sb.Append(term);
				sb.Append(", ");
			}

			sb.Remove(sb.Length - 2, 2);
			sb.Append(']');
			return sb.ToString();
		}

		public override bool Equals(object? obj)
		{
			if (obj is Expression exp)
			{
				if (exp.Action.Equals(Action) && exp.Args.Length == Args.Length)
				{
					for (int i = 0; i < Args.Length; i++)
					{
						if (!Args[i].Equals(exp.Args[i]))
						{
							return false;
						}
					}
					return true;
				}
				return false;
			}
			return false;
		}
	}
}
