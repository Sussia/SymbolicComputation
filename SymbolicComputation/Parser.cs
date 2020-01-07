using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using SymbolicComputation.Model;

namespace SymbolicComputation
{
    public static class Parser
    {
        public static object ParseInput(string jsonInput)
        {
            dynamic a = JsonConvert.DeserializeObject<dynamic>(jsonInput);
            // Check if it is an Expression
            try
            {
                string temp = a.Action.ToString();
            }
            catch (RuntimeBinderException e)
            {
                try
                {
                    return new Constant(Convert.ToDecimal(a.Value.ToString()));
                }
                catch (RuntimeBinderException ex)
                {
                    return new StringSymbol(a.Name.ToString());
                }
            }

            //Console.WriteLine(a.Action.Name.ToString());
            Symbol action = new StringSymbol(a.Action.Name.ToString());
            List<Symbol> argumentList = new List<Symbol>(); 
            for (int i = 0; i < Int32.MaxValue; i++)
            {
                try
                {
                    argumentList.Add(ParseInput(a.Args[i].ToString()));
                }
                catch (ArgumentOutOfRangeException e)
                {
                    break;
                }
            }


            return new Expression(action, argumentList.ToArray());
        }
    }
}