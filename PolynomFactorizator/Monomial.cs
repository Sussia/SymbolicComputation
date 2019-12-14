using System;
using System.Collections.Generic;
using System.Text;

namespace PolynomialFactorizator
{
	public class Monomial
	{
		public bool Sign;

		public int Coefficient;

		public List<Indeterminate> IndeterminatesList;

		public Monomial()
		{
			IndeterminatesList = new List<Indeterminate>();
		}

		public Monomial(bool sign, int coefficient, List<Indeterminate> indeterminates)
		{
			this.Sign = sign;
			this.Coefficient = coefficient;
			this.IndeterminatesList = indeterminates;
		}

		public override string ToString()
		{
			string indeterminateString = GetIndeterminatesString();
            string sign = !Sign ? "-" : "";
			return $"{sign} {Coefficient}*{indeterminateString}";
		}

		private string GetIndeterminatesString()
		{
			StringBuilder sb = new StringBuilder();
			foreach (Indeterminate indeterminate in IndeterminatesList) {
				sb.Append(indeterminate.ToString());
				sb.Append('*');
			}
			sb.Remove(sb.Length - 1, 1);
			return sb.ToString();
		}
	}
}
