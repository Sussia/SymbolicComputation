using System;
using System.Windows;
using OxyPlot;
using OxyPlot.Series;
using SymbolicComputationLib;
using SymbolicComputationLib.Model;
using Expression = SymbolicComputationLib.Model.Expression;
using static SymbolicComputationLib.PredefinedSymbols;

namespace SymbolicComputationPlots
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow(Expression expression, decimal width, decimal height)
        {
            InitializeComponent();
            DataContext = this;

            Width = (double)width < MinWidth ? MinWidth : (double)width;
            Height = (double)height < MinHeight ? MinHeight : (double)height;
            Plot.Model = new PlotModel();
            DrawPlot(expression);
        }

        public MainWindow()
        {
            InitializeComponent();
            Plot.Model = new PlotModel();
            int a = 5;
            Plot.Model.Series.Add(new FunctionSeries(x =>a++*Math.Cos(x), y => a * Math.Sin(y), -4 * Math.PI, 0, 0.001));
        }

        public void DrawPlot(Expression expression)
        {
            Plot.Model.Series.Clear();
            if (expression.Action.Equals(L))
            {
                Symbol[] lines = expression.Args;
                foreach (var brokenLine in lines)
                {
                    LineSeries newLine = new LineSeries();
                    foreach (Expression point in ((Expression) brokenLine).Args)
                    {
                        newLine.Points.Add(new DataPoint(Convert.ToDouble(point.Args[0].ToString()),
                            Convert.ToDouble(point.Args[1].ToString())));
                    }

                    Plot.Model.Series.Add(newLine);
                }

                Plot.Model.InvalidatePlot(true);
            }
        }
    }
}