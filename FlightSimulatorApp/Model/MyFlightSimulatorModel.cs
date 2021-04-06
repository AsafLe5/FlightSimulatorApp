using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlightSimulatorApp.Model
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

        private bool isPaused = false;
        public void start()
        {
            this.PlaybackSpeed = 10;
            new Thread(delegate ()
            {
                bool playbackSpeedZero = false;
                while (!stop)
                {
                    float playbackSpeedRational = 10;
                    if (this.playbackSpeed == 0.0)
                    {
                        playbackSpeedZero = true;
                    }
                    else
                    {
                        playbackSpeedZero = false;
                        playbackSpeedRational = 1000 / this.playbackSpeed;
                    }

                    // Keeping the animation running
                    if (this.CurrentLineIndex < this.csvLinesNumber && !playbackSpeedZero && !isPaused)
                    {
                        telnetClient.write(this.csvLines[this.currentLineIndex]);
                        AileronCurrentValue = (float)Convert.ToDouble(this.csvDict[0][this.currentLineIndex]);
                        ElevatorCurrentValue = (float)Convert.ToDouble(this.csvDict[1][this.currentLineIndex]);
                        ThrottleCurrentValue = (float)Convert.ToDouble(this.csvDict[6][this.currentLineIndex]);
                        RudderCurrentValue = (float)Convert.ToDouble(this.csvDict[2][this.currentLineIndex]);
                        CurrentAltimeter = (float)Convert.ToDouble(this.csvDict[24][this.currentLineIndex]);
                        CurrentAirSpeed = (float)Convert.ToDouble(this.csvDict[20][this.currentLineIndex]);
                        CurrentHeading = (float)Convert.ToDouble(this.csvDict[18][this.currentLineIndex]);
                        CurrentPitch = (float)Convert.ToDouble(this.csvDict[17][this.CurrentLineIndex]);
                        CurrentRoll = (float)Convert.ToDouble(this.csvDict[16][this.CurrentLineIndex]);
                        CurrentYaw = (float)Convert.ToDouble(this.csvDict[19][this.currentLineIndex]);
                        this.CurrentLineIndex += 1;
                    }

                    Thread.Sleep((int)playbackSpeedRational);

                }
            }).Start();
        }

        public void onPlay()
        {
            this.isPaused = false;
        }

        public void onPause()
        {
            this.isPaused = true;
        }



        private int playbackSpeed;
        public int PlaybackSpeed
        {
            get { return playbackSpeed; }
            set
            {
                playbackSpeed = value;
                NotifyPropertyChanged("PlaybackSpeed");
            }
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

        private void setMinAndMax(float max, float min, int columnNumber)
        {
            float tempMaximunValue = float.MinValue;
            float tempMinimumValue = float.MaxValue;
            for (int i = 0; i < this.csvLinesNumber; i++)
            {
                float currentValue = (float)Convert.ToDouble(this.csvDict[columnNumber][i]);
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
        private Dictionary<int, List<string>> csvDict = new Dictionary<int, List<string>>();
        public void csvParser()
        {

            // Init dict:
            for (int i = 0; i < 50; i++)
            {
                csvDict.Add(i, new List<string>());
            }

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
                    csvDict[j].Add(parts_of_line[j].Trim());
                }
                csvLinesNumberCounter++;
            }
            this.CSVLinesNumber = csvLinesNumberCounter;

            setMinAndMax(this.AileronMaximunValue, this.AileronMinimumValue, 0);
            setMinAndMax(this.ElevatorMaximunValue, this.ElevatorMinimumValue, 1);
            setMinAndMax(this.ThrottleMaximunValue, this.ThrottleMaximunValue, 6);
            setMinAndMax(this.RudderMinimumValue, this.RudderMinimumValue, 2);
        }

        private CSVProperty aileron = new CSVProperty();
        public float AileronCurrentValue
        {
            get { return aileron.propertyCurrentValue; }
            set
            {
                aileron.propertyCurrentValue = value;
                NotifyPropertyChanged("AileronCurrentValue");
            }
        }

        public float AileronMaximunValue
        {
            get { return aileron.propertyMaximunValue; }
            set
            {
                aileron.propertyMaximunValue = value;
                NotifyPropertyChanged("AileronMaximunValue");
            }
        }

        public float AileronMinimumValue
        {
            get { return aileron.propertyMinimumValue; }
            set
            {
                aileron.propertyMinimumValue = value;
                NotifyPropertyChanged("AileronMinimumValue");
            }
        }

        private CSVProperty elevator = new CSVProperty();
        public float ElevatorCurrentValue
        {
            get { return elevator.propertyCurrentValue; }
            set
            {
                elevator.propertyCurrentValue = value;
                NotifyPropertyChanged("ElevatorCurrentValue");
            }
        }

        public float ElevatorMaximunValue
        {
            get { return elevator.propertyMaximunValue; }
            set
            {
                elevator.propertyMaximunValue = value;
                NotifyPropertyChanged("ElevatorMaximunValue");
            }
        }

        public float ElevatorMinimumValue
        {
            get { return elevator.propertyMinimumValue; }
            set
            {
                elevator.propertyMinimumValue = value;
                NotifyPropertyChanged("ElevatorMinimumValue");
            }
        }

        private CSVProperty throttle = new CSVProperty();
        public float ThrottleCurrentValue
        {
            get { return throttle.propertyCurrentValue; }
            set
            {
                throttle.propertyCurrentValue = value;
                NotifyPropertyChanged("ThrottleCurrentValue");
            }
        }

        public float ThrottleMaximunValue
        {
            get { return throttle.propertyMaximunValue; }
            set
            {
                throttle.propertyMaximunValue = value;
                NotifyPropertyChanged("ThrottleMaximunValue");
            }
        }

        public float ThrottleMinimumValue
        {
            get { return throttle.propertyMinimumValue; }
            set
            {
                throttle.propertyMinimumValue = value;
                NotifyPropertyChanged("ThrottleMaximunValue");
            }
        }

        private CSVProperty rudder = new CSVProperty();
        public float RudderCurrentValue
        {
            get { return rudder.propertyCurrentValue; }
            set
            {
                rudder.propertyCurrentValue = value;
                NotifyPropertyChanged("RudderCurrentValue");
            }
        }

        public float RudderMaximunValue
        {
            get { return rudder.propertyMaximunValue; }
            set
            {
                rudder.propertyMaximunValue = value;
                NotifyPropertyChanged("RudderMaximunValue");
            }
        }

        public float RudderMinimumValue
        {
            get { return rudder.propertyMinimumValue; }
            set
            {
                rudder.propertyMinimumValue = value;
                NotifyPropertyChanged("RudderMinimumValue");
            }
        }

        private CSVProperty altimeter = new CSVProperty();
        public float CurrentAltimeter
        {
            get { return altimeter.propertyCurrentValue; }
            set
            {
                altimeter.propertyCurrentValue = value;
                NotifyPropertyChanged("CurrentAltimeter");
            }
        }

        private CSVProperty airSpeed = new CSVProperty();
        public float CurrentAirSpeed
        {
            get { return airSpeed.propertyCurrentValue; }
            set
            {
                airSpeed.propertyCurrentValue = value;
                NotifyPropertyChanged("CurrentAirSpeed");
            }
        }

        private CSVProperty heading = new CSVProperty();
        public float CurrentHeading
        {
            get { return heading.propertyCurrentValue; }
            set
            {
                heading.propertyCurrentValue = value;
                NotifyPropertyChanged("CurrentHeading");
            }
        }

        private CSVProperty pitch = new CSVProperty();
        public float CurrentPitch
        {
            get { return pitch.propertyCurrentValue; }
            set
            {
                pitch.propertyCurrentValue = value;
                NotifyPropertyChanged("CurrentPitch");
            }
        }

        private CSVProperty roll = new CSVProperty();
        public float CurrentRoll
        {
            get { return roll.propertyCurrentValue; }
            set
            {
                roll.propertyCurrentValue = value;
                NotifyPropertyChanged("CurrentRoll");
            }
        }

        private CSVProperty yaw = new CSVProperty();
        public float CurrentYaw
        {
            get { return yaw.propertyCurrentValue; }
            set
            {
                yaw.propertyCurrentValue = value;
                NotifyPropertyChanged("CurrentYaw");
            }
        }
    }
}
