﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PolynomialFactorizator
{
    public class Monomial: ICloneable
    {
        public bool Sign;

        public int Coefficient;

        public List<Indeterminate> IndeterminatesList;

        public Monomial()
        {
            IndeterminatesList = new List<Indeterminate>();
        }

        public Monomial(bool sign, int coefficient, List<Indeterminate> indeterminates)
        {
            this.Sign = sign;
            this.Coefficient = coefficient;
            this.IndeterminatesList = indeterminates;
        }

        public override string ToString()
        {
            string indeterminateString = GetIndeterminatesString();
            string sign = !Sign ? "-" : "+";
            return $"{sign} {Coefficient}*{indeterminateString}";
        }

        public object Clone()
        { 
            var indeterminateList =  IndeterminatesList.Select(i => (Indeterminate)i.Clone()).ToList();
            return new Monomial(this.Sign, this.Coefficient, indeterminateList);
        }

        private string GetIndeterminatesString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Indeterminate indeterminate in IndeterminatesList)
            {
                sb.Append(indeterminate.ToString());
                sb.Append('*');
            }

            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        public int FindIndeterminateByChar(char symbol)
        {
            int outIndex = 0;
            for (int i = 0; i < IndeterminatesList.Count; i++)
            {
                if (IndeterminatesList[i].Symbol == symbol)
                {
                    outIndex = i;
                    break;
                }
            }

            return outIndex;
        }
    }
}