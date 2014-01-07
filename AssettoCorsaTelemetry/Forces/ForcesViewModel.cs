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

        public int CanvasWidth { get { return 300; } }
        public int CanvasHeight { get { return 300; } }

        public ForcesViewModel()
        {
            AccelerationMap = new Canvas();
        }

        public void DrawAccelerationMap(List<float> xPositions, List<float> yPositions)
        {
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

            float ratioX = (CanvasWidth / 2) / maxHorizontalRounded;
            float ratioY = (CanvasHeight / 2) / maxVerticalRounded;

            float ratio = Math.Max(ratioX, ratioY);

            Line xAxis = new Line();
            xAxis.X1 = 0;
            xAxis.X2 = CanvasWidth;
            xAxis.Y1 = xAxis.Y2 = CanvasHeight / 2;
            xAxis.Fill = Brushes.White;
            xAxis.Stroke = Brushes.White;
            xAxis.StrokeThickness = 0.4;

            Line yAxis = new Line();
            yAxis.X1 = yAxis.X2 = CanvasWidth / 2;
            yAxis.Y1 = 0;
            yAxis.Y2 = CanvasHeight;
            yAxis.Fill = Brushes.White;
            yAxis.Stroke = Brushes.White;
            yAxis.StrokeThickness = 0.4;

            int counter = 1;
            for (double i = CanvasWidth / 8.0; i < CanvasWidth / 2.0; i += CanvasWidth / 8.0)
            {
                Ellipse e = new Ellipse();
                e.Height = (8 - 2 * counter) / 8.0 * CanvasHeight;
                Canvas.SetTop(e, counter * CanvasHeight / 8);
                e.Width = (8 - 2 * counter) / 8.0 * CanvasWidth;
                Canvas.SetLeft(e, counter * CanvasWidth / 8);
                e.Fill = Brushes.Transparent;
                e.Stroke = Brushes.Maroon;
                e.StrokeThickness = 0.35f;
                AccelerationMap.Children.Add(e);
                counter++;
            }

            counter = 0;
            for (int i = 0; i <= CanvasWidth; i += CanvasWidth / 8)
            {
                Line newLine = new Line();
                newLine.X1 = newLine.X2 = i;
                newLine.Y1 = CanvasHeight / 2 - 6;
                newLine.Y2 = CanvasHeight / 2 + 6;
                newLine.Fill = Brushes.White;
                newLine.Stroke = Brushes.White;
                newLine.StrokeThickness = 0.4;
                AccelerationMap.Children.Add(newLine);

                TextBlock tb = new TextBlock();
                tb.Text = ((-4.0 + counter) / 4.0 * maxHorizontalRounded).ToString();
                Canvas.SetTop(tb, CanvasHeight / 2 + 8);
                Canvas.SetLeft(tb, i);
                tb.Foreground = Brushes.Gray;
                tb.FontSize = 7;
                AccelerationMap.Children.Add(tb);
                counter++;
            }

            counter = 0;
            for (int i = 0; i <= CanvasHeight; i += CanvasHeight / 8)
            {
                Line newLine = new Line();
                newLine.Y1 = newLine.Y2 = i;
                newLine.X1 = CanvasWidth / 2 - 6;
                newLine.X2 = CanvasWidth / 2 + 6;
                newLine.Fill = Brushes.White;
                newLine.Stroke = Brushes.White;
                newLine.StrokeThickness = 0.4;
                AccelerationMap.Children.Add(newLine);

                TextBlock tb = new TextBlock();
                tb.Text = ((-4.0 + counter) / 4.0 * maxVerticalRounded).ToString();
                Canvas.SetLeft(tb, CanvasWidth / 2 + 8);
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

                Canvas.SetTop(e, CanvasHeight / 2 - yPositions[i] * ratio);
                Canvas.SetLeft(e, CanvasWidth / 2 - xPositions[i] * ratio);

                AccelerationMap.Children.Add(e);
            }
        }
    }
}
