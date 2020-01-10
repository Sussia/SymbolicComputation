namespace SymbolicComputationModel
{
    public class Constant : Symbol
    {
        public decimal Value;

        public Constant(decimal value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public override bool Equals(object? obj)
        {
            if (obj is Constant constant) return constant.Value == Value;

            return false;
        }
    }
}