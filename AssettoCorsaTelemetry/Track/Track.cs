using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AssettoCorsaTelemetry.Track
{
    public class TrackViewModel : BaseViewModel
    {
        private const int ellipseSize = 10;

        private int _canvasWidth = 300;
        public int CanvasWidth 
        {
            get
            {
                return _canvasWidth;
            }
            set
            {
                SetProperty(ref _canvasWidth, value);
                DrawTrack(_cachedXCoords, _cachedYCoords, _cachedTurnSections);
            }
        }

        private int _canvasHeight = 300;
        public int CanvasHeight
        {
            get
            {
                return _canvasHeight;
            }
            set
            {
                SetProperty(ref _canvasHeight, value);
                DrawTrack(_cachedXCoords, _cachedYCoords, _cachedTurnSections);
            }
        }

        private Canvas _trackCanvas;
        public Canvas TrackCanvas
        {
            get
            {
                return _trackCanvas;
            }
            set
            {
                SetProperty(ref _trackCanvas, value);
            }
        }

        public TrackViewModel()
        {
            TrackCanvas = new Canvas();
        }

        List<float> _cachedXCoords;
        List<float> _cachedYCoords;
        List<int> _cachedTurnSections;

        public void DrawTrack(List<float> xcoordinates, List<float> ycoordinates, List<int> turnSections)
        {
            _cachedXCoords = xcoordinates; //not actually "caching" as its not a deep copy
            _cachedYCoords = ycoordinates;
            _cachedTurnSections = turnSections;

            if (xcoordinates.Count == 0)
            {
                return;
            }

            TrackCanvas.Children.Clear();

            float minX = xcoordinates.Min();
            float minY = ycoordinates.Min();
            float maxX = xcoordinates.Max();
            float maxY = ycoordinates.Max();
            float xDist = maxX - minX;
            float yDist = maxY - minY;

            if (xDist > 1.0 && yDist > 1.0)
            {

                //double scalar = Math.Sqrt(25000.0f / (xDist * yDist));
                //CanvasWidth = Math.Max((int)(xDist * scalar), 50);
                //CanvasHeight = Math.Max((int)(yDist * scalar), 50);

                float xRatio = (CanvasWidth - 20) / xDist;
                float yRatio = (CanvasHeight - 50) / yDist;
                float ratio = Math.Min(xRatio, yRatio);

                int previousSection = 0;
                float previousX = (xcoordinates[0] - minX) * ratio;
                float previousY = (ycoordinates[0] - minY) * ratio;
                Brush currentBrush = Brushes.White;
                for (int i = 0; i < xcoordinates.Count; i++)
                {
                    Line l = new Line();
                    l.X1 = previousX;
                    l.Y1 = previousY;
                    l.X2 = previousX = (xcoordinates[i] - minX) * ratio;
                    l.Y2 = previousY = (ycoordinates[i] - minY) * ratio;
                    if (turnSections[i] != previousSection)
                    {
                        currentBrush = (currentBrush == Brushes.Blue) ? Brushes.White : Brushes.Blue;
                    }
                    l.Stroke = currentBrush;
                    TrackCanvas.Children.Add(l);
                    previousSection = turnSections[i];
                }



                Ellipse ellipse = new Ellipse() { Width = ellipseSize, Height = ellipseSize };
                ellipse.Fill = Brushes.Black;
                Canvas.SetTop(ellipse, (ycoordinates[ycoordinates.Count - 1] - minY) * ratio - ellipseSize / 2);
                Canvas.SetLeft(ellipse, (xcoordinates[xcoordinates.Count - 1] - minX) * ratio - ellipseSize / 2);

                TrackCanvas.Children.Add(ellipse);
            }
        }


    }
}
