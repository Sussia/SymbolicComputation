using System;
using System.Windows;
using OxyPlot;
using OxyPlot.Series;
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
        public MainWindow(Expression expression)
        {

			InitializeComponent();

			Plot.Model = new PlotModel
			{
				Title = "Plot"
			};
			PlotExpression = expression;
			DrawPlot(PlotExpression);
		}

        public void DrawPlot(Expression expression)
        {
	        if (expression.Action.Equals(L))
	        {
		        Symbol[] lines = expression.Args;
		        foreach (var brokenLine in lines)
		        {
			        LineSeries newLine = new LineSeries();
			        foreach (Expression point in ((Expression)brokenLine).Args)
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
