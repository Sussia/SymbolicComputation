using SymbolicComputationPlots;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text.Json.Serialization;
using System.Windows;
using Newtonsoft.Json;
using SymbolicComputationLib;
using SymbolicComputationLib.Model;
using static SymbolicComputationLib.PredefinedSymbols;
using Expression = SymbolicComputationLib.Model.Expression;


namespace SymbolicComputation
{
    class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            string filepath = "../../../inputPlot2.json";
            Scope context = new Scope();
            StreamReader sr = new StreamReader(filepath);
            string inputJson = sr.ReadToEnd();
            Expression inputExp = (Expression) (Parser.ParseInput(inputJson, context));
            Console.WriteLine(JsonConvert.SerializeObject(Plot[L[L[L[0, 0], L[1, 0]], L[L[0, 1], L[2, 1]], L[L[1, 2], L[2, 2]], L[L[0, 1], L[0, 2]], L[L[1, 0], L[1, 2]], L[L[2, 0], L[2, 1]]], 800, 500]));
            
            sr.Close();
            Console.WriteLine($"Got expression: {inputExp}");

            if (inputExp.Action.Equals(Plot))
            {
                //Expression plotExpression = Plot[L[L[L[1, 2], L[3, 4], L[4, -1]], L[L[4, 5], L[6, 3]]], 800, 500];
                Expression plotExpression = (Expression) inputExp.Args[0];
                Constant width = (Constant) inputExp.Args[1];
                Constant height = (Constant) inputExp.Args[2];
                var window = new MainWindow(plotExpression, inputJson, width.Value, height.Value);
                window.ShowDialog();
            }
            else if (inputExp.Action.Equals(Gcd))
            { 
                Expression exp1 = (Expression) inputExp.Args[0];
                //Test delayed functions
                Expression p1Func = Sum["t", 1];
                Symbol P1 = new StringSymbol("P1");
                Expression delExp = Delayed[P1, "t", p1Func];
                Expression testDelExp = List[delExp, P1[2]];

                Expression minFunc = List[
                    Set["lest", GetPolynomialCoefficients[exp1]],
                    Set["minEl", First["lest"]],
                    Set["tempLest", "lest"],
                    While[Not[Equal[First["tempLest"], "null"]],
                        List[
                            If[Less[First["tempLest"], "minEl"],
                                List[
                                    Set["minEl", First["tempLest"]]
                                ],
                                List[
                                    False
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
                                List[False]
                            ],
                            If[Greater["power", 1],
                                Set["firstTerm", Mul["firstTerm", Pow["cur", "power"]]],
                                List[False]
                            ],
                            Set["lest", Rest["lest"]]
                        ]
                    ],
                    Set["lest", GetPolynomialCoefficients[exp1]], //TODO : Get list before evaluation
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

                Console.WriteLine($"\n{exp1} = {alg.Evaluate(context)}");
            }
            else
            {
                Console.WriteLine($"\nThe result of {inputExp}: {inputExp.Evaluate(context)}");
            }
        }
    }
}