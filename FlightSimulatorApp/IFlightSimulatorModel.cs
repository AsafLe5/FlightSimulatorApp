using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;

namespace FlightSimulatorApp
{
    interface IFlightSimulatorModel : INotifyPropertyChanged
    {
        void connect();
        void disconnect();
        void start();

        void updateCSVPath(string csvPath);

        int CurrentLineIndex { set; get; }

        int CSVLinesNumber { set; get; }

        void csvParser();

        double AileronCurrentValue { set; get; }
        double AileronMaximunValue { set; get; }
        double AileronMinimumValue { set; get; }

        double ElevatorCurrentValue { set; get; }
        double ElevatorMaximunValue { set; get; }
        double ElevatorMinimumValue { set; get; }

        double ThrottleCurrentValue { set; get; }
        double ThrottleMaximunValue { set; get; }
        double ThrottleMinimumValue { set; get; }

        double RudderCurrentValue { set; get; }
        double RudderMaximunValue { set; get; }
        double RudderMinimumValue { set; get; }
    }


}
