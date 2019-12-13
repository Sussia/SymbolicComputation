using System;
using System.Collections.Generic;
using System.Text;

namespace PolynomialFactorizator
{
	public class Polynomial
	{
		public List<Monomial> terms;

		public Polynomial()
		{
			terms = new List<Monomial>();
		}

		public Polynomial(List<Monomial> terms)
		{
			this.terms = terms;
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			foreach (Monomial term in terms) {
				sb.Append(term.ToString());
				sb.Append(' ');
			}
			sb.Remove(sb.Length - 1, 1);
			return sb.ToString();
		}
	}
}
