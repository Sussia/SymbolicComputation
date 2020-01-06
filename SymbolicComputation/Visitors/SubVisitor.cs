using System;
using SymbolicComputation.Model;

namespace SymbolicComputation
{
	class SubVisitor : Visitor
	{
		public override Symbol Evaluate(Expression exp)
		{
			var arg1 = exp.Args[0];
			var arg2 = exp.Args[1];

			if (arg1 is Constant constant1 && arg2 is Constant constant2)
			{
				return new Constant(constant1.Value - constant2.Value);
			}

			return exp;
		}
	}
}