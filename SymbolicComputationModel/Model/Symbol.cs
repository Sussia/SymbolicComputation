namespace SymbolicComputationModel.Model
{
	public abstract class Symbol
	{
		public Expression this[params Symbol[] arguments] =>
			new Expression(this,  arguments);

		public static implicit operator Symbol(decimal value) =>
			new Constant(value);

		public static implicit operator Symbol(string name) =>
			new StringSymbol(name);
	}
}
