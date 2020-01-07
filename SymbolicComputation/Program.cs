using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using SymbolicComputation.Model;

namespace SymbolicComputation
{
    class Program
    {
        private static void Main(string[] args)
        {
            Symbol Sum = new StringSymbol("Sum");
            Expression exp1 = Sum[5, 3];

            Symbol Mul = new StringSymbol("Mul");
            Symbol Sub = new StringSymbol("Sub");

            Expression exp2 = Mul[5, exp1];

            Symbol res = exp2.Evaluate();

            Console.WriteLine(res.ToString());


            Expression exp_ = Mul[5, "x"];
            Expression exp_1 = Mul[3, "y"];
            Expression sum = Sub[exp_, exp_1];
            Symbol result = sum.Evaluate();
            //Console.WriteLine(JsonConvert.SerializeObject(result));
            using (StreamReader sr = new StreamReader("../../../input.json"))
            {
                Expression asd = (Expression)(Parser.ParseInput(sr.ReadToEnd()));
                Console.WriteLine(asd.Evaluate());
            }

             
        }
    }
}