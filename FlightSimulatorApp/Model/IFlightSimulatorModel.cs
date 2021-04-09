using OxyPlot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorApp.Model
{
    public interface IFlightSimulatorModel : INotifyPropertyChanged
    {
        #region Start and Connect

        void connect();
        void disconnect();
        void start();

        #endregion

        #region CSV

        void updateCSVPath(string csvPath);

        void praseCSV();

        #endregion

        #region Media

        void onPlay();

        void onPause();

        int PlaybackSpeed { set; get; }

        int CurrentLineIndex { set; get; }

        int CSVLinesNumber { set; get; }

        #endregion

        #region Joystick

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

        #endregion

        #region Data Display

        float CurrentAltimeter { set; get; }
        float CurrentAirSpeed { set; get; }
        float CurrentHeading { set; get; }
        float CurrentPitch { set; get; }
        float CurrentRoll { set; get; }
        float CurrentYaw { set; get; }

        #endregion

        #region Graph

        List<string> AttributesList { set; get; }
        string CurrentAttribute { set; get; }
        List<DataPoint> DataPointsList { set; get; }

        #endregion
    }
}
