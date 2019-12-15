using System;
using System.Collections.Generic;
using System.Linq;

namespace PolynomialFactorizator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var x2 = new Indeterminate('x', 3);
            var z1 = new Indeterminate('z', 1);
            var y = new Indeterminate('y', 1);
            var M_2_x2_y = new Monomial(false, 70, new List<Indeterminate>
            {
                x2,
                z1,
                y
            });

            var x1 = new Indeterminate('x', 2);
            var y2 = new Indeterminate('y', 2);
            var M__4_y2_x = new Monomial(true, 66, new List<Indeterminate>
            {
                x1,
                y2
            });


            var x3 = new Indeterminate('x', 2);
            var y3 = new Indeterminate('y', 2);
            var z3 = new Indeterminate('z', 1);
            var M_4_y2_x = new Monomial(true, 24, new List<Indeterminate>
            {
                x3,
                y3
            });

            var polynomial = new Polynomial(new List<Monomial> {M_2_x2_y, M__4_y2_x, M_4_y2_x});
            Console.WriteLine($"IN POLYNOMIAL   {polynomial.ToString()}");

            var coefficientFactorList = new List<List<int>>();
            var indeterminateList = new List<List<char>>();
            foreach (var term in polynomial.Terms)
            {
                var factorList = Generate(term.Coefficient);
                coefficientFactorList.Add(factorList);


                var indList = new List<char>();
                foreach (var indeterminate in term.IndeterminatesList)
                {
                    indList.Add(indeterminate.Symbol);
                }

                indeterminateList.Add(indList);
            }

            var coefficientIntersection =
                coefficientFactorList.Aggregate((previousList, nextList) => previousList.Intersect(nextList).ToList());

            var coefficientPowerList = new List<int>();
            foreach (var coefficient in coefficientIntersection)
            {
                var powers = new List<int>();
                foreach (var asd in coefficientFactorList)
                {
                    powers.Add(asd.FindAll(x => x == coefficient).Count());
                }

                coefficientPowerList.Add(powers.Min());
            }

            var indeterminateIntersection =
                indeterminateList.Aggregate((previousList, nextList) => previousList.Intersect(nextList).ToList());
            var indeterminatePowerList = new List<int>();
            foreach (var indeterminate in indeterminateIntersection)
            {
                var powers = new List<int>();
                foreach (var term in polynomial.Terms)
                {
                    foreach (var a in term.IndeterminatesList)
                    {
                        if (a.Symbol == indeterminate)
                        {
                            powers.Add(a.Power);
                        }
                    }
                }

                indeterminatePowerList.Add(powers.Min());
            }


            int outCoefficient = 1;
            for (int i = 0; i < coefficientPowerList.Count; i++)
            {
                outCoefficient *= (int) Math.Pow(coefficientIntersection[i], coefficientPowerList[i]);
            }

            var outIndeterminatesList = new List<Indeterminate>();
            for (int i = 0; i < indeterminatePowerList.Count; i++)
            {
                outIndeterminatesList.Add(new Indeterminate(indeterminateIntersection[i], indeterminatePowerList[i]));
            }

            bool outSign = (polynomial.Terms[0].Sign);

            OutPolynomial outPolynomial =
                new OutPolynomial(new Monomial(outSign, outCoefficient, outIndeterminatesList), CloneTerms(polynomial.Terms));
            Console.WriteLine($"OUTPOLYNOMIAL : {outPolynomial.ToString()}");
            // Console.WriteLine(polynomial.ToString());
        }

        private static List<Monomial> CloneTerms(List<Monomial> originalList)
        {
            List<Monomial> lstCloned = originalList.Select(i => (Monomial)i.Clone()).ToList();
            return lstCloned;
        }
        public static List<int> Generate(int number)
        {
            var primes = new List<int>();

            for (int div = 2; div <= number; div++)
            {
                while (number % div == 0)
                {
                    primes.Add(div);
                    number /= div;
                }
            }

            return primes;
        }
    }
}