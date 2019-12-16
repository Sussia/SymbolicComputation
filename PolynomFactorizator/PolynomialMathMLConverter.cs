using System;
using System.Collections.Generic;
using System.Text;

namespace PolynomialFactorizator
{
	public static class PolynomialMathMlConverter
	{
		public static string ToMathMl(Polynomial polynomial)
		{
			var sb = new StringBuilder();
			sb.Append("<math>\r\n<mrow>\r\n");
			foreach (Monomial monomial in polynomial.Terms)
			{
				char sign = !monomial.Sign ? '-' : '+';
				sb.Append($"<mn>{sign} {monomial.Coefficient}</mn>\r\n");
				foreach (Indeterminate indeterminate in monomial.IndeterminatesList)
				{
					sb.Append(
						$"<msup>\r\n<mi>{indeterminate.Symbol}</mi>\r\n<mn>{indeterminate.Power}</mn>\r\n</msup>\r\n");
				}
			}

			sb.Append("</mrow>\r\n</math>");
			return sb.ToString();
		}
	}
}
