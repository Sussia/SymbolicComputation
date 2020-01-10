using System.Collections.Generic;
using System.Text;

namespace PolynomialFactorizator.SymbolicModels
{
    public class Polynomial
    {
        public readonly List<Monomial> Terms;

        public Polynomial()
        {
            Terms = new List<Monomial>();
        }

        public Polynomial(List<Monomial> terms)
        {
            this.Terms = terms;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var term in Terms)
            {
                sb.Append(term.ToString());
                sb.Append(' ');
            }

            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }
    }
}