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
			string filepath = "../../../inputPlot1.json";
			Scope context = new Scope();
			StreamReader sr = new StreamReader(filepath);
			Expression exp1 = (Expression)(Parser.ParseInput(sr.ReadToEnd(), context));
			sr.Close();
			Console.WriteLine($"Got expression: {exp1}");

			if (exp1.Action.Equals(Plot))
			{
				//Expression plotExpression = Plot[L[L[L[1, 2], L[3, 4], L[4, -1]], L[L[4, 5], L[6, 3]]], 800, 500];
				Expression plotExpression = (Expression)exp1.Args[0];
				Constant width = (Constant) exp1.Args[1];
				Constant height = (Constant) exp1.Args[2];
				var window = new MainWindow(plotExpression, width.Value, height.Value);
				window.ShowDialog();
			}
			else
			{
				// Expression exp1 = Sum[Mul[Pow["x", 2], Pow["y", 3], 12], Mul[6, Pow["x", 7], Pow["y", 2]], Mul[Pow["x", 3], 3]];

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
		}
	}
}