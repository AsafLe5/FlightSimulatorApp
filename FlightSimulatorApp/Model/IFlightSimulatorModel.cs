using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorApp.Model
{
    interface IFlightSimulatorModel : INotifyPropertyChanged
    {
        void connect();
        void disconnect();
        void start();

        void onPlay();


        void onPause();
        void updateCSVPath(string csvPath);

        int CurrentLineIndex { set; get; }

        int CSVLinesNumber { set; get; }

        void csvParser();

        int PlaybackSpeed { set; get; }

        float AileronCurrentValue { set; get; }
        float AileronMaximunValue { set; get; }
        float AileronMinimumValue { set; get; }

        float ElevatorCurrentValue { set; get; }
        float ElevatorMaximunValue { set; get; }
        float ElevatorMinimumValue { set; get; }

        float ThrottleCurrentValue { set; get; }
        float ThrottleMaximunValue { set; get; }
        float ThrottleMinimumValue { set; get; }

        float RudderCurrentValue { set; get; }
        float RudderMaximunValue { set; get; }
        float RudderMinimumValue { set; get; }

        float CurrentAltimeter { set; get; }
        float CurrentAirSpeed { set; get; }
        float CurrentHeading { set; get; }
        float CurrentPitch { set; get; }
        float CurrentRoll { set; get; }
        float CurrentYaw { set; get; }
    }
}
