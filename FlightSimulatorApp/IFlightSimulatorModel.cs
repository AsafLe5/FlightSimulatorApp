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

        
    }


}
