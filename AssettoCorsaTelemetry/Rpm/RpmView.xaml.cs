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

namespace AssettoCorsaTelemetry.Rpm
{
    /// <summary>
    /// Interaction logic for RpmView.xaml
    /// </summary>
    public partial class RpmView : Window
    {
        public RpmViewModel Rpms { get { return DataContext as RpmViewModel; } }

        public RpmView()
        {
            InitializeComponent();
            DataContext = new RpmViewModel();
        }
    }
}
