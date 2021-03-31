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
        void uploadCSV();
        void connect();
        void disconnect();
        void start();

        string CSVPath { get; set; }
    }
}
