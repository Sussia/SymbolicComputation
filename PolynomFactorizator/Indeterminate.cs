using System;
using System.Collections.Generic;
using System.Text;

namespace PolynomialFactorizator
{
	public class Indeterminate :  ICloneable
	{
		public char Symbol;

		public int Power;

		public Indeterminate()
		{
		}

		public Indeterminate(char symbol, int power)
		{
			this.Symbol = symbol;
			this.Power = power;
		}

		public override string ToString()
		{
			if (Power != 1) {
				return $"{Symbol}^{Power}";
			} else {
				return $"{Symbol}";
			}

		}

        public object Clone()
        {
            return new Indeterminate(this.Symbol, this.Power);
        }
    }
}