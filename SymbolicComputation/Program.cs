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
            string filepath = "../../../input.json";

            StreamReader sr = new StreamReader(filepath);

            Expression asd = (Expression) (Parser.ParseInput(sr.ReadToEnd()));
            sr.Close();
            Console.WriteLine($"Got expression: {asd}");

            Scope context = new Scope();

            Symbol Sum = new StringSymbol("Sum");
            Symbol Sub = new StringSymbol("Sub");
            Symbol Mul = new StringSymbol("Mul");
            Symbol List = new StringSymbol("List");
            Symbol Set = new StringSymbol("Set");
            Symbol Pow = new StringSymbol("Pow");
            Symbol Delayed = new StringSymbol("Delayed");
            Symbol Equal = new StringSymbol("Equal");
            Symbol Or = new StringSymbol("Or");
            Symbol And = new StringSymbol("And");
            Symbol Xor = new StringSymbol("Xor");
            Symbol Not = new StringSymbol("Not");
            Symbol Greater = new StringSymbol("Greater");
            Symbol GreaterOrEqual = new StringSymbol("GreaterOrEqual");
            Symbol Less = new StringSymbol("Less");
            Symbol LessOrEqual = new StringSymbol("LessOrEqual");
            Symbol If = new StringSymbol("If");
            Symbol While = new StringSymbol("While");
            Symbol Divide = new StringSymbol("Divide");
            Symbol L = new StringSymbol("L");
            Symbol First = new StringSymbol("First");
            Symbol Rest = new StringSymbol("Rest");

            Expression p1Func = Sum["t", 1];

            Symbol P1 = new StringSymbol("P1");

            Expression delExp = Delayed["P1", "t", p1Func];
            Expression testDelExp = List[delExp, P1[2]];

            Expression exp2 = Mul["y", 2];

            Expression exp1 = Sum[Mul[Pow["x", 2], Pow["y", 3], 12], Mul[6, Pow["x", 7], Pow["y", 2]], "x"];

            Expression inputExpDivision = Divide[asd, "y"];

            Expression exp4 = Sum[5, 2];
            Expression exp3 = List[Set["y", 10], Mul["x", Sum["y", 1]]];

            Expression setExp = Set["x", 2];
            Expression setExp2 = Set["y", Mul["x", 4]];


            Expression restExp = Rest[Rest[L[1, 2, 3, 4, 5]]];

            StringSymbol f1 = new StringSymbol("f1");
            Expression alg = List[
                Set["lest", L["x", "y"]],
                Set["firstTerm", 1],
                Set["ETMP", exp1],
                While[Not[Equal[First["lest"], "null"]],
                    List[
                        Set["cur", First["lest"]],
                        Delayed[f1, "cur", "ETMP"],
                        Set["zero-checker", f1[0]],
                        Set["power", 0],
                        While[Equal["zero-checker", 0],
                            List[
                                Set["ETMP", Divide["ETMP", "cur"]],
                                Delayed[f1, "cur", "ETMP"],
                                Set["zero-checker", f1[0]],
                                Set["power", Sum["power", 1]]
                            ]
                        ],
                        If[Equal["power", 1],
                            Set["firstTerm", Mul["firstTerm", "cur"]],
                            List[Boolean.False]
                        ],
                        If[Greater["power", 1],
                            Set["firstTerm", Mul["firstTerm", Pow["cur", "power"]]],
                            List[Boolean.False]
                        ],
                        Set["lest", Rest["lest"]]
                    ]
                ],
                Mul["firstTerm", "ETMP"]
            ];
            Expression test =
                List[
                    Set["x", 3],
                    While[Not[Equal["x", 0]],
                        Set["x", Sub["x", 1]]
                    ]
                ];
            Console.WriteLine(alg.Evaluate(context).ToString());
        }
    }
}