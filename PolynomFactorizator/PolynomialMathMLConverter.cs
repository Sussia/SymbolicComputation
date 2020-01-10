using System;
using System.Collections.Generic;
using System.Text;
using PolynomialFactorizator.SymbolicModels;

namespace PolynomialFactorizator
{
	public static class PolynomialMathMlConverter
	{
		public static string ToMathMl(Polynomial polynomial)
		{
			var sb = new StringBuilder();
			sb.Append("<math>\r\n<mrow>\r\n");
			AppendPolynomialsMathMlString(sb, polynomial);

			sb.Append("</mrow>\r\n</math>");
			return sb.ToString();
		}

		private static void AppendPolynomialsMathMlString(StringBuilder sb, Polynomial polynomial)
		{
			foreach (Monomial monomial in polynomial.Terms)
			{
				char sign = !monomial.Sign ? '-' : '+';
				string coefficient = (monomial.Coefficient == 1 && monomial.IndeterminatesList.Count > 0) ? "" : monomial.Coefficient.ToString();
				sb.Append($"<mn>{sign} {coefficient}</mn>\r\n");
				foreach (Indeterminate indeterminate in monomial.IndeterminatesList)
				{
					string power = indeterminate.Power == 1 ? "" : indeterminate.Power.ToString();
					sb.Append($"<msup>\r\n<mi>{indeterminate.Symbol}</mi>\r\n<mn>{power}</mn>\r\n</msup>\r\n");
				}
			}
		}

		public static string ToMathMl(List<Polynomial> polynomials)
		{
			var sb = new StringBuilder();
			sb.Append("<math>\r\n<mrow>\r\n");
			foreach (Polynomial polynomial in polynomials)
			{
				sb.Append("<mo>(</mo>\r\n");
				AppendPolynomialsMathMlString(sb, polynomial);
				sb.Append("<mo>)</mo>\r\n");
			}
			
			sb.Append("</mrow>\r\n</math>");
			return sb.ToString();
		}
	}
}
