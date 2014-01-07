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
        private int _canvasWidth = 600;
        public int CanvasWidth
        {
            get
            {
                return _canvasWidth;
            }
            set
            {
                SetProperty(ref _canvasWidth, value);
                Draw(_cachedYCoords, _cachedTimeLeft, _cachedNames, _cachedMin, _cachedMax);
            }
        }

        private int _canvasHeight = 350;
        public int CanvasHeight
        {
            get
            {
                return _canvasHeight;
            }
            set
            {
                SetProperty(ref _canvasHeight, value);
                Draw(_cachedYCoords, _cachedTimeLeft, _cachedNames, _cachedMin, _cachedMax);
            }
        }

        private int _plotAreaWidth;
        public int PlotAreaWidth
        {
            get
            {
                return _plotAreaWidth;
            }
            set
            {
                SetProperty(ref _plotAreaWidth, value);
            }
        }

        private int _plotAreaHeight;
        public int PlotAreaHeight
        {
            get
            {
                return _plotAreaHeight;
            }
            set
            {
                SetProperty(ref _plotAreaHeight, value);
            }
        }


        private Canvas _plotCanvas;
        public Canvas PlotCanvas
        {
            get
            {
                return _plotCanvas;
            }
            set
            {
                SetProperty(ref _plotCanvas, value);
            }
        }

        public PlotViewModel()
        {
            PlotCanvas = new Canvas();
        }

        List<List<float>> _cachedYCoords;
        List<float> _cachedTimeLeft;
        List<string> _cachedNames;
        float _cachedMin;
        float _cachedMax;

        List<Brush> Colors = new List<Brush>() { Brushes.Black, Brushes.Blue, Brushes.Red, Brushes.Green, Brushes.White, Brushes.Purple };

        public void Draw(List<List<float>> yCoords, List<float> timeLeft, List<string> names, float min, float max)
        {
            _cachedYCoords = yCoords;
            _cachedTimeLeft = timeLeft;
            _cachedNames = names;
            _cachedMin = min;
            _cachedMax = max;

            PlotAreaHeight = CanvasHeight - 40;
            PlotAreaWidth = 2000;

            float dataMin = yCoords.Min(d => d.Min());
            float dataMax = yCoords.Max(d => d.Max());
            float dataDifference = dataMax - dataMin;
            float plotYDifference = max - min;
            float yRatio = CanvasHeight / plotYDifference;

            PlotCanvas.Children.Clear();

            float startTime = timeLeft[0];

            float timeDifference = startTime - timeLeft[timeLeft.Count - 1];
            PlotAreaWidth = (int)(timeDifference / 200);
            float xRatio = 1.0f / 200;

            for (int i = 0; i < yCoords.Count; i++)
            {
                float previousY = yCoords[i][0];
                float previousX = 0;

                for (int j = 0; j < yCoords[i].Count; j++)
                {
                    Line l = new Line();
                    l.X1 = previousX;
                    l.Y1 = previousY;
                    l.X2 = previousX = (startTime - timeLeft[j]) * xRatio;
                    l.Y2 = previousY = yCoords[i][j] * yRatio;
                    l.Stroke = Colors[i];

                    PlotCanvas.Children.Add(l);
                }
            }
        }
    }
}
