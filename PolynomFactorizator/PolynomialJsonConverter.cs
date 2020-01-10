using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using PolynomialFactorizator.NewtonsoftModels;
using PolynomialFactorizator.SymbolicModels;

namespace PolynomialFactorizator
{
    public static class PolynomialJsonConverter
    {
        public static Polynomial FromJson(string jsonInput)
        {

            var inPolynomial = JsonConvert.DeserializeObject<NewtonsoftPolynomial>(jsonInput);
            var termList = new List<Monomial>();
            foreach (var term in inPolynomial.Terms)
            {
                var inIndeterminateList = new List<Indeterminate>();
                foreach (var indeterminate in term.IndeterminatesList)
                {
                    inIndeterminateList.Add(new Indeterminate(indeterminate.Symbol, indeterminate.Power));
                }
                termList.Add(new Monomial(term.Sign, term.Coefficient, inIndeterminateList));
            }
            return new Polynomial(termList);
        }
    }
}
