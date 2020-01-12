using SymbolicComputationPlots;
using System;
using System.Diagnostics.Eventing.Reader;
using System.IO;
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
			if (true)
			{
				//    Test zone    \\
				Expression testExpression = List[
					Set["fi", Mul[-4, 3.14m]],
					Set["line", L[L[0, 0]]],
					Set["a", 0.1m],
					While[Less["fi", 1],
						List[
							Set["x", Mul["a", Sin["fi"]]],
							Set["y", Mul["a", Cos["fi"]]],
							Set["a", Sum["a", 0.1m]],
							Set["fi", Sum["fi", 0.01m]],
							Prepend["line", L["x","y"]]
						]
					],
					"line"
				];
				Console.WriteLine(testExpression.Evaluate(new Scope()));
				return;
			}
			//	If implementation:
			//Delayed[If, List[
			//	SetDelayed[localIf[True], "then"],
			//	SetDelayed[localIf[False], "else"],
			//	localIf["expression"]
			//], "expression", "then", "else"],
			//SetAttribute[If, HoldRest]

			string filepath = "../../../TaskExamples/inputPlot.json";
			Scope context = new Scope();
			StreamReader sr = new StreamReader(filepath);
			string inputJson = sr.ReadToEnd();
			Expression inputExp = (Expression)(Parser.ParseInput(inputJson, context));

			sr.Close();
			Console.WriteLine($"Got expression: {inputExp}");

			//If we got Plot() operator -----------------------------------------------------
			if (inputExp.Action.Equals(Plot))
			{
				Expression plotExpression = (Expression)inputExp.Args[0];
				Constant width = (Constant)inputExp.Args[1];
				Constant height = (Constant)inputExp.Args[2];
				var window = new MainWindow(plotExpression, width.Value, height.Value);
				window.ShowDialog();
			}
			//If we got Gcd() operator ------------------------------------------------------
			else if (inputExp.Action.Equals(Gcd))
			{
				Expression exp1 = (Expression)inputExp.Args[0];

				//Expression minFunc = List[
				//	Set["lest", GetPolynomialCoefficients[exp1]],
				//	Set["minEl", First["lest"]],
				//	Set["tempLest", "lest"],
				//	While[Not[Equal[First["tempLest"], "null"]],
				//		List[
				//			If[Less[First["tempLest"], "minEl"],
				//				List[
				//					Set["minEl", First["tempLest"]]
				//				],
				//				List[
				//					False
				//				]
				//			],
				//			Set["tempLest", Rest["tempLest"]]
				//		]
				//	],
				//	"minEl"
				//];

				//StringSymbol f1 = new StringSymbol("f1");
				//Expression alg = List[
				//	//This part finds common symbol divisor
				//	Set["lest", GetIndeterminateList[exp1]],
				//	Set["firstTerm", 1],
				//	Set["ETMP", exp1],
				//	While[Not[Equal[First["lest"], "null"]],
				//		List[
				//			Set["cur", First["lest"]],
				//			Delayed[f1, "cur", "ETMP"],
				//			Set["zero-checker", f1[0]],
				//			Set["power", 0],
				//			While[Equal["zero-checker", 0],
				//				List[
				//					Set["ETMP", Divide["ETMP", "cur"]],
				//					Delayed[f1, "cur", "ETMP"],
				//					Set["zero-checker", f1[0]],
				//					Set["power", Sum["power", 1]]
				//				]
				//			],
				//			If[Equal["power", 1],
				//				Set["firstTerm", Mul["firstTerm", "cur"]],
				//				List[False]
				//			],
				//			If[Greater["power", 1],
				//				Set["firstTerm", Mul["firstTerm", Pow["cur", "power"]]],
				//				List[False]
				//			],
				//			Set["lest", Rest["lest"]]
				//		]
				//	],
				//	//This part computes common number divisor
				//	Set["lest", GetPolynomialCoefficients[exp1]],
				//	Set["divisor", minFunc],
				//	Set["commonDivisor", 1],
				//	Set["tempLest", "lest"],
				//	While[Not[Equal["divisor", 1]],
				//		List[
				//			Set["reminder", 0],
				//			While[Not[Equal[First["tempLest"], "null"]],
				//				List[
				//					Set["reminder", Sum[Rem[First["tempLest"], "divisor"], "reminder"]],
				//					Set["tempLest", Rest["tempLest"]]
				//				]
				//			],
				//			If[Equal["reminder", 0],
				//				List[
				//					Set["commonDivisor", Mul["commonDivisor", "divisor"]],
				//					Set["tempLest", Divide["lest", "divisor"]],
				//					Set["divisor", 1]
				//				],
				//				List[
				//					Set["tempLest", "lest"],
				//					Set["divisor", Sub["divisor", 1]]
				//				]
				//			]
				//		]
				//	],
				//	"commonDivisor",
				//	Mul[Mul["firstTerm", "commonDivisor"], Divide["ETMP", "commonDivisor"]]
				//];
				Expression alg = null;
				Expression resultExpression = (Expression)alg.Evaluate(context);
				Expression beautifiedExp = (Expression)exp1.Evaluate(context);
				string link = "http://fred-wang.github.io/mathml.css/mspace.js";
				string script = $"<script src=\"{link}\"></script>\r\n";
				System.IO.File.WriteAllText(
					Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\index.html",
					script + Parser.ToMathML(Equal[beautifiedExp, resultExpression]));
				Console.WriteLine($"\n{exp1} = {resultExpression}");
			}
			//Otherwise ------------------------------------------------------------------
			else
			{
				Console.WriteLine($"\nThe result of {inputExp}: {inputExp.Evaluate(context)}");
			}
		}
	}
}