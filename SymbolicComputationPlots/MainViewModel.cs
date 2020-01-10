using System;
using System.Collections.Generic;
using System.Text;
using OxyPlot;
using OxyPlot.Series;

namespace SymbolicComputationPlots
{
	public class MainViewModel
	{
		public MainViewModel()
		{
			this.MyModel = new PlotModel { Title = "Example 1" };
			LineSeries lineL = new LineSeries();
			lineL.Points.AddRange(new[]
			{
				new DataPoint(1,2),
				new DataPoint(2,6),
				new DataPoint(3,2),
			});
			lineL.Title = "Л";
			this.MyModel.Series.Add(lineL);

			LineSeries lineO = new LineSeries();
			lineO.Points.AddRange(new[]
			{
				new DataPoint(4,2),
				new DataPoint(4,6),
				new DataPoint(5,6),
				new DataPoint(5,2),
				new DataPoint(4,2)
			});
			lineO.Title = "O";
			this.MyModel.Series.Add(lineO);

			LineSeries lineX1 = new LineSeries();
			lineX1.Points.AddRange(new[]
			{
				new DataPoint(6,2),
				new DataPoint(7,6)
			});
			lineX1.Title = "Х1";
			this.MyModel.Series.Add(lineX1);

			LineSeries lineX2 = new LineSeries();
			lineX2.Points.AddRange(new[]
			{
				new DataPoint(6,6),
				new DataPoint(7,2)
			});
			lineX2.Title = "Х2";
			this.MyModel.Series.Add(lineX2);

			LineSeries line = new LineSeries();
			line.Points.AddRange(new[]
			{
				new DataPoint(-1,1),
				new DataPoint(9,1)
			});
			line.Title = "line";
			this.MyModel.Series.Add(line);

			LineSeries line1 = new LineSeries();
			line1.Points.AddRange(new[]
			{
				new DataPoint(-1,8),
				new DataPoint(9,8)
			});
			line1.Title = "line1";
			this.MyModel.Series.Add(line1);
		}

		public PlotModel MyModel { get; private set; }
	}
}
