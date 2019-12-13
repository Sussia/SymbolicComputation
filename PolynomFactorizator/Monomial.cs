using System;
using System.Collections.Generic;
using System.Text;

namespace PolynomialFactorizator
{
	public class Monomial
	{
		public char sign;

		public int coefficient;

		public List<Indeterminate> indeterminates;

		public Monomial()
		{
			indeterminates = new List<Indeterminate>();
		}

		public Monomial(char sign, int coefficient, List<Indeterminate> indeterminates)
		{
			this.sign = sign;
			this.coefficient = coefficient;
			this.indeterminates = indeterminates;
		}

		public override string ToString()
		{
			string indeterminatesString = GetIndeterminatesString();
			return $"{sign} {coefficient}*{indeterminatesString}";
		}

		private string GetIndeterminatesString()
		{
			StringBuilder sb = new StringBuilder();
			foreach (Indeterminate indeterminate in indeterminates) {
				sb.Append(indeterminate.ToString());
				sb.Append('*');
			}
			sb.Remove(sb.Length - 1, 1);
			return sb.ToString();
		}
	}
}
