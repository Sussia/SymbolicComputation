using System;
using System.Collections.Generic;
using System.Linq;
using SymbolicComputationModel.Model;
using static SymbolicComputationModel.PredefinedSymbols;

namespace SymbolicComputationModel
{
    public static class BuildInFunctions
    {
        private static Dictionary<string, Func<Expression, Scope, Symbol>> functionsDictionary =
            new Dictionary<string, Func<Expression, Scope, Symbol>>
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
                {"LessOrEqual", LessOrEqual},
                {"First", First},
                {"Rest", Rest},
                {"GetIndeterminateList", GetIndeterminateList},
                {"GetPolynomialCoefficients", GetPolynomialCoefficients}
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
                return (func(constant1.Value, constant2.Value)) ? True : False;
            }

            return False;
        }

        public static Symbol Evaluate(Expression exp, Scope context)
        {
            Console.WriteLine($"\nEvaluating expression {exp}:");

            if (exp.Action.Equals(PredefinedSymbols.Set))
            {
                return Set(exp, context);
            }

            if (exp.Action.Equals(PredefinedSymbols.Delayed))
            {
                return Delayed(exp, context);
            }

            if (exp.Action.Equals(PredefinedSymbols.If))
            {
                return If(exp, context);
            }

            if (exp.Action.Equals(PredefinedSymbols.While))
            {
                return While(exp, context);
            }

            if (exp.Action.Equals(PredefinedSymbols.L))
            {
                return L(exp, context);
            }

            List<Symbol> newArgs = new List<Symbol>();
            foreach (var arg in exp.Args)
            {
                if (arg is Expression argExpression)
                {
                    newArgs.Add(Evaluate(argExpression, context));
                }
                else if (arg is StringSymbol symbol)
                {
                    newArgs.Add(Substitute(symbol, context));
                }
                else
                {
                    newArgs.Add(arg);
                }
            }

            Expression newExp = new Expression(exp.Action, newArgs.ToArray());
            if (!newExp.Equals(exp))
            {
                Console.WriteLine($"\n  Evaluating expression {newExp}:");
            }

            Symbol result = functionsDictionary[exp.Action.ToString()](newExp, context);
            Console.WriteLine($"The result of {newExp} is {result}");
            return result;
        }

        private static Symbol If(Expression exp, Scope context)
        {
            Symbol cond = exp.Args[0];
            Symbol condResult;
            if (cond is Expression expression)
            {
                condResult = expression.Evaluate(context);
            }
            else if (cond.Equals(True) || cond.Equals(False))
            {
                condResult = cond;
            }
            else
            {
                throw new Exception("Wrong condition");
            }

            if (exp.Args[1] is Expression body1 && exp.Args[2] is Expression body2)
            {
                return condResult.Equals(True) ? body1.Evaluate(context) : body2.Evaluate(context);
            }

            throw new Exception("Wrong body");
        }

        private static Symbol While(Expression exp, Scope context)
        {
            Symbol cond = exp.Args[0];
            Symbol condResult = PredefinedSymbols.If[cond, PredefinedSymbols.List[True], PredefinedSymbols.List[False]].Evaluate(context);
            if (condResult.Equals(False))
            {
                return Ok;
            }

            Symbol body = exp.Args[1];
            if (body is Expression bodyExp)
            {
                bodyExp.Evaluate(context);
                While(exp, context);
                return Ok;
            }

            throw new Exception("Wrong body");
        }

        private static Symbol Equal(Expression exp, Scope scope)
        {
            return exp.Args[0].Equals(exp.Args[1]) ? True : False;
        }

        private static Symbol Or(Expression exp, Scope scope)
        {
            Symbol arg1 = exp.Args[0];
            Symbol arg2 = exp.Args[1];
            if ((arg1.Equals(True) || arg1.Equals(False)) &&
                (arg2.Equals(True) || arg2.Equals(False)))
            {
                return arg1.Equals(False) && arg2.Equals(False) ? False : True;
            }

            throw new Exception("Wrong arguments");
        }

        private static Symbol And(Expression exp, Scope scope)
        {
            Symbol arg1 = exp.Args[0];
            Symbol arg2 = exp.Args[1];
            if ((arg1.Equals(True) || arg1.Equals(False)) &&
                (arg2.Equals(True) || arg2.Equals(False)))
            {
                return arg1.Equals(True) && arg2.Equals(True) ? True : False;
            }

            throw new Exception("Wrong arguments");
        }

        private static Symbol Not(Expression exp, Scope scope)
        {
            Symbol arg1 = exp.Args[0];
            if (arg1.Equals(True) || arg1.Equals(False))
            {
                return arg1.Equals(True) ? False : True;
            }

            throw new Exception("Wrong arguments");
        }

        private static Symbol Xor(Expression exp, Scope scope)
        {
            Symbol arg1 = exp.Args[0];
            Symbol arg2 = exp.Args[1];
            if ((arg1.Equals(True) || arg1.Equals(False)) &&
                (arg2.Equals(True) || arg2.Equals(False)))
            {
                return arg1.Equals(arg2) ? False : True;
            }

            throw new Exception("Wrong arguments");
        }

        private static Symbol Greater(Expression exp, Scope scope)
        {
            return LogicEval(exp, (a, b) => a > b);
        }

        private static Symbol GreaterOrEqual(Expression exp, Scope scope)
        {
            return LogicEval(exp, (a, b) => a >= b);
        }

        private static Symbol LessOrEqual(Expression exp, Scope scope)
        {
            return LogicEval(exp, (a, b) => a <= b);
        }

        private static Symbol Less(Expression exp, Scope scope)
        {
            return LogicEval(exp, (a, b) => a < b);
        }

        private static Symbol Sum(Expression exp, Scope context)
        {
            Symbol[] symbols = exp.Args.Select(x => x is Constant symbol ? null : x).Where(x => x != null).ToArray();
            decimal sum = exp.Args.Where(x => x is Constant).Aggregate(0m, (acc, x) => acc + ((Constant) x).Value);
            Symbol constant = new Constant(sum);
            if (symbols.Length == 0)
            {
                return constant;
            }

            if (sum == 0)
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

        private static Symbol Sub(Expression exp, Scope scope)
        {
            return MathEval(exp, (a, b) => a - b);
        }

        private static Symbol Mul(Expression exp, Scope scope)
        {
            Symbol[] symbols = exp.Args.Select(x => x ! is Constant symbol ? null : x)
                .Where(x => x != null).ToArray();
            decimal sum = exp.Args.Where(x => x is Constant).Aggregate(1m, (acc, x) => acc * ((Constant) x).Value);
            Symbol constant = new Constant(sum);
            if (sum == 0)
            {
                return constant;
            }

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

        private static Symbol Divide(Expression exp, Scope context)
        {
            Symbol arg1 = exp.Args[0];
            Symbol arg2 = exp.Args[1];
            if (arg1.Equals(arg2)) return new Constant(1);
            if (arg1 is Expression innerExpression)
            {
                if (innerExpression.Action.Equals(PredefinedSymbols.Pow) && innerExpression.Args[0].Equals(arg2))
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
	                    return PredefinedSymbols.Pow[innerExpression.Args[0], new Constant(power.Value - 1)];
                    }
                }

                if (innerExpression.Action.Equals(PredefinedSymbols.Mul))
                {
                    bool dividable = false;
                    Symbol dividend = null;
                    foreach (Symbol arg in innerExpression.Args)
                    {
                        if (!(PredefinedSymbols.Divide[arg, arg2].Evaluate(context) is Expression divideExpression &&
                              divideExpression.Action.Equals(PredefinedSymbols.Divide)))
                        {
                            dividable = true;
                            dividend = arg;
                            break;
                        }
                    }

                    if (dividable)
                    {
                        Symbol newArg = Divide(PredefinedSymbols.Divide[dividend, arg2], context);
                        Symbol[] newArgs = innerExpression.Args.Select(x => x.Equals(dividend) ? newArg : x).ToArray();
                        return new Expression(innerExpression.Action, newArgs).Evaluate(context);
                    }
                }

                if (innerExpression.Action.Equals(PredefinedSymbols.Sum))
                {
                    Symbol[] newArgs = innerExpression.Args.Select(x => Divide(PredefinedSymbols.Divide[x, arg2], context)).ToArray();
                    if (!newArgs.Any(x => x is Expression divisionExp && divisionExp.Action.Equals(PredefinedSymbols.Divide)))
                    {
                        return new Expression(innerExpression.Action, newArgs).Evaluate(context);
                    }
                }

                if (innerExpression.Action.Equals(PredefinedSymbols.L))
                {
                    Symbol[] newArgs = innerExpression.Args.Select(x => Divide(PredefinedSymbols.Divide[x, arg2], context)).ToArray();
                    if (!newArgs.Any(x => x is Expression divisionExp && divisionExp.Action.Equals(PredefinedSymbols.Divide)))
                    {
                        return new Expression(innerExpression.Action, newArgs);
                    }
                }
            }

            return MathEval(exp, (a, b) => a / b);
        }

        private static Symbol Div(Expression exp, Scope scope)
        {
            return MathEval(exp, (a, b) => (int) (a / b));
        }

        private static Symbol Rem(Expression exp, Scope scope)
        {
            return MathEval(exp, (a, b) => a % b);
        }

        private static Symbol Pow(Expression exp, Scope scope)
        {
            return MathEval(exp, (a, b) => (decimal) Math.Pow((double) a, (double) b));
        }

        private static Symbol List(Expression exp, Scope scope)
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
                    newArg = Evaluate(exp2, localContext);
                }
                else if (arg2 is StringSymbol stringSymbol)
                {
                    newArg = Substitute(stringSymbol, localContext);
                }
                else
                {
                    newArg = arg2;
                }

                localContext.SymbolRules[symbol.Name] = newArg;
                Console.WriteLine($"{symbol.Name} is initialized by {arg2}");
                return arg2;
            }

            throw new Exception("First parameter of Set isn't string symbol");
        }

        private static Symbol Substitute(StringSymbol symbol, Scope localContext)
        {
            return localContext.SymbolRules.TryGetValue(symbol.Name, out var resultSymbol) ? resultSymbol : symbol;
        }

        private static Symbol Delayed(Expression exp, Scope localContext)
        {
            Symbol name = exp.Args[0];
            StringSymbol variable = (StringSymbol) Substitute((StringSymbol) exp.Args[1], localContext);
            Expression function;
            if (exp.Args[2] is Expression ex)
            {
                function = ex;
            }
            else if (exp.Args[2] is StringSymbol ss)
            {
                function = (Expression) Substitute(ss, localContext);
            }
            else
            {
                throw new Exception("Wrong body");
            }

            functionsDictionary[name.ToString()] = ComputeDelayed;
            customFunctions[name.ToString()] = new Tuple<StringSymbol, Expression>(variable, function);
            Console.WriteLine($"{name}({variable}) is defined as {function}");
            return exp;
        }

        private static Symbol ComputeDelayed(Expression exp, Scope context)
        {
            string name = exp.Action.ToString();
            var (variable, function) = customFunctions[name];
            Expression newExpression = ReplaceVariable(function, variable, exp.Args[0]);
            return Evaluate(newExpression, context);
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
                    if (argExp.Action.Equals(PredefinedSymbols.Set))
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

        private static Symbol GetIndeterminateList(Expression exp, Scope scope)
        {
            return new Expression(PredefinedSymbols.L, scope.IndeterminateList.ToArray());
        }

        public static Symbol L(Expression exp, Scope context)
        {
            return exp;
        }

        public static Symbol First(Expression exp, Scope context)
        {
	        if (exp.Args[0] is Expression listExp && listExp.Action.Equals(PredefinedSymbols.L))
            {
                return listExp.Args.Length > 0 ? listExp.Args[0] : Null;
            }
	        if (exp.Args[0].Equals(Null)) return exp.Args[0];

            throw new Exception("Argument is not list");
        }

        public static Symbol Rest(Expression exp, Scope context)
        {
            if (exp.Args[0] is Expression listExp && listExp.Action.Equals(PredefinedSymbols.L))
            {
                return listExp.Args.Length < 2
                    ? Null
                    : new Expression(listExp.Action, listExp.Args.Skip(1).ToArray());
            }

            throw new Exception("Argument is not list");
        }

        public static Symbol GetPolynomialCoefficients(Expression exp, Scope context)
        {
	        if (exp.Args.Length == 1 && exp.Args[0] is Expression sumExp && sumExp.Action.Equals(PredefinedSymbols.Sum))
	        {
		        if (sumExp.Args.Any(x => x is Constant))
		        {
                    throw new Exception("Unable to factor out: Polynomial has free term");
		        }
                List<Constant> coefficients = new List<Constant>();
                foreach (var term in sumExp.Args)
                {
	                if (term is Expression powExp && powExp.Action.Equals(PredefinedSymbols.Pow))
	                {
						coefficients.Add(new Constant(1));
	                }

	                if (term is Expression mulExp && mulExp.Action.Equals(PredefinedSymbols.Mul))
	                {
		                Constant coef = (Constant) mulExp.Args.SingleOrDefault(x => x is Constant);
                        coef ??= new Constant(1);
                        coefficients.Add(coef);
                    }
                }

                if (coefficients.Count > 0)
                {
	                return new Expression(PredefinedSymbols.L, coefficients.ToArray());
                }
	        }
            throw new Exception("Argument is not polynomial");
        }
    }
}