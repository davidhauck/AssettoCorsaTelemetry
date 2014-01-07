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

        public void DrawTrack(List<float> xcoordinates, List<float> ycoordinates, List<int> turnSections)
        {
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

                double scalar = Math.Sqrt(25000.0f / (xDist * yDist));
                CanvasWidth = Math.Max((int)(xDist * scalar), 50);
                CanvasHeight = Math.Max((int)(yDist * scalar), 50);

                float xRatio = CanvasWidth / xDist;
                float yRatio = CanvasHeight / yDist;
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
                            if (previousTurn == 1 && previousTurnCount < 13)
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
                            if (previousTurn == -1 && previousTurnCount < 13)
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

        internal List<int> FindTurnsBasedOnLap(List<int> completedLaps, List<List<float>> accelerations, List<List<float>> carCoords, int lap, out int numSections) //only works for lap 0 right now
        {
            if (lap != 0)
            {
                throw new Exception("Only use the first lap");
            }
            List<List<float>> lapAccelerations = new List<List<float>>();
            lapAccelerations.Add(new List<float>());
            lapAccelerations.Add(new List<float>());
            lapAccelerations.Add(new List<float>());
            for (int i = 0; i < completedLaps.Count; i++)
            {
                if (completedLaps[i] == lap)
                {
                    lapAccelerations[0].Add(accelerations[0][i]);
                    lapAccelerations[1].Add(accelerations[2][i]);
                }
            }
            List<int> turns = FindIsInTurns(lapAccelerations);
            List<int> sections = CalculateTurnSections(turns);

            List<float> xs = new List<float>();
            List<float> ys = new List<float>();
            
            int previousSection = 0;
            for (int i = 0; i < sections.Count; i++)
            {
                if (sections[i] != previousSection)
                {
                    xs.Add(carCoords[0][i]);
                    ys.Add(carCoords[2][i]);
                    previousSection = sections[i];
                }
            }

            List<List<int>> laps = new List<List<int>>();
            int j = 0;
            List<int> newLap = new List<int>();
            while (true)
            {
                newLap.Add(j);
                j++;
                if (j >= completedLaps.Count)
                {
                    break;
                }
                if (completedLaps[j] > completedLaps[j - 1])
                {
                    laps.Add(newLap);
                    newLap = new List<int>();
                }
            }

            List<int> sectionSplits = new List<int>();

            for (int k = 0; k < laps.Count; k++)
            {
                for (int i = 0; i < xs.Count; i++)
                {
                    float x = xs[i];
                    float y = ys[i];
                    sectionSplits.Add(FindIndexOfClosestPosition(carCoords, laps[k], x, y));
                }
            }

            sectionSplits.Add(completedLaps.Count);
            numSections = sectionSplits.Count / laps.Count;

            List<int> newSections = new List<int>();

            int currentSection = 0;
            for (int i = 0; i < numSections * laps.Count + 1; i++)
            {
                for (int k = (i == 0)? 0 : sectionSplits[i-1]; k < sectionSplits[i]; k++)
                {
                    newSections.Add(currentSection);
                }
                currentSection++;
                currentSection %= numSections;
            }

            return newSections;
        }

        private int FindIndexOfClosestPosition(List<List<float>> coords, List<int> list, float x1, float y1)
        {
            float shortestDistance = float.MaxValue;
            int shortestIndex = -1;
            for (int i = list[0]; i < list[list.Count - 1]; i++)
            {
                float x2 = coords[0][i];
                float y2 = coords[2][i];
                float distance = (float)Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    shortestIndex = i;
                }
            }
            return shortestIndex;
        }

        private List<int> CalculateTurnSections(List<int> turns)
        {
            List<int> result = new List<int>();

            int section = 0;
            int previousTurn = 0;
            for (int i = 0; i < turns.Count; i++)
            {
                if (turns[i] != previousTurn)
                {
                    section++;
                }
                result.Add(section);
                previousTurn = turns[i];
            }

            return result;
        }
    }
}
