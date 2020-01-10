using SymbolicComputationLib.Model;

namespace SymbolicComputationLib
{
	public static class PredefinedSymbols
	{
		public static readonly Symbol Sum = new StringSymbol("Sum");
		public static readonly Symbol Sub = new StringSymbol("Sub");
		public static readonly Symbol Mul = new StringSymbol("Mul");
		public static readonly Symbol Pow = new StringSymbol("Pow");
		public static readonly Symbol Rem = new StringSymbol("Rem");
		public static readonly Symbol Divide = new StringSymbol("Divide");
		public static readonly Symbol List = new StringSymbol("List");
		public static readonly Symbol Set = new StringSymbol("Set");
		public static readonly Symbol Delayed = new StringSymbol("Delayed");
		public static readonly Symbol Equal = new StringSymbol("Equal");
		public static readonly Symbol Or = new StringSymbol("Or");
		public static readonly Symbol And = new StringSymbol("And");
		public static readonly Symbol Xor = new StringSymbol("Xor");
		public static readonly Symbol Not = new StringSymbol("Not");
		public static readonly Symbol Greater = new StringSymbol("Greater");
		public static readonly Symbol GreaterOrEqual = new StringSymbol("GreaterOrEqual");
		public static readonly Symbol Less = new StringSymbol("Less");
		public static readonly Symbol LessOrEqual = new StringSymbol("LessOrEqual");
		public static readonly Symbol If = new StringSymbol("If");
		public static readonly Symbol While = new StringSymbol("While");
		public static readonly Symbol L = new StringSymbol("L");
		public static readonly Symbol First = new StringSymbol("First");
		public static readonly Symbol Rest = new StringSymbol("Rest");
		public static readonly Symbol GetPolynomialCoefficients = new StringSymbol("GetPolynomialCoefficients");
		public static readonly Symbol GetIndeterminateList = new StringSymbol("GetIndeterminateList");
		public static readonly Symbol True = new StringSymbol("True");
		public static readonly Symbol False = new StringSymbol("False");
		public static readonly Symbol Ok = new StringSymbol("Ok");
		public static readonly Symbol Null = new StringSymbol("null");
	}
}
