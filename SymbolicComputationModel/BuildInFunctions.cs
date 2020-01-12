using System;
using System.Collections.Generic;
using System.Linq;
using SymbolicComputationLib.Model;
using static SymbolicComputationLib.PredefinedSymbols;

namespace SymbolicComputationLib
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
                //{"Or", Or},
                //{"And", And},
                //{"Xor", Xor},
                //{"Not", Not},
                {"Greater", Greater},
                {"GreaterOrEqual", GreaterOrEqual},
                {"Less", Less},
                {"LessOrEqual", LessOrEqual},
                {"First", First},
                {"Rest", Rest},
                {"GetPolynomialCoefficients", GetPolynomialCoefficients},
                {"GetPolynomialIndeterminates", GetPolynomialIndeterminates},
                {"Prepend", Prepend},
                {"Set", Set},
                {"SetDelayed", Set},
                {"SetAttribute", SetAttribute},
                {"Delayed", Delayed}
            };

        private static Dictionary<string, Tuple<Symbol[], Symbol>> customFunctions =
            new Dictionary<string, Tuple<Symbol[], Symbol>>();

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

        public static Symbol EvaluateExpression(Expression exp, Scope context)
        {
            Console.WriteLine($"\nEvaluating {exp}:");
            if (exp.Action.Equals(PredefinedSymbols.Delayed))
            {
                return Delayed(exp, context);
            }

            List<Symbol> newArgs = new List<Symbol>();

            if (context.AttributeDictionary.ContainsKey(exp.Action.ToString()))
            {
                if (context.AttributeDictionary[exp.Action.ToString()].Equals(HoldFirst))
                {
                    newArgs.Add(exp.Args[0]);
                    newArgs.AddRange(exp.Args.Skip(1).Select(symbol => symbol.Evaluate(context)));
                }

                if (context.AttributeDictionary[exp.Action.ToString()].Equals(HoldAll))
                {
                    newArgs.AddRange(exp.Args);
                }

                if (context.AttributeDictionary[exp.Action.ToString()].Equals(HoldRest))
                {
                    newArgs.Add(exp.Args[0].Evaluate(context));
                    newArgs.AddRange(exp.Args.Skip(1));
                }
            }
            else
            {
                foreach (var arg in exp.Args)
                {
                    newArgs.Add(arg.Evaluate(context));
                }
            }

            Expression newExp = new Expression(exp.Action, newArgs.ToArray());

            if (functionsDictionary.ContainsKey(newExp.Action.ToString()))
            {
                Symbol result = functionsDictionary[newExp.Action.ToString()](newExp, context);
                Console.WriteLine($"The result of {newExp} is {result}");
                return result;
            }

            if (context.SymbolRules.ContainsKey(newExp.ToString()))
            {
                Symbol result = context.SymbolRules[newExp.ToString()].Evaluate(context);
                Console.WriteLine($"The result of {newExp} is {result}");
                return result;
            }

            Console.WriteLine(
                $"----------------------------------\n\n\t\tThere is no such function     {newExp}\n\n----------------------------------");
            return newExp;
        }

        //private static Symbol If(Expression exp, Scope context)
        //{
        //	Symbol cond = exp.Args[0];
        //	Symbol condResult;
        //	if (cond is Expression expression)
        //	{
        //		condResult = expression.EvaluateExpression(context);
        //	}
        //	else if (cond.Equals(True) || cond.Equals(False))
        //	{
        //		condResult = cond;
        //	}
        //	else
        //	{
        //		throw new Exception("Wrong condition");
        //	}

        //	if (exp.Args[1] is Expression body1 && exp.Args[2] is Expression body2)
        //	{
        //		return condResult.Equals(True) ? body1.EvaluateExpression(context) : body2.EvaluateExpression(context);
        //	}

        //	throw new Exception("Wrong body");
        //}

        //private static Symbol While(Expression exp, Scope context)
        //{
        //	Symbol cond = exp.Args[0];
        //	Symbol condResult = PredefinedSymbols.If[cond, PredefinedSymbols.List[True], PredefinedSymbols.List[False]].EvaluateExpression(context);
        //	if (condResult.Equals(False))
        //	{
        //		return Ok;
        //	}

        //	Symbol body = exp.Args[1];
        //	if (body is Expression bodyExp)
        //	{
        //		bodyExp.EvaluateExpression(context);
        //		While(exp, context);
        //		return Ok;
        //	}

        //	throw new Exception("Wrong body");
        //}

        private static Symbol Equal(Expression exp, Scope scope)
        {
            return exp.Args[0].Equals(exp.Args[1]) ? True : False;
        }

        //private static Symbol Or(Expression exp, Scope scope)
        //{
        //	Symbol arg1 = exp.Args[0];
        //	Symbol arg2 = exp.Args[1];
        //	if ((arg1.Equals(True) || arg1.Equals(False)) &&
        //		(arg2.Equals(True) || arg2.Equals(False)))
        //	{
        //		return arg1.Equals(False) && arg2.Equals(False) ? False : True;
        //	}

        //	throw new Exception("Wrong arguments");
        //}

        //private static Symbol And(Expression exp, Scope scope)
        //{
        //	Symbol arg1 = exp.Args[0];
        //	Symbol arg2 = exp.Args[1];
        //	if ((arg1.Equals(True) || arg1.Equals(False)) &&
        //		(arg2.Equals(True) || arg2.Equals(False)))
        //	{
        //		return arg1.Equals(True) && arg2.Equals(True) ? True : False;
        //	}

        //	throw new Exception("Wrong arguments");
        //}

        //private static Symbol Not(Expression exp, Scope scope)
        //{
        //	Symbol arg1 = exp.Args[0];
        //	if (arg1.Equals(True) || arg1.Equals(False))
        //	{
        //		return arg1.Equals(True) ? False : True;
        //	}

        //	throw new Exception("Wrong arguments");
        //}

        //private static Symbol Xor(Expression exp, Scope scope)
        //{
        //	Symbol arg1 = exp.Args[0];
        //	Symbol arg2 = exp.Args[1];
        //	if ((arg1.Equals(True) || arg1.Equals(False)) &&
        //		(arg2.Equals(True) || arg2.Equals(False)))
        //	{
        //		return arg1.Equals(arg2) ? False : True;
        //	}

        //	throw new Exception("Wrong arguments");
        //}

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
            Symbol[] symbols = exp.Args.Select(x => x! is Constant symbol ? null : x)
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
                    Symbol[] newArgs = innerExpression.Args
                        .Select(x => Divide(PredefinedSymbols.Divide[x, arg2], context)).ToArray();
                    if (!newArgs.Any(x =>
                        x is Expression divisionExp && divisionExp.Action.Equals(PredefinedSymbols.Divide)))
                    {
                        return new Expression(innerExpression.Action, newArgs).Evaluate(context);
                    }
                }

                if (innerExpression.Action.Equals(PredefinedSymbols.L))
                {
                    Symbol[] newArgs = innerExpression.Args
                        .Select(x => Divide(PredefinedSymbols.Divide[x, arg2], context)).ToArray();
                    if (!newArgs.Any(x =>
                        x is Expression divisionExp && divisionExp.Action.Equals(PredefinedSymbols.Divide)))
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

            localContext.SymbolRules[arg1.ToString()] = arg2;
            Console.WriteLine($"{arg1} is initialized by {arg2}");
            return Ok;
        }

        private static Symbol Substitute(StringSymbol symbol, Scope localContext)
        {
            return localContext.SymbolRules.TryGetValue(symbol.Name, out var resultSymbol) ? resultSymbol : symbol;
        }

        private static Symbol Delayed(Expression exp, Scope localContext)
        {
            Symbol name = exp.Args[0];
            Symbol function = exp.Args[1];

            function = function is StringSymbol a ? Substitute(a, localContext) : function;

            Symbol[] localArgs = exp.Args.Skip(2).Select(x => Substitute((StringSymbol) x, localContext)).ToArray();

            functionsDictionary[name.ToString()] = ComputeDelayed;
            customFunctions[name.ToString()] = new Tuple<Symbol[], Symbol>(localArgs, function);
            Console.WriteLine($"{name}({localArgs}) is defined as {function}");
            return Ok;
        }

        private static Symbol ComputeDelayed(Expression exp, Scope context)
        {
            string name = exp.Action.ToString();
            var (variables, function) = customFunctions[name];
            Expression newExpression = (Expression) function;
            int counter = 0;
            foreach (StringSymbol variable in variables)
            {
                Symbol value = exp.Args[counter];
                Scope localContext = new Scope();
                localContext.SymbolRules.Add(variable.Name, value);
                newExpression = ReplaceVariable(newExpression, localContext);
                counter++;
            }

            return newExpression.Evaluate(context);
        }

        public static Expression ReplaceVariable(Expression exp, Scope context)
        {
            List<Symbol> newArgs = new List<Symbol>();
            foreach (var arg in exp.Args)
            {
                if (arg is Expression argExp)
                {
                    newArgs.Add(ReplaceVariable(argExp, context));
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
            return newExp;
        }

        public static Symbol First(Expression exp, Scope context)
        {
            return !(exp.Args[0].Equals(PredefinedSymbols.Null)) && ((Expression) exp.Args[0]).Args.Length > 0
                ? ((Expression) exp.Args[0]).Args[0]
                : Null;
        }

        public static Symbol Rest(Expression exp, Scope context)
        {
            return ((Expression) exp.Args[0]).Args.Length < 2
                ? Null
                : new Expression(((Expression) exp.Args[0]).Action, ((Expression) exp.Args[0]).Args.Skip(1).ToArray());
        }

        public static Symbol Prepend(Expression exp, Scope context)
        {
            Symbol prependedSymbol = exp.Args[1];
            Expression listHolder = (Expression) exp.Args[0];
            return new Expression(listHolder.Action, listHolder.Args.Prepend(prependedSymbol).ToArray());
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

        public static Symbol GetPolynomialIndeterminates(Expression exp, Scope context)
        {
            if (exp.Args.Length == 1 && exp.Args[0] is Expression sumExp && sumExp.Action.Equals(PredefinedSymbols.Sum))
            {
                Dictionary<string, int> coefficients = new Dictionary<string, int>();
                foreach (var term in sumExp.Args)
                {
                    if (term is StringSymbol stringSymbol)
                    {
                        coefficients[stringSymbol.Name] = 1;
                    }

                    if (term is Expression powExp && powExp.Action.Equals(PredefinedSymbols.Pow))
                    {
                        coefficients[((StringSymbol) powExp.Args[0]).Name] = 1;
                    }

                    if (term is Expression mulExp && mulExp.Action.Equals(PredefinedSymbols.Mul))
                    {
                        foreach (var mulExpArg in mulExp.Args)
                        {
                            if (mulExpArg is StringSymbol stringSymbol1)
                            {
                                coefficients[stringSymbol1.Name] = 1;
                            }

                            if (mulExpArg is Expression powExp1 && powExp1.Action.Equals(PredefinedSymbols.Pow))
                            {
                                coefficients[((StringSymbol) powExp1.Args[0]).Name] = 1;
                            }
                        }
                    }
                }

                if (coefficients.Count > 0)
                {
                    return new Expression(PredefinedSymbols.L,
                        coefficients.Keys.Select(x => new StringSymbol(x)).ToArray());
                }
            }

            throw new Exception("Argument is not polynomial");
        }

        public static Symbol EvaluateStringSymbol(StringSymbol stringSymbol, Scope context)
        {
            return Substitute(stringSymbol, context);
        }

        public static Symbol EvaluateConstant(Constant constant, Scope context)
        {
            return constant;
        }

        public static Symbol SetAttribute(Expression exp, Scope context)
        {
            context.AttributeDictionary.Add(exp.Args[0].ToString(), exp.Args[1]);
            return Ok;
        }
    }
}