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
            new Thread(delegate ()
            {
                while (!stop)
                {
                    telnetClient.write(this.csvLines[this.currentLineIndex]);
                    AileronCurrentValue = Convert.ToDouble(this.csvAsList[0][this.currentLineIndex]);
                    ElevatorCurrentValue = Convert.ToDouble(this.csvAsList[1][this.currentLineIndex]);
                    ThrottleCurrentValue = Convert.ToDouble(this.csvAsList[6][this.currentLineIndex]);
                    RudderCurrentValue = Convert.ToDouble(this.csvAsList[2][this.currentLineIndex]);

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
            csvParser();
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

        private void setMinAndMax(double max, double min, int columnNumber)
        {
            double tempMaximunValue = Double.MinValue;
            double tempMinimumValue = Double.MaxValue;
            for (int i = 0; i < this.csvLinesNumber; i++)
            {
                double currentValue = Convert.ToDouble(this.csvAsList[columnNumber][i]);
                if (currentValue > tempMaximunValue)
                {
                    tempMaximunValue = currentValue;
                }
                if (currentValue < tempMinimumValue)
                {
                    tempMinimumValue = currentValue;
                }
            }
            max = tempMaximunValue;
            min = tempMinimumValue;
        }

        private List<string> csvLines;
        private int csvColumnsNumber;
        List<List<string>> csvAsList = new List<List<string>>();
        public void csvParser()
        {
            this.csvLines = File.ReadLines(this.csvPath).ToList();

            int csvLinesNumberCounter = 0;
            int j = 0;
            String line = String.Empty;
            System.IO.StreamReader file = new System.IO.StreamReader(this.csvPath);
            while ((line = file.ReadLine()) != null)
            {
                String[] parts_of_line = line.Split(',');
                for (j = 0; j < parts_of_line.Length; j++)
                {
                    this.csvAsList[csvLinesNumberCounter].Add(parts_of_line[j].Trim());
                }
                csvLinesNumberCounter++;
            }
            this.csvColumnsNumber = j;
            this.CSVLinesNumber = csvLinesNumberCounter;

            setMinAndMax(this.AileronMaximunValue, this.AileronMinimumValue, 0);
            setMinAndMax(this.ElevatorMaximunValue, this.ElevatorMinimumValue, 1);
            setMinAndMax(this.ThrottleMaximunValue, this.ThrottleMaximunValue, 6);
            setMinAndMax(this.RudderMinimumValue, this.RudderMinimumValue, 2);


            Dictionary<int, List<string>> csvDic = new Dictionary<int, List<string>>();

            String CurrentLine = String.Empty;
            System.IO.StreamReader filCurrentLinee = new System.IO.StreamReader(this.csvPath);
            while ((CurrentLine = file.ReadLine()) != null)
            {
                String[] parts_of_line = CurrentLine.Split(',');
                for (j = 0; j < parts_of_line.Length; j++)
                {
                    csvDic[csvLinesNumberCounter].Add(parts_of_line[j].Trim());
                }
                csvLinesNumberCounter++;
            }
        }

        private CSVProperty aileron = new CSVProperty();
        public double AileronCurrentValue
        {
            get { return aileron.propertyCurrentValue; }
            set
            {
                aileron.propertyCurrentValue = value;
                NotifyPropertyChanged("AileronCurrentValue");
            }
        }

        public double AileronMaximunValue
        {
            get { return aileron.propertyMaximunValue; }
            set
            {
                aileron.propertyMaximunValue = value;
                NotifyPropertyChanged("AileronMaximunValue");
            }
        }

        public double AileronMinimumValue
        {
            get { return aileron.propertyMinimumValue; }
            set
            {
                aileron.propertyMinimumValue = value;
                NotifyPropertyChanged("AileronMinimumValue");
            }
        }

        private CSVProperty elevator = new CSVProperty();
        public double ElevatorCurrentValue
        {
            get { return elevator.propertyCurrentValue; }
            set
            {
                elevator.propertyCurrentValue = value;
                NotifyPropertyChanged("ElevatorCurrentValue");
            }
        }

        public double ElevatorMaximunValue
        {
            get { return elevator.propertyMaximunValue; }
            set
            {
                elevator.propertyMaximunValue = value;
                NotifyPropertyChanged("ElevatorMaximunValue");
            }
        }

        public double ElevatorMinimumValue
        {
            get { return elevator.propertyMinimumValue; }
            set
            {
                elevator.propertyMinimumValue = value;
                NotifyPropertyChanged("ElevatorMinimumValue");
            }
        }

        private CSVProperty throttle = new CSVProperty();
        public double ThrottleCurrentValue
        {
            get { return throttle.propertyCurrentValue; }
            set
            {
                throttle.propertyCurrentValue = value;
                NotifyPropertyChanged("ThrottleCurrentValue");
            }
        }

        public double ThrottleMaximunValue
        {
            get { return throttle.propertyMaximunValue; }
            set
            {
                throttle.propertyMaximunValue = value;
                NotifyPropertyChanged("ThrottleMaximunValue");
            }
        }

        public double ThrottleMinimumValue
        {
            get { return throttle.propertyMinimumValue; }
            set
            {
                throttle.propertyMinimumValue = value;
                NotifyPropertyChanged("ThrottleMaximunValue");
            }
        }

        private CSVProperty rudder = new CSVProperty();
        public double RudderCurrentValue
        {
            get { return rudder.propertyCurrentValue; }
            set
            {
                rudder.propertyCurrentValue = value;
                NotifyPropertyChanged("RudderCurrentValue");
            }
        }

        public double RudderMaximunValue
        {
            get { return rudder.propertyMaximunValue; }
            set
            {
                rudder.propertyMaximunValue = value;
                NotifyPropertyChanged("RudderMaximunValue");
            }
        }

        public double RudderMinimumValue
        {
            get { return rudder.propertyMinimumValue; }
            set
            {
                rudder.propertyMinimumValue = value;
                NotifyPropertyChanged("RudderMinimumValue");
            }
        }

    }

}