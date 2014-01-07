using AcSdk.Data;
using AssettoCorsaTelemetry.Forces;
using AssettoCorsaTelemetry.Track;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

        private TrackViewModel _track;
        public TrackViewModel Track
        {
            get
            {
                return _track;
            }
            set
            {
                SetProperty(ref _track, value);
            }
        }

        private ForcesViewModel _forces;
        public ForcesViewModel Forces
        {
            get
            {
                return _forces;
            }
            set
            {
                SetProperty(ref _forces, value);
            }
        }

        private RpmViewModel _rpms;
        public RpmViewModel Rpms
        {
            get
            {
                return _rpms;
            }
            set
            {
                SetProperty(ref _rpms, value);
            }
        }

        private ICommand _clickCommand;
        public ICommand ClickCommand
        {
            get
            {
                return _clickCommand ?? (_clickCommand = new CommandHandler(() => OpenOldRace(), true));
            }
        }

        System.Collections.ObjectModel.ObservableCollection<string> _collectionLaps;
        public System.Collections.ObjectModel.ObservableCollection<string> CollectionLaps
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

        RaceModel rm;

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
                rm.Turns = Track.FindIsInTurns(rm.AccG);
                rm.TurnSections = CalculateTurnSections(rm.Turns, rm.CompletedLaps);
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
            List<int> turns = null;
            if (SelectedLap == "All Laps")
            {
                xCoor = rm.CarCoordinates[0];
                yCoor = rm.CarCoordinates[2];
                turns = rm.Turns;
            }
            else
            {
                xCoor = new List<float>();
                yCoor = new List<float>();
                turns = new List<int>();
                int lapnumber = int.Parse(SelectedLap);
                for (int i = 0; i < rm.AccG[0].Count; i++)
                {
                    if (rm.CompletedLaps[i] == lapnumber - 1)
                    {
                        xCoor.Add(rm.CarCoordinates[0][i]);
                        yCoor.Add(rm.CarCoordinates[2][i]);
                        turns.Add(rm.Turns[i]);
                    }
                }
            }
            Track.DrawTrack(xCoor, yCoor, turns);
        }

        private void DrawForces()
        {
            List<float> xAcc = null;
            List<float> yAcc = null;
            if (SelectedLap == "All Laps")
            {
                xAcc = rm.AccG[0];
                yAcc = rm.AccG[1];
            }
            else
            {
                xAcc = new List<float>();
                yAcc = new List<float>();
                int lapnumber = int.Parse(SelectedLap);
                for (int i = 0; i < rm.AccG[0].Count; i++)
                {
                    if (rm.CompletedLaps[i] == lapnumber - 1)
                    {
                        xAcc.Add(rm.AccG[0][i]);
                        yAcc.Add(rm.AccG[1][i]);
                    }
                }
            }
            Forces.DrawAccelerationMap(xAcc, yAcc);
        }

        private void DrawRpms()
        {
            List<int> rpm = null;
            List<float> kmh = null;
            if (SelectedLap == "All Laps")
            {
                rpm = rm.Rpms;
                kmh = rm.SpeedKmh;
            }
            else
            {
                rpm = new List<int>();
                kmh = new List<float>();
                int lapnumber = int.Parse(SelectedLap);
                for (int i = 0; i < rm.AccG[0].Count; i++)
                {
                    if (rm.CompletedLaps[i] == lapnumber - 1)
                    {
                        rpm.Add(rm.Rpms[i]);
                        kmh.Add(rm.SpeedKmh[i]);
                    }
                }
            }
            Rpms.DrawRpmCanvas(rpm, kmh);
        }

        private List<int> CalculateTurnSections(List<int> turns, List<int> completedLaps)
        {
            if (completedLaps[0] == completedLaps[completedLaps.Count - 1])
            {
                return new List<int>();
            }
            int lap = completedLaps[0];
            List<List<int>> laps = new List<List<int>>();

            List<int> result = new List<int>();


            int starting = 0;
            for (int i = 0; ; i++) //get rid of junk before the lap starts (only works for flying laps for now)
            {
                if (completedLaps[i] > lap)
                {
                    starting = i;
                    break;
                }
                result.Add(-1);
            }

            List<int> totalSections = new List<int>();

            int section = 0;
            int j = 0;
            int previousTurn = 0;
            lap = completedLaps[starting];
            for (int i = starting; i < turns.Count;)
            {
                laps.Add(new List<int>());
                while (lap == completedLaps[i])
                {
                    if (turns[i] != previousTurn)
                    {
                        section++;
                    }
                    result.Add(section);
                    laps[lap - completedLaps[0] - 1].Add(section);
                    previousTurn = turns[i];
                    i++;
                    if (i >= turns.Count)
                        break;
                }
                lap++;
                totalSections.Add(section);
                section = 0;
                if (lap == completedLaps[completedLaps.Count - 1])
                    break;
            }

            for (int i = result.Count; i < turns.Count; i++) //Get rid of any trailing lap
            {
                result.Add(-1);
            }

            return result;
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
            Track = new TrackViewModel();
            Forces = new ForcesViewModel();
            Rpms = new RpmViewModel();
            xCoordinates = new List<float>();
            yCoordinates = new List<float>();
            _isInTurns = new List<int>();
            _trackSections = new List<int>();
            CollectionLaps = new System.Collections.ObjectModel.ObservableCollection<string>();
            SelectedLap = "All Laps";

            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AssettoCorsaTelemetry\\";
            string fileName = DateTime.Now.ToString("MM-dd-yyyy_HH-mm") + ".csv";
            _filepath = System.IO.Path.Combine(path, fileName);

            Type t = typeof(Physics);
            _physicsMembers = t.GetFields(BindingFlags.Public | BindingFlags.Instance);
            Type type = typeof(Graphics);
            _graphicsMembers = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

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

            UpdateTrack(physics, graphics);
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
            Track.DrawTrack(xCoordinates, yCoordinates, _isInTurns);
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
