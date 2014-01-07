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

        //public void DrawTrack(List<float> xcoordinates, List<float> ycoordinates, List<int> isInTurns)
        //{
        //    TrackCanvas.Children.Clear();

        //    float minX = xcoordinates.Min();
        //    float minY = ycoordinates.Min();
        //    float maxX = xcoordinates.Max();
        //    float maxY = ycoordinates.Max();
        //    float xDist = maxX - minX;
        //    float yDist = maxY - minY;

        //    double scalar = Math.Sqrt(25000.0f / (xDist * yDist));
        //    CanvasWidth = Math.Max((int)(xDist * scalar), 50);
        //    CanvasHeight = Math.Max((int)(yDist * scalar), 50);

        //    float xRatio = CanvasWidth / xDist;
        //    float yRatio = CanvasHeight / yDist;
        //    float ratio = Math.Min(xRatio, yRatio);

        //    var path = new Path();
        //    var geometry = new PathGeometry();
        //    var figure = new PathFigure();

        //    int isInTurn = 0;

        //    figure.StartPoint = new Point((xcoordinates[0] - minX) * ratio, (ycoordinates[0] - minY) * ratio);
        //    for (int i = 0; i < xcoordinates.Count; i++)
        //    {
        //        figure.Segments.Add(new LineSegment() { Point = new Point((xcoordinates[i] - minX) * ratio, (ycoordinates[i] - minY) * ratio)});
        //        if (isInTurn != isInTurns[i])
        //        {
        //            geometry.Figures.Add(figure);
        //            path.Data = geometry;
        //            isInTurn = isInTurns[i];
        //            if (isInTurn == 0)
        //                path.Stroke = new SolidColorBrush(Colors.LightBlue);
        //            else if (isInTurn == 1)
        //                path.Stroke = Brushes.Red;
        //            else if (isInTurn == -1)
        //                path.Stroke = Brushes.Blue;
        //            else
        //                path.Stroke = Brushes.Black;
        //            path.Width = CanvasWidth;
        //            path.Height = CanvasHeight;
        //            TrackCanvas.Children.Add(path);

        //            path = new Path();
        //            geometry = new PathGeometry();
        //            figure = new PathFigure();

        //            figure.StartPoint = new Point((xcoordinates[i] - minX) * ratio, (ycoordinates[i] - minY) * ratio);
        //        }
        //    }

        //    geometry.Figures.Add(figure);
        //    path.Data = geometry;
        //    if (isInTurn == 0)
        //        path.Stroke = new SolidColorBrush(Colors.LightBlue);
        //    else if (isInTurn == 1)
        //        path.Stroke = Brushes.Red;
        //    else if (isInTurn == -1)
        //        path.Stroke = Brushes.Blue;
        //    else
        //        path.Stroke = Brushes.Black;
        //    path.Width = CanvasWidth;
        //    path.Height = CanvasHeight;
        //    TrackCanvas.Children.Add(path);
            
            

        //    Ellipse ellipse = new Ellipse() { Width = ellipseSize, Height = ellipseSize };
        //    ellipse.Fill = Brushes.Black;
        //    Canvas.SetTop(ellipse, (ycoordinates[ycoordinates.Count - 1] - minY) * ratio - ellipseSize / 2);
        //    Canvas.SetLeft(ellipse, (xcoordinates[xcoordinates.Count - 1] - minX) * ratio - ellipseSize / 2);

        //    TrackCanvas.Children.Add(ellipse);
        //}

        public void DrawTrack(List<float> xcoordinates, List<float> ycoordinates, List<int> isInTurns)
        {
                TrackCanvas.Children.Clear();

                float minX = xcoordinates.Min();
                float minY = ycoordinates.Min();
                float maxX = xcoordinates.Max();
                float maxY = ycoordinates.Max();
                float xDist = maxX - minX;
                float yDist = maxY - minY;

            if (xDist > 1.0 && yDist > 1.0)
            {

                double scalar = Math.Sqrt(25000.0f / (xDist * yDist));
                CanvasWidth = Math.Max((int)(xDist * scalar), 50);
                CanvasHeight = Math.Max((int)(yDist * scalar), 50);

                float xRatio = CanvasWidth / xDist;
                float yRatio = CanvasHeight / yDist;
                float ratio = Math.Min(xRatio, yRatio);

                float previousX = (xcoordinates[0] - minX) * ratio;
                float previousY = (ycoordinates[0] - minY) * ratio;
                for (int i = 0; i < xcoordinates.Count; i++)
                {
                    Line l = new Line();
                    l.X1 = previousX;
                    l.Y1 = previousY;
                    l.X2 = previousX = (xcoordinates[i] - minX) * ratio;
                    l.Y2 = previousY = (ycoordinates[i] - minY) * ratio;
                    if (isInTurns[i] == 1)
                        l.Stroke = Brushes.Red;
                    else if (isInTurns[i] == -1)
                        l.Stroke = Brushes.Blue;
                    else
                        l.Stroke = Brushes.White;
                    TrackCanvas.Children.Add(l);
                }



                Ellipse ellipse = new Ellipse() { Width = ellipseSize, Height = ellipseSize };
                ellipse.Fill = Brushes.Black;
                Canvas.SetTop(ellipse, (ycoordinates[ycoordinates.Count - 1] - minY) * ratio - ellipseSize / 2);
                Canvas.SetLeft(ellipse, (xcoordinates[xcoordinates.Count - 1] - minX) * ratio - ellipseSize / 2);

                TrackCanvas.Children.Add(ellipse);
            }
        }

        public List<int> FindIsInTurns(List<List<float>> accelerations)
        {
            List<int> isInTurns = new List<int>();
            List<float> xAcc = accelerations[0];
            List<float> yAcc = accelerations[1]; 

            int isTurning = 0;
            int previousTurn = 0;
            int previousTurnCount = 0;
            int maxTurnCounter = 5;
            int turnCounter = maxTurnCounter;

            for (int i = 0; i < xAcc.Count; i++)
            {
                previousTurnCount++;
                if (isTurning == 1)
                {
                    if (xAcc[i] > 0.5 || yAcc[i] < -0.5)
                    {
                        isInTurns.Add(1);
                        turnCounter = maxTurnCounter;
                    }
                    else if (turnCounter > 0)
                    {
                        turnCounter--;
                        isInTurns.Add(1);
                    }
                    else
                    {
                        isTurning = 0;
                        isInTurns.Add(0);
                        previousTurn = 1;
                        previousTurnCount = 0;
                    }
                }
                else if (isTurning == -1)
                {
                    if (xAcc[i] < -0.5 || yAcc[i] < -0.5)
                    {
                        isInTurns.Add(-1);
                        turnCounter = maxTurnCounter;
                    }
                    else if (turnCounter > 0)
                    {
                        turnCounter--;
                        isInTurns.Add(-1);
                    }
                    else
                    {
                        isTurning = 0;
                        isInTurns.Add(0);
                        previousTurn = -1;
                        previousTurnCount = 0;
                    }
                }
                else if (isTurning == 0)
                {
                    if (!(Math.Abs(xAcc[i]) > 0.5 || yAcc[i] < -0.5))
                    {
                        isInTurns.Add(0);
                        turnCounter = maxTurnCounter;
                    }
                    else if (turnCounter > 0)
                    {
                        turnCounter--;
                        isInTurns.Add(0);
                    }
                    else
                    {
                        if (xAcc[i] > 0.5)
                        {
                            turnCounter = maxTurnCounter;
                            isTurning = 1;
                            isInTurns.Add(1);
                            if (previousTurn == 1 && previousTurnCount < 10)
                            {
                                for (int j = 2; j <= previousTurnCount + 1; j++)
                                {
                                    isInTurns[isInTurns.Count - j] = 1;
                                }
                            }
                        }
                        else if (xAcc[i] < -0.5)
                        {
                            turnCounter = maxTurnCounter;
                            isTurning = -1;
                            isInTurns.Add(-1);
                            if (previousTurn == -1 && previousTurnCount < 10)
                            {
                                for (int j = 2; j <= previousTurnCount + 1; j++)
                                {
                                    isInTurns[isInTurns.Count - j] = -1;
                                }
                            }
                        }
                        else
                        {
                            isInTurns.Add(0);
                        }
                    }
                }
            }
            return isInTurns;
        }
   
    }
}
