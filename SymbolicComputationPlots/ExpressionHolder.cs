using SymbolicComputationModel;

namespace SymbolicComputationPlots
{
    public static class ExpressionHolder
    {
        static Symbol L = new StringSymbol("L");

        public static Expression expression = L[L[L[1, 2], L[3, 4], L[4, -1]], L[L[4, 1], L[6, 3]]];
    }
}
