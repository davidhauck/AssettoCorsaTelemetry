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
        public int CanvasWidth { get { return _canvasWidth; } set { SetProperty(ref _canvasWidth, value); } }

        private int _canvasHeight = 300;
        public int CanvasHeight { get { return _canvasHeight; } set { SetProperty(ref _canvasHeight, value); } }

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

        public void DrawTrack(List<float> xcoordinates, List<float> ycoordinates)
        {
            TrackCanvas.Children.Clear();

            float minX = xcoordinates.Min();
            float minY = ycoordinates.Min();
            float maxX = xcoordinates.Max();
            float maxY = ycoordinates.Max();
            float xDist = maxX - minX;
            float yDist = maxY - minY;

            double scalar = Math.Sqrt(25000.0f / (xDist * yDist));
            CanvasWidth = Math.Max((int)(xDist * scalar), 50);
            CanvasHeight = Math.Max((int)(yDist * scalar), 50);

            float xRatio = CanvasWidth / xDist;
            float yRatio = CanvasHeight / yDist;
            float ratio = Math.Min(xRatio, yRatio);

            var path = new Path();
            var geometry = new PathGeometry();
            var figure = new PathFigure();

            figure.StartPoint = new Point((xcoordinates[0] - minX) * ratio, (ycoordinates[0] - minY) * ratio);
            for (int i = 0; i < xcoordinates.Count; i++)
            {
                figure.Segments.Add(new LineSegment() { Point = new Point((xcoordinates[i] - minX) * ratio, (ycoordinates[i] - minY) * ratio)});
            }

            geometry.Figures.Add(figure);
            path.Data = geometry;
            path.Stroke = new SolidColorBrush(Colors.LightBlue);
            path.Width = CanvasWidth;
            path.Height = CanvasHeight;

            Ellipse ellipse = new Ellipse() { Width = ellipseSize, Height = ellipseSize };
            ellipse.Fill = Brushes.Black;
            Canvas.SetTop(ellipse, (ycoordinates[ycoordinates.Count - 1] - minY) * ratio - ellipseSize / 2);
            Canvas.SetLeft(ellipse, (xcoordinates[xcoordinates.Count - 1] - minX) * ratio - ellipseSize / 2);

            TrackCanvas.Children.Add(path);
            TrackCanvas.Children.Add(ellipse);
        }
    }
}
