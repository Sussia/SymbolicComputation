using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using SymbolicComputation.Model;
using Expression = SymbolicComputation.Model.Expression;

namespace SymbolicComputation
{
    public static class BuildInFunctions
    {
        private static Scope context = new Scope();

        private static Dictionary<string, Func<Expression, Symbol>> functionsDictionary =
            new Dictionary<string, Func<Expression, Symbol>>
            {
                {"Sum", Sum},
                {"Sub", Sub},
                {"Mul", Mul},
                {"Div", Div},
                {"Rem", Rem},
                {"Divide", Divide},
                {"Pow", Pow},
                {"List", List},
                {"Equal", Equal},
                {"Or", Or},
                {"And", And},
                {"Xor", Xor},
                {"Not", Not},
                {"Greater", Greater},
                {"GreaterOrEqual", GreaterOrEqual},
                {"Less", Less},
                {"LessOrEqual", LessOrEqual}
            };

        private static Dictionary<string, Tuple<StringSymbol, Expression>> customFunctions =
            new Dictionary<string, Tuple<StringSymbol, Expression>>();

        private static Symbol MathEval(Expression exp, Func<decimal, decimal, decimal> func)
        {
            var arg1 = exp.Args[0];
            var arg2 = exp.Args[1];

            if (arg1 is Constant constant1 && arg2 is Constant constant2)
            {
                return new Constant(func(constant1.Value, constant2.Value));
            }

            return exp;
        }
        private static Symbol LogicEval(Expression exp, Func<decimal, decimal, bool> func)
        {
	        var arg1 = exp.Args[0];
	        var arg2 = exp.Args[1];
	        if (arg1 is Constant constant1 && arg2 is Constant constant2)
	        {
		        return (func(constant1.Value, constant2.Value)) ? new StringSymbol("True") : new StringSymbol("False");
	        }

	        return new StringSymbol("False");
        }
        public static Symbol Evaluate(Expression exp)
		{
			Console.WriteLine($"\nEvaluating expression {exp}:");
			if (exp.Action.ToString() == "Set")
			{
				return Set(exp, context);
			}
			if (exp.Action.ToString() == "Delayed")
			{
				return Delayed(exp);
			}

			if (exp.Action.ToString() == "If")
			{
				return If(exp);
			}
			List<Symbol> newArgs = new List<Symbol>();
			foreach (var arg in exp.Args)
			{
				if (arg is Expression argExpression)
				{
					newArgs.Add(Evaluate(argExpression));
				}
				else if (arg is StringSymbol symbol)
				{
					newArgs.Add(Substitute(symbol, context));
				} else
				{
					newArgs.Add(arg);
				}
			}
			Expression newExp = new Expression(exp.Action, newArgs.ToArray());
			if (!newExp.Equals(exp))
			{
				Console.WriteLine($"\n  Evaluating expression {newExp}:");
			}
			Symbol result = functionsDictionary[exp.Action.ToString()](newExp);
			Console.WriteLine($"The result of {newExp} is {result}");
			return result;
		}

		private static Symbol If(Expression exp)
		{
			Symbol cond = exp.Args[0];
			Symbol condResult;
			if (cond is Expression expression)
			{
				condResult = expression.Evaluate();
			} else if (cond.Equals(Boolean.True) || cond.Equals(Boolean.False))
			{
				condResult = cond;
			}
			else
			{
				throw new Exception("Wrong condition");
			}

			if (exp.Args[1] is Expression body1 && exp.Args[2] is Expression body2)
			{
				return condResult.Equals(Boolean.True) ? body1.Evaluate() : body2.Evaluate();
			}
			throw new Exception("Wrong body");
		}
		private static Symbol Equal(Expression exp)
		{
			return exp.Args[0].Equals(exp.Args[1]) ? new StringSymbol("True") : new StringSymbol("False");
		}

		private static Symbol Or(Expression exp)
		{
			Symbol arg1 = exp.Args[0];
			Symbol arg2 = exp.Args[1];
			if ((arg1.Equals(Boolean.True) || arg1.Equals(Boolean.False)) && (arg2.Equals(Boolean.True) || arg2.Equals(Boolean.False)))
			{
				return arg1.Equals(Boolean.False) && arg2.Equals(Boolean.False) ? Boolean.False : Boolean.True;
			}
			throw new Exception("Wrong arguments");
		}

		private static Symbol And(Expression exp)
		{
			Symbol arg1 = exp.Args[0];
			Symbol arg2 = exp.Args[1];
			if ((arg1.Equals(Boolean.True) || arg1.Equals(Boolean.False)) && (arg2.Equals(Boolean.True) || arg2.Equals(Boolean.False)))
			{
				return arg1.Equals(Boolean.True) && arg2.Equals(Boolean.True) ? Boolean.True : Boolean.False;
			}
			throw new Exception("Wrong arguments");
		}

		private static Symbol Not(Expression exp)
		{
			Symbol arg1 = exp.Args[0];
			if (arg1.Equals(Boolean.True) || arg1.Equals(Boolean.False))
			{
				return arg1.Equals(Boolean.True) ? Boolean.False : Boolean.True;
			}
			throw new Exception("Wrong arguments");
		}

		private static Symbol Xor(Expression exp)
		{
			Symbol arg1 = exp.Args[0];
			Symbol arg2 = exp.Args[1];
			if ((arg1.Equals(Boolean.True) || arg1.Equals(Boolean.False)) && (arg2.Equals(Boolean.True) || arg2.Equals(Boolean.False)))
			{
				return arg1.Equals(arg2)? Boolean.False : Boolean.True;
			}
			throw new Exception("Wrong arguments");
		}

		private static Symbol Greater(Expression exp)
		{
		return LogicEval(exp, (a, b) => a > b);
		}

		private static Symbol GreaterOrEqual(Expression exp)
		{
		return LogicEval(exp, (a, b) => a >= b);
		}

		private static Symbol LessOrEqual(Expression exp)
		{
		return LogicEval(exp, (a, b) => a <= b);
		}

		private static Symbol Less(Expression exp)
		{
		return LogicEval(exp, (a, b) => a < b);
		}

        private static Symbol Sum(Expression exp)
		{
			Symbol[] symbols = exp.Args.Select(x => x is Constant symbol ? null : x).Where(x => x != null).ToArray();
			decimal sum = exp.Args.Where(x => x is Constant).Aggregate(0m, (acc, x) => acc + ((Constant)x).Value);
			Symbol constant = new Constant(sum);
			if (symbols.Length == 0)
			{
				return constant;
			}

            if (sum == 0)
            {
                return new Expression(exp.Action, symbols);
            }

            Symbol[] args = new Symbol[symbols.Length + 1];
            args[0] = constant;
            Array.Copy(symbols, 0, args, 1, symbols.Length);
            return new Expression(exp.Action, args);
        }

        private static Symbol Sub(Expression exp)
        {
            return MathEval(exp, (a, b) => a - b);
        }

        private static Symbol Mul(Expression exp)
        {
            Symbol[] symbols = exp.Args.Select(x => x !is Constant symbol ? null : x)
                .Where(x => x != null).ToArray();
            decimal sum = exp.Args.Where(x => x is Constant).Aggregate(1m, (acc, x) => acc * ((Constant) x).Value);
            Symbol constant = new Constant(sum);
            if (symbols.Length == 0)
            {
                return constant;
            }

            if (sum == 1)
            {
	            if (symbols.Length == 1)
	            {
		            return symbols[0];
	            }
                return new Expression(exp.Action, symbols);
            }

            Symbol[] args = new Symbol[symbols.Length + 1];
            args[0] = constant;
            Array.Copy(symbols, 0, args, 1, symbols.Length);
            return new Expression(exp.Action, args);
        }

        private static Symbol Divide(Expression exp)
        {
	        Symbol arg1 = exp.Args[0];
	        Symbol arg2 = exp.Args[1];
			if (arg1.Equals(arg2)) return new Constant(1);
	        if (arg1 is Expression innerExpression)
	        {
		        if (innerExpression.Action.ToString() == "Pow" && innerExpression.Args[0].Equals(arg2))
		        {
			        if (innerExpression.Args[1] is Constant power1 && power1.Value == 2)
			        {
				        return innerExpression.Args[0];
			        }
			        if (innerExpression.Args[1] is Constant power0 && power0.Value == 1)
			        {
				        return new Constant(0);
			        }
                    if (innerExpression.Args[1] is Constant power && power.Value != 2)
			        {
						return new Expression(innerExpression.Action, new []{innerExpression.Args[0], new Constant(power.Value - 1)});
			        }
		        }
		        if (innerExpression.Action.ToString() == "Mul")
		        {
                    Symbol divide = new StringSymbol("Divide");
                    bool dividable = false;
                    Symbol dividend = null;
			        foreach (Symbol arg in innerExpression.Args)
			        {
				        if (!(divide[arg, arg2].Evaluate() is Expression divideExpression &&
				            divideExpression.Action.ToString() == "Divide"))
				        {
					        dividable = true;
					        dividend = arg;
                            break;
				        }
			        }

			        if (dividable)
			        {
				        Symbol newArg = Divide(divide[dividend, arg2]);
				        Symbol[] newArgs = innerExpression.Args.Select(x => x.Equals(dividend) ? newArg : x).ToArray();
                        return new Expression(innerExpression.Action, newArgs).Evaluate();
			        }
		        }

		        if (innerExpression.Action.ToString() == "Sum")
		        {
			        Symbol divide = new StringSymbol("Divide");
			        Symbol[] newArgs = innerExpression.Args.Select(x => Divide(divide[x, arg2])).ToArray();
			        if (!newArgs.Any(x => x is Expression divisionExp && divisionExp.Action.ToString() == "Divide"))
			        {
						return new Expression(innerExpression.Action, newArgs).Evaluate();
			        }
		        }
	        }
            return MathEval(exp, (a, b) => a / b);
        }

        private static Symbol Div(Expression exp)
        {
            return MathEval(exp, (a, b) => (int) (a / b));
        }

        private static Symbol Rem(Expression exp)
        {
            return MathEval(exp, (a, b) => a % b);
        }

        private static Symbol Pow(Expression exp)
        {
            return MathEval(exp, (a, b) => (decimal) Math.Pow((double) a, (double) b));
        }

        private static Symbol List(Expression exp)
        {
            return exp.Args.Last();
        }

        private static Symbol Set(Expression exp, Scope localContext)
        {
            var arg1 = exp.Args[0];
            var arg2 = exp.Args[1];
            if (arg1 is StringSymbol symbol)
            {
                Symbol newArg;
                if (arg2 is Expression exp2)
                {
                    newArg = Evaluate(exp2);
                }
                else if (arg2 is StringSymbol stringSymbol)
                {
                    newArg = Substitute(stringSymbol, localContext);
                }
                else
                {
                    newArg = arg2;
                }

                localContext.SymbolRules.Add(symbol.Name, newArg);
                Console.WriteLine($"{symbol.Name} is initialized by {arg2}");
                return arg2;
            }

            throw new Exception("First parameter of Set isn't string symbol");
        }

        private static Symbol Substitute(StringSymbol symbol, Scope localContext)
        {
            return localContext.SymbolRules.TryGetValue(symbol.Name, out var resultSymbol) ? resultSymbol : symbol;
        }

        private static Symbol Delayed(Expression exp)
        {
            Symbol name = exp.Args[0];
            StringSymbol variable = (StringSymbol) exp.Args[1];
            Expression function = (Expression) exp.Args[2];
            functionsDictionary.Add(name.ToString(), ComputeDelayed);
            customFunctions.Add(name.ToString(), new Tuple<StringSymbol, Expression>(variable, function));
            Console.WriteLine($"{name}({variable}) is defined as {function}");
            return exp;
        }

        private static Symbol ComputeDelayed(Expression exp)
        {
            string name = exp.Action.ToString();
            var (variable, function) = customFunctions[name];
            Expression newExpression = ReplaceVariable(function, variable, exp.Args[0]);
            return Evaluate(newExpression);
        }

        public static Expression ReplaceVariable(Expression exp, StringSymbol localVariable, Symbol value)
        {
            Console.Write($"Replacing {localVariable} with {value}... ");
            Scope localScope = new Scope();
            localScope.SymbolRules.Add(localVariable.Name, value);
            List<Symbol> newArgs = new List<Symbol>();
            foreach (var arg in exp.Args)
            {
                if (arg is Expression argExp)
                {
                    if (argExp.Action.ToString() == "Set")
                    {
                        newArgs.Add(argExp);
                    }
                    else
                    {
                        newArgs.Add(ReplaceVariable(argExp, localVariable, value));
                    }
                }
                else if (arg is StringSymbol symbol)
                {
                    newArgs.Add(Substitute(symbol, localScope));
                }
                else
                {
                    newArgs.Add(arg);
                }
            }

            Expression newExp = new Expression(exp.Action, newArgs.ToArray());
            Console.WriteLine($"Result: {newExp}");
            return newExp;
        }
    }
}