using PolynomialFactorizator.NewtonsoftModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PolynomialFactorizator
{
    class NewtonsoftPolynomial
    {
        public List<NewtonsoftMonomial> Terms;

        public NewtonsoftPolynomial(List<NewtonsoftMonomial> terms)
        {
            this.Terms = terms;
        }

    }
}
