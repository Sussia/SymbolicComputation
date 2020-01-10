using System.Collections.Generic;

namespace PolynomialFactorizator.NewtonsoftModels
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
