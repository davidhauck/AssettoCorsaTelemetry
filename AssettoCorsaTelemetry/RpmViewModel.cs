using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AssettoCorsaTelemetry
{
    class RpmViewModel : BaseViewModel
    {
        public int CanvasHeight { get { return 350; } }
        public int CanvasWidth { get { return 600; } }

        private Canvas _rpmCanvas;
        public Canvas RpmCanvas
        {
            get
            {
                return _rpmCanvas;
            }
            set
            {
                SetProperty(ref _rpmCanvas, value);
            }
        }

        public RpmViewModel()
        {
            RpmCanvas = new Canvas();
        }

        public void DrawRpmCanvas(List<int> rpms, List<float> speeds)
        {
            RpmCanvas.Children.Clear();
            if (rpms.Count == 0)
            {
                return;
            }
            int maxRpm = rpms.Max();
            float maxSpeed = speeds.Max();

            float yRatio = CanvasHeight / (float)maxRpm;
            float xRatio = CanvasWidth / maxSpeed;

            for (int i = 0; i < rpms.Count; i++)
            {
                Ellipse e = new Ellipse();
                e.Height = 2;
                e.Width = 2;
                e.Fill = Brushes.Blue;

                Canvas.SetTop(e, CanvasHeight - rpms[i] * yRatio);
                Canvas.SetLeft(e, speeds[i] * xRatio);

                RpmCanvas.Children.Add(e);
            }
        }
    }
}
