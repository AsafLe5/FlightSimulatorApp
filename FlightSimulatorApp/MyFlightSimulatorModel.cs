using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Threading;
using System.IO;

namespace FlightSimulatorApp
{
    class MyFlightSimulatorModel : IFlightSimulatorModel
    {

        ITelnetClient telnetClient;
        volatile Boolean stop;


        public MyFlightSimulatorModel(ITelnetClient telnetClient)
        {
            this.telnetClient = telnetClient;
            stop = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public void connect()
        {
            telnetClient.connect();
        }

        public void disconnect()
        {
            stop = true;
            telnetClient.disconnect();
        }
        public void start()
        {
            //TODO csvpath
            var lines = File.ReadLines("C:\\Users\\97250\\Downloads\\reg_flight.csv");
            new Thread(delegate ()
            {
                foreach (string line in lines)
                {
                    if (stop)
                    {
                        break;
                    }
                    telnetClient.write(line);
                    Thread.Sleep(100);
                }
            }).Start();
        }


        private string csvPath;
        public void updateCSVPath(string csvPath)
        {
            this.csvPath = csvPath;
        }


    }
}
