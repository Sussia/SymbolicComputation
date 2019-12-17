using System;
using System.Collections.Generic;
using System.Text;

namespace PolynomialFactorizator
{
    public class Indeterminate
    {
        public readonly char Symbol;

        public readonly int Power;

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
            if (Power != 1)
            {
                return $"{Symbol}^{Power}";
            }
            else
            {
                return $"{Symbol}";
            }
        }
    }
}