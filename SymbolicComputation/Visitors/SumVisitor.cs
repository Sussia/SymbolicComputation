using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SymbolicComputation.Model;

namespace SymbolicComputation
{
	public class SumVisitor : Visitor
	{
		public override Symbol Evaluate(Expression exp)
		{
			var arg1 = exp.Args[0];
			var arg2 = exp.Args[1];

			if (arg1 is Constant constant1 && arg2 is Constant constant2)
			{
				return new Constant(constant1.Value + constant2.Value);
			}

			return exp;
		}
	}
}
