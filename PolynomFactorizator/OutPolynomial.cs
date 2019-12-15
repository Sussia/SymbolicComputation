using System;
using System.Collections.Generic;
using System.Text;

namespace PolynomialFactorizator
{
    public class OutPolynomial
    {
        public Monomial Prefix;
        public List<Monomial> Terms;

        public OutPolynomial(Monomial prefix, List<Monomial> terms)
        {
            Prefix = prefix;
            // Sign = prefix.Sign;
            // Coefficient = prefix.Coefficient;
            // IndeterminatesList = prefix.IndeterminatesList;
            var _terms = terms;
            foreach (var term in terms)
            {
                if (!prefix.Sign)
                    term.Sign = !term.Sign;
                if (prefix.Coefficient != 1)
                {
                    term.Coefficient /= prefix.Coefficient;
                }

                foreach (var indeterminate in prefix.IndeterminatesList)
                {
                    int indeterminateIndex = term.FindIndeterminateByChar(indeterminate.Symbol);
                    if (term.IndeterminatesList[indeterminateIndex].Power != indeterminate.Power)
                    {
                        Console.WriteLine(term.IndeterminatesList.Count);
                        term.IndeterminatesList[indeterminateIndex].Power -= indeterminate.Power;
                        Console.WriteLine(term.IndeterminatesList.Count);
                    }
                    else
                    {
                        term.IndeterminatesList.RemoveAt(indeterminateIndex);
                    }
                }
            }

            Terms = terms;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            var sign = !Prefix.Sign ? "-" : "+";
            // sb.Append(sign + Prefix.Coefficient + Prefix.IndeterminatesList.ToString() + "(");
            sb.Append(Prefix.ToString() + "(");
            foreach (var term in Terms)
            {
                sb.Append(term.ToString());
                sb.Append(' ');
            }

            sb.Append(")");
            // sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }
    }
}