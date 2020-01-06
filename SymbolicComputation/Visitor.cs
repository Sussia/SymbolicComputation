using System.Collections.Generic;
using System.Text;
using SymbolicComputation.Model;

namespace SymbolicComputation
{
	public abstract class Visitor
	{
		public abstract Symbol Evaluate(Expression exp);
	}
}
