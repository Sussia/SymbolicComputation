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
        public Expression PlotExpression { get; private set; }
        public string Json { get; set; }

        public MainWindow(Expression expression, string jsonInput, decimal width, decimal height)
        {
            InitializeComponent();
            DataContext = this;

            Width = (double) width < MinWidth ? MinWidth : (double) width;
            Height = (double) height < MinHeight ? MinHeight : (double) height;
            Plot.Model = new PlotModel();
            PlotExpression = expression;
            Json = jsonInput;
            DrawPlot(PlotExpression);
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

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Expression exp = (Expression) Parser.ParseInput(Json, new Scope());
            PlotExpression = (Expression) exp.Args[0];
            DrawPlot(PlotExpression);
        }
    }
}