using System;
using System.Collections.Generic;

namespace PolynomialFactorizator
{
    internal class Program
    {
        private static void Main(string[] args) 
        {
            var x2 = new Indeterminate('x', 2);
            var y = new Indeterminate('y', 1);
            var M_2_x2_y = new Monomial(true, 2, new List<Indeterminate> { x2, y });

            var x1 = new Indeterminate('x', 1);
            var y2 = new Indeterminate('y', 2);
            var M__4_y2_x = new Monomial(false, 4, new List<Indeterminate> { x1, y2 });


            var polynomial = new Polynomial(new List<Monomial> { M_2_x2_y, M__4_y2_x });

            Console.WriteLine(polynomial.ToString());
        }
    }
}
