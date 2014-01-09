using AcSdk.Data;
using AssettoCorsaTelemetry.Forces;
using AssettoCorsaTelemetry.Plot;
using AssettoCorsaTelemetry.Rpm;
using AssettoCorsaTelemetry.Track;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace AssettoCorsaTelemetry
{
    class MainWindowViewModel : BaseViewModel
    {
        private Visibility _racingVisibility = Visibility.Visible;
        public Visibility RacingVisibility { get { return _racingVisibility; } set { SetProperty(ref _racingVisibility, value); } }

        private Visibility _analyzingVisibility = Visibility.Collapsed;
        public Visibility AnalyzingVisibility { get { return _analyzingVisibility; } set { SetProperty(ref _analyzingVisibility, value); } }

        List<float> xCoordinates;
        List<float> yCoordinates;

        string _filepath;
        MemberInfo[] _physicsMembers;
        MemberInfo[] _graphicsMembers;

        private string _status;
        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                SetProperty(ref _status, value);
            }
        }

        private TrackView TrackView { get; set; }

        private FrictionCircleView ForcesView { get; set; }

        private RpmView RpmView { get; set; }

        private ICommand _clickCommand;
        public ICommand ClickCommand
        {
            get
            {
                if (_clickCommand == null)
                {
                    _clickCommand = new RelayCommand(p => this.OpenOldRace());
                }
                return _clickCommand;
            }
        }

        private ObservableCollection<string> _collectionLaps;
        public ObservableCollection<string> CollectionLaps
        {
            get
            {
                return _collectionLaps;
            }
            set
            {
                SetProperty(ref _collectionLaps, value);
            }
        }

        private string _selectedLap;
        public string SelectedLap
        {
            get
            {
                return _selectedLap;
            }
            set
            {
                SetProperty(ref _selectedLap, value);
                UpdateScreen();
            }
        }

        private ObservableCollection<string> _collectionSections;
        public ObservableCollection<string> CollectionSections
        {
            get
            {
                return _collectionSections;
            }
            set
            {
                SetProperty(ref _collectionSections, value);
            }
        }

        private string _selectedSection;
        public string SelectedSection
        {
            get
            {
                return _selectedSection;
            }
            set
            {
                SetProperty(ref _selectedSection, value);
                UpdateScreen();
            }
        }

        private ObservableCollection<string> _listOfPlottableDatas = new ObservableCollection<string>() { "Gas", "Brake", "Fuel", "Gear", "Rpms", "SteerAngle", "SpeedKmh", "Velocity", "AccG", "WheelSlip", "WheelLoad", "WheelsPressure", "WheelAngularSpeed", "TyreWear", "TyreDirtyLevel", "TyreCoreTemperature", "CamberRad", "SuspensionTravel", "Heading", "Pitch", "Roll", "CgHeight", "CarDamage", "NumberOfTyresOut", "Abs" };
        public ObservableCollection<string> ListOfPlottableDatas
        {
            get
            {
                return _listOfPlottableDatas;
            }
            set
            {
                SetProperty(ref _listOfPlottableDatas, value);
            }
        }

        private string _selectedData1;
        public string SelectedData1
        {
            get
            {
                return _selectedData1;
            }
            set
            {
                SetProperty(ref _selectedData1, value);
            }
        }

        private string _selectedData2;
        public string SelectedData2
        {
            get
            {
                return _selectedData2;
            }
            set
            {
                SetProperty(ref _selectedData2, value);
            }
        }

        private string _selectedData3;
        public string SelectedData3
        {
            get
            {
                return _selectedData3;
            }
            set
            {
                SetProperty(ref _selectedData3, value);
            }
        }

        private string _selectedData4;
        public string SelectedData4
        {
            get
            {
                return _selectedData4;
            }
            set
            {
                SetProperty(ref _selectedData4, value);
            }
        }

        private string _selectedData5;
        public string SelectedData5
        {
            get
            {
                return _selectedData5;
            }
            set
            {
                SetProperty(ref _selectedData5, value);
            }
        }

        private string _selectedData6;
        public string SelectedData6
        {
            get
            {
                return _selectedData6;
            }
            set
            {
                SetProperty(ref _selectedData6, value);
            }
        }

        private List<string> SelectedPlottingDatas
        {
            get
            {
                var newList = new List<string>();
                if (!string.IsNullOrWhiteSpace(SelectedData1))
                {
                    newList.Add(SelectedData1);
                }
                if (!string.IsNullOrWhiteSpace(SelectedData2))
                {
                    newList.Add(SelectedData2);
                }
                if (!string.IsNullOrWhiteSpace(SelectedData3))
                {
                    newList.Add(SelectedData3);
                }
                if (!string.IsNullOrWhiteSpace(SelectedData4))
                {
                    newList.Add(SelectedData4);
                }
                if (!string.IsNullOrWhiteSpace(SelectedData5))
                {
                    newList.Add(SelectedData5);
                }
                if (!string.IsNullOrWhiteSpace(SelectedData6))
                {
                    newList.Add(SelectedData6);
                }
                return newList;
            }
        }

        private List<string> FullSelectedPlottingData
        {
            get
            {
                return new List<string>() { SelectedData1, SelectedData2, SelectedData3, SelectedData4, SelectedData5, SelectedData6 };
            }
        }

        private ICommand _createPlotCommand;
        public ICommand CreatePlotCommand
        {
            get
            {
                if (_createPlotCommand == null)
                {
                    _createPlotCommand = new RelayCommand(p => this.CreatePlot());
                }
                return _createPlotCommand;
            }
        }
        RaceModel rm;

        public Boolean[] IsChecked1
        {
            get;
            set;
        }

        public Boolean[] IsChecked2
        {
            get;
            set;
        }
        public Boolean[] IsChecked3
        {
            get;
            set;
        }
        public Boolean[] IsChecked4
        {
            get;
            set;
        }
        public Boolean[] IsChecked5
        {
            get;
            set;
        }
        public Boolean[] IsChecked6
        {
            get;
            set;
        }

        private List<Boolean[]> IsChecked
        {
            get
            {
                return new List<Boolean[]>() { IsChecked1, IsChecked2, IsChecked3, IsChecked4, IsChecked5, IsChecked6 };
            }
        }

        private void CreatePlot()
        {
            PlotView plot = new PlotView();
            List<string> names = SelectedPlottingDatas;
            List<List<float>> plots = new List<List<float>>();
            List<string> graphNames = new List<string>();
            for (int i = 0; i < names.Count; i++)
            {
                var info = typeof(RaceModel).GetProperty(names[i]);
                var newValue = info.GetValue(rm);
                if (newValue.GetType() == typeof(List<List<float>>))
                {
                    var newListList = newValue as List<List<float>>;
                    for (int j = 0; j < newListList.Count; j++)
                    {
                        int index = FullSelectedPlottingData.IndexOf(names[i]);
                        if (IsChecked[i][j] == true)
                        {
                            plots.Add(newListList[j]);
                            graphNames.Add(info.Name);
                        }
                    }
                }
                else if (newValue.GetType() == typeof(List<float>))
                {
                    var newList = newValue as List<float>;
                    plots.Add(newList);
                    graphNames.Add(info.Name);
                }
                else if (newValue.GetType() == typeof(List<int>))
                {
                    var newListInts = newValue as List<int>;
                    var newList = new List<float>();
                    newListInts.ForEach(element => newList.Add((float)element));
                    plots.Add(newList);
                    graphNames.Add(info.Name);
                }
            }
            if (graphNames.Count == 0)
            {
                return;
            }
            plot.Plot.Draw(plots, rm.SessionTimeLeft, graphNames, 0, 0.5f);
            plot.Topmost = true;
            plot.Title = graphNames[0];
            plot.Show();
        }

        public void OpenOldRace()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = "sample";
            ofd.DefaultExt = ".csv";
            ofd.Filter = "Comma Separated Values (.csv)|*.csv";
            Nullable<bool> result = ofd.ShowDialog();
            if (result == true)
            { 
                string filename = ofd.FileName;
                RacingVisibility = Visibility.Collapsed;
                AnalyzingVisibility = Visibility.Visible;
                rm = ReadFile(filename);
                CollectionLaps.Clear();
                CollectionLaps.Add("All Laps");
                for (int i = 1; i < rm.CompletedLaps[rm.CompletedLaps.Count - 1] + 1; i++)
                {
                    CollectionLaps.Add(i.ToString());
                }
                int numberOfSections;
                rm.TurnSections = Utils.FindTurnsBasedOnLap(rm.CompletedLaps, rm.AccG, rm.CarCoordinates, 0, out numberOfSections);
                CollectionSections.Clear();
                CollectionSections.Add("All Sections");
                for (int i = 1; i <= numberOfSections; i++)
                {
                    CollectionSections.Add(i.ToString());
                }
                UpdateScreen();
            }
        }

        private void UpdateScreen()
        {
            if (rm != null)
            {
                DrawForces();
                DrawRpms();
                DrawTrack();
            }
        }

        private void DrawTrack()
        {
            List<float> xCoor = null;
            List<float> yCoor = null;
            List<int> turnSections = null;
            int selectedlap;
            int selectedsection;

            if (SelectedLap == "All Laps")
                selectedlap = -1;
            else
                selectedlap = int.Parse(SelectedLap);

            if (SelectedSection == "All Sections")
                selectedsection = -1;
            else
                selectedsection = int.Parse(SelectedSection);

            xCoor = new List<float>();
            yCoor = new List<float>();
            turnSections = new List<int>();

            for (int i = 0; i < rm.AccG[0].Count; i++)
            {
                if ((rm.CompletedLaps[i] == selectedlap - 1 || selectedlap == -1) && (rm.TurnSections[i] == selectedsection - 1 || selectedsection == -1))
                {
                    xCoor.Add(rm.CarCoordinates[0][i]);
                    yCoor.Add(rm.CarCoordinates[2][i]);
                    turnSections.Add(rm.TurnSections[i]);
                }
            }
            if (TrackView == null)
            {
                TrackView = new TrackView();
            }
            TrackView.Track.DrawTrack(xCoor, yCoor, turnSections);
            TrackView.Topmost = true;
            TrackView.Show();
        }

        private void DrawForces()
        {
            List<float> xAcc = null;
            List<float> yAcc = null;
            int selectedlap;
            int selectedsection;

            if (SelectedLap == "All Laps")
                selectedlap = -1;
            else
                selectedlap = int.Parse(SelectedLap);

            if (SelectedSection == "All Sections")
                selectedsection = -1;
            else
                selectedsection = int.Parse(SelectedSection);

            xAcc = new List<float>();
            yAcc = new List<float>();

            for (int i = 0; i < rm.AccG[0].Count; i++)
            {
                if ((rm.CompletedLaps[i] == selectedlap - 1 || selectedlap == -1) && (rm.TurnSections[i] == selectedsection - 1 || selectedsection == -1))
                {
                    xAcc.Add(rm.AccG[0][i]);
                    yAcc.Add(rm.AccG[1][i]);
                }
            }
            if (ForcesView == null)
            {
                ForcesView = new FrictionCircleView();
            }
            ForcesView.Forces.DrawAccelerationMap(xAcc, yAcc);
            ForcesView.Topmost = true;
            ForcesView.Show();
        }

        private void DrawRpms()
        {
            List<int> rpm = null;
            List<float> kmh = null;
            int selectedlap;
            int selectedsection;

            if (SelectedLap == "All Laps")
                selectedlap = -1;
            else
                selectedlap = int.Parse(SelectedLap);

            if (SelectedSection == "All Sections")
                selectedsection = -1;
            else
                selectedsection = int.Parse(SelectedSection);

            rpm = new List<int>();
            kmh = new List<float>();

            for (int i = 0; i < rm.AccG[0].Count; i++)
            {
                if ((rm.CompletedLaps[i] == selectedlap - 1 || selectedlap == -1) && (rm.TurnSections[i] == selectedsection - 1 || selectedsection == -1))
                {
                    rpm.Add(rm.Rpms[i]);
                    kmh.Add(rm.SpeedKmh[i]);
                }
            }
            if (RpmView == null)
            {
                RpmView = new RpmView();
            }
            RpmView.Rpms.DrawRpmCanvas(rpm, kmh);
            RpmView.Topmost = true;
            RpmView.Show();
        }

        private RaceModel ReadFile(string filename)
        {
            RaceModel model = new RaceModel();

            var fields = typeof(RaceModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            System.IO.StreamReader filestream = new System.IO.StreamReader(filename);
            string line = filestream.ReadLine();
            //string[] instancenames = line.Split(',');
            while ((line = filestream.ReadLine()) != null)
            {
                string[] values = line.Split(',');
                for (int i = 0; i < values.Count(); i++)
                {
                    var oldvalue = fields[i].GetValue(model);
                    if (oldvalue.GetType() == typeof(List<float>))
                    {
                        var array = oldvalue as List<float>;
                        array.Add(float.Parse(values[i]));
                        fields[i].SetValue(model, array);
                    }
                    else if (oldvalue.GetType() == typeof(List<List<float>>))
                    {
                        var array = oldvalue as List<List<float>>;
                        string[] numbersString = values[i].Split(':'); 
                        if (array.Count == 0)
                        {
                            for (int j = 0; j < numbersString.Count() - 1; j++)
                            {
                                array.Add(new List<float>());
                            }
                        }
                        for (int j = 0; j < numbersString.Count() - 1; j++)  //one less time, since the last one is blank
                        {
                            array[j].Add(float.Parse(numbersString[j]));
                        }
                        fields[i].SetValue(model, array);
                    }
                    else if (oldvalue.GetType() == typeof(List<string>))
                    {
                        var array = oldvalue as List<string>;
                        array.Add(values[i]);
                        fields[i].SetValue(model, array);
                    }
                    else if (oldvalue.GetType() == typeof(List<int>))
                    {
                        var array = oldvalue as List<int>;
                        array.Add(int.Parse(values[i]));
                        fields[i].SetValue(model, array);
                    }
                }
            }
            return model;
        }

        public MainWindowViewModel()
        {
            IsChecked1 = new Boolean[4];
            IsChecked2 = new Boolean[4];
            IsChecked3 = new Boolean[4];
            IsChecked4 = new Boolean[4];
            IsChecked5 = new Boolean[4];
            IsChecked6 = new Boolean[4];

            xCoordinates = new List<float>();
            yCoordinates = new List<float>();
            _isInTurns = new List<int>();
            _trackSections = new List<int>();
            CollectionLaps = new System.Collections.ObjectModel.ObservableCollection<string>();
            CollectionSections = new ObservableCollection<string>();
            SelectedLap = "All Laps";
            SelectedSection = "All Sections";

            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AssettoCorsaTelemetry\\";
            string fileName = DateTime.Now.ToString("MM-dd-yyyy_HH-mm") + ".csv";
            _filepath = System.IO.Path.Combine(path, fileName);

            Type t = typeof(Physics);
            _physicsMembers = t.GetFields(BindingFlags.Public | BindingFlags.Instance);
            Type type = typeof(Graphics);
            _graphicsMembers = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

            bool isExists = System.IO.Directory.Exists(path);

            if (!isExists)
                System.IO.Directory.CreateDirectory(path);

            using (StreamWriter f = File.AppendText(_filepath))
            {
                foreach (var physMember in _physicsMembers)
                {
                    if (physMember.Name != "PacketId")
                        f.Write(physMember.Name + ",");
                }
                foreach (var graphMember in _graphicsMembers)
                {
                    if (graphMember.Name != "PacketId")
                        f.Write(graphMember.Name + ",");
                }
                f.Write("\r\n");
            }

            var sdk = new AcSdk.AcSdk();
            sdk.UpdateFrequency = 5;

            sdk.StaticInfoUpdated += StaticInfoUpdated;
            sdk.Updated += Updated;

            sdk.Start();
        }

        void StaticInfoUpdated(object sender, AcSdk.AcSdk.StaticEventArgs e)
        {

        }

        void Updated(object sender, AcSdk.AcSdk.UpdateEventArgs e)
        {
            Physics physics = e.Physics;
            Graphics graphics = e.Graphics;

            AppendToFile(physics, graphics);

            //UpdateTrack(physics, graphics);
        }

        List<int> _trackSections;
        List<int> _isInTurns;
        private int _isTurning = 0;
        private int _turnCounter = 4;

        private void UpdateTrack(Physics physics, Graphics graphics)
        {
            xCoordinates.Add(graphics.CarCoordinates[0]);
            yCoordinates.Add(graphics.CarCoordinates[2]);
            if (_isTurning == 1)
            {
                if (physics.AccG[0] > 0.5 || physics.AccG[2] < -0.5)
                {
                    _turnCounter = 4;
                    _isInTurns.Add(1);
                }
                else if (_turnCounter > 0)
                {
                    _turnCounter--;
                    _isInTurns.Add(1);
                }
                else
                {
                    if (physics.AccG[0] < -0.5)
                    {
                        _isTurning = -1;
                        _isInTurns.Add(-1);
                    }
                    else
                    {
                        _isTurning = 0;
                        _isInTurns.Add(0);
                    }
                }
            }
            else if (_isTurning == -1)
            {
                if (physics.AccG[0] < -0.5 || physics.AccG[2] < -0.5)
                {
                    _turnCounter = 4;
                    _isInTurns.Add(-1);
                }
                else if (_turnCounter > 0)
                {
                    _turnCounter--;
                    _isInTurns.Add(-1);
                }
                else
                {
                    if (physics.AccG[0] > 0.5)
                    {
                        _isTurning = 1;
                        _isInTurns.Add(1);
                    }
                    else
                    {
                        _isTurning = 0;
                        _isInTurns.Add(0);
                    }
                }
            }
            else if (_isTurning == 0)
            {
                if (!(Math.Abs(physics.AccG[0]) > 0.5 || physics.AccG[2] < -0.5))
                {
                    _turnCounter = 4;
                    _isInTurns.Add(0);
                }
                else if (_turnCounter > 0)
                {
                    _turnCounter--;
                    _isInTurns.Add(0);
                }
                else
                {
                    if (physics.AccG[0] > 0.5)
                    {
                        _isTurning = 1;
                        _isInTurns.Add(1);
                    }
                    else if (physics.AccG[0] < -0.5)
                    {
                        _isTurning = -1;
                        _isInTurns.Add(-1);
                    }
                    else
                    {
                        _isTurning = 3;
                        _isInTurns.Add(3);
                    }
                }
            }
            else if (_isTurning == 3)
            {
                if (!(Math.Abs(physics.AccG[0]) > 0.5 || physics.AccG[2] < -0.5))
                {
                    _turnCounter--;
                    _isInTurns.Add(3);
                }
                else if (_turnCounter <= 0)
                {
                    _isTurning = 0;
                    for (int i = _isInTurns.Count() - 1; ; i--)
                    {
                        if (_isInTurns[i] != 3)
                        {
                            break;
                        }
                        _isInTurns[i] = 0;
                    }
                    _isInTurns.Add(0);
                }
                else
                {
                    _turnCounter = 4;
                    if (physics.AccG[0] > 0.5)
                    {
                        _isTurning = 1;
                        for (int i = _isInTurns.Count() - 1; ; i--)
                        {
                            if (_isInTurns[i] != 3)
                            {
                                break;
                            }
                            _isInTurns[i] = 1;
                        }
                        _isInTurns.Add(1);
                    }
                    else if (physics.AccG[0] < -0.5)
                    {
                        _isTurning = -1;
                        for (int i = _isInTurns.Count() - 1; ; i--)
                        {
                            if (_isInTurns[i] != 3)
                            {
                                break;
                            }
                            _isInTurns[i] = -1;
                        }
                        _isInTurns.Add(-1);
                    }
                    else
                    {
                        _isInTurns.Add(3);
                    }
                }
            }
            //Track.DrawTrack(xCoordinates, yCoordinates, _isInTurns);
        }

        private void AppendToFile(Physics physics, Graphics graphics)
        {
            using (StreamWriter f = File.AppendText(_filepath))
            {
                foreach (var x in _physicsMembers)
                {
                    if (x.Name != "PacketId")
                    {
                        Object propvalue = null;
                        if (x is FieldInfo)
                        {
                            propvalue = (x as FieldInfo).GetValue(physics);
                        }
                        WriteObjToFile(f, propvalue);
                    }
                }

                foreach (var x in _graphicsMembers)
                {
                    if (x.Name != "PacketId")
                    {
                        Object propvalue = null;
                        if (x is FieldInfo)
                        {
                            if (x.Name == "Status" || x.Name == "Session")
                            {
                                propvalue = (int)(x as FieldInfo).GetValue(graphics);
                            }
                            else
                            {
                                propvalue = (x as FieldInfo).GetValue(graphics);
                            }
                        }
                        WriteObjToFile(f, propvalue);
                    }
                }


                f.Write("\r\n");
            }
        }

        private static void WriteObjToFile(StreamWriter f, Object propvalue)
        {
            if (propvalue.GetType().IsArray)
            {
                foreach (var y in propvalue as float[])
                {
                    f.Write(y.ToString() + ":");
                }
                f.Write(",");
            }
            else
            {
                f.Write(propvalue.ToString() + ",");
            }
        }
    }
}
