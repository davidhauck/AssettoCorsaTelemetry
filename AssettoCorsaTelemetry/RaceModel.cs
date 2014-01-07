using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssettoCorsaTelemetry
{
    class RaceModel
    {
        public enum Identifiers
        {
            packetId_physics,
            Gas,
            Brake,
            Fuel,
            Gear,
            RPMs,
            SteerAngle,
            SpeedKmh,
            Velocity,
            Acceleration,
            WheelSlip,
            WheelLoad,
            WheelsPressure,
            WheelAngularSpeed,
            TyreWear,
            TyreDirtyLevel,
            TyreCoreTemperature,
            CamberRAD,
            SuspensionTravel,
            DRS,
            TC,
            Heading,
            Pitch,
            Roll,
            CgHeight,
            CarDamage,
            NumberOfTyresOut,
            PitLimiterOn,
            Abs,

            packetId_graphics,
            Status,
            Session,
            CurrentTime,
            LastTime,
            BestTime,
            Split,
            CompletedLaps,
            Position,
            iCurrentTime,
            iLastTime,
            iBestTime,
            SessionTimeLeft,
            DistanceTraveled,
            IsInPit,
            CurrentSectorIndex,
            LastSectorTime,
            NumberOfLaps,
            TyreCompound,
            ReplayTimeMultiplier,
            NormalizedCarPosition,
            CarCoordinates
        }

        public RaceModel()
        {
            Gas = new List<float>();
            Brake = new List<float>();
            Fuel = new List<float>();
            Gear = new List<int>();
            Rpms = new List<int>();
            SteerAngle = new List<float>();
            SpeedKmh = new List<float>();
            Velocity = new List<List<float>>();
            AccG = new List<List<float>>();
            WheelSlip = new List<List<float>>();
            WheelLoad = new List<List<float>>();
            WheelPressure = new List<List<float>>();
            WheelAngularSpeed = new List<List<float>>();
            TyreWear = new List<List<float>>();
            TyreDirtyLevel = new List<List<float>>();
            TyreCoreTemp = new List<List<float>>();
            CamberRad = new List<List<float>>();
            SuspensionTravel = new List<List<float>>();
            DRS = new List<float>();
            TC = new List<float>();
            Heading = new List<float>();
            Pitch = new List<float>();
            Roll = new List<float>();
            CgHeight = new List<float>();
            CarDamage = new List<List<float>>();
            NumberOfTyresOut = new List<int>();
            PitLimiterOn = new List<int>();
            Abs = new List<float>();

            Status = new List<int>();
            Session = new List<int>();
            CurrentTime = new List<string>();
            LastTime = new List<string>();
            BestTime = new List<string>();
            Split = new List<string>();
            CompletedLaps = new List<int>();
            Position = new List<int>();
            iCurrentTime = new List<int>();
            iLastTime = new List<int>();
            iBestTime = new List<int>();
            SessionTimeLeft = new List<float>();
            DistanceTraveled = new List<float>();
            IsInPit = new List<int>();
            CurrentSectorIndex = new List<int>();
            LastSectorTime = new List<int>();
            NumberOfLaps = new List<int>();
            TyreCompound = new List<string>();
            ReplayTimeMultiplier = new List<float>();
            NormalizedCarPosition = new List<float>();
            CarCoordinates = new List<List<float>>();

            Empty = new List<string>();
            TurnSections = new List<int>(); 
            Turns = new List<int>();
        }

        #region Physics
        public List<float> Gas { get; set; }
        public List<float> Brake { get; set; }
        public List<float> Fuel { get; set; }
        public List<int> Gear { get; set; }
        public List<int> Rpms { get; set; }
        public List<float> SteerAngle { get; set; }
        public List<float> SpeedKmh { get; set; }
        public List<List<float>> Velocity { get; set; }
        public List<List<float>> AccG { get; set; }
        public List<List<float>> WheelSlip { get; set; }
        public List<List<float>> WheelLoad { get; set; }
        public List<List<float>> WheelPressure { get; set; }
        public List<List<float>> WheelAngularSpeed { get; set; }
        public List<List<float>> TyreWear { get; set; }
        public List<List<float>> TyreDirtyLevel { get; set; }
        public List<List<float>> TyreCoreTemp { get; set; }
        public List<List<float>> CamberRad { get; set; }
        public List<List<float>> SuspensionTravel { get; set; }
        public List<float> DRS { get; set; }
        public List<float> TC { get; set; }
        public List<float> Heading { get; set; }
        public List<float> Pitch { get; set; }
        public List<float> Roll { get; set; }
        public List<float> CgHeight { get; set; }
        public List<List<float>> CarDamage { get; set; }
        public List<int> NumberOfTyresOut { get; set; }
        public List<int> PitLimiterOn { get; set; }
        public List<float> Abs { get; set; }
        #endregion

        #region Graphics
        public List<int> Status {get;set;}
        public List<int> Session {get;set;}
        public List<string> CurrentTime { get; set; }
        public List<string> LastTime { get; set; }
        public List<string> BestTime { get; set; }
        public List<string> Split { get; set; }
        public List<int> CompletedLaps { get; set; }
        public List<int> Position { get; set; }
        public List<int> iCurrentTime { get; set; }
        public List<int> iLastTime { get; set; }
        public List<int> iBestTime { get; set; }
        public List<float> SessionTimeLeft { get; set; }
        public List<float> DistanceTraveled { get; set; }
        public List<int> IsInPit { get; set; }
        public List<int> CurrentSectorIndex { get; set; }
        public List<int> LastSectorTime { get; set; }
        public List<int> NumberOfLaps { get; set; }
        public List<string> TyreCompound { get; set; }
        public List<float> ReplayTimeMultiplier { get; set; }
        public List<float> NormalizedCarPosition { get; set; }
        public List<List<float>> CarCoordinates { get; set; }
        #endregion

        public List<string> Empty { get; set; } //Used since there is a comma at the end of each line which is an empty string. This should be fixed
        public List<int> TurnSections { get; set; }
        public List<int> Turns { get; set; }
    }
}
