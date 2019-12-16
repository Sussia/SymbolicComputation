using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace PolynomialFactorizator
{
    public class Polynomial
    {
        public List<Monomial> Terms;

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