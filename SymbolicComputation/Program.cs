using SymbolicComputationModel;
using SymbolicComputationPlots;
using System;
using System.Collections.Generic;
using System.IO;

namespace SymbolicComputation
{
    class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            string filepath = "../../../input.json";
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
            Symbol Rem = new StringSymbol("Rem");
            Symbol GetPolynomialCoefficients = new StringSymbol("GetPolynomialCoefficients");
            Symbol GetIndeterminateList = new StringSymbol("GetIndeterminateList");


            SymbolicComputationPlots.ExpressionHolder.expression =
                L[L[L[1, 2], L[3, 4], L[4, -1]], L[L[4, 1], L[6, 3]]];
            var app = new App();
            var window = new SymbolicComputationPlots.MainWindow();
            app.Run();

            Expression p1Func = Sum["t", 1];

            Symbol P1 = new StringSymbol("P1");

            Expression delExp = Delayed["P1", "t", p1Func];
            Expression testDelExp = List[delExp, P1[2]];

            Expression exp2 = Mul["y", 2];
            Expression exp4 = Sum[5, 2];
            Expression exp3 = List[Set["y", 10], Mul["x", Sum["y", 1]]];
            Expression setExp = Set["x", 2];
            Expression setExp2 = Set["y", Mul["x", 4]];


            Expression restExp = Rest[Rest[L[1, 2, 3, 4, 5]]];


            Scope context = new Scope();
            StreamReader sr = new StreamReader(filepath);

            // Expression exp1 = Sum[Mul[Pow["x", 2], Pow["y", 3], 12], Mul[6, Pow["x", 7], Pow["y", 2]], Mul[Pow["x", 3], 3]];
            Expression exp1 = (Expression) (Parser.ParseInput(sr.ReadToEnd(), context));
            sr.Close();

            Console.WriteLine($"Got expression: {exp1}");

            Expression ourList = (Expression) GetPolynomialCoefficients[exp1].Evaluate(context);

            Expression minFunc = List[
                Set["lest", ourList],
                Set["minEl", First["lest"]],
                Set["tempLest", "lest"],
                While[Not[Equal[First["tempLest"], "null"]],
                    List[
                        If[Less[First["tempLest"], "minEl"],
                            List[
                                Set["minEl", First["tempLest"]]
                            ],
                            List[
                                SymbolicComputationModel.Boolean.False
                            ]
                        ],
                        Set["tempLest", Rest["tempLest"]]
                    ]
                ],
                "minEl"
            ];

            StringSymbol f1 = new StringSymbol("f1");
            Expression alg = List[
                Set["lest", GetIndeterminateList[exp1]],
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
                            List[SymbolicComputationModel.Boolean.False]
                        ],
                        If[Greater["power", 1],
                            Set["firstTerm", Mul["firstTerm", Pow["cur", "power"]]],
                            List[SymbolicComputationModel.Boolean.False]
                        ],
                        Set["lest", Rest["lest"]]
                    ]
                ],
                Set["lest", ourList], //TODO : Get list before evaluation
                Set["isFound", "False"],
                Set["divisor", minFunc],
                Set["commonDivisor", 1],
                Set["tempLest", "lest"],
                While[Not[Equal["divisor", 1]],
                    List[
                        Set["reminder", 0],
                        While[Not[Equal[First["tempLest"], "null"]],
                            List[
                                Set["reminder", Sum[Rem[First["tempLest"], "divisor"], "reminder"]],
                                Set["tempLest", Rest["tempLest"]]
                            ]
                        ],
                        If[Equal["reminder", 0],
                            List[
                                Set["commonDivisor", Mul["commonDivisor", "divisor"]],
                                Set["tempLest", Divide["lest", "divisor"]],
                                Set["divisor", 1]
                            ],
                            List[
                                Set["tempLest", "lest"],
                                Set["divisor", Sub["divisor", 1]]
                            ]
                        ]
                    ]
                ],
                "commonDivisor",
                Mul[Mul["firstTerm", "commonDivisor"], Divide["ETMP", "commonDivisor"]]
            ];


            //Expression numAlg = List[
            //    Set["lest", ourList],
            //    Set["isFound", "False"],
            //    Set["divisor", minFunc],
            //    Set["commonDivisor", 1],
            //    Set["tempLest", "lest"],
            //    While[Not[Equal["divisor", 1]],
            //        List[
            //            Set["reminder", 0],
            //            While[Not[Equal[First["tempLest"], "null"]],
            //                List[
            //                    Set["reminder", Sum[Rem[First["tempLest"], "divisor"], "reminder"]],
            //                    Set["tempLest", Rest["tempLest"]]
            //                ]
            //            ],
            //            If[Equal["reminder", 0],
            //                List[
            //                    Set["commonDivisor", Mul["commonDivisor", "divisor"]],
            //                    Set["tempLest", Divide["lest", "divisor"]],
            //                    Set["divisor", 1]
            //                ],
            //                List[
            //                    Set["tempLest", "lest"],
            //                    Set["divisor", Sub["divisor", 1]]
            //                ]
            //            ]
            //        ]
            //    ],
            //    "commonDivisor"
            //];

            List<Symbol> b = new List<Symbol>() {new StringSymbol("a"), new StringSymbol("q")};
            Expression a = GetPolynomialCoefficients[exp1];
            Console.WriteLine(alg.Evaluate(context).ToString());
        }
    }
}