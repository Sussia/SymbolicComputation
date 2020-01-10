using System;
using System.Collections.Generic;
using System.Text;
using OxyPlot;
using OxyPlot.Series;
using SymbolicComputation;
using SymbolicComputation.Model;

namespace SymbolicComputationPlots
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            this.MyModel = new PlotModel { Title = "Example 1" };

            Symbol L = new StringSymbol("L");

            Expression exp = L[L[L[1, 2], L[3, 4],L[4,-1]], L[L[4, 1], L[6, 3]]];

            if (exp.Action.ToString() == "L")
            {
                Symbol[] lines = exp.Args;
                foreach (var brokenLine in lines)
                {
                    LineSeries newLine = new LineSeries(); 
                    foreach (Expression point in ((Expression)brokenLine).Args)
                    {
                        newLine.Points.Add(new DataPoint(Convert.ToDouble(point.Args[0].ToString()),
                            Convert.ToDouble(point.Args[1].ToString())));
                    }
                    this.MyModel.Series.Add(newLine);

                }
            }
        }

        public PlotModel MyModel { get; private set; }
    }
}