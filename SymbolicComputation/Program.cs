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

			Expression exp3 = Mul["x", "y"];

			Symbol Set = new StringSymbol("Set");

			Expression setExp = Set["x", 2];
			Expression setExp2 = Set["y", 3];

			Symbol List = new StringSymbol("List");

			Expression exp4 = List[setExp, setExp2, exp3];

			Symbol res = exp4.Evaluate();

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