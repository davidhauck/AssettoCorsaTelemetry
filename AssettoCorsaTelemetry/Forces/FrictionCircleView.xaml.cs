using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AssettoCorsaTelemetry.Forces
{
    /// <summary>
    /// Interaction logic for FrictionCircleView.xaml
    /// </summary>
    public partial class FrictionCircleView : Window
    {
        public ForcesViewModel Forces { get { return DataContext as ForcesViewModel; } }

        public FrictionCircleView()
        {
            InitializeComponent();
            DataContext = new ForcesViewModel();
        }
    }
}
