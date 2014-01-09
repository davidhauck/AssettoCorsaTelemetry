using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AssettoCorsaTelemetry.Plot
{
    public class PlotViewModel : BaseViewModel
    {
        public PlotModel Model { get; private set; }

        private LineSeries _myLineSeries;
        public LineSeries MyLineSeries
        {
            get
            {
                return _myLineSeries;
            }
            set
            {
                SetProperty(ref _myLineSeries, value);
            }
        }

        List<Brush> Colors = new List<Brush>() { Brushes.Black, Brushes.Blue, Brushes.Red, Brushes.Green, Brushes.White, Brushes.Purple };

        public void Draw(List<List<float>> yCoords, List<float> timeLeft, List<string> names, float min, float max)
        {
            if (yCoords.Count == 0)
            {
                return;
            }

            float startTime = timeLeft[0];

            var tmp = new PlotModel(names[0]);
            

            for (int i = 0; i < yCoords.Count; i++)
            {
                LineSeries series = new LineSeries(names[i]);
                for (int j = 0; j < yCoords[i].Count; j++)
                {
                    series.Points.Add(new DataPoint(startTime - timeLeft[j], yCoords[i][j]));
                }
                tmp.Series.Add(series);
            }

            this.Model = tmp;
        }
    }
}
