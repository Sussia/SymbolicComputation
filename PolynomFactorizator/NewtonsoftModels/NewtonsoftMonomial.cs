using System;
using System.Collections.Generic;
using System.Text;

namespace PolynomialFactorizator.NewtonsoftModels
{
    class NewtonsoftMonomial
    {
        public bool Sign;

        public int Coefficient;

        public List<NewtonsoftIndeterminate> IndeterminatesList;

        public NewtonsoftMonomial()
        {
            IndeterminatesList = new List<NewtonsoftIndeterminate>();
        }

        public NewtonsoftMonomial(bool sign, int coefficient, List<NewtonsoftIndeterminate> indeterminates)
        {
            this.Sign = sign;
            this.Coefficient = coefficient;
            this.IndeterminatesList = indeterminates;
        }
    }
}
