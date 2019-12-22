using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

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

            var polynomial = PolynomialJsonConverter.FromJson(jsonInput);
            foreach (var term in polynomial.Terms)
            {
                Console.WriteLine(term.ToString());
            }
            Console.WriteLine($"IN POLYNOMIAL   {polynomial.ToString()}");


            var firstMultiplier = PolynomialAlgorythms.GetFirstMultiplier(polynomial);


            var secondMultiplier = PolynomialAlgorythms.GetSecondMultiplier(polynomial, firstMultiplier);
            Console.WriteLine($"OUTPOLYNOMIAL : {firstMultiplier.ToString()} ({secondMultiplier.ToString()})");
            string link = "http://fred-wang.github.io/mathml.css/mspace.js";
            string script = (args.Length == 3 && args[2] == "-S") ? $"<script src=\"{link}\"></script>\r\n" : "";
            System.IO.File.WriteAllText(args[1], script + PolynomialMathMlConverter.ToMathMl(new List<Polynomial>() { firstMultiplier, secondMultiplier }));
        }
    }
}