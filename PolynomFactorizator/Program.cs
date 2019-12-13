using System;
using System.Collections.Generic;

namespace PolynomialFactorizator
{
    class Program
    {
        static void Main(string[] args)
        {
            Indeterminate x2 = new Indeterminate('x', 2);
            Indeterminate y = new Indeterminate('y', 1);
            Monomial M_2_x2_y = new Monomial('+', 2, new List<Indeterminate> { x2, y });

            Indeterminate x1 = new Indeterminate('x', 1);
            Indeterminate y2 = new Indeterminate('y', 2);
            Monomial M__4_y2_x = new Monomial('-', 4, new List<Indeterminate> { x1, y2 });


            Polynomial polynomial = new Polynomial(new List<Monomial> { M_2_x2_y, M__4_y2_x });

            Console.WriteLine(polynomial.ToString());
        }
    }
}
