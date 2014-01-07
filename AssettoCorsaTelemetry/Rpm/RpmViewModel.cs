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
    public class RpmViewModel : BaseViewModel
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
                DrawRpmCanvas(_cachedRpms, _cachedSpeeds);
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
                DrawRpmCanvas(_cachedRpms, _cachedSpeeds);
            }
        }

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

        List<int> _cachedRpms;
        List<float> _cachedSpeeds;

        public void DrawRpmCanvas(List<int> rpms, List<float> speeds)
        {
            _cachedRpms = rpms;
            _cachedSpeeds = speeds;

            RpmCanvas.Children.Clear();
            if (rpms.Count == 0)
            {
                return;
            }
            int maxRpm = rpms.Max();
            float maxSpeed = speeds.Max();

            float yRatio = (CanvasHeight-40) / (float)maxRpm;
            float xRatio = (CanvasWidth-20) / (float)maxSpeed;

            for (int i = 0; i < rpms.Count; i++)
            {
                Ellipse e = new Ellipse();
                e.Height = 2;
                e.Width = 2;
                e.Fill = Brushes.Blue;

                Canvas.SetTop(e, (CanvasHeight-40) - rpms[i] * yRatio);
                Canvas.SetLeft(e, speeds[i] * xRatio);

                RpmCanvas.Children.Add(e);
            }
        }
    }
}
