using System;
using System.Collections.Generic;
using System.Text;

namespace PolynomialFactorizator
{
	public class Indeterminate
	{
		public char symbol;

		public int power;

		public Indeterminate()
		{
		}

		public Indeterminate(char symbol, int power)
		{
			this.symbol = symbol;
			this.power = power;
		}

		public override string ToString()
		{
			if (power != 1) {
				return $"{symbol}^{power}";
			} else {
				return $"{symbol}";
			}

		}
	}
}