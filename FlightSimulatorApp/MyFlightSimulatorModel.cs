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
            this.csvLines = System.IO.File.ReadLines(this.csvPath).ToList();
            this.CSVLinesNumber = this.csvLines.Count();
            new Thread(delegate ()
            {
                while (!stop)
                {
                    telnetClient.write(this.csvLines[this.currentLineIndex]);
                    Thread.Sleep(100);
                    this.CurrentLineIndex += 1;

                    // Keeping the animation running
                    if (this.CurrentLineIndex >= this.CSVLinesNumber)
                    {
                        this.CurrentLineIndex -= 2;
                    }
                }
            }).Start();
        }

        private List<string> csvLines;

        private int currentLineIndex;
        public int CurrentLineIndex
        {
            get { return currentLineIndex; }
            set
            {
                currentLineIndex = value;
                NotifyPropertyChanged("CurrentLineIndex");
            }
        }

        private string csvPath;
        public void updateCSVPath(string csvPath)
        {
            this.csvPath = csvPath;
        }

        private int csvLinesNumber;
        public int CSVLinesNumber
        {
            get { return csvLinesNumber; }
            set
            {
                csvLinesNumber = value;
                NotifyPropertyChanged("CSVLinesNumber");
            }
        }

    }
}
