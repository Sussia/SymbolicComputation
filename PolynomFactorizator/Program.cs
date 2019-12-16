using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PolynomialFactorizator
{
    internal class Program
    {
        private static void Main(string[] args)
        {  
            string jsonInput = "";
            using (StreamReader r = new StreamReader(args[0]))
            {
                jsonInput = r.ReadToEnd();
            }

            var polynomial = new Polynomial(jsonInput);
            foreach (var term in polynomial.Terms)
            {
                Console.WriteLine(term.ToString());
            } 
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
                new OutPolynomial(new Monomial(outSign, outCoefficient, outIndeterminatesList),
                    CloneTerms(polynomial.Terms));
            Console.WriteLine($"OUTPOLYNOMIAL : {outPolynomial.ToString()}"); 
        }

        private static List<Monomial> CloneTerms(List<Monomial> originalList)
        {
            List<Monomial> lstCloned = originalList.Select(i => (Monomial) i.Clone()).ToList();
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