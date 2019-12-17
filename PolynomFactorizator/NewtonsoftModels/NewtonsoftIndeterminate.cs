using System;
using System.Collections.Generic;
using System.Text;

namespace PolynomialFactorizator.NewtonsoftModels
{
    class NewtonsoftIndeterminate
    {
        public char Symbol;

        public int Power;

        public NewtonsoftIndeterminate()
        {
        }

        public NewtonsoftIndeterminate(char symbol, int power)
        {
            this.Symbol = symbol;
            this.Power = power;
        }
    }
}



