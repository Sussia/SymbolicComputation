using OxyPlot;
using OxyPlot.Series;
using SymbolicComputationModel;
using System;
using SymbolicComputationModel.Model;


namespace SymbolicComputationPlots
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            this.MyModel = new PlotModel {Title = "Example 1"};

            Symbol L = new StringSymbol("L");
            Expression exp = ExpressionHolder.expression;

            if (exp.Action.ToString() == "L")
            {
                Symbol[] lines = exp.Args;
                foreach (var brokenLine in lines)
                {
                    LineSeries newLine = new LineSeries();
                    var a = (Expression) brokenLine;
                    foreach (Expression point in ((Expression) brokenLine).Args)
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