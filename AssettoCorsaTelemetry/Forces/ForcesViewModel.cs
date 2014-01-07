using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AssettoCorsaTelemetry.Forces
{
    public class ForcesViewModel : BaseViewModel
    {
        private Canvas _accelerationMap;
        public Canvas AccelerationMap { get { return _accelerationMap; } set { SetProperty(ref _accelerationMap, value); } }


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
                DrawAccelerationMap(_cachedXPositions, _cachedYPositions);
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
                DrawAccelerationMap(_cachedXPositions, _cachedYPositions);
            }
        }

        public ForcesViewModel()
        {
            AccelerationMap = new Canvas();
        }

        List<float> _cachedXPositions;
        List<float> _cachedYPositions;

        private int ActualWidth { get { return CanvasWidth - 20; } }

        private int ActualHeight { get { return CanvasHeight - 40; } }

        public void DrawAccelerationMap(List<float> xPositions, List<float> yPositions)
        {
            _cachedXPositions = xPositions;
            _cachedYPositions = yPositions;

            AccelerationMap.Children.Clear();
            if (xPositions.Count == 0)
            {
                return;
            }
            float minX = xPositions.Min();
            float minY = yPositions.Min();
            float maxX = xPositions.Max();
            float maxY = xPositions.Max();

            float maxHorizontal = Math.Max(Math.Abs(minX), maxX);
            float maxVertical = Math.Max(Math.Abs(minY), maxY);

            int maxHorizontalRounded = (int)(maxHorizontal + 1.0f);
            int maxVerticalRounded = (int)(maxVertical + 1.0f);

            float ratioX = (ActualWidth / 2) / maxHorizontalRounded;
            float ratioY = (ActualHeight / 2) / maxVerticalRounded;

            float ratio = Math.Max(ratioX, ratioY);

            Line xAxis = new Line();
            xAxis.X1 = 0;
            xAxis.X2 = ActualWidth;
            xAxis.Y1 = xAxis.Y2 = ActualHeight / 2;
            xAxis.Fill = Brushes.White;
            xAxis.Stroke = Brushes.White;
            xAxis.StrokeThickness = 0.4;

            Line yAxis = new Line();
            yAxis.X1 = yAxis.X2 = ActualWidth / 2;
            yAxis.Y1 = 0;
            yAxis.Y2 = ActualHeight;
            yAxis.Fill = Brushes.White;
            yAxis.Stroke = Brushes.White;
            yAxis.StrokeThickness = 0.4;

            int counter = 1;
            for (double i = ActualWidth / 8.0; i < ActualWidth / 2.0; i += ActualWidth / 8.0)
            {
                Ellipse e = new Ellipse();
                e.Height = (8 - 2 * counter) / 8.0 * ActualHeight;
                Canvas.SetTop(e, counter * ActualHeight / 8);
                e.Width = (8 - 2 * counter) / 8.0 * ActualWidth;
                Canvas.SetLeft(e, counter * ActualWidth / 8);
                e.Fill = Brushes.Transparent;
                e.Stroke = Brushes.Maroon;
                e.StrokeThickness = 0.35f;
                AccelerationMap.Children.Add(e);
                counter++;
            }

            counter = 0;
            for (int i = 0; i <= ActualWidth; i += ActualWidth / 8)
            {
                Line newLine = new Line();
                newLine.X1 = newLine.X2 = i;
                newLine.Y1 = ActualHeight / 2 - 6;
                newLine.Y2 = ActualHeight / 2 + 6;
                newLine.Fill = Brushes.White;
                newLine.Stroke = Brushes.White;
                newLine.StrokeThickness = 0.4;
                AccelerationMap.Children.Add(newLine);

                TextBlock tb = new TextBlock();
                tb.Text = ((-4.0 + counter) / 4.0 * maxHorizontalRounded).ToString();
                Canvas.SetTop(tb, ActualHeight / 2 + 8);
                Canvas.SetLeft(tb, i);
                tb.Foreground = Brushes.Gray;
                tb.FontSize = 7;
                AccelerationMap.Children.Add(tb);
                counter++;
            }

            counter = 0;
            for (int i = 0; i <= ActualHeight; i += ActualHeight / 8)
            {
                Line newLine = new Line();
                newLine.Y1 = newLine.Y2 = i;
                newLine.X1 = ActualWidth / 2 - 6;
                newLine.X2 = ActualWidth / 2 + 6;
                newLine.Fill = Brushes.White;
                newLine.Stroke = Brushes.White;
                newLine.StrokeThickness = 0.4;
                AccelerationMap.Children.Add(newLine);

                TextBlock tb = new TextBlock();
                tb.Text = ((-4.0 + counter) / 4.0 * maxVerticalRounded).ToString();
                Canvas.SetLeft(tb, ActualWidth / 2 + 8);
                Canvas.SetTop(tb, i);
                tb.Foreground = Brushes.Gray;
                tb.FontSize = 7;
                AccelerationMap.Children.Add(tb);
                counter++;
            }

            AccelerationMap.Children.Add(xAxis);
            AccelerationMap.Children.Add(yAxis);

            for (int i = 0; i < xPositions.Count; i++)
            {
                Ellipse e = new Ellipse();
                e.Height = 2;
                e.Width = 2;
                e.Fill = Brushes.Blue;

                Canvas.SetTop(e, ActualHeight / 2 - yPositions[i] * ratioY);
                Canvas.SetLeft(e, ActualWidth / 2 - xPositions[i] * ratioX);

                AccelerationMap.Children.Add(e);
            }
        }
    }
}
