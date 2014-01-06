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
        private Visibility _racingVisibility;
        public Visibility RacingVisibility { get { return _racingVisibility; } set { SetProperty(ref _racingVisibility, value); } }

        private Visibility _analyzingVisibility;
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
                RaceModel rm = ReadFile(filename);
                Forces.DrawAccelerationMap(rm.AccG[0], rm.AccG[1]);
                Rpms.DrawRpmCanvas(rm.Rpms, rm.SpeedKmh);
            }
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
                for (int i = 0; i < fields.Count(); i++)
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

        private void UpdateTrack(Physics physics, Graphics graphics)
        {
            xCoordinates.Add(graphics.CarCoordinates[0]);
            yCoordinates.Add(graphics.CarCoordinates[2]);
            Track.DrawTrack(xCoordinates, yCoordinates);
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
